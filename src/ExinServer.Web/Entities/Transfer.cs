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

namespace ExinServer.Web.Entities
{
    public class Transfer
    {
        public Category Category { get; set; }

        public Currency Currency { get; set; }

        public decimal Discount { get; set; }

        public int Id { get; set; }

        public TransferItem[] Items { get; set; }

        public string Note { get; set; }

        public Partner Partner { get; set; }

        public DateTime Time { get; set; }

        public string Title { get; set; }
    }

    public class NewTransfer
    {
        public int CategoryId { get; set; }

        public int CurrencyId { get; set; }

        public decimal Discount { get; set; }

        public NewTransferItem[] Items { get; set; }

        public string Note { get; set; }

        public int PartnerId { get; set; }

        public DateTime Time { get; set; }

        public string Title { get; set; }
    }

    public class TransferUpdate
    {
        public int CategoryId { get; set; }

        public int CurrencyId { get; set; }

        public decimal Discount { get; set; }

        public int Id { get; set; }

        public TransferItemUpdate[] Items { get; set; }

        public string Note { get; set; }

        public int PartnerId { get; set; }

        public DateTime Time { get; set; }

        public string Title { get; set; }
    }
}
