using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                ClearHistoryMounts.Execute(null);
                GetNotSendedMounts.Execute(null);
                GetGoogleMounts.Execute(null);
                GetHistoryMounts.Execute(null);

            Opacity = 1;
            IndicatorVisible = false;
            ArrowCircleGoogle = IconName("arrow_circle_down");
            ArrowCircleNotSended = IconName("arrow_circle_down");
            ArrowCircleHistory = IconName("arrow_circle_down");
            DeleteImage = IconName("clear");

            //Analytics.TrackEvent("Страница монтажей",
            //new Dictionary<string,string> {
            //    {"MounterPhone",Mounters.FirstOrDefault().NewPhone }
            //});
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
                _ntMounts = App.Database.GetMounts().Where(x => x.State == 0 && x.MounterID == Mounters.FirstOrDefault().NewMounterId).ToList();
                if(_ntMounts != null)
                        foreach(var item in _ntMounts)
                            NotSendedMounts.Add(item);
                        HeaderNotSended = "Неотправленные (" + _ntMounts.Count.ToString() + ")";
                        NotSendedMountsExpander = NotSendedMounts.Count > 0;
                //Analytics.TrackEvent("Получение списка монтажей из локальной базы данных (неотправленные)",new Dictionary<string,string> {
                //{"CountMounts",_ntMounts.Count.ToString() }
                //});
            });
        }
        private RelayCommand _HelpCommand;
        public RelayCommand HelpCommand {
            get => _HelpCommand ??= new RelayCommand(async obj => {
                string msg = " - На вкладке неотправленные, протаскивание(свайп) монтажа влево - позволяет открыть его, так же как и простым нажатием, свайп вправо - вызывает команду удаления";
                HelpPopupViewModel vm = new HelpPopupViewModel(msg);
                await App.Current.MainPage.Navigation.PushPopupAsync(new HelpPopupPage(vm));
            });
        }

        private RelayCommand _ClearHistoryMounts;
        public RelayCommand ClearHistoryMounts {
            get => _ClearHistoryMounts ??= new RelayCommand(async obj => {
                DateTime _dt = DateTime.Now.AddDays(-7);
                var _ntMounts = App.Database.GetMounts().Where(x => x.State == 1 && x.MounterID == Mounters.FirstOrDefault().NewMounterId).ToList();
                if(_ntMounts != null)
                        foreach(var item in _ntMounts) {
                            if (item.DateSended!=null)
                                if (item.DateSended.Value.Date < _dt.Date)
                                App.Database.DeleteMount(item.ID);
                        //Analytics.TrackEvent("Очистка списка монтажей старше недели от текущей даты",new Dictionary<string,string> {
                        //{"CountMounts",_ntMounts.Count.ToString() }
                        //});
                    }
            });
        }
        private RelayCommand _GetHistoryMounts;
        public RelayCommand GetHistoryMounts {
            get => _GetHistoryMounts ??= new RelayCommand(async obj => {
                HistoryMounts.Clear();
                List<Mounts> _ntMounts = new List<Mounts>();
                _ntMounts = App.Database.GetMounts().Where(x => x.State == 1 && x.MounterID == Mounters.FirstOrDefault().NewMounterId).ToList();
                if(_ntMounts != null)
                        foreach(var item in _ntMounts)
                            HistoryMounts.Add(item);
                        HeaderHistory = "Отправленные (" + _ntMounts.Count.ToString() + ")";
                //Analytics.TrackEvent("Получение списка монтажей из локальной базы данных (история)",new Dictionary<string,string> {
                //{"CountMounts",_ntMounts.Count.ToString() }
                //});
            });
        }

        private ImageSource _DeleteImage;
        public ImageSource DeleteImage {
            get => _DeleteImage;
            set {
                _DeleteImage = value;
                OnPropertyChanged(nameof(DeleteImage));
            }
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
                var mounter = Mounters.FirstOrDefault();
                if (mounter == null)
                    return;
                GoogleMounts = await ClientHttp.Get<ObservableCollection<GoogleMountModel>>("/api/Common?phone=7" + mounter.NewPhone + "&date=" + DateTime.Now.Date + "");
                if (GoogleMounts == null)
                    return;
                GoogleMountsExpander = GoogleMounts != null;
                if (GoogleMounts != null)
                    HeaderGoogle = "Запланированные (" + GoogleMounts.Count + ")";
                else
                    HeaderGoogle = "Запланированные (0)";
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
                    //Analytics.TrackEvent("Создание нового монтажа",
                    //    new Dictionary<string,string> {
                    //        {"MounterPhone",Mounters.FirstOrDefault().NewPhone }
                    //    });
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
                    if(int.TryParse(obj.ToString(),out _id)) {
                        NotSendedMount = NotSendedMounts.FirstOrDefault(x => x.ID == _id);
                        if(NotSendedMount != null) {
                            //Analytics.TrackEvent("Переход к раннее заполненному монтажу",
                            //    new Dictionary<string,string> {
                            //{"MounterPhone",Mounters.FirstOrDefault().NewPhone },
                            //{"ObjectNumber",NotSendedMount.ObjectNumber }
                            //    });
                            NewMountPageViewModel vm = new NewMountPageViewModel(NotSendedMount,Mounters,false,Servicemans);
                            App.Current.MainPage = new NewMountpage(vm);
                        }
                    }
                    Guid guid = Guid.Empty;
                    if (Guid.TryParse(obj.ToString(),out guid)) {
                        GoogleMount = GoogleMounts.First(x => x.id == guid);
                        if(GoogleMount != null) {
                            //Analytics.TrackEvent("Переход к запланированному монтажу",
                            //    new Dictionary<string,string> {
                            //        {"MounterPhone",Mounters.FirstOrDefault().NewPhone }
                            //    });
                            NotSendedMount = GoogleMount;
                            NewMountPageViewModel vm = new NewMountPageViewModel(NotSendedMount,Mounters,false,Servicemans);
                            App.Current.MainPage = new NewMountpage(vm);
                        }
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
                            App.Database.DeleteMount(_id);
                            List<Mounts> _ntMounts = new List<Mounts>();
                            _ntMounts = App.Database.GetMounts().Where(x => x.State == 0 && x.MounterID == Mounters.FirstOrDefault().NewMounterId).ToList();
                            HeaderNotSended = "Неотправленные (" + _ntMounts.Count.ToString() + ")";
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Монтаж удален из локальной базы",Color.Green,LayoutOptions.EndAndExpand),4000));
                        }
                    }
                }
            });
        }

        private RelayCommand _GetEvents;
        public RelayCommand GetEvents {
            get => _GetEvents ??= new RelayCommand(async obj => {
                int _id = -1;
                int.TryParse(obj.ToString(),out _id);
                EventsPopupViewModel vm = new EventsPopupViewModel(HistoryMounts.First(x => x.ID == _id).ObjectNumber,HistoryMounts.First(x => x.ID == _id).DateSended);
                await App.Current.MainPage.Navigation.PushPopupAsync(new EventsPopupPage(vm));
            });
        }
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