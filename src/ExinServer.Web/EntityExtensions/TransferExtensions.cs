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
    public static class TransferExtensions
    {
        public static AbstractEntities.NewTransfer ToAbstract(this NewTransfer web)
        {
            if (web == null)
                return null;

            return new AbstractEntities.NewTransfer(
                categoryId: web.CategoryId,
                currencyId: web.CurrencyId,
                discount: web.Discount,
                items: web.Items.ToAbstract().ToArray(),
                note: web.Note,
                partnerId: web.PartnerId,
                title: web.Title,
                time: web.Time);
        }

        public static AbstractEntities.TransferUpdate ToAbstract(this TransferUpdate web)
        {
            if (web == null)
                return null;

            return new AbstractEntities.TransferUpdate(
                categoryId: web.CategoryId,
                currencyId: web.CurrencyId,
                discount: web.Discount,
                id: web.Id,
                items: web.Items.ToAbstract().ToArray(),
                note: web.Note,
                partnerId: web.PartnerId,
                title: web.Title,
                time: web.Time);
        }

        public static Transfer ToWeb(this AbstractEntities.Transfer dto)
        {
            if (dto == null)
                return null;

            return new Transfer
            {
                Category = dto.Category.ToWeb(),
                Currency = dto.Currency.ToWeb(),
                Discount = dto.Discount,
                Id = dto.Id,
                Items = dto.Items.ToWeb().ToArray(),
                Note = dto.Note,
                Partner = dto.Partner.ToWeb(),
                Title = dto.Title,
                Time = dto.Time,
            };
        }

        public static IEnumerable<Transfer> ToWeb(this IEnumerable<AbstractEntities.Transfer> web)
        {
            return web?.Where(t => t != null).Select(t => t.ToWeb());
        }
    }
}
