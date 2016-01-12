using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TradeSearchServiceLibrary.Model;
using TradeSearchServiceLibrary.TradeSearch.Base;

namespace TradeSearchServiceLibrary.TradeSearch.Bot
{
    class TradeTFBot : AbstractTradeBot
    {
        public TradeTFBot(string url) : base("Trade.tf", url)
        {
        }


        protected override void Build(StreamReader readStream)
        {
            String line = "";
            while ((line = readStream.ReadLine()) != null)
            {
                bool buy = false;
                if (line.Contains("$scope.sellers = "))
                    buy = false;
                else if (line.Contains("$scope.buyers = "))
                    buy = true;

                if (line.Contains("$scope.sellers = ") || line.Contains("$scope.buyers = "))
                {
                    line = line.Replace("$scope.sellers = ", "");
                    line = line.Trim();
                    string[] itemStrings = Regex.Split(line,"\\}, \\{");
                    foreach (String itemString in itemStrings)
                    {
                       
                        string tmp= itemString.Substring(itemString.LastIndexOf("\"price\": {"));
                        tmp= tmp.Replace("\"price\": {", "");
                        String[] itemS = Regex.Split(tmp,"\\}, ");
                        string name= itemS[1].Replace("\"name\": ", "").Replace("\"", "").Replace("}];", "").Trim();

                        ItemInBot item = new ItemInBot()
                        {
                            Bot = bot,
                            Item = ItemHelper.GetItemByName(name),
                            BuyPrice = 0,
                            SellPrice = 999999999,
                            Max = 0,
                            Stock=0
                        };

                        String[] price = itemS[0].Replace("\"keys\":", "").Replace("\"refs\":", "").Replace(".0", "00").Replace(".", "").Split(',');
                        int sum = (int)(int.Parse(price[0].Trim()) * ItemHelper.KEY_PRICE + int.Parse(price[2].Trim())) ;
                        if (items.Keys.Contains(item.Item.Name))
                        {
                            item = items[item.Item.Name];
                        }
                        else {
                            items.Add(item.Item.Name, item);
                            ItemHelper.CTX.ItemsInBots.Add(item);
                        }
                        if (buy) {
                            item.BuyPrice = sum;
                            item.Max++;
                        }
                        else { item.SellPrice = sum; item.Stock++; item.Max++; }
                    }
                }
            }
        }
    }
}
