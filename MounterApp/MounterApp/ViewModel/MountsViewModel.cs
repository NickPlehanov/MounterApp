using Android.Widget;
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
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class MountsViewModel : BaseViewModel {
        private ObservableCollection<Mounts> _Mounts = new ObservableCollection<Mounts>();
        public ObservableCollection<Mounts> Mounts {
            get => _Mounts;
            set {
                _Mounts = value;
                OnPropertyChanged(nameof(Mounts));
            }
        }

        private ObservableCollection<Mounts> _NotSendedMounts = new ObservableCollection<Mounts>();
        public ObservableCollection<Mounts> NotSendedMounts {
            get => _NotSendedMounts;
            set {
                _NotSendedMounts = value;
                OnPropertyChanged(nameof(NotSendedMounts));
            }
        }
        private Mounts _NotSendedMount;
        public Mounts NotSendedMount {
            get => _NotSendedMount;
            set {
                _NotSendedMount = value;
                OnPropertyChanged(nameof(NotSendedMount));
            }
        }
        public MountsViewModel() {

        }
        public MountsViewModel(List<NewMounterExtensionBase> mounters,List<NewServicemanExtensionBase> servicemans) {
            Servicemans = servicemans;
            Mounters = mounters;
            HeaderNotSended = "Неотправленные (0)";
            HeaderGoogle = "Запланированные (0)";
            HeaderHistory = "Отправленные (0)";
            try {
                ClearHistoryMounts.Execute(null);
            }
            catch(Exception ex) {
                Crashes.TrackError(new Exception("Ошибка автоматической очистки монтажей старше чем неделя от текущей даты"),
                new Dictionary<string,string> {
                    {"Error",ex.Message }
                });
            }
            try {
                GetNotSendedMounts.Execute(null);
            }
            catch(Exception ex) {
                Crashes.TrackError(new Exception("Ошибка получения монтажей из локальной базы данных (неотправленные)"),
                new Dictionary<string,string> {
                    {"Error",ex.Message }
                });
            }
            try {
                GetGoogleMounts.Execute(null);
            }
            catch(Exception ex) {
                Crashes.TrackError(new Exception("Ошибка получения монтажей из google-таблицы"),
                new Dictionary<string,string> {
                    {"Error",ex.Message }
                });
            }
            try {
                GetHistoryMounts.Execute(null);
            }
            catch(Exception ex) {
                Crashes.TrackError(new Exception("Ошибка получения монтажей из локальной базы данных (история)"),
                new Dictionary<string,string> {
                    {"Error",ex.Message }
                });
            }

            Opacity = 1;
            IndicatorVisible = false;
            ArrowCircleGoogle = IconName("arrow_circle_down");
            ArrowCircleNotSended = IconName("arrow_circle_down");
            ArrowCircleHistory = IconName("arrow_circle_down");

            Analytics.TrackEvent("Страница монтажей",
            new Dictionary<string,string> {
                {"MounterPhone",Mounters.FirstOrDefault().NewPhone }
            });
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        }

        private string _HeaderHistory;
        public string HeaderHistory {
            get => _HeaderHistory;
            set {
                _HeaderHistory = value;
                OnPropertyChanged(nameof(HeaderHistory));
            }
        }
        private RelayCommand _GetNotSendedMounts;
        public RelayCommand GetNotSendedMounts {
            get => _GetNotSendedMounts ??= new RelayCommand(async obj => {
                NotSendedMounts.Clear();
                List<Mounts> _ntMounts = new List<Mounts>();
                //_ntMounts = App.Database.GetMounts(Mounters.FirstOrDefault().NewMounterId).Where(x => x.State == 0).ToList();
                _ntMounts = App.Database.GetMounts().Where(x => x.State == 0 && x.MounterID == Mounters.FirstOrDefault().NewMounterId).ToList();
                if(_ntMounts != null)
                    if(_ntMounts.Any()) {
                        foreach(var item in _ntMounts)
                            NotSendedMounts.Add(item);
                        HeaderNotSended = "Неотправленные (" + _ntMounts.Count.ToString() + ")";
                        if(NotSendedMounts.Count() > 0)
                            NotSendedMountsExpander = true;
                    }
                Analytics.TrackEvent("Получение списка монтажей из локальной базы данных (неотправленные)",new Dictionary<string,string> {
                {"CountMounts",_ntMounts.Count.ToString() }
                });
            });
        }
        private RelayCommand _ClearHistoryMounts;
        public RelayCommand ClearHistoryMounts {
            get => _ClearHistoryMounts ??= new RelayCommand(async obj => {
                DateTime _dt = DateTime.Now.AddDays(-7);
                var _ntMounts = App.Database.GetMounts().Where(x => x.State == 1 && x.MounterID == Mounters.FirstOrDefault().NewMounterId).ToList();
                if(_ntMounts != null)
                    if(_ntMounts.Any()) {
                        foreach(var item in _ntMounts) {
                            if (item.DateSended!=null)
                                if (item.DateSended.Value.Date < _dt.Date)
                                App.Database.DeleteMount(item.ID);
                        }
                        Analytics.TrackEvent("Очистка списка монтажей старше недели от текущей даты",new Dictionary<string,string> {
                        {"CountMounts",_ntMounts.Count.ToString() }
                        });
                    }
            });
        }
        private RelayCommand _GetHistoryMounts;
        public RelayCommand GetHistoryMounts {
            get => _GetHistoryMounts ??= new RelayCommand(async obj => {
                HistoryMounts.Clear();
                List<Mounts> _ntMounts = new List<Mounts>();
                //_ntMounts = App.Database.GetMounts(Mounters.FirstOrDefault().NewMounterId).Where(x => x.State == 0).ToList();
                _ntMounts = App.Database.GetMounts().Where(x => x.State == 1 && x.MounterID == Mounters.FirstOrDefault().NewMounterId).ToList();
                if(_ntMounts != null)
                    if(_ntMounts.Any()) {
                        foreach(var item in _ntMounts)
                            HistoryMounts.Add(item);
                        HeaderHistory = "Отправленные (" + _ntMounts.Count.ToString() + ")";
                    }
                Analytics.TrackEvent("Получение списка монтажей из локальной базы данных (история)",new Dictionary<string,string> {
                {"CountMounts",_ntMounts.Count.ToString() }
                });
            });
        }

        private string _HeaderGoogle;
        public string HeaderGoogle {
            get => _HeaderGoogle;
            set {
                _HeaderGoogle = value;
                OnPropertyChanged(nameof(HeaderGoogle));
            }
        }
        private bool _GoogleMountsExpander;
        public bool GoogleMountsExpander {
            get => _GoogleMountsExpander;
            set {
                _GoogleMountsExpander = value;
                if(_GoogleMountsExpander)
                    ArrowCircleGoogle = IconName("arrow_circle_up");
                else
                    ArrowCircleGoogle = IconName("arrow_circle_down");
                OnPropertyChanged(nameof(GoogleMountsExpander));
            }
        }

        private bool _HistoryMountsExpander;
        public bool HistoryMountsExpander {
            get => _HistoryMountsExpander;
            set {
                _HistoryMountsExpander = value;
                if(_HistoryMountsExpander)
                    ArrowCircleHistory = IconName("arrow_circle_up");
                else
                    ArrowCircleHistory = IconName("arrow_circle_down");
                OnPropertyChanged(nameof(HistoryMountsExpander));
            }
        }
        private bool _NotSendedMountsExpander;
        public bool NotSendedMountsExpander {
            get => _NotSendedMountsExpander;
            set {
                _NotSendedMountsExpander = value;
                if(_NotSendedMountsExpander)
                    ArrowCircleNotSended = IconName("arrow_circle_up");
                else
                    ArrowCircleNotSended = IconName("arrow_circle_down");
                OnPropertyChanged(nameof(NotSendedMountsExpander));
            }
        }

        private RelayCommand _GoogleMountsExpanderCommand;
        public RelayCommand GoogleMountsExpanderCommand {
            get => _GoogleMountsExpanderCommand ??= new RelayCommand(async obj => {
                GoogleMountsExpander = !GoogleMountsExpander;
            });
        }

        private RelayCommand _HistoryMountsExpanderCommand;
        public RelayCommand HistoryMountsExpanderCommand {
            get => _HistoryMountsExpanderCommand ??= new RelayCommand(async obj => {
                HistoryMountsExpander = !HistoryMountsExpander;
            });
        }

        private RelayCommand _NotSendedMountsExpanderCommand;
        public RelayCommand NotSendedMountsExpanderCommand {
            get => _NotSendedMountsExpanderCommand ??= new RelayCommand(async obj => {
                NotSendedMountsExpander = !NotSendedMountsExpander;
            });
        }
        private double _Opacity;
        public double Opacity {
            get => _Opacity;
            set {
                _Opacity = value;
                OnPropertyChanged(nameof(Opacity));
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

        private RelayCommand _GetGoogleMounts;
        public RelayCommand GetGoogleMounts {
            get => _GetGoogleMounts ??= new RelayCommand(async obj => {
                Opacity = 0.1;
                IndicatorVisible = true;
                using HttpClient client = new HttpClient(GetHttpClientHandler());
                try {
                    Analytics.TrackEvent("Получение списка монтажей из google таблицы",new Dictionary<string,string> {
                        {"MounterPhone",Mounters.FirstOrDefault().NewPhone },
                        {"DateTime",DateTime.Now.Date.ToString() }
                    });
                    //Phone = Application.Current.Properties["Phone"].ToString();
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common?phone=7" + Mounters.FirstOrDefault().NewPhone + "&date=" + DateTime.Now.Date + "");
                    var resp = response.Content.ReadAsStringAsync().Result;
                    List<GoogleMountModel> googleMounts = new List<GoogleMountModel>();
                    try {
                        if(response.StatusCode.ToString() == "OK")
                            googleMounts = JsonConvert.DeserializeObject<List<GoogleMountModel>>(resp).ToList();
                    }
                    catch(Exception GoogleMountParseException) {
                        Crashes.TrackError(GoogleMountParseException,new Dictionary<string,string> {
                        {"Error","Не удалось десерилизовать список монтажей из google таблицы" },
                        {"ErrorMessage",GoogleMountParseException.Message }
                    });
                    }
                    GoogleMounts.Clear();
                    foreach(GoogleMountModel item in googleMounts) {
                        GoogleMounts.Add(item);
                    }
                    if(GoogleMounts.Count() > 0)
                        GoogleMountsExpander = true;
                }
                catch(Exception GetGoogleMountsException) {
                    Crashes.TrackError(GetGoogleMountsException,new Dictionary<string,string> {
                        {"Error","Не удалось получить список монтажей из google таблицы" },
                        {"ErrorMessage",GetGoogleMountsException.Message }
                    });
                }
                HeaderGoogle = "Запланированные (" + GoogleMounts.Count() + ")";
                Opacity = 1;
                IndicatorVisible = false;
            });
        }
        private ImageSource _ArrowCircleGoogle;
        public ImageSource ArrowCircleGoogle {
            get => _ArrowCircleGoogle;
            set {
                _ArrowCircleGoogle = value;
                OnPropertyChanged(nameof(ArrowCircleGoogle));
            }
        }

        private ImageSource _ArrowCircleNotSended;
        public ImageSource ArrowCircleNotSended {
            get => _ArrowCircleNotSended;
            set {
                _ArrowCircleNotSended = value;
                OnPropertyChanged(nameof(ArrowCircleNotSended));
            }
        }

        private ImageSource _ArrowCircleHistory;
        public ImageSource ArrowCircleHistory {
            get => _ArrowCircleHistory;
            set {
                _ArrowCircleHistory = value;
                OnPropertyChanged(nameof(ArrowCircleHistory));
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

        private RelayCommand _NewMountCommand;
        public RelayCommand NewMountCommand {
            get => _NewMountCommand ??= new RelayCommand(async obj => {
                if(Mounters.Count > 0) {
                    Analytics.TrackEvent("Создание нового монтажа",
                        new Dictionary<string,string> {
                            {"MounterPhone",Mounters.FirstOrDefault().NewPhone }
                        });
                    NewMountPageViewModel vm = new NewMountPageViewModel(Mounters,Servicemans);
                    App.Current.MainPage = new NewMountpage(vm);
                }
            });
        }
        private RelayCommand _SelectMountCommand;
        public RelayCommand SelectMountCommand {
            get => _SelectMountCommand ??= new RelayCommand(async obj => {
                if(obj != null) {
                    int _id = -1;
                    int.TryParse(obj.ToString(),out _id);
                    NotSendedMount = NotSendedMounts.FirstOrDefault(x => x.ID == _id);
                    if(NotSendedMount != null) {
                        Analytics.TrackEvent("Переход к раннее заполненному монтажу",
                            new Dictionary<string,string> {
                            {"MounterPhone",Mounters.FirstOrDefault().NewPhone },
                            {"ObjectNumber",NotSendedMount.ObjectNumber }
                            });
                        NewMountPageViewModel vm = new NewMountPageViewModel(NotSendedMount,Mounters,false,Servicemans);
                        App.Current.MainPage = new NewMountpage(vm);
                    }
                }
            });
        }

        private RelayCommand _DeleteNotSendedMountCommand;
        public RelayCommand DeleteNotSendedMountCommand {
            get => _DeleteNotSendedMountCommand ??= new RelayCommand(async obj => {
                if(obj != null) {
                    int _id = -1;
                    int.TryParse(obj.ToString(),out _id);
                    NotSendedMount = NotSendedMounts.FirstOrDefault(x => x.ID == _id);
                    if(NotSendedMount != null) {
                        bool result = await Application.Current.MainPage.DisplayAlert("Удаление","Подтвердите удаление","Удалить","Отмена");
                        if(result) {
                            NotSendedMounts.Remove(NotSendedMount);
                            Toast.MakeText(Android.App.Application.Context,"Монтаж удален из локальной базы",ToastLength.Long).Show();
                        }
                    }
                }
            });
        }

        //private RelayCommand _TestCommand;
        //public RelayCommand TestCommand {
        //    get => _TestCommand ??= new RelayCommand(async obj => {
        //        if(NotSendedMount != null) { }
        //        bool f = false;
        //    });
        //}
        private string _HeaderNotSended;
        public string HeaderNotSended {
            get => _HeaderNotSended;
            set {
                _HeaderNotSended = value;
                OnPropertyChanged(nameof(HeaderNotSended));
            }
        }
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                MainMenuPageViewModel vm = new MainMenuPageViewModel(Mounters,Servicemans);
                App.Current.MainPage = new MainMenuPage(vm);
            });
        }

        private List<NewServicemanExtensionBase> _Servicemans;
        public List<NewServicemanExtensionBase> Servicemans {
            get => _Servicemans;
            set {
                _Servicemans = value;
                OnPropertyChanged(nameof(Servicemans));
            }
        }

        private ObservableCollection<GoogleMountModel> _GoogleMounts = new ObservableCollection<GoogleMountModel>();
        public ObservableCollection<GoogleMountModel> GoogleMounts {
            get => _GoogleMounts;
            set {
                _GoogleMounts = value;
                OnPropertyChanged(nameof(GoogleMounts));
            }
        }

        private GoogleMountModel _GoogleMount;
        public GoogleMountModel GoogleMount {
            get => _GoogleMount;
            set {
                _GoogleMount = value;
                NotSendedMount = new Mounts();
                NotSendedMount.MounterID = Mounters.FirstOrDefault().NewMounterId;
                NotSendedMount.GoogleComment = _GoogleMount.FullInfo;
                OnPropertyChanged(nameof(GoogleMount));
            }
        }

        private ObservableCollection<Mounts> _HistoryMounts = new ObservableCollection<Mounts>();
        public ObservableCollection<Mounts> HistoryMounts {
            get => _HistoryMounts;
            set {
                _HistoryMounts = value;
                OnPropertyChanged(nameof(HistoryMounts));
            }
        }

        private Mounts _HistoryMount;
        public Mounts HistoryMount {
            get => _HistoryMount;
            set {
                _HistoryMount = value;
                OnPropertyChanged(nameof(HistoryMount));
            }
        }
    }
}