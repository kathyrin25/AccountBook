using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AccountBook.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public IUnitOfWork UnitOfWork { get; set; }

        private DbSet<T> _Objectset;
        private DbSet<T> ObjectSet
        {
            get
            {
                if (_Objectset == null)
                {
                    _Objectset = UnitOfWork.Context.Set<T>();
                }
                return _Objectset;
            }
        }
        public Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        public IQueryable<T> LookupAll()
        {
            return ObjectSet;
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> filter)
        {
            return ObjectSet.Where(filter);
        }

        public T GetSingle(Expression<Func<T, bool>> filter)
        {
            return ObjectSet.SingleOrDefault(filter);
        }

        public void Create(T entity)
        {
            ObjectSet.Add(entity);
        }

        public void Remove(T entity)
        {
            ObjectSet.Remove(entity);
        }

        public void Commit()
        {
            UnitOfWork.Save();
        }
    }
}