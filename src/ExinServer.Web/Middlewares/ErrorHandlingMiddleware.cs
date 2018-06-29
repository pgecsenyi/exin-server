// exin server
// Copyright (C) 2018  pgecsenyi
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Net;
using System.Threading.Tasks;
using ExinServer.Data.Abstraction.Exceptions;
using ExinServer.Web.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExinServer.Web.Middlewares
{
    internal class ErrorHandlingMiddleware
    {
        private readonly ILogger logger;
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger = null)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = GetHttpCode(exception);
            var message = GetErrorMessage(exception);

            var result = JsonConvert.SerializeObject(new { error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            logger?.LogWarning(exception, $"Returning HTTP code {code}.");

            return context.Response.WriteAsync(result);
        }

        private int GetHttpCode(Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            if (exception is InvalidRequestArgumentException)
                code = HttpStatusCode.BadRequest;
            else if (exception is RecordAlreadyExistsException)
                code = HttpStatusCode.Conflict;
            else if (exception is RecordDoesNotExistException)
                code = HttpStatusCode.NotFound;

            return (int)code;
        }

        private string GetErrorMessage(Exception exception)
        {
            return exception.Message;
        }
    }
}
