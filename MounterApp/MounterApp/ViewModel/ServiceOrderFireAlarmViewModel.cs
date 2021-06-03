using Android.Content;
using Android.Widget;
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
    public class ServiceOrderFireAlarmViewModel : BaseViewModel {
        public ServiceOrderFireAlarmViewModel() { }
        /// <summary>
        /// Конструктор страницы заявок на ПС
        /// </summary>
        /// <param name="_so">Заявка на ПС</param>
        /// <param name="_servicemans">Список техников</param>
        /// <param name="_mounters">Список монтажников</param>
        public ServiceOrderFireAlarmViewModel(NewTest2ExtensionBase_ex _so, List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            ServiceOrderFireAlarm = _so;
            ServiceOrderFireAlarm.NewDate = ServiceOrderFireAlarm.NewDate.Value.AddHours(5);
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
        }
        /// <summary>
        /// Получаем наименование объекта из Андромеды
        /// Дополнительно получаем. Контрольное время, Шаблон событий, наименование устройства установленного на объекте
        /// </summary>
        private RelayCommand _GetObjectNameCommand;
        public RelayCommand GetObjectNameCommand {
            get => _GetObjectNameCommand ??= new RelayCommand(async obj => {
                if (ServiceOrderFireAlarm.NewNumber.HasValue) {
                    if (ServiceOrderFireAlarm.NewNumber != 0) {
                        if (!string.IsNullOrEmpty(ServiceOrderFireAlarm.NewNumber.Value.ToString())) {
                            List<Info> obj_info = await ClientHttp.Get<List<Info>>("/api/Andromeda/Objinfo?objNumber=" + ServiceOrderFireAlarm.NewNumber.ToString() + "");
                            if (obj_info != null) {
                                if (obj_info.Count() > 0) {
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
        /// <summary>
        /// Наименование устройства установленного на объекте
        /// </summary>
        private string _DeviceName;
        public string DeviceName {
            get => _DeviceName;
            set {
                _DeviceName = value;
                OnPropertyChanged(nameof(DeviceName));
            }
        }
        /// <summary>
        /// Шаблон событий
        /// </summary>
        private string _EventTemplate;
        public string EventTemplate {
            get => _EventTemplate;
            set {
                _EventTemplate = value;
                OnPropertyChanged(nameof(EventTemplate));
            }
        }
        /// <summary>
        /// Контрольное время
        /// </summary>
        private string _ControlTime;
        public string ControlTime {
            get => _ControlTime;
            set {
                _ControlTime = value;
                OnPropertyChanged(nameof(ControlTime));
            }
        }
        /// <summary>
        /// Наименование объекта
        /// </summary>
        private string _ObjectName;
        public string ObjectName {
            get => _ObjectName;
            set {
                _ObjectName = value;
                OnPropertyChanged(nameof(ObjectName));
            }
        }
        /// <summary>
        /// Команда показа информации. Параметром передаем значение.
        /// Требуется для полноэкранного просмотра. 
        /// Например поодъездные пути не всегда вмещаются на экран.
        /// </summary>
        private RelayCommand _ShowInfoCommand;
        public RelayCommand ShowInfoCommand {
            get => _ShowInfoCommand ??= new RelayCommand(async obj => {
                await Application.Current.MainPage.DisplayAlert("Информация", obj.ToString(), "OK");
            });
        }
        /// <summary>
        /// иконка информации
        /// </summary>
        private ImageSource _InfoImage;
        public ImageSource InfoImage {
            get => _InfoImage;
            set {
                _InfoImage = value;
                OnPropertyChanged(nameof(InfoImage));
            }
        }
        /// <summary>
        /// Иконка прошлых заявок технику
        /// </summary>
        private ImageSource _ReorderImage;
        public ImageSource ReorderImage {
            get => _ReorderImage;
            set {
                _ReorderImage = value;
                OnPropertyChanged(nameof(ReorderImage));
            }
        }
        /// <summary>
        /// Икона событий
        /// </summary>
        private ImageSource _EventImage;
        public ImageSource EventImage {
            get => _EventImage;
            set {
                _EventImage = value;
                OnPropertyChanged(nameof(EventImage));
            }
        }
        /// <summary>
        /// Иконка закрытия заявки
        /// </summary>
        private ImageSource _CloseImage;
        public ImageSource CloseImage {
            get => _CloseImage;
            set {
                _CloseImage = value;
                OnPropertyChanged(nameof(CloseImage));
            }
        }
        /// <summary>
        /// Икона для кнопки пришел
        /// </summary>
        private ImageSource _TransferImage;
        public ImageSource TransferImage {
            get => _TransferImage;
            set {
                _TransferImage = value;
                OnPropertyChanged(nameof(TransferImage));
            }
        }
        /// <summary>
        /// Список монтажников
        /// </summary>
        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
        }
        ///// <summary>
        ///// Список шлефов
        ///// </summary>
        //private ObservableCollection<Wires> _WiresCollection = new ObservableCollection<Wires>();
        //public ObservableCollection<Wires> WiresCollection {
        //    get => _WiresCollection;
        //    set {
        //        _WiresCollection = value;
        //        OnPropertyChanged(nameof(WiresCollection));
        //    }
        //}
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
        /// <summary>
        /// Прозрачность формы
        /// </summary>
        private double _OpacityForm;
        public double OpacityForm {
            get => _OpacityForm;
            set {
                _OpacityForm = value;
                OnPropertyChanged(nameof(OpacityForm));
            }
        }
        /// <summary>
        /// Список техников
        /// </summary>
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
        /// <summary>
        /// Объект Заявка на ПС
        /// </summary>
        private NewTest2ExtensionBase_ex _ServiceOrderFireAlarm;
        public NewTest2ExtensionBase_ex ServiceOrderFireAlarm {
            get => _ServiceOrderFireAlarm;
            set {
                _ServiceOrderFireAlarm = value;
                OnPropertyChanged(nameof(ServiceOrderFireAlarm));
            }
        }
        /// <summary>
        /// Подъездные пути
        /// </summary>
        private string _Siding;
        public string Siding {
            get => _Siding;
            set {
                _Siding = value;
                OnPropertyChanged(nameof(Siding));
            }
        }
        /// <summary>
        /// Основной контакт
        /// </summary>
        private string _Contact;
        public string Contact {
            get => _Contact;
            set {
                _Contact = value;
                OnPropertyChanged(nameof(Contact));
            }
        }
        /// <summary>
        /// Регламентые работы по ОС
        /// </summary>
        private bool _rrOS;
        public bool rrOS {
            get => _rrOS;
            set {
                _rrOS = value;
                OnPropertyChanged(nameof(rrOS));
            }
        }
        /// <summary>
        /// Регламентные работы по ПС
        /// </summary>
        private bool _rrPS;
        public bool rrPS {
            get => _rrPS;
            set {
                _rrPS = value;
                OnPropertyChanged(nameof(rrPS));
            }
        }
        /// <summary>
        /// Регламентные работы по видео
        /// </summary>
        private bool _rrVideo;
        public bool rrVideo {
            get => _rrVideo;
            set {
                _rrVideo = value;
                OnPropertyChanged(nameof(rrVideo));
            }
        }
        /// <summary>
        /// Регламентные работы по скуду
        /// </summary>
        private bool _rrAccess;
        public bool rrAccess {
            get => _rrAccess;
            set {
                _rrAccess = value;
                OnPropertyChanged(nameof(rrAccess));
            }
        }
        /// <summary>
        /// Категория заявки
        /// </summary>
        private string _Category;
        public string Category {
            get => _Category;
            set {
                _Category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        /// <summary>
        /// Видимость индикатора загрузки
        /// </summary>
        private bool _IndicatorVisible;
        public bool IndicatorVisible {
            get => _IndicatorVisible;
            set {
                _IndicatorVisible = value;
                OnPropertyChanged(nameof(IndicatorVisible));
            }
        }
        /// <summary>
        /// Прозрачность формы
        /// </summary>
        private double _Opacity;
        public double Opacity {
            get => _Opacity;
            set {
                _Opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }
        //private bool _WiresVisible;
        //public bool WiresVisible {
        //    get => _WiresVisible;
        //    set {
        //        _WiresVisible = value;
        //        OnPropertyChanged(nameof(WiresVisible));
        //    }
        //}
        //private bool _ExtFieldsVisible;
        //public bool ExtFieldsVisible {
        //    get => _ExtFieldsVisible;
        //    set {
        //        _ExtFieldsVisible = value;
        //        OnPropertyChanged(nameof(ExtFieldsVisible));
        //    }
        //}
        //private DateTime _DateStart;
        //public DateTime DateStart {
        //    get => _DateStart;
        //    set {
        //        if (value == DateTime.Parse("01.01.1900 00:00:00")) 
        //            _DateStart = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy")).AddDays(-1);                
        //        else 
        //            _DateStart = value;
        //        OnPropertyChanged(nameof(DateStart));
        //    }
        //}
        //private DateTime _DateEnd;
        //public DateTime DateEnd {
        //    get => _DateEnd;
        //    set {
        //        if (value == DateTime.Parse("01.01.1900 00:00:00")) 
        //            _DateEnd = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
        //        else 
        //            _DateEnd = value;
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
        /// <summary>
        /// Широта
        /// </summary>
        private string _Latitude;
        public string Latitude {
            get => _Latitude;
            set {
                _Latitude = value;
                OnPropertyChanged(nameof(Latitude));
            }
        }
        /// <summary>
        /// Долгота
        /// </summary>
        private string _Longitude;
        public string Longitude {
            get => _Longitude;
            set {
                _Longitude = value;
                OnPropertyChanged(nameof(Longitude));
            }
        }
        /// <summary>
        /// Получение списка категорий 
        /// </summary>
        private RelayCommand _GetCategory;
        public RelayCommand GetCategory {
            get => _GetCategory ??= new RelayCommand(async obj => {
                List<MetadataModel> mm = await ClientHttp.Get<List<MetadataModel>>("/api/Common/metadata?ColumnName=new_category&ObjectName=New_test2");
                if (mm != null && ServiceOrderFireAlarm.NewCategory != null) 
                    Category = mm.First(x => x.Value == ServiceOrderFireAlarm.NewCategory).Label;
            });
        }
        /// <summary>
        /// Выход с формы
        /// </summary>
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Servicemans, Mounters, false);
                App.Current.MainPage = new ServiceOrdersPage(vm);
            });
        }
        /// <summary>
        /// Получение информации об объекте
        /// </summary>
        private RelayCommand _GetInfoByGuardObject;
        public RelayCommand GetInfoByGuardObject {
            get => _GetInfoByGuardObject ??= new RelayCommand(async obj => {
                Opacity = 0.1;
                IndicatorVisible = true;

                //Если нет номера объекта в заявке, то получаем номер объекта из объекта андромеды
                if (!ServiceOrderFireAlarm.NewNumber.HasValue) {
                    if (ServiceOrderFireAlarm.NewAndromedaServiceorder.HasValue) {
                        NewAndromedaExtensionBase andromeda = await ClientHttp.Get<NewAndromedaExtensionBase>("/api/NewAndromedaExtensionBases/id?id=" + ServiceOrderFireAlarm.NewAndromedaServiceorder);
                        ServiceOrderFireAlarm.NewNumber = andromeda.NewNumber;
                    }
                }
                if (ServiceOrderFireAlarm.NewNumber.HasValue) {
                    if (ServiceOrderFireAlarm.NewNumber != 0) {
                        NewGuardObjectExtensionBase goeb = await ClientHttp.Get<NewGuardObjectExtensionBase>("/api/NewGuardObjectExtensionBases/GetInfoByNumberNew?number=" + ServiceOrderFireAlarm.NewNumber);
                        if (goeb != null) {
                            Contact = goeb.NewFirstcontact;
                            Siding = goeb.NewSiding;
                            rrOS = goeb.NewRrOs.HasValue ? (bool)goeb.NewRrOs : false;
                            rrPS = goeb.NewRrPs.HasValue ? (bool)goeb.NewRrPs : false;
                            rrVideo = goeb.NewRrVideo.HasValue ? (bool)goeb.NewRrVideo : false;
                            rrAccess = goeb.NewRrSkud.HasValue ? (bool)goeb.NewRrSkud : false;
                        }
                    }
                }
                Opacity = 1;
                IndicatorVisible = false;
            });
        }
        /// <summary>
        /// Отметка времени пришел
        /// </summary>
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
                }
                catch (Exception) {}
                if (status == PermissionStatus.Granted) {
                    NewTest2ExtensionBase soeb = await ClientHttp.Get<NewTest2ExtensionBase>("/api/NewServiceOrderForFireAlarmExtensionBase/id?id=" + ServiceOrderFireAlarm.NewTest2Id);
                    if (soeb != null) {
                        soeb.NewIncome = DateTime.Now.AddHours(-5);
                        var httpContent = new StringContent(JsonConvert.SerializeObject(soeb), Encoding.UTF8, "application/json");
                        var result = await ClientHttp.Put("/api/NewServiceOrderForFireAlarmExtensionBase", httpContent);
                        if (!result.Equals(System.Net.HttpStatusCode.Accepted))
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("При попытке сохранения данных произошла ошибка. Повторите попытку позже", Color.Red, LayoutOptions.EndAndExpand), 4000));
                        else
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Отметка времени прихода сохранена", Color.Green, LayoutOptions.EndAndExpand), 4000));
                    }
                    if (!string.IsNullOrEmpty(Latitude) && !string.IsNullOrEmpty(Longitude)) {
                        var data = JsonConvert.SerializeObject(new ServiceOrderCoordinates() {
                            SocId = Guid.NewGuid(),
                            SocServiceOrderId = ServiceOrderFireAlarm.NewTest2Id,
                            SocIncomeLatitude = Latitude,
                            SocIncomeLongitude = Longitude
                        });
                        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                        await ClientHttp.PostStateCode("/api/ServiceOrderCoordinates", content);
                    }
                    ServiceOrderFireAlarm.NewIncome = DateTime.Now.AddHours(-5);
                    IncomeCommand.ChangeCanExecute();
                }
                Opacity = 1;
                IndicatorVisible = false;
            }, obj => ServiceOrderFireAlarm.NewIncome == null);
        }
        /// <summary>
        /// Иконка ответсвенных лиц
        /// </summary>
        private ImageSource _PeopleImage;
        public ImageSource PeopleImage {
            get => _PeopleImage;
            set {
                _PeopleImage = value;
                OnPropertyChanged(nameof(PeopleImage));
            }
        }
        /// <summary>
        /// Звонок основному контакту
        /// </summary>
        private RelayCommand _CallClientCommand;
        public RelayCommand CallClientCommand {
            get => _CallClientCommand ??= new RelayCommand(async obj => {
                if (obj != null) {
                    if (string.IsNullOrEmpty(obj.ToString())) {
                        Uri uri = new Uri("tel:" + obj);
                        await Launcher.OpenAsync(uri);
                    }
                    else 
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Не указан номер телефона", Color.Red, LayoutOptions.EndAndExpand), 4000));
                }
                else 
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Не указан номер телефона", Color.Red, LayoutOptions.EndAndExpand), 4000));
            });
        }
        /// <summary>
        /// закрытие заявки
        /// </summary>
        private RelayCommand _CloseOrderCommand;
        public RelayCommand CloseOrderCommand {
            get => _CloseOrderCommand ??= new RelayCommand(async obj => {
                CloseOrderPopupPageViewModel vm = new CloseOrderPopupPageViewModel(ServiceOrderFireAlarm, Servicemans, Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new CloseOrderPopupPage(vm));
            });
        }
        /// <summary>
        /// Переход к форме получения информации по объекту
        /// </summary>
        private RelayCommand _GetObjectInfoCommand;
        public RelayCommand GetObjectInfoCommand {
            get => _GetObjectInfoCommand ??= new RelayCommand(async obj => {
                ObjectInfoViewModel vm = new ObjectInfoViewModel(ServiceOrderFireAlarm, Servicemans, Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new ObjectInfoPopup(vm));
            });
        }
        /// <summary>
        /// Переход к форме пользователей объекта
        /// </summary>
        private RelayCommand _GetCustomersCommand;
        public RelayCommand GetCustomersCommand {
            get => _GetCustomersCommand ??= new RelayCommand(async obj => {
                ObjCustsPopupViewModel vm = new ObjCustsPopupViewModel(ServiceOrderFireAlarm);
                await App.Current.MainPage.Navigation.PushPopupAsync(new ObjCustsPopupPage(vm));
            });
        }
        /// <summary>
        /// Переход к форме событий
        /// </summary>
        private RelayCommand _GetEventsCommand;
        public RelayCommand GetEventsCommand {
            get => _GetEventsCommand ??= new RelayCommand(async obj => {
                EventsPopupViewModel vm = new EventsPopupViewModel(ServiceOrderFireAlarm, Servicemans, Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new EventsPopupPage(vm));
            });
        }
        /// <summary>
        /// Переход к форме прошлых заявок
        /// </summary>
        private RelayCommand _ServiceOrderByObjectCommand;
        public RelayCommand ServiceOrderByObjectCommand {
            get => _ServiceOrderByObjectCommand ??= new RelayCommand(async obj => {
                PastOrdersPopupViewModel vm = new PastOrdersPopupViewModel(ServiceOrderFireAlarm, Servicemans, Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new PastOrdersPopupPage(vm));
            });
        }
    }
}
