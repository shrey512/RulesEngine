using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using RulesEngineEditor.Models;
using RulesEngine.Models;

namespace RuleEngineSample.Data
{
    public class RulesEngineDbContext : DbContext
    {
        public RulesEngineDbContext()
        {
        }

        public RulesEngineDbContext(DbContextOptions<RulesEngineDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=RulesEngineEditorDB;Integrated Security=true;");
            }
        }

        public DbSet<Demographic> Demographic { get; set; }
        public DbSet<JsonDataModel> JsonDataModels { get; set; }
        public DbSet<ClientModel> ClientTable { get; set; }




        public List<Demographic> GetdemographicEF()
            {
                return Demographic.ToList();
            }

        public List<JsonDataModel> GetjsonDataModels()
        {
            return JsonDataModels.ToList();
        }

        public List<ClientModel> GetClientModels()
        {
            return ClientTable.ToList();
        }
    }

    public class JsonDataModel
    {
        public int? Id { get; set; }
        public string? JsonData { get; set; }

    }

    public class ClientModel
    {
        public int? Id { get; set; }
        public string Client { get; set; }
        public string WorkflowName { get; set; }
    }

}
