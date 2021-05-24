using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
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
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace MounterApp.ViewModel {
    /// <summary>
    /// ViewModel закрытия заявки технику + заявки на пс
    /// Объекты so(NewServiceorderExtensionBase) и sofa(NewTest2ExtensionBase) заполняются в момент вызова конструктора заявкой технику или заявкой на пс, соответсвенно
    /// </summary>
    public class CloseOrderPopupPageViewModel : BaseViewModel {
        public void IsLoading(bool state) {
            IndicatorVisible = state;
            OpacityForm = state ? 0.1 : 1;
        }
        //readonly ClientHttp http = new ClientHttp();
        /// <summary>
        /// Конструктор для закрытия заявки технику
        /// </summary>
        /// <param name="soeb">Объект Заявка технику</param>
        /// <param name="_servicemans">Техники(список)</param>
        /// <param name="_mounters">Монтажники(список)</param>
        public CloseOrderPopupPageViewModel(NewServiceorderExtensionBase soeb, List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            so = soeb;
            Servicemans = _servicemans;
            Mounters = _mounters;
            //GetResults.ExecuteAsync(null);
            NecesseryRead = false;
            SaveImage = IconName("save");
            if (Application.Current.Properties.ContainsKey("ConclusionByOrder"))
                ConclusionByOrder = Application.Current.Properties["ConclusionByOrder"] as string;
            IsLoading(false);
        }
        /// <summary>
        /// Конструктор для закрытия заявки на ПС
        /// </summary>
        /// <param name="soeb">Объект заявка на ПС</param>
        /// <param name="_servicemans">Техники(список)</param>
        /// <param name="_mounters">Монтажники(список)</param>
        public CloseOrderPopupPageViewModel(NewTest2ExtensionBase soeb, List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            sofa = soeb;
            Servicemans = _servicemans;
            Mounters = _mounters;
            //GetResults.ExecuteAsync(null);
            NecesseryRead = false;
            SaveImage = IconName("save");
            if (Application.Current.Properties.ContainsKey("ConclusionByOrder"))
                ConclusionByOrder = Application.Current.Properties["ConclusionByOrder"] as string;
            IsLoading(false);
        }
        /// <summary>
        /// Устанавливает параметр прозрачности формы
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
        private AsyncCommand _SetValueNecesseryReadCommand;
        public AsyncCommand SetValueNecesseryReadCommand {
            get => _SetValueNecesseryReadCommand ??= new AsyncCommand(async () => {
                NecesseryRead = !NecesseryRead;
            });
        }

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
                Application.Current.Properties["ConclusionByOrder"] = ConclusionByOrder;
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
                if (SelectedResult.Value != 1) {
                    ReasonVisibility = true;
                    //GetReasons.ExecuteAsync(null);
                }
                else
                    ReasonVisibility = false;
                if (SelectedResult.Value == 2) {
                    TransferLayoutVisibility = true;
                }
                if (SelectedResult.Value == 3) {
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
        /// Объект для хранения даты переноса 
        /// </summary>
        private DateTime _TransferDate;
        public DateTime TransferDate {
            get => _TransferDate;
            set {
                if (value == DateTime.Parse("01.01.2010 00:00:00"))
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
        /// Проверяем и возвращаем статус разрешений на местоположение.
        /// </summary>
        public async Task<PermissionStatus> CheckPermission() {
            PermissionStatus status = PermissionStatus.Unknown;
            try {
                status = await CheckAndRequestPermissionAsync(new LocationWhenInUse());
                return status;
            }
            catch (Exception ex) {
                Crashes.TrackError(new Exception("Заявка технику.Закрытие. Ошибка получения координат.")
                    , new Dictionary<string, string> {
                                            {"ErrorMessage",ex.Message },
                                            {"phone",Servicemans.First().NewPhone },
                                            {"name",Servicemans.First().NewName },
                                            {"PermissionStatus",status.ToString()}
                });
                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(
                    new MessagePopupPageViewModel("Ошибка при попытке сохранения. Не получены необходимые разрешения", Color.Red, LayoutOptions.EndAndExpand), 4000));
                IsLoading(false);
                return PermissionStatus.Denied;
            }
        }
        /// <summary>
        /// Получаем и присваиваем в свойства значения для координат
        /// </summary>
        private async Task<bool> GetLocation() {
            //Location location = await Geolocation.GetLastKnownLocationAsync();
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;
            Plugin.Geolocator.Abstractions.Position position;
            if (CrossGeolocator.Current.IsGeolocationEnabled) {
                Location location = await Geolocation.GetLocationAsync();
                if (location == null) {
                    position = await locator.GetPositionAsync();
                    if (position == null) {
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Отметка \"Ушёл\" не может быть установлена", Color.Red, LayoutOptions.EndAndExpand), 4000));
                        return false;
                    }
                    Latitude = position.Latitude.ToString();
                    Longitude = position.Longitude.ToString();
                    return true;
                }
                else {
                    Latitude = location.Latitude.ToString();
                    Longitude = location.Longitude.ToString();
                    return true;
                }
            }
            else 
                return false;
        }
        /// <summary>
        /// Команда закрытия заявки технику
        /// </summary>
        private RelayCommand _CloseServiceOrderCommand;
        public RelayCommand CloseServiceOrderCommand {
            get => _CloseServiceOrderCommand ??= new RelayCommand(async obj => {
                try {
                    IsLoading(true);
                    ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Servicemans, Mounters,false);
                    //Analytics.TrackEvent("Начало закрытия заявки",
                    //    new Dictionary<string, string> {
                    //{ "ServiceOrder",so != null ? so.NewServiceorderId.ToString() : sofa.NewTest2Id.ToString() },
                    //{ "ServicemanPhone",Servicemans.FirstOrDefault().NewPhone }
                    //    });
                    //проверяем значение необходимых полей NecesseryRead - Обязательно для прочтения оператором ConclusionByOrder - заключение по заявке
                    //if (NecesseryRead && string.IsNullOrEmpty(ConclusionByOrder)) {
                    if (string.IsNullOrEmpty(ConclusionByOrder)) {
                        //Analytics.TrackEvent("Не указано заключение по заявке",
                        //    new Dictionary<string, string> {
                        //        { "ServiceOrder",so != null ? so.NewServiceorderId.ToString() : sofa.NewTest2Id.ToString() },
                        //        { "ServicemanPhone",Servicemans.FirstOrDefault().NewPhone }
                        //    });
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Не указано заключение по заявке", Color.Red, LayoutOptions.EndAndExpand), 4000));
                        IsLoading(false);
                        return;
                    }
                    if (!string.IsNullOrEmpty(ConclusionByOrder)) {
                        if (!Application.Current.Properties.ContainsKey("ConclusionByOrder")) {
                            Application.Current.Properties["ConclusionByOrder"] = ConclusionByOrder;
                        }
                        //Заявка технику
                        if (so != null) {
                            NewServiceorderExtensionBase soeb = await ClientHttp.Get<NewServiceorderExtensionBase>("/api/NewServiceorderExtensionBases/id?id=" + so.NewServiceorderId);
                            if (soeb != null) {
                                PermissionStatus permissionStatus = await CheckPermission();
                                if (permissionStatus.Equals(PermissionStatus.Granted)) {
                                    soeb.NewOutgone = DateTime.Now.AddHours(-5);
                                    soeb.NewTechConclusion = ConclusionByOrder;
                                    soeb.NewMustRead = NecesseryRead;
                                    soeb.NewNewServiceman = Servicemans.First().NewServicemanId;

                                    ServiceOrderCoordinates soc = await ClientHttp.Get<ServiceOrderCoordinates>("/api/ServiceOrderCoordinates/id?so_id=" + so.NewServiceorderId);
                                    if (soc != null) {
                                        await GetLocation();
                                        if (string.IsNullOrEmpty(Latitude) || string.IsNullOrEmpty(Longitude)) {
                                            IsLoading(false);
                                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Отметка \"Ушел\" не может быть установлена", Color.Red, LayoutOptions.EndAndExpand), 4000));
                                            return;
                                        }
                                        soc.SocOutcomeLatitide = Latitude;
                                        soc.SocOutcomeLongitude = Longitude;
                                        await ClientHttp.Put("/api/ServiceOrderCoordinates", new StringContent(JsonConvert.SerializeObject(soc), Encoding.UTF8, "application/json"));
                                    }

                                    HttpStatusCode code = await ClientHttp.Put("/api/NewServiceorderExtensionBases", new StringContent(JsonConvert.SerializeObject(soeb), Encoding.UTF8, "application/json"));
                                    if (code.Equals(HttpStatusCode.Accepted)) {
                                        if (App.Current.MainPage.Navigation.NavigationStack.Any())
                                            await App.Current.MainPage.Navigation.PopPopupAsync();
                                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Заключение и время ухода сохранены", Color.Green, LayoutOptions.EndAndExpand), 4000));
                                        App.Current.MainPage = new ServiceOrdersPage(new ServiceOrdersPageViewModel(Servicemans, Mounters,false));
                                    }
                                    else
                                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("При сохранении информации о заявке технику, произошла ошибка, не был получен корректный ответ от сервера. Попробуйте позже, в случае повторной ошибки, сообщите в ИТ-отдел", Color.Red, LayoutOptions.EndAndExpand), 7000));


                                    Application.Current.Properties["ConclusionByOrder"] = null;
                                    await Application.Current.SavePropertiesAsync();
                                    App.Current.MainPage = new ServiceOrdersPage(vm);
                                    IsLoading(false);
                                }
                                else
                                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("При сохранении информации о заявке технику, произошла ошибка. Не предоставлены разрешения", Color.Red, LayoutOptions.EndAndExpand), 7000));
                            }
                            else
                                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("При сохранении информации о заявке технику, произошла ошибка", Color.Red, LayoutOptions.EndAndExpand), 7000));
                        }
                        //заявка на пс
                        if (sofa != null) {
                            NewTest2ExtensionBase soeb = await ClientHttp.Get<NewTest2ExtensionBase>("/api/NewServiceOrderForFireAlarmExtensionBase/id?id=" + sofa.NewTest2Id);
                            if (soeb != null) {
                                PermissionStatus permissionStatus = await CheckPermission();
                                if (permissionStatus.Equals(PermissionStatus.Granted)) {
                                    soeb.NewOutgone = DateTime.Now.AddHours(-5);
                                    soeb.NewTechconclusion = ConclusionByOrder;
                                    soeb.NewTechniqueEnd = Servicemans.First().NewServicemanId;

                                    ServiceOrderCoordinates soc = await ClientHttp.Get<ServiceOrderCoordinates>("/api/ServiceOrderCoordinates/id?so_id=" + sofa.NewTest2Id);
                                    if (soc != null) {
                                        await GetLocation();
                                        if (string.IsNullOrEmpty(Latitude) || string.IsNullOrEmpty(Longitude)) {
                                            IsLoading(false);
                                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Отметка \"Ушел\" не может быть установлена", Color.Red, LayoutOptions.EndAndExpand), 4000));
                                            return;
                                        }
                                        soc.SocOutcomeLatitide = Latitude;
                                        soc.SocOutcomeLongitude = Longitude;
                                        await ClientHttp.Put("/api/ServiceOrderCoordinates", new StringContent(JsonConvert.SerializeObject(soc), Encoding.UTF8, "application/json"));
                                    }

                                    HttpStatusCode code = await ClientHttp.Put("/api/NewServiceOrderForFireAlarmExtensionBase", new StringContent(JsonConvert.SerializeObject(soeb), Encoding.UTF8, "application/json"));
                                    if (code.Equals(HttpStatusCode.Accepted)) {
                                        if (App.Current.MainPage.Navigation.NavigationStack.Any())
                                            await App.Current.MainPage.Navigation.PopPopupAsync();
                                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Заключение и время ухода сохранены", Color.Green, LayoutOptions.EndAndExpand), 4000));
                                        App.Current.MainPage = new ServiceOrdersPage(new ServiceOrdersPageViewModel(Servicemans, Mounters,false));
                                    }
                                    else
                                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("При сохранении информации о заявке технику, произошла ошибка, не был получен корректный ответ от сервера. Попробуйте позже, в случае повторной ошибки, сообщите в ИТ-отдел", Color.Red, LayoutOptions.EndAndExpand), 7000));

                                    Application.Current.Properties["ConclusionByOrder"] = null;
                                    await Application.Current.SavePropertiesAsync();
                                    App.Current.MainPage = new ServiceOrdersPage(vm);
                                    IsLoading(false);
                                }
                                else
                                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("При сохранении информации о заявке технику, произошла ошибка. Не предоставлены разрешения", Color.Red, LayoutOptions.EndAndExpand), 7000));
                            }
                            else
                                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("При сохранении информации о заявке технику, произошла ошибка", Color.Red, LayoutOptions.EndAndExpand), 7000));
                        }
                    }
                    IsLoading(false);
                    await App.Current.MainPage.Navigation.PopAllPopupAsync();
                    Application.Current.Properties["ConclusionByOrder"] = null;
                    await Application.Current.SavePropertiesAsync();
                }
                catch (Exception ex) {
                    IsLoading(false);
                    ErrorReport crashReport = await Crashes.GetLastSessionCrashReportAsync();
                    Crashes.TrackError(new Exception("Проблема при закрытии заявки")
                        , new Dictionary<string, string> {
                                            {"ErrorMessage",ex.Message },
                                            {"phone",Servicemans.First().NewPhone },
                                            {"name",Servicemans.First().NewName },
                                            {"StackTrace",crashReport.StackTrace },
                                            {"AndroidDetails",crashReport.AndroidDetails.StackTrace }
                    });
                }
            });
        }
    }
}
