using Microsoft.AppCenter.Analytics;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public MainMenuPageViewModel(List<NewMounterExtensionBase> mounters,List<NewServicemanExtensionBase> servicemans) {
            Mounters = mounters;
            Serviceman = servicemans;
            SettingsImage = IconName("settings");
            Analytics.TrackEvent("Инициализация окна главного меню приложения");
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
            CheckVersionApp.Execute(null);
        }
        /// <summary>
        /// Команда перехода к странице монтажей
        /// </summary>
        private RelayCommand _GetMountworksCommand;
        public RelayCommand GetMountworksCommand {
            get => _GetMountworksCommand ??= new RelayCommand(async obj => {
                if(Mounters.Any()) {
                    Analytics.TrackEvent("Переход к монтажам",
                    new Dictionary<string,string> {
                        {"MountersPhone",Mounters.First().NewPhone }
                    });
                    MountsViewModel vm = new MountsViewModel(Mounters,Serviceman);
                    App.Current.MainPage = new MountsPage(vm);
                }
                else
                    //await Application.Current.MainPage.DisplayAlert("Ошибка","Не определен сотрудник, переход невозможен","OK");
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Не определен сотрудник, переход невозможен",Color.Red,LayoutOptions.EndAndExpand),4000));
            },obj => Mounters != null && Mounters.Count > 0);
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
                    //await Application.Current.MainPage.DisplayAlert("Ошибка","Не определен сотрудник, переход невозможен","OK");
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Не определен сотрудник, переход невозможен",Color.Red,LayoutOptions.EndAndExpand),4000));
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

        private RelayCommand _CheckVersionApp;
        public RelayCommand CheckVersionApp {
            get => _CheckVersionApp ??= new RelayCommand(async obj => {
                AppVersions av = new AppVersions();
                string Version = av.GetVersionAndBuildNumber().VersionNumber;
                //HttpStatusCode code = await ClientHttp.GetQuery("/api/Common/VersionNumber?appVersion=" + Version);
                //if (code.Equals(HttpStatusCode.MethodNotAllowed))
                //    await Application.Current.MainPage.DisplayAlert("Информация"
                //                ,"У Вас установлена не актуальная версия приложения, пожалуйста обновите её." + Environment.NewLine + Environment.NewLine + "Если Вы не получили ссылку на новую версию, то сообщите свою почту в ИТ-отдел."
                //                ,"OK");
                //else if (code.Equals(HttpStatusCode.OK)) { }
                //else {
                //    Analytics.TrackEvent("Выполнение запроса",
                //    new Dictionary<string,string> {
                //        {"query","/api/Common/VersionNumber?appVersion=" + Version },
                //        {"code",code.ToString() },
                //        {"phone", Application.Current.Properties["Phone"].ToString() }
                //    });
                //}

                //using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                //    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/VersionNumber?appVersion=" + Version);
                //    if(response != null) {
                //        if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) { }
                //        if(response.StatusCode.Equals(HttpStatusCode.MethodNotAllowed)) {
                //            //await Application.Current.MainPage.DisplayAlert("Информация"
                //            //    ,"У Вас установлена не актуальная версия приложения, пожалуйста обновите её." + Environment.NewLine + Environment.NewLine + "Если Вы не получили ссылку на новую версию, то сообщите свою почту в ИТ-отдел."
                //            //    ,"OK");
                //            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("У Вас установлена не актуальная версия приложения, пожалуйста обновите её",Color.Red,LayoutOptions.EndAndExpand),4000));
                //        }
                //    }
                //}

                HttpStatusCode code = await ClientHttp.Get("/api/Common/VersionNumber?appVersion=" + Version);
                //if(code.Equals(HttpStatusCode.OK)) { }
                if(code.Equals(HttpStatusCode.MethodNotAllowed)) 
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("У Вас установлена не актуальная версия приложения, пожалуйста обновите её",Color.Red,LayoutOptions.EndAndExpand),4000));
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
