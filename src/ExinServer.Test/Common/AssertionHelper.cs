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

using System;
using System.Collections.Generic;
using System.Linq;
using ExinServer.Web.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExinServer.Test.Common
{
    internal static class AssertionHelper
    {
        public static void AssertOnlyItemsInList<T>(
            Func<T, T, bool> equals,
            Func<T, string> stringify,
            IEnumerable<T> actual,
            params T[] expected)
        {
            Assert.AreNotEqual(null, actual, "Actual list should not be NULL.");
            Assert.AreEqual(
                expected.Length,
                actual.Count(),
                $"The size of the two lists are not equal. Expected: {expected.Length}. Actual: {actual.Count()}.");

            foreach (var e in expected)
            {
                var doesContain = actual.Any(a => equals(a, e));
                Assert.IsTrue(doesContain, $"Result should contain ${stringify(e)}.");
            }
        }
    }
}
