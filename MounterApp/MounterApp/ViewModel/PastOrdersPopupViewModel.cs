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
using System.Threading;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class PastOrdersPopupViewModel : BaseViewModel {
        public PastOrdersPopupViewModel(NewServiceorderExtensionBase _so,List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            Mounters = _mounters;
            Servicemans = _servicemans;
            ServiceOrder = _so;
            GetPastServiceOrders.Execute(null);
            ArrowCirclePastServiceOrders = "arrow_circle_down.png";
            CloseImage = "close.png";
            IndicatorVisible = false;
            OpacityForm = 1;
        }
        public PastOrdersPopupViewModel(NewTest2ExtensionBase _so,List<NewServicemanExtensionBase> _servicemans,List<NewMounterExtensionBase> _mounters) {
            Mounters = _mounters;
            Servicemans = _servicemans;
            ServiceOrderFireAlarm = _so;
            GetPastServiceOrders.Execute(null);
            ArrowCirclePastServiceOrders = "arrow_circle_down.png";
            CloseImage = "close.png";
            IndicatorVisible = false;
            OpacityForm = 1;
        }

        private NewTest2ExtensionBase _ServiceOrderFireAlarm;
        public NewTest2ExtensionBase ServiceOrderFireAlarm {
            get => _ServiceOrderFireAlarm;
            set {
                _ServiceOrderFireAlarm = value;
                OnPropertyChanged(nameof(ServiceOrderFireAlarm));
            }
        }
        private bool _IndicatorVisible;
        public bool IndicatorVisible {
            get => _IndicatorVisible;
            set {
                _IndicatorVisible = value;
                OnPropertyChanged(nameof(IndicatorVisible));
            }
        }

        private double _OpacityForm;
        public double OpacityForm {
            get => _OpacityForm;
            set {
                _OpacityForm = value;
                OnPropertyChanged(nameof(OpacityForm));
            }
        }
        private ImageSource _CloseImage;
        public ImageSource CloseImage {
            get => _CloseImage;
            set {
                _CloseImage = value;
                OnPropertyChanged(nameof(CloseImage));
            }
        }
        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
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
                IndicatorVisible = true;
                OpacityForm = 0.1;
                PastServiceOrders.Clear();
                List<NewServiceorderExtensionBase> _pso = new List<NewServiceorderExtensionBase>();
                string resp = null;
                HttpResponseMessage response = null;
                using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    if(ServiceOrder != null) {
                        response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/ServiceOrderByObject?Andromeda_ID=" + ServiceOrder.NewAndromedaServiceorder + "&ObjectNumber=" + ServiceOrder.NewNumber);
                    }
                    else if(ServiceOrderFireAlarm != null) {
                        response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/ServiceOrderByObject?Andromeda_ID=" + ServiceOrderFireAlarm.NewAndromedaServiceorder + "&ObjectNumber=" + ServiceOrder.NewNumber);
                    }
                    resp = response.Content.ReadAsStringAsync().Result;
                }
                try {
                    //TODO: в данном методе происходит возврат различных сущностей: "заявка технику" или "заявка на пс" надо проверять внутри if
                    _pso = JsonConvert.DeserializeObject<List<NewServiceorderExtensionBase>>(resp);
                }
                catch { }
                if(_pso.Count() > 0) {
                    foreach(var item in _pso.OrderByDescending(o => o.NewDate)) {
                        NewServiceorderExtensionBase _item = item;
                        _item.NewDate = _item.NewDate.Value.AddHours(5).Date;
                        PastServiceOrders.Add(_item);
                    }
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            });
        }

        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand {
            get => _CloseCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(true);
                //ServiceOrderViewModel vm = new ServiceOrderViewModel(ServiceOrder,Servicemans,Mounters);
                //App.Current.MainPage = new ServiceOrder(vm);
            });
        }

        private bool _PastServiceOrdersExpandedState;
        public bool PastServiceOrdersExpandedState {
            get => _PastServiceOrdersExpandedState;
            set {
                if(_PastServiceOrdersExpandedState)
                    ArrowCirclePastServiceOrders = "arrow_circle_up.png";
                else
                    ArrowCirclePastServiceOrders = "arrow_circle_down.png";
                _PastServiceOrdersExpandedState = value;
                OnPropertyChanged(nameof(PastServiceOrdersExpandedState));
            }
        }
        private ImageSource _ArrowCirclePastServiceOrders;
        public ImageSource ArrowCirclePastServiceOrders {
            get => _ArrowCirclePastServiceOrders;
            set {
                _ArrowCirclePastServiceOrders = value;
                OnPropertyChanged(nameof(ArrowCirclePastServiceOrders));
            }
        }

        private RelayCommand _PastServiceOrdersExpanderCommand;
        public RelayCommand PastServiceOrdersExpanderCommand {
            get => _PastServiceOrdersExpanderCommand ??= new RelayCommand(async obj => {
                PastServiceOrdersExpandedState = !PastServiceOrdersExpandedState;
            });
        }
    }
}
