using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSearchService.Model;
using TradeSearchService.TradeSearch.Base;
using TradeSearchService.TradeSearch.Bot;

namespace TradeSearchService
{
    class Program
    {
        static readonly string connString = @"Data Source=(localdb)\v11.0;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=30;Encrypt=False; TrustServerCertificate=False; MultipleActiveResultSets=True";
        static void Main(string[] args)
        {
            using (ServiceContext ctx = new ServiceContext(connString))
            {
                ctx.Database.ExecuteSqlCommand("delete from ItemInBots");
                ctx.Database.ExecuteSqlCommand("delete from Bots");
                ctx.Database.ExecuteSqlCommand("delete from Items");
                ItemHelper.CTX = ctx;
                ItemHelper.KEY_PRICE = 18.00f;
                TradeBot bot1 = new ScrapTFBot("https://scrap.tf/items");
                bot1.Refresh();
                

                ctx.SaveChanges();
            }
        }
    }
}

