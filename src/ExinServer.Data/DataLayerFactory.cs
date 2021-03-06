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
using ExinServer.Data.Abstraction;

namespace ExinServer.Data
{
    public class DataLayerFactory
    {
        public static IDataLayer CreateDataLayer(DatabaseConfiguration configuration)
        {
            switch (configuration.Type)
            {
                case DatabaseType.Sqlite:
                    return CreateSqliteLayer(configuration.ConnectionString);
                case DatabaseType.SqliteMemory:
                    return CreateSqliteMemoryLayer();
                default:
                    throw new ApplicationException("Invalid database configuration.");
            }
        }

        private static IDataLayer CreateSqliteLayer(string connectionString)
        {
            var sqliteConfiguration = new ExinServer.Data.Sqlite.SqliteDatabaseConfiguration(connectionString);

            return new ExinServer.Data.Sqlite.SqliteDataLayer(sqliteConfiguration);
        }

        private static IDataLayer CreateSqliteMemoryLayer()
        {
            var sqliteConfiguration = new ExinServer.Data.Sqlite.SqliteDatabaseConfiguration("DataSource=:memory:");

            return new ExinServer.Data.Sqlite.SqliteDataLayer(sqliteConfiguration);
        }
    }
}
