using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSearchService.Model;

namespace TradeSearchService.TradeSearch.Base
{
    public abstract class AbstractTradeBot : TradeBot
    {
        protected Dictionary<string, ItemInBot> items = new Dictionary<string, ItemInBot>();
        protected string name;
        protected string url;
        protected Model.Bot bot;
        public AbstractTradeBot(String name, string url)
        {
            this.name = name;
            this.url = url;
            ItemHelper.CTX.Bots.Add(bot=new Model.Bot() {Name=name,URL=url });
        }


        public void Refresh()
        {
            items.Clear();
            Build();
        }

        public abstract void Build();



        public ItemInBot getItem(string name)
        {
            return items[name];
        }



        public string getName()
        {
            return name;
        }

        public ICollection<String> getItemNames()
        {
            return items.Keys;
        }
    }
}
