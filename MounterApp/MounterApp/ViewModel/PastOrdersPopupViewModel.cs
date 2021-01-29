using Android.Widget;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class PastOrdersPopupViewModel : BaseViewModel {
        //readonly ClientHttp http = new ClientHttp();
        public PastOrdersPopupViewModel(NewServiceorderExtensionBase_ex _so,List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            Mounters = _mounters;
            Servicemans = _servicemans;
            ServiceOrder = _so;
            GetPastServiceOrders.Execute(null);
            ArrowCirclePastServiceOrders = IconName("arrow_circle_down");
            CloseImage = IconName("close");
            IndicatorVisible = false;
            OpacityForm = 1;
            CallImage = IconName("call");
        }
        public PastOrdersPopupViewModel(NewTest2ExtensionBase_ex _so,List<NewServicemanExtensionBase> _servicemans,List<NewMounterExtensionBase> _mounters) {
            Mounters = _mounters;
            Servicemans = _servicemans;
            ServiceOrderFireAlarm = _so;
            GetPastServiceOrders.Execute(null);
            ArrowCirclePastServiceOrders = IconName("arrow_circle_down");
            CloseImage = IconName("close");
            IndicatorVisible = false;
            OpacityForm = 1;
            CallImage = IconName("call");
        }

        private NewTest2ExtensionBase_ex _ServiceOrderFireAlarm;
        public NewTest2ExtensionBase_ex ServiceOrderFireAlarm {
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
        private NewServiceorderExtensionBase_ex _ServiceOrder;
        public NewServiceorderExtensionBase_ex ServiceOrder {
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

        private ObservableCollection<NewServiceorderExtensionBase_ex> _PastServiceOrders = new ObservableCollection<NewServiceorderExtensionBase_ex>();
        public ObservableCollection<NewServiceorderExtensionBase_ex> PastServiceOrders {
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

                //List<NewServiceorderExtensionBase_ex> _pso = new List<NewServiceorderExtensionBase_ex>();
                //List<NewTest2ExtensionBase> _pso_fa = new List<NewTest2ExtensionBase>();
                Guid? andr = ServiceOrder != null ? ServiceOrder.NewAndromedaServiceorder : ServiceOrderFireAlarm.NewAndromedaServiceorder;
                //var t = await http.GetQuery<ObservableCollection<NewServiceorderExtensionBase>>("/api/NewServiceorderExtensionBases/ServiceOrderByObjectNew?Andromeda_ID=" + andr);
                //PastServiceOrders = await ClientHttp.GetQuery<ObservableCollection<NewServiceorderExtensionBase_ex>>("/api/NewServiceorderExtensionBases/ServiceOrderByObjectNew?Andromeda_ID=" + andr);

                PastServiceOrders.Clear();
                List<NewServiceorderExtensionBase_ex> _pso = new List<NewServiceorderExtensionBase_ex>();
                List<NewTest2ExtensionBase> _pso_fa = new List<NewTest2ExtensionBase>();
                string resp = null;
                HttpResponseMessage response = null;
                using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    if(ServiceOrder != null) {
                        response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/ServiceOrderByObjectNew?Andromeda_ID=" + ServiceOrder.NewAndromedaServiceorder);
                        resp = response.Content.ReadAsStringAsync().Result;
                        try {
                            _pso = JsonConvert.DeserializeObject<List<NewServiceorderExtensionBase_ex>>(resp);
                        }
                        catch(Exception ex) {
                            _pso = null;
                            Crashes.TrackError(new Exception("Ошибка десериализации запроса по старым заявкам на объект"),
                            new Dictionary<string,string> {
                            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                            {"ErrorMessage",ex.Message },
                            {"StatusCode",response.StatusCode.ToString() },
                            {"Response",response.ToString() },
                            {"Query","NewServiceorderExtensionBases/ServiceOrderByObjectIDNew?Andromeda_ID=" + ServiceOrder.NewAndromedaServiceorder}
                            });
                        }
                        if(_pso.Count() > 0) {
                            foreach(var item in _pso.OrderByDescending(o => o.NewDate)) {
                                NewServiceorderExtensionBase_ex _item = item;
                                _item.NewDate = _item.NewDate.Value.AddHours(5).Date;
                                PastServiceOrders.Add(_item);
                            }
                        }
                    }
                    else if(ServiceOrderFireAlarm != null) {
                        response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/ServiceOrderByObjectNew?Andromeda_ID=" + ServiceOrderFireAlarm.NewAndromedaServiceorder);
                        resp = response.Content.ReadAsStringAsync().Result;
                        try {
                            _pso = JsonConvert.DeserializeObject<List<NewServiceorderExtensionBase_ex>>(resp);
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
                        if(_pso.Count() > 0) {
                            foreach(var item in _pso.OrderByDescending(o => o.NewDate)) {
                                PastServiceOrders.Add(item);
                                //NewServiceorderExtensionBase_ex _item = item;
                                //_item.NewDate = _item.NewDate.Value.AddHours(5).Date;
                                //PastServiceOrders.Add(new NewServiceorderExtensionBase_ex() {
                                //    NewServiceorderId = _item.NewServiceorderId,
                                //    NewDate=_item.NewDate,
                                //    NewTime=_item.NewTime,
                                //    NewResult=_item.NewResult
                                //});
                            }
                        }
                    }
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            });
        }

        private ImageSource _CallImage;
        public ImageSource CallImage {
            get => _CallImage;
            set {
                _CallImage = value;
                OnPropertyChanged(nameof(CallImage));
            }
        }
        private RelayCommand _CallCustomer;
        public RelayCommand CallCustomer {
            get => _CallCustomer ??= new RelayCommand(async obj => {
                if(!string.IsNullOrEmpty(obj.ToString())) {
                    Analytics.TrackEvent("Звонок клиенту",
                        new Dictionary<string,string> {
                        //{"ServiceOrderID",ServiceOrder.NewServiceorderId.ToString() },
                        {"PhoneNumber",obj.ToString() }
                        });
                    Uri uri = new Uri("tel:" + obj);
                    await Launcher.OpenAsync(uri);
                }
                else
                    Toast.MakeText(Android.App.Application.Context,"Номер телефона не указан",ToastLength.Long).Show();
            });
        }

        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand {
            get => _CloseCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(false);
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
