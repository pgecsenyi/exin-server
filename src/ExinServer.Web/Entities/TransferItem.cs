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
    public class TransferItem
    {
        private readonly decimal discount;
        private readonly int id;
        private readonly string name;
        private readonly decimal price;

        public TransferItem(int id, string name, decimal price, decimal discount)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.discount = discount;
        }

        public decimal Discount => discount;

        public int Id => id;

        public string Name => name;

        public decimal Price => price;
    }

    public class NewTransferItem
    {
        private readonly decimal discount;
        private readonly string name;
        private readonly decimal price;

        public NewTransferItem(string name, decimal price, decimal discount)
        {
            this.name = name;
            this.price = price;
            this.discount = discount;
        }

        public decimal Discount => discount;

        public string Name => name;

        public decimal Price => price;
    }

    public class TransferItemUpdate
    {
        private readonly decimal discount;
        private readonly int id;
        private readonly string name;
        private readonly decimal price;

        public TransferItemUpdate(int id, string name, decimal price, decimal discount)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.discount = discount;
        }

        public decimal Discount => discount;

        public int Id => id;

        public string Name => name;

        public decimal Price => price;
    }
}
