using System;
using Microsoft.EntityFrameworkCore;

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
    }
}
