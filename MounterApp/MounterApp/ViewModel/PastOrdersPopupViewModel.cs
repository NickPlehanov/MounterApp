using Microsoft.AppCenter.Crashes;
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
            ArrowCirclePastServiceOrders = IconName("arrow_circle_down");
            CloseImage = IconName("close");
            IndicatorVisible = false;
            OpacityForm = 1;
        }
        public PastOrdersPopupViewModel(NewTest2ExtensionBase _so,List<NewServicemanExtensionBase> _servicemans,List<NewMounterExtensionBase> _mounters) {
            Mounters = _mounters;
            Servicemans = _servicemans;
            ServiceOrderFireAlarm = _so;
            GetPastServiceOrders.Execute(null);
            ArrowCirclePastServiceOrders = IconName("arrow_circle_down");
            CloseImage = IconName("close");
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
                List<NewTest2ExtensionBase> _pso_fa = new List<NewTest2ExtensionBase>();
                string resp = null;
                HttpResponseMessage response = null;
                using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    if(ServiceOrder != null) {
                        response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/ServiceOrderByObject?Andromeda_ID=" + ServiceOrder.NewAndromedaServiceorder + "&ObjectNumber=" + ServiceOrder.NewNumber);
                        resp = response.Content.ReadAsStringAsync().Result;
                        try {
                            _pso = JsonConvert.DeserializeObject<List<NewServiceorderExtensionBase>>(resp);
                        }
                        catch (Exception ex) {
                            _pso = null;
                            Crashes.TrackError(new Exception("Ошибка десериализации запроса по старым заявкам на объект"),
                            new Dictionary<string,string> {
                            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                            {"ErrorMessage",ex.Message },
                            {"StatusCode",response.StatusCode.ToString() },
                            {"Response",response.ToString() },
                            {"Query","NewServiceorderExtensionBases/ServiceOrderByObject?Andromeda_ID=" + ServiceOrder.NewAndromedaServiceorder + "&ObjectNumber=" + ServiceOrder.NewNumber }
                            });
                        }
                        if(_pso.Count() > 0) {
                            foreach(var item in _pso.OrderByDescending(o => o.NewDate)) {
                                NewServiceorderExtensionBase _item = item;
                                _item.NewDate = _item.NewDate.Value.AddHours(5).Date;
                                PastServiceOrders.Add(_item);
                            }
                        }
                    }
                    else if(ServiceOrderFireAlarm != null) {
                        response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/ServiceOrderByObject?Andromeda_ID=" + ServiceOrderFireAlarm.NewAndromedaServiceorder + "&ObjectNumber=" + ServiceOrderFireAlarm.NewNumber);
                        resp = response.Content.ReadAsStringAsync().Result;
                        try {
                            _pso_fa = JsonConvert.DeserializeObject<List<NewTest2ExtensionBase>>(resp);
                        }
                        catch(Exception ex) {
                            _pso = null;
                            Crashes.TrackError(new Exception("Ошибка десериализации запроса по старым заявкам на объект"),
                            new Dictionary<string,string> {
                            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                            {"ErrorMessage",ex.Message },
                            {"StatusCode",response.StatusCode.ToString() },
                            {"Response",response.ToString() },
                            {"Query","NewServiceorderExtensionBases/ServiceOrderByObject?Andromeda_ID=" + ServiceOrder.NewAndromedaServiceorder + "&ObjectNumber=" + ServiceOrder.NewNumber }
                            });
                        }
                        if(_pso_fa.Count() > 0) {
                            foreach(var item in _pso_fa.OrderByDescending(o => o.NewDate)) {
                                NewTest2ExtensionBase _item = item;
                                _item.NewDate = _item.NewDate.Value.AddHours(5).Date;
                                PastServiceOrders.Add(new NewServiceorderExtensionBase() {
                                    NewServiceorderId = _item.NewTest2Id,
                                    NewDate=_item.NewDate,
                                    NewTime=_item.NewTime,
                                    NewResult=_item.NewResult
                                });
                            }
                        }
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
                    ArrowCirclePastServiceOrders = IconName("arrow_circle_up");
                else
                    ArrowCirclePastServiceOrders = IconName("arrow_circle_down");
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
