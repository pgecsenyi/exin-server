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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExinServer.Data.Sqlite.Entities
{
    internal class Transfer
    {
        [Column(Order = 0)]
        public int Id { get; set; }

        [Column(Order = 1)]
        public int CategoryId { get; set; }

        [Column(Order = 2)]
        public int PartnerId { get; set; }

        [Column(Order = 3)]
        public int CurrencyId { get; set; }

        [Column(Order = 4, TypeName = "VARCHAR(255)")]
        [MaxLength(255)]
        [Required]
        public string Title { get; set; }

        [Column(Order = 5, TypeName = "DATETIME")]
        [Required]
        public DateTime Time { get; set; }

        [Column(Order = 6)]
        public decimal Discount { get; set; }

        [Column(Order = 7)]
        public string Note { get; set; }

        public Category Category { get; set; }

        public Currency Currency { get; set; }

        public Partner Partner { get; set; }

        public ICollection<TransferItem> Items { get; set; }
    }
}
