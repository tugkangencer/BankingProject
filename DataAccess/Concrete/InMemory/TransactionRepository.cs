using Entities.Concrete;
using DataAccess.Abstract;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace DataAccess.Concrete.InMemory
{
    public class TransactionRepository : ITransactionRepository
    {
        readonly List<Transaction> _transactions;

        public TransactionRepository()
        {
            _transactions = new List<Transaction> { };
        }

        public void Create(Transaction transaction)
        {
            _transactions.Add(transaction);
        }

        public List<Transaction> Get(Expression<Func<Transaction, bool>> expression = null)
        {
            return _transactions;
        }
    }
}
