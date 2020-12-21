﻿using Android.Widget;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class ServiceOrdersPageViewModel : BaseViewModel {
        public ServiceOrdersPageViewModel() {

        }
        public ServiceOrdersPageViewModel(List<NewServicemanExtensionBase> _servicemans,List<NewMounterExtensionBase> _mounters) {
            Servicemans = _servicemans;
            Mounters = _mounters;
            OpacityForm = 1;
            IndicatorVisible = false;
            GetServiceOrders.Execute(Servicemans);
            GetServiceOrderByTransfer.Execute(Servicemans);
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
            Analytics.TrackEvent("Инициализация страницы заявок технику",
            new Dictionary<string,string> {
                {"Serviceman",Servicemans.FirstOrDefault().NewPhone }
            });
            RefreshImage = "refresh.png";
            MapImage = "map.png";
            TransferImage = "transfer.png";
            FrameColor = Color.Red;
            ArrowCircleTransferServiceOrder = "arrow_circle_down.png";
            ArrowCircleTimeServiceOrder = "arrow_circle_down.png";
            ArrowCircleOtherServiceOrder = "arrow_circle_down.png";
            TransferServiceOrder = "Перенесенные (0)";
            TimeServiceOrder = "Временные (0)";
            OtherServiceOrder = "Прочие (0)";
            GetCategory.Execute(Category);
            var sm = Servicemans.First(x => x.NewCategory == Category.FirstOrDefault(x => x.Value == 6).Value);
            if(sm!=null) {
                GetServiceOrderByTransferFireAlarm.Execute(null);
                GetServiceOrdersFireAlarm.Execute(null);
            }
            #region Данный код прекрасно мог бы обновлять заявки в фоне, но иногда он крашится, из-за коллекции
            //Device.StartTimer(TimeSpan.FromMinutes(1),() => {
            //    Task.Run(async () => {
            //        if(ServiceOrdersByTime != null)
            //            if(ServiceOrdersByTime.Any()) {
            //                int count_time = ServiceOrdersByTime.Count;
            //            }
            //        if(ServiceOrders != null)
            //            if(ServiceOrders.Any()) {
            //                int count_ordr = ServiceOrders.Count;
            //            }
            //        if(ServiceOrderByTransfer != null)
            //            if(ServiceOrderByTransfer.Any()) {
            //                int count_transfer = ServiceOrderByTransfer.Count;
            //            }
            //        GetServiceOrders.Execute(Servicemans);
            //        GetServiceOrderByTransfer.Execute(Servicemans);
            //        //if (count_time< ServiceOrdersByTime.Count || count_ordr< ServiceOrders.Count || count_transfer< ServiceOrderByTransfer.Count) { }
            //    });
            //    return true; //use this to run continuously 
            //    //return false; //to stop running continuously 

            //});
            #endregion
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

        private RelayCommand _IncomeCommand;
        public RelayCommand IncomeCommand {
            get => _IncomeCommand ??= new RelayCommand(async obj => {
                //if(obj != null) {
                //    Analytics.TrackEvent("Заявка технику: вызов команды Пришел. Попытка получения координат",
                //new Dictionary<string,string> {
                //    {"ServiceOrderID",obj.ToString() },
                //    {"Serviceman",Servicemans.FirstOrDefault().NewPhone}
                //});
                //    OpacityForm = 0.1;
                //    IndicatorVisible = true;
                //    Location location = await Geolocation.GetLastKnownLocationAsync();
                //    if(location != null) {
                //        Latitude = location.Latitude.ToString();
                //        Longitude = location.Longitude.ToString();
                //    }
                //    Analytics.TrackEvent("Запрос данных на сервере (могло же что-то измениться",
                //    new Dictionary<string,string> {
                //    {"ServiceOrderID",obj.ToString() }
                //    });
                //    using HttpClient client = new HttpClient(GetHttpClientHandler());
                //    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/id?id=" + obj);
                //    NewServiceorderExtensionBase soeb = null;
                //    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                //        var resp = response.Content.ReadAsStringAsync().Result;
                //        try {
                //            soeb = JsonConvert.DeserializeObject<NewServiceorderExtensionBase>(resp);
                //        }
                //        catch(Exception ex) {
                //            Crashes.TrackError(new Exception("Ошибка десериализации объекта заявка технику"),
                //            new Dictionary<string,string> {
                //        {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                //        {"ErrorMessage",ex.Message },
                //        {"StatusCode",response.StatusCode.ToString() }
                //            });
                //        }
                //    }
                //    else
                //        Crashes.TrackError(new Exception("Ошибка получения данных об объекте заявка технику с сервера"),
                //        new Dictionary<string,string> {
                //    {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                //    {"StatusCode",response.StatusCode.ToString() },
                //    {"Response",response.ToString() }
                //        });
                //    if(soeb != null) {
                //        Analytics.TrackEvent("Попытка записи данных на сервер по объекту заявка технику, заполняем поле Пришел",
                //        new Dictionary<string,string> {
                //        {"ServiceOrderID",obj.ToString() }
                //        });
                //        soeb.NewIncome = DateTime.Now.AddHours(-5);
                //        using HttpClient clientPut = new HttpClient(GetHttpClientHandler());
                //        var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
                //        HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases",httpContent);
                //        if(!responsePut.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                //            Crashes.TrackError(new Exception("Ошибка при сохранении объекта Заявка технику"),
                //            new Dictionary<string,string> {
                //        {"ServerResponse",responsePut.Content.ReadAsStringAsync().Result },
                //        {"StatusCode",responsePut.StatusCode.ToString() },
                //        {"Response",responsePut.ToString() }
                //            });
                //        }
                //        else
                //            Toast.MakeText(Android.App.Application.Context,"Время прихода записано",ToastLength.Long).Show();
                //    }
                //    //запишем координаты
                //    Analytics.TrackEvent("Попытка записи координат на сервер по объекту заявка технику",
                //        new Dictionary<string,string> {
                //        {"ServiceOrderID",obj.ToString() }
                //        });
                //    using(HttpClient clientPost = new HttpClient(GetHttpClientHandler())) {
                //        var data = JsonConvert.SerializeObject(new ServiceOrderCoordinates() {
                //            SocId = Guid.NewGuid(),
                //            SocServiceOrderId = Guid.Parse(obj.ToString()),
                //            SocIncomeLatitude = Latitude,
                //            SocIncomeLongitude = Longitude
                //        });
                //        StringContent content = new StringContent(data,Encoding.UTF8,"application/json");
                //        HttpResponseMessage responsePost = await clientPost.PostAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates",content);
                //        if(!responsePost.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                //            Crashes.TrackError(new Exception("Ошибка при сохранении объекта Заявка технику"),
                //            new Dictionary<string,string> {
                //        {"ServerResponse",responsePost.Content.ReadAsStringAsync().Result },
                //        {"StatusCode",responsePost.StatusCode.ToString() },
                //        {"Response",responsePost.ToString() }
                //            });
                //        }
                //    }
                //    OpacityForm = 1;
                //    IndicatorVisible = false;
                //}
            });
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

        private RelayCommand _GetCategory;
        public RelayCommand GetCategory {
            get => _GetCategory ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Получение категорий техников",
                new Dictionary<string,string> {
                    {"Query","Common/metadata?ColumnName=new_category&ObjectName=New_serviceman" }
                });
                HttpClient client = new HttpClient(GetHttpClientHandler());
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/metadata?ColumnName=new_category&ObjectName=New_serviceman");
                List <MetadataModel> mm = new List<MetadataModel>();
                if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                    var resp = response.Content.ReadAsStringAsync().Result;
                    try {
                        mm = JsonConvert.DeserializeObject<List<MetadataModel>>(resp);
                    }
                    catch(Exception ex) {
                        mm = null;
                        Crashes.TrackError(new Exception("Ошибка десериализации категорий техников"),
                        new Dictionary<string,string> {
                            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                            {"ErrorMessage",ex.Message },
                            {"StatusCode",response.StatusCode.ToString() },
                            {"Response",response.ToString() },
                            {"Query","Common/metadata?ColumnName=new_category&ObjectName=New_serviceman" }
                        });
                    }
                    if(mm != null) {
                        Category.Clear();
                        foreach(MetadataModel item in mm)
                            Category.Add(item);
                    }
                    else {
                        Analytics.TrackEvent("Не получен список категорий техников. Список причин пустой");
                    }
                }
                else
                    Crashes.TrackError(new Exception("Категории техников. От сервера не получен корректный ответ"),
                        new Dictionary<string,string> {
                            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                            {"StatusCode",response.StatusCode.ToString() },
                            {"Response",response.ToString() },
                            {"Query","Common/metadata?ColumnName=new_category&ObjectName=New_serviceman" }
                        });
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

        private RelayCommand _OpenMapCommand;
        public RelayCommand OpenMapCommand {
            get => _OpenMapCommand ??= new RelayCommand(async obj => {
                OpacityForm = 0.1;
                IndicatorVisible = true;
                using HttpClient client = new HttpClient(GetHttpClientHandler());
                if(obj != null) {
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/GetObjectInfo?ObjectNumber=" + obj.ToString() + "");
                    A28Object a28Object = new A28Object();
                    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                        var resp = response.Content.ReadAsStringAsync().Result;
                        try {
                            a28Object = JsonConvert.DeserializeObject<A28Object>(resp);
                        }
                        catch {
                            a28Object = null;
                        }
                    }
                    if(a28Object != null) {
                        if(a28Object.Longitude != null && a28Object.Latitude != null) {
                            var location = new Location((double)a28Object.Latitude,(double)a28Object.Longitude);
                            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
                            await Map.OpenAsync(location,options);
                        }
                    }
                }
                else {
                    Analytics.TrackEvent("У заявки технику не определен номер объекта, невозможно получить координаты и открыть карту");
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
                    using HttpClient client = new HttpClient(GetHttpClientHandler());
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/ServiceOrderByUser?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);
                    List<NewServiceorderExtensionBase> _serviceorders = new List<NewServiceorderExtensionBase>();
                    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                        var resp = response.Content.ReadAsStringAsync().Result;
                        try {
                            _serviceorders = JsonConvert.DeserializeObject<List<NewServiceorderExtensionBase>>(resp).ToList();
                        }
                        catch(Exception ex) {
                            _serviceorders = null;
                            Crashes.TrackError(new Exception("Ошибка десериализации результата запроса(Заявки технику)"),
                            new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                {"ErrorMessage",ex.Message },
                                {"StatusCode",response.StatusCode.ToString() },
                                {"Response",response.ToString() }
                            });
                        }
                    }
                    else {
                        _serviceorders = null;
                        Crashes.TrackError(new Exception("Ошибка запроса(Заявки технику)"),
                        new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                {"StatusCode",response.StatusCode.ToString() },
                                {"Response",response.ToString() }
                        });
                    }
                    if(_serviceorders != null) {
                        if (ServiceOrders!=null)
                            ServiceOrders.Clear();
                        if(ServiceOrdersByTime != null)
                            ServiceOrdersByTime.Clear();
                        foreach(NewServiceorderExtensionBase item in _serviceorders) {
                            if(string.IsNullOrEmpty(item.NewTime))
                                ServiceOrders.Add(item);
                            else
                                ServiceOrdersByTime.Add(item);
                        }
                        TimeServiceOrder = "Временные (" + ServiceOrdersByTime.Count.ToString() + ")";
                        OtherServiceOrder = "Прочие (" + ServiceOrders.Count.ToString() + ")";
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
                    using HttpClient client = new HttpClient(GetHttpClientHandler());
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceOrderForFireAlarmExtensionBase/ServiceOrderByUser?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);
                    List<NewTest2ExtensionBase> _serviceorders = new List<NewTest2ExtensionBase>();
                    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                        var resp = response.Content.ReadAsStringAsync().Result;
                        try {
                            _serviceorders = JsonConvert.DeserializeObject<List<NewTest2ExtensionBase>>(resp).ToList();
                        }
                        catch(Exception ex) {
                            _serviceorders = null;
                            Crashes.TrackError(new Exception("Ошибка десериализации результата запроса(Заявки технику ПС)"),
                            new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                {"ErrorMessage",ex.Message },
                                {"StatusCode",response.StatusCode.ToString() },
                                {"Response",response.ToString() }
                            });
                        }
                    }
                    else {
                        _serviceorders = null;
                        Crashes.TrackError(new Exception("Ошибка запроса(Заявки технику)"),
                        new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                {"StatusCode",response.StatusCode.ToString() },
                                {"Response",response.ToString() }
                        });
                    }
                    if(_serviceorders != null) {
                        if(ServiceOrdersFireAlarm != null)
                            ServiceOrdersFireAlarm.Clear();
                        if(ServiceOrdersByTimeFireAlarm != null)
                            ServiceOrdersByTimeFireAlarm.Clear();
                        foreach(NewTest2ExtensionBase item in _serviceorders) {
                            if(string.IsNullOrEmpty(item.NewTime))
                                ServiceOrdersFireAlarm.Add(item);
                            else
                                ServiceOrdersByTimeFireAlarm.Add(item);
                        }
                        //TimeServiceOrder = "Временные (" + ServiceOrdersByTime.Count.ToString() + ")";
                        //OtherServiceOrder = "Прочие (" + ServiceOrders.Count.ToString() + ")";
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
                    using HttpClient client = new HttpClient(GetHttpClientHandler());
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/ServiceOrderByUserTransferReason?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);
                    List<NewServiceorderExtensionBase> _serviceorders = new List<NewServiceorderExtensionBase>();
                    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                        var resp = response.Content.ReadAsStringAsync().Result;
                        try {
                            _serviceorders = JsonConvert.DeserializeObject<List<NewServiceorderExtensionBase>>(resp).ToList();
                        }
                        catch(Exception ex) {
                            _serviceorders = null;
                            Crashes.TrackError(new Exception("Ошибка десериализации результата запроса(Заявки технику - переносы)"),
                            new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                {"ErrorMessage",ex.Message },
                                {"StatusCode",response.StatusCode.ToString() },
                                {"Response",response.ToString() }
                            });
                        }
                    }
                    else {
                        _serviceorders = null;
                        Crashes.TrackError(new Exception("Ошибка запроса(Заявки технику - переносы)"),
                        new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                {"StatusCode",response.StatusCode.ToString() },
                                {"Response",response.ToString() }
                        });
                    }
                    if(_serviceorders != null) {
                        if(ServiceOrderByTransfer!=null)
                            ServiceOrderByTransfer.Clear();
                        foreach(NewServiceorderExtensionBase item in _serviceorders) {
                            ServiceOrderByTransfer.Add(item);
                        }
                        TransferServiceOrder = "Перенесенные (" + ServiceOrderByTransfer.Count.ToString() + ")";
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
                    ///api/NewServiceorderExtensionBases/ServiceOrderByUser?usr_ID=FEF46B07-8D7A-E311-920A-00155D01051D&date=18.11.2020
                    if(Date == DateTime.Parse("01.01.0001 00:00:00"))
                        Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
                    using HttpClient client = new HttpClient(GetHttpClientHandler());
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceOrderForFireAlarmExtensionBase/ServiceOrderByUserTransferReason?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);
                    List<NewTest2ExtensionBase> _serviceorders = new List<NewTest2ExtensionBase>();
                    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                        var resp = response.Content.ReadAsStringAsync().Result;
                        try {
                            _serviceorders = JsonConvert.DeserializeObject<List<NewTest2ExtensionBase>>(resp).ToList();
                        }
                        catch(Exception ex) {
                            _serviceorders = null;
                            Crashes.TrackError(new Exception("Ошибка десериализации результата запроса(Заявки технику - переносы(ПС))"),
                            new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                {"ErrorMessage",ex.Message },
                                {"StatusCode",response.StatusCode.ToString() },
                                {"Response",response.ToString() }
                            });
                        }
                    }
                    else {
                        _serviceorders = null;
                        Crashes.TrackError(new Exception("Ошибка запроса(Заявки технику - переносы)"),
                        new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                {"StatusCode",response.StatusCode.ToString() },
                                {"Response",response.ToString() }
                        });
                    }
                    if(_serviceorders != null) {
                        if(ServiceOrderByTransferFireAlarm != null)
                            ServiceOrderByTransferFireAlarm.Clear();
                        foreach(NewTest2ExtensionBase item in _serviceorders) {
                            ServiceOrderByTransferFireAlarm.Add(item);
                        }
                        //TransferServiceOrder = "Перенесенные (" + ServiceOrderByTransferFireAlarm.Count.ToString() + ")";
                    }
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            });
        }
        private RelayCommand _SelectServiceOrderCommand;
        public RelayCommand SelectServiceOrderCommand {
            get => _SelectServiceOrderCommand ??= new RelayCommand(async obj => {
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

        private ObservableCollection<MetadataModel> _Category = new ObservableCollection<MetadataModel>();
        public ObservableCollection<MetadataModel> Category {
            get => _Category;
            set {
                _Category = value;
                OnPropertyChanged(nameof(Category));
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

        private ObservableCollection<NewServiceorderExtensionBase> _ServiceOrders = new ObservableCollection<NewServiceorderExtensionBase>();
        public ObservableCollection<NewServiceorderExtensionBase> ServiceOrders {
            get => _ServiceOrders;
            set {
                _ServiceOrders = value;
                OnPropertyChanged(nameof(ServiceOrders));
            }
        }

        private ObservableCollection<NewServiceorderExtensionBase> _ServiceOrderByTransfer = new ObservableCollection<NewServiceorderExtensionBase>();
        public ObservableCollection<NewServiceorderExtensionBase> ServiceOrderByTransfer {
            get => _ServiceOrderByTransfer;
            set {
                _ServiceOrderByTransfer = value;
                OnPropertyChanged(nameof(ServiceOrderByTransfer));
            }
        }
        private ObservableCollection<NewServiceorderExtensionBase> _ServiceOrdersByTime = new ObservableCollection<NewServiceorderExtensionBase>();
        public ObservableCollection<NewServiceorderExtensionBase> ServiceOrdersByTime {
            get => _ServiceOrdersByTime;
            set {
                _ServiceOrdersByTime = value;
                OnPropertyChanged(nameof(ServiceOrdersByTime));
            }
        }

        private ObservableCollection<NewTest2ExtensionBase> _ServiceOrdersByTimeFireAlarm=new ObservableCollection<NewTest2ExtensionBase>();
        public ObservableCollection<NewTest2ExtensionBase> ServiceOrdersByTimeFireAlarm {
            get => _ServiceOrdersByTimeFireAlarm;
            set {
                _ServiceOrdersByTimeFireAlarm = value;
                OnPropertyChanged(nameof(ServiceOrdersByTimeFireAlarm));
            }
        }

        private ObservableCollection<NewTest2ExtensionBase> _ServiceOrdersFireAlarm=new ObservableCollection<NewTest2ExtensionBase>();
        public ObservableCollection<NewTest2ExtensionBase> ServiceOrdersFireAlarm {
            get => _ServiceOrdersFireAlarm;
            set {
                _ServiceOrdersFireAlarm = value;
                OnPropertyChanged(nameof(ServiceOrdersFireAlarm));
            }
        }

        private ObservableCollection<NewTest2ExtensionBase> _ServiceOrderByTransferFireAlarm=new ObservableCollection<NewTest2ExtensionBase>();
        public ObservableCollection<NewTest2ExtensionBase> ServiceOrderByTransferFireAlarm {
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
                    ArrowCircleTransferServiceOrder = "arrow_circle_up.png";
                else
                    ArrowCircleTransferServiceOrder = "arrow_circle_down.png";
                OnPropertyChanged(nameof(TransferServiceOrderExpanded));
            }
        }

        private bool _TimeServiceOrderExpanded;
        public bool TimeServiceOrderExpanded {
            get => _TimeServiceOrderExpanded;
            set {
                _TimeServiceOrderExpanded = value;
                if(_TimeServiceOrderExpanded)
                    ArrowCircleTimeServiceOrder = "arrow_circle_up.png";
                else
                    ArrowCircleTimeServiceOrder = "arrow_circle_down.png";
                OnPropertyChanged(nameof(TimeServiceOrderExpanded));
            }
        }

        private bool _OtherServiceOrderExpanded;
        public bool OtherServiceOrderExpanded {
            get => _OtherServiceOrderExpanded;
            set {
                _OtherServiceOrderExpanded = value;
                if(_OtherServiceOrderExpanded)
                    ArrowCircleOtherServiceOrder = "arrow_circle_up.png";
                else
                    ArrowCircleOtherServiceOrder = "arrow_circle_down.png";
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
