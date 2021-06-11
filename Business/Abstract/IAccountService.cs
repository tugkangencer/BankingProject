using Core.Utilities.Results;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IAccountService
    {
        IDataResult<List<Account>> Get();
        IResult Create(Account account);
    }
}
