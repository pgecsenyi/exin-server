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
using ExinServer.Data.Sqlite.Entities;
using ExinServer.Data.Sqlite.EntityExtensions;
using Microsoft.EntityFrameworkCore;
using AbstractEntities = ExinServer.Data.Abstraction.Entities;

namespace ExinServer.Data.Sqlite
{
    internal class TransferUpdater
    {
        private Category category;
        private Currency currency;
        private DatabaseContext context;
        private Partner partner;

        public TransferUpdater(DatabaseContext context)
        {
            this.context = context;
        }

        public AbstractEntities.Transfer UpdateTransfer(AbstractEntities.TransferUpdate transferUpdate)
        {
            var record = context.Transfers
                .Include(t => t.Items)
                .SingleOrDefault(c => c.Id == transferUpdate.Id);
            if (record == null)
                throw new RecordDoesNotExistException($"Transfer with ID {transferUpdate.Id} does not exist.");

            RetrieveNavigationProperties(record, transferUpdate);
            record.Update(transferUpdate);
            UpdateItems(record, transferUpdate);

            context.Update(record);
            context.SaveChanges();
            UpdateNavigationProperties(record);

            return record.ToAbstract();
        }

        private void RetrieveNavigationProperties(Transfer record, AbstractEntities.TransferUpdate transferUpdate)
        {
            category = QueryHelper.GetCategory(context, transferUpdate.CategoryId);
            currency = QueryHelper.GetCurrency(context, transferUpdate.CurrencyId);
            partner = QueryHelper.GetPartner(context, transferUpdate.PartnerId);
        }

        private void UpdateItems(Transfer record, AbstractEntities.TransferUpdate transferUpdate)
        {
            if (record.Items == null && transferUpdate.Items != null)
            {
                AddAllItems(record, transferUpdate.Items);
                return;
            }

            if (transferUpdate.Items == null || transferUpdate.Items.Count() <= 0)
            {
                RemoveAllItems(record);
                return;
            }

            var existingItems = new HashSet<AbstractEntities.TransferItemUpdate>();
            var itemsToRemoveFromRecord = new List<TransferItem>();
            UpdateExistingItems(record, transferUpdate.Items, existingItems, itemsToRemoveFromRecord);
            RemoveItems(record, itemsToRemoveFromRecord);
            AddNewItems(record, transferUpdate.Items, existingItems);
        }

        private void AddAllItems(Transfer record, IEnumerable<AbstractEntities.TransferItemUpdate> itemUpdates)
        {
            record.Items = new List<TransferItem>();
            foreach (var item in itemUpdates)
                record.Items.Add(item.ToDao());
        }

        private void RemoveAllItems(Transfer record)
        {
            foreach (var item in record.Items)
                context.Remove(item);

            record.Items.Clear();
        }

        private void UpdateExistingItems(
            Transfer record,
            IEnumerable<AbstractEntities.TransferItemUpdate> itemUpdates,
            HashSet<AbstractEntities.TransferItemUpdate> existingItems,
            List<TransferItem> itemsToRemoveFromRecord)
        {
            foreach (var item in record.Items)
            {
                var source = itemUpdates.SingleOrDefault(i => i.Id == item.Id);
                if (source == null)
                {
                    itemsToRemoveFromRecord.Add(item);
                }
                else
                {
                    item.Update(source);
                    existingItems.Add(source);
                }
            }
        }

        private void RemoveItems(Transfer record, List<TransferItem> itemsToRemove)
        {
            foreach (var item in itemsToRemove)
            {
                record.Items.Remove(item);
                context.Remove(item);
            }
        }

        private void AddNewItems(
            Transfer record,
            AbstractEntities.TransferItemUpdate[] itemUpdates,
            HashSet<AbstractEntities.TransferItemUpdate> existingItems)
        {
            foreach (var item in itemUpdates)
            {
                if (!existingItems.Contains(item))
                    record.Items.Add(item.ToDao());
            }
        }

        private void UpdateNavigationProperties(Transfer record)
        {
            record.Category = category;
            record.Currency = currency;
            record.Partner = partner;
        }
    }
}
