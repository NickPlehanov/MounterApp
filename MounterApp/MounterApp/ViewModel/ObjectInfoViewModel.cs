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
    public class ObjectInfoViewModel : BaseViewModel {
        public ObjectInfoViewModel(NewServiceorderExtensionBase _so,List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            Mounters = _mounters;
            Servicemans = _servicemans;
            ServiceOrder = _so;
            GetWires.Execute(null);
            GetExtFields.Execute(null);
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
        private ObservableCollection<Wires> _WiresCollection = new ObservableCollection<Wires>();
        public ObservableCollection<Wires> WiresCollection {
            get => _WiresCollection;
            set {
                _WiresCollection = value;
                OnPropertyChanged(nameof(WiresCollection));
            }
        }
        private ObservableCollection<ExtFields> _ExtFields = new ObservableCollection<ExtFields>();
        public ObservableCollection<ExtFields> ExtFields {
            get => _ExtFields;
            set {
                _ExtFields = value;
                OnPropertyChanged(nameof(ExtFields));
            }
        }
        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand {
            get => _CloseCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(true);
                ServiceOrderViewModel vm = new ServiceOrderViewModel(ServiceOrder,Servicemans,Mounters);
                App.Current.MainPage = new ServiceOrder(vm);
            });
        }

        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
        }
        private RelayCommand _GetWires;
        public RelayCommand GetWires {
            get => _GetWires ??= new RelayCommand(async obj => {
                WiresCollection.Clear();
                using HttpClient client = new HttpClient(GetHttpClientHandler());
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/wires?objNumber=" + ServiceOrder.NewNumber);
                var resp = response.Content.ReadAsStringAsync().Result;
                List<Wires> _wrs = new List<Wires>();
                try {
                    _wrs = JsonConvert.DeserializeObject<List<Wires>>(resp);
                }
                catch { }
                if(_wrs.Count() > 0) {
                    foreach(var item in _wrs) {
                        WiresCollection.Add(item);
                    }
                }
            });
        }
        private RelayCommand _GetExtFields;
        public RelayCommand GetExtFields {
            get => _GetExtFields ??= new RelayCommand(async obj => {
                ExtFields.Clear();
                using HttpClient client = new HttpClient(GetHttpClientHandler());
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/ext?objNumber=" + ServiceOrder.NewNumber);
                var resp = response.Content.ReadAsStringAsync().Result;
                List<ExtFields> _ext = new List<ExtFields>();
                try {
                    _ext = JsonConvert.DeserializeObject<List<ExtFields>>(resp);
                }
                catch { }
                if(_ext.Count() > 0) {
                    foreach(var item in _ext) {
                        ExtFields.Add(item);
                    }
                }
            });
        }
    }
}
