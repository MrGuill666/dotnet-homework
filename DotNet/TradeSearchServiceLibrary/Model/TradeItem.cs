using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TradeSearchServiceLibrary.Model
{
    [DataContract]
    public class TradeItem
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int SellPrice { get; set; }
        [DataMember]
        public int BuyPrice { get; set; }
        [DataMember]
        public int Max { get; set; }
        [DataMember]
        public int Stock { get; set; }
        [DataMember]
        public string BotURL { get; set; }
        [DataMember]
        public string BotName { get; set; }

    }
}
