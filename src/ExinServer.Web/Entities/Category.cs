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

namespace ExinServer.Web.Entities
{
    public class Category
    {
        private readonly int id;
        private readonly string name;

        public Category(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int Id => id;

        public string Name => name;
    }

    public class NewCategory
    {
        private readonly string name;

        public NewCategory(string name)
        {
            this.name = name;
        }

        public string Name => name;
    }

    public class CategoryUpdate
    {
        private readonly int id;
        private readonly string name;

        public CategoryUpdate(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int Id => id;

        public string Name => name;
    }
}