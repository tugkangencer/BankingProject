using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace Tests.DataAccess
{
    [TestFixture]
    public class AccountRepositoryTests
    {
        private const int accountNumber = 123;
        private const string currencyCode = "TRY";
        private const decimal balance = 100;

        private static Account CreateAccount(
            int accountNumber = accountNumber,
            string currencyCode = currencyCode,
            decimal balance =  balance)
        {
            return new()
            {
                AccountNumber = accountNumber,
                CurrencyCode = currencyCode,
                Balance = balance,
            };
        }

        [Test]
        public void CreateAccount_Success()
        {
            IAccountRepository accountRepository = new AccountRepository();
            var account = CreateAccount();

            accountRepository.Create(account);

            var result = accountRepository.Get().Where(a => a.AccountNumber == accountNumber);
            result.Should().Equals(account);
        }

        [Test]
        public void GetAccounts_Success()
        {
            IAccountRepository accountRepository = new AccountRepository();

            var result = accountRepository.Get();

            result.Count.Should().Be(0);
        }
    }
}
