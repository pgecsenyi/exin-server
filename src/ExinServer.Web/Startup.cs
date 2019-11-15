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
using ExinServer.Data;
using ExinServer.Data.Abstraction;
using ExinServer.Web.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExinServer.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostEnvironment env,
            ILoggerFactory loggerFactory,
            IDataLayer dataLayer)
        {
            ILogger logger = null;

            try
            {
                logger = loggerFactory.CreateLogger<Startup>();

                ConfigureLogging(loggerFactory);
                ConfigureDevelopmentEnvironment(app, env);
                dataLayer.EnsureCreated();
                app.UseMiddleware<ErrorHandlingMiddleware>();
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                    });
            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to start the application. " + exception.Message);
                logger?.LogCritical(exception, "Failed to start the application.");
                throw exception;
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                ConfigureDataService(services);
                services.AddControllers().AddNewtonsoftJson();
                services.AddRouting();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to start the application. " + exception.Message);
                throw exception;
            }
        }

        private void ConfigureLogging(ILoggerFactory loggerFactory)
        {
            var loggingSettings = Configuration.GetSection("Logging");
            var filename = loggingSettings.GetValue("Filename", string.Empty);
            if (!string.IsNullOrWhiteSpace(filename))
                loggerFactory.AddFile(filename);
        }

        private void ConfigureDevelopmentEnvironment(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
        }

        private void ConfigureDataService(IServiceCollection services)
        {
            var databaseSettings = Configuration.GetSection("Database");
            var type = databaseSettings.GetValue("Type", string.Empty);
            var connectionString = databaseSettings.GetValue("ConnectionString", string.Empty);

            var databaseType = DetermineDatabaseType(type, connectionString);
            var configuration = new DatabaseConfiguration(databaseType, connectionString);

            services.AddScoped<IDataLayer>(provider => DataLayerFactory.CreateDataLayer(configuration));
        }

        private DatabaseType DetermineDatabaseType(string type, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ApplicationException("Database type is not specified.");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ApplicationException("Connection string is not specified.");

            DatabaseType result;
            if (!Enum.TryParse(type, true, out result))
                throw new ApplicationException("Invalid database type.");

            return result;
        }
    }
}
