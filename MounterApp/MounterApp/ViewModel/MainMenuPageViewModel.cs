using Microsoft.AppCenter.Analytics;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Views;
using Rg.Plugins.Popup.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class MainMenuPageViewModel : BaseViewModel {
        /// <summary>
        /// Конструктор страницы главного меню
        /// </summary>
        /// <param name="mounters">Монтажники(список)</param>
        /// <param name="servicemans">Техники(список)</param>
        public MainMenuPageViewModel(List<NewMounterExtensionBase> mounters,List<NewServicemanExtensionBase> servicemans) {
            Mounters = mounters;
            Serviceman = servicemans;
            SettingsImage = IconName("settings");
            Analytics.TrackEvent("Инициализация окна главного меню приложения");
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        }        
        /// <summary>
        /// Команда перехода к странице монтажей
        /// </summary>
        private RelayCommand _GetMountworksCommand;
        public RelayCommand GetMountworksCommand {
            get => _GetMountworksCommand ??= new RelayCommand(async obj  => {
                if(Mounters.Any()) {
                    Analytics.TrackEvent("Переход к монтажам",
                    new Dictionary<string,string> {
                        {"MountersPhone",Mounters.First().NewPhone }
                    });
                    MountsViewModel vm = new MountsViewModel(Mounters,Serviceman);
                    App.Current.MainPage = new MountsPage(vm);
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Ошибка","Не определен сотрудник, переход невозможен","OK");
            },obj=>Mounters != null && Mounters.Count > 0);
        }
        /// <summary>
        /// Команда перехода к странице с заявками технику
        /// </summary>
        private RelayCommand _GetServiceordersCommand;
        public RelayCommand GetServiceordersCommand {
            get => _GetServiceordersCommand ??= new RelayCommand(async obj => {
                if(Serviceman.Any()) {
                    Analytics.TrackEvent("Переход к заявкам технику",
                        new Dictionary<string,string> {
                        {"ServicemansPhone",Serviceman.First().NewPhone }
                        });
                    ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Serviceman,Mounters);
                    App.Current.MainPage = new ServiceOrdersPage(vm);
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Ошибка","Не определен сотрудник, переход невозможен","OK");
            },obj => Serviceman != null && Serviceman.Count > 0);
        }
        /// <summary>
        /// Список для хранения монтажника
        /// </summary>
        private List<NewMounterExtensionBase> _Mounters = new List<NewMounterExtensionBase>();
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
        }
        /// <summary>
        /// Список для хранения техника
        /// </summary>
        private List<NewServicemanExtensionBase> _Serviceman = new List<NewServicemanExtensionBase>();
        public List<NewServicemanExtensionBase> Serviceman {
            get => _Serviceman;
            set {
                _Serviceman = value;
                OnPropertyChanged(nameof(Serviceman));
            }
        }
        /// <summary>
        /// Команда выполняемая по нажатию кнопки "Назад"
        /// </summary>
        private AsyncCommand _BackPressCommand;
        public AsyncCommand BackPressCommand {
            get => _BackPressCommand ??= new AsyncCommand(async () => {
                MainPageViewModel vm = new MainPageViewModel();
                App.Current.MainPage = new MainPage(vm);
            });
        }
        /// <summary>
        /// Команда открытия настроек
        /// </summary>
        private AsyncCommand _OpenSettingsCommand;
        public AsyncCommand OpenSettingsCommand {
            get => _OpenSettingsCommand ??= new AsyncCommand(async () => {
                Analytics.TrackEvent("Переход к настройкам");
                SettingsPageViewModel vm = new SettingsPageViewModel(Mounters,Serviceman);
                App.Current.MainPage = new SettingsPage(vm);
            });
        }
        /// <summary>
        /// Картинка - Настройки
        /// </summary>
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
