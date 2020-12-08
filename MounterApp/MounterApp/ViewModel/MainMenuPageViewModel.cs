using Microsoft.AppCenter.Analytics;
using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class MainMenuPageViewModel : BaseViewModel {
        private ObservableCollection<MountWorksModel> _MountWorks;
        public ObservableCollection<MountWorksModel> MountWorks {
            get => _MountWorks;
            set {
                _MountWorks = value;
                OnPropertyChanged(nameof(MountWorks));
            }
        }
        public MainMenuPageViewModel(List<NewMounterExtensionBase> mounters,List<NewServicemanExtensionBase> servicemans) {
            Mounters = mounters;
            Serviceman = servicemans;
            SettingsImage= "settings.png";
            Analytics.TrackEvent("Инициализация окна главного меню приложения");
        }
        public MainMenuPageViewModel(List<NewMounterExtensionBase> mounters) {
            Mounters = mounters;
            SettingsImage = "settings.png";
            Analytics.TrackEvent("Инициализация окна главного меню приложения");
        }
        public MainMenuPageViewModel(List<NewServicemanExtensionBase> servicemans) {
            Serviceman = servicemans;
            SettingsImage = "settings.png";
            Analytics.TrackEvent("Инициализация окна главного меню приложения");
        }
        public MainMenuPageViewModel() {
            SettingsImage = "settings.png";
            Analytics.TrackEvent("Инициализация окна главного меню приложения");
        }

        private string _Message;
        public string Message {
            get => _Message;
            set {
                _Message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private RelayCommand _GetMountworksCommand;
        public RelayCommand GetMountworksCommand {
            get => _GetMountworksCommand ??= new RelayCommand(async obj => {
                Dictionary<string,string> parameters = new Dictionary<string,string> {
                    {"MountersPhone",Mounters.First().NewPhone }
                };
                Analytics.TrackEvent("Переход к монтажам",parameters);
                MountsViewModel vm = new MountsViewModel(Mounters);
                App.Current.MainPage = new MountsPage(vm);
            });
        }
        //private RelayCommand _ClearDatabaseCommand;
        //public RelayCommand ClearDatabaseCommand {
        //    get => _ClearDatabaseCommand ??= new RelayCommand(async obj => {
        //        Message="Очищено объектов: "+App.Database.ClearDatabase().ToString();
        //    });
        //}

        private RelayCommand _GetServiceordersCommand;
        public RelayCommand GetServiceordersCommand {
            get => _GetServiceordersCommand ??= new RelayCommand(async obj => {
                Dictionary<string,string> parameters = new Dictionary<string,string> {
                    {"ServicemansPhone",Serviceman.First().NewPhone }
                };
                Analytics.TrackEvent("Переход к заявкам технику",parameters);
                ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Serviceman);
                App.Current.MainPage = new ServiceOrdersPage(vm);
            });
        }
        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
        }
        private List<NewServicemanExtensionBase> _Serviceman;
        public List<NewServicemanExtensionBase> Serviceman {
            get => _Serviceman;
            set {
                _Serviceman = value;
                OnPropertyChanged(nameof(Serviceman));
            }
        }
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                MainPageViewModel vm = new MainPageViewModel();
                App.Current.MainPage = new MainPage(vm);
                //System.Environment.Exit(0);
            });
        }

        private RelayCommand _OpenSettingsCommand;
        public RelayCommand OpenSettingsCommand {
            get => _OpenSettingsCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Переход к настройкам");
                SettingsPageViewModel vm = new SettingsPageViewModel(Mounters,Serviceman);
                App.Current.MainPage = new SettingsPage(vm);
            });
        }

        private ImageSource _SettingsImage;
        public ImageSource SettingsImage {
            get => _SettingsImage;
            set {
                _SettingsImage = value;
                OnPropertyChanged(nameof(SettingsImage));
            }
        }
    }
}
