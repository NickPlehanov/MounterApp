﻿using Android.Widget;
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
    /// <summary>
    /// ViewModel закрытия заявки технику + заявки на пс
    /// Объекты so(NewServiceorderExtensionBase) и sofa(NewTest2ExtensionBase) заполняются в момент вызова конструктора заявкой технику или заявкой на пс, соответсвенно
    /// </summary>
    public class CloseOrderPopupPageViewModel : BaseViewModel {
        /// <summary>
        /// Конструктор для закрытия заявки технику
        /// </summary>
        /// <param name="soeb">Объект Заявка технику</param>
        /// <param name="_servicemans">Техники(список)</param>
        /// <param name="_mounters">Монтажники(список)</param>
        public CloseOrderPopupPageViewModel(NewServiceorderExtensionBase soeb,List<NewServicemanExtensionBase> _servicemans,List<NewMounterExtensionBase> _mounters) {
            so = soeb;
            Servicemans = _servicemans;
            Mounters = _mounters;
            GetResults.ExecuteAsync(null);
            NecesseryRead = false;
            SaveImage = "save.png";
        }
        /// <summary>
        /// Конструктор для закрытия заявки на ПС
        /// </summary>
        /// <param name="soeb">Объект заявка на ПС</param>
        /// <param name="_servicemans">Техники(список)</param>
        /// <param name="_mounters">Монтажники(список)</param>
        public CloseOrderPopupPageViewModel(NewTest2ExtensionBase soeb,List<NewServicemanExtensionBase> _servicemans,List<NewMounterExtensionBase> _mounters) {
            sofa = soeb;
            Servicemans = _servicemans;
            Mounters = _mounters;
            GetResults.ExecuteAsync(null);
            NecesseryRead = false;
            SaveImage = "save.png";
        }
        /// <summary>
        /// Объект для хранения заявки на ПС, заполянется в конструкторе
        /// </summary>
        private NewTest2ExtensionBase _sofa;
        public NewTest2ExtensionBase sofa {
            get => _sofa;
            set {
                _sofa = value;
                OnPropertyChanged(nameof(sofa));
            }
        }
        /// <summary>
        /// Для хранения картинки на кнопке Сохранить
        /// </summary>
        private ImageSource _SaveImage;
        public ImageSource SaveImage {
            get => _SaveImage;
            set {
                _SaveImage = value;
                OnPropertyChanged(nameof(SaveImage));
            }
        }
        /// <summary>
        /// По нажатию на флажок "Обязательно для прочтения оператором" меняет его значение на противоположное от исходного
        /// </summary>
        private RelayCommand _SetValueNecesseryReadCommand;
        public RelayCommand SetValueNecesseryReadCommand {
            get => _SetValueNecesseryReadCommand ??= new RelayCommand(async obj => {
                NecesseryRead = !NecesseryRead;
            });
        }
        //private RelayCommand _IncomeCommand;
        //public RelayCommand IncomeCommand {
        //    get => _IncomeCommand ??= new RelayCommand(async obj => {
        //        Analytics.TrackEvent("Заявка технику: вызов команды Пришел. Попытка получения координат",
        //        new Dictionary<string,string> {
        //            {"ServiceOrderID",so.NewServiceorderId.ToString() },
        //            {"Serviceman",Servicemans.FirstOrDefault().NewPhone}
        //        });
        //        //OpacityForm = 0.1;
        //        //IndicatorVisible = true;
        //        Location location = await Geolocation.GetLastKnownLocationAsync();
        //        if(location != null) {
        //            Latitude = location.Latitude.ToString();
        //            Longitude = location.Longitude.ToString();
        //        }
        //        Analytics.TrackEvent("Запрос данных на сервере (могло же что-то измениться",
        //        new Dictionary<string,string> {
        //            {"ServiceOrderID",so.NewServiceorderId.ToString() }
        //        });
        //        using HttpClient client = new HttpClient(GetHttpClientHandler());
        //        HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/id?id=" + so.NewServiceorderId);
        //        NewServiceorderExtensionBase soeb = null;
        //        if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
        //            var resp = response.Content.ReadAsStringAsync().Result;
        //            try {
        //                soeb = JsonConvert.DeserializeObject<NewServiceorderExtensionBase>(resp);
        //            }
        //            catch(Exception ex) {
        //                Crashes.TrackError(new Exception("Ошибка десериализации объекта заявка технику"),
        //                new Dictionary<string,string> {
        //                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
        //                {"ErrorMessage",ex.Message },
        //                {"StatusCode",response.StatusCode.ToString() }
        //                });
        //            }
        //        }
        //        else
        //            Crashes.TrackError(new Exception("Ошибка получения данных об объекте заявка технику с сервера"),
        //            new Dictionary<string,string> {
        //            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
        //            {"StatusCode",response.StatusCode.ToString() },
        //            {"Response",response.ToString() }
        //            });
        //        if(soeb != null) {
        //            Analytics.TrackEvent("Попытка записи данных на сервер по объекту заявка технику, заполняем поле Пришел",
        //            new Dictionary<string,string> {
        //                {"ServiceOrderID",so.NewServiceorderId.ToString() }
        //            });
        //            soeb.NewIncome = DateTime.Now.AddHours(-5);
        //            using HttpClient clientPut = new HttpClient(GetHttpClientHandler());
        //            var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
        //            HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases",httpContent);
        //            if(!responsePut.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
        //                Crashes.TrackError(new Exception("Ошибка при сохранении объекта Заявка технику"),
        //                new Dictionary<string,string> {
        //                {"ServerResponse",responsePut.Content.ReadAsStringAsync().Result },
        //                {"StatusCode",responsePut.StatusCode.ToString() },
        //                {"Response",responsePut.ToString() }
        //                });
        //            }
        //            else
        //                Toast.MakeText(Android.App.Application.Context,"Время прихода записано",ToastLength.Long).Show();
        //        }
        //        //запишем координаты
        //        Analytics.TrackEvent("Попытка записи координат на сервер по объекту заявка технику",
        //            new Dictionary<string,string> {
        //                {"ServiceOrderID",so.NewServiceorderId.ToString() }
        //            });
        //        using(HttpClient clientPost = new HttpClient(GetHttpClientHandler())) {
        //            var data = JsonConvert.SerializeObject(new ServiceOrderCoordinates() {
        //                SocId = Guid.NewGuid(),
        //                SocServiceOrderId = so.NewServiceorderId,
        //                SocIncomeLatitude = Latitude,
        //                SocIncomeLongitude = Longitude
        //            });
        //            StringContent content = new StringContent(data,Encoding.UTF8,"application/json");
        //            HttpResponseMessage responsePost = await clientPost.PostAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates",content);
        //            if(!responsePost.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
        //                Crashes.TrackError(new Exception("Ошибка при сохранении объекта Заявка технику"),
        //                new Dictionary<string,string> {
        //                {"ServerResponse",responsePost.Content.ReadAsStringAsync().Result },
        //                {"StatusCode",responsePost.StatusCode.ToString() },
        //                {"Response",responsePost.ToString() }
        //                });
        //            }
        //        }
        //        //Opacity = 1;
        //        //IndicatorVisible = false;
        //    },obj => so.NewIncome == null);
        //}
        /// <summary>
        /// Список монтажников заполняется в конструкторе
        /// TODO: стоило бы передавать объект, а не список, но исторически так сложилось
        /// </summary>
        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
        }
        /// <summary>
        /// Заключение по заявке
        /// </summary>
        private string _ConclusionByOrder;
        public string ConclusionByOrder {
            get => _ConclusionByOrder;
            set {
                _ConclusionByOrder = value;
                OnPropertyChanged(nameof(ConclusionByOrder));
            }
        }
        /// <summary>
        /// Обязательно для прочтения оператором
        /// </summary>
        private bool _NecesseryRead;
        public bool NecesseryRead {
            get => _NecesseryRead;
            set {
                _NecesseryRead = value;
                OnPropertyChanged(nameof(NecesseryRead));
            }
        }
        /// <summary>
        /// Объект для хранения заявки технику, заполянется в конструкторе
        /// </summary>
        private NewServiceorderExtensionBase _so;
        public NewServiceorderExtensionBase so {
            get => _so;
            set {
                _so = value;
                OnPropertyChanged(nameof(so));
            }
        }
        /// <summary>
        /// Список техников заполняется в конструкторе, стоило бы так же передавать объект, а не список (TODO)
        /// </summary>
        private List<NewServicemanExtensionBase> _Servicemans;
        public List<NewServicemanExtensionBase> Servicemans {
            get => _Servicemans;
            set {
                _Servicemans = value;
                OnPropertyChanged(nameof(Servicemans));
            }
        }
        /// <summary>
        /// Условие видимости списка причин, при условии указания, что заявка Выполнена - <see langword="false"/>, иначе (Перенос, Отмена) - "<see langword="true"/>"
        /// </summary>
        private bool _ReasonVisibility;
        public bool ReasonVisibility {
            get => _ReasonVisibility;
            set {
                _ReasonVisibility = value;
                OnPropertyChanged(nameof(ReasonVisibility));
            }
        }
        /// <summary>
        /// Объект хранящий выбранную причину для отмены или переноса
        /// </summary>
        private MetadataModel _SelectedReason;
        public MetadataModel SelectedReason {
            get => _SelectedReason;
            set {
                _SelectedReason = value;
                OnPropertyChanged(nameof(SelectedReason));
            }
        }
        /// <summary>
        /// Объект хранит выбранный пользователем результат по заявке: Выполнено, Отмена, Перенос 
        /// </summary>
        private MetadataModel _SelectedResult;
        public MetadataModel SelectedResult {
            get => _SelectedResult;
            set {
                _SelectedResult = value;
                OnPropertyChanged(nameof(SelectedResult));
                if(SelectedResult.Value != 1) {
                    ReasonVisibility = true;
                    GetReasons.ExecuteAsync(null);
                }
                else
                    ReasonVisibility = false;
                if(SelectedResult.Value == 2) {
                    TransferLayoutVisibility = true;
                }
                if(SelectedResult.Value == 3) {
                    TransferLayoutVisibility = false;
                }
            }
        }
        /// <summary>
        /// Отслеживаемая коллекция для хранения всех вариантов результатов для закрытя заявки
        /// </summary>
        private ObservableCollection<MetadataModel> _Results = new ObservableCollection<MetadataModel>();
        public ObservableCollection<MetadataModel> Results {
            get => _Results;
            set {
                _Results = value;
                OnPropertyChanged(nameof(Results));
            }
        }
        /// <summary>
        /// Отслеживаемая коллекция для хранения всех вариантов причин для отмены и переноса заявки технику
        /// </summary>
        private ObservableCollection<MetadataModel> _Reasons = new ObservableCollection<MetadataModel>();
        public ObservableCollection<MetadataModel> Reasons {
            get => _Reasons;
            set {
                _Reasons = value;
                OnPropertyChanged(nameof(_Reasons));
            }
        }
        /// <summary>
        /// Комментарий к причине Отмены или Переноса
        /// </summary>
        private string _ReasonComment;
        public string ReasonComment {
            get => _ReasonComment;
            set {
                _ReasonComment = value;
                OnPropertyChanged(nameof(ReasonComment));
            }
        }
        /// <summary>
        /// Команда для получения списка причин для отмены и переноса заявки технику
        /// </summary>
        private AsyncCommand _GetReasons;
        public AsyncCommand GetReasons {
            get => _GetReasons ??= new AsyncCommand(async () => {
                Analytics.TrackEvent("Форма закрытия заявок. Получение причин для переноса заявки",
                new Dictionary<string,string> {
                    {"Query","Common/metadata?ColumnName=New_resultid&ObjectName=New_serviceorder" }
                });
                using HttpClient client = new HttpClient(GetHttpClientHandler());
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/metadata?ColumnName=New_resultid&ObjectName=New_serviceorder");
                List<MetadataModel> mm = new List<MetadataModel>();
                if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                    var resp = response.Content.ReadAsStringAsync().Result;
                    try {
                        mm = JsonConvert.DeserializeObject<List<MetadataModel>>(resp);
                    }
                    catch(Exception ex) {
                        mm = null;
                        Crashes.TrackError(new Exception("Форма закрытия заявок. Ошибка десереализации причин для переноса заявки"),
                        new Dictionary<string,string> {
                            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                            {"ErrorMessage",ex.Message },
                            {"StatusCode",response.StatusCode.ToString() },
                            {"Response",response.ToString() },
                            {"Query","Common/metadata?ColumnName=New_resultid&ObjectName=New_serviceorder" }
                        });
                    }
                    if(mm != null) {
                        Reasons.Clear();
                        foreach(MetadataModel item in mm)
                            Reasons.Add(item);
                    }
                    else {
                        Analytics.TrackEvent("Форма закрытия заявок. Не получен список причин. Список причин пустой");
                    }
                }
                else
                    Crashes.TrackError(new Exception("Форма закрытия заявок. От сервера не получен корректный ответ"),
                        new Dictionary<string,string> {
                            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                            {"StatusCode",response.StatusCode.ToString() },
                            {"Response",response.ToString() },
                            {"Query","Common/metadata?ColumnName=New_resultid&ObjectName=New_serviceorder" }
                        });
            });
        }
        /// <summary>
        /// Команда для получения списка результатов по заявке
        /// </summary>
        private AsyncCommand _GetResults;
        public AsyncCommand GetResults {
            get => _GetResults ??= new AsyncCommand(async () => {
                Analytics.TrackEvent("Форма закрытия заявок. Получение результата работы по заявке",
                new Dictionary<string,string> {
                    {"Query","Common/metadata?ColumnName=New_result&ObjectName=New_serviceorder" }
                });
                using HttpClient client = new HttpClient(GetHttpClientHandler());
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/metadata?ColumnName=New_result&ObjectName=New_serviceorder");
                List<MetadataModel> mm = new List<MetadataModel>();
                if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                    var resp = response.Content.ReadAsStringAsync().Result;
                    try {
                        mm = JsonConvert.DeserializeObject<List<MetadataModel>>(resp);
                    }
                    catch(Exception ex) {
                        mm = null;
                        Crashes.TrackError(new Exception("Форма закрытия заявок. Ошибка десереализации результатов по заявке"),
                        new Dictionary<string,string> {
                            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                            {"ErrorMessage",ex.Message },
                            {"StatusCode",response.StatusCode.ToString() },
                            {"Response",response.ToString() },
                            {"Query","Common/metadata?ColumnName=New_result&ObjectName=New_serviceorder" }
                        });
                    }
                    if(mm != null) {
                        Results.Clear();
                        foreach(MetadataModel item in mm)
                            Results.Add(item);
                    }
                    else {
                        Analytics.TrackEvent("Форма закрытия заявок. Не получен список результатов. Список результатов пустой");
                    }
                }
                else
                    Crashes.TrackError(new Exception("Форма закрытия заявок. От сервера не получен корректный ответ"),
                        new Dictionary<string,string> {
                            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                            {"StatusCode",response.StatusCode.ToString() },
                            {"Response",response.ToString() },
                            {"Query","Common/metadata?ColumnName=New_result&ObjectName=New_serviceorder" }
                        });
            });
        }
        //private bool _IsTransfer;
        //public bool IsTransfer {
        //    get => _IsTransfer;
        //    set {
        //        _IsTransfer = value;
        //        OnPropertyChanged(nameof(IsTransfer));
        //    }
        //}

        //private bool _IsCancel;
        //public bool IsCancel {
        //    get => _IsCancel;
        //    set {
        //        _IsCancel = value;
        //        OnPropertyChanged(nameof(IsCancel));
        //    }
        //}
        /// <summary>
        /// Объект для хранения даты переноса 
        /// </summary>
        private DateTime _TransferDate;
        public DateTime TransferDate {
            get => _TransferDate;
            set {
                if(value == DateTime.Parse("01.01.2010 00:00:00"))
                    _TransferDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy")).AddDays(1);
                else
                    _TransferDate = value;
                OnPropertyChanged(nameof(TransferDate));
            }
        }
        /// <summary>
        /// Объект для хранения времени переноса
        /// </summary>
        private TimeSpan _TransferTime;
        public TimeSpan TransferTime {
            get => _TransferTime;
            set {
                _TransferTime = value;
                OnPropertyChanged(nameof(TransferTime));
            }
        }
        /// <summary>
        /// Свойство отвечающее за видимость блока с контролами для переноса заявки 
        /// </summary>
        private bool _TransferLayoutVisibility;
        public bool TransferLayoutVisibility {
            get => _TransferLayoutVisibility;
            set {
                _TransferLayoutVisibility = value;
                OnPropertyChanged(nameof(TransferLayoutVisibility));
            }
        }
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
        /// Команда закрытия заявки технику
        /// </summary>
        private RelayCommand _CloseServiceOrderCommand;
        public RelayCommand CloseServiceOrderCommand {
            get => _CloseServiceOrderCommand ??= new RelayCommand(async obj => {                
                Analytics.TrackEvent("Начало закрытия заявки",
                    new Dictionary<string,string> {
                        {"ServiceOrder",so!=null ? so.NewServiceorderId.ToString() : sofa.NewTest2Id.ToString() },
                        {"ServicemanPhone",Servicemans.FirstOrDefault().NewPhone }
                    });
                if(NecesseryRead && string.IsNullOrEmpty(ConclusionByOrder)) {
                    Analytics.TrackEvent("Не указано заключение по заявке",
                    new Dictionary<string,string> {
                        {"ServiceOrder",so!=null ? so.NewServiceorderId.ToString() : sofa.NewTest2Id.ToString() },
                        {"ServicemanPhone",Servicemans.FirstOrDefault().NewPhone }
                    });
                    await Application.Current.MainPage.DisplayAlert("Ошибка","Не указано заключение по заявке","OK");
                    return;
                }
                if(SelectedResult == null) {
                    Analytics.TrackEvent("Не выбрана результат",
                    new Dictionary<string,string> {
                        {"ServiceOrder",so!=null ? so.NewServiceorderId.ToString() : sofa.NewTest2Id.ToString() },
                        {"ServicemanPhone",Servicemans.FirstOrDefault().NewPhone }
                    });
                    await Application.Current.MainPage.DisplayAlert("Ошибка","Не выбрана результат","OK");
                    return;
                }
                if(SelectedResult.Value != 1) {
                    if(SelectedReason != null && !string.IsNullOrEmpty(ReasonComment) && !string.IsNullOrEmpty(ConclusionByOrder)) {
                        HttpResponseMessage response = null;
                        using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                            if(so != null) {
                                NewServiceorderExtensionBase soeb = null;
                                response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/id?id=" + so.NewServiceorderId);
                                var resp = response.Content.ReadAsStringAsync().Result;
                                if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                                    try {
                                        soeb = JsonConvert.DeserializeObject<NewServiceorderExtensionBase>(resp);
                                    }
                                    catch(Exception ex) {
                                        soeb = null;
                                        Crashes.TrackError(new Exception("Ошибка десериализации результата запроса(информация о заявке технику)"),
                                        new Dictionary<string,string> {
                                    {"Servicemans",Servicemans.First().NewPhone },
                                    {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                    {"ErrorMessage",ex.Message },
                                    {"StatusCode",response.StatusCode.ToString() },
                                    {"Response",response.ToString() },
                                    {"Query","/api/NewServiceorderExtensionBases/id?id=" + so.NewServiceorderId.ToString() }
                                        });
                                    }
                                    if(soeb != null) {
                                        soeb.NewOutgone = DateTime.Now.AddHours(-5);
                                        //soeb.NewNewServiceman = Servicemans.FirstOrDefault().NewServicemanId;
                                        soeb.NewResult = SelectedResult.Value;
                                        soeb.NewResultId = SelectedReason.Value;
                                        soeb.NewTransferReason = ReasonComment;
                                        soeb.NewTechConclusion = ConclusionByOrder;
                                        soeb.NewMustRead = NecesseryRead;
                                        if(SelectedResult.Value == 2) {
                                            Analytics.TrackEvent("Указание времени переноса",
                                            new Dictionary<string,string> {
                                        {"ServiceOrder",so.NewServiceorderId.ToString() },
                                        {"ServicemanPhone",Servicemans.FirstOrDefault().NewPhone }
                                            });
                                            soeb.NewMoved = new DateTime(TransferDate.Year,TransferDate.Month,TransferDate.Day,TransferTime.Hours,TransferTime.Minutes,TransferTime.Seconds).AddHours(-5);
                                            //soeb.NewMoved = new DateTime(TransferDate.Year,TransferDate.Month,TransferDate.Day,0,0,0).AddHours(-5);
                                        }
                                        using(HttpClient clientPut = new HttpClient(GetHttpClientHandler())) {
                                            Analytics.TrackEvent("Сохранение информации о заявке технику",
                                            new Dictionary<string,string> {
                                        {"ServiceOrder",so.NewServiceorderId.ToString() },
                                        {"ServicemanPhone",Servicemans.FirstOrDefault().NewPhone }
                                            });
                                            var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
                                            HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases",httpContent);
                                            if(!responsePut.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                                                Crashes.TrackError(new Exception("При сохранении информации о заявке технику, произошла ошибка, не был получен корректный ответ"),
                                                new Dictionary<string,string> {
                                            {"ServiceOrder",so.NewServiceorderId.ToString() },
                                            {"Servicemans",Servicemans.First().NewPhone },
                                            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                            //{"ErrorMessage",ex.Message },
                                            {"StatusCode",response.StatusCode.ToString() },
                                            {"Response",response.ToString() }
                                                });
                                                await Application.Current.MainPage.DisplayAlert("Ошибка","При сохранении информации о заявке технику, произошла ошибка, не был получен корректный ответ от сервера. Попробуйте позже, в случае повторной ошибки, сообщите в ИТ-отдел.","OK");
                                            }
                                            else
                                                Toast.MakeText(Android.App.Application.Context,"Информация о заявке сохранена успешно",ToastLength.Long).Show();
                                        }
                                        Analytics.TrackEvent("Попытка получения координат (Перенос/Отмена)");
                                        //запишем координаты
                                        Location location = await Geolocation.GetLastKnownLocationAsync();
                                        if(location != null) {
                                            Latitude = location.Latitude.ToString();
                                            Longitude = location.Longitude.ToString();
                                        }
                                        using HttpClient clientGet = new HttpClient(GetHttpClientHandler());
                                        HttpResponseMessage responseGet = await clientGet.GetAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates/id?so_id=" + so.NewServiceorderId);
                                        List<ServiceOrderCoordinates> soc = null;
                                        if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                                            var respGet = responseGet.Content.ReadAsStringAsync().Result;
                                            try {
                                                soc = JsonConvert.DeserializeObject<List<ServiceOrderCoordinates>>(respGet).Where(x => x.SocOutcomeLatitide == null && x.SocOutcomeLongitude == null).ToList();
                                            }
                                            catch(Exception ex) {
                                                soc = null;
                                                Crashes.TrackError(new Exception("Ошибка десериализации при получении координат, для формирования запроса"),
                                                new Dictionary<string,string> {
                                            {"Servicemans",Servicemans.First().NewPhone },
                                            {"ServerResponse",responseGet.Content.ReadAsStringAsync().Result },
                                            {"ErrorMessage",ex.Message },
                                            {"StatusCode",responseGet.StatusCode.ToString() },
                                            {"Response",responseGet.ToString() },
                                            {"Query","/api/ServiceOrderCoordinates/id?so_id=" + so.NewServiceorderId.ToString() }
                                                });
                                            }
                                            if(soc != null) {
                                                using(HttpClient clientPut = new HttpClient(GetHttpClientHandler())) {
                                                    var httpContent = new StringContent(JsonConvert.SerializeObject(soc.First()),Encoding.UTF8,"application/json");
                                                    HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates",httpContent);
                                                }
                                            }
                                            else {
                                                Crashes.TrackError(new Exception("Не найдены запись с координатами прихода техника на объект по данной заявке"),
                                                new Dictionary<string,string> {
                                            {"Servicemans",Servicemans.First().NewPhone },
                                            {"ServiceOrder", so.NewServiceorderId.ToString() }
                                                });
                                            }
                                        }
                                    }
                                    await App.Current.MainPage.Navigation.PopPopupAsync(true);
                                    ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Servicemans,Mounters);
                                    App.Current.MainPage = new ServiceOrdersPage(vm);
                                }
                                else
                                    Crashes.TrackError(new Exception("При запросе информация о заявке технику от сервера пришел ответ с ошибкой"),
                                    new Dictionary<string,string> {
                                        {"Servicemans",Servicemans.First().NewPhone },
                                        {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                        {"StatusCode",response.StatusCode.ToString() },
                                        {"Response",response.ToString() },
                                        {"Query","/api/NewServiceorderExtensionBases/id?id=" + so.NewServiceorderId.ToString() }
                                    });
                            }
                            else if(sofa != null) {
                                NewTest2ExtensionBase soeb = null;
                                response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceOrderForFireAlarmExtensionBase/id?id=" + sofa.NewTest2Id);
                                var resp = response.Content.ReadAsStringAsync().Result;
                                if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                                    try {
                                        soeb = JsonConvert.DeserializeObject<NewTest2ExtensionBase>(resp);
                                    }
                                    catch(Exception ex) {
                                        soeb = null;
                                        Crashes.TrackError(new Exception("Ошибка десериализации результата запроса(информация о заявке технику)"),
                                        new Dictionary<string,string> {
                                    {"Servicemans",Servicemans.First().NewPhone },
                                    {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                    {"ErrorMessage",ex.Message },
                                    {"StatusCode",response.StatusCode.ToString() },
                                    {"Response",response.ToString() },
                                    {"Query","/api/NewServiceOrderForFireAlarmExtensionBase/id?id=" + sofa.NewTest2Id.ToString() }
                                        });
                                    }
                                    if(soeb != null) {
                                        soeb.NewOutgone = DateTime.Now.AddHours(-5);
                                        //soeb.NewNewServiceman = Servicemans.FirstOrDefault().NewServicemanId;
                                        soeb.NewResult = SelectedResult.Value;
                                        soeb.NewResultid = SelectedReason.Value;
                                        soeb.NewTransferReason = ReasonComment;
                                        soeb.NewTechconclusion = ConclusionByOrder;
                                        if(SelectedResult.Value == 2) {
                                            Analytics.TrackEvent("Указание времени переноса",
                                            new Dictionary<string,string> {
                                        {"ServiceOrder",sofa.NewTest2Id.ToString() },
                                        {"ServicemanPhone",Servicemans.FirstOrDefault().NewPhone }
                                            });
                                            soeb.NewMoved = new DateTime(TransferDate.Year,TransferDate.Month,TransferDate.Day,TransferTime.Hours,TransferTime.Minutes,TransferTime.Seconds).AddHours(-5);
                                            //soeb.NewMoved = new DateTime(TransferDate.Year,TransferDate.Month,TransferDate.Day,0,0,0).AddHours(-5);
                                        }
                                        using(HttpClient clientPut = new HttpClient(GetHttpClientHandler())) {
                                            Analytics.TrackEvent("Сохранение информации о заявке технику(пс)",
                                            new Dictionary<string,string> {
                                        {"ServiceOrder",sofa.NewTest2Id.ToString() },
                                        {"ServicemanPhone",Servicemans.FirstOrDefault().NewPhone }
                                            });
                                            var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
                                            HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceOrderForFireAlarmExtensionBase",httpContent);
                                            if(!responsePut.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                                                Crashes.TrackError(new Exception("При сохранении информации о заявке технику(пс), произошла ошибка, не был получен корректный ответ"),
                                                new Dictionary<string,string> {
                                            {"ServiceOrder",sofa.NewTest2Id.ToString()  },
                                            {"Servicemans",Servicemans.First().NewPhone },
                                            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                            //{"ErrorMessage",ex.Message },
                                            {"StatusCode",response.StatusCode.ToString() },
                                            {"Response",response.ToString() }
                                                });
                                                await Application.Current.MainPage.DisplayAlert("Ошибка","При сохранении информации о заявке технику, произошла ошибка, не был получен корректный ответ от сервера. Попробуйте позже, в случае повторной ошибки, сообщите в ИТ-отдел.","OK");
                                            }
                                            else
                                                Toast.MakeText(Android.App.Application.Context,"Информация о заявке сохранена успешно",ToastLength.Long).Show();
                                        }
                                        Analytics.TrackEvent("Попытка получения координат (Перенос/Отмена)");
                                        //запишем координаты
                                        Location location = await Geolocation.GetLastKnownLocationAsync();
                                        if(location != null) {
                                            Latitude = location.Latitude.ToString();
                                            Longitude = location.Longitude.ToString();
                                        }
                                        using HttpClient clientGet = new HttpClient(GetHttpClientHandler());
                                        HttpResponseMessage responseGet = await clientGet.GetAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates/id?so_id=" + sofa.NewTest2Id);
                                        List<ServiceOrderCoordinates> soc = null;
                                        if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                                            var respGet = responseGet.Content.ReadAsStringAsync().Result;
                                            try {
                                                soc = JsonConvert.DeserializeObject<List<ServiceOrderCoordinates>>(respGet).Where(x => x.SocOutcomeLatitide == null && x.SocOutcomeLongitude == null).ToList();
                                            }
                                            catch(Exception ex) {
                                                soc = null;
                                                Crashes.TrackError(new Exception("Ошибка десериализации при получении координат, для формирования запроса"),
                                                new Dictionary<string,string> {
                                            {"Servicemans",Servicemans.First().NewPhone },
                                            {"ServerResponse",responseGet.Content.ReadAsStringAsync().Result },
                                            {"ErrorMessage",ex.Message },
                                            {"StatusCode",responseGet.StatusCode.ToString() },
                                            {"Response",responseGet.ToString() },
                                            {"Query","/api/ServiceOrderCoordinates/id?so_id=" + sofa.NewTest2Id.ToString() }
                                                });
                                            }
                                            if(soc != null) {
                                                using(HttpClient clientPut = new HttpClient(GetHttpClientHandler())) {
                                                    var httpContent = new StringContent(JsonConvert.SerializeObject(soc.First()),Encoding.UTF8,"application/json");
                                                    HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates",httpContent);
                                                }
                                            }
                                            else {
                                                Crashes.TrackError(new Exception("Не найдены запись с координатами прихода техника на объект по данной заявке"),
                                                new Dictionary<string,string> {
                                            {"Servicemans",Servicemans.First().NewPhone },
                                            {"ServiceOrder", sofa.NewTest2Id.ToString() }
                                                });
                                            }
                                        }
                                    }
                                    await App.Current.MainPage.Navigation.PopPopupAsync(true);
                                    ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Servicemans,Mounters);
                                    App.Current.MainPage = new ServiceOrdersPage(vm);
                                }
                                else
                                    Crashes.TrackError(new Exception("При запросе информация о заявке технику от сервера пришел ответ с ошибкой"),
                                    new Dictionary<string,string> {
                                        {"Servicemans",Servicemans.First().NewPhone },
                                        {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                        {"StatusCode",response.StatusCode.ToString() },
                                        {"Response",response.ToString() },
                                        {"Query","/api/NewServiceorderExtensionBases/id?id=" + so.NewServiceorderId.ToString() }
                                    });
                            }
                        }

                        //using HttpClient client = new HttpClient(GetHttpClientHandler());
                        //HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/id?id=" + so.NewServiceorderId);
                        //NewServiceorderExtensionBase soeb = null;
                        //var resp = response.Content.ReadAsStringAsync().Result;

                    }
                    else
                        await Application.Current.MainPage.DisplayAlert("Ошибка","Не выбрана причина отмены(переноса), не указан комментарий к причине или заключение по заявке пустое","OK");
                }
                else {//выполнено
                    if(!string.IsNullOrEmpty(ConclusionByOrder)) {
                        HttpResponseMessage response = null;
                        using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                            if(so != null) {
                                NewServiceorderExtensionBase soeb = null;
                                response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/id?id=" + so.NewServiceorderId);
                                if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                                    var resp = response.Content.ReadAsStringAsync().Result;
                                    try {
                                        soeb = JsonConvert.DeserializeObject<NewServiceorderExtensionBase>(resp);
                                    }
                                    catch(Exception ex) {
                                        soeb = null;
                                        Crashes.TrackError(new Exception("Ошибка десериализации результата запроса для получения информации по заявке технику. Ветка(выполнено)"),
                                        new Dictionary<string,string> {
                                    {"Servicemans",Servicemans.First().NewPhone },
                                    {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                    {"ErrorMessage",ex.Message },
                                    {"StatusCode",response.StatusCode.ToString() },
                                    {"Response",response.ToString() },
                                    {"Query","NewServiceorderExtensionBases/id?id=" + so.NewServiceorderId.ToString() }
                                        });
                                    }
                                    if(soeb != null) {
                                        soeb.NewOutgone = DateTime.Now.AddHours(-5);
                                        soeb.NewNewServiceman = Servicemans.FirstOrDefault().NewServicemanId;
                                        soeb.NewResult = SelectedResult.Value;
                                        soeb.NewTechConclusion = ConclusionByOrder;
                                        soeb.NewMustRead = NecesseryRead;
                                        //soeb.NewResultId = SelectedReason.Value;
                                        //soeb.NewTransferReason = ReasonComment;
                                        //if(SelectedResult.Value == 2) {
                                        //    soeb.NewMoved = new DateTime(TransferDate.Year,TransferDate.Month,TransferDate.Day,TransferTime.Hours,TransferTime.Minutes,TransferTime.Seconds).AddHours(-5);
                                        //}
                                        using(HttpClient clientPut = new HttpClient(GetHttpClientHandler())) {
                                            var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
                                            HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases",httpContent);
                                            if(responsePut.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                                                Toast.MakeText(Android.App.Application.Context,"Заявка технику сохранена",ToastLength.Long).Show();
                                            }
                                            else {
                                                Crashes.TrackError(new Exception("Ошибка при сохранении заявки технику"),
                                                new Dictionary<string,string> {
                                            {"Servicemans",Servicemans.First().NewPhone },
                                            {"ServiceOrder",so.NewServiceorderId.ToString() },
                                            {"ServerResponse",responsePut.Content.ReadAsStringAsync().Result },
                                            {"StatusCode",responsePut.StatusCode.ToString() },
                                            {"Response",responsePut.ToString() }
                                                });
                                            }
                                        }
                                        //запишем координаты
                                        Analytics.TrackEvent("Попытка получения координат (Выполнено)");
                                        Location location = await Geolocation.GetLastKnownLocationAsync();
                                        if(location != null) {
                                            Latitude = location.Latitude.ToString();
                                            Longitude = location.Longitude.ToString();
                                        }
                                        using HttpClient clientGet = new HttpClient(GetHttpClientHandler());
                                        HttpResponseMessage responseGet = await clientGet.GetAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates/id?so_id=" + so.NewServiceorderId);
                                        ServiceOrderCoordinates soc = null;
                                        if(responseGet.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                                            var respGet = responseGet.Content.ReadAsStringAsync().Result;
                                            try {
                                                soc = JsonConvert.DeserializeObject<ServiceOrderCoordinates>(respGet);
                                            }
                                            catch(Exception ex) {
                                                Crashes.TrackError(new Exception("Ошибка десериализации результата запросакоординат по заявке "),
                                                new Dictionary<string,string> {
                                            {"Servicemans",Servicemans.First().NewPhone },
                                            {"Serviceorder",so.NewServiceorderId.ToString() },
                                            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                            {"ErrorMessage",ex.Message },
                                            {"StatusCode",response.StatusCode.ToString() },
                                            {"Response",response.ToString() }
                                                });
                                                soc = null;
                                            }
                                            if(soc != null) {
                                                soc.SocOutcomeLatitide = Latitude;
                                                soc.SocOutcomeLongitude = Longitude;
                                                using HttpClient clientPut = new HttpClient(GetHttpClientHandler());
                                                var httpContent = new StringContent(JsonConvert.SerializeObject(soc),Encoding.UTF8,"application/json");
                                                HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates",httpContent);
                                                if(!responsePut.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                                                    Crashes.TrackError(new Exception("Ошибка при записи координат в базу"),
                                                    new Dictionary<string,string> {
                                                {"Servicemans",Servicemans.First().NewPhone },
                                                {"Serviceorder",so.NewServiceorderId.ToString() },
                                                {"ServerResponse",responsePut.Content.ReadAsStringAsync().Result },
                                                //{"ErrorMessage",ex.Message },
                                                {"StatusCode",responsePut.StatusCode.ToString() },
                                                {"Response",responsePut.ToString() }
                                                    });
                                                }
                                                try {
                                                    await App.Current.MainPage.Navigation.PopPopupAsync(true);
                                                }
                                                catch(Exception ex) {
                                                    Crashes.TrackError(new Exception("Ошибка при закрытии текущей страницы CloseOrderPopupPage при установлении статуса заявки выполнено"),
                                                    new Dictionary<string,string> {
                                                {"Servicemans",Servicemans.First().NewPhone },
                                                {"Serviceorder",so.NewServiceorderId.ToString() },
                                                {"ErrorMessage",ex.Message }
                                                    });
                                                }
                                                ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Servicemans,Mounters);
                                                App.Current.MainPage = new ServiceOrdersPage(vm);
                                            }
                                        }
                                        else
                                            Crashes.TrackError(new Exception("При попытке получения записи с координатами по текущей заявке, сервер вернул ошибку"),
                                            new Dictionary<string,string> {
                                        {"Servicemans",Servicemans.First().NewPhone },
                                        {"ServerResponse",responseGet.Content.ReadAsStringAsync().Result },
                                        //{"ErrorMessage",ex.Message },
                                        {"StatusCode",responseGet.StatusCode.ToString() },
                                        {"Response",responseGet.ToString() }
                                            });
                                    }
                                }
                            }
                            else if(sofa != null) {
                                NewTest2ExtensionBase soeb = null;
                                response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceOrderForFireAlarmExtensionBase/id?id=" + sofa.NewTest2Id);
                                if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                                    var resp = response.Content.ReadAsStringAsync().Result;
                                    try {
                                        soeb = JsonConvert.DeserializeObject<NewTest2ExtensionBase>(resp);
                                    }
                                    catch(Exception ex) {
                                        soeb = null;
                                        Crashes.TrackError(new Exception("Ошибка десериализации результата запроса для получения информации по заявке технику. Ветка(выполнено)"),
                                        new Dictionary<string,string> {
                                    {"Servicemans",Servicemans.First().NewPhone },
                                    {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                    {"ErrorMessage",ex.Message },
                                    {"StatusCode",response.StatusCode.ToString() },
                                    {"Response",response.ToString() },
                                    {"Query","NewServiceOrderForFireAlarmExtensionBase/id?id=" + sofa.NewTest2Id.ToString() }
                                        });
                                    }
                                    if(soeb != null) {
                                        soeb.NewOutgone = DateTime.Now.AddHours(-5);
                                        soeb.NewTechniqueEnd = Servicemans.FirstOrDefault().NewServicemanId;
                                        soeb.NewResult = SelectedResult.Value;
                                        soeb.NewTechconclusion = ConclusionByOrder;
                                        //soeb.NewMustRead = NecesseryRead;
                                        //soeb.NewResultId = SelectedReason.Value;
                                        //soeb.NewTransferReason = ReasonComment;
                                        //if(SelectedResult.Value == 2) {
                                        //    soeb.NewMoved = new DateTime(TransferDate.Year,TransferDate.Month,TransferDate.Day,TransferTime.Hours,TransferTime.Minutes,TransferTime.Seconds).AddHours(-5);
                                        //}
                                        using(HttpClient clientPut = new HttpClient(GetHttpClientHandler())) {
                                            var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
                                            HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases",httpContent);
                                            if(responsePut.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                                                Toast.MakeText(Android.App.Application.Context,"Заявка технику сохранена",ToastLength.Long).Show();
                                            }
                                            else {
                                                Crashes.TrackError(new Exception("Ошибка при сохранении заявки технику"),
                                                new Dictionary<string,string> {
                                            {"Servicemans",Servicemans.First().NewPhone },
                                            {"ServiceOrder",sofa.NewTest2Id.ToString() },
                                            {"ServerResponse",responsePut.Content.ReadAsStringAsync().Result },
                                            {"StatusCode",responsePut.StatusCode.ToString() },
                                            {"Response",responsePut.ToString() }
                                                });
                                            }
                                        }
                                        //запишем координаты
                                        Analytics.TrackEvent("Попытка получения координат (Выполнено)");
                                        Location location = await Geolocation.GetLastKnownLocationAsync();
                                        if(location != null) {
                                            Latitude = location.Latitude.ToString();
                                            Longitude = location.Longitude.ToString();
                                        }
                                        using HttpClient clientGet = new HttpClient(GetHttpClientHandler());
                                        HttpResponseMessage responseGet = await clientGet.GetAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates/id?so_id=" + sofa.NewTest2Id);
                                        ServiceOrderCoordinates soc = null;
                                        if(responseGet.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                                            var respGet = responseGet.Content.ReadAsStringAsync().Result;
                                            try {
                                                soc = JsonConvert.DeserializeObject<ServiceOrderCoordinates>(respGet);
                                            }
                                            catch(Exception ex) {
                                                Crashes.TrackError(new Exception("Ошибка десериализации результата запросакоординат по заявке "),
                                                new Dictionary<string,string> {
                                            {"Servicemans",Servicemans.First().NewPhone },
                                            {"Serviceorder",sofa.NewTest2Id.ToString() },
                                            {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                            {"ErrorMessage",ex.Message },
                                            {"StatusCode",response.StatusCode.ToString() },
                                            {"Response",response.ToString() }
                                                });
                                                soc = null;
                                            }
                                            if(soc != null) {
                                                soc.SocOutcomeLatitide = Latitude;
                                                soc.SocOutcomeLongitude = Longitude;
                                                using HttpClient clientPut = new HttpClient(GetHttpClientHandler());
                                                var httpContent = new StringContent(JsonConvert.SerializeObject(soc),Encoding.UTF8,"application/json");
                                                HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates",httpContent);
                                                if(!responsePut.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                                                    Crashes.TrackError(new Exception("Ошибка при записи координат в базу"),
                                                    new Dictionary<string,string> {
                                                {"Servicemans",Servicemans.First().NewPhone },
                                                {"Serviceorder",sofa.NewTest2Id.ToString() },
                                                {"ServerResponse",responsePut.Content.ReadAsStringAsync().Result },
                                                //{"ErrorMessage",ex.Message },
                                                {"StatusCode",responsePut.StatusCode.ToString() },
                                                {"Response",responsePut.ToString() }
                                                    });
                                                }
                                                try {
                                                    await App.Current.MainPage.Navigation.PopPopupAsync(true);
                                                }
                                                catch(Exception ex) {
                                                    Crashes.TrackError(new Exception("Ошибка при закрытии текущей страницы CloseOrderPopupPage при установлении статуса заявки выполнено"),
                                                    new Dictionary<string,string> {
                                                {"Servicemans",Servicemans.First().NewPhone },
                                                {"Serviceorder",sofa.NewTest2Id.ToString() },
                                                {"ErrorMessage",ex.Message }
                                                    });
                                                }
                                                ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Servicemans,Mounters);
                                                App.Current.MainPage = new ServiceOrdersPage(vm);
                                            }
                                        }
                                        else
                                            Crashes.TrackError(new Exception("При попытке получения записи с координатами по текущей заявке, сервер вернул ошибку"),
                                            new Dictionary<string,string> {
                                        {"Servicemans",Servicemans.First().NewPhone },
                                        {"ServerResponse",responseGet.Content.ReadAsStringAsync().Result },
                                        //{"ErrorMessage",ex.Message },
                                        {"StatusCode",responseGet.StatusCode.ToString() },
                                        {"Response",responseGet.ToString() }
                                            });
                                    }
                                }
                            }
                        }

                    //using HttpClient client = new HttpClient(GetHttpClientHandler());
                    //HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/id?id=" + so.NewServiceorderId);
                    //NewServiceorderExtensionBase soeb = null;
                    //if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                    //    var resp = response.Content.ReadAsStringAsync().Result;
                    //    try {
                    //        soeb = JsonConvert.DeserializeObject<NewServiceorderExtensionBase>(resp);
                    //    }
                    //    catch(Exception ex) {
                    //        soeb = null;
                    //        Crashes.TrackError(new Exception("Ошибка десериализации результата запроса для получения информации по заявке технику. Ветка(выполнено)"),
                    //        new Dictionary<string,string> {
                    //                {"Servicemans",Servicemans.First().NewPhone },
                    //                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                    //                {"ErrorMessage",ex.Message },
                    //                {"StatusCode",response.StatusCode.ToString() },
                    //                {"Response",response.ToString() },
                    //                {"Query","NewServiceorderExtensionBases/id?id=" + so.NewServiceorderId.ToString() }
                    //        });
                    //    }
                    //    if(soeb != null) {
                    //        soeb.NewOutgone = DateTime.Now.AddHours(-5);
                    //        soeb.NewNewServiceman = Servicemans.FirstOrDefault().NewServicemanId;
                    //        soeb.NewResult = SelectedResult.Value;
                    //        soeb.NewTechConclusion = ConclusionByOrder;
                    //        soeb.NewMustRead = NecesseryRead;
                    //        //soeb.NewResultId = SelectedReason.Value;
                    //        //soeb.NewTransferReason = ReasonComment;
                    //        //if(SelectedResult.Value == 2) {
                    //        //    soeb.NewMoved = new DateTime(TransferDate.Year,TransferDate.Month,TransferDate.Day,TransferTime.Hours,TransferTime.Minutes,TransferTime.Seconds).AddHours(-5);
                    //        //}
                    //        using(HttpClient clientPut = new HttpClient(GetHttpClientHandler())) {
                    //            var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
                    //            HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases",httpContent);
                    //            if(responsePut.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                    //                Toast.MakeText(Android.App.Application.Context,"Заявка технику сохранена",ToastLength.Long).Show();
                    //            }
                    //            else {
                    //                Crashes.TrackError(new Exception("Ошибка при сохранении заявки технику"),
                    //                new Dictionary<string,string> {
                    //                        {"Servicemans",Servicemans.First().NewPhone },
                    //                        {"ServiceOrder",so.NewServiceorderId.ToString() },
                    //                        {"ServerResponse",responsePut.Content.ReadAsStringAsync().Result },
                    //                        {"StatusCode",responsePut.StatusCode.ToString() },
                    //                        {"Response",responsePut.ToString() }
                    //                });
                    //            }
                    //        }
                    //        //запишем координаты
                    //        Analytics.TrackEvent("Попытка получения координат (Выполнено)");
                    //        Location location = await Geolocation.GetLastKnownLocationAsync();
                    //        if(location != null) {
                    //            Latitude = location.Latitude.ToString();
                    //            Longitude = location.Longitude.ToString();
                    //        }
                    //        using HttpClient clientGet = new HttpClient(GetHttpClientHandler());
                    //        HttpResponseMessage responseGet = await clientGet.GetAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates/id?so_id=" + so.NewServiceorderId);
                    //        ServiceOrderCoordinates soc = null;
                    //        if(responseGet.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                    //            var respGet = responseGet.Content.ReadAsStringAsync().Result;
                    //            try {
                    //                soc = JsonConvert.DeserializeObject<ServiceOrderCoordinates>(respGet);
                    //            }
                    //            catch(Exception ex) {
                    //                Crashes.TrackError(new Exception("Ошибка десериализации результата запросакоординат по заявке "),
                    //                new Dictionary<string,string> {
                    //                        {"Servicemans",Servicemans.First().NewPhone },
                    //                        {"Serviceorder",so.NewServiceorderId.ToString() },
                    //                        {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                    //                        {"ErrorMessage",ex.Message },
                    //                        {"StatusCode",response.StatusCode.ToString() },
                    //                        {"Response",response.ToString() }
                    //                });
                    //                soc = null;
                    //            }
                    //            if(soc != null) {
                    //                soc.SocOutcomeLatitide = Latitude;
                    //                soc.SocOutcomeLongitude = Longitude;
                    //                using HttpClient clientPut = new HttpClient(GetHttpClientHandler());
                    //                var httpContent = new StringContent(JsonConvert.SerializeObject(soc),Encoding.UTF8,"application/json");
                    //                HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/ServiceOrderCoordinates",httpContent);
                    //                if(!responsePut.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)) {
                    //                    Crashes.TrackError(new Exception("Ошибка при записи координат в базу"),
                    //                    new Dictionary<string,string> {
                    //                            {"Servicemans",Servicemans.First().NewPhone },
                    //                            {"Serviceorder",so.NewServiceorderId.ToString() },
                    //                            {"ServerResponse",responsePut.Content.ReadAsStringAsync().Result },
                    //                            //{"ErrorMessage",ex.Message },
                    //                            {"StatusCode",responsePut.StatusCode.ToString() },
                    //                            {"Response",responsePut.ToString() }
                    //                    });
                    //                }
                    //                try {
                    //                    await App.Current.MainPage.Navigation.PopPopupAsync(true);
                    //                }
                    //                catch(Exception ex) {
                    //                    Crashes.TrackError(new Exception("Ошибка при закрытии текущей страницы CloseOrderPopupPage при установлении статуса заявки выполнено"),
                    //                    new Dictionary<string,string> {
                    //                            {"Servicemans",Servicemans.First().NewPhone },
                    //                            {"Serviceorder",so.NewServiceorderId.ToString() },
                    //                            {"ErrorMessage",ex.Message }
                    //                    });
                    //                }
                    //                ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Servicemans,Mounters);
                    //                App.Current.MainPage = new ServiceOrdersPage(vm);
                    //            }
                    //        }
                    //        else
                    //            Crashes.TrackError(new Exception("При попытке получения записи с координатами по текущей заявке, сервер вернул ошибку"),
                    //            new Dictionary<string,string> {
                    //                    {"Servicemans",Servicemans.First().NewPhone },
                    //                    {"ServerResponse",responseGet.Content.ReadAsStringAsync().Result },
                    //                    //{"ErrorMessage",ex.Message },
                    //                    {"StatusCode",responseGet.StatusCode.ToString() },
                    //                    {"Response",responseGet.ToString() }
                    //            });
                    //    }
                    //}
                }
                    else
                    await Application.Current.MainPage.DisplayAlert("Ошибка","Заключение по заявке не может быть пустым!","OK");
            }
            });
        }
}
}
