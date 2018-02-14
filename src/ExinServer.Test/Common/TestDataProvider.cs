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
            return new NewCategory("Food");
        }

        public static NewCategory CreateAnotherNewCategory()
        {
            return new NewCategory("Cinema");
        }

        public static NewCurrency CreateNewCurrency()
        {
            return new NewCurrency("EUR");
        }

        public static NewCurrency CreateAnotherNewCurrency()
        {
            return new NewCurrency("USD");
        }

        public static NewPartner CreateNewPartner()
        {
            return new NewPartner("Nowhere Man", "Nowhere Land");
        }

        public static NewPartner CreateAnotherNewPartner()
        {
            return new NewPartner("Elenaor Rigby", "Liverpool");
        }

        public static NewTransfer CreateNewTransfer(
            int categoryId,
            int partnerId,
            int currencyId,
            params NewTransferItem[] items)
        {
            return new NewTransfer(
                categoryId: categoryId,
                partnerId: partnerId,
                currencyId: currencyId,
                title: "Cottege cheese pasta",
                time: DateTime.UtcNow,
                discount: 0,
                note: "Delicious.",
                items: items);
        }

        public static TransferUpdate CreateTransferUpdate(
            int transferId,
            int categoryId,
            int partnerId,
            int currencyId,
            params TransferItemUpdate[] items)
        {
            return new TransferUpdate(
                id: transferId,
                categoryId: categoryId,
                partnerId: partnerId,
                currencyId: currencyId,
                title: "Lasagne",
                time: DateTime.UtcNow,
                discount: 0.1M,
                note: null,
                items: items);
        }

        public static NewTransferItem CreateNewTransferItem()
        {
            return new NewTransferItem("Gum", 0.9M, 0.2M);
        }

        public static TransferItemUpdate CreateTransferItemUpdate(int id)
        {
            return new TransferItemUpdate(id, "Bread", 1.0M, 0);
        }
    }
}
