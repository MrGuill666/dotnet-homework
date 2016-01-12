using System;
using System.Collections.Generic;
using System.Configuration;
using TradeSearchServiceLibrary.Model;
using TradeSearchServiceLibrary.TradeSearch.Base;
using TradeSearchServiceLibrary.TradeSearch.Bot;

namespace TradeSearchServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TradeSearchServiceLibrary" in both code and config file together.
    public class TradeSearchService : ITradeSearchService
    {

        private List<TradeBot> bots = new List<TradeBot>();

        public TradeSearchService()
        {
            bots.Add(new ScrapTFBot("http://scrap.tf/items"));
            bots.Add(new TradeTFBot("http://www.trade.tf/mybots/index"));
            bots.Add(new WarehouseBot("http://www.tf2wh.com/priceguide"));
        }
        public List<string> RefreshDatabase()
        {

            using (ServiceContext ctx = new ServiceContext("name=Database"))
            {

                ctx.Database.ExecuteSqlCommand("delete from ItemInBots");
                ctx.Database.ExecuteSqlCommand("delete from Bots");
                ctx.Database.ExecuteSqlCommand("delete from Items");

                ItemHelper.CTX = ctx;
                ItemHelper.KEY_PRICE = 18.00f;
                ItemHelper.Initialize();

                List<string> failedBotNames = new List<string>();

                foreach (var b in bots)
                {
                    try
                    {
                        b.Refresh();
                    }
                    catch (Exception e)
                    {
                        failedBotNames.Add(b.getName());
                    }
                }
                //bots.ForEach(b => b.Refresh());

                Console.WriteLine("Refreshed database");
                ctx.SaveChanges();
                return failedBotNames;
            }
        }


        public List<TradeItem> GetItems()
        {
            List<TradeItem> items = new List<TradeItem>();
            using (ServiceContext ctx = new ServiceContext("name=Database"))
            {
                foreach (ItemInBot i in ctx.ItemsInBots)
                {
                    items.Add(new TradeItem()
                    {
                        BotName = i.Bot.Name,
                        BotURL = i.Bot.URL,
                        Name = i.Item.Name,
                        ID = i.Item.ID,
                        Max = i.Max,
                        Stock = i.Stock,
                        BuyPrice = i.BuyPrice,
                        SellPrice = i.SellPrice
                    });

                }

            }
            return items;
        }
    }
}
