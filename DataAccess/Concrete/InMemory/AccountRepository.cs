using Entities.Concrete;
using DataAccess.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace DataAccess.Concrete.InMemory
{
    public class AccountRepository : IAccountRepository
    {
        readonly List<Account> _accounts;

        public AccountRepository()
        {
            _accounts = new List<Account> { };
        }
        
        public void Create(Account account)
        {
            _accounts.Add(account);
        }

        public List<Account> Get(Expression<Func<Account, bool>> expression = null)
        {
            return _accounts;
        }

        public void UpdateBalance(Transaction transaction)
        {
            Account SenderAccount = _accounts.SingleOrDefault(a => a.AccountNumber == transaction.SenderAccountNumber);
            SenderAccount.Balance -= transaction.Amount;
            Account ReceiverAccount = _accounts.SingleOrDefault(a => a.AccountNumber == transaction.ReceiverAccountNumber);
            ReceiverAccount.Balance += transaction.Amount;
        }
    }
}