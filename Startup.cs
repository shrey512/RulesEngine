using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RuleEngineSample.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngineSample
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        private readonly RulesEngineDbContext _dbcontext;

        public Startup(IConfiguration configuration, RulesEngineDbContext dbContext)
        {
            Configuration = configuration;
            _dbcontext = dbContext;
        }

        public void ConfigurationServices(IServiceCollection services)
        {
            services.AddDbContext<RulesEngineDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("RulesEngineEditorDB"));
            });
        }

        
    }
}
