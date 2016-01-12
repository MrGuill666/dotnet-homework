using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TradeSearchServiceLibrary.Model
{

    public class ItemInBot
    {
        public int ID { get; set; }
        
        public virtual Item Item { get; set; }
        
        public virtual Bot Bot { get; set; }
        
        public int SellPrice { get; set; }
        
        public int BuyPrice { get; set; }
        
        public int Max { get; set; }

        public int Stock { get; set; }

        public ItemInBot() { }
    }
}
