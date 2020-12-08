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
        public MountsViewModel(List<NewMounterExtensionBase> mounters) {
            Mounters = mounters;
            HeaderNotSended = "Неотправленные (0)";
            var _ntMounts = App.Database.GetMounts(Mounters.FirstOrDefault().NewMounterId).Where(x => x.State == 0).ToList();
            if(_ntMounts != null)
                if(_ntMounts.Any()) {
                    foreach(var item in _ntMounts)
                        NotSendedMounts.Add(item);
                    HeaderNotSended = "Неотправленные (" + _ntMounts.Count.ToString() + ")";
                }
            Analytics.TrackEvent("Получение списка монтажей из локальной базы данных",new Dictionary<string,string> {
                {"CountMounts",_ntMounts.Count.ToString() }
            });

            GetGoogleMounts.Execute(null);

            Opacity = 1;
            IndicatorVisible = false;
            ArrowCircleGoogle = "arrow_circle_down.png";
            ArrowCircleNotSended = "arrow_circle_down.png";
        }

        private bool _GoogleMountsExpander;
        public bool GoogleMountsExpander {
            get => _GoogleMountsExpander;
            set {
                _GoogleMountsExpander = value;
                if(_GoogleMountsExpander)
                    ArrowCircleGoogle = "arrow_circle_up.png";
                else
                    ArrowCircleGoogle = "arrow_circle_down.png";
                OnPropertyChanged(nameof(GoogleMountsExpander));
            }
        }

        private bool _NotSendedMountsExpander;
        public bool NotSendedMountsExpander {
            get => _NotSendedMountsExpander;
            set {
                _NotSendedMountsExpander = value;
                if(_NotSendedMountsExpander)
                    ArrowCircleNotSended = "arrow_circle_up.png";
                else
                    ArrowCircleNotSended = "arrow_circle_down.png";
                OnPropertyChanged(nameof(NotSendedMountsExpander));
            }
        }

        private RelayCommand _GoogleMountsExpanderCommand;
        public RelayCommand GoogleMountsExpanderCommand {
            get => _GoogleMountsExpanderCommand ??= new RelayCommand(async obj => {
                GoogleMountsExpander = !GoogleMountsExpander;
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
                using HttpClient client = new HttpClient();
                try {
                    Analytics.TrackEvent("Получение списка монтажей из google таблицы",new Dictionary<string,string> {
                        {"MounterPhone",Mounters.FirstOrDefault().NewPhone },
                        {"DateTime",DateTime.Now.Date.ToString() }
                    });
                    //Phone = Application.Current.Properties["Phone"].ToString();
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common?phone=8" + Mounters.FirstOrDefault().NewPhone + "&date=" + DateTime.Now.Date + "");
                    var resp = response.Content.ReadAsStringAsync().Result;
                    List<GoogleMountModel> googleMounts = new List<GoogleMountModel>();
                    try {
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
                }
                catch(Exception GetGoogleMountsException) {
                    Crashes.TrackError(GetGoogleMountsException,new Dictionary<string,string> {
                        {"Error","Не удалось получить список монтажей из google таблицы" },
                        {"ErrorMessage",GetGoogleMountsException.Message }
                    });
                }
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
                    NewMountPageViewModel vm = new NewMountPageViewModel(Mounters);
                    App.Current.MainPage = new NewMountpage(vm);
                }
            });
        }
        private RelayCommand _SelectMountCommand;
        public RelayCommand SelectMountCommand {
            get => _SelectMountCommand ??= new RelayCommand(async obj => {
                if(NotSendedMount != null) {
                    Analytics.TrackEvent("Переход к раннее заполненному монтажу",
                        new Dictionary<string,string> {
                            {"MounterPhone",Mounters.FirstOrDefault().NewPhone },
                            {"ObjectNumber",NotSendedMount.ObjectNumber }
                        });
                    NewMountPageViewModel vm = new NewMountPageViewModel(NotSendedMount,Mounters);
                    App.Current.MainPage = new NewMountpage(vm);
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
                MainMenuPageViewModel vm = new MainMenuPageViewModel(Mounters);
                App.Current.MainPage = new MainMenuPage(vm);
            });
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
    }
}