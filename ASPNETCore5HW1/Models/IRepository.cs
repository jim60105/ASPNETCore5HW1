using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Repository {
    public interface IRepository<T> where T : class, new()  {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        EntityEntry<T> Create(T entity);
        EntityEntry<T> Update(T entity);
        EntityEntry<T> Delete(T entity);
        int SaveChanges();
        public EntityEntry<T> Entry(T entity);
        public T Reload(T entity);
    }
}