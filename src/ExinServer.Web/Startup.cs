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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IDataLayer dataLayer)
        {
            ConfigureLogging(loggerFactory);
            ConfigureDevelopmentEnvironment(app, env);
            dataLayer.EnsureCreated();
            ConfigureExceptionHandling(app);

            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDataService(services);
            services.AddMvc();
        }

        private void ConfigureLogging(ILoggerFactory loggerFactory)
        {
            var loggingSettings = Configuration.GetSection("Logging");
            var filename = loggingSettings.GetValue("Filename", string.Empty);
            if (!string.IsNullOrWhiteSpace(filename))
                loggerFactory.AddFile(filename);
        }

        private void ConfigureDevelopmentEnvironment(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
        }

        private void ConfigureExceptionHandling(IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }

        private void ConfigureDataService(IServiceCollection services)
        {
            var databaseSettings = Configuration.GetSection("Database");
            var type = databaseSettings.GetValue("Type", "SQLite");
            var path = databaseSettings.GetValue("Path", "MyDatabase.db");

            var databaseType = DetermineDatabaseType(type);
            var configuration = new DatabaseConfiguration(databaseType, path);

            services.AddScoped<IDataLayer>(provider => DataLayerFactory.CreateDataLayer(configuration));
        }

        private DatabaseType DetermineDatabaseType(string type)
        {
            DatabaseType result;
            if (Enum.TryParse(type, true, out result))
                return result;

            return DatabaseType.Sqlite;
        }
    }
}
