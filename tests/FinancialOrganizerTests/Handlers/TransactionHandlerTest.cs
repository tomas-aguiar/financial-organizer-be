using System;
using System.Threading.Tasks;
using FinancialOrganizer.Data;
using FinancialOrganizer.Handlers;
using FinancialOrganizer.Model;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace FinancialOrganizer.Tests.Handlers
{
    [TestClass]
    public class TransactionHandlerTest
    {
        private readonly ITransactionHandler _transactionHandler;
        private readonly DataContext _dataContext;

        public TransactionHandlerTest()
        {
            var configuration = Substitute.For<IConfiguration>();
            _dataContext = new DataContext(configuration, "test-database-transaction-handler");
            _transactionHandler = new TransactionHandler(_dataContext);
        }

        [TestMethod("Updates Account Balance When Account is Valid")]
        public async Task UpdatesAccountBalance()
        {
            const decimal initialAccountBalance = 10.0m;
            const decimal transactionAmount = 5.0m;
            const decimal expectedAccountFinalBalance = initialAccountBalance + transactionAmount;
            var account = new Account
            {
                AccountType = new AccountType
                {
                    Id = 1,
                    Name = "account-type-test"
                },
                AccountTypeId = 1,
                Balance = 10.0m,
                Id = 1,
                Name = "account-test",
                Status = true
            };
            _dataContext.Accounts.Add(account);
            await _dataContext.SaveChangesAsync();
            var transaction = new Transaction
            {
                Account = account,
                AccountId = 1,
                Amount = 5.0m,
                Category = new Category
                {
                    CostType = new CostType
                    {
                        Id = 1,
                        Name = "cost-type-test"
                    },
                    CostTypeId = 1,
                    Id = 1,
                    Name = "category-test"
                },
                CategoryId = 1,
                Description = "description-test",
                Id = 1,
                IsBudget = false,
                TimeStamp = DateTime.Now
            };
            _dataContext.Transactions.Add(transaction);
            await _dataContext.SaveChangesAsync();

            await _transactionHandler.Handle(transaction);

            account.Balance.Should().Be(expectedAccountFinalBalance);
        }

        [TestCleanup]
        public void CleanUpDatabase()
        {
            _dataContext.Dispose();
        }
    }
}