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
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExinServer.Test.ControllerTests
{
    [TestClass]
    public class CategoriesControllerTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Create_WhenParamNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
                controller.Create(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Create_WhenNameNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
                controller.Create(new NewCategory(null));
        }

        [TestMethod]
        [ExpectedException(typeof(RecordAlreadyExistsException), "Category with name \"Food\" already exists.")]
        public void Create_WhenRecordExists_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
            {
                var newCategory = TestDataProvider.CreateNewCategory();
                controller.Create(newCategory);
                controller.Create(newCategory);
            }
        }

        [TestMethod]
        public void Create_Normally_ShouldReturn_Category()
        {
            var newCategory = TestDataProvider.CreateNewCategory();
            CreatedAtActionResult actionResult;
            Category category;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
            {
                actionResult = (CreatedAtActionResult)controller.Create(newCategory);
                category = (Category)actionResult.Value;
            }

            Assert.AreEqual(actionResult.ActionName, "Get");
            Assert.IsTrue(
                category.Id > 0,
                $"Category ID is expected to greater than 0. Actual: {category.Id}.");
            Assert.AreEqual(
                newCategory.Name,
                category.Name,
                $"Invalid category name. Expected: \"{newCategory.Name}\", actual: \"{category.Name}\".");
        }

        [TestMethod]
        [ExpectedException(typeof(RecordDoesNotExistException), "Category with ID 1 does not exist.")]
        public void Delete_WhenRecordDoesNotExist_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
                controller.Delete(1);
        }

        [TestMethod]
        public void Delete_Normally_ShouldWork()
        {
            Category createdCategory, queriedCategory;
            IEnumerable<Category> listedCategories;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
            {
                createdCategory = controller.CreateCategory(TestDataProvider.CreateNewCategory());
                queriedCategory = controller.Get(createdCategory.Id);
                controller.Delete(createdCategory.Id);
                listedCategories = controller.List();
            }

            Assert.AreEqual(createdCategory.Id, queriedCategory.Id, "Unexpected ID.");
            Assert.AreEqual(false, listedCategories.Any(), "There should be no categories returned.");
        }

        [TestMethod]
        [ExpectedException(typeof(RecordDoesNotExistException), "Category with ID 0 does not exist.")]
        public void Get_WhenRecordDoesNotExist_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
                controller.Get(0);
        }

        [TestMethod]
        public void Get_Normally_ShouldReturn_CategoryWithId()
        {
            Category createdCategory, queriedCategory;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
            {
                createdCategory = controller.CreateCategory(TestDataProvider.CreateNewCategory());
                queriedCategory = controller.Get(createdCategory.Id);
            }

            Assert.IsTrue(
                queriedCategory.IsEqualTo(createdCategory),
                "The two categories should be equal. "
                    + $"Expected: {createdCategory.Stringify()}, actual: {queriedCategory.Stringify()}.");
        }

        [TestMethod]
        public void List_ShouldReturn_CreatedCategories()
        {
            Category createdCategory1, createdCategory2;
            IEnumerable<Category> queriedCategories;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
            {
                createdCategory1 = controller.CreateCategory(TestDataProvider.CreateNewCategory());
                createdCategory2 = controller.CreateCategory(TestDataProvider.CreateAnotherNewCategory());
                queriedCategories = controller.List();
            }

            AssertCategoriesInList(queriedCategories, createdCategory1, createdCategory2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Update_WhenNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
                controller.Update(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestArgumentException))]
        public void Update_WhenNameNull_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
                controller.Update(new CategoryUpdate(1, null));
        }

        [TestMethod]
        [ExpectedException(typeof(RecordDoesNotExistException), "Category with ID 1 does not exist.")]
        public void Update_WhenRecordDoesNotExist_ShouldThrow()
        {
            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
            {
                var categoryUpdate = new CategoryUpdate(1, "Cinema");
                controller.Update(categoryUpdate);
            }
        }

        [TestMethod]
        public void Update_Normally_ShouldWork()
        {
            Category createdCategory, updatedCategory;

            using (var dataLayer = DataLayerHelper.CreateDataLayer())
            using (var controller = new CategoriesController(dataLayer))
            {
                createdCategory = controller.CreateCategory(TestDataProvider.CreateNewCategory());
                controller.Update(new CategoryUpdate(createdCategory.Id, "Cinema"));
                updatedCategory = controller.Get(createdCategory.Id);
            }

            Assert.AreEqual("Cinema", updatedCategory.Name);
        }

        private void AssertCategoriesInList(IEnumerable<Category> categories, params Category[] createdCategories)
        {
            AssertionHelper.AssertOnlyItemsInList(
                Common.EqualityCheckerExtensions.IsEqualTo,
                Common.StringifyExtensions.Stringify,
                categories,
                createdCategories);
        }
    }
}
