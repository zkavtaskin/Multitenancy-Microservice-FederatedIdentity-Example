using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Server.Core.Domain;
using System.Linq.Expressions;

namespace Server.Core.Repository.Adapters.NHibernate
{
    public class NHRepository<TEntity> : IRepository<TEntity>
        where TEntity : IAggregateRoot
    {
        readonly NHUnitOfWork nhUnitOfWork;

        public NHRepository(IUnitOfWork unitOfWork)
        {
            this.nhUnitOfWork = (NHUnitOfWork)unitOfWork;
        }

        public void Add(TEntity entity)
        {
            this.nhUnitOfWork.Session.Save(entity);
        }

        public void Remove(TEntity entity)
        {
            this.nhUnitOfWork.Session.Delete(entity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.nhUnitOfWork.Session.Query<TEntity>();
        }

        public TEntity FindById(Guid id)
        {
            return this.nhUnitOfWork.Session.Query<TEntity>().FirstOrDefault(x => x.Id == id);
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return this.nhUnitOfWork.Session.Query<TEntity>().Where(predicate).FirstOrDefault();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this.nhUnitOfWork.Session.Query<TEntity>().Where(predicate);
        }

        public int Count()
        {
            return this.nhUnitOfWork.Session.Query<TEntity>().Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return this.nhUnitOfWork.Session.Query<TEntity>().Count(predicate);
        }
    }
}