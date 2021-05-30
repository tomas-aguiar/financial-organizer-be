using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialOrganizer.Data;
using FinancialOrganizer.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinancialOrganizer.Handlers
{
    public interface ITransactionHandler
    {
        Task Handle(Transaction transaction);
    }
    public class TransactionHandler : ITransactionHandler
    {
        private readonly DataContext _dataContext;

        public TransactionHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task Handle(Transaction transaction)
        {
            var account =
                (from a in _dataContext.Accounts
                where a.Id == transaction.AccountId
                select a).FirstOrDefault();
            // var account = _dataContext.Accounts.FirstOrDefault(a => a.Id == transaction.AccountId);
            if (account == null) throw new KeyNotFoundException();
            account.Balance += transaction.Amount;
            _dataContext.Accounts.Update(account);
            await _dataContext.SaveChangesAsync();

            var test = from t in _dataContext.Transactions
                where t.CategoryId == 8
                      && t.TimeStamp > new DateTime(2021, 05, 28, 20, 00, 00)
                      && t.TimeStamp < new DateTime(2021, 05, 29, 02, 00, 00)
                select t.Amount;
            var test2 = 0.0m;
            foreach (var value in test)
            {
                test2 += value;
            }
        }
    }
}