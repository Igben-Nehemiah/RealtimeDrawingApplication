using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int Id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> Predicate);

        void Add(TEntity entity);
        void Remove(TEntity entity);
    }
}
