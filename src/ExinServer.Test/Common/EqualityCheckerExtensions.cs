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
using ExinServer.Web.Entities;

namespace ExinServer.Test.Common
{
    internal static class EqualityCheckerExtensions
    {
        public static bool IsEqualTo(this Category c1, Category c2)
        {
            return c1.Id == c2.Id && string.Compare(c1.Name, c2.Name) == 0;
        }

        public static bool IsEqualTo(this Currency c1, Currency c2)
        {
            return c1.Id == c2.Id && string.Compare(c1.Code, c2.Code) == 0;
        }

        public static bool IsEqualTo(this Partner p1, Partner p2)
        {
            return p1.Id == p2.Id
                && string.Compare(p1.Address, p2.Address) == 0
                && string.Compare(p1.Name, p2.Name) == 0;
        }

        public static bool IsEqualTo(this Transfer t1, Transfer t2)
        {
            return t1.Id == t2.Id
                && t1.Category.IsEqualTo(t2.Category)
                && t1.Partner.IsEqualTo(t2.Partner)
                && t1.Currency.IsEqualTo(t2.Currency)
                && string.Compare(t1.Title, t2.Title) == 0
                && t1.Time == t2.Time
                && t1.Discount == t2.Discount
                && string.Compare(t1.Note, t2.Note) == 0;
        }

        public static bool IsEqualTo(this TransferItem i1, TransferItem i2)
        {
            return i1.Id == i2.Id
                && string.Compare(i1.Name, i2.Name) == 0
                && i1.Price == i2.Price
                && i1.Discount == i2.Discount;
        }

        public static bool IsEqualTo(this IEnumerable<TransferItem> is1, IEnumerable<TransferItem> is2)
        {
            var e2 = is2.GetEnumerator();

            foreach (var i1 in is1)
            {
                e2.MoveNext();
                if (!i1.IsEqualTo(e2.Current))
                    return false;
            }

            return true;
        }
    }
}
