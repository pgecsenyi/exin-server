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
using ExinServer.Web.Entities;
using AbstractEntities = ExinServer.Data.Abstraction.Entities;

namespace ExinServer.Web.EntityExtensions
{
    public static class CurrencyExtensions
    {
        public static AbstractEntities.NewCurrency ToAbstract(this NewCurrency web)
        {
            if (web == null)
                return null;

            return new AbstractEntities.NewCurrency(web.Code);
        }

        public static AbstractEntities.CurrencyUpdate ToAbstract(this CurrencyUpdate web)
        {
            if (web == null)
                return null;

            return new AbstractEntities.CurrencyUpdate(web.Id, web.Code);
        }

        public static Currency ToWeb(this AbstractEntities.Currency dao)
        {
            if (dao == null)
                return null;

            return new Currency(dao.Id, dao.Code);
        }

        public static IEnumerable<Currency> ToWeb(this IEnumerable<AbstractEntities.Currency> dao)
        {
            return dao?.Where(c => c != null).Select(c => c.ToWeb());
        }
    }
}
