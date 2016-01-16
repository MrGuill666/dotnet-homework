using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSearchClient.TradeSearchServiceReference;
using TradeSearchClient.ViewModel;

namespace TradeSearchClient.DesignData
{
    public class DesignItem:ItemViewModel
    {
        public DesignItem():base()
        {
            for (int i = 0; i < 3; i++)
            {
                Info.Add(new TradeItem()
                {
                    Name = "TestName",
                    BotName = "TestBot",
                    BotURL = "https://google.com",
                    SellPrice = 666,
                    BuyPrice = 999,
                    Stock = 1,
                    Max = 2,
                    ID = 1234

                });
            }
        }
    }
}
