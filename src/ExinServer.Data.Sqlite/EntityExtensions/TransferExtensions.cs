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
    internal static class TransferExtensions
    {
        public static AbstractEntities.Transfer ToAbstract(this Transfer dao)
        {
            if (dao == null)
                return null;

            return new AbstractEntities.Transfer(
                id: dao.Id,
                category: dao.Category.ToAbstract(),
                currency: dao.Currency.ToAbstract(),
                discount: dao.Discount,
                items: dao.Items.ToAbstract().ToArray(),
                note: dao.Note,
                partner: dao.Partner.ToAbstract(),
                title: dao.Title,
                time: dao.Time);
        }

        public static IEnumerable<AbstractEntities.Transfer> ToAbstract(this IEnumerable<Transfer> dao)
        {
            return dao?.Where(t => t != null).Select(t => t.ToAbstract());
        }

        public static Transfer ToDao(this AbstractEntities.NewTransfer dto)
        {
            if (dto == null)
                return null;

            return new Transfer
            {
                CategoryId = dto.CategoryId,
                CurrencyId = dto.CurrencyId,
                Discount = dto.Discount,
                Items = dto.Items.ToDao().ToArray(),
                Note = dto.Note,
                PartnerId = dto.PartnerId,
                Title = dto.Title,
                Time = dto.Time,
            };
        }

        public static void Update(this Transfer target, AbstractEntities.TransferUpdate source)
        {
            if (source == null || target == null)
                return;

            target.CategoryId = source.CategoryId;
            target.CurrencyId = source.CurrencyId;
            target.Discount = source.Discount;
            target.Note = source.Note;
            target.PartnerId = source.PartnerId;
            target.Title = source.Title;
            target.Time = source.Time;
        }
    }
}
