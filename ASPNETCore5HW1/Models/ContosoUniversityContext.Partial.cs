using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

#nullable disable

namespace ASPNETCore5HW1.Models {
    public partial class ContosoUniversityContext : DbContext {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder) {
            // 這不成功
            SavingChanges+= delegate {
                var entities = this.ChangeTracker.Entries();

                foreach (var entry in entities) {
                    if (entry.State == EntityState.Modified) {
                        entry.CurrentValues.SetValues(new { DateModified = DateTime.Now });
                    }
                }
            };
        }

        // override public object Find(Type entityType, params object[] keyValues)
        // {
        //     var data = base.Find(entityType, keyValues);
        //     bool isDeleted = (bool)data.GetType().GetProperty("IsDeleted", BindingFlags.Public).GetValue(data);

        //     return isDeleted ? null : data;
        // }
    }
}

