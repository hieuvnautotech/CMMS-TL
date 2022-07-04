using Furion;
using Microsoft.Extensions.DependencyInjection;

namespace CCMS.EntityFramework.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseAccessor(options =>
            {
                options.AddDbPool<DefaultDbContext>();
            }, "CCMS.Database.Migrations");
        }
    }
}