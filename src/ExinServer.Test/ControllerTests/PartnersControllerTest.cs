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
    public class PartnersControllerTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Create_WhenParamNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new PartnersController(dataLayer))
                controller.Create(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Create_WhenNameNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new PartnersController(dataLayer))
                controller.Create(new NewPartner(null, null));
        }

        [TestMethod]
        [ExpectedException(typeof(RecordAlreadyExistsException), "Partner with name \"Company Ltd.\" already exists.")]
        public void Create_WhenRecordExists_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new PartnersController(dataLayer))
            {
                var newPartner = TestDataProvider.CreateNewPartner();
                controller.Create(newPartner);
                controller.Create(newPartner);
            }
        }

        [TestMethod]
        public void Create_Normally_ShouldReturn_Partner()
        {
            var newPartner = TestDataProvider.CreateNewPartner();
            Partner partner;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new PartnersController(dataLayer))
                partner = controller.Create(newPartner);

            Assert.IsTrue(
                partner.Id > 0,
                $"Partner ID is expected to greater than 0. Actual: {partner.Id}.");
            Assert.AreEqual(
                newPartner.Name,
                partner.Name,
                $"Invalid partner name. Expected: \"{newPartner.Name}\", actual: \"{partner.Name}\".");
            Assert.AreEqual(
                newPartner.Address,
                partner.Address,
                $"Invalid partner address. Expected: \"{newPartner.Address}\", actual: \"{partner.Address}\".");
        }

        [TestMethod]
        [ExpectedException(typeof(RecordDoesNotExistException), "Partner with ID 1 does not exist.")]
        public void Delete_WhenRecordDoesNotExist_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
                controller.Delete(1);
        }

        [TestMethod]
        public void Delete_Normally_ShouldWork()
        {
            Partner createdPartner, queriedPartner;
            IEnumerable<Partner> listedPartners;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new PartnersController(dataLayer))
            {
                createdPartner = controller.Create(TestDataProvider.CreateNewPartner());
                queriedPartner = controller.Get(createdPartner.Id);
                controller.Delete(createdPartner.Id);
                listedPartners = controller.List();
            }

            Assert.AreEqual(createdPartner.Id, queriedPartner.Id, "Unexpected ID.");
            Assert.AreEqual(false, listedPartners.Any(), "There should be no partners returned.");
        }

        [TestMethod]
        [ExpectedException(typeof(RecordDoesNotExistException), "Partner with ID 0 does not exist.")]
        public void Get_WhenRecordDoesNotExist_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new PartnersController(dataLayer))
                controller.Get(0);
        }

        [TestMethod]
        public void Get_Normally_ShouldReturn_PartnerWithId()
        {
            Partner createdPartner, queriedPartner;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new PartnersController(dataLayer))
            {
                var newPartner = TestDataProvider.CreateNewPartner();
                createdPartner = controller.Create(newPartner);
                queriedPartner = controller.Get(createdPartner.Id);
            }

            Assert.IsTrue(
                queriedPartner.IsEqualTo(createdPartner),
                "The two partners should be equal. "
                    + $"Expected: {createdPartner.Stringify()}, actual: {queriedPartner.Stringify()}.");
        }

        [TestMethod]
        public void List_ShouldReturn_CreatedPartners()
        {
            Partner createdPartner1, createdPartner2;
            IEnumerable<Partner> queriedPartners;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new PartnersController(dataLayer))
            {
                createdPartner1 = controller.Create(TestDataProvider.CreateNewPartner());
                createdPartner2 = controller.Create(TestDataProvider.CreateAnotherNewPartner());
                queriedPartners = controller.List();
            }

            AssertPartnersInList(queriedPartners, createdPartner1, createdPartner2);
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
        public void Update_WhenNameNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new PartnersController(dataLayer))
                controller.Update(new PartnerUpdate(1, null, null));
        }

        [TestMethod]
        [ExpectedException(typeof(RecordDoesNotExistException), "Partner with ID 1 does not exist.")]
        public void Update_WhenRecordDoesNotExist_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new PartnersController(dataLayer))
            {
                var partnerUpdate = new PartnerUpdate(1, "Nowhere Man", "Nowhere Land");
                controller.Update(partnerUpdate);
            }
        }

        [TestMethod]
        public void Update_Normally_ShouldWork()
        {
            Partner createdPartner, updatedPartner;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new PartnersController(dataLayer))
            {
                createdPartner = controller.Create(TestDataProvider.CreateNewPartner());
                controller.Update(new PartnerUpdate(createdPartner.Id, "Eleanor Rigby", "Liverpool"));
                updatedPartner = controller.Get(createdPartner.Id);
            }

            Assert.AreEqual("Eleanor Rigby", updatedPartner.Name);
            Assert.AreEqual("Liverpool", updatedPartner.Address);
        }

        private void AssertPartnersInList(IEnumerable<Partner> partners, params Partner[] createdPartners)
        {
            AssertionHelper.AssertOnlyItemsInList(
                Common.EqualityCheckerExtensions.IsEqualTo,
                Common.StringifyExtensions.Stringify,
                partners,
                createdPartners);
        }
    }
}
