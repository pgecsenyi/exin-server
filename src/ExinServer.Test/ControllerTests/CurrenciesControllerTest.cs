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
using ExinServer.Data.Abstraction.Exceptions;
using ExinServer.Test.Common;
using ExinServer.Web.Controllers;
using ExinServer.Web.Entities;
using ExinServer.Web.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExinServer.Test.ControllerTests
{
    [TestClass]
    public class CurrenciesControllerTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Create_WhenParamNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CurrenciesController(dataLayer))
                controller.Create(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Create_WhenCodeNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CurrenciesController(dataLayer))
                controller.Create(new NewCurrency(null));
        }

        [TestMethod]
        [ExpectedException(typeof(RecordAlreadyExistsException), "Currency with code \"EUR\" already exists.")]
        public void Create_WhenRecordExists_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CurrenciesController(dataLayer))
            {
                var newCurrency = TestDataProvider.CreateNewCurrency();
                controller.Create(newCurrency);
                controller.Create(newCurrency);
            }
        }

        [TestMethod]
        public void Create_Normally_ShouldReturn_Currency()
        {
            var newCurrency = TestDataProvider.CreateNewCurrency();
            Currency currency;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CurrenciesController(dataLayer))
                currency = controller.CreateCurrency(newCurrency);

            Assert.IsTrue(
                currency.Id > 0,
                $"Currency ID is expected to greater than 0. Actual: {currency.Id}.");
            Assert.AreEqual(
                newCurrency.Code,
                currency.Code,
                $"Invalid currency code. Expected: \"{newCurrency.Code}\", actual: \"{currency.Code}\".");
        }

        [TestMethod]
        [ExpectedException(typeof(RecordDoesNotExistException), "Currency with ID 1 does not exist.")]
        public void Delete_WhenRecordDoesNotExist_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CurrenciesController(dataLayer))
                controller.Delete(1);
        }

        [TestMethod]
        public void Delete_Normally_ShouldWork()
        {
            Currency createdCurrency, queriedCurrency;
            IEnumerable<Currency> listedCurrencies;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CurrenciesController(dataLayer))
            {
                createdCurrency = controller.CreateCurrency(TestDataProvider.CreateNewCurrency());
                queriedCurrency = controller.Get(createdCurrency.Id);
                controller.Delete(createdCurrency.Id);
                listedCurrencies = controller.List();
            }

            Assert.AreEqual(createdCurrency.Id, queriedCurrency.Id, "Unexpected ID.");
            Assert.AreEqual(false, listedCurrencies.Any(), "There should be no partners returned.");
        }

        [TestMethod]
        [ExpectedException(typeof(RecordDoesNotExistException), "Currency with ID 0 does not exist.")]
        public void Get_WhenRecordDoesNotExist_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CurrenciesController(dataLayer))
                controller.Get(0);
        }

        [TestMethod]
        public void Get_Normally_ShouldReturn_CurrencyWithId()
        {
            Currency createdCurrency, queriedCurrency;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CurrenciesController(dataLayer))
            {
                createdCurrency = controller.CreateCurrency(TestDataProvider.CreateNewCurrency());
                queriedCurrency = controller.Get(createdCurrency.Id);
            }

            Assert.IsTrue(
                queriedCurrency.IsEqualTo(createdCurrency),
                "The two currencies should be equal. "
                    + $"Expected: {createdCurrency.Stringify()}, actual: {queriedCurrency.Stringify()}.");
        }

        [TestMethod]
        public void List_ShouldReturn_CreatedCurrencies()
        {
            Currency queriedCurrency1, queriedCurrency2;
            IEnumerable<Currency> queriedCurrencies;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CurrenciesController(dataLayer))
            {
                queriedCurrency1 = controller.CreateCurrency(TestDataProvider.CreateNewCurrency());
                queriedCurrency2 = controller.CreateCurrency(TestDataProvider.CreateAnotherNewCurrency());
                queriedCurrencies = controller.List();
            }

            AssertCurrenciesInList(queriedCurrencies, queriedCurrency1, queriedCurrency2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Update_WhenNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CurrenciesController(dataLayer))
                controller.Update(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Update_WhenCodeNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CurrenciesController(dataLayer))
                controller.Update(new CurrencyUpdate(1, null));
        }

        [TestMethod]
        [ExpectedException(typeof(RecordDoesNotExistException), "Currency with ID 1 does not exist.")]
        public void Update_WhenRecordDoesNotExist_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CurrenciesController(dataLayer))
            {
                var currencyUpdate = new CurrencyUpdate(1, "USD");
                controller.Update(currencyUpdate);
            }
        }

        [TestMethod]
        public void Update_Normally_ShouldWork()
        {
            Currency createdCurrency, updatedCurrency;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CurrenciesController(dataLayer))
            {
                createdCurrency = controller.CreateCurrency(TestDataProvider.CreateNewCurrency());
                controller.Update(new CurrencyUpdate(createdCurrency.Id, "USD"));
                updatedCurrency = controller.Get(createdCurrency.Id);
            }

            Assert.AreEqual("USD", updatedCurrency.Code);
        }

        private void AssertCurrenciesInList(IEnumerable<Currency> currencies, params Currency[] createdCurrencies)
        {
            AssertionHelper.AssertOnlyItemsInList(
                Common.EqualityCheckerExtensions.IsEqualTo,
                Common.StringifyExtensions.Stringify,
                currencies,
                createdCurrencies);
        }
    }
}
