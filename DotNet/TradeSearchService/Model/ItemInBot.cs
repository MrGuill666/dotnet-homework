using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSearchService.Model
{
    public class ItemInBot
    {
        public int ID { get; set; }
        public Item Item { get; set; }
        public Bot Bot { get; set; }
        public int SellPrice { get; set; }
        public int BuyPrice { get; set; }
        public int Max { get; set; }
        public int Stock { get; set; }

        public ItemInBot(Item item)
        {
            Item = item;
        }
    }
}
