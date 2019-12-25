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
    public static class CategoryExtensions
    {
        public static AbstractEntities.NewCategory ToAbstract(this NewCategory web)
        {
            if (web == null)
                return null;

            return new AbstractEntities.NewCategory(web.Name);
        }

        public static AbstractEntities.CategoryUpdate ToAbstract(this CategoryUpdate web)
        {
            if (web == null)
                return null;

            return new AbstractEntities.CategoryUpdate(web.Id, web.Name);
        }

        public static Category ToWeb(this AbstractEntities.Category dao)
        {
            if (dao == null)
                return null;

            return new Category { Id = dao.Id, Name = dao.Name };
        }

        public static IEnumerable<Category> ToWeb(this IEnumerable<AbstractEntities.Category> dao)
        {
            return dao?.Where(c => c != null).Select(c => c.ToWeb());
        }
    }
}
