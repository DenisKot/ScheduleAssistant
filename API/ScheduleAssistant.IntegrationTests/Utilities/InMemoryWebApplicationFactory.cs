using System.Linq;
using ScheduleAssistant.Data.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace ScheduleAssistant.IntegrationTests.Utilities
{
    public class InMemoryWebApplicationFactory : APIWebApplicationFactory
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var appDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (appDescriptor != null)
                    services.Remove(appDescriptor);
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("InMemoryAppDbContext")
                        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            });
        }
    }
}
