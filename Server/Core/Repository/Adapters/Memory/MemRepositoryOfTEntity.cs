using Server.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Repository.Adapters
{
    public class MemRepository<TEntity> : IRepository<TEntity>
        where TEntity : IAggregateRoot
    {

        public static List<TEntity> items = new List<TEntity>();

        public void Add(TEntity entity)
        {
            items.Add(entity);
        }

        public void Remove(TEntity entity)
        {
            items.Remove(entity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return items;
        }

        public TEntity FindById(Guid id)
        {
            return items.FirstOrDefault(x => x.Id == id);
        }


        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return items.Where(predicate.Compile()).FirstOrDefault();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return items.Where(predicate.Compile());
        }

        public int Count()
        {
            return items.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return items.Count(predicate.Compile());
        }
    }
}
