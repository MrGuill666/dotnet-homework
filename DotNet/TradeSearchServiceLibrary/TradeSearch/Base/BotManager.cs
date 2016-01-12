using System;
using System.Collections.Generic;
using System.IO;
using TradeSearchServiceLibrary.Model;

namespace TradeSearchServiceLibrary.TradeSearch.Base
{
    class BotManager
    {
        private Dictionary<string, List<ItemInBot>> items = new Dictionary<string, List<ItemInBot>>();
        private List<TradeBot> bots = new List<TradeBot>();

        public List<List<ItemInBot>> Search(String name)
        {
            List<List<ItemInBot>> ret = new List<List<ItemInBot>>();
            name = name.ToLower();
            foreach (string s in items.Keys)
            {
                if (s.ToLower().Contains(name))
                {
                    ret.Add(items[s]);
                }
            }

            return ret;
        }


        public void AddBot(TradeBot bot)
        {
            bots.Add(bot);
        }

        /*
    public void Compare() {
        ICollection<string> keys = new HashSet<string>();


        foreach (TradeBot tb in bots) {
            keys.Add(tb.getItemNames());
        }

        foreach (string name in keys) {
            ItemInBot toBuy = null, toSell = null;
            foreach (TradeBot bot in bots) {
                ItemInBot item = bot.getItem(name);
                if (item != null) {
                    if (toBuy == null) toBuy = item;
                    if (toSell == null) toSell = item;
                    if (toBuy.SellPrice > item.SellPrice)
                        toBuy = item;
                    if (toSell.BuyPrice < item.BuyPrice)
                        toSell = item;

                }
            }

            if (toBuy != null) {
                toBuy.buy = true;
                toSell.sell = true;

                //    System.out.println(toBuy + " " + toSell);
                if (toSell.BuyPrice > 0)
//                if ((toSell.buyPrice/(float)toBuy.sellPrice)>=1.1)
                    if ((toSell.BuyPrice - toBuy.SellPrice) >= 20)
                        if (toBuy.Stock > 0 && toSell.Stock < toSell.Max)
                            if (toSell.Bot.Name != "WareHouse")

                                //                      if(toBuy.sellPrice<=2400)
                                if (toBuy.Bot.Name != "Wasda" || name.Contains("Lime"))
                                    //System.out.println("Buy " + toBuy.stock + " " + name + " at " + toBuy.origin + " for " + toBuy.sellPrice / 100.00 + " and sell at " + toSell.origin + " for " + toSell.buyPrice / 100.00);

            }
        }
    }*/

        public void Refresh()
        {
            items.Clear();
            foreach (TradeBot b in bots)
            {
                try
                {
                    b.Refresh();
                }
                catch (IOException ioe)
                {
                    //TODO
                }

                HashSet<string> keys = new HashSet<string>();
                keys.UnionWith(b.getItemNames());
                foreach (string k in keys)
                {
                    if (items[k] == null)
                        items.Add(k, new List<ItemInBot>());
                    items[k].Add(b.getItem(k));
                }
            }
        }
    }
}
