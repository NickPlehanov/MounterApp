//using Android.Content.Res;
using Android.Hardware;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace MounterApp.ViewModel {
    public class MainPageViewModel : BaseViewModel {

        //HttpClientHandler clientHandler = new HttpClientHandler();
        public MainPageViewModel() {
            IndicatorVisible = false;
            OpacityForm = 1;
            if(Application.Current.Properties.ContainsKey("Phone"))
                PhoneNumber = Application.Current.Properties["Phone"] as string;
            if(Application.Current.Properties.ContainsKey("AutoEnter")) {
                if(bool.TryParse(Application.Current.Properties["AutoEnter"].ToString(),out bool tmp))
                    if(tmp)
                        AuthCommand.Execute(null);
            }
            else {
                Application.Current.Properties["AutoEnter"] = true;
                Application.Current.SavePropertiesAsync();
            }
            if(!Application.Current.Properties.ContainsKey("Quality")) {
                Application.Current.Properties["Quality"] = 50;
                Application.Current.SavePropertiesAsync();
            }
            if(!Application.Current.Properties.ContainsKey("Compression")) {
                Application.Current.Properties["Compression"] = 50;
                Application.Current.SavePropertiesAsync();
            }
            CheckAndRequestPermissions.Execute(null);

            //clientHandler.ServerCertificateCustomValidationCallback = (sender,cert,chain,sslPolicyErrors) => { return true; };
        }

        private RelayCommand _CheckAndRequestPermissions;
        public RelayCommand CheckAndRequestPermissions {
            get => _CheckAndRequestPermissions ??= new RelayCommand(async obj => {
                await CheckAndRequestPermissionAsync(new StorageRead());
                await CheckAndRequestPermissionAsync(new LocationWhenInUse());
                await CheckAndRequestPermissionAsync(new NetworkState());
                await CheckAndRequestPermissionAsync(new Permissions.Camera());
                await CheckAndRequestPermissionAsync(new StorageWrite());
            });
        }
        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : BasePermission {
            var status = await permission.CheckStatusAsync();
            if(status != PermissionStatus.Granted) {
                status = await permission.RequestAsync();
            }

            return status;
        }
        private RelayCommand _AuthCommand;
        public RelayCommand AuthCommand {
            get => _AuthCommand ??= new RelayCommand(async obj => {
                IndicatorVisible = true;
                OpacityForm = 0.1;
                //App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
                Analytics.TrackEvent("App start");
                using HttpClient client = new HttpClient(GetHttpClientHandler());
                string Phone = null;
                if(PhoneNumber.Substring(0,2) == "+7")
                    PhoneNumber=PhoneNumber.Replace("+7","8");
                else if(PhoneNumber.Length == 11) 
                    Phone = PhoneNumber.Substring(1,PhoneNumber.Length - 1);
                else if(PhoneNumber.Length == 10) 
                    Phone = PhoneNumber;
                else {
                    Message = "Введен не корректный номер телефона";
                    Analytics.TrackEvent("Ошибка ввода номера телефона");
                }
                try {
                    Application.Current.Properties["Phone"] = PhoneNumber;
                    await Application.Current.SavePropertiesAsync();
                    Analytics.TrackEvent("Сохранение номера телефона в локальную базу данных");
                    Analytics.TrackEvent("Запрос монтажников по номеру телефона");
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewMounterExtensionBases/phone?phone=" + Phone);
                    var resp = response.Content.ReadAsStringAsync().Result;
                    List<NewMounterExtensionBase> mounters = new List<NewMounterExtensionBase>();
                    try {
                        if(response.StatusCode.ToString() == "OK") {
                            Analytics.TrackEvent("Попытка сериализации результата запроса монтажников");
                            mounters = JsonConvert.DeserializeObject<List<NewMounterExtensionBase>>(resp).Where(x => x.NewIsWorking == true).ToList();
                        }
                    }
                    catch(Exception MountersParseExecption) {
                        Dictionary<string,string> parameters = new Dictionary<string,string> {
                            { "Phone",PhoneNumber },
                            { "Error","Не удалось провести сериализацию объекта монтажники" }
                        };
                        Crashes.TrackError(MountersParseExecption,parameters);
                    }
                    Analytics.TrackEvent("Запрос техников по номеру телефона");
                    response = await client.GetAsync(Resources.BaseAddress + "/api/NewServicemanExtensionBases/phone?phone=" + Phone);
                    resp = response.Content.ReadAsStringAsync().Result;
                    List<NewServicemanExtensionBase> servicemans = new List<NewServicemanExtensionBase>();
                    try {
                        if(response.StatusCode.ToString() == "OK") {
                            Analytics.TrackEvent("Попытка сериализации результата запроса техников");
                            servicemans = JsonConvert.DeserializeObject<List<NewServicemanExtensionBase>>(resp).Where(x => x.NewIswork == true).ToList();
                        }
                    }
                    catch(Exception ServicemansParseException) {
                        Dictionary<string,string> parameters = new Dictionary<string,string> {
                            { "Phone",PhoneNumber },
                            { "Error","Не удалось провести сериализацию объекта техники" }
                        };
                        Crashes.TrackError(ServicemansParseException,parameters);
                    }
                    if(mounters != null || servicemans != null) {
                        if(mounters.Count > 0 || servicemans.Count > 0) {
                            if(mounters.Count > 1 || servicemans.Count > 1) {
                                Message = "Неоднозначно определен сотрудник";
                                Dictionary<string,string> parameters = new Dictionary<string,string> {
                                    { "Phone",PhoneNumber },
                                    { "Error","Неоднозначно определен сотрудник" }
                                };
                                Crashes.TrackError(new Exception("Неоднозначно определен сотрудник"),parameters);
                            }
                            else {
                                IndicatorVisible = false;
                                MainMenuPageViewModel vm = new MainMenuPageViewModel(mounters,servicemans);
                                App.Current.MainPage = new MainMenuPage(vm);
                            }
                        }
                        else {
                            Message = "Сотрудника с таким номером телефона не найдено" + Environment.NewLine + "Проверьте правильность ввода номера телефона";
                            Dictionary<string,string> parameters = new Dictionary<string,string> {
                                { "Phone",PhoneNumber },
                                { "Error","Не найден сотрудник по номеру телефона" }
                            };
                            Crashes.TrackError(new Exception("Не найден сотрудник по номеру телефона"),parameters);
                        }
                    }
                    else {
                        Message = "Сотрудника с таким номером телефона не найдено";
                        Dictionary<string,string> parameters = new Dictionary<string,string> {
                            { "Phone",PhoneNumber },
                            { "Error","Не найден сотрудник по номеру телефона" }
                        };
                        Crashes.TrackError(new Exception("Не найден сотрудник по номеру телефона"),parameters);
                    }
                }
                catch(Exception ex) {
                    Message = "Нет подключения к интернету или сетевой адрес недоступен";
                    Dictionary<string,string> parameters = new Dictionary<string,string> {
                        { "Phone",PhoneNumber },
                        { "UserMessage",Message },
                        { "Error","Не удалось соединится с сервером или десерелизовать результаты запроса" }
                    };
                    Crashes.TrackError(ex,parameters);
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            });
        }
        private bool _IndicatorVisible;
        public bool IndicatorVisible {
            get => _IndicatorVisible;
            set {
                _IndicatorVisible = value;
                OnPropertyChanged(nameof(IndicatorVisible));
            }
        }
        private string _PhoneNumber;
        public string PhoneNumber {
            get => _PhoneNumber;
            set {
                _PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
        private bool _ProgressBarVisible;
        public bool ProgressBarVisible {
            get => _ProgressBarVisible;
            set {
                _ProgressBarVisible = value;
                OnPropertyChanged(nameof(ProgressBarVisible));
            }
        }
        private double _OpacityForm;
        public double OpacityForm {
            get => _OpacityForm;
            set {
                _OpacityForm = value;
                OnPropertyChanged(nameof(OpacityForm));
            }
        }
        private string _Message;
        public string Message {
            get => _Message;
            set {
                _Message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                System.Environment.Exit(0);
            });
        }
    }
}
