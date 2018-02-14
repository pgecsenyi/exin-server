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
    internal static class TransferItemExtensions
    {
        public static AbstractEntities.TransferItem ToAbstract(this TransferItem dao)
        {
            if (dao == null)
                return null;

            return new AbstractEntities.TransferItem(dao.Id, dao.Name, dao.Price, dao.Discount);
        }

        public static IEnumerable<AbstractEntities.TransferItem> ToAbstract(this IEnumerable<TransferItem> dao)
        {
            return dao?.Where(i => i != null).Select(i => i.ToAbstract()) ?? new AbstractEntities.TransferItem[0];
        }

        public static TransferItem ToDao(this AbstractEntities.NewTransferItem dto)
        {
            if (dto == null)
                return null;

            return new TransferItem
            {
                Discount = dto.Discount,
                Name = dto.Name,
                Price = dto.Price
            };
        }

        public static IEnumerable<TransferItem> ToDao(this IEnumerable<AbstractEntities.NewTransferItem> dto)
        {
            return dto?.Where(i => i != null).Select(i => i.ToDao()) ?? new TransferItem[0];
        }

        public static TransferItem ToDao(this AbstractEntities.TransferItemUpdate dto)
        {
            if (dto == null)
                return null;

            return new TransferItem
            {
                Discount = dto.Discount,
                Name = dto.Name,
                Price = dto.Price
            };
        }

        public static void Update(this TransferItem target, AbstractEntities.TransferItemUpdate source)
        {
            if (source == null || target == null)
                return;

            target.Discount = source.Discount;
            target.Name = source.Name;
            target.Price = source.Price;
        }
    }
}
