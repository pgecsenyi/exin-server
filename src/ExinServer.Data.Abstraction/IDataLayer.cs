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
using ExinServer.Data.Abstraction.Entities;

namespace ExinServer.Data.Abstraction
{
    public interface IDataLayer : IDisposable
    {
        Category CreateCategory(NewCategory newCategory);

        Currency CreateCurrency(NewCurrency newCurrency);

        Partner CreatePartner(NewPartner newPartner);

        Transfer CreateTransfer(NewTransfer newTransfer);

        void DeleteCategory(int categoryId);

        void DeleteCurrency(int currencyId);

        void DeletePartner(int partnerId);

        void DeleteTransfer(int transferId);

        void EnsureCreated();

        Category GetCategory(int categoryId);

        Currency GetCurrency(int currencyId);

        Partner GetPartner(int partnerId);

        Transfer GetTransfer(int transferId);

        Category[] ListCategories();

        Currency[] ListCurrencies();

        Partner[] ListPartners();

        Transfer[] ListTransfers();

        Category UpdateCategory(CategoryUpdate categoryUpdate);

        Currency UpdateCurrency(CurrencyUpdate currencyUpdate);

        Partner UpdatePartner(PartnerUpdate partnerUpdate);

        Transfer UpdateTransfer(TransferUpdate transferUpdate);
    }
}
