using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TradeSearchServiceLibrary.Model;

namespace TradeSearchServiceLibrary.TradeSearch.Base
{
    public abstract class AbstractTradeBot : TradeBot
    {
        protected Dictionary<string, ItemInBot> items = new Dictionary<string, ItemInBot>();
        protected string name;
        private string url;
        protected Model.Bot bot;

        public AbstractTradeBot(String name, string url)
        {
            this.name = name;
            this.url = url;
        }

        protected void OpenConnectionAndDoSomething(Action<StreamReader> Action, string url)
        {
            //Open connection
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HttpWebRequest httpcon = (HttpWebRequest)WebRequest.Create(url);
            httpcon.UserAgent = "Mozilla/4.0";
            HttpWebResponse response = (HttpWebResponse)httpcon.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                //Do something
                Action(readStream);


                response.Close();
                readStream.Close();
            }

        }

        public void Refresh()
        {
            items.Clear();
            ItemHelper.CTX.Bots.Add(bot = new Model.Bot() { Name = name, URL = url });
            OpenConnectionAndDoSomething(Build,url);
        }

        protected abstract void Build(StreamReader readStream);

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
