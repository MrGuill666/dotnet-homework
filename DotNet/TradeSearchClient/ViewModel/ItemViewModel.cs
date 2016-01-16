using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSearchClient.TradeSearchServiceReference;

namespace TradeSearchClient.ViewModel
{
    public class ItemViewModel
    {
        public string Name { get; set; }
        public int Profit { get; private set; }
        public int ProfitRate { get; private set; }
        public string BuyAtName { get; private set; }
        public int BuyPrice { get; private set; }
        public int SellPrice { get; private set; }
        public string SellAtName { get; private set; }
        public string BuyAtURL { get; private set; }
        public string SellAtURL { get; private set; }
        private ObservableCollection<TradeItem> _info = new ObservableCollection<TradeItem>();
        public ObservableCollection<TradeItem> Info {
            get
            {
                return _info;
            }
        }

        public ItemViewModel()
        {
            string text= "NoName";
            Name = text;
            Profit = 0;
            ProfitRate = 0;
            BuyAtName = "none";
            SellAtName = "none";
           /* 
            Info.Add(new TradeItem()
            {
                Name = text,
                BotName="TestBot",
                BotURL="https://google.com",
                SellPrice=666,
                BuyPrice=999,
                Stock=1,
                Max=2,
                ID=1234
                
            });*/
        }

        public void Analyze()
        {
            TradeItem buyAt = null;
            TradeItem sellAt = null;
            Profit = 0;
            ProfitRate = 0;
            foreach(var i in Info)
            {
                if (i.Stock > 0) {
                    if (buyAt == null || i.SellPrice < buyAt.SellPrice)
                    {
                        buyAt = i;
                    }
                }

                if ((i.Max - i.Stock) > 0) {
                    if (sellAt == null || i.BuyPrice > sellAt.BuyPrice)
                    {
                        sellAt = i;
                    }
                }
            }
            if (buyAt != null && sellAt != null)
            {
                Profit = sellAt.BuyPrice-buyAt.SellPrice;
                ProfitRate = Profit * 100 / buyAt.SellPrice;
            }
            if(buyAt!=null)
            {
                BuyAtName = buyAt.BotName;
                BuyAtURL = buyAt.BotURL;
                BuyPrice = buyAt.SellPrice;
            }
            if (sellAt != null)
            {
                SellAtName = sellAt.BotName;
                SellAtURL = sellAt.BotURL;
                SellPrice = sellAt.BuyPrice;
            }
        }
        
    }
}
