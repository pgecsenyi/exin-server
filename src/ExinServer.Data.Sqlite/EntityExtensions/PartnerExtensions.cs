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
    internal static class PartnerExtensions
    {
        public static AbstractEntities.Partner ToAbstract(this Partner dao)
        {
            if (dao == null)
                return null;

            return new AbstractEntities.Partner(dao.Id, dao.Name, dao.Address);
        }

        public static IEnumerable<AbstractEntities.Partner> ToAbstract(this IEnumerable<Partner> dao)
        {
            return dao?.Where(p => p != null).Select(p => p.ToAbstract());
        }

        public static Partner ToDao(this AbstractEntities.NewPartner dto)
        {
            if (dto == null)
                return null;

            return new Partner
            {
                Address = dto.Address,
                Name = dto.Name
            };
        }

        public static void Update(this Partner target, AbstractEntities.PartnerUpdate source)
        {
            if (source == null || target == null)
                return;

            target.Address = source.Address;
            target.Name = source.Name;
        }
    }
}
