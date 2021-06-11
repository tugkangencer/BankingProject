using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity
    {
        void Create(T entity);
        List<T> Get(Expression<Func<T, bool>> expression = null);
    }
}
