using Business.Abstract;
using Business.Concrete;
using Business.ValidationRules.FluentValidation;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests.Business
{
    [TestFixture]
    public class AccountManagerTests
    {
        private Mock<IAccountRepository> _accountRepository;
        private List<Account> _accounts;
        private AccountValidator _validator;

        [SetUp]
        public void Setup()
        {
            _accountRepository = new Mock<IAccountRepository>();
            _accounts = new List<Account>
            {
                new Account { AccountNumber = 156, CurrencyCode = "TRY", Balance = 53.1m },
                new Account { AccountNumber = 234, CurrencyCode = "USD", Balance = 36.23m },
                new Account { AccountNumber = 300, CurrencyCode = "EUR", Balance = 300 },
                new Account { AccountNumber = 267, CurrencyCode = "EUR", Balance = 1232.45m },
                new Account { AccountNumber = 189, CurrencyCode = "USD", Balance = 13 },
            };
            _accountRepository.Setup(a => a.Get(null)).Returns(_accounts);
            _validator = new AccountValidator();
        }

        [Test]
        [TestCase(123, "TRY", 100)]
        public void CreateAccount_Success(int accountNumber, string currencyCode, decimal balance)
        {
            IAccountService accountService = new AccountManager(_accountRepository.Object);
            var account = new Account { AccountNumber = accountNumber, CurrencyCode = currencyCode, Balance = balance };

            var testValidationResult = _validator.TestValidate(account);
            var result = accountService.Create(account);
            
            testValidationResult.ShouldNotHaveAnyValidationErrors();
            result.IsError.Should().BeFalse();
        }

        [Test]
        [TestCase(156, "TRY", 100)]
        public void CreateAccount_AccountAlreadyExist_Failure(int accountNumber, string currencyCode, decimal balance)
        {
            IAccountService accountService = new AccountManager(_accountRepository.Object);
            var account = new Account { AccountNumber = accountNumber, CurrencyCode = currencyCode, Balance = balance };

            var result = accountService.Create(account);

            result.IsError.Should().BeTrue();
        }

        [Test]
        [TestCase(123, "GBP", 100)]
        public void CreateAccount_InvalidCurrency_Failure(int accountNumber, string currencyCode, decimal balance)
        {
            IAccountService accountService = new AccountManager(_accountRepository.Object);
            var account = new Account { AccountNumber = accountNumber, CurrencyCode = currencyCode, Balance = balance };

            var testValidationResult = _validator.TestValidate(account);

            testValidationResult.ShouldHaveValidationErrorFor(a => a.CurrencyCode);
        }

        [Test]
        [TestCase(123, "TRY", 100.333)]
        public void CreateAccount_BadPrecision_Failure(int accountNumber, string currencyCode, decimal balance)
        {
            IAccountService accountService = new AccountManager(_accountRepository.Object);
            var account = new Account { AccountNumber = accountNumber, CurrencyCode = currencyCode, Balance = balance };

            var testValidationResult = _validator.TestValidate(account);

            testValidationResult.ShouldHaveValidationErrorFor(a => a.Balance);
        }

        [Test]
        public void GetAccounts_Success()
        {
            IAccountService accountService = new AccountManager(_accountRepository.Object);

            var result = accountService.Get();

            result.IsError.Should().BeFalse();
        }
    }
}
