using Android.Content;
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
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace MounterApp.ViewModel {
    public class ServiceOrderViewModel : BaseViewModel {
        public ServiceOrderViewModel() { }
        /// <summary>
        /// Конструктор окна заявка технику
        /// </summary>
        /// <param name="_so"></param>
        /// <param name="_servicemans"></param>
        /// <param name="_mounters"></param>
        public ServiceOrderViewModel(NewServiceorderExtensionBase_ex _so, List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            ServiceOrderID = _so;
            ServiceOrderID.NewDate = ServiceOrderID.NewDate.Value.AddHours(5);
            Servicemans = _servicemans;
            Mounters = _mounters;
            OpacityForm = 1;
            //WiresVisible = false;
            //ExtFieldsVisible = false;
            //EventsVisible = false;
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

        //public ServiceOrderViewModel(NewTest2ExtensionBase _so, List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
        //    ServiceOrderFireAlarm = _so;
        //    ServiceOrderFireAlarm.NewDate = ServiceOrderFireAlarm.NewDate.Value.AddHours(5);
        //    Servicemans = _servicemans;
        //    Mounters = _mounters;
        //    OpacityForm = 1;
        //    WiresVisible = false;
        //    ExtFieldsVisible = false;
        //    EventsVisible = false;
        //    App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        //    InfoImage = IconName("info");
        //    ReorderImage = IconName("reorder");
        //    EventImage = IconName("event");
        //    CloseImage = IconName("close");
        //    TransferImage = IconName("transfer");
        //    PeopleImage = IconName("people");
        //}


        //private RelayCommand _NotifyCommand;
        //public RelayCommand NotifyCommand {
        //    get => _NotifyCommand ??= new RelayCommand(async obj => {
        //    });
        //}

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
        ///// <summary>
        ///// Коллекция шлейфов
        ///// </summary>
        //private ObservableCollection<Wires> _WiresCollection = new ObservableCollection<Wires>();
        //public ObservableCollection<Wires> WiresCollection {
        //    get => _WiresCollection;
        //    set {
        //        _WiresCollection = value;
        //        OnPropertyChanged(nameof(WiresCollection));
        //    }
        //}
        ///// <summary>
        ///// Коллекция дополнительных полей в объекте Андромеды
        ///// </summary>
        //private ObservableCollection<ExtFields> _ExtFields = new ObservableCollection<ExtFields>();
        //public ObservableCollection<ExtFields> ExtFields {
        //    get => _ExtFields;
        //    set {
        //        _ExtFields = value;
        //        OnPropertyChanged(nameof(ExtFields));
        //    }
        //}

        //private ObservableCollection<GetEventsReceivedFromObject_Result> _Events = new ObservableCollection<GetEventsReceivedFromObject_Result>();
        //public ObservableCollection<GetEventsReceivedFromObject_Result> Events {
        //    get => _Events;
        //    set {
        //        _Events = value;
        //        OnPropertyChanged(nameof(Events));
        //    }
        //}
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

        //private NewTest2ExtensionBase _ServiceOrderFireAlarm;
        //public NewTest2ExtensionBase ServiceOrderFireAlarm {
        //    get => _ServiceOrderFireAlarm;
        //    set {
        //        _ServiceOrderFireAlarm = value;
        //        OnPropertyChanged(nameof(ServiceOrderFireAlarm));
        //    }
        //}
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
        //private DateTime _DateStart;
        //public DateTime DateStart {
        //    get => _DateStart;
        //    set {
        //        if (value == DateTime.Parse("01.01.1900 00:00:00")) {
        //            _DateStart = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy")).AddDays(-1);
        //        }
        //        else {
        //            _DateStart = value;
        //        }

        //        OnPropertyChanged(nameof(DateStart));
        //    }
        //}
        //private DateTime _DateEnd;
        //public DateTime DateEnd {
        //    get => _DateEnd;
        //    set {
        //        if (value == DateTime.Parse("01.01.1900 00:00:00")) {
        //            _DateEnd = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
        //        }
        //        else {
        //            _DateEnd = value;
        //        }

        //        OnPropertyChanged(nameof(DateEnd));
        //    }
        //}
        //private bool _EventsVisible;
        //public bool EventsVisible {
        //    get => _EventsVisible;
        //    set {
        //        _EventsVisible = value;
        //        OnPropertyChanged(nameof(EventsVisible));
        //    }
        //}
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
                List<MetadataModel> mm = await ClientHttp.Get<List<MetadataModel>>("/api/Common/metadata?ColumnName=new_category&ObjectName=New_serviceorder");
                if (mm != null && ServiceOrderID.NewCategory != null) 
                    Category = mm.FirstOrDefault(x => x.Value == ServiceOrderID.NewCategory).Label;                
            });
        }
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                //Analytics.TrackEvent("Выход с формы заявки технику");
                ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Servicemans, Mounters, false);
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
                }
                if (ServiceOrderID.NewNumber != 0) {
                    NewGuardObjectExtensionBase goeb = await ClientHttp.Get<NewGuardObjectExtensionBase>("/api/NewGuardObjectExtensionBases/GetInfoByNumberNew?number=" + ServiceOrderID.NewNumber);
                    if (goeb != null) {
                        Contact = goeb.NewFirstcontact;
                        Siding = goeb.NewSiding;
                        rrOS = goeb.NewRrOs.HasValue && (bool)goeb.NewRrOs;
                        rrPS = goeb.NewRrPs.HasValue && (bool)goeb.NewRrPs;
                        rrVideo = goeb.NewRrVideo.HasValue && (bool)goeb.NewRrVideo;
                        rrAccess = goeb.NewRrSkud.HasValue && (bool)goeb.NewRrSkud;
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
                        await Application.Current.MainPage.DisplayAlert("Ошибка", "Отметка о времени прихода, не была записана!", "OK");
                        Opacity = 1;
                        IndicatorVisible = false;
                        return;
                    }
                }
                catch (Exception) {
                    Opacity = 1;
                    IndicatorVisible = false;
                    MessagePopupPageViewModel vm = new MessagePopupPageViewModel("Ошибка при попытке записи времени прихода", Color.Red, LayoutOptions.EndAndExpand);
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(vm, 4000));
                    return;
                }
                if (status == PermissionStatus.Granted) {
                    NewServiceorderExtensionBase soeb = await ClientHttp.Get<NewServiceorderExtensionBase>("/api/NewServiceorderExtensionBases/id?id=" + ServiceOrderID.NewServiceorderId);

                    if (soeb != null) {
                        soeb.NewIncome = DateTime.Now.AddHours(-5);
                        var httpContent = new StringContent(JsonConvert.SerializeObject(soeb), Encoding.UTF8, "application/json");
                        var result = await ClientHttp.Put("/api/NewServiceorderExtensionBases", httpContent);
                        if (!result.Equals(System.Net.HttpStatusCode.Accepted)) 
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("От сервера не получена информация о текущей заявке. Повторите попытку позже, в случае если ошибка повторяется, сообщите в IT-отдел.", Color.Red, LayoutOptions.EndAndExpand), 4000));
                        else 
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Время прихода записано.", Color.Green, LayoutOptions.EndAndExpand), 4000));
                    }
                    if (!string.IsNullOrEmpty(Latitude) && !string.IsNullOrEmpty(Longitude)) {
                        var data = JsonConvert.SerializeObject(new ServiceOrderCoordinates() {
                            SocId = Guid.NewGuid(),
                            SocServiceOrderId = ServiceOrderID.NewServiceorderId,
                            SocIncomeLatitude = Latitude,
                            SocIncomeLongitude = Longitude
                        });
                        await ClientHttp.PostStateCode("/api/ServiceOrderCoordinates", new StringContent(data, Encoding.UTF8, "application/json"));
                    }
                    else {
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
                if (ServiceOrderID.NewNumber.HasValue) {
                    if (!string.IsNullOrEmpty(ServiceOrderID.NewNumber.Value.ToString())) {
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
                CloseOrderPopupPageViewModel vm = new CloseOrderPopupPageViewModel(ServiceOrderID, Servicemans, Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new CloseOrderPopupPage(vm));
            }, obj => ServiceOrderID.NewIncome != null);
        }
        private RelayCommand _GetObjectInfoCommand;
        public RelayCommand GetObjectInfoCommand {
            get => _GetObjectInfoCommand ??= new RelayCommand(async obj => {
                ObjectInfoViewModel vm = new ObjectInfoViewModel(ServiceOrderID, Servicemans, Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new ObjectInfoPopup(vm));
            });
        }

        private RelayCommand _GetCustomersCommand;
        public RelayCommand GetCustomersCommand {
            get => _GetCustomersCommand ??= new RelayCommand(async obj => {
                ObjCustsPopupViewModel vm = new ObjCustsPopupViewModel(ServiceOrderID);
                await App.Current.MainPage.Navigation.PushPopupAsync(new ObjCustsPopupPage(vm));
            });
        }
        private RelayCommand _GetEventsCommand;
        public RelayCommand GetEventsCommand {
            get => _GetEventsCommand ??= new RelayCommand(async obj => {
                EventsPopupViewModel vm = new EventsPopupViewModel(ServiceOrderID, Servicemans, Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new EventsPopupPage(vm));
            });
        }
        private RelayCommand _ServiceOrderByObjectCommand;
        public RelayCommand ServiceOrderByObjectCommand {
            get => _ServiceOrderByObjectCommand ??= new RelayCommand(async obj => {
                PastOrdersPopupViewModel vm = new PastOrdersPopupViewModel(ServiceOrderID, Servicemans, Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new PastOrdersPopupPage(vm));
            });
        }
    }
}
