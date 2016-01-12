using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSearchServiceLibrary.Model;

namespace TradeSearchServiceLibrary.TradeSearch.Base
{
    public class ItemHelper
    {
        public static float KEY_PRICE;
        public static ServiceContext CTX { get; set; }
        private static Dictionary<string, Item> items;
        public static void Initialize()
        {
            items = new Dictionary<string, Item>();

            if (CTX.Items == null) return;
            foreach (var item in CTX.Items)
            {
                items[item.Name] = item;
            }
        }
        public static Item GetItemByName(string name)
        {
            Item item = null;
            items.TryGetValue(name, out item);
            if(item== null)
            {
                item = new Item()
                {
                    Name = name
                };
                items[name] = item;
                CTX.Items.Add(item);
            }

            return item;
        }

        public static int ParsePrice(string line)
        {

            int ret = 0;
            line = line.Replace("<td>", "");
            string[] priceS = line.Split(',');
            foreach (string si in priceS)
            {
                var s = si;
                if (s.Contains("key"))
                {

                    s = s.Replace("keys", "");
                    s = s.Replace("key", "");
                    ret += int.Parse(s.Trim()) * (int)(KEY_PRICE * 100.00f);
                }
                if (s.Contains("ref"))
                {
                    int multiplier = 1;
                    s = s.Replace(" refined", "");
                    if (s.Contains("."))
                    {
                        s = s.Replace(".", "");
                    }
                    else
                    {
                        multiplier = 100;
                    }
                    ret += int.Parse(s.Trim()) * multiplier;
                }
            }
            return ret;
        }
    }
}