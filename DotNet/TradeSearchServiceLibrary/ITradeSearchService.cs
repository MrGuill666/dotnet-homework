using System.Collections.Generic;
using System.ServiceModel;
using TradeSearchServiceLibrary.Model;

namespace TradeSearchServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITradeSearchService" in both code and config file together.
    [ServiceContract]
    public interface ITradeSearchService
    {
        [OperationContract]
        List<string> RefreshDatabase();

        [OperationContract]
        List<TradeItem> GetItems();
    }
}
