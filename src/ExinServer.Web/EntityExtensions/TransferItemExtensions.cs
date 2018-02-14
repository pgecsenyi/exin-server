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
    public static class TransferItemExtensions
    {
        public static AbstractEntities.NewTransferItem ToAbstract(this NewTransferItem web)
        {
            if (web == null)
                return null;

            return new AbstractEntities.NewTransferItem(web.Name, web.Price, web.Discount);
        }

        public static IEnumerable<AbstractEntities.NewTransferItem> ToAbstract(this IEnumerable<NewTransferItem> web)
        {
            return web?.Where(i => i != null).Select(i => i.ToAbstract()) ?? new AbstractEntities.NewTransferItem[0];
        }

        public static AbstractEntities.TransferItemUpdate ToAbstract(this TransferItemUpdate web)
        {
            if (web == null)
                return null;

            return new AbstractEntities.TransferItemUpdate(web.Id, web.Name, web.Price, web.Discount);
        }

        public static IEnumerable<AbstractEntities.TransferItemUpdate> ToAbstract(
            this IEnumerable<TransferItemUpdate> web)
        {
            return web?.Where(i => i != null).Select(i => i.ToAbstract()) ?? new AbstractEntities.TransferItemUpdate[0];
        }

        public static TransferItem ToWeb(this AbstractEntities.TransferItem dto)
        {
            if (dto == null)
                return null;

            return new TransferItem(dto.Id, dto.Name, dto.Price, dto.Discount);
        }

        public static IEnumerable<TransferItem> ToWeb(this IEnumerable<AbstractEntities.TransferItem> dto)
        {
            return dto?.Where(i => i != null).Select(i => i.ToWeb()) ?? new TransferItem[0];
        }
    }
}
