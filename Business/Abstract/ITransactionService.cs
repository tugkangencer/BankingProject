using Core.Utilities.Results;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface ITransactionService
    {
        IDataResult<List<Transaction>> Get();
        IResult Create(Transaction transaction);
    }
}
