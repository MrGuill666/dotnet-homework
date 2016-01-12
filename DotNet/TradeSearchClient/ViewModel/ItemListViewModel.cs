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

        public void Filter()
        {
            _items.Clear();
            _storedItems.Where(i =>
                i.Name.ToLower().Contains(_filterName.ToLower()) 
                //&& i.Profit>FilterProfit &&
                //true
            ).ToList().ForEach(i => _items.Add(i));
        }


        public string Message { get; set; }

        string _filterName = "";
        public string FilterName {
            get { return _filterName; }
            set { _filterName = value; Filter(); } }

        int _filterProfit =0;
        public int FilterProfit
        {
            get { return _filterProfit; }
            set { _filterProfit = value; Filter(); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
