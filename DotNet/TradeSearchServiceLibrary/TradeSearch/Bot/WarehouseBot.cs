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
    public class WarehouseBot : AbstractTradeBot
    {
        public WarehouseBot(string url)
            : base("Warehouse", url)
        {
        }

        private void GetStockInformation(StreamReader br)
        {
            string line = "";
            while ((line = br.ReadLine()) != null)
            {
                if (line.Contains("data-name="))
                {
                    String name = Regex.Split(line, "data-name=\"")[1].Split('"')[0];
                    ItemInBot item = items[name];
                    if (item != null)
                    {
                      //  item.Stock = item.temp;
                    }
                }
            }
        }

        protected override void Build(StreamReader readStream)
        {
            string line;
            while ((line = readStream.ReadLine()) != null)
            {
                if (line.Contains("<tr data-name"))
                {

                    line = line.Substring(line.IndexOf('\"') + 1, line.LastIndexOf('\"'));
                    line = line.Replace(" [BS]", "; Battle Scarred");
                    line = line.Replace(" [FN]", "; Factory New");
                    line = line.Replace(" [FT]", "; Field-Tested");
                    line = line.Replace(" [MW]", "; Minimal Wear");
                    line = line.Replace(" [WW]", "; Well-Worn");

                    var item = new ItemInBot()
                    {
                        Bot = bot,
                        Item = ItemHelper.GetItemByName(line)
                    };

                    readStream.ReadLine();

                    line = readStream.ReadLine();
                    line = line.Substring(line.IndexOf('>') + 1, line.LastIndexOf('<'));
                    String[] stock = line.Split('/');
                    item.Stock = 0;

                    /*var temp */ 
                    item.Stock= int.Parse(stock[0]);
                    item.Max = int.Parse(stock[1]);
                    //if (item.Max == item.temp) item.Max -= item.temp;

                    line = readStream.ReadLine();
                    line = line.Substring(line.IndexOf('>') + 1, line.LastIndexOf('<')).Replace(",", "");
                    item.SellPrice = int.Parse(line) / 25;
                    line = readStream.ReadLine();
                    line = line.Substring(line.IndexOf('>') + 1, line.LastIndexOf('<')).Replace(",", "");
                    item.BuyPrice = int.Parse(line) / 25;
                    items.Add(item.Item.Name, item);
                }
            }

           // OpenConnectionAndDoSomething(
           //     s => GetStockInformation(s),
           //     "https://www.tf2wh.com/allitems");

        }

    }
}
