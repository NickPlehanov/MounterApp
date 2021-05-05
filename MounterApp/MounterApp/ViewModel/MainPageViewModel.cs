using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;
using Android.OS;
using MounterApp.Properties;
using Android.App;
using Android;
using Android.Content;

namespace MounterApp.ViewModel {
    public class MainPageViewModel : BaseViewModel {

        public MainPageViewModel() {

            IndicatorVisible = false;
            OpacityForm = 1;
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("Phone"))
                PhoneNumber = Xamarin.Forms.Application.Current.Properties["Phone"] as string;
            if(Xamarin.Forms.Application.Current.Properties.ContainsKey("AutoEnter")) {
                if(bool.TryParse(Xamarin.Forms.Application.Current.Properties["AutoEnter"].ToString(),out bool tmp))
                    if(tmp && PhoneNumber != null)
                        AuthCommand.Execute(null);
            }
            else {
                Xamarin.Forms.Application.Current.Properties["AutoEnter"] = true;
            }
            if(!Xamarin.Forms.Application.Current.Properties.ContainsKey("Quality")) {
                Xamarin.Forms.Application.Current.Properties["Quality"] = 50;
            }
            if(!Xamarin.Forms.Application.Current.Properties.ContainsKey("Compression")) {
                Xamarin.Forms.Application.Current.Properties["Compression"] = 50;
            }
            CheckAndRequestPermissions.Execute(null);

            Xamarin.Forms.Application.Current.SavePropertiesAsync();

        }
        /// <summary>
        /// Проверяем и спрашиваем разрешения для приложения
        /// </summary>
        private RelayCommand _CheckAndRequestPermissions;
        public RelayCommand CheckAndRequestPermissions {
            get => _CheckAndRequestPermissions ??= new RelayCommand(async obj => {
                await CheckAndRequestPermissionAsync(new StorageRead());
                await CheckAndRequestPermissionAsync(new LocationWhenInUse());
                await CheckAndRequestPermissionAsync(new NetworkState());
                await CheckAndRequestPermissionAsync(new Camera());
                await CheckAndRequestPermissionAsync(new StorageWrite());
            });
        }
        /// <summary>
        /// Список для хранения монтажников
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
        /// Список для хранения техников
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
        /// Команда авторизации в приложении
        /// </summary>
        private RelayCommand _AuthCommand;
        public RelayCommand AuthCommand {
            get => _AuthCommand ??= new RelayCommand(async obj => {

                IndicatorVisible = true;
                OpacityForm = 0.1;
                Analytics.TrackEvent("App start");
                string Phone = null;
                Phone = NormalizePhone(phone: PhoneNumber);
                if (string.IsNullOrEmpty(Phone)) {
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Введен не корректный номер телефона", Color.Red, LayoutOptions.EndAndExpand), 4000));
                    return;
                }
                if (Phone.Length < 10) {
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Введен не корректный номер телефона", Color.Red, LayoutOptions.EndAndExpand), 4000));
                    return;
                }
                //if (Phone.Substring(0,2) == "+7")
                //    Phone = Phone.Replace("+7","8");
                //else if (Phone.Length == 11)
                //    Phone = Phone.Substring(1, Phone.Length - 1);
                //else {
                //    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Введен не корректный номер телефона", Color.Red, LayoutOptions.EndAndExpand), 4000));
                //    Analytics.TrackEvent("Ошибка ввода номера телефона");
                //}
                try {
                    Xamarin.Forms.Application.Current.Properties["Phone"] = Phone;
                    await Xamarin.Forms.Application.Current.SavePropertiesAsync();
                    Analytics.TrackEvent("Сохранение номера телефона в локальную базу данных");
                    Analytics.TrackEvent("Запрос монтажников по номеру телефона");
                    Mounters = await ClientHttp.Get <List<NewMounterExtensionBase>>("/api/NewMounterExtensionBases/phone?phone=" + Phone);
                    Servicemans = await ClientHttp.Get <List<NewServicemanExtensionBase>>("/api/NewServicemanExtensionBases/phone?phone=" + Phone);

                    if(Mounters != null || Servicemans != null) {
                        if(Mounters.Count > 0 || Servicemans.Count > 0) {
                            if(Mounters.Count > 1 || Servicemans.Count > 1) {
                                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Неоднозначно определен сотрудник", Color.Red, LayoutOptions.EndAndExpand), 4000));
                                Dictionary<string,string> parameters = new Dictionary<string,string> {
                                    { "Phone",Phone },
                                    { "Error","Неоднозначно определен сотрудник" }
                                };
                                Crashes.TrackError(new Exception("Неоднозначно определен сотрудник"),parameters);
                            }
                            else {
                                IndicatorVisible = false;
                                MainMenuPageViewModel vm = new MainMenuPageViewModel(Mounters,Servicemans);
                                App.Current.MainPage = new MainMenuPage(vm);
                            }
                        }
                    }
                    else {
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Сотрудника с таким номером телефона не найдено", Color.Red, LayoutOptions.EndAndExpand), 4000));
                        Dictionary<string,string> parameters = new Dictionary<string,string> {
                            { "Phone",Phone },
                            { "Error","Не найден сотрудник по номеру телефона" }
                        };
                        Crashes.TrackError(new Exception("Не найден сотрудник по номеру телефона"),parameters);
                    }
                }
                catch(Exception ex) {
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Нет подключения к интернету или сетевой адрес недоступен", Color.Red, LayoutOptions.EndAndExpand), 4000));
                    Dictionary<string,string> parameters = new Dictionary<string,string> {
                        { "Phone",Phone },
                        { "Error","Не удалось соединится с сервером или десерелизовать результаты запроса" }
                    };
                    Crashes.TrackError(ex,parameters);
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            //},obj=>!string.IsNullOrEmpty(PhoneNumber));
            },obj=>PhoneNumber.Length==16);
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
        /// Номер телефона для авторизации
        /// </summary>
        private string _PhoneNumber;
        public string PhoneNumber {
            get => _PhoneNumber;
            set {
                _PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
        /// <summary>
        /// Прозрачность формы при активации/деактивации индикатора загрузки
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
        /// Сообщение об обшике
        /// </summary>
        private string _Message;
        public string Message {
            get => _Message;
            set {
                _Message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        /// <summary>
        /// При нажатии назад приложение закрывается
        /// </summary>
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                System.Environment.Exit(0);
            });
        }
    }
}
