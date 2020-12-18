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
            //if(Application.Current.Properties.ContainsKey("Phone")) {
            //    if(mounters == null)
            //        GetMounter.Execute(null);
            //    if(servicemans == null)
            //        GetServiceman.Execute(null);
            //}
            Mounters = mounters;
            Serviceman = servicemans;
            SettingsImage = "settings.png";
            Analytics.TrackEvent("Инициализация окна главного меню приложения");
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
            
        }
        public MainMenuPageViewModel(List<NewMounterExtensionBase> mounters) {
            Mounters = mounters;
            SettingsImage = "settings.png";
            Analytics.TrackEvent("Инициализация окна главного меню приложения");
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        }
        public MainMenuPageViewModel(List<NewServicemanExtensionBase> servicemans) {
            Serviceman = servicemans;
            SettingsImage = "settings.png";
            Analytics.TrackEvent("Инициализация окна главного меню приложения");
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        }
        public MainMenuPageViewModel() {
            SettingsImage = "settings.png";
            Analytics.TrackEvent("Инициализация окна главного меню приложения");
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        }

        private string _PhoneNumber;
        public string PhoneNumber {
            get => _PhoneNumber;
            set {
                _PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        //private RelayCommand _GetMounter;
        //public RelayCommand GetMounter {
        //    get => _GetMounter ??= new RelayCommand(async obj => {
        //        //if(Application.Current.Properties.ContainsKey("Phone")) {
        //        PhoneNumber = Application.Current.Properties["Phone"] as string;
        //        using HttpClient client = new HttpClient(GetHttpClientHandler());
        //        HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewMounterExtensionBases/phone?phone=" + PhoneNumber);
        //        var resp = response.Content.ReadAsStringAsync().Result;
        //        List<NewMounterExtensionBase> mounters = new List<NewMounterExtensionBase>();
        //        try {
        //            if(response.StatusCode.ToString() == "OK") {
        //                Analytics.TrackEvent("Попытка сериализации результата запроса монтажников");
        //                mounters = JsonConvert.DeserializeObject<List<NewMounterExtensionBase>>(resp).Where(x => x.NewIsWorking == true).ToList();
        //            }
        //        }
        //        catch(Exception MountersParseExecption) {
        //            Dictionary<string,string> parameters = new Dictionary<string,string> {
        //                    { "Phone",PhoneNumber },
        //                    { "Error","Не удалось провести сериализацию объекта монтажники" }
        //                };
        //            Crashes.TrackError(MountersParseExecption,parameters);
        //        }
        //        if(mounters != null) {
        //            Mounters.Clear();
        //            foreach(var item in mounters)
        //                Mounters.Add(item);
        //        }
        //        //}
        //    });
        //}

        //private RelayCommand _GetServiceman;
        //public RelayCommand GetServiceman {
        //    get => _GetServiceman ??= new RelayCommand(async obj => {
        //        //if(Application.Current.Properties.ContainsKey("Phone")) {
        //        PhoneNumber = Application.Current.Properties["Phone"] as string;
        //        using HttpClient client = new HttpClient(GetHttpClientHandler());
        //        Analytics.TrackEvent("Запрос техников по номеру телефона");
        //        HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServicemanExtensionBases/phone?phone=" + PhoneNumber.Substring(1,PhoneNumber.Length - 1));
        //        var resp = response.Content.ReadAsStringAsync().Result;
        //        List<NewServicemanExtensionBase> servicemans = new List<NewServicemanExtensionBase>();
        //        try {
        //            if(response.StatusCode.ToString() == "OK") {
        //                Analytics.TrackEvent("Попытка сериализации результата запроса техников");
        //                servicemans = JsonConvert.DeserializeObject<List<NewServicemanExtensionBase>>(resp).Where(x => x.NewIswork == true).ToList();
        //            }
        //        }
        //        catch(Exception ServicemansParseException) {
        //            Dictionary<string,string> parameters = new Dictionary<string,string> {
        //                    { "Phone",PhoneNumber },
        //                    { "Error","Не удалось провести сериализацию объекта техники" }
        //                };
        //            Crashes.TrackError(ServicemansParseException,parameters);
        //        }
        //        if(servicemans != null) {
        //            if(servicemans.Count > 0) {
        //                Serviceman = new List<NewServicemanExtensionBase>();
        //                //Serviceman.Clear();
        //                foreach(var item in servicemans)
        //                    Serviceman.Add(item);
        //            }
        //        }
        //        //}
        //    });
        //}
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
                try {
                    Dictionary<string,string> parameters = new Dictionary<string,string> {
                    {"MountersPhone",Mounters.First().NewPhone }
                    };
                    Analytics.TrackEvent("Переход к монтажам",parameters);
                }
                catch(Exception ex) {
                    Crashes.TrackError(new Exception("Ошибка перехода к странице монтажей(возможно не найден монтажник)"),
                    new Dictionary<string,string> {
                        {"Mounters",Application.Current.Properties["Phone"] as string },
                        {"Exception",ex.Message }
                    });
                }
                MountsViewModel vm = new MountsViewModel(Mounters,Serviceman);
                App.Current.MainPage = new MountsPage(vm);
            },obj => Mounters != null && Mounters.Count > 0);
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
                try {
                    Dictionary<string,string> parameters = new Dictionary<string,string> {
                    {"ServicemansPhone",Serviceman.First().NewPhone }
                };
                    Analytics.TrackEvent("Переход к заявкам технику",parameters);
                }
                catch(Exception ex) {
                    Crashes.TrackError(new Exception("Ошибка перехода к странице заявок техника(возможно не найден техник)"),
                    new Dictionary<string,string> {
                        {"Servicemans",Application.Current.Properties["Phone"] as string },
                        {"Exception",ex.Message }
                    });
                }
                ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Serviceman,Mounters);
                App.Current.MainPage = new ServiceOrdersPage(vm);
            },obj => Serviceman != null && Serviceman.Count > 0);
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
