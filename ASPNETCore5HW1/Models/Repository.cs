using System;
using System.Linq;
using System.Linq.Expressions;
using ASPNETCore5HW1.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Repository {
    public class Repository<T> : IRepository<T> where T : class, new() {
        protected ContosoUniversityContext RepositoryContext { get; set; }

        public Repository(ContosoUniversityContext repositoryContext) => this.RepositoryContext = repositoryContext;

        public virtual IQueryable<T> FindAll() => this.RepositoryContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => FindAll().Where(expression);

        public EntityEntry<T> Create(T entity) {
            EntityEntry<T> entry = this.RepositoryContext.Set<T>().Add(entity);
            entry.CurrentValues.SetValues(new { DateModified = DateTime.Now });
            return entry;
        }

        public EntityEntry<T> Update(T entity) {
            EntityEntry<T> entry = this.RepositoryContext.Set<T>().Update(entity);
            entry.CurrentValues.SetValues(new { DateModified = DateTime.Now });
            return entry;
        }

        public EntityEntry<T> Delete(T entity) => this.RepositoryContext.Set<T>().Remove(entity);
        public int SaveChanges() => RepositoryContext.SaveChanges();
        public EntityEntry<T> Entry(T entity) => RepositoryContext.Entry(entity);
        public T Reload(T entity) {
            EntityEntry<T> entry = Entry(entity);
            entry.Reload();
            return entry.Entity;
        }
    }
}
