using Server.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Server.Core.Repository
{
    public interface IRepository<TEntity> where TEntity : IAggregateRoot
    {
        void Add(TEntity entity);
        void Remove(TEntity entity);
        IEnumerable<TEntity> GetAll();
        TEntity FindById(Guid id);

        TEntity FindOne(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        int Count();

        int Count(Expression<Func<TEntity, bool>> predicate);
    }
}