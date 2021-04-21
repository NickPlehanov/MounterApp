using Android.Content;
using Android.Widget;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace MounterApp.ViewModel {
    public class ServiceOrderViewModel : BaseViewModel {
        public ServiceOrderViewModel() { }
        public ServiceOrderViewModel(NewServiceorderExtensionBase_ex _so, List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            Analytics.TrackEvent("Инициализация окна заявки технику",
            new Dictionary<string, string> {
                {"ServicemanPhone",_servicemans.FirstOrDefault().NewPhone },
                {"ServiceOrderID",_so.NewServiceorderId.ToString() }
            });
            ServiceOrderID = _so;
            ServiceOrderID.NewDate = ServiceOrderID.NewDate.Value.AddHours(5);
            Servicemans = _servicemans;
            Mounters = _mounters;
            OpacityForm = 1;
            WiresVisible = false;
            ExtFieldsVisible = false;
            EventsVisible = false;
            GetInfoByGuardObject.Execute(null);
            GetCategory.Execute(null);
            GetObjectNameCommand.Execute(null);
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
            InfoImage = IconName("info");
            ReorderImage = IconName("reorder");
            EventImage = IconName("event");
            CloseImage = IconName("close");
            TransferImage = IconName("transfer");
            PeopleImage = IconName("people");
            IncomeButtonText = ServiceOrderID.NewIncome.HasValue ? ServiceOrderID.NewIncome.Value.AddHours(5).ToShortTimeString() : "Пришел";
        }

        public ServiceOrderViewModel(NewTest2ExtensionBase _so, List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            Analytics.TrackEvent("Инициализация окна заявки технику",
            new Dictionary<string, string> {
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
            EventsVisible = false;
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
            InfoImage = IconName("info");
            ReorderImage = IconName("reorder");
            EventImage = IconName("event");
            CloseImage = IconName("close");
            TransferImage = IconName("transfer");
            PeopleImage = IconName("people");
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
        private NewServiceorderExtensionBase_ex _ServiceOrderID;
        public NewServiceorderExtensionBase_ex ServiceOrderID {
            get => _ServiceOrderID;
            set {
                _ServiceOrderID = value;
                OnPropertyChanged(nameof(ServiceOrderID));
            }
        }

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
                if (value == DateTime.Parse("01.01.1900 00:00:00"))
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
                if (value == DateTime.Parse("01.01.1900 00:00:00"))
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
                    new Dictionary<string, string> {
                        {"Servicemans",Servicemans.First().NewPhone } });
                //List<MetadataModel> mm = new List<MetadataModel>();
                List<MetadataModel> mm = await ClientHttp.Get<List<MetadataModel>>("/api/Common/metadata?ColumnName=new_category&ObjectName=New_serviceorder");
                if (mm != null && ServiceOrderID.NewCategory != null)
                    Category = mm.FirstOrDefault(x => x.Value == ServiceOrderID.NewCategory).Label;
                //using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                //    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/metadata?ColumnName=new_category&ObjectName=New_serviceorder");
                //    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                //        var resp = response.Content.ReadAsStringAsync().Result;
                //        try {
                //            Analytics.TrackEvent("Попытка десериализации ответа от сервера с категориями техников");
                //            mm = JsonConvert.DeserializeObject<List<MetadataModel>>(resp);
                //        }
                //        catch(Exception ex) {
                //            Crashes.TrackError(new Exception("Ошибка получения списка категорий заявок технику"),
                //            new Dictionary<string,string> {
                //        {"Servicemans",Servicemans.First().NewPhone },
                //        {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                //        {"ErrorMessage",ex.Message },
                //        {"StatusCode",response.StatusCode.ToString() },
                //        {"Response",response.ToString() }
                //            });
                //        }
                //        if(mm != null && ServiceOrderID.NewCategory != null)
                //            Category = mm.FirstOrDefault(x => x.Value == ServiceOrderID.NewCategory).Label;
                //    }
                //}
            });
        }
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Выход с формы заявки технику");
                ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Servicemans, Mounters);
                App.Current.MainPage = new ServiceOrdersPage(vm);
            });
        }
        private RelayCommand _GetInfoByGuardObject;
        public RelayCommand GetInfoByGuardObject {
            get => _GetInfoByGuardObject ??= new RelayCommand(async obj => {
                Opacity = 0.1;
                IndicatorVisible = true;

                //Номера объекта нет, ссылаемся на объект андромеды указанный в заявке, нам нужен номер
                if (!ServiceOrderID.NewNumber.HasValue) {
                    if (ServiceOrderID.NewAndromedaServiceorder.HasValue) {
                        NewAndromedaExtensionBase andromeda = await ClientHttp.Get<NewAndromedaExtensionBase>("/api/NewAndromedaExtensionBases/id?id=" + ServiceOrderID.NewAndromedaServiceorder);
                        ServiceOrderID.NewNumber = andromeda.NewNumber;
                    }
                    //TODO: сообщение что заявка неправильно заполнена
                }
                //if (ServiceOrderFireAlarm.NewNumber.HasValue)
                if (ServiceOrderID.NewNumber != 0) {
                    NewGuardObjectExtensionBase goeb = await ClientHttp.Get<NewGuardObjectExtensionBase>("/api/NewGuardObjectExtensionBases/GetInfoByNumberNew?number=" + ServiceOrderID.NewNumber);
                    //NULL - если может быть ситуация при которой не заведен охраняемый объект
                    if (goeb != null) {
                        Contact = goeb.NewFirstcontact;
                        Siding = goeb.NewSiding;
                        rrOS = goeb.NewRrOs.HasValue ? (bool)goeb.NewRrOs : false;
                        rrPS = goeb.NewRrPs.HasValue ? (bool)goeb.NewRrPs : false;
                        rrVideo = goeb.NewRrVideo.HasValue ? (bool)goeb.NewRrVideo : false;
                        rrAccess = goeb.NewRrSkud.HasValue ? (bool)goeb.NewRrSkud : false;
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
                new Dictionary<string, string> {
                    {"ServiceOrderID",ServiceOrderID.NewServiceorderId.ToString() },
                    {"Serviceman",Servicemans.FirstOrDefault().NewPhone}
                });
                Opacity = 0.1;
                IndicatorVisible = true;
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 20;
                Plugin.Geolocator.Abstractions.Position position;
                if (!CrossGeolocator.Current.IsGeolocationEnabled) {
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Определение местоположения отключено. Отметка \"Пришёл\" не может быть установлена", Color.Red, LayoutOptions.EndAndExpand), 4000));
                    Opacity = 1;
                    IndicatorVisible = false;
                    Intent intent = new Intent(Android.Provider.Settings.ActionLocat‌​ionSourceSettings);
                    intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(intent);
                    return;
                }
                PermissionStatus status = PermissionStatus.Unknown;
                try {
                    status = await CheckAndRequestPermissionAsync(new LocationWhenInUse());
                    if (status == PermissionStatus.Granted) {
                        Location location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
                        if (location == null) {
                            position = await locator.GetPositionAsync();
                            if (position == null) {
                                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Отметка \"Пришёл\" не может быть установлена", Color.Red, LayoutOptions.EndAndExpand), 4000));
                                Opacity = 1;
                                IndicatorVisible = false;
                                return;
                            }
                            Latitude = position.Latitude.ToString();
                            Longitude = position.Longitude.ToString();
                        }
                        if (location != null) {
                            Latitude = location.Latitude.ToString();
                            Longitude = location.Longitude.ToString();
                        }
                    }
                    else {
                        Crashes.TrackError(new Exception("Заявка технику. Ошибка получения координат.Доступ к геопозиции непредоставлен")
                        , new Dictionary<string, string> {
                        {"PermissionStatus",status.ToString()},
                        {"phone",Servicemans.First().NewPhone },
                        {"name",Servicemans.First().NewName }
                    });
                        await Application.Current.MainPage.DisplayAlert("Ошибка", "Отметка о времени прихода, не была записана!", "OK");
                        Opacity = 1;
                        IndicatorVisible = false;
                        return;
                    }
                }
                catch (Exception ex) {
                    Crashes.TrackError(new Exception("Заявка на ПС. Ошибка получения координат.")
                        , new Dictionary<string, string> {
                        {"ErrorMessage",ex.Message },
                        {"PermissionStatus",status.ToString()},
                        {"phone",Servicemans.First().NewPhone },
                        {"name",Servicemans.First().NewName }
                    });
                    Opacity = 1;
                    IndicatorVisible = false;
                    //await Application.Current.MainPage.DisplayAlert("Ошибка","Отметка о времени прихода, не была записана!","OK");
                    MessagePopupPageViewModel vm = new MessagePopupPageViewModel("Ошибка при попытке записи времени прихода", Color.Red, LayoutOptions.EndAndExpand);
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(vm, 4000));
                    return;
                }
                if (status == PermissionStatus.Granted) {
                    Analytics.TrackEvent("Запрос данных на сервере (могло же что-то измениться",
                    new Dictionary<string, string> {
                        {"ServiceOrderID",ServiceOrderID.NewServiceorderId.ToString() }
                    });
                    NewServiceorderExtensionBase soeb = await ClientHttp.Get<NewServiceorderExtensionBase>("/api/NewServiceorderExtensionBases/id?id=" + ServiceOrderID.NewServiceorderId);

                    if (soeb != null) {
                        Analytics.TrackEvent("Попытка записи данных на сервер по объекту заявка технику, заполняем поле Пришел",
                        new Dictionary<string, string> {
                            {"ServiceOrderID",ServiceOrderID.NewServiceorderId.ToString() }
                        });
                        soeb.NewIncome = DateTime.Now.AddHours(-5);


                        using HttpClient clientPut = new HttpClient(GetHttpClientHandler());
                        var httpContent = new StringContent(JsonConvert.SerializeObject(soeb), Encoding.UTF8, "application/json");
                        HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases", httpContent);
                        if (!responsePut.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                            Crashes.TrackError(new Exception("Ошибка при сохранении объекта Заявка технику"),
                            new Dictionary<string, string> {
                        {"ServerResponse",responsePut.Content.ReadAsStringAsync().Result },
                        {"StatusCode",responsePut.StatusCode.ToString() },
                        {"Response",responsePut.ToString() }
                            });
                            //await Application.Current.MainPage.DisplayAlert("Ошибка"
                            //    ,"При попытке сохранения данных произошла ошибка. Повторите попытку позже, в случае если ошибка повторяется, сообщите в IT-отдел."
                            //    ,"OK");
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("От сервера не получена информация о текущей заявке. Повторите попытку позже, в случае если ошибка повторяется, сообщите в IT-отдел.", Color.Red, LayoutOptions.EndAndExpand), 4000));
                        }
                        else {
                            //toast.maketext(android.app.application.context,"время прихода записано",toastlength.long).show();
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Время прихода записано.", Color.Green, LayoutOptions.EndAndExpand), 4000));
                            //await App.Current.MainPage.Navigation.PopAsync(false);
                        }
                    }
                    //запишем координаты
                    Analytics.TrackEvent("Попытка записи координат на сервер по объекту заявка технику",
                        new Dictionary<string, string> {
                                {"ServiceOrderID",ServiceOrderID.NewServiceorderId.ToString() }
                        });
                    if (!string.IsNullOrEmpty(Latitude) && !string.IsNullOrEmpty(Longitude)) {
                        var data = JsonConvert.SerializeObject(new ServiceOrderCoordinates() {
                            SocId = Guid.NewGuid(),
                            SocServiceOrderId = ServiceOrderID.NewServiceorderId,
                            SocIncomeLatitude = Latitude,
                            SocIncomeLongitude = Longitude
                        });
                        //await ClientHttp.PostQuery("/api/ServiceOrderCoordinates",new StringContent(data,Encoding.UTF8,"application/json"));

                        using (HttpClient clientPost = new HttpClient(GetHttpClientHandler())) {

                            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                            HttpResponseMessage responsePost = await clientPost.PostAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates", content);
                            if (!responsePost.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                                Crashes.TrackError(new Exception("Ошибка при сохранении объекта Заявка технику"),
                                new Dictionary<string, string> {
                        {"ServerResponse",responsePost.Content.ReadAsStringAsync().Result },
                        {"StatusCode",responsePost.StatusCode.ToString() },
                        {"Response",responsePost.ToString() }
                                });
                            }
                        }
                    }
                    else {
                        Crashes.TrackError(new Exception("Заявка технику. Пустые координаты"),
                                new Dictionary<string, string> {
                                { "PermissionStatus_StorageRead",CheckAndRequestPermissionAsync(new StorageRead()).Result.ToString() },
                                { "PermissionStatus_LocationWhenInUse",CheckAndRequestPermissionAsync(new LocationWhenInUse()).Result.ToString() },
                                { "PermissionStatus_NetworkState",CheckAndRequestPermissionAsync(new NetworkState()).Result.ToString() },
                                { "PermissionStatus_Permissions.Camera",CheckAndRequestPermissionAsync(new Permissions.Camera()).Result.ToString() },
                                { "PermissionStatus_StorageWrite",CheckAndRequestPermissionAsync(new StorageWrite()).Result.ToString() },
                                { "Phone",Servicemans.First().NewPhone },
                                { "Name",Servicemans.First().NewName }
                                    //{ "ID",ServiceOrderFireAlarm.NewTest2Id.ToString() }
                                });
                    }
                    ServiceOrderID.NewIncome = DateTime.Now.AddHours(-5);
                    IncomeButtonText = ServiceOrderID.NewIncome.Value.AddHours(5).ToShortTimeString();
                    IncomeCommand.ChangeCanExecute();
                    CloseOrderCommand.ChangeCanExecute();
                }

                Opacity = 1;
                IndicatorVisible = false;
            }, obj => ServiceOrderID.NewIncome == null);
        }

        private string _IncomeButtonText;
        public string IncomeButtonText {
            get => _IncomeButtonText;
            set {
                _IncomeButtonText = value;
                OnPropertyChanged(nameof(IncomeButtonText));
            }
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
                    new Dictionary<string, string> {
                        {"ServiceOrderID",ServiceOrderID.NewServiceorderId.ToString() },
                        {"PhoneNumber",obj.ToString() }
                    });
                string phone = null;
                foreach (char c in obj.ToString().ToCharArray())
                    if (char.IsDigit(c))
                        phone += c;
                if (phone != null) {
                    Uri uri = new Uri("tel:" + phone);
                    await Launcher.OpenAsync(uri);
                }
            }, obj => obj != null);
        }
        private RelayCommand _GetObjectNameCommand;
        public RelayCommand GetObjectNameCommand {
            get => _GetObjectNameCommand ??= new RelayCommand(async obj => {
                if (ServiceOrderID.NewNumber.HasValue)
                    if (!string.IsNullOrEmpty(ServiceOrderID.NewNumber.Value.ToString()))
                        if (ServiceOrderID.NewNumber != 0) {
                            List<Info> obj_info = await ClientHttp.Get<List<Info>>("/api/Andromeda/Objinfo?objNumber=" + ServiceOrderID.NewNumber.ToString() + "");
                            if (obj_info != null) {
                                if (obj_info.Count > 0) {
                                    Info inf = obj_info.First();
                                    if (inf != null) {
                                        ObjectName = inf.Name;
                                        ControlTime = inf.ControlTime.ToString();
                                        EventTemplate = inf.EventTemplateName.ToString();
                                        DeviceName = inf.DeviceName.ToString();
                                    }
                                }
                            }
                        }
            });
        }

        private string _ControlTime;
        public string ControlTime {
            get => _ControlTime;
            set {
                _ControlTime = value;
                OnPropertyChanged(nameof(ControlTime));
            }
        }

        private string _DeviceName;
        public string DeviceName {
            get => _DeviceName;
            set {
                _DeviceName = value;
                OnPropertyChanged(nameof(DeviceName));
            }
        }

        private string _EventTemplate;
        public string EventTemplate {
            get => _EventTemplate;
            set {
                _EventTemplate = value;
                OnPropertyChanged(nameof(EventTemplate));
            }
        }

        private string _ObjectName;
        public string ObjectName {
            get => _ObjectName;
            set {
                _ObjectName = value;
                OnPropertyChanged(nameof(ObjectName));
            }
        }
        private RelayCommand _ShowInfoCommand;
        public RelayCommand ShowInfoCommand {
            get => _ShowInfoCommand ??= new RelayCommand(async obj => {
                await Application.Current.MainPage.DisplayAlert("Информация", obj.ToString(), "OK");
            });
        }
        private RelayCommand _CloseOrderCommand;
        public RelayCommand CloseOrderCommand {
            get => _CloseOrderCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Переход на страницу закрытия заявки",
                    new Dictionary<string, string> {
                        {"ServiceOrderID",ServiceOrderID.NewServiceorderId.ToString() }
                    });
                CloseOrderPopupPageViewModel vm = new CloseOrderPopupPageViewModel(ServiceOrderID, Servicemans, Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new CloseOrderPopupPage(vm));
            }, obj => ServiceOrderID.NewIncome != null);
        }
        private RelayCommand _GetObjectInfoCommand;
        public RelayCommand GetObjectInfoCommand {
            get => _GetObjectInfoCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Переход на страницу получения информации об объекте",
                    new Dictionary<string, string> {
                        {"ServiceOrderID",ServiceOrderID.NewServiceorderId.ToString() }
                    });
                ObjectInfoViewModel vm = new ObjectInfoViewModel(ServiceOrderID, Servicemans, Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new ObjectInfoPopup(vm));
            });
        }

        private RelayCommand _GetCustomersCommand;
        public RelayCommand GetCustomersCommand {
            get => _GetCustomersCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Переход на страницу получения информации об ответсвенных лицах объекта",
                    new Dictionary<string, string> {
                        {"ServiceOrderID",ServiceOrderID.NewServiceorderId.ToString() }
                    });
                ObjCustsPopupViewModel vm = new ObjCustsPopupViewModel(ServiceOrderID);
                await App.Current.MainPage.Navigation.PushPopupAsync(new ObjCustsPopupPage(vm));
            });
        }
        private RelayCommand _GetEventsCommand;
        public RelayCommand GetEventsCommand {
            get => _GetEventsCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Переход на страницу получения событий по объекту",
                    new Dictionary<string, string> {
                        {"ServiceOrderID",ServiceOrderID.NewServiceorderId.ToString() }
                    });
                EventsPopupViewModel vm = new EventsPopupViewModel(ServiceOrderID, Servicemans, Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new EventsPopupPage(vm));
            });
        }
        private RelayCommand _ServiceOrderByObjectCommand;
        public RelayCommand ServiceOrderByObjectCommand {
            get => _ServiceOrderByObjectCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Переход на страницу получения прошлых заявок технику",
                    new Dictionary<string, string> {
                        {"ServiceOrderID",ServiceOrderID.NewServiceorderId.ToString() }
                    });
                PastOrdersPopupViewModel vm = new PastOrdersPopupViewModel(ServiceOrderID, Servicemans, Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new PastOrdersPopupPage(vm));
            });
        }

        private RelayCommand _TestCommand;
        public RelayCommand TestCommand {
            get => _TestCommand ??= new RelayCommand(async obj => {
                bool g = false;
            });
        }
    }
}
