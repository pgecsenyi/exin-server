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
    public class CurrenciesController : Controller
    {
        private IDataLayer dataLayer;

        public CurrenciesController(IDataLayer dataLayer)
        {
            this.dataLayer = dataLayer;
        }

        // PUT api/currencies
        [HttpPut]
        public Currency Create([FromBody]NewCurrency newCurrency)
        {
            if (newCurrency == null)
                throw new InvalidRequestArgumentException("The currency cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(newCurrency.Code))
                throw new InvalidRequestArgumentException("Currency code cannot be null or empty.");

            return dataLayer.CreateCurrency(newCurrency.ToAbstract()).ToWeb();
        }

        // DELETE api/currencies/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            dataLayer.DeleteCurrency(id);
        }

        // GET api/currencies/5
        [HttpGet("{id}")]
        public Currency Get(int id)
        {
            return dataLayer.GetCurrency(id).ToWeb();
        }

        // GET api/currencies
        [HttpGet]
        public IEnumerable<Currency> List()
        {
            return dataLayer.ListCurrencies().ToWeb();
        }

        // POST api/currencies
        [HttpPost]
        public Currency Update([FromBody]CurrencyUpdate currencyUpdate)
        {
            if (currencyUpdate == null)
                throw new InvalidRequestArgumentException("The currency cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(currencyUpdate.Code))
                throw new InvalidRequestArgumentException("Currency code cannot be null or empty.");

            return dataLayer.UpdateCurrency(currencyUpdate.ToAbstract()).ToWeb();
        }
    }
}
