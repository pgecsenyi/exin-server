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

namespace ExinServer.Data.Abstraction.Entities
{
    public class Transfer
    {
        private readonly Category category;
        private readonly Currency currency;
        private readonly decimal discount;
        private readonly int id;
        private readonly TransferItem[] items;
        private readonly string note;
        private readonly Partner partner;
        private readonly DateTime time;
        private readonly string title;

        public Transfer(
            int id,
            Category category,
            Partner partner,
            Currency currency,
            string title,
            DateTime time,
            decimal discount,
            string note,
            TransferItem[] items)
        {
            this.id = id;
            this.category = category;
            this.partner = partner;
            this.currency = currency;
            this.title = title;
            this.time = time;
            this.discount = discount;
            this.note = note;
            this.items = items;
        }

        public Category Category => category;

        public Currency Currency => currency;

        public decimal Discount => discount;

        public int Id => id;

        public TransferItem[] Items => items;

        public string Note => note;

        public Partner Partner => partner;

        public DateTime Time => time;

        public string Title => title;
    }

    public class NewTransfer
    {
        private readonly int categoryId;
        private readonly int currencyId;
        private readonly decimal discount;
        private readonly NewTransferItem[] items;
        private readonly string note;
        private readonly int partnerId;
        private readonly DateTime time;
        private readonly string title;

        public NewTransfer(
            int categoryId,
            int partnerId,
            int currencyId,
            string title,
            DateTime time,
            decimal discount,
            string note,
            NewTransferItem[] items)
        {
            this.categoryId = categoryId;
            this.partnerId = partnerId;
            this.currencyId = currencyId;
            this.title = title;
            this.time = time;
            this.discount = discount;
            this.note = note;
            this.items = items;
        }

        public int CategoryId => categoryId;

        public int CurrencyId => currencyId;

        public decimal Discount => discount;

        public NewTransferItem[] Items => items;

        public string Note => note;

        public int PartnerId => partnerId;

        public DateTime Time => time;

        public string Title => title;
    }

    public class TransferUpdate
    {
        private readonly int categoryId;
        private readonly int currencyId;
        private readonly decimal discount;
        private readonly int id;
        private readonly TransferItemUpdate[] items;
        private readonly string note;
        private readonly int partnerId;
        private readonly DateTime time;
        private readonly string title;

        public TransferUpdate(
            int id,
            int categoryId,
            int partnerId,
            int currencyId,
            string title,
            DateTime time,
            decimal discount,
            string note,
            TransferItemUpdate[] items)
        {
            this.id = id;
            this.categoryId = categoryId;
            this.partnerId = partnerId;
            this.currencyId = currencyId;
            this.title = title;
            this.time = time;
            this.discount = discount;
            this.note = note;
            this.items = items;
        }

        public int CategoryId => categoryId;

        public int CurrencyId => currencyId;

        public decimal Discount => discount;

        public int Id => id;

        public TransferItemUpdate[] Items => items;

        public string Note => note;

        public int PartnerId => partnerId;

        public DateTime Time => time;

        public string Title => title;
    }
}
