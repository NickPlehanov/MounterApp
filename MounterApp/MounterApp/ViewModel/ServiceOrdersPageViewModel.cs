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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class ServiceOrdersPageViewModel : BaseViewModel {
        //readonly ClientHttp http = new ClientHttp();
        //public ServiceOrdersPageViewModel() {

        //}
        public ServiceOrdersPageViewModel(List<NewServicemanExtensionBase> _servicemans,List<NewMounterExtensionBase> _mounters) {
            Category.Clear();
            ServiceOrders.Clear();
            ServiceOrdersByTime.Clear();
            ServiceOrderByTransfer.Clear();
            ServiceOrdersFireAlarm.Clear();
            ServiceOrderByTransferFireAlarm.Clear();
            ServiceOrdersByTimeFireAlarm.Clear();

            if(Date == DateTime.Parse("01.01.0001 00:00:00"))
                Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
            Servicemans = _servicemans;
            Mounters = _mounters;
            OpacityForm = 1;
            IndicatorVisible = false;
            RefreshOrdersCommand.Execute(null);
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
            Analytics.TrackEvent("Инициализация страницы заявок технику",
            new Dictionary<string,string> {
                {"Serviceman",Servicemans.FirstOrDefault().NewPhone }
            });
            Width = DeviceDisplay.MainDisplayInfo.Width - 10;
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
            RefreshImage = IconName("refresh");
            MapImage = IconName("map");
            TransferImage = IconName("transfer");
            HelpImage = IconName("help");
            FrameColor = Color.Red;
            ArrowCircleServiceOrder = IconName("arrow_circle_down");
            ArrowCircleTransferServiceOrder = IconName("arrow_circle_down");
            ArrowCircleTimeServiceOrder = IconName("arrow_circle_down");
            ArrowCircleOtherServiceOrder = IconName("arrow_circle_down");
            ArrowCircleFireAlarmServiceOrder = IconName("arrow_circle_down");
            ArrowCircleFireAlarmTransferServiceOrder = IconName("arrow_circle_down");
            ArrowCircleFireAlarmTimeServiceOrder = IconName("arrow_circle_down");
            ArrowCircleFireAlarmOtherServiceOrder = IconName("arrow_circle_down");
            TransferServiceOrder = "Перенесенные (0)";
            FireAlarmTransferServiceOrderText = "Перенесенные(пс) (0)";
            TimeServiceOrder = "Временные (0)";
            FireAlarmTimeServiceOrderText = "Временные(пс) (0)";
            OtherServiceOrder = "Прочие (0)";
            FireAlarmOtherServiceOrderText = "Прочие(пс) (0)";

            TransferServiceOrderVisible = false;
            TimeServiceOrderVisible = false;
            OtherServiceOrderVisible = false;
            FireAlarmTransferServiceOrderVisible = false;
            FireAlarmTimeServiceOrderVisible = false;
            FireAlarmOtherServiceOrderVisible = false;
            if(Application.Current.Properties.ContainsKey("AutoUpdateTime"))
                AutoUpdateTime = double.Parse(Application.Current.Properties["AutoUpdateTime"].ToString());
            else
                AutoUpdateTime = null;
            AutoUpdateOrdersCommand.Execute(AutoUpdateTime);
        }


        private double? _AutoUpdateTime;
        public double? AutoUpdateTime {
            get => _AutoUpdateTime;
            set {
                _AutoUpdateTime = value;
                OnPropertyChanged(nameof(AutoUpdateTime));
            }
        }
        private RelayCommand _AutoUpdateOrdersCommand;
        public RelayCommand AutoUpdateOrdersCommand {
            get => _AutoUpdateOrdersCommand ??= new RelayCommand(async obj => {
                #region Данный код прекрасно мог бы обновлять заявки в фоне, но иногда он крашится, из-за коллекции
                if(obj != null)
                    if(double.Parse(obj.ToString()) > 0) {
                        Device.StartTimer(TimeSpan.FromMinutes(Convert.ToDouble(obj)),() => {
                            Task.Run(async () => {
                                try {
                                    if(ServiceOrdersByTime != null)
                                        if(ServiceOrdersByTime.Any()) {
                                            int count_time = ServiceOrdersByTime.Count;
                                        }
                                    if(ServiceOrders != null)
                                        if(ServiceOrders.Any()) {
                                            int count_ordr = ServiceOrders.Count;
                                        }
                                    if(ServiceOrderByTransfer != null)
                                        if(ServiceOrderByTransfer.Any()) {
                                            int count_transfer = ServiceOrderByTransfer.Count;
                                        }
                                    FireAlarmOtherServiceOrderExpanded = true;
                                    FireAlarmServiceOrderExpanded = true;
                                    FireAlarmTimeServiceOrderExpanded = true;
                                    FireAlarmTransferServiceOrderExpanded = true;
                                    OtherServiceOrderExpanded = true;
                                    ServiceOrderExpanded = true;
                                    TimeServiceOrderExpanded = true;
                                    TransferServiceOrderExpanded = true;
                                    GetServiceOrders.Execute(Servicemans);
                                    GetServiceOrderByTransfer.Execute(Servicemans);
                                    GetServiceOrdersFireAlarm.Execute(Servicemans);
                                    GetServiceOrderByTransferFireAlarm.Execute(Servicemans);
                                }
                                catch { }
                                //if (count_time< ServiceOrdersByTime.Count || count_ordr< ServiceOrders.Count || count_transfer< ServiceOrderByTransfer.Count) { }
                            });
                            return true; //use this to run continuously 
                                         //return false; //to stop running continuously 

                        });
                    }
                #endregion
            });
        }
        private ImageSource _HelpImage;
        public ImageSource HelpImage {
            get => _HelpImage;
            set {
                _HelpImage = value;
                OnPropertyChanged(nameof(HelpImage));
            }
        }
        private double _Width;
        public double Width {
            get => _Width;
            set {
                _Width = value;
                OnPropertyChanged(nameof(Width));
            }
        }
        private bool _TransferServiceOrderVisible;
        public bool TransferServiceOrderVisible {
            get => _TransferServiceOrderVisible;
            set {
                _TransferServiceOrderVisible = value;
                OnPropertyChanged(nameof(TransferServiceOrderVisible));
            }
        }

        private bool _TimeServiceOrderVisible;
        public bool TimeServiceOrderVisible {
            get => _TimeServiceOrderVisible;
            set {
                _TimeServiceOrderVisible = value;
                OnPropertyChanged(nameof(TimeServiceOrderVisible));
            }
        }

        private bool _OtherServiceOrderVisible;
        public bool OtherServiceOrderVisible {
            get => _OtherServiceOrderVisible;
            set {
                _OtherServiceOrderVisible = value;
                OnPropertyChanged(nameof(OtherServiceOrderVisible));
            }
        }

        private bool _FireAlarmTransferServiceOrderVisible;
        public bool FireAlarmTransferServiceOrderVisible {
            get => _FireAlarmTransferServiceOrderVisible;
            set {
                _FireAlarmTransferServiceOrderVisible = value;
                OnPropertyChanged(nameof(FireAlarmTransferServiceOrderVisible));
            }
        }

        private bool _FireAlarmTimeServiceOrderVisible;
        public bool FireAlarmTimeServiceOrderVisible {
            get => _FireAlarmTimeServiceOrderVisible;
            set {
                _FireAlarmTimeServiceOrderVisible = value;
                OnPropertyChanged(nameof(FireAlarmTimeServiceOrderVisible));
            }
        }

        private bool _FireAlarmOtherServiceOrderVisible;
        public bool FireAlarmOtherServiceOrderVisible {
            get => _FireAlarmOtherServiceOrderVisible;
            set {
                _FireAlarmOtherServiceOrderVisible = value;
                OnPropertyChanged(nameof(FireAlarmOtherServiceOrderVisible));
            }
        }
        private ImageSource _RefreshImage;
        public ImageSource RefreshImage {
            get => _RefreshImage;
            set {
                _RefreshImage = value;
                OnPropertyChanged(nameof(RefreshImage));
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

        //private RelayCommand _IncomeCommand;
        //public RelayCommand IncomeCommand {
        //    get => _IncomeCommand ??= new RelayCommand(async obj => {
        //    });
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
        private List<NewServicemanExtensionBase> _Servicemans = new List<NewServicemanExtensionBase>();
        public List<NewServicemanExtensionBase> Servicemans {
            get => _Servicemans;
            set {
                _Servicemans = value;
                OnPropertyChanged(nameof(Servicemans));
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
        private DateTime _Date;
        public DateTime Date {
            get => _Date;
            set {
                if(value == DateTime.Parse("01.01.0001 00:00:00"))
                    _Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
                else
                    _Date = value;
                GetServiceOrders.Execute(null);
                GetServiceOrderByTransfer.Execute(null);
                OnPropertyChanged(nameof(Date));
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


        private RelayCommand _HelpCommand;
        public RelayCommand HelpCommand {
            get => _HelpCommand ??= new RelayCommand(async obj => {
                string msg = " - Цвет рамки вокруг заявки, отражает от кого была получена заявка: Красный - ВИП-клиент, Желтый - клиент, Синий - сотрудник" + Environment.NewLine + Environment.NewLine +
                " - Протаскивание заявки слева направо (свайп) позволяет построить маршрут до объекта, от Вашего местоположения" + Environment.NewLine + Environment.NewLine +
                " - Если Вы видите только что закрытую заявку, попробуйте обновить страницу" + Environment.NewLine + Environment.NewLine +
                " - В информации об ответсвенных лицах - свайп вправо позволяет позвонить по номеру указанному на карточке" + Environment.NewLine + Environment.NewLine +
                " - В прошлых заявках по объекту - свайп вправо позволяет позвонить технику закрывшему заявку" + Environment.NewLine + Environment.NewLine +
                " - В общей информации по заявкам - нажатия на такие поля как Название, Адрес, Подездные пути, Причина, Примечание - открывают дополнительное окно для просмотра информации" + Environment.NewLine + Environment.NewLine
                ;
                HelpPopupViewModel vm = new HelpPopupViewModel(msg);
                await App.Current.MainPage.Navigation.PushPopupAsync(new HelpPopupPage(vm));
            });
        }
        private RelayCommand _GetCategoryTech;
        public RelayCommand GetCategoryTech {
            get => _GetCategoryTech ??= new RelayCommand(async obj => {
                //List<MetadataModel> mm = new List<MetadataModel>();
                Category = await ClientHttp.Get<ObservableCollection<MetadataModel>>("/api/Common/metadata?ColumnName=new_category&ObjectName=New_serviceman");

                Analytics.TrackEvent("Получение категорий техников",
                new Dictionary<string,string> {
                    {"Query","Common/metadata?ColumnName=new_category&ObjectName=New_serviceman" }
                });
                //using HttpClient httpClient = new HttpClient(GetHttpClientHandler());
                //HttpResponseMessage httpResponse = await httpClient.GetAsync(Resources.BaseAddress + "/api/Common/metadata?ColumnName=new_category&ObjectName=New_serviceman");
                //List<MetadataModel> mm = new List<MetadataModel>();
                //if(httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                //    var resp = httpResponse.Content.ReadAsStringAsync().Result;
                //    try {
                //        mm = JsonConvert.DeserializeObject<List<MetadataModel>>(resp);
                //    }
                //    catch(Exception ex) {
                //        mm = null;
                //        Crashes.TrackError(new Exception("Ошибка десериализации категорий техников"),
                //        new Dictionary<string,string> {
                //            {"ServerResponse",httpResponse.Content.ReadAsStringAsync().Result },
                //            {"ErrorMessage",ex.Message },
                //            {"StatusCode",httpResponse.StatusCode.ToString() },
                //            {"Response",httpResponse.ToString() },
                //            {"Query","Common/metadata?ColumnName=new_category&ObjectName=New_serviceman" }
                //        });
                //    }
                //    if(mm != null) {
                //        Category.Clear();
                //        foreach(MetadataModel item in mm)
                //            Category.Add(item);
                //    }
                //    else {
                //        Analytics.TrackEvent("Не получен список категорий техников. Список причин пустой");
                //    }
                //}
                //else
                //    Crashes.TrackError(new Exception("Категории техников. От сервера не получен корректный ответ"),
                //        new Dictionary<string,string> {
                //            {"ServerResponse",httpResponse.Content.ReadAsStringAsync().Result },
                //            {"StatusCode",httpResponse.StatusCode.ToString() },
                //            {"Response",httpResponse.ToString() },
                //            {"Query","Common/metadata?ColumnName=new_category&ObjectName=New_serviceman" }
                //        });
            });
        }
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Выход со страницы заявок технику",
               new Dictionary<string,string> {
                    {"Serviceman",Servicemans.FirstOrDefault().NewPhone }
               });
                MainMenuPageViewModel vm = new MainMenuPageViewModel(Mounters,Servicemans);
                App.Current.MainPage = new MainMenuPage(vm);
            });
        }

        private RelayCommand _ViewDescriptionServiceOrder;
        public RelayCommand ViewDescriptionServiceOrder {
            get => _ViewDescriptionServiceOrder ??= new RelayCommand(async obj => {
                //if(obj != null) {
                //    await Application.Current.MainPage.DisplayAlert("Информация",obj.ToString(),"OK");
                //}
                NewServiceorderExtensionBase_ex so = null;
                NewTest2ExtensionBase_ex fso = null;
                if(obj != null) {
                    if(!string.IsNullOrEmpty(obj.ToString())) {
                        //int? _obj = int.Parse(obj.ToString());
                        int? _obj = int.TryParse(obj.ToString(),out _) ? int.Parse(obj.ToString()) : -1;
                        if(_obj != -1) {
                            try {
                                if(ServiceOrders.Count > 0)
                                    so = ServiceOrders.First(x => x.NewNumber == _obj);
                            }
                            catch { }
                            try {
                                if(ServiceOrdersFireAlarm.Count > 0)
                                    fso = ServiceOrdersFireAlarm.First(x => x.NewNumber == _obj);
                            }
                            catch { }
                        }
                    }
                    ServiceOrderInfoPopupViewModel vm = null;
                    if(so != null)
                        vm = new ServiceOrderInfoPopupViewModel(so);
                    if(fso != null)
                        vm = new ServiceOrderInfoPopupViewModel(fso);
                    await App.Current.MainPage.Navigation.PushPopupAsync(new ServiceOrderInfoPopupPage(vm));
                }
                else
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Ошибка при получении номера объекта",Color.Red,LayoutOptions.EndAndExpand),4000));
                //App.Current.MainPage = new ServiceOrderInfoPopupPage(vm);
            });
        }

        private RelayCommand _OpenMapCommand;
        public RelayCommand OpenMapCommand {
            get => _OpenMapCommand ??= new RelayCommand(async obj => {
                OpacityForm = 0.1;
                IndicatorVisible = true;
                //using HttpClient client = new HttpClient(GetHttpClientHandler());
                //if(obj != null) {
                //    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/GetObjectInfo?ObjectNumber=" + obj.ToString() + "");
                //    A28Object a28Object = new A28Object();
                //    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                //        var resp = response.Content.ReadAsStringAsync().Result;
                //        try {
                //            a28Object = JsonConvert.DeserializeObject<A28Object>(resp);
                //        }
                //        catch {
                //            a28Object = null;
                //        }
                //    }
                //    if(a28Object != null) {
                //        if(a28Object.Longitude != null && a28Object.Latitude != null) {
                //            var location = new Location((double)a28Object.Latitude,(double)a28Object.Longitude);
                //            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
                //            await Map.OpenAsync(location,options);
                //        }
                //    }
                //}
                //else {
                //    Analytics.TrackEvent("У заявки технику не определен номер объекта, невозможно получить координаты и открыть карту");
                //}


                A28Object a28Object = await ClientHttp.Get<A28Object>("/api/Andromeda/GetObjectInfo?ObjectNumber=" + obj.ToString());
                if(a28Object != null) {
                    if(a28Object.Longitude != null && a28Object.Latitude != null) {
                        var location = new Location((double)a28Object.Latitude,(double)a28Object.Longitude);
                        var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
                        await Map.OpenAsync(location,options);
                    }
                }
                OpacityForm = 1;
                IndicatorVisible = false;
            });
        }

        private ImageSource _MapImage;
        public ImageSource MapImage {
            get => _MapImage;
            set {
                _MapImage = value;
                OnPropertyChanged(nameof(MapImage));
            }
        }
        private Color _FrameColor;
        public Color FrameColor {
            get => _FrameColor;
            set {
                _FrameColor = value;
                OnPropertyChanged(nameof(FrameColor));
            }
        }

        private RelayCommand _RefreshOrdersCommand;
        public RelayCommand RefreshOrdersCommand {
            get => _RefreshOrdersCommand ??= new RelayCommand(async obj => {
                //ActivityIndicatorViewModel vm = new ActivityIndicatorViewModel(false);
                //await App.Current.MainPage.Navigation.PushModalAsync(new ActivityIndicatorPopupPage(vm));

                GetCategoryTech.Execute(Category);
                GetServiceOrders.Execute(Servicemans);
                GetServiceOrderByTransfer.Execute(Servicemans);

                NewServicemanExtensionBase sm = null;
                if(Category != null)
                    if(Category.Count > 0) {
                        try {
                            sm = Servicemans.FirstOrDefault(x => x.NewCategory == Category.FirstOrDefault(x => x.Value == 6).Value);
                        }
                        catch(Exception ex) {
                            sm = null;
                            Analytics.TrackEvent("Не удалось найти техника с необходимой категорией для техников по ПС",
                            new Dictionary<string,string> {
                                {"ErrorMessage",ex.Message }
                            });
                        }
                        if(sm != null) {
                            GetServiceOrderByTransferFireAlarm.Execute(null);
                            GetServiceOrdersFireAlarm.Execute(null);
                        }
                    }
                //vm = new ActivityIndicatorViewModel(false);
                //await App.Current.MainPage.Navigation.PushModalAsync(new ActivityIndicatorPopupPage(vm));
                //await PopupNavigation.Instance.PushAsync(new ActivityIndicatorPopupPage(vm));
            });
        }
        private RelayCommand _GetServiceOrders;
        public RelayCommand GetServiceOrders {
            get => _GetServiceOrders ??= new RelayCommand(async obj => {
                OpacityForm = 0.1;
                IndicatorVisible = true;
                if(Servicemans.Count > 0) {
                    Analytics.TrackEvent("Получение заявок технику",
                    new Dictionary<string,string> {
                        {"Serviceman",Servicemans.FirstOrDefault().NewPhone },
                        {"Date",Date.ToString() }
                    });
                    ///api/NewServiceorderExtensionBases/ServiceOrderByUser?usr_ID=FEF46B07-8D7A-E311-920A-00155D01051D&date=18.11.2020
                    if(Date == DateTime.Parse("01.01.0001 00:00:00"))
                        Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));

                    //List<NewServiceorderExtensionBase_ex> _serviceorders = new List<NewServiceorderExtensionBase_ex>();
                    List<NewServiceorderExtensionBase_ex> _serviceorders = await ClientHttp.Get<List<NewServiceorderExtensionBase_ex>>("/api/NewServiceorderExtensionBases/ServiceOrderByUserNew?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);

                    //using HttpClient client = new HttpClient(GetHttpClientHandler());
                    //HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/ServiceOrderByUserNew?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);
                    //List<NewServiceorderExtensionBase_ex> _serviceorders = new List<NewServiceorderExtensionBase_ex>();
                    //if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                    //    var resp = response.Content.ReadAsStringAsync().Result;
                    //    try {
                    //        _serviceorders = JsonConvert.DeserializeObject<List<NewServiceorderExtensionBase_ex>>(resp).ToList();
                    //    }
                    //    catch(Exception ex) {
                    //        _serviceorders = null;
                    //        Crashes.TrackError(new Exception("Ошибка десериализации результата запроса(Заявки технику)"),
                    //        new Dictionary<string,string> {
                    //            {"Servicemans",Servicemans.First().NewPhone },
                    //            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                    //            {"ErrorMessage",ex.Message },
                    //            {"StatusCode",response.StatusCode.ToString() },
                    //            {"Response",response.ToString() }
                    //        });
                    //    }
                    //}
                    //else {
                    //    _serviceorders = null;
                    //    Crashes.TrackError(new Exception("Ошибка запроса(Заявки технику)"),
                    //    new Dictionary<string,string> {
                    //            {"Servicemans",Servicemans.First().NewPhone },
                    //            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                    //            {"StatusCode",response.StatusCode.ToString() },
                    //            {"Response",response.ToString() }
                    //    });
                    //}
                    if(_serviceorders != null) {
                        if(ServiceOrders != null)
                            ServiceOrders.Clear();
                        if(ServiceOrdersByTime != null)
                            ServiceOrdersByTime.Clear();
                        foreach(NewServiceorderExtensionBase_ex item in _serviceorders) {
                            if(string.IsNullOrEmpty(item.NewTime))
                                ServiceOrders.Add(item);
                            else
                                ServiceOrdersByTime.Add(item);
                        }
                        TimeServiceOrder = "Временные (" + ServiceOrdersByTime.Count.ToString() + ")";
                        OtherServiceOrder = "Прочие (" + ServiceOrders.Count.ToString() + ")";
                        TimeServiceOrderVisible = ServiceOrdersByTime.Count > 0;
                        OtherServiceOrderVisible = ServiceOrders.Count > 0;
                    }
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            });
        }
        private RelayCommand _GetServiceOrdersFireAlarm;
        public RelayCommand GetServiceOrdersFireAlarm {
            get => _GetServiceOrdersFireAlarm ??= new RelayCommand(async obj => {
                OpacityForm = 0.1;
                IndicatorVisible = true;
                if(Servicemans.Count > 0) {
                    Analytics.TrackEvent("Получение заявок технику на ПС",
                    new Dictionary<string,string> {
                        {"Serviceman",Servicemans.FirstOrDefault().NewPhone },
                        {"Date",Date.ToString() }
                    });
                    ///api/NewServiceorderExtensionBases/ServiceOrderByUser?usr_ID=FEF46B07-8D7A-E311-920A-00155D01051D&date=18.11.2020
                    if(Date == DateTime.Parse("01.01.0001 00:00:00"))
                        Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));

                    //List<NewTest2ExtensionBase_ex> _serviceorders = new List<NewTest2ExtensionBase_ex>();
                    List<NewTest2ExtensionBase_ex> _serviceorders = await ClientHttp.Get<List<NewTest2ExtensionBase_ex>>("/api/NewServiceOrderForFireAlarmExtensionBase/ServiceOrderByUserNew?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);

                    //using HttpClient client = new HttpClient(GetHttpClientHandler());
                    //HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceOrderForFireAlarmExtensionBase/ServiceOrderByUserNew?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);
                    //List<NewTest2ExtensionBase_ex> _serviceorders = new List<NewTest2ExtensionBase_ex>();
                    //if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                    //    var resp = response.Content.ReadAsStringAsync().Result;
                    //    try {
                    //        _serviceorders = JsonConvert.DeserializeObject<List<NewTest2ExtensionBase_ex>>(resp).ToList();
                    //    }
                    //    catch(Exception ex) {
                    //        _serviceorders = null;
                    //        Crashes.TrackError(new Exception("Ошибка десериализации результата запроса(Заявки технику ПС)"),
                    //        new Dictionary<string,string> {
                    //            {"Servicemans",Servicemans.First().NewPhone },
                    //            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                    //            {"ErrorMessage",ex.Message },
                    //            {"StatusCode",response.StatusCode.ToString() },
                    //            {"Response",response.ToString() }
                    //        });
                    //    }
                    //}
                    //else {
                    //    _serviceorders = null;
                    //    Crashes.TrackError(new Exception("Ошибка запроса(Заявки технику)"),
                    //    new Dictionary<string,string> {
                    //            {"Servicemans",Servicemans.First().NewPhone },
                    //            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                    //            {"StatusCode",response.StatusCode.ToString() },
                    //            {"Response",response.ToString() }
                    //    });
                    //}
                    if(_serviceorders != null) {
                        if(ServiceOrdersFireAlarm != null)
                            ServiceOrdersFireAlarm.Clear();
                        if(ServiceOrdersByTimeFireAlarm != null)
                            ServiceOrdersByTimeFireAlarm.Clear();
                        foreach(NewTest2ExtensionBase_ex item in _serviceorders) {
                            if(string.IsNullOrEmpty(item.NewTime))
                                ServiceOrdersFireAlarm.Add(item);
                            else
                                ServiceOrdersByTimeFireAlarm.Add(item);
                        }
                        FireAlarmTimeServiceOrderText = "Временные(пс) (" + ServiceOrdersByTimeFireAlarm.Count.ToString() + ")";
                        FireAlarmOtherServiceOrderText = "Прочие(пс) (" + ServiceOrdersFireAlarm.Count.ToString() + ")";
                        FireAlarmTimeServiceOrderVisible = ServiceOrdersByTimeFireAlarm.Count > 0;
                        FireAlarmOtherServiceOrderVisible = ServiceOrdersFireAlarm.Count > 0;
                    }
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            });
        }

        private RelayCommand _GetServiceOrderByTransfer;
        public RelayCommand GetServiceOrderByTransfer {
            get => _GetServiceOrderByTransfer ??= new RelayCommand(async obj => {
                OpacityForm = 0.1;
                IndicatorVisible = true;
                if(Servicemans.Count > 0) {
                    Analytics.TrackEvent("Получение заявок технику заявок технику - переносы",
                    new Dictionary<string,string> {
                        {"Serviceman",Servicemans.FirstOrDefault().NewPhone },
                        {"Date",Date.ToString() }
                    });
                    ///api/NewServiceorderExtensionBases/ServiceOrderByUser?usr_ID=FEF46B07-8D7A-E311-920A-00155D01051D&date=18.11.2020
                    if(Date == DateTime.Parse("01.01.0001 00:00:00"))
                        Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));

                    //List<NewServiceorderExtensionBase_ex> _serviceorders = new List<NewServiceorderExtensionBase_ex>();
                    List<NewServiceorderExtensionBase_ex> _serviceorders = await ClientHttp.Get<List<NewServiceorderExtensionBase_ex>>("/api/NewServiceorderExtensionBases/ServiceOrderByUserTransferReasonNew?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);

                    //using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    //    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/ServiceOrderByUserTransferReasonNew?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);
                    //    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                    //        var resp = response.Content.ReadAsStringAsync().Result;
                    //        try {
                    //            _serviceorders = JsonConvert.DeserializeObject<List<NewServiceorderExtensionBase_ex>>(resp).ToList();
                    //        }
                    //        catch(Exception ex) {
                    //            _serviceorders = null;
                    //            Crashes.TrackError(new Exception("Ошибка десериализации результата запроса(Заявки технику - переносы)"),
                    //            new Dictionary<string,string> {
                    //            {"Servicemans",Servicemans.First().NewPhone },
                    //            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                    //            {"ErrorMessage",ex.Message },
                    //            {"StatusCode",response.StatusCode.ToString() },
                    //            {"Response",response.ToString() }
                    //            });
                    //        }
                    //    }
                    //    else {
                    //        _serviceorders = null;
                    //        //Crashes.TrackError(new Exception("Ошибка запроса(Заявки технику - переносы)"),
                    //        //new Dictionary<string,string> {
                    //        //    {"Servicemans",Servicemans.First().NewPhone },
                    //        //    {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                    //        //    {"StatusCode",response.StatusCode.ToString() },
                    //        //    {"Response",response.ToString() }
                    //        //});
                    //    }
                    //}
                    if(_serviceorders != null) {
                        if(ServiceOrderByTransfer != null)
                            ServiceOrderByTransfer.Clear();
                        foreach(NewServiceorderExtensionBase_ex item in _serviceorders) {
                            ServiceOrderByTransfer.Add(item);
                        }
                        TransferServiceOrder = "Перенесенные (" + ServiceOrderByTransfer.Count.ToString() + ")";
                        TransferServiceOrderVisible = ServiceOrderByTransfer.Count > 0;
                    }
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            });
        }
        private RelayCommand _GetServiceOrderByTransferFireAlarm;
        public RelayCommand GetServiceOrderByTransferFireAlarm {
            get => _GetServiceOrderByTransferFireAlarm ??= new RelayCommand(async obj => {
                OpacityForm = 0.1;
                IndicatorVisible = true;
                if(Servicemans.Count > 0) {
                    Analytics.TrackEvent("Получение заявок технику на ПС - переносы",
                    new Dictionary<string,string> {
                        {"Serviceman",Servicemans.FirstOrDefault().NewPhone },
                        {"Date",Date.ToString() }
                    });
                    if(Date == DateTime.Parse("01.01.0001 00:00:00"))
                        Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));

                    //List<NewTest2ExtensionBase_ex> _serviceorders = new List<NewTest2ExtensionBase_ex>();
                    List<NewTest2ExtensionBase_ex> _serviceorders = await ClientHttp.Get<List<NewTest2ExtensionBase_ex>>("/api/NewServiceOrderForFireAlarmExtensionBase/ServiceOrderByUserTransferReasonNew?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);

                    ///api/NewServiceorderExtensionBases/ServiceOrderByUser?usr_ID=FEF46B07-8D7A-E311-920A-00155D01051D&date=18.11.2020

                    //using HttpClient client = new HttpClient(GetHttpClientHandler());
                    //HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceOrderForFireAlarmExtensionBase/ServiceOrderByUserTransferReasonNew?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);
                    //List<NewTest2ExtensionBase_ex> _serviceorders = new List<NewTest2ExtensionBase_ex>();
                    //if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                    //    var resp = response.Content.ReadAsStringAsync().Result;
                    //    try {
                    //        _serviceorders = JsonConvert.DeserializeObject<List<NewTest2ExtensionBase_ex>>(resp).ToList();
                    //    }
                    //    catch(Exception ex) {
                    //        _serviceorders = null;
                    //        Crashes.TrackError(new Exception("Ошибка десериализации результата запроса(Заявки технику - переносы(ПС))"),
                    //        new Dictionary<string,string> {
                    //            {"Servicemans",Servicemans.First().NewPhone },
                    //            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                    //            {"ErrorMessage",ex.Message },
                    //            {"StatusCode",response.StatusCode.ToString() },
                    //            {"Response",response.ToString() }
                    //        });
                    //    }
                    //}
                    //else {
                    //    _serviceorders = null;
                    //    //Crashes.TrackError(new Exception("Ошибка запроса(Заявки технику - переносы)"),
                    //    //new Dictionary<string,string> {
                    //    //        {"Servicemans",Servicemans.First().NewPhone },
                    //    //        {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                    //    //        {"StatusCode",response.StatusCode.ToString() },
                    //    //        {"Response",response.ToString() }
                    //    //});
                    //}
                    if(_serviceorders != null) {
                        if(ServiceOrderByTransferFireAlarm != null)
                            ServiceOrderByTransferFireAlarm.Clear();
                        foreach(NewTest2ExtensionBase_ex item in _serviceorders) {
                            ServiceOrderByTransferFireAlarm.Add(item);
                        }
                        FireAlarmTransferServiceOrderText = "Перенесенные(пс) (" + ServiceOrderByTransferFireAlarm.Count.ToString() + ")";
                        FireAlarmTransferServiceOrderVisible = ServiceOrderByTransferFireAlarm.Count > 0;
                    }
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            });
        }
        private AsyncCommand _SelectServiceOrderCommand;
        public AsyncCommand SelectServiceOrderCommand {
            get => _SelectServiceOrderCommand ??= new AsyncCommand(async () => {
                if(ServiceOrder != null) {
                    Analytics.TrackEvent("Переход к заявке технику",
                    new Dictionary<string,string> {
                        {"ServiceOrder",ServiceOrder.NewServiceorderId.ToString()}
                    });
                    ServiceOrderViewModel vm = new ServiceOrderViewModel(ServiceOrder,Servicemans,Mounters);
                    App.Current.MainPage = new ServiceOrder(vm);
                }
            });
        }


        private RelayCommand _SelectServiceOrderFireAlarmCommand;
        public RelayCommand SelectServiceOrderFireAlarmCommand {
            get => _SelectServiceOrderFireAlarmCommand ??= new RelayCommand(async obj => {
                if(ServiceOrderFireAlarm != null) {
                    Analytics.TrackEvent("Переход к заявке технику",
                    new Dictionary<string,string> {
                        {"ServiceOrder",ServiceOrderFireAlarm.NewTest2Id.ToString()}
                    });
                    ServiceOrderFireAlarmViewModel vm = new ServiceOrderFireAlarmViewModel(ServiceOrderFireAlarm,Servicemans,Mounters);
                    App.Current.MainPage = new ServiceOrderFireAlarm(vm);
                }
            });
        }

        private ObservableCollection<MetadataModel> _Category = new ObservableCollection<MetadataModel>();
        public ObservableCollection<MetadataModel> Category {
            get => _Category;
            set {
                _Category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        #region Управление Expander-ом для общих заявок
        private bool _ServiceOrderExpanded;
        public bool ServiceOrderExpanded {
            get => _ServiceOrderExpanded;
            set {
                if(_ServiceOrderExpanded)
                    ArrowCircleServiceOrder = IconName("arrow_circle_up");
                else
                    ArrowCircleServiceOrder = IconName("arrow_circle_down");
                _ServiceOrderExpanded = value;
                OnPropertyChanged(nameof(ServiceOrderExpanded));
            }
        }
        private ImageSource _ArrowCircleServiceOrder;
        public ImageSource ArrowCircleServiceOrder {
            get => _ArrowCircleServiceOrder;
            set {
                _ArrowCircleServiceOrder = value;
                OnPropertyChanged(nameof(ArrowCircleServiceOrder));
            }
        }
        private RelayCommand _ServiceOrderExpanderCommand;
        public RelayCommand ServiceOrderExpanderCommand {
            get => _ServiceOrderExpanderCommand ??= new RelayCommand(async obj => {
                ServiceOrderExpanded = !ServiceOrderExpanded;
            });
        }
        #endregion

        #region Управление Expander-ом для общих пожарных заявок
        private bool _FireAlarmServiceOrderExpanded;
        public bool FireAlarmServiceOrderExpanded {
            get => _FireAlarmServiceOrderExpanded;
            set {
                if(_FireAlarmServiceOrderExpanded)
                    ArrowCircleFireAlarmServiceOrder = IconName("arrow_circle_up");
                else
                    ArrowCircleFireAlarmServiceOrder = IconName("arrow_circle_down");
                _FireAlarmServiceOrderExpanded = value;
                OnPropertyChanged(nameof(FireAlarmServiceOrderExpanded));
            }
        }
        private ImageSource _ArrowCircleFireAlarmServiceOrder;
        public ImageSource ArrowCircleFireAlarmServiceOrder {
            get => _ArrowCircleFireAlarmServiceOrder;
            set {
                _ArrowCircleFireAlarmServiceOrder = value;
                OnPropertyChanged(nameof(ArrowCircleFireAlarmServiceOrder));
            }
        }
        private RelayCommand _FireAlarmServiceOrderExpanderCommand;
        public RelayCommand FireAlarmServiceOrderExpanderCommand {
            get => _FireAlarmServiceOrderExpanderCommand ??= new RelayCommand(async obj => {
                FireAlarmServiceOrderExpanded = !FireAlarmServiceOrderExpanded;
            });
        }
        #endregion

        #region Управление Expander-ом для перенесенных пожарных заявок
        private bool _FireAlarmTransferServiceOrderExpanded;
        public bool FireAlarmTransferServiceOrderExpanded {
            get => _FireAlarmTransferServiceOrderExpanded;
            set {
                if(_FireAlarmTransferServiceOrderExpanded)
                    ArrowCircleFireAlarmTransferServiceOrder = IconName("arrow_circle_up");
                else
                    ArrowCircleFireAlarmTransferServiceOrder = IconName("arrow_circle_down");
                _FireAlarmTransferServiceOrderExpanded = value;
                OnPropertyChanged(nameof(FireAlarmTransferServiceOrderExpanded));
            }
        }
        private ImageSource _ArrowCircleFireAlarmTransferServiceOrder;
        public ImageSource ArrowCircleFireAlarmTransferServiceOrder {
            get => _ArrowCircleFireAlarmTransferServiceOrder;
            set {
                _ArrowCircleFireAlarmTransferServiceOrder = value;
                OnPropertyChanged(nameof(ArrowCircleFireAlarmTransferServiceOrder));
            }
        }
        private RelayCommand _FireAlarmTransferServiceOrderExpanderCommand;
        public RelayCommand FireAlarmTransferServiceOrderExpanderCommand {
            get => _FireAlarmTransferServiceOrderExpanderCommand ??= new RelayCommand(async obj => {
                FireAlarmTransferServiceOrderExpanded = !FireAlarmTransferServiceOrderExpanded;
            });
        }

        private string _FireAlarmTransferServiceOrderText;
        public string FireAlarmTransferServiceOrderText {
            get => _FireAlarmTransferServiceOrderText;
            set {
                _FireAlarmTransferServiceOrderText = value;
                OnPropertyChanged(nameof(FireAlarmTransferServiceOrderText));
            }
        }
        #endregion

        #region Управление Expander-ом для временных пожарных заявок
        private bool _FireAlarmTimeServiceOrderExpanded;
        public bool FireAlarmTimeServiceOrderExpanded {
            get => _FireAlarmTimeServiceOrderExpanded;
            set {
                if(_FireAlarmTimeServiceOrderExpanded)
                    ArrowCircleFireAlarmTimeServiceOrder = IconName("arrow_circle_up");
                else
                    ArrowCircleFireAlarmTimeServiceOrder = IconName("arrow_circle_down");
                _FireAlarmTimeServiceOrderExpanded = value;
                OnPropertyChanged(nameof(FireAlarmTimeServiceOrderExpanded));
            }
        }
        private ImageSource _ArrowCircleFireAlarmTimeServiceOrder;
        public ImageSource ArrowCircleFireAlarmTimeServiceOrder {
            get => _ArrowCircleFireAlarmTimeServiceOrder;
            set {
                _ArrowCircleFireAlarmTimeServiceOrder = value;
                OnPropertyChanged(nameof(ArrowCircleFireAlarmTimeServiceOrder));
            }
        }
        private RelayCommand _FireAlarmTimeServiceOrderExpanderCommand;
        public RelayCommand FireAlarmTimeServiceOrderExpanderCommand {
            get => _FireAlarmTimeServiceOrderExpanderCommand ??= new RelayCommand(async obj => {
                FireAlarmTimeServiceOrderExpanded = !FireAlarmTimeServiceOrderExpanded;
            });
        }

        private string _FireAlarmTimeServiceOrderText;
        public string FireAlarmTimeServiceOrderText {
            get => _FireAlarmTimeServiceOrderText;
            set {
                _FireAlarmTimeServiceOrderText = value;
                OnPropertyChanged(nameof(FireAlarmTimeServiceOrderText));
            }
        }
        #endregion

        #region Управление Expander-ом для остальных пожарных заявок
        private bool _FireAlarmOtherServiceOrderExpanded;
        public bool FireAlarmOtherServiceOrderExpanded {
            get => _FireAlarmOtherServiceOrderExpanded;
            set {
                if(_FireAlarmOtherServiceOrderExpanded)
                    ArrowCircleFireAlarmOtherServiceOrder = IconName("arrow_circle_up");
                else
                    ArrowCircleFireAlarmOtherServiceOrder = IconName("arrow_circle_down");
                _FireAlarmOtherServiceOrderExpanded = value;
                OnPropertyChanged(nameof(FireAlarmOtherServiceOrderExpanded));
            }
        }
        private ImageSource _ArrowCircleFireAlarmOtherServiceOrder;
        public ImageSource ArrowCircleFireAlarmOtherServiceOrder {
            get => _ArrowCircleFireAlarmOtherServiceOrder;
            set {
                _ArrowCircleFireAlarmOtherServiceOrder = value;
                OnPropertyChanged(nameof(ArrowCircleFireAlarmOtherServiceOrder));
            }
        }
        private RelayCommand _FireAlarmOtherServiceOrderExpanderCommand;
        public RelayCommand FireAlarmOtherServiceOrderExpanderCommand {
            get => _FireAlarmOtherServiceOrderExpanderCommand ??= new RelayCommand(async obj => {
                FireAlarmOtherServiceOrderExpanded = !FireAlarmOtherServiceOrderExpanded;
            });
        }

        private string _FireAlarmOtherServiceOrderText;
        public string FireAlarmOtherServiceOrderText {
            get => _FireAlarmOtherServiceOrderText;
            set {
                _FireAlarmOtherServiceOrderText = value;
                OnPropertyChanged(nameof(FireAlarmOtherServiceOrderText));
            }
        }
        #endregion

        private NewServiceorderExtensionBase_ex _ServiceOrder;
        public NewServiceorderExtensionBase_ex ServiceOrder {
            get => _ServiceOrder;
            set {
                _ServiceOrder = value;
                OnPropertyChanged(nameof(ServiceOrder));
            }
        }

        private NewTest2ExtensionBase_ex _ServiceOrderFireAlarm;
        public NewTest2ExtensionBase_ex ServiceOrderFireAlarm {
            get => _ServiceOrderFireAlarm;
            set {
                _ServiceOrderFireAlarm = value;
                OnPropertyChanged(nameof(ServiceOrderFireAlarm));
            }
        }

        private ObservableCollection<NewServiceorderExtensionBase_ex> _ServiceOrders = new ObservableCollection<NewServiceorderExtensionBase_ex>();
        public ObservableCollection<NewServiceorderExtensionBase_ex> ServiceOrders {
            get => _ServiceOrders;
            set {
                _ServiceOrders = value;
                OnPropertyChanged(nameof(ServiceOrders));
            }
        }

        private ObservableCollection<NewServiceorderExtensionBase_ex> _ServiceOrderByTransfer = new ObservableCollection<NewServiceorderExtensionBase_ex>();
        public ObservableCollection<NewServiceorderExtensionBase_ex> ServiceOrderByTransfer {
            get => _ServiceOrderByTransfer;
            set {
                _ServiceOrderByTransfer = value;
                OnPropertyChanged(nameof(ServiceOrderByTransfer));
            }
        }
        private ObservableCollection<NewServiceorderExtensionBase_ex> _ServiceOrdersByTime = new ObservableCollection<NewServiceorderExtensionBase_ex>();
        public ObservableCollection<NewServiceorderExtensionBase_ex> ServiceOrdersByTime {
            get => _ServiceOrdersByTime;
            set {
                _ServiceOrdersByTime = value;
                OnPropertyChanged(nameof(ServiceOrdersByTime));
            }
        }

        private ObservableCollection<NewTest2ExtensionBase_ex> _ServiceOrdersByTimeFireAlarm = new ObservableCollection<NewTest2ExtensionBase_ex>();
        public ObservableCollection<NewTest2ExtensionBase_ex> ServiceOrdersByTimeFireAlarm {
            get => _ServiceOrdersByTimeFireAlarm;
            set {
                _ServiceOrdersByTimeFireAlarm = value;
                OnPropertyChanged(nameof(ServiceOrdersByTimeFireAlarm));
            }
        }

        private ObservableCollection<NewTest2ExtensionBase_ex> _ServiceOrdersFireAlarm = new ObservableCollection<NewTest2ExtensionBase_ex>();
        public ObservableCollection<NewTest2ExtensionBase_ex> ServiceOrdersFireAlarm {
            get => _ServiceOrdersFireAlarm;
            set {
                _ServiceOrdersFireAlarm = value;
                OnPropertyChanged(nameof(ServiceOrdersFireAlarm));
            }
        }

        private ObservableCollection<NewTest2ExtensionBase_ex> _ServiceOrderByTransferFireAlarm = new ObservableCollection<NewTest2ExtensionBase_ex>();
        public ObservableCollection<NewTest2ExtensionBase_ex> ServiceOrderByTransferFireAlarm {
            get => _ServiceOrderByTransferFireAlarm;
            set {
                _ServiceOrderByTransferFireAlarm = value;
                OnPropertyChanged(nameof(ServiceOrderByTransferFireAlarm));
            }
        }

        private bool _TransferServiceOrderExpanded;
        public bool TransferServiceOrderExpanded {
            get => _TransferServiceOrderExpanded;
            set {
                _TransferServiceOrderExpanded = value;
                if(_TransferServiceOrderExpanded)
                    ArrowCircleTransferServiceOrder = IconName("arrow_circle_up");
                else
                    ArrowCircleTransferServiceOrder = IconName("arrow_circle_down");
                OnPropertyChanged(nameof(TransferServiceOrderExpanded));
            }
        }

        private bool _TimeServiceOrderExpanded;
        public bool TimeServiceOrderExpanded {
            get => _TimeServiceOrderExpanded;
            set {
                _TimeServiceOrderExpanded = value;
                if(_TimeServiceOrderExpanded)
                    ArrowCircleTimeServiceOrder = IconName("arrow_circle_up");
                else
                    ArrowCircleTimeServiceOrder = IconName("arrow_circle_down");
                OnPropertyChanged(nameof(TimeServiceOrderExpanded));
            }
        }

        private bool _OtherServiceOrderExpanded;
        public bool OtherServiceOrderExpanded {
            get => _OtherServiceOrderExpanded;
            set {
                _OtherServiceOrderExpanded = value;
                if(_OtherServiceOrderExpanded)
                    ArrowCircleOtherServiceOrder = IconName("arrow_circle_up");
                else
                    ArrowCircleOtherServiceOrder = IconName("arrow_circle_down");
                OnPropertyChanged(nameof(OtherServiceOrderExpanded));
            }
        }

        private RelayCommand _TransferServiceOrderExpanderCommand;
        public RelayCommand TransferServiceOrderExpanderCommand {
            get => _TransferServiceOrderExpanderCommand ??= new RelayCommand(async obj => {
                TransferServiceOrderExpanded = !TransferServiceOrderExpanded;
            });
        }

        private string _TransferServiceOrder;
        public string TransferServiceOrder {
            get => _TransferServiceOrder;
            set {
                _TransferServiceOrder = value;
                OnPropertyChanged(nameof(TransferServiceOrder));
            }
        }

        private ImageSource _ArrowCircleTransferServiceOrder;
        public ImageSource ArrowCircleTransferServiceOrder {
            get => _ArrowCircleTransferServiceOrder;
            set {
                _ArrowCircleTransferServiceOrder = value;
                OnPropertyChanged(nameof(ArrowCircleTransferServiceOrder));
            }
        }

        private RelayCommand _TimeServiceOrderExpanderCommand;
        public RelayCommand TimeServiceOrderExpanderCommand {
            get => _TimeServiceOrderExpanderCommand ??= new RelayCommand(async obj => {
                TimeServiceOrderExpanded = !TimeServiceOrderExpanded;
            });
        }


        //private RelayCommand _ViewEntraceCommand;
        //public RelayCommand ViewEntraceCommand {
        //    get => _ViewEntraceCommand ??= new RelayCommand(async obj => {
        //        IndicatorVisible = true;
        //        OpacityForm = 0.1;
        //        ImageSource _is = null;
        //        using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
        //            HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/download?ObjectNumber=" + obj + "&PhotoType=Вывеска объекта");
        //            if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
        //                var result = await response.Content.ReadAsStringAsync();
        //                _is = ImageSource.FromStream(() => {
        //                    return new MemoryStream(Convert.FromBase64String(result));
        //                });
        //                ImagePopupViewModel vm = new ImagePopupViewModel(_is);
        //                await App.Current.MainPage.Navigation.PushPopupAsync(new ImagePopupPage(vm));
        //                IndicatorVisible = false;
        //                OpacityForm = 1;
        //            }
        //            else {
        //                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Фотография не найдена",Color.Red,LayoutOptions.EndAndExpand),4000));
        //                IndicatorVisible = false;
        //                OpacityForm = 1;
        //            }
        //        }
        //    });
        //}
        //private RelayCommand _ViewSchemeCommand;
        //public RelayCommand ViewSchemeCommand {
        //    get => _ViewSchemeCommand ??= new RelayCommand(async obj => {
        //        IndicatorVisible = true;
        //        OpacityForm = 0.1;
        //        ImageSource _is = null;
        //        using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
        //            HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/download?ObjectNumber=" + obj + "&PhotoType=Схема объекта");
        //            if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
        //                var result = await response.Content.ReadAsStringAsync();
        //                _is = ImageSource.FromStream(() => {
        //                    return new MemoryStream(Convert.FromBase64String(result));
        //                });
        //                ImagePopupViewModel vm = new ImagePopupViewModel(_is);
        //                await App.Current.MainPage.Navigation.PushPopupAsync(new ImagePopupPage(vm));
        //                IndicatorVisible = false;
        //                OpacityForm = 1;
        //            }
        //            else {
        //                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Фотография не найдена",Color.Red,LayoutOptions.EndAndExpand),4000));
        //                IndicatorVisible = false;
        //                OpacityForm = 1;
        //            }
        //            //    if(response.IsSuccessStatusCode) {
        //            //        var result = await response.Content.ReadAsStringAsync();
        //            //        _is = ImageSource.FromStream(() => {
        //            //            return new MemoryStream(Convert.FromBase64String(result));
        //            //        });
        //            //    }
        //            //}
        //            //if(_is != null) {
        //            //    ImagePopupViewModel vm = new ImagePopupViewModel(_is);
        //            //    await App.Current.MainPage.Navigation.PushPopupAsync(new ImagePopupPage(vm));
        //            //    IndicatorVisible = false;
        //            //    OpacityForm = 1;
        //            //}
        //            //else {
        //            //    //await Application.Current.MainPage.DisplayAlert("Ошибка","Фотография не найдена!","OK");
        //            //    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Фотография не найдена",Color.Red,LayoutOptions.EndAndExpand),4000));
        //            //    IndicatorVisible = false;
        //            //    OpacityForm = 1;
        //            //}
        //        }
        //    });
        //}


        private string _TimeServiceOrder;
        public string TimeServiceOrder {
            get => _TimeServiceOrder;
            set {
                _TimeServiceOrder = value;
                OnPropertyChanged(nameof(TimeServiceOrder));
            }
        }

        private ImageSource _ArrowCircleTimeServiceOrder;
        public ImageSource ArrowCircleTimeServiceOrder {
            get => _ArrowCircleTimeServiceOrder;
            set {
                _ArrowCircleTimeServiceOrder = value;
                OnPropertyChanged(nameof(ArrowCircleTimeServiceOrder));
            }
        }

        private RelayCommand _OtherServiceOrderExpanderCommand;
        public RelayCommand OtherServiceOrderExpanderCommand {
            get => _OtherServiceOrderExpanderCommand ??= new RelayCommand(async obj => {
                OtherServiceOrderExpanded = !OtherServiceOrderExpanded;
            });
        }

        private string _OtherServiceOrder;
        public string OtherServiceOrder {
            get => _OtherServiceOrder;
            set {
                _OtherServiceOrder = value;
                OnPropertyChanged(nameof(OtherServiceOrder));
            }
        }

        private ImageSource _ArrowCircleOtherServiceOrder;
        public ImageSource ArrowCircleOtherServiceOrder {
            get => _ArrowCircleOtherServiceOrder;
            set {
                _ArrowCircleOtherServiceOrder = value;
                OnPropertyChanged(nameof(ArrowCircleOtherServiceOrder));
            }
        }
    }
}
