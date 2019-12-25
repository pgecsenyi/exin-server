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
using System.Linq;
using ExinServer.Data.Abstraction;
using ExinServer.Data.Abstraction.Exceptions;
using ExinServer.Data.Sqlite.EntityExtensions;
using Microsoft.EntityFrameworkCore;
using AbstractEntities = ExinServer.Data.Abstraction.Entities;

namespace ExinServer.Data.Sqlite
{
    public class SqliteDataLayer : IDataLayer, IDisposable
    {
        private static object syncObject = new object();
        private DatabaseContext context;

        public SqliteDataLayer(SqliteDatabaseConfiguration databaseConfiguration)
        {
            context = new DatabaseContext(databaseConfiguration);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public AbstractEntities.Category CreateCategory(AbstractEntities.NewCategory newCategory)
        {
            if (newCategory == null)
                throw new ArgumentNullException("newCategory");

            lock (syncObject)
            {
                if (context.Categories.Any(c => string.Compare(c.Name, newCategory.Name, true) == 0))
                    throw new RecordAlreadyExistsException($"Category with name \"{newCategory.Name}\" already exists.");

                var dao = newCategory.ToDao();
                context.Categories.Add(dao);
                context.SaveChanges();

                return dao.ToAbstract();
            }
        }

        public AbstractEntities.Currency CreateCurrency(AbstractEntities.NewCurrency newCurrency)
        {
            if (newCurrency == null)
                throw new ArgumentNullException("newCurrency");

            lock (syncObject)
            {
                if (context.Currencies.Any(c => string.Compare(c.Code, newCurrency.Code, true) == 0))
                    throw new RecordAlreadyExistsException($"Currency with name \"{newCurrency.Code}\" already exists.");

                var dao = newCurrency.ToDao();
                context.Currencies.Add(dao);
                context.SaveChanges();

                return dao.ToAbstract();
            }
        }

        public AbstractEntities.Partner CreatePartner(AbstractEntities.NewPartner newPartner)
        {
            if (newPartner == null)
                throw new ArgumentNullException("newPartner");

            lock (syncObject)
            {
                if (context.Partners.Any(p => string.Compare(p.Name, newPartner.Name, true) == 0))
                    throw new RecordAlreadyExistsException($"Partner with name \"{newPartner.Name}\" already exists.");

                var dao = newPartner.ToDao();
                context.Partners.Add(dao);
                context.SaveChanges();

                return dao.ToAbstract();
            }
        }

        public AbstractEntities.Transfer CreateTransfer(AbstractEntities.NewTransfer newTransfer)
        {
            if (newTransfer == null)
                throw new ArgumentNullException("newTransfer");

            lock (syncObject)
            {
                var category = context.Categories.FirstOrDefault(c => c.Id == newTransfer.CategoryId);
                if (category == null)
                    throw new RecordDoesNotExistException($"Category with ID {newTransfer.CategoryId} does not exist.");
                var partner = context.Partners.FirstOrDefault(c => c.Id == newTransfer.PartnerId);
                if (partner == null)
                    throw new RecordDoesNotExistException($"Partner with ID {newTransfer.PartnerId} does not exist.");
                var currency = context.Currencies.FirstOrDefault(c => c.Id == newTransfer.CurrencyId);
                if (currency == null)
                    throw new RecordDoesNotExistException($"Currency with ID {newTransfer.CurrencyId} does not exist.");

                var dao = newTransfer.ToDao();
                dao.Category = category;
                dao.Currency = currency;
                dao.Partner = partner;

                context.Transfers.Add(dao);
                context.SaveChanges();

                return dao.ToAbstract();
            }
        }

        public void DeleteCategory(int categoryId)
        {
            lock (syncObject)
            {
                var record = context.Categories.SingleOrDefault(c => c.Id == categoryId);
                if (record == null)
                    throw new RecordDoesNotExistException($"Category with ID {categoryId} does not exist.");

                context.Remove(record);
                context.SaveChanges();
            }
        }

        public void DeleteCurrency(int currencyId)
        {
            lock (syncObject)
            {
                var record = context.Currencies.SingleOrDefault(c => c.Id == currencyId);
                if (record == null)
                    throw new RecordDoesNotExistException($"Currency with ID {currencyId} does not exist.");

                context.Remove(record);
                context.SaveChanges();
            }
        }

        public void DeletePartner(int partnerId)
        {
            lock (syncObject)
            {
                var record = context.Partners.SingleOrDefault(p => p.Id == partnerId);
                if (record == null)
                    throw new RecordDoesNotExistException($"Partner with ID {partnerId} does not exist.");

                context.Remove(record);
                context.SaveChanges();
            }
        }

        public void DeleteTransfer(int transferId)
        {
            lock (syncObject)
            {
                var record = context.Transfers.SingleOrDefault(t => t.Id == transferId);
                if (record == null)
                    throw new RecordDoesNotExistException($"Transfer with ID {transferId} does not exist.");

                context.Remove(record);
                context.SaveChanges();
            }
        }

        public void EnsureCreated()
        {
            context.Database.EnsureCreated();
        }

        public AbstractEntities.Category GetCategory(int categoryId)
        {
            return QueryHelper.GetCategory(context, categoryId).ToAbstract();
        }

        public AbstractEntities.Currency GetCurrency(int currencyId)
        {
            return QueryHelper.GetCurrency(context, currencyId).ToAbstract();
        }

        public AbstractEntities.Partner GetPartner(int partnerId)
        {
            return QueryHelper.GetPartner(context, partnerId).ToAbstract();
        }

        public AbstractEntities.Transfer GetTransfer(int transferId)
        {
            return QueryHelper.GetTransfer(context, transferId).ToAbstract();
        }

        public AbstractEntities.Category[] ListCategories()
        {
            return context.Categories.ToAbstract().ToArray();
        }

        public AbstractEntities.Currency[] ListCurrencies()
        {
            return context.Currencies.ToAbstract().ToArray();
        }

        public AbstractEntities.Partner[] ListPartners()
        {
            return context.Partners.ToAbstract().ToArray();
        }

        public AbstractEntities.Transfer[] ListTransfers()
        {
            return context.Transfers
                .Include(t => t.Category)
                .Include(t => t.Partner)
                .Include(t => t.Currency)
                .Include(t => t.Items)
                .ToAbstract()
                .ToArray();
        }

        public AbstractEntities.Category UpdateCategory(AbstractEntities.CategoryUpdate categoryUpdate)
        {
            if (categoryUpdate == null)
                throw new ArgumentNullException("categoryUpdate");

            lock (syncObject)
            {
                var record = context.Categories.SingleOrDefault(c => c.Id == categoryUpdate.Id);
                if (record == null)
                    throw new RecordDoesNotExistException($"Category with ID {categoryUpdate.Id} does not exist.");

                record.Update(categoryUpdate);

                context.SaveChanges();

                return record.ToAbstract();
            }
        }

        public AbstractEntities.Currency UpdateCurrency(AbstractEntities.CurrencyUpdate currencyUpdate)
        {
            if (currencyUpdate == null)
                throw new ArgumentNullException("currencyUpdate");

            lock (syncObject)
            {
                var record = context.Currencies.SingleOrDefault(c => c.Id == currencyUpdate.Id);
                if (record == null)
                    throw new RecordDoesNotExistException($"Currency with ID {currencyUpdate.Id} does not exist.");

                record.Update(currencyUpdate);

                context.SaveChanges();

                return record.ToAbstract();
            }
        }

        public AbstractEntities.Partner UpdatePartner(AbstractEntities.PartnerUpdate partnerUpdate)
        {
            if (partnerUpdate == null)
                throw new ArgumentNullException("partnerUpdate");

            lock (syncObject)
            {
                var record = context.Partners.SingleOrDefault(p => p.Id == partnerUpdate.Id);
                if (record == null)
                    throw new RecordDoesNotExistException($"Partner with ID {partnerUpdate.Id} does not exist.");

                record.Update(partnerUpdate);

                context.SaveChanges();

                return record.ToAbstract();
            }
        }

        public AbstractEntities.Transfer UpdateTransfer(AbstractEntities.TransferUpdate transferUpdate)
        {
            if (transferUpdate == null)
                throw new ArgumentNullException("transferUpdate");

            lock (syncObject)
            {
                var updater = new TransferUpdater(context);

                return updater.UpdateTransfer(transferUpdate);
            }
        }
    }
}
