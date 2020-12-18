using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ASPNETCore5HW1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Repository {
    public class IsDeletedRepository<T> : Repository<T>, IRepository<T> where T : class, new() {
        public IsDeletedRepository(ContosoUniversityContext repositoryContext) : base(repositoryContext) {
        }

        // IsDeleted是Course、Department、Preson專用邏輯，所以寫在這裡
        public override IQueryable<T> FindAll() =>
            this.RepositoryContext.Set<T>()
            .ToList()
            .Where(d => (bool)typeof(T).GetProperty("IsDeleted").GetValue(d) == false)
            .AsQueryable();

        public new EntityEntry<T> Delete(T entity) {
            var entry = RepositoryContext.Entry(entity);
            entry.CurrentValues.SetValues(new { IsDeleted = true });
            entry.State = EntityState.Modified;
            return entry;
        }
    }
}
