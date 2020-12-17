using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ASPNETCore5HW1.Models
{
    public partial class ContosoUniversityContext : DbContext
    {
        public new async Task<int> SaveChangesAsync( CancellationToken cancellationToken = default){
            var entities = this.ChangeTracker.Entries();
            foreach (var entry in entities)
            {
                if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
                {
                    entry.CurrentValues.SetValues(new { DateModified = DateTime.Now });
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        // override public object Find(Type entityType, params object[] keyValues)
        // {
        //     var data = base.Find(entityType, keyValues);
        //     bool isDeleted = (bool)data.GetType().GetProperty("IsDeleted", BindingFlags.Public).GetValue(data);

        //     return isDeleted ? null : data;
        // }
    }
}

