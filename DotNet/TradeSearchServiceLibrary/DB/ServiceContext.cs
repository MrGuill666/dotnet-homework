using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSearchServiceLibrary.Model;

namespace TradeSearchServiceLibrary
{
    public class ServiceContext:DbContext
    {
        public DbSet<Bot> Bots { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemInBot> ItemsInBots { get; set; }

        public ServiceContext(string connstring):base(connstring){
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Properties().Where(p=>p.Name.Contains("ID")).Configure(p=>p.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity));
        }

        public ServiceContext() {
            Database.SetInitializer<ServiceContext>(null);
        }
    }
}
