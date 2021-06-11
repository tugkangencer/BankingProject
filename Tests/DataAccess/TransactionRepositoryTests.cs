using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace Tests.DataAccess
{
    [TestFixture]
    public class TransactionRepositoryTests
    {
        private const int senderAccountNumber = 123;
        private const int receiverAccountNumber = 156;
        private const decimal amount = 23.45m;

        private static Transaction CreateTransaction(
            int senderAccountNumber = senderAccountNumber,
            int receiverAccountNumber = receiverAccountNumber,
            decimal amount = amount)
        {
            return new()
            {
                SenderAccountNumber = senderAccountNumber,
                ReceiverAccountNumber = receiverAccountNumber,
                Amount = amount,
            };
        }

        [Test]
        public void CreateTransaction_Success()
        {
            ITransactionRepository transactionRepository = new TransactionRepository();
            var transaction = CreateTransaction();

            transactionRepository.Create(transaction);

            var result = transactionRepository.Get().Where(t => t.SenderAccountNumber == senderAccountNumber);
            result.Should().Equals(transaction);
        }

        [Test]
        public void GetTransactions_Success()
        {
            ITransactionRepository transactionRepository = new TransactionRepository();

            var result = transactionRepository.Get();

            result.Count.Should().Be(0);
        }
    }
}
