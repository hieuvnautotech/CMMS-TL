using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;

namespace CCMS.EntityFramework.Core
{
    [AppDbContext("CCMS", DbProvider.Sqlite)]
    public class DefaultDbContext : AppDbContext<DefaultDbContext>
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
        }
    }
}