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
    public class TransactionManagerTests
    {
        private Mock<ITransactionRepository> _transactionRepository;
        private List<Transaction> _transactions;

        private Mock<IAccountRepository> _accountRepository;
        private List<Account> _accounts;

        private TransactionValidator _validator;

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
                new Account { AccountNumber = 123, CurrencyCode = "TRY", Balance = 100 },
            };
            _accountRepository.Setup(a => a.Get(null)).Returns(_accounts);

            _transactionRepository = new Mock<ITransactionRepository>();
            _transactions = new List<Transaction>
            {
                new Transaction { SenderAccountNumber = 189, ReceiverAccountNumber = 234, Amount = 5 },
                new Transaction { SenderAccountNumber = 267, ReceiverAccountNumber = 300, Amount = 267.55m },
                new Transaction { SenderAccountNumber = 234, ReceiverAccountNumber = 189, Amount = 10 },
            };
            _transactionRepository.Setup(t => t.Get(null)).Returns(_transactions);

            _validator = new TransactionValidator();
        }

        [Test]
        [TestCase(123, 156, 23.45)]
        public void CreateTransaction_Success(int senderAccountNumber, int receiverAccountNumber, decimal amount)
        {
            ITransactionService transactionService = new TransactionManager(_transactionRepository.Object, _accountRepository.Object);
            var transaction = new Transaction { SenderAccountNumber = senderAccountNumber, ReceiverAccountNumber = receiverAccountNumber, Amount = amount };

            var testValidationResult = _validator.TestValidate(transaction);
            var result = transactionService.Create(transaction);

            testValidationResult.ShouldNotHaveAnyValidationErrors();
            result.IsError.Should().BeFalse();
        }

        [Test]
        [TestCase(123, 146, 23.45)]
        public void CreateTransaction_AccountNotFound_Failure(int senderAccountNumber, int receiverAccountNumber, decimal amount)
        {
            ITransactionService transactionService = new TransactionManager(_transactionRepository.Object, _accountRepository.Object);
            var transaction = new Transaction { SenderAccountNumber = senderAccountNumber, ReceiverAccountNumber = receiverAccountNumber, Amount = amount };

            var result = transactionService.Create(transaction);

            result.IsError.Should().BeTrue();
        }

        [Test]
        [TestCase(123, 189, 23.45)]
        public void CreateTransaction_MismatchedCurrencies_Failure(int senderAccountNumber, int receiverAccountNumber, decimal amount)
        {
            ITransactionService transactionService = new TransactionManager(_transactionRepository.Object, _accountRepository.Object);
            var transaction = new Transaction { SenderAccountNumber = senderAccountNumber, ReceiverAccountNumber = receiverAccountNumber, Amount = amount };

            var result = transactionService.Create(transaction);

            result.IsError.Should().BeTrue();
        }

        [Test]
        [TestCase(123, 156, 123.45)]
        public void CreateTransaction_InsufficientBalance_Failure(int senderAccountNumber, int receiverAccountNumber, decimal amount)
        {
            ITransactionService transactionService = new TransactionManager(_transactionRepository.Object, _accountRepository.Object);
            var transaction = new Transaction { SenderAccountNumber = senderAccountNumber, ReceiverAccountNumber = receiverAccountNumber, Amount = amount };

            var result = transactionService.Create(transaction);

            result.IsError.Should().BeTrue();
        }

        [Test]
        [TestCase(123, 156, 23.455)]
        public void CreateTransaction_BadPrecision_Failure(int senderAccountNumber, int receiverAccountNumber, decimal amount)
        {
            ITransactionService transactionService = new TransactionManager(_transactionRepository.Object, _accountRepository.Object);
            var transaction = new Transaction { SenderAccountNumber = senderAccountNumber, ReceiverAccountNumber = receiverAccountNumber, Amount = amount };

            var testValidationResult = _validator.TestValidate(transaction);

            testValidationResult.ShouldHaveValidationErrorFor(a => a.Amount);
        }

        [Test]
        public void GetTransactions_Success()
        {
            ITransactionService transactionService = new TransactionManager(_transactionRepository.Object, _accountRepository.Object);
            
            var result = transactionService.Get();

            result.IsError.Should().BeFalse();
        }
    }
}
