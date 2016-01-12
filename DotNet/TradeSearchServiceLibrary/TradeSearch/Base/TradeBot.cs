using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSearchServiceLibrary.Model;

namespace TradeSearchServiceLibrary.TradeSearch.Base
{
    public interface TradeBot
    {
        void Refresh();
        ItemInBot getItem(string name);
        String getName();
        ICollection<string> getItemNames();
    }
}
