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
    public static class PartnerExtensions
    {
        public static AbstractEntities.NewPartner ToAbstract(this NewPartner web)
        {
            if (web == null)
                return null;

            return new AbstractEntities.NewPartner(web.Name, web.Address);
        }

        public static AbstractEntities.PartnerUpdate ToAbstract(this PartnerUpdate web)
        {
            if (web == null)
                return null;

            return new AbstractEntities.PartnerUpdate(web.Id, web.Name, web.Address);
        }

        public static Partner ToWeb(this AbstractEntities.Partner dao)
        {
            if (dao == null)
                return null;

            return new Partner(dao.Id, dao.Name, dao.Address);
        }

        public static IEnumerable<Partner> ToWeb(this IEnumerable<AbstractEntities.Partner> dao)
        {
            return dao?.Where(p => p != null).Select(p => p.ToWeb());
        }
    }
}
