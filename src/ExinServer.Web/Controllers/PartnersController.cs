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
    public class PartnersController : Controller
    {
        private IDataLayer dataLayer;

        public PartnersController(IDataLayer dataLayer)
        {
            this.dataLayer = dataLayer;
        }

        // POST api/partners
        [HttpPost]
        public IActionResult Create([FromBody]NewPartner newPartner)
        {
            if (newPartner == null)
                throw new InvalidRequestArgumentException("The partner cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(newPartner.Name))
                throw new InvalidRequestArgumentException("Partner name cannot be null or empty.");

            var createdPartner = dataLayer.CreatePartner(newPartner.ToAbstract());

            return CreatedAtAction("Get", new { id = createdPartner.Id }, createdPartner.ToWeb());
        }

        // DELETE api/partners/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            dataLayer.DeletePartner(id);

            return NoContent();
        }

        // GET api/partners/5
        [HttpGet("{id}")]
        public Partner Get(int id)
        {
            return dataLayer.GetPartner(id).ToWeb();
        }

        // GET api/partners
        [HttpGet]
        public IEnumerable<Partner> List()
        {
            return dataLayer.ListPartners().ToWeb();
        }

        // PUT api/partners
        [HttpPut]
        public IActionResult Update([FromBody]PartnerUpdate partnerUpdate)
        {
            if (partnerUpdate == null)
                throw new InvalidRequestArgumentException("The partner cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(partnerUpdate.Name))
                throw new InvalidRequestArgumentException("Partner name cannot be null or empty.");

            dataLayer.UpdatePartner(partnerUpdate.ToAbstract());

            return NoContent();
        }
    }
}
