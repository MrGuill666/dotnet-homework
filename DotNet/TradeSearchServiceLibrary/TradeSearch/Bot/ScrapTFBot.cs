using System;
using System.IO;
using TradeSearchServiceLibrary.Model;
using TradeSearchServiceLibrary.TradeSearch.Base;

namespace TradeSearchServiceLibrary.TradeSearch.Bot
{
    class ScrapTFBot : AbstractTradeBot
    {
        public ScrapTFBot(string url)
            : base("Scraptf", url)
        {
        }

        protected override void Build(StreamReader readStream)
        {            
            string line;
            int item_found_state = 0;
            ItemInBot item = null;
            while ((line = readStream.ReadLine()) != null)
            {
                line = line.Trim();
                line = line.Replace("<i18n>", "").Replace("</i18n>", "");

                switch (item_found_state)
                {
                    case 0:
                        if (line.Contains("data-original-title=\"\"></td>"))
                        {
                            line = readStream.ReadLine();
                            line = line.Replace("</td>", "").Replace("<td>", "");

                            Console.WriteLine(line);
                            item_found_state++;


                            string name = line.Trim();
                            
                            item = new ItemInBot()
                            {
                                Item = ItemHelper.GetItemByName(name),
                                Bot = bot
                            };
                            items.Add(name, item);
                            ItemHelper.CTX.ItemsInBots.Add(item);
                        }

                        break;
                    case 1:
                        if (line.Contains("<td>"))
                        {
                            item_found_state++;
                            int sell = ItemHelper.ParsePrice(line);
                            item.SellPrice = sell;
                        }
                        break;
                    case 2:
                        if (line.Contains("<td>"))
                        {
                            item_found_state++;
                            int buy = ItemHelper.ParsePrice(line);
                            item.BuyPrice = buy;
                        }
                        if (line.Contains(("<div rel=\"tooltip\" title=\"")))
                        {
                            line = line.Replace("<div rel=\"tooltip\" title=\"", "");
                            string[] stockMax = line.Split('"')[0].Split('/');
                            int stock = 0; int max = 0;
                            if (stockMax.Length >= 2)
                            {
                                stock = int.Parse(stockMax[0]);
                                max = int.Parse((stockMax[1]));
                            }
                            item.Stock = stock;
                            item.Max = max;


                            item_found_state = 0;
                        }

                        break;
                    case 3:
                        if (line.Contains(("<div rel=\"tooltip\" title=\"")))
                        {
                            line = line.Replace("<div rel=\"tooltip\" title=\"", "");
                            string[] stockMax = line.Split('"')[0].Split('/');
                            int stock = 0; int max = 0;
                            if (stockMax.Length >= 2)
                            {
                                stock = int.Parse(stockMax[0]);
                                max = int.Parse((stockMax[1]));
                            }
                            item.Stock = stock;
                            item.Max = max;


                            item_found_state = 0;
                        }

                        break;
                }
            }
        }
    }
}
