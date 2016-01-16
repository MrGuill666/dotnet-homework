using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeSearchClient.TradeSearchServiceReference;

namespace TradeSearchClient.ViewModel
{
    public class ItemListViewModel : INotifyPropertyChanged
    {
        ObservableCollection<ItemViewModel> _items = new ObservableCollection<ItemViewModel>();
        List<ItemViewModel> _storedItems = new List<ItemViewModel>();
        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                return _items;
            }
        }

        private RelayCommand<Object> _refreshCommand;
        private RelayCommand<Object> _updateCommand;

        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand<Object>(a => LoadItems());
                }
                return _refreshCommand;
            }
        }
        public ICommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new RelayCommand<Object>(a => UpdateDatabase());
                }
                return _updateCommand;
            }
        }

        async void UpdateDatabase()
        {
            using (TradeSearchServiceReference.TradeSearchServiceClient service = new TradeSearchServiceReference.TradeSearchServiceClient())
            {
                Message = "Updating database";
                RaisePropertyChanged("Message");
                try
                {
                    await service.RefreshDatabaseAsync();
                    Message = "Database Update Finished";
                }
                catch (Exception e)
                {
                    Message = "Database Error";
                }
                RaisePropertyChanged("Message");
            }
        }

        async void LoadItems()
        {
            using (TradeSearchServiceReference.TradeSearchServiceClient service = new TradeSearchServiceReference.TradeSearchServiceClient())
            {
                _storedItems.Clear();
                
                TradeItem[] items;
                Message = "Getting items...";
                RaisePropertyChanged("Message");
                try
                {
                    items= await service.GetItemsAsync();
                    Message = "Done";
                }
                catch (Exception e)
                {
                    Message = "Error";
                    return;
                }
                RaisePropertyChanged("Message");

                Dictionary<int, ItemViewModel> orderedItems = new Dictionary<int, ItemViewModel>();
                foreach (TradeItem i in items)
                {
                    ItemViewModel item;
                    orderedItems.TryGetValue(i.ID, out item);
                    if (item == null)
                    {
                        item = new ItemViewModel()
                        {
                            Name = i.Name
                        };
                        orderedItems[i.ID] = item;
                    }
                    item.Info.Add(i);

                }
                foreach (var i in orderedItems.Values)
                {
                    i.Analyze();
                    _storedItems.Add(i);
                    Filter();
                }


            }
        }

        private int ConvertStringToInt(string text, int valueIfFailed)
        {
            int ret;
            if(!Int32.TryParse(text,out ret))
                ret = valueIfFailed;
            return ret;
        }

        public void Filter()
        {
            _items.Clear();
            int minP, minBP, maxBP, minSP, maxSP, minRP;

            minP = ConvertStringToInt(MinimumProfit, int.MinValue);
            minRP = ConvertStringToInt(RelativeMinimumProfit, int.MinValue);
            minBP = ConvertStringToInt(MinBuyPrice, int.MinValue);
            minSP = ConvertStringToInt(MinSellPrice, int.MinValue);
            maxBP = ConvertStringToInt(MaxBuyPrice, int.MaxValue);
            maxSP = ConvertStringToInt(MaxSellPrice, int.MaxValue);
            _storedItems.Where(i =>
                i.Name.ToLower().Contains(_filterName.ToLower()) 
                && i.Profit>=minP && i.ProfitRate>=minRP
                   && i.SellPrice<=maxSP&&i.SellPrice>=minSP &&
                i.BuyPrice<=maxBP&&i.BuyPrice>=minBP

            ).ToList().ForEach(i => _items.Add(i));
        }


        public string Message { get; set; }

        string _filterName = "";
        public string FilterName {
            get { return _filterName; }
            set { _filterName = value; RaisePropertyChanged("Name"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            Filter();
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string minp;
        public string MinimumProfit { get { return minp; } set { minp = value; RaisePropertyChanged("MinProfit"); } }
        private string relminp;
        public string RelativeMinimumProfit { get { return relminp; } set { relminp = value; RaisePropertyChanged("MinRelProfit"); } }
        private string minbp;
        public string MinBuyPrice { get { return minbp; } set { minbp = value; RaisePropertyChanged("MinBuyPrice"); } }
        private string maxbp;
        public string MaxBuyPrice { get { return maxbp; } set { maxbp = value; RaisePropertyChanged("MaxBuyPrice"); } }
        private string minsp;
        public string MinSellPrice { get { return minsp; } set { minsp = value; RaisePropertyChanged("MinSellPrice"); } }
        private string maxsp;
        public string MaxSellPrice { get { return maxsp; } set { maxsp = value; RaisePropertyChanged("MaxSellPrice"); } }


    }
}
