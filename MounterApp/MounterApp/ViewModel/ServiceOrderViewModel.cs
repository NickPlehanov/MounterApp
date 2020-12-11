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
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class ServiceOrderViewModel : BaseViewModel {
        public ServiceOrderViewModel() {}
        public ServiceOrderViewModel(NewServiceorderExtensionBase _so) {
            ServiceOrderID = _so;
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        }
        public ServiceOrderViewModel(NewServiceorderExtensionBase _so,List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            ServiceOrderID = _so;
            ServiceOrderID.NewDate = ServiceOrderID.NewDate.Value.AddHours(5);
            Servicemans = _servicemans;
            Mounters = _mounters;
            OpacityForm = 1;
            WiresVisible = false;
            ExtFieldsVisible = false;
            //EventsDatesFilterVisible = false;
            EventsVisible = false;
            GetInfoByGuardObject.Execute(null);
            GetCategory.Execute(null);
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        }

        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
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
        private string _Latitude;
        public string Latitude {
            get => _Latitude;
            set {
                _Latitude = value;
                OnPropertyChanged(nameof(Latitude));
            }
        }
        private string _Longitude;
        public string Longitude {
            get => _Longitude;
            set {
                _Longitude = value;
                OnPropertyChanged(nameof(Longitude));
            }
        }
        private RelayCommand _GetCategory;
        public RelayCommand GetCategory {
            get => _GetCategory ??= new RelayCommand(async obj => {
                using HttpClient client = new HttpClient(GetHttpClientHandler());
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
                ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Servicemans,Mounters);
                App.Current.MainPage = new ServiceOrdersPage(vm);
            });
        }
        private RelayCommand _GetInfoByGuardObject;
        public RelayCommand GetInfoByGuardObject {
            get => _GetInfoByGuardObject ??= new RelayCommand(async obj => {
                Opacity = 0.1;
                IndicatorVisible = true;
                using HttpClient client = new HttpClient(GetHttpClientHandler());
                if(ServiceOrderID.NewNumber.HasValue) {
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
                }
                else {
                    using HttpClient clientA28 = new HttpClient(GetHttpClientHandler());
                    HttpResponseMessage responseA28 = await clientA28.GetAsync(Resources.BaseAddress + "/api/NewAndromedaExtensionBases/id?id=" + ServiceOrderID.NewAndromedaServiceorder);
                    var respA28 = responseA28.Content.ReadAsStringAsync().Result;
                    NewAndromedaExtensionBase andromeda = new NewAndromedaExtensionBase();
                    try {
                        andromeda = JsonConvert.DeserializeObject<NewAndromedaExtensionBase>(respA28);
                    }
                    catch { }
                    if(andromeda != null) {
                        HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewGuardObjectExtensionBases/GetInfoByNumber?number=" + andromeda.NewNumber);
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
                    }
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
                Location location = await Geolocation.GetLastKnownLocationAsync();
                if(location != null) {
                    Latitude = location.Latitude.ToString();
                    Longitude = location.Longitude.ToString();
                }
                using HttpClient client = new HttpClient(GetHttpClientHandler());
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/id?id=" + ServiceOrderID.NewServiceorderId);
                var resp = response.Content.ReadAsStringAsync().Result;
                NewServiceorderExtensionBase soeb = null;
                try {
                    soeb = JsonConvert.DeserializeObject<NewServiceorderExtensionBase>(resp);
                }
                catch { }
                if(soeb != null) {
                    soeb.NewIncome = DateTime.Now.AddHours(-5);
                    using HttpClient clientPut = new HttpClient(GetHttpClientHandler());
                    var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
                    HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases",httpContent);
                }
                //запишем координаты
                using(HttpClient clientPost = new HttpClient(GetHttpClientHandler())) {
                    var data = JsonConvert.SerializeObject(new ServiceOrderCoordinates() {
                        SocId=Guid.NewGuid(),
                        SocServiceOrderId=ServiceOrderID.NewServiceorderId,
                        SocIncomeLatitude= Latitude,
                        SocIncomeLongitude=Longitude
                    });
                    StringContent content = new StringContent(data,Encoding.UTF8,"application/json");
                    HttpResponseMessage responsePost = await clientPost.PostAsync(Resources.BaseAddress+ "/api/ServiceOrderCoordinates",content);
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
                CloseOrderPopupPageViewModel vm = new CloseOrderPopupPageViewModel(ServiceOrderID,Servicemans,Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new CloseOrderPopupPage(vm));
            });
        }
        private RelayCommand _GetObjectInfoCommand;
        public RelayCommand GetObjectInfoCommand {
            get => _GetObjectInfoCommand ??= new RelayCommand(async obj => {
                ObjectInfoViewModel vm = new ObjectInfoViewModel(ServiceOrderID,Servicemans,Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new ObjectInfoPopup(vm));
            });
        }
        private RelayCommand _GetEventsCommand;
        public RelayCommand GetEventsCommand {
            get => _GetEventsCommand ??= new RelayCommand(async obj => {
                EventsPopupViewModel vm = new EventsPopupViewModel(ServiceOrderID,Servicemans,Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new EventsPopupPage(vm));
            });
        }
        private RelayCommand _ServiceOrderByObjectCommand;
        public RelayCommand ServiceOrderByObjectCommand {
            get => _ServiceOrderByObjectCommand ??= new RelayCommand(async obj => {
                PastOrdersPopupViewModel vm = new PastOrdersPopupViewModel(ServiceOrderID,Servicemans,Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new PastOrdersPopupPage(vm));
            });
        }
    }
}
