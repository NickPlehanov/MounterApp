using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;

namespace MounterApp.ViewModel {
    public class ServiceOrdersPageViewModel : BaseViewModel {

        private List<NewServicemanExtensionBase> _Servicemans;
        public List<NewServicemanExtensionBase> Servicemans {
            get => _Servicemans;
            set {
                _Servicemans = value;
                OnPropertyChanged(nameof(Servicemans));
            }
        }
        private DateTime _Date;
        public DateTime Date {
            get => _Date;
            set {
                if(value == DateTime.Parse("01.01.2010 00:00:00"))
                    _Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
                else
                    _Date = value;
                //_Date = value.AddHours(-5);
                OnPropertyChanged(nameof(Date));
            }
        }
        public ServiceOrdersPageViewModel(List<NewServicemanExtensionBase> _servicemans) {
            Servicemans = _servicemans;            
        }
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                MainMenuPageViewModel vm = new MainMenuPageViewModel(Servicemans);
                App.Current.MainPage = new MainMenuPage(vm);
            });
        }
        private RelayCommand _GetServiceOrders;
        public RelayCommand GetServiceOrders {
            get => _GetServiceOrders ??= new RelayCommand(async obj => {
                if(Servicemans.Count > 0) {
                    ///api/NewServiceorderExtensionBases/ServiceOrderByUser?usr_ID=FEF46B07-8D7A-E311-920A-00155D01051D&date=18.11.2020
                    using HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/ServiceOrderByUser?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);
                    var resp = response.Content.ReadAsStringAsync().Result;
                    List<NewServiceorderExtensionBase> _servieorders = JsonConvert.DeserializeObject<List<NewServiceorderExtensionBase>>(resp).Where(x=>x.NewNewServiceman == null).ToList();
                    foreach(NewServiceorderExtensionBase item in _servieorders) {
                        ServiceOrders.Add(item);
                    }
                }
            });
        }
        private RelayCommand _SelectServiceOrderCommand;
        public RelayCommand SelectServiceOrderCommand {
            get => _SelectServiceOrderCommand ??= new RelayCommand(async obj => {
                if(ServiceOrder!=null) {
                    ServiceOrderViewModel vm = new ServiceOrderViewModel(ServiceOrder);
                    App.Current.MainPage = new ServiceOrder(vm);
                }
            });
        }


        private NewServiceorderExtensionBase _ServiceOrder;
        public NewServiceorderExtensionBase ServiceOrder {
            get => _ServiceOrder;
            set {
                _ServiceOrder = value;
                OnPropertyChanged(nameof(ServiceOrder));
            }
        }

        private ObservableCollection<NewServiceorderExtensionBase> _ServiceOrders = new ObservableCollection<NewServiceorderExtensionBase>();
        public ObservableCollection<NewServiceorderExtensionBase> ServiceOrders {
            get => _ServiceOrders;
            set {
                _ServiceOrders = value;
                OnPropertyChanged(nameof(ServiceOrders));
            }
        }
    }
}
