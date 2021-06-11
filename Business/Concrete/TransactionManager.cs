using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class TransactionManager : ITransactionService
    {
        readonly ITransactionRepository _transactionRepository;
        readonly IAccountRepository _accountRepository;

        public TransactionManager(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        [ValidationAspect(typeof(TransactionValidator))]
        public IResult Create(Transaction transaction)
        {
            IResult result = BusinessRules.Run(CheckIfAccountNumbersExist(transaction.SenderAccountNumber, transaction.ReceiverAccountNumber));

            if (result == null)
            {
                result = BusinessRules.Run(CheckIfCurrencyCodesMatch(transaction.SenderAccountNumber, transaction.ReceiverAccountNumber),
                                                CheckIfSenderHasFund(transaction.SenderAccountNumber, transaction.Amount));
            }
            
            if (result != null)
            {
                return result;
            }

            _accountRepository.UpdateBalance(transaction);
            _transactionRepository.Create(transaction);

            return new SuccessResult(Messages.TransactionCreated);
        }

        public IDataResult<List<Transaction>> Get()
        {
            return new SuccessDataResult<List<Transaction>>(_transactionRepository.Get(), Messages.TransactionsListed);
        }

        private IResult CheckIfAccountNumbersExist(int senderAccountNumber, int receiverAccountNumber)
        {
            if (!(_accountRepository.Get().Any(a => a.AccountNumber == senderAccountNumber)
               && _accountRepository.Get().Any(a => a.AccountNumber == receiverAccountNumber)))
            {
                return new ErrorResult(Messages.AccountNotFound);
            }

            return new SuccessResult();
        }

        private IResult CheckIfCurrencyCodesMatch(int senderAccountNumber, int receiverAccountNumber)
        {
            if (!(_accountRepository.Get().SingleOrDefault(a => a.AccountNumber == senderAccountNumber).CurrencyCode
               == _accountRepository.Get().SingleOrDefault(a => a.AccountNumber == receiverAccountNumber).CurrencyCode))
            {
                return new ErrorResult(Messages.MismatchedCurrencies);
            }

            return new SuccessResult();
        }

        private IResult CheckIfSenderHasFund(int senderAccountNumber, decimal amount)
        {
            if (_accountRepository.Get().SingleOrDefault(a => a.AccountNumber == senderAccountNumber).Balance < amount)
            {
                return new ErrorResult(Messages.InsufficientBalance);
            }

            return new SuccessResult();
        }
    }
}
