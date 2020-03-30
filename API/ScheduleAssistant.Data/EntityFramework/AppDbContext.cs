using Microsoft.EntityFrameworkCore;
using ScheduleAssistant.Domain.Windows;

namespace ScheduleAssistant.Data.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            EfConfiguration.ConfigureEf(modelBuilder);
        }

        public virtual DbSet<ExpressWindow> ExpressWindows { get; set; }
        public virtual DbSet<UsualWindow> UsualWindows { get; set; }
    }
}
