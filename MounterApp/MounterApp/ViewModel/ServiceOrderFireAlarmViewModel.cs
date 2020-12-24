﻿using Android.Content;
using Android.OS;
using Android.Widget;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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
    public class ServiceOrderFireAlarmViewModel : BaseViewModel {
        public ServiceOrderFireAlarmViewModel() { }
        //public ServiceOrderViewModel(NewServiceorderExtensionBase _so) {
        //    ServiceOrderID = _so;
        //    App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        //    Analytics.TrackEvent("Инициализация окна заявки технику",
        //    new Dictionary<string,string> {
        //        {"ServiceOrderID",_so.NewServiceorderId.ToString() }
        //    });
        //}
        //public ServiceOrderFireAlarmViewModel(NewServiceorderExtensionBase _so,List<NewServicemanExtensionBase> _servicemans,List<NewMounterExtensionBase> _mounters) {
        //    Analytics.TrackEvent("Инициализация окна заявки технику",
        //    new Dictionary<string,string> {
        //        {"ServicemanPhone",_servicemans.FirstOrDefault().NewPhone },
        //        {"ServiceOrderID",_so.NewServiceorderId.ToString() }
        //    });
        //    ServiceOrderID = _so;
        //    ServiceOrderID.NewDate = ServiceOrderID.NewDate.Value.AddHours(5);
        //    Servicemans = _servicemans;
        //    Mounters = _mounters;
        //    OpacityForm = 1;
        //    WiresVisible = false;
        //    ExtFieldsVisible = false;
        //    //EventsDatesFilterVisible = false;
        //    EventsVisible = false;
        //    GetInfoByGuardObject.Execute(null);
        //    GetCategory.Execute(null);
        //    App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        //    InfoImage = "info.png";
        //    ReorderImage = "reorder.png";
        //    EventImage = "event.png";
        //    CloseImage = "close.png";
        //    TransferImage = "transfer.png";
        //    PeopleImage = "people.png";
        //}

        public ServiceOrderFireAlarmViewModel(NewTest2ExtensionBase _so,List<NewServicemanExtensionBase> _servicemans,List<NewMounterExtensionBase> _mounters) {
            Analytics.TrackEvent("Инициализация окна заявки технику",
            new Dictionary<string,string> {
                {"ServicemanPhone",_servicemans.FirstOrDefault().NewPhone },
                {"ServiceOrderID",_so.NewTest2Id.ToString() }
            });
            ServiceOrderFireAlarm = _so;
            ServiceOrderFireAlarm.NewDate = ServiceOrderFireAlarm.NewDate.Value.AddHours(5);
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
            InfoImage = "info.png";
            ReorderImage = "reorder.png";
            EventImage = "event.png";
            CloseImage = "close.png";
            TransferImage = "transfer.png";
            PeopleImage = "people.png";
        }


        private RelayCommand _NotifyCommand;
        public RelayCommand NotifyCommand {
            get => _NotifyCommand ??= new RelayCommand(async obj => {
            });
        }
        private ImageSource _InfoImage;
        public ImageSource InfoImage {
            get => _InfoImage;
            set {
                _InfoImage = value;
                OnPropertyChanged(nameof(InfoImage));
            }
        }
        private ImageSource _ReorderImage;
        public ImageSource ReorderImage {
            get => _ReorderImage;
            set {
                _ReorderImage = value;
                OnPropertyChanged(nameof(ReorderImage));
            }
        }
        private ImageSource _EventImage;
        public ImageSource EventImage {
            get => _EventImage;
            set {
                _EventImage = value;
                OnPropertyChanged(nameof(EventImage));
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

        private ImageSource _TransferImage;
        public ImageSource TransferImage {
            get => _TransferImage;
            set {
                _TransferImage = value;
                OnPropertyChanged(nameof(TransferImage));
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
        //private NewServiceorderExtensionBase _ServiceOrderID;
        //public NewServiceorderExtensionBase ServiceOrderID {
        //    get => _ServiceOrderID;
        //    set {
        //        _ServiceOrderID = value;
        //        OnPropertyChanged(nameof(ServiceOrderID));
        //    }
        //}

        private NewTest2ExtensionBase _ServiceOrderFireAlarm;
        public NewTest2ExtensionBase ServiceOrderFireAlarm {
            get => _ServiceOrderFireAlarm;
            set {
                _ServiceOrderFireAlarm = value;
                OnPropertyChanged(nameof(ServiceOrderFireAlarm));
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
                Analytics.TrackEvent("Получение списка категорий заявок технику",
                    new Dictionary<string,string> {
                        {"Servicemans",Servicemans.First().NewPhone } });
                using HttpClient client = new HttpClient(GetHttpClientHandler());
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/metadata?ColumnName=new_category&ObjectName=New_test2");                
                List<MetadataModel> mm = new List<MetadataModel>();
                if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                    var resp = response.Content.ReadAsStringAsync().Result;
                    try {
                        Analytics.TrackEvent("Попытка десериализации ответа от сервера с категориями техников");
                        mm = JsonConvert.DeserializeObject<List<MetadataModel>>(resp);
                    }
                    catch (Exception ex) {
                        Crashes.TrackError(new Exception("Ошибка получения списка категорий заявок технику"),
                        new Dictionary<string,string> {
                        {"Servicemans",Servicemans.First().NewPhone },
                        {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                        {"ErrorMessage",ex.Message },
                        {"StatusCode",response.StatusCode.ToString() },
                        {"Response",response.ToString() }
                        });
                    }
                }
                if(mm != null)
                    Category = mm.FirstOrDefault(x => x.Value == ServiceOrderFireAlarm.NewCategory).Label;
            });
        }
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Выход с формы заявки технику");
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
                if(ServiceOrderFireAlarm.NewNumber.HasValue) {
                    Analytics.TrackEvent("Выполнение запроса(Охр.объекты) для получения информации по объекту",
                    new Dictionary<string,string> {
                        {"Servicemans",Servicemans.First().NewPhone },
                        {"ObjectNumber",ServiceOrderFireAlarm.NewNumber.ToString() } 
                    });
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewGuardObjectExtensionBases/GetInfoByNumber?number=" + ServiceOrderFireAlarm.NewNumber);
                    List<NewGuardObjectExtensionBase> goeb = new List<NewGuardObjectExtensionBase>();
                    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                        var resp = response.Content.ReadAsStringAsync().Result;
                        try {
                            Analytics.TrackEvent("Попытка десериализации результата запроса(Охр.объекты) для получения информации по объекту",
                            new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"ObjectNumber",ServiceOrderFireAlarm.NewNumber.ToString() }
                            });
                            goeb = JsonConvert.DeserializeObject<List<NewGuardObjectExtensionBase>>(resp);
                        }
                        catch (Exception ex) {
                            Crashes.TrackError(new Exception("Ошибка десериализации результата запроса(Охр.объекты) для получения информации по объекту"),
                            new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                {"ErrorMessage",ex.Message },
                                {"StatusCode",response.StatusCode.ToString() },
                                {"Response",response.ToString() }
                            });
                        }
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
                else {
                    Analytics.TrackEvent("Выполнение запроса(Андромеда) для получения номера объекта, так как в заявке не был найден номер объекта для поиска в охр. объектах",
                    new Dictionary<string,string> {
                        {"Servicemans",Servicemans.First().NewPhone },
                        {"NewAndromedaServiceorder",ServiceOrderFireAlarm.NewAndromedaServiceorder.ToString() }
                    });
                    using HttpClient clientA28 = new HttpClient(GetHttpClientHandler());
                    HttpResponseMessage responseA28 = await clientA28.GetAsync(Resources.BaseAddress + "/api/NewAndromedaExtensionBases/id?id=" + ServiceOrderFireAlarm.NewAndromedaServiceorder);
                    NewAndromedaExtensionBase andromeda = new NewAndromedaExtensionBase();
                    if(responseA28.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                        var respA28 = responseA28.Content.ReadAsStringAsync().Result;                        
                        try {
                            Analytics.TrackEvent("Попытка десериализации результата запросы из базы(объект: Андромеда)",
                            new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"NewAndromedaServiceorder",ServiceOrderFireAlarm.NewAndromedaServiceorder.ToString() }
                            });
                            andromeda = JsonConvert.DeserializeObject<NewAndromedaExtensionBase>(respA28);
                        }
                        catch (Exception ex) {
                            Crashes.TrackError(new Exception("Ошибка десериализации результата запроса(Андромеда) для получения номера объекта"),
                            new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"ServerResponse",responseA28.Content.ReadAsStringAsync().Result },
                                {"ErrorMessage",ex.Message },
                                {"StatusCode",responseA28.StatusCode.ToString() },
                                {"Response",responseA28.ToString() }
                            });
                        }
                    }
                    if(andromeda != null) {
                        Analytics.TrackEvent("Выполнение запроса(Охр.объекта) для получения информации по объекту",
                                new Dictionary<string,string> {
                                    {"Servicemans",Servicemans.First().NewPhone },
                                    {"ObjectNumber",ServiceOrderFireAlarm.NewNumber.ToString() }
                                });
                        HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewGuardObjectExtensionBases/GetInfoByNumber?number=" + andromeda.NewNumber);                        
                        List<NewGuardObjectExtensionBase> goeb = new List<NewGuardObjectExtensionBase>();
                        if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                            var resp = response.Content.ReadAsStringAsync().Result;
                            try {
                                Analytics.TrackEvent("Попытка десериализации результата запроса(Охр.объекты) для получения информации по объекту",
                                new Dictionary<string,string> {
                                    {"Servicemans",Servicemans.First().NewPhone },
                                    {"ObjectNumber",ServiceOrderFireAlarm.NewNumber.ToString() }
                                });
                                goeb = JsonConvert.DeserializeObject<List<NewGuardObjectExtensionBase>>(resp);
                            }
                            catch (Exception ex) {
                                Crashes.TrackError(new Exception("Ошибка десериализации результата запроса(Охр.объекты) для получения информации по объекту"),
                                new Dictionary<string,string> {
                                    {"Servicemans",Servicemans.First().NewPhone },
                                    {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                    {"ErrorMessage",ex.Message },
                                    {"StatusCode",response.StatusCode.ToString() },
                                    {"Response",response.ToString() }
                                });
                            }
                        }
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
                Analytics.TrackEvent("Заявка технику: вызов команды Пришел. Попытка получения координат",
                new Dictionary<string,string> {
                    {"ServiceOrderID",ServiceOrderFireAlarm.NewTest2Id.ToString() },
                    {"Serviceman",Servicemans.FirstOrDefault().NewPhone}
                });
                Opacity = 0.1;
                IndicatorVisible = true;
                Location location = await Geolocation.GetLastKnownLocationAsync();
                if(location != null) {
                    Latitude = location.Latitude.ToString();
                    Longitude = location.Longitude.ToString();
                }
                Analytics.TrackEvent("Запрос данных на сервере (могло же что-то измениться",
                new Dictionary<string,string> {
                    {"ServiceOrderID",ServiceOrderFireAlarm.NewTest2Id.ToString() }
                });
                using HttpClient client = new HttpClient(GetHttpClientHandler());
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceOrderForFireAlarmExtensionBase/id?id=" + ServiceOrderFireAlarm.NewTest2Id);                
                NewTest2ExtensionBase soeb = null;
                if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                    var resp = response.Content.ReadAsStringAsync().Result;
                    try {
                        soeb = JsonConvert.DeserializeObject<NewTest2ExtensionBase>(resp);
                    }
                    catch(Exception ex) {
                        Crashes.TrackError(new Exception("Ошибка десериализации объекта заявка технику(пс)"),
                        new Dictionary<string,string> {
                        {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                        {"ErrorMessage",ex.Message },
                        {"StatusCode",response.StatusCode.ToString() }
                        });
                    }
                }
                else {
                    Crashes.TrackError(new Exception("Ошибка получения данных об объекте заявка технику(пс) с сервера"),
                    new Dictionary<string,string> {
                    {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                    {"StatusCode",response.StatusCode.ToString() },
                    {"Response",response.ToString() }
                    });
                    await Application.Current.MainPage.DisplayAlert("Ошибка"
                            ,"От сервера не получена информация о текущей заявке. Повторите попытку позже, в случае если ошибка повторяется, сообщите в IT-отдел."
                            ,"OK");
                }
                if(soeb != null) {
                    Analytics.TrackEvent("Попытка записи данных на сервер по объекту заявка технику(пс), заполняем поле Пришел",
                    new Dictionary<string,string> {
                        {"ServiceOrderID",ServiceOrderFireAlarm.NewTest2Id.ToString() }
                    });
                    soeb.NewIncome = DateTime.Now.AddHours(-5);
                    using HttpClient clientPut = new HttpClient(GetHttpClientHandler());
                    var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
                    HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceOrderForFireAlarmExtensionBase",httpContent);
                    if(!responsePut.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                        Crashes.TrackError(new Exception("Ошибка при сохранении объекта Заявка технику"),
                        new Dictionary<string,string> {
                        {"ServerResponse",responsePut.Content.ReadAsStringAsync().Result },
                        {"StatusCode",responsePut.StatusCode.ToString() },
                        {"Response",responsePut.ToString() }
                        });
                        await Application.Current.MainPage.DisplayAlert("Ошибка"
                            ,"При попытке сохранения данных произошла ошибка. Повторите попытку позже, в случае если ошибка повторяется, сообщите в IT-отдел."
                            ,"OK");
                    }
                    else
                        Toast.MakeText(Android.App.Application.Context,"Время прихода записано",ToastLength.Long).Show();
                }
                //запишем координаты
                Analytics.TrackEvent("Попытка записи координат на сервер по объекту заявка технику",
                    new Dictionary<string,string> {
                        {"ServiceOrderID",ServiceOrderFireAlarm.NewTest2Id.ToString() }
                    });
                using(HttpClient clientPost = new HttpClient(GetHttpClientHandler())) {
                    var data = JsonConvert.SerializeObject(new ServiceOrderCoordinates() {
                        SocId = Guid.NewGuid(),
                        SocServiceOrderId = ServiceOrderFireAlarm.NewTest2Id,
                        SocIncomeLatitude = Latitude,
                        SocIncomeLongitude = Longitude
                    });
                    StringContent content = new StringContent(data,Encoding.UTF8,"application/json");
                    HttpResponseMessage responsePost = await clientPost.PostAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates",content);
                    if(!responsePost.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                        Crashes.TrackError(new Exception("Ошибка при сохранении объекта Заявка технику"),
                        new Dictionary<string,string> {
                        {"ServerResponse",responsePost.Content.ReadAsStringAsync().Result },
                        {"StatusCode",responsePost.StatusCode.ToString() },
                        {"Response",responsePost.ToString() }
                        });
                    }
                }
                Opacity = 1;
                IndicatorVisible = false;
            },obj=> ServiceOrderFireAlarm.NewIncome==null);
        }

        private ImageSource _PeopleImage;
        public ImageSource PeopleImage {
            get => _PeopleImage;
            set {
                _PeopleImage = value;
                OnPropertyChanged(nameof(PeopleImage));
            }
        }
        private RelayCommand _CallClientCommand;
        public RelayCommand CallClientCommand {
            get => _CallClientCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Звонок клиенту",
                    new Dictionary<string,string> {
                        {"ServiceOrderID",ServiceOrderFireAlarm.NewTest2Id.ToString() },
                        {"PhoneNumber",obj.ToString() }
                    });
                Uri uri = new Uri("tel:" + obj);
                await Launcher.OpenAsync(uri);
            });
        }
        private RelayCommand _CloseOrderCommand;
        public RelayCommand CloseOrderCommand {
            get => _CloseOrderCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Переход на страницу закрытия заявки",
                    new Dictionary<string,string> {
                        {"ServiceOrderID",ServiceOrderFireAlarm.NewTest2Id.ToString() }
                    });
                CloseOrderPopupPageViewModel vm = new CloseOrderPopupPageViewModel(ServiceOrderFireAlarm,Servicemans,Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new CloseOrderPopupPage(vm));
            });
        }
        private RelayCommand _GetObjectInfoCommand;
        public RelayCommand GetObjectInfoCommand {
            get => _GetObjectInfoCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Переход на страницу получения информации об объекте",
                    new Dictionary<string,string> {
                        {"ServiceOrderID",ServiceOrderFireAlarm.NewTest2Id.ToString() }
                    });
                ObjectInfoViewModel vm = new ObjectInfoViewModel(ServiceOrderFireAlarm,Servicemans,Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new ObjectInfoPopup(vm));
            });
        }

        private RelayCommand _GetCustomersCommand;
        public RelayCommand GetCustomersCommand {
            get => _GetCustomersCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Переход на страницу получения информации об ответсвенных лицах объекта",
                    new Dictionary<string,string> {
                        {"ServiceOrderID",ServiceOrderFireAlarm.NewTest2Id.ToString() }
                    });
                ObjCustsPopupViewModel vm = new ObjCustsPopupViewModel(ServiceOrderFireAlarm);
                await App.Current.MainPage.Navigation.PushPopupAsync(new ObjCustsPopupPage(vm));
            });
        }
        private RelayCommand _GetEventsCommand;
        public RelayCommand GetEventsCommand {
            get => _GetEventsCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Переход на страницу получения событий по объекту",
                    new Dictionary<string,string> {
                        {"ServiceOrderID",ServiceOrderFireAlarm.NewTest2Id.ToString() }
                    });
                EventsPopupViewModel vm = new EventsPopupViewModel(ServiceOrderFireAlarm,Servicemans,Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new EventsPopupPage(vm));
            });
        }
        private RelayCommand _ServiceOrderByObjectCommand;
        public RelayCommand ServiceOrderByObjectCommand {
            get => _ServiceOrderByObjectCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Переход на страницу получения прошлых заявок технику",
                    new Dictionary<string,string> {
                        {"ServiceOrderID",ServiceOrderFireAlarm.NewTest2Id.ToString() }
                    });
                PastOrdersPopupViewModel vm = new PastOrdersPopupViewModel(ServiceOrderFireAlarm,Servicemans,Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new PastOrdersPopupPage(vm));
            });
        }
    }
}