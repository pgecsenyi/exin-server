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
using ExinServer.Data.Abstraction;
using ExinServer.Web.Entities;
using ExinServer.Web.EntityExtensions;
using ExinServer.Web.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ExinServer.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private IDataLayer dataLayer;

        public CategoriesController(IDataLayer dataLayer)
        {
            this.dataLayer = dataLayer;
        }

        // POST api/categories
        [HttpPost]
        public IActionResult Create([FromBody]NewCategory newCategory)
        {
            if (newCategory == null)
                throw new InvalidRequestArgumentException("The category cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(newCategory.Name))
                throw new InvalidRequestArgumentException("Category name cannot be null or empty.");

            var createdCategory = dataLayer.CreateCategory(newCategory.ToAbstract());

            return CreatedAtAction("Get", new { id = createdCategory.Id }, createdCategory.ToWeb());
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            dataLayer.DeleteCategory(id);

            return NoContent();
        }

        // GET api/categories/5
        [HttpGet("{id}")]
        public Category Get(int id)
        {
            return dataLayer.GetCategory(id).ToWeb();
        }

        // GET api/categories
        [HttpGet]
        public IEnumerable<Category> List()
        {
            return dataLayer.ListCategories().ToWeb();
        }

        // PUT api/categories
        [HttpPut]
        public IActionResult Update([FromBody]CategoryUpdate updatedCategory)
        {
            if (updatedCategory == null)
                throw new InvalidRequestArgumentException("The category cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(updatedCategory.Name))
                throw new InvalidRequestArgumentException("Category name cannot be null or empty.");

            dataLayer.UpdateCategory(updatedCategory.ToAbstract());

            return NoContent();
        }
    }
}
