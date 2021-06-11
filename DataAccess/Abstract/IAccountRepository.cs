using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IAccountRepository : IEntityRepository<Account>
    {
        void UpdateBalance(Transaction transaction);
    }
}