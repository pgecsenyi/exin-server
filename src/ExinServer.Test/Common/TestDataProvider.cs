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
using ExinServer.Web.Entities;

namespace ExinServer.Test.Common
{
    internal static class TestDataProvider
    {
        public static NewCategory CreateNewCategory()
        {
            return new NewCategory { Name = "Food" };
        }

        public static NewCategory CreateAnotherNewCategory()
        {
            return new NewCategory { Name = "Cinema" };
        }

        public static NewCurrency CreateNewCurrency()
        {
            return new NewCurrency { Code = "EUR" };
        }

        public static NewCurrency CreateAnotherNewCurrency()
        {
            return new NewCurrency { Code = "USD" };
        }

        public static NewPartner CreateNewPartner()
        {
            return new NewPartner { Name = "Nowhere Man", Address = "Nowhere Land" };
        }

        public static NewPartner CreateAnotherNewPartner()
        {
            return new NewPartner { Name = "Elenaor Rigby", Address = "Liverpool" };
        }

        public static NewTransfer CreateNewTransfer(
            int categoryId,
            int partnerId,
            int currencyId,
            params NewTransferItem[] items)
        {
            return new NewTransfer
            {
                CategoryId = categoryId,
                PartnerId = partnerId,
                CurrencyId = currencyId,
                Title = "Cottege cheese pasta",
                Time = DateTime.UtcNow,
                Discount = 0,
                Note = "Delicious.",
                Items = items,
            };
        }

        public static TransferUpdate CreateTransferUpdate(
            int transferId,
            int categoryId,
            int partnerId,
            int currencyId,
            params TransferItemUpdate[] items)
        {
            return new TransferUpdate
            {
                Id = transferId,
                CategoryId = categoryId,
                PartnerId = partnerId,
                CurrencyId = currencyId,
                Title = "Lasagne",
                Time = DateTime.UtcNow,
                Discount = 0.1M,
                Note = null,
                Items = items,
            };
        }

        public static NewTransferItem CreateNewTransferItem()
        {
            return new NewTransferItem { Name = "Gum", Price = 0.9M, Discount = 0.2M };
        }

        public static TransferItemUpdate CreateTransferItemUpdate(int id)
        {
            return new TransferItemUpdate { Id = id, Name = "Bread", Price = 1.0M, Discount = 0 };
        }
    }
}
