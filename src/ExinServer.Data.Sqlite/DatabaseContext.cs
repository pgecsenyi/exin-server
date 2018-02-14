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

using ExinServer.Data.Sqlite.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExinServer.Data.Sqlite
{
    internal class DatabaseContext : DbContext
    {
        private SqliteDatabaseConfiguration configuration;
        private SqliteConnection connection;

        public DatabaseContext(SqliteDatabaseConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Partner> Partners { get; set; }

        public DbSet<Transfer> Transfers { get; set; }

        public DbSet<TransferItem> TransferItems { get; set; }

        public override void Dispose()
        {
            base.Dispose();

            connection?.Close();
            connection?.Dispose();
            connection = null;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (configuration.InMemory)
            {
                connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                optionsBuilder.UseSqlite(connection);
            }
            else
            {
                optionsBuilder.UseSqlite("Filename=" + configuration.Path);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.SetSingleUnderscoreNameConvention(true);
        }
    }
}
