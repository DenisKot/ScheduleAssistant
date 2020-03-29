using Microsoft.EntityFrameworkCore;

namespace ScheduleAssistant.Data.EntityFramework
{
    public class EfConfiguration
    {
        public static void ConfigureEf(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Window>().Property(x => x.Name).IsRequired();
        }
    }
}