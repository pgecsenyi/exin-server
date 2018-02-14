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

namespace ExinServer.Data.Abstraction.Entities
{
    public class Currency
    {
        private readonly string code;
        private readonly int id;

        public Currency(int id, string code)
        {
            this.id = id;
            this.code = code;
        }

        public string Code => code;

        public int Id => id;
    }

    public class NewCurrency
    {
        private readonly string code;

        public NewCurrency(string code)
        {
            this.code = code;
        }

        public string Code => code;
    }

    public class CurrencyUpdate
    {
        private readonly string code;
        private readonly int id;

        public CurrencyUpdate(int id, string code)
        {
            this.id = id;
            this.code = code;
        }

        public string Code => code;

        public int Id => id;
    }
}
