using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class MainMenuPageViewModel : BaseViewModel {
        //ClientHttp http = new ClientHttp();
        /// <summary>
        /// Конструктор страницы главного меню
        /// </summary>
        /// <param name="mounters">Монтажники(список)</param>
        /// <param name="servicemans">Техники(список)</param>
        public MainMenuPageViewModel(List<NewMounterExtensionBase> mounters, List<NewServicemanExtensionBase> servicemans) {
            Mounters = mounters;
            Serviceman = servicemans;
            SettingsImage = IconName("settings");
            Analytics.TrackEvent("Инициализация окна главного меню приложения");
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
            CheckVersionApp.Execute(null);
            //GetRatingServicemanCommand.Execute(null);
        }
        /// <summary>
        /// текстовое поле, форматируется вручную для отображения топ-3 техников по количеству заявок за текущий месяц
        /// </summary>
        private string _RatingServiceman;
        public string RatingServiceman {
            get => _RatingServiceman;
            set {
                _RatingServiceman = value;
                OnPropertyChanged(nameof(RatingServiceman));
            }
        }
        /// <summary>
        /// Список техников отсортированный по количеству выполненных заявок
        /// </summary>
        private ObservableCollection<RatingServiceman> _Rating = new ObservableCollection<RatingServiceman>();
        public ObservableCollection<RatingServiceman> Rating {
            get => _Rating;
            set {
                _Rating = value;
                OnPropertyChanged(nameof(Rating));
            }
        }
        /// <summary>
        /// Команда получения списка техников с количеством выполненных заявок
        /// </summary>
        //private RelayCommand _GetRatingServicemanCommand;
        //public RelayCommand GetRatingServicemanCommand {
        //    get => _GetRatingServicemanCommand ??= new RelayCommand(async obj => {
        //        if (Serviceman.Count > 0)
        //            if (Serviceman.Any(x => x.NewCategory != 2)) {
        //                using (HttpClient client = new HttpClient(GetHttpClientHandler())) {
        //                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/toptech?date=" + DateTime.Now);
        //                    if (response.IsSuccessStatusCode) {
        //                        if (!string.IsNullOrEmpty(await response.Content.ReadAsStringAsync())) {
        //                            Rating = JsonConvert.DeserializeObject<ObservableCollection<RatingServiceman>>(await response.Content.ReadAsStringAsync());
        //                            if (Rating != null)
        //                                if (Rating.Count > 0) {
        //                                    int cnt = 1;
        //                                    foreach (var item in Rating.Take(3)) {
        //                                        RatingServiceman += cnt.ToString() + ". " + item.ServicemanName + " (" + item.CountOrders + ")" + Environment.NewLine;
        //                                        cnt++;
        //                                    }
        //                                }
        //                        }
        //                    }
        //                }
        //            }
        //    }, obj => Serviceman != null);
        //}

        /// <summary>
        /// Команда перехода к странице монтажей
        /// </summary>
        private RelayCommand _GetMountworksCommand;
        public RelayCommand GetMountworksCommand {
            get => _GetMountworksCommand ??= new RelayCommand(async obj => {
                if (Mounters.Any()) {
                    Analytics.TrackEvent("Переход к монтажам",
                    new Dictionary<string, string> {
                        {"MountersPhone",Mounters.First().NewPhone }
                    });
                    MountsViewModel vm = new MountsViewModel(Mounters, Serviceman);
                    App.Current.MainPage = new MountsPage(vm);
                }
                else
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Не определен сотрудник, переход невозможен", Color.Red, LayoutOptions.EndAndExpand), 4000));
            }, obj => Mounters != null && Mounters.Count > 0);
        }
        /// <summary>
        /// Команда перехода к странице с заявками технику
        /// </summary>
        private RelayCommand _GetServiceordersCommand;
        public RelayCommand GetServiceordersCommand {
            get => _GetServiceordersCommand ??= new RelayCommand(async obj => {
                if (Serviceman.Any()) {
                    Analytics.TrackEvent("Переход к заявкам технику",
                        new Dictionary<string, string> {
                        {"ServicemansPhone",Serviceman.First().NewPhone }
                        });
                    ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Serviceman, Mounters,true);
                    App.Current.MainPage = new ServiceOrdersPage(vm);
                }
                else
                    //await Application.Current.MainPage.DisplayAlert("Ошибка","Не определен сотрудник, переход невозможен","OK");
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Не определен сотрудник, переход невозможен", Color.Red, LayoutOptions.EndAndExpand), 4000));
            }, obj => Serviceman != null && Serviceman.Count > 0);
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
                //GetRatingServicemanCommand.ChangeCanExecute();
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
                SettingsPageViewModel vm = new SettingsPageViewModel(Mounters, Serviceman);
                App.Current.MainPage = new SettingsPage(vm);
            });
        }
        /// <summary>
        /// Команда проверки версии приложения и напоминания обновления
        /// </summary>
        private RelayCommand _CheckVersionApp;
        public RelayCommand CheckVersionApp {
            get => _CheckVersionApp ??= new RelayCommand(async obj => {
                //Получаем имя версии приложения из свойств
                AppVersions av = new AppVersions();
                string Version = av.GetVersionAndBuildNumber().VersionNumber;
                HttpStatusCode code = await ClientHttp.Get("/api/Common/VersionNumber?appVersion=" + Version);
                if (code.Equals(HttpStatusCode.MethodNotAllowed)) {//версия установленого приложения и версия указанная как актуальная на сервере - не совпали
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("У Вас установлена не актуальная версия приложения, пожалуйста обновите её. Настройки - Скачать", Color.Red, LayoutOptions.EndAndExpand), 4000));
                    Crashes.TrackError(new Exception("Необновленнная версия приложения"),
                        new Dictionary<string, string> {
                                {"Version",Version },
                                {"MountersName",Mounters.First().NewName.ToString() },
                                {"MountersPhone",Mounters.First().NewPhone.ToString() },
                                {"ServicemanName",Serviceman.First().NewName.ToString() },
                                {"ServicemanPhone",Serviceman.First().NewPhone.ToString() }
                        });
                }
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
