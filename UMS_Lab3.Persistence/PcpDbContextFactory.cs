using Microsoft.EntityFrameworkCore;

namespace UMS_Lab3.Persistence
{
    public class PcpDbContextFactory 
        : DesignTimeDbContextFactoryBase<postgresContext>
    {
        protected override postgresContext CreateNewInstance(DbContextOptions<postgresContext> options)
        {
            return new postgresContext(options);
        }
    }
}
