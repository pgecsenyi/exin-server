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

using ExinServer.Web.Controllers;
using ExinServer.Web.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExinServer.Test.Common
{
    internal static class ControllerHelper
    {
        public static Category CreateCategory(this CategoriesController controller, NewCategory newCategory)
        {
            var actionResult = (CreatedAtActionResult)controller.Create(newCategory);

            return (Category)actionResult.Value;
        }

        public static Currency CreateCurrency(this CurrenciesController controller, NewCurrency newCurrency)
        {
            var actionResult = (CreatedAtActionResult)controller.Create(newCurrency);

            return (Currency)actionResult.Value;
        }

        public static Partner CreatePartner(this PartnersController controller, NewPartner newPartner)
        {
            var actionResult = (CreatedAtActionResult)controller.Create(newPartner);

            return (Partner)actionResult.Value;
        }

        public static Transfer CreateTransfer(this TransfersController controller, NewTransfer newTransfer)
        {
            var actionResult = (CreatedAtActionResult)controller.Create(newTransfer);

            return (Transfer)actionResult.Value;
        }
    }
}
