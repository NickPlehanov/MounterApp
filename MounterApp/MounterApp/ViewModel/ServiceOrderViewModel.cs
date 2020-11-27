using MounterApp.Helpers;
using MounterApp.InternalModel;
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
using Xamarin.Essentials;

namespace MounterApp.ViewModel {
    public class ServiceOrderViewModel : BaseViewModel {
        public ServiceOrderViewModel() {}
        public ServiceOrderViewModel(NewServiceorderExtensionBase _so) {
            ServiceOrderID = _so;
        }
        public ServiceOrderViewModel(NewServiceorderExtensionBase _so,List<NewServicemanExtensionBase> _servicemans) {
            ServiceOrderID = _so;
            ServiceOrderID.NewDate = ServiceOrderID.NewDate.Value.AddHours(5);
            Servicemans = _servicemans;
            OpacityForm = 1;
            WiresVisible = false;
            ExtFieldsVisible = false;
            //EventsDatesFilterVisible = false;
            EventsVisible = false;
            GetInfoByGuardObject.Execute(null);
            GetCategory.Execute(null);
        }
        private bool _ObjectInfoVisible;
        public bool ObjectInfoVisible {
            get => _ObjectInfoVisible;
            set {
                _ObjectInfoVisible = value;
                OnPropertyChanged(nameof(ObjectInfoVisible));
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
        private ObservableCollection<GetEventsReceivedFromObject_Result> _Events = new ObservableCollection<GetEventsReceivedFromObject_Result>();
        public ObservableCollection<GetEventsReceivedFromObject_Result> Events {
            get => _Events;
            set {
                _Events = value;
                OnPropertyChanged(nameof(Events));
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
        private List<NewServicemanExtensionBase> _Servicemans;
        public List<NewServicemanExtensionBase> Servicemans {
            get => _Servicemans;
            set {
                _Servicemans = value;
                OnPropertyChanged(nameof(Servicemans));
            }
        }
        private NewServiceorderExtensionBase _ServiceOrderID;
        public NewServiceorderExtensionBase ServiceOrderID {
            get => _ServiceOrderID;
            set {
                _ServiceOrderID = value;
                OnPropertyChanged(nameof(ServiceOrderID));
            }
        }
        private string _Siding;
        public string Siding {
            get => _Siding;
            set {
                _Siding = value;
                OnPropertyChanged(nameof(Siding));
            }
        }
        private string _Contact;
        public string Contact {
            get => _Contact;
            set {
                _Contact = value;
                OnPropertyChanged(nameof(Contact));
            }
        }
        private bool _rrOS;
        public bool rrOS {
            get => _rrOS;
            set {
                _rrOS = value;
                OnPropertyChanged(nameof(rrOS));
            }
        }
        private bool _rrPS;
        public bool rrPS {
            get => _rrPS;
            set {
                _rrPS = value;
                OnPropertyChanged(nameof(rrPS));
            }
        }
        private bool _rrVideo;
        public bool rrVideo {
            get => _rrVideo;
            set {
                _rrVideo = value;
                OnPropertyChanged(nameof(rrVideo));
            }
        }
        private bool _rrAccess;
        public bool rrAccess {
            get => _rrAccess;
            set {
                _rrAccess = value;
                OnPropertyChanged(nameof(rrAccess));
            }
        }
        private string _Category;
        public string Category {
            get => _Category;
            set {
                _Category = value;
                OnPropertyChanged(nameof(Category));
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
        private double _Opacity;
        public double Opacity {
            get => _Opacity;
            set {
                _Opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }
        private bool _WiresVisible;
        public bool WiresVisible {
            get => _WiresVisible;
            set {
                _WiresVisible = value;
                OnPropertyChanged(nameof(WiresVisible));
            }
        }
        private bool _ExtFieldsVisible;
        public bool ExtFieldsVisible {
            get => _ExtFieldsVisible;
            set {
                _ExtFieldsVisible = value;
                OnPropertyChanged(nameof(ExtFieldsVisible));
            }
        }
        private DateTime _DateStart;
        public DateTime DateStart {
            get => _DateStart;
            set {
                if(value == DateTime.Parse("01.01.1900 00:00:00"))
                    _DateStart = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy")).AddDays(-1);
                else
                    _DateStart = value;
                OnPropertyChanged(nameof(DateStart));
            }
        }
        private DateTime _DateEnd;
        public DateTime DateEnd {
            get => _DateEnd;
            set {
                if(value == DateTime.Parse("01.01.1900 00:00:00"))
                    _DateEnd = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
                else
                    _DateEnd = value;
                OnPropertyChanged(nameof(DateEnd));
            }
        }
        private bool _EventsVisible;
        public bool EventsVisible {
            get => _EventsVisible;
            set {
                _EventsVisible = value;
                OnPropertyChanged(nameof(EventsVisible));
            }
        }
        private RelayCommand _GetCategory;
        public RelayCommand GetCategory {
            get => _GetCategory ??= new RelayCommand(async obj => {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/metadata?ColumnName=new_category&ObjectName=New_serviceorder");
                var resp = response.Content.ReadAsStringAsync().Result;
                List<MetadataModel> mm = new List<MetadataModel>();
                try {
                    mm = JsonConvert.DeserializeObject<List<MetadataModel>>(resp);
                }
                catch { }
                if(mm != null)
                    Category = mm.FirstOrDefault(x => x.Value == ServiceOrderID.NewCategory).Label;
            });
        }
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Servicemans);
                App.Current.MainPage = new ServiceOrdersPage(vm);
            });
        }
        private RelayCommand _GetInfoByGuardObject;
        public RelayCommand GetInfoByGuardObject {
            get => _GetInfoByGuardObject ??= new RelayCommand(async obj => {
                Opacity = 0.1;
                IndicatorVisible = true;
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewGuardObjectExtensionBases/GetInfoByNumber?number=" + ServiceOrderID.NewNumber);
                var resp = response.Content.ReadAsStringAsync().Result;
                List<NewGuardObjectExtensionBase> goeb = new List<NewGuardObjectExtensionBase>();
                try {
                    goeb = JsonConvert.DeserializeObject<List<NewGuardObjectExtensionBase>>(resp);
                }
                catch { }
                if(goeb.Count > 0) {
                    Contact = goeb.FirstOrDefault().NewFirstcontact;
                    Siding = goeb.FirstOrDefault().NewSiding;
                    rrOS = goeb.FirstOrDefault().NewRrOs.HasValue ? (bool)goeb.FirstOrDefault().NewRrOs : false;
                    rrPS = goeb.FirstOrDefault().NewRrPs.HasValue ? (bool)goeb.FirstOrDefault().NewRrPs : false;
                    rrVideo = goeb.FirstOrDefault().NewRrVideo.HasValue ? (bool)goeb.FirstOrDefault().NewRrVideo : false;
                    rrAccess = goeb.FirstOrDefault().NewRrSkud.HasValue ? (bool)goeb.FirstOrDefault().NewRrSkud : false;
                }
                Opacity = 1;
                IndicatorVisible = false;
            });
        }
        private RelayCommand _IncomeCommand;
        public RelayCommand IncomeCommand {
            get => _IncomeCommand ??= new RelayCommand(async obj => {
                Opacity = 0.1;
                IndicatorVisible = true;
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/id?id=" + ServiceOrderID.NewServiceorderId);
                var resp = response.Content.ReadAsStringAsync().Result;
                NewServiceorderExtensionBase soeb = null;
                try {
                    soeb = JsonConvert.DeserializeObject<NewServiceorderExtensionBase>(resp);
                }
                catch { }
                if(soeb != null) {
                    soeb.NewIncome = DateTime.Now.AddHours(-5);
                    using HttpClient clientPut = new HttpClient();
                    var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
                    HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases",httpContent);
                }
                Opacity = 1;
                IndicatorVisible = false;
            });
        }
        private RelayCommand _OutcomeCommand;
        public RelayCommand OutcomeCommand {
            get => _OutcomeCommand ??= new RelayCommand(async obj => {

            });
        }
        private RelayCommand _CallClientCommand;
        public RelayCommand CallClientCommand {
            get => _CallClientCommand ??= new RelayCommand(async obj => {
                Uri uri = new Uri("tel:" + obj);
                await Launcher.OpenAsync(uri);
            });
        }
        private RelayCommand _CloseOrderCommand;
        public RelayCommand CloseOrderCommand {
            get => _CloseOrderCommand ??= new RelayCommand(async obj => {
                //OpacityForm = 0.1;
                CloseOrderPopupPageViewModel vm = new CloseOrderPopupPageViewModel(ServiceOrderID,Servicemans);
                await App.Current.MainPage.Navigation.PushPopupAsync(new CloseOrderPopupPage(vm));
            });
        }
        private RelayCommand _GetObjectInfoCommand;
        public RelayCommand GetObjectInfoCommand {
            get => _GetObjectInfoCommand ??= new RelayCommand(async obj => {

                //Opacity = 0.1;
                //IndicatorVisible = true;
                //WiresCollection.Clear();
                //ExtFields.Clear();
                //using HttpClient client = new HttpClient();
                //HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/wires?objNumber=" + ServiceOrderID.NewNumber);
                //var resp = response.Content.ReadAsStringAsync().Result;
                //List<Wires> _wrs = new List<Wires>();
                //try {
                //    _wrs = JsonConvert.DeserializeObject<List<Wires>>(resp);
                //}
                //catch { }
                //if(_wrs.Count()>0) {
                //    foreach(var item in _wrs) {
                //        WiresCollection.Add(item);
                //    }
                //}

                //response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/ext?objNumber=" + ServiceOrderID.NewNumber);
                //resp = response.Content.ReadAsStringAsync().Result;
                //List<ExtFields> _ext = new List<ExtFields>();
                //try {
                //    _ext = JsonConvert.DeserializeObject<List<ExtFields>>(resp);
                //}
                //catch { }
                //if(_wrs.Count() > 0) {
                //    foreach(var item in _ext) {
                //        ExtFields.Add(item);
                //    }
                //}
                //Opacity = 1;
                //IndicatorVisible = false;
                //WiresVisible = true;
                //ExtFieldsVisible = true;
                ObjectInfoViewModel vm = new ObjectInfoViewModel(ServiceOrderID,Servicemans);
                await App.Current.MainPage.Navigation.PushPopupAsync(new ObjectInfoPopup(vm));
            });
        }
        private RelayCommand _GetEventsCommand;
        public RelayCommand GetEventsCommand {
            get => _GetEventsCommand ??= new RelayCommand(async obj => {
                //Opacity = 0.1;
                //IndicatorVisible = true;
                //EventsVisible = true;
                //Events.Clear();
                //using HttpClient client = new HttpClient();
                //HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/events?objNumber="+ServiceOrderID.NewNumber +
                //    "&startDate=" + DateStart+
                //    "&endDate="+DateEnd+
                //    "&testFiltered=0&doubleFiltered=0"
                //    );
                //var resp = response.Content.ReadAsStringAsync().Result;
                //List<GetEventsReceivedFromObject_Result> _evnts = new List<GetEventsReceivedFromObject_Result>();
                //try {
                //    _evnts = JsonConvert.DeserializeObject<List<GetEventsReceivedFromObject_Result>>(resp);
                //}                
                //catch (Exception ex) {}
                //if(_evnts.Count > 0) {
                //    foreach(var item in _evnts) 
                //        Events.Add(item);
                //    EventsVisible = true;
                //}
                //Opacity = 1;
                //IndicatorVisible = false;
                EventsPopupViewModel vm = new EventsPopupViewModel(ServiceOrderID,Servicemans);
                await App.Current.MainPage.Navigation.PushPopupAsync(new EventsPopupPage(vm));
            });
        }
    }
}
