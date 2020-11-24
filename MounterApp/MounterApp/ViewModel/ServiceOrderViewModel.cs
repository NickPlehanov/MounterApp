using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;

namespace MounterApp.ViewModel {
    public class ServiceOrderViewModel : BaseViewModel {
        public ServiceOrderViewModel(NewServiceorderExtensionBase _so) {
            ServiceOrderID = _so;
        }
        public ServiceOrderViewModel(NewServiceorderExtensionBase _so,List<NewServicemanExtensionBase> _servicemans) {
            ServiceOrderID = _so;
            Servicemans = _servicemans;
            GetInfoByGuardObject.Execute(null);
        }
        private List<NewServicemanExtensionBase> _Servicemans;
        public List<NewServicemanExtensionBase> Servicemans {
            get => _Servicemans;
            set {
                _Servicemans = value;
                OnPropertyChanged(nameof(Servicemans));
            }
        }
        private NewServiceorderExtensionBase _ServiceOrderID;
        public NewServiceorderExtensionBase ServiceOrderID {
            get => _ServiceOrderID;
            set {
                _ServiceOrderID = value;
                OnPropertyChanged(nameof(ServiceOrderID));
            }
        }
        private string _Siding;
        public string Siding {
            get => _Siding;
            set {
                _Siding = value;
                OnPropertyChanged(nameof(Siding));
            }
        }
        private string _Contact;
        public string Contact {
            get => _Contact;
            set {
                _Contact = value;
                OnPropertyChanged(nameof(Contact));
            }
        }
        private bool _rrOS;
        public bool rrOS {
            get => _rrOS;
            set {
                _rrOS = value;
                OnPropertyChanged(nameof(rrOS));
            }
        }
        private bool _rrPS;
        public bool rrPS {
            get => _rrPS;
            set {
                _rrPS = value;
                OnPropertyChanged(nameof(rrPS));
            }
        }
        private bool _rrVideo;
        public bool rrVideo {
            get => _rrVideo;
            set {
                _rrVideo = value;
                OnPropertyChanged(nameof(rrVideo));
            }
        }
        private bool _rrAccess;
        public bool rrAccess {
            get => _rrAccess;
            set {
                _rrAccess = value;
                OnPropertyChanged(nameof(rrAccess));
            }
        }

        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Servicemans);
                App.Current.MainPage = new ServiceOrdersPage(vm);
            });
        }
        private RelayCommand _GetInfoByGuardObject;
        public RelayCommand GetInfoByGuardObject {
            get => _GetInfoByGuardObject ??= new RelayCommand(async obj => {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewGuardObjectExtensionBases/GetInfoByNumber?number=" + ServiceOrderID.NewNumber);
                var resp = response.Content.ReadAsStringAsync().Result;
                List<NewGuardObjectExtensionBase> goeb = new List<NewGuardObjectExtensionBase>();
                try {
                    goeb = JsonConvert.DeserializeObject<List<NewGuardObjectExtensionBase>>(resp);
                }
                catch { }
                if(goeb.Count > 0) {
                    Contact = goeb.FirstOrDefault().NewFirstcontact;
                    Siding = goeb.FirstOrDefault().NewSiding;
                    rrOS = goeb.FirstOrDefault().NewRrOs.HasValue ? (bool)goeb.FirstOrDefault().NewRrOs : false;
                    rrPS = goeb.FirstOrDefault().NewRrPs.HasValue ? (bool)goeb.FirstOrDefault().NewRrPs : false;
                    rrVideo = goeb.FirstOrDefault().NewRrVideo.HasValue ? (bool)goeb.FirstOrDefault().NewRrVideo : false;
                    rrAccess = goeb.FirstOrDefault().NewRrSkud.HasValue ? (bool)goeb.FirstOrDefault().NewRrSkud : false;
                }
                //}
            });
        }
        private RelayCommand _IncomeCommand;
        public RelayCommand IncomeCommand {
            get => _IncomeCommand ??= new RelayCommand(async obj => {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/id?id=" + ServiceOrderID.NewServiceorderId);
                var resp = response.Content.ReadAsStringAsync().Result;
                NewServiceorderExtensionBase soeb = null;
                try {
                    soeb = JsonConvert.DeserializeObject<NewServiceorderExtensionBase>(resp);
                }
                catch { }
                if(soeb != null) {
                    soeb.NewIncome = DateTime.Now.AddHours(-5);
                    using HttpClient clientPut = new HttpClient();
                    var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
                    HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases",httpContent);
                }
            });
        }
        private RelayCommand _OutcomeCommand;
        public RelayCommand OutcomeCommand {
            get => _OutcomeCommand ??= new RelayCommand(async obj => {
                
            });
        }

        private RelayCommand _CallClientCommand;
        public RelayCommand CallClientCommand {
            get => _CallClientCommand ??= new RelayCommand(async obj => {
                Uri uri = new Uri("tel:" + obj);
                await Launcher.OpenAsync(uri);
            });
        }
        private RelayCommand _CloseOrderCommand;
        public RelayCommand CloseOrderCommand {
            get => _CloseOrderCommand ??= new RelayCommand(async obj => {
                CloseOrderPopupPageViewModel vm = new CloseOrderPopupPageViewModel(ServiceOrderID,Servicemans);
                await App.Current.MainPage.Navigation.PushPopupAsync(new CloseOrderPopupPage(vm));
            });
        }
    }
}
