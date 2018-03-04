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
    public class TransfersControllerTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Create_WhenParamNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new TransfersController(dataLayer))
                controller.Create(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Create_WhenTitleNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new TransfersController(dataLayer))
                controller.Create(new NewTransfer(1, 1, 1, null, DateTime.UtcNow, 0.3M, null, null));
        }

        [TestMethod]
        [ExpectedException(typeof(RecordDoesNotExistException), "Transfer with ID 1 does not exist.")]
        public void Create_WhenRecordExists_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new TransfersController(dataLayer))
            {
                var newTransfer = TestDataProvider.CreateNewTransfer(1, 1, 1, TestDataProvider.CreateNewTransferItem());
                controller.Create(newTransfer);
                controller.Create(newTransfer);
            }
        }

        [TestMethod]
        public void Create_Normally_ShouldReturn_Transfer()
        {
            NewTransfer newTransfer;
            Transfer transfer;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var categoriesController = new CategoriesController(dataLayer))
            using (var currenciesController = new CurrenciesController(dataLayer))
            using (var partnersController = new PartnersController(dataLayer))
            using (var transfersController = new TransfersController(dataLayer))
            {
                var category = categoriesController.Create(TestDataProvider.CreateNewCategory());
                var partner = partnersController.Create(TestDataProvider.CreateAnotherNewPartner());
                var currency = currenciesController.Create(TestDataProvider.CreateAnotherNewCurrency());

                newTransfer = TestDataProvider.CreateNewTransfer(
                    category.Id,
                    partner.Id,
                    currency.Id,
                    TestDataProvider.CreateNewTransferItem());
                transfer = transfersController.Create(newTransfer);
            }

            Assert.AreEqual(newTransfer.CategoryId, transfer.Category.Id, "Unexpected category.");
            Assert.AreEqual(newTransfer.CurrencyId, transfer.Currency.Id, "Unexpected currency.");
            Assert.AreEqual(newTransfer.Discount, transfer.Discount, "Unexpected discount.");
            Assert.IsTrue(transfer.Id > 0, $"Transfer ID is expected to greater than 0. Actual: {transfer.Id}.");
            Assert.AreEqual(newTransfer.Items.Length, transfer.Items.Length, "Unexpected transfer item count.");
            Assert.AreEqual(newTransfer.Items[0].Discount, transfer.Items[0].Discount, "Unexpected item discount.");
            Assert.IsTrue(
                transfer.Items[0].Id > 0,
                $"Item ID is expected to greater than 0. Actual: {transfer.Items[0].Id}.");
            Assert.AreEqual(newTransfer.Items[0].Name, transfer.Items[0].Name, "Unexpected item name.");
            Assert.AreEqual(newTransfer.Items[0].Price, transfer.Items[0].Price, "Unexpected item price.");
            Assert.AreEqual(newTransfer.Note, transfer.Note, "Unexpected note.");
            Assert.AreEqual(newTransfer.PartnerId, transfer.Partner.Id, "Unexpected partner.");
            Assert.AreEqual(newTransfer.Time, transfer.Time, "Unexpected time.");
            Assert.AreEqual(newTransfer.Title, transfer.Title, "Unexpected title.");
        }

        [TestMethod]
        [ExpectedException(typeof(RecordDoesNotExistException), "Transfer with ID 1 does not exist.")]
        public void Delete_WhenRecordDoesNotExist_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new TransfersController(dataLayer))
                controller.Delete(1);
        }

        [TestMethod]
        public void Delete_Normally_ShouldWork()
        {
            Transfer createdTransfer, queriedTransfer;
            IEnumerable<Transfer> listedTransfers;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var categoriesController = new CategoriesController(dataLayer))
            using (var currenciesController = new CurrenciesController(dataLayer))
            using (var partnersController = new PartnersController(dataLayer))
            using (var transfersController = new TransfersController(dataLayer))
            {
                var category = categoriesController.Create(TestDataProvider.CreateNewCategory());
                var partner = partnersController.Create(TestDataProvider.CreateAnotherNewPartner());
                var currency = currenciesController.Create(TestDataProvider.CreateAnotherNewCurrency());

                var newTransfer = TestDataProvider.CreateNewTransfer(
                    category.Id,
                    partner.Id,
                    currency.Id,
                    TestDataProvider.CreateNewTransferItem());
                createdTransfer = transfersController.Create(newTransfer);

                queriedTransfer = transfersController.Get(createdTransfer.Id);
                transfersController.Delete(createdTransfer.Id);
                listedTransfers = transfersController.List();
            }

            Assert.AreEqual(createdTransfer.Id, queriedTransfer.Id, "Unexpected ID.");
            Assert.AreEqual(false, listedTransfers.Any(), "There should be no transfers returned.");
        }

        [TestMethod]
        [ExpectedException(typeof(RecordDoesNotExistException), "Transfer with ID 0 does not exist.")]
        public void Get_WhenRecordDoesNotExist_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new TransfersController(dataLayer))
                controller.Get(0);
        }

        [TestMethod]
        public void Get_Normally_ShouldReturn_TransferWithId()
        {
            Transfer createdTransfer, queriedTransfer;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var categoriesController = new CategoriesController(dataLayer))
            using (var currenciesController = new CurrenciesController(dataLayer))
            using (var partnersController = new PartnersController(dataLayer))
            using (var transfersController = new TransfersController(dataLayer))
            {
                var category = categoriesController.Create(TestDataProvider.CreateAnotherNewCategory());
                var partner = partnersController.Create(TestDataProvider.CreateAnotherNewPartner());
                var currency = currenciesController.Create(TestDataProvider.CreateNewCurrency());

                var newTransfer = TestDataProvider.CreateNewTransfer(
                    category.Id,
                    partner.Id,
                    currency.Id,
                    TestDataProvider.CreateNewTransferItem());
                createdTransfer = transfersController.Create(newTransfer);
                queriedTransfer = transfersController.Get(createdTransfer.Id);
            }

            Assert.IsTrue(
                queriedTransfer.IsEqualTo(createdTransfer),
                "The two transfers should be equal. "
                    + $"Expected: {createdTransfer.Stringify()}, actual: {queriedTransfer.Stringify()}.");
        }

        [TestMethod]
        public void List_ShouldReturn_CreatedTransfers()
        {
            Transfer createdTransfer1, createdTransfer2;
            IEnumerable<Transfer> queriedTransfers;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var categoriesController = new CategoriesController(dataLayer))
            using (var currenciesController = new CurrenciesController(dataLayer))
            using (var partnersController = new PartnersController(dataLayer))
            using (var transfersController = new TransfersController(dataLayer))
            {
                var category = categoriesController.Create(TestDataProvider.CreateNewCategory());
                var partner = partnersController.Create(TestDataProvider.CreateNewPartner());
                var currency = currenciesController.Create(TestDataProvider.CreateNewCurrency());

                var newTransfer1 = TestDataProvider.CreateNewTransfer(
                    category.Id,
                    partner.Id,
                    currency.Id,
                    TestDataProvider.CreateNewTransferItem());
                createdTransfer1 = transfersController.Create(newTransfer1);
                var newTransfer2 = TestDataProvider.CreateNewTransfer(
                    category.Id,
                    partner.Id,
                    currency.Id,
                    TestDataProvider.CreateNewTransferItem());
                createdTransfer2 = transfersController.Create(newTransfer2);
                queriedTransfers = transfersController.List();
            }

            AssertTransfersInList(queriedTransfers, createdTransfer1, createdTransfer2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Update_WhenNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new TransfersController(dataLayer))
                controller.Update(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Update_WhenNameNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new TransfersController(dataLayer))
                controller.Update(new TransferUpdate(1, 1, 1, 1, null, DateTime.UtcNow, 0.3M, null, null));
        }

        [TestMethod]
        [ExpectedException(typeof(RecordDoesNotExistException), "Transfer with ID 1 does not exist.")]
        public void Update_WhenRecordDoesNotExist_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new TransfersController(dataLayer))
            {
                var transferUpdate = TestDataProvider.CreateTransferUpdate(1, 1, 1, 1);
                controller.Update(transferUpdate);
            }
        }

        [TestMethod]
        public void Update_Normally_ShouldWork()
        {
            Category category2;
            Transfer createdTransfer, updatedTransfer;
            TransferUpdate transferUpdate;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var categoriesController = new CategoriesController(dataLayer))
            using (var currenciesController = new CurrenciesController(dataLayer))
            using (var partnersController = new PartnersController(dataLayer))
            using (var transfersController = new TransfersController(dataLayer))
            {
                var category1 = categoriesController.Create(TestDataProvider.CreateNewCategory());
                var partner = partnersController.Create(TestDataProvider.CreateNewPartner());
                var currency = currenciesController.Create(TestDataProvider.CreateNewCurrency());

                category2 = categoriesController.Create(TestDataProvider.CreateAnotherNewCategory());

                var newTransfer = TestDataProvider.CreateNewTransfer(
                    category1.Id,
                    partner.Id,
                    currency.Id,
                    TestDataProvider.CreateNewTransferItem());
                createdTransfer = transfersController.Create(newTransfer);

                transferUpdate = TestDataProvider.CreateTransferUpdate(
                    createdTransfer.Id,
                    category2.Id,
                    partner.Id,
                    currency.Id,
                    TestDataProvider.CreateTransferItemUpdate(createdTransfer.Items[0].Id));
                updatedTransfer = transfersController.Update(transferUpdate);
            }

            // Note that EF Core behaves slightly differently when an in-memory database is used, these checks are not
            // necessarily enough. Use "curl" or something similar, preferably a REST test-client to make sure
            // everything works as expected.
            Assert.IsTrue(
                updatedTransfer.Category.IsEqualTo(category2),
                $"Unexpected category. Expected: ${category2}. Actual: ${updatedTransfer.Category}.");
            Assert.IsTrue(
                updatedTransfer.Currency.IsEqualTo(createdTransfer.Currency),
                $"Unexpected currency. Expected: ${createdTransfer.Currency}. Actual: ${updatedTransfer.Currency}.");
            Assert.AreEqual(transferUpdate.Discount, updatedTransfer.Discount, "Unexpected discount.");
            Assert.AreEqual(
                transferUpdate.Items.Length,
                updatedTransfer.Items.Length,
                "Unexpected transfer item count.");
            Assert.AreEqual(
                transferUpdate.Items[0].Discount,
                updatedTransfer.Items[0].Discount,
                "Unexpected item discount.");
            Assert.AreEqual(
                createdTransfer.Items[0].Id,
                updatedTransfer.Items[0].Id,
                "Unexpected item ID");
            Assert.AreEqual(transferUpdate.Items[0].Name, updatedTransfer.Items[0].Name, "Unexpected item name.");
            Assert.AreEqual(transferUpdate.Items[0].Price, updatedTransfer.Items[0].Price, "Unexpected item price.");
            Assert.AreEqual(transferUpdate.Note, updatedTransfer.Note, "Unexpected note.");
            Assert.IsTrue(
                updatedTransfer.Partner.IsEqualTo(createdTransfer.Partner),
                $"Unexpected partner. Expected: ${createdTransfer.Partner}. Actual: ${updatedTransfer.Partner}.");
            Assert.AreEqual(transferUpdate.Time, updatedTransfer.Time, "Unexpected time.");
            Assert.AreEqual(transferUpdate.Title, updatedTransfer.Title, "Unexpected title.");
        }

        private void AssertTransfersInList(IEnumerable<Transfer> transfers, params Transfer[] createdTransfers)
        {
            AssertionHelper.AssertOnlyItemsInList(
                Common.EqualityCheckerExtensions.IsEqualTo,
                Common.StringifyExtensions.Stringify,
                transfers,
                createdTransfers);
        }
    }
}
