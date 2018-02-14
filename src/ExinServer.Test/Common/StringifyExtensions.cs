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
using System.Text;
using ExinServer.Web.Entities;

namespace ExinServer.Test.Common
{
    internal static class StringifyExtensions
    {
        public static string Stringify(this Category category)
        {
            return $"{{{category.Id}; {category.Name}}}";
        }

        public static string Stringify(this Currency currency)
        {
            return $"{{{currency.Id}; {currency.Code}}}";
        }

        public static string Stringify(this Partner partner)
        {
            return $"{{{partner.Id}; {partner.Name}; {partner.Address}}}";
        }

        public static string Stringify(this Transfer transfer)
        {
            return $"{{{transfer.Category.Id}; {transfer.Currency.Id}; {transfer.Partner.Id}"
                + $"{transfer.Title}; {transfer.Time}; {transfer.Discount}"
                + $"{transfer.Note}; {transfer.Items.Stringify()}}}";
        }

        public static string Stringify(this TransferItem transferItem)
        {
            return $"{{{transferItem.Name}; {transferItem.Price}; {transferItem.Discount}}}";
        }

        public static string Stringify(this IEnumerable<TransferItem> transferItems)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("[");

            foreach (var i in transferItems)
                stringBuilder.Append(i.Stringify());

            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }
    }
}
