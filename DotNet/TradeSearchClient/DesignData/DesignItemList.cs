using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSearchClient.ViewModel;

namespace TradeSearchClient.DesignData
{
    public class DesignItemList:ItemListViewModel
    {
        public DesignItemList():base()
        {
            for (int i = 0; i < 10; i++ )
                Items.Add(new DesignItem());
        }
    }
}
