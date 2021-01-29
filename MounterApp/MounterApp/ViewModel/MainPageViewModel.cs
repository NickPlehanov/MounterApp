//using Android.Content.Res;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace MounterApp.ViewModel {
    public class MainPageViewModel : BaseViewModel {
        //ClientHttp http = new ClientHttp();

        public MainPageViewModel() {

            IndicatorVisible = false;
            OpacityForm = 1;
            if(Application.Current.Properties.ContainsKey("Phone"))
                PhoneNumber = Application.Current.Properties["Phone"] as string;
            if(Application.Current.Properties.ContainsKey("AutoEnter")) {
                if(bool.TryParse(Application.Current.Properties["AutoEnter"].ToString(),out bool tmp))
                    if(tmp && PhoneNumber != null)
                        AuthCommand.Execute(null);
            }
            else {
                Application.Current.Properties["AutoEnter"] = true;
            }
            if(!Application.Current.Properties.ContainsKey("Quality")) {
                Application.Current.Properties["Quality"] = 50;
            }
            if(!Application.Current.Properties.ContainsKey("Compression")) {
                Application.Current.Properties["Compression"] = 50;
            }
            CheckAndRequestPermissions.Execute(null);

            Application.Current.SavePropertiesAsync();
        }

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
        List<NewMounterExtensionBase> mounters = new List<NewMounterExtensionBase>();
        List<NewServicemanExtensionBase> servicemans = new List<NewServicemanExtensionBase>();
        private RelayCommand _GetMounters;
        public RelayCommand GetMounters {
            get => _GetMounters ??= new RelayCommand(async obj => {
                using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.ConnectionClose = true;
                    client.DefaultRequestHeaders.ExpectContinue = false;
                    //HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress + "/api/NewMounterExtensionBases/phone?phone=" + Application.Current.Properties["Phone"].ToString().Substring(1,PhoneNumber.Length - 1));
                    HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress + "/api/NewMounterExtensionBases/phone?phone=" + obj);
                    if(httpResponse.IsSuccessStatusCode) {
                        mounters = JsonConvert.DeserializeObject<List<NewMounterExtensionBase>>(await httpResponse.Content.ReadAsStringAsync());
                    }
                }
                //mounters = await ClientHttp.GetQuery<List<NewMounterExtensionBase>>("/api/NewMounterExtensionBases/phone?phone=" + Application.Current.Properties["Phone"].ToString().Substring(1,PhoneNumber.Length - 1)).ConfigureAwait(false);
            });
        }

        private RelayCommand _GetServicemans;
        public RelayCommand GetServicemans {
            get => _GetServicemans ??= new RelayCommand(async obj => {
                using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.ConnectionClose = true;
                    client.DefaultRequestHeaders.ExpectContinue = false;
                    HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress + "/api/NewServicemanExtensionBases/phone?phone=" + obj);
                    if(httpResponse.IsSuccessStatusCode) {
                        servicemans = JsonConvert.DeserializeObject<List<NewServicemanExtensionBase>>(await httpResponse.Content.ReadAsStringAsync());
                    }
                }
                //servicemans =await  ClientHttp.GetQuery<List<NewServicemanExtensionBase>>("/api/NewServicemanExtensionBases/phone?phone=" + Application.Current.Properties["Phone"].ToString().Substring(1,PhoneNumber.Length - 1)).ConfigureAwait(false);
            });
        }
        private RelayCommand _AuthCommand;
        public RelayCommand AuthCommand {
            get => _AuthCommand ??= new RelayCommand(async obj => {
                IndicatorVisible = true;
                OpacityForm = 0.1;
                Analytics.TrackEvent("App start");
                //using HttpClient client = new HttpClient(GetHttpClientHandler());
                string Phone = null;
                if(PhoneNumber.Substring(0,2) == "+7")
                    PhoneNumber = PhoneNumber.Replace("+7","8");
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

                    using HttpClient client = new HttpClient(GetHttpClientHandler());
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
                    //response = await client.GetAsync(Resources.BaseAddress + "/api/Common/download?ObjectNumber=63020&PhotoType=Схема объекта");
                    //var t = 0;

                    //GetMounters.Execute(Phone);
                    //GetServicemans.Execute(Phone);
                    //using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    //    //client.DefaultRequestHeaders.Clear();
                    //    //client.DefaultRequestHeaders.ConnectionClose = true;
                    //    //client.DefaultRequestHeaders.ExpectContinue = false;
                    //    //HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress + "/api/NewMounterExtensionBases/phone?phone=" + Application.Current.Properties["Phone"].ToString().Substring(1,PhoneNumber.Length - 1));
                    //    HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress + "/api/NewMounterExtensionBases/phone?phone=" + Phone);
                    //    if(httpResponse.IsSuccessStatusCode) {
                    //        mounters = JsonConvert.DeserializeObject<List<NewMounterExtensionBase>>(await httpResponse.Content.ReadAsStringAsync());
                    //    }
                    //}
                    //using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    //    //client.DefaultRequestHeaders.Clear();
                    //    //client.DefaultRequestHeaders.ConnectionClose = true;
                    //    //client.DefaultRequestHeaders.ExpectContinue = false;
                    //    HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress + "/api/NewServicemanExtensionBases/phone?phone=" + Phone);
                    //    if(httpResponse.IsSuccessStatusCode) {
                    //        servicemans = JsonConvert.DeserializeObject<List<NewServicemanExtensionBase>>(await httpResponse.Content.ReadAsStringAsync());
                    //    }
                    //}

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
