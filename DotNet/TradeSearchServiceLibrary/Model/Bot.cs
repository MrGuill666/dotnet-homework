using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TradeSearchServiceLibrary.Model
{
    
    public class Bot
    {
        public int ID { get; set; }
        public Bot()
        {
            Items = new List<ItemInBot>();
        }
        
        public string URL { get; set; }
        
        public string  Name { get; set; }
        
        public virtual ICollection<ItemInBot> Items { get; set; }
    }
}
