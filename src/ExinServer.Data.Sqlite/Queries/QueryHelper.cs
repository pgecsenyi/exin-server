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

using System.Linq;
using ExinServer.Data.Abstraction.Exceptions;
using ExinServer.Data.Sqlite.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExinServer.Data.Sqlite
{
    internal static class QueryHelper
    {
        public static Category GetCategory(DatabaseContext context, int categoryId)
        {
            var record = context.Categories.SingleOrDefault(c => c.Id == categoryId);
            if (record == null)
                throw new RecordDoesNotExistException($"Category with ID {categoryId} does not exist.");

            return record;
        }

        public static Currency GetCurrency(DatabaseContext context, int currencyId)
        {
            var record = context.Currencies.SingleOrDefault(c => c.Id == currencyId);
            if (record == null)
                throw new RecordDoesNotExistException($"Currency with ID {currencyId} does not exist.");

            return record;
        }

        public static Partner GetPartner(DatabaseContext context, int partnerId)
        {
            var record = context.Partners.SingleOrDefault(p => p.Id == partnerId);
            if (record == null)
                throw new RecordDoesNotExistException($"Partner with ID {partnerId} does not exist.");

            return record;
        }

        public static Transfer GetTransfer(DatabaseContext context, int transferId)
        {
            var record = context.Transfers
                .Include(t => t.Category)
                .Include(t => t.Partner)
                .Include(t => t.Currency)
                .Include(t => t.Items)
                .SingleOrDefault(t => t.Id == transferId);
            if (record == null)
                throw new RecordDoesNotExistException($"Transfer with ID {transferId} does not exist.");

            return record;
        }
    }
}
