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

using System.Collections.Generic;
using System.Linq;
using ExinServer.Data.Sqlite.Entities;
using AbstractEntities = ExinServer.Data.Abstraction.Entities;

namespace ExinServer.Data.Sqlite.EntityExtensions
{
    internal static class CurrencyExtensions
    {
        public static AbstractEntities.Currency ToAbstract(this Currency dao)
        {
            if (dao == null)
                return null;

            return new AbstractEntities.Currency(dao.Id, dao.Code);
        }

        public static IEnumerable<AbstractEntities.Currency> ToAbstract(this IEnumerable<Currency> dao)
        {
            return dao?.Where(c => c != null).Select(c => c.ToAbstract());
        }

        public static Currency ToDao(this AbstractEntities.NewCurrency dto)
        {
            if (dto == null)
                return null;

            return new Currency
            {
                Code = dto.Code
            };
        }

        public static void Update(this Currency target, AbstractEntities.CurrencyUpdate source)
        {
            if (source == null || target == null)
                return;

            target.Code = source.Code;
        }
    }
}
