using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace MounterApp.ViewModel {
    public class PastOrdersPopupViewModel : BaseViewModel {
        public PastOrdersPopupViewModel(NewServiceorderExtensionBase _so,List<NewServicemanExtensionBase> _servicemans) {
            Servicemans = _servicemans;
            ServiceOrder = _so;
            GetPastServiceOrders.Execute(null);
        }
        private NewServiceorderExtensionBase _ServiceOrder;
        public NewServiceorderExtensionBase ServiceOrder {
            get => _ServiceOrder;
            set {
                _ServiceOrder = value;
                OnPropertyChanged(nameof(ServiceOrder));
            }
        }
        private List<NewServicemanExtensionBase> _Servicemans;
        public List<NewServicemanExtensionBase> Servicemans {
            get => _Servicemans;
            set {
                _Servicemans = value;
                OnPropertyChanged(nameof(Servicemans));
            }
        }

        private ObservableCollection<NewServiceorderExtensionBase> _PastServiceOrders = new ObservableCollection<NewServiceorderExtensionBase>();
        public ObservableCollection<NewServiceorderExtensionBase> PastServiceOrders {
            get => _PastServiceOrders;
            set {
                _PastServiceOrders = value;
                OnPropertyChanged(nameof(PastServiceOrders));
            }
        }

        private RelayCommand _GetPastServiceOrders;
        public RelayCommand GetPastServiceOrders {
            get => _GetPastServiceOrders ??= new RelayCommand(async obj => {
                PastServiceOrders.Clear();
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/ServiceOrderByObject?Andromeda_ID="+ServiceOrder.NewAndromedaServiceorder+"&ObjectNumber="+ServiceOrder.NewNumber);
                var resp = response.Content.ReadAsStringAsync().Result;
                List<NewServiceorderExtensionBase> _pso = new List<NewServiceorderExtensionBase>();
                try {
                    _pso = JsonConvert.DeserializeObject<List<NewServiceorderExtensionBase>>(resp);
                }
                catch { }
                if(_pso.Count() > 0)
                    foreach(var item in _pso.OrderByDescending(o => o.NewDate)) {
                        NewServiceorderExtensionBase _item = item;
                        _item.NewDate = _item.NewDate.Value.AddHours(5).Date;
                        PastServiceOrders.Add(_item);
                    }
            });
        }

        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand {
            get => _CloseCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(true);
                ServiceOrderViewModel vm = new ServiceOrderViewModel(ServiceOrder,Servicemans);
                App.Current.MainPage = new ServiceOrder(vm);
            });
        }
    }
}
