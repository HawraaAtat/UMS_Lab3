using CourseService.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Persistence
{
    public class PcpDbContextFactory 
        : DesignTimeDbContextFactoryBase<classContext>
    {
        protected override classContext CreateNewInstance(DbContextOptions<classContext> options)
        {
            return new classContext(options);
        }
    }
}
