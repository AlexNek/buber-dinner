using BuberDinner.Infrastructure.Persistence;
using BuberDinner.Infrastructure.Persistence.Interceptors;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BuberDinner.Infrastructure
{
    public class BuberDinnerContextFactory : IDesignTimeDbContextFactory<BuberDinnerDbContext>
    {
        public BuberDinnerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BuberDinnerDbContext>();
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=BuberDinner;TrustServerCertificate=True;Integrated Security=true");
            PublishDomainEventsInterceptor eventsInterceptor = new PublishDomainEventsInterceptor(null);
            return new BuberDinnerDbContext(optionsBuilder.Options, eventsInterceptor);
        }
    }
}
