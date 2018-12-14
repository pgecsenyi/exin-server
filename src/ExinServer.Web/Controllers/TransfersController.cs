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
    [Route("api/[controller]")]
    public class TransfersController : Controller
    {
        private IDataLayer dataLayer;

        public TransfersController(IDataLayer dataLayer)
        {
            this.dataLayer = dataLayer;
        }

        // POST api/transfers
        [HttpPost]
        public IActionResult Create([FromBody]NewTransfer newTransfer)
        {
            if (newTransfer == null)
                throw new InvalidRequestArgumentException("The transfer cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(newTransfer.Title))
                throw new InvalidRequestArgumentException("Transfer title cannot be null or empty.");

            var createdTransfer = dataLayer.CreateTransfer(newTransfer.ToAbstract());

            return CreatedAtAction("Get", new { id = createdTransfer.Id }, createdTransfer.ToWeb());
        }

        // DELETE api/transfers/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            dataLayer.DeleteTransfer(id);

            return NoContent();
        }

        // GET api/transfers/5
        [HttpGet("{id}")]
        public Transfer Get(int id)
        {
            return dataLayer.GetTransfer(id).ToWeb();
        }

        // GET api/transfers
        [HttpGet]
        public IEnumerable<Transfer> List()
        {
            return dataLayer.ListTransfers().ToWeb();
        }

        // PUT api/currencies
        [HttpPut]
        public IActionResult Update([FromBody]TransferUpdate transferUpdate)
        {
            if (transferUpdate == null)
                throw new InvalidRequestArgumentException("The transfer cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(transferUpdate.Title))
                throw new InvalidRequestArgumentException("Transfer title cannot be null or empty.");

            dataLayer.UpdateTransfer(transferUpdate.ToAbstract());

            return NoContent();
        }
    }
}
