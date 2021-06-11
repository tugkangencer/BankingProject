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
    public class AccountManager : IAccountService
    {
        readonly IAccountRepository _accountRepository;

        public AccountManager(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [ValidationAspect(typeof(AccountValidator))]
        public IResult Create(Account account)
        {
            IResult result = BusinessRules.Run(CheckIfAccountNumberExist(account.AccountNumber));

            if (result != null)
            {
                return result;
            }

            account.CurrencyCode = account.CurrencyCode.ToUpper();

            _accountRepository.Create(account);

            return new SuccessResult(Messages.AccountCreated);
        }

        public IDataResult<List<Account>> Get()
        {
            return new SuccessDataResult<List<Account>>(_accountRepository.Get(), Messages.AccountsListed);
        }

        private IResult CheckIfAccountNumberExist(int accountNumber)
        {
            if (_accountRepository.Get().Any(a => a.AccountNumber == accountNumber))
            {
                return new ErrorResult(Messages.AccountAlreadyExist);
            }

            return new SuccessResult();
        }
    }
}
