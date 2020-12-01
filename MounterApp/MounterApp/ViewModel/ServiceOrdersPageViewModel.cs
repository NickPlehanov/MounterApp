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

        private List<NewServicemanExtensionBase> _Servicemans = new List<NewServicemanExtensionBase>();
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
                if(value == DateTime.Parse("01.01.0001 00:00:00"))
                    _Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
                else
                    _Date = value;
                //_Date = value.AddHours(-5);
                OnPropertyChanged(nameof(Date));
            }
        }
        private bool _IndicatorVisible;
        public bool IndicatorVisible {
            get => _IndicatorVisible;
            set {
                _IndicatorVisible = value;
                OnPropertyChanged(nameof(IndicatorVisible));
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
        public ServiceOrdersPageViewModel(List<NewServicemanExtensionBase> _servicemans) {
            Servicemans = _servicemans;
            OpacityForm = 1;
            IndicatorVisible = false;
            GetServiceOrders.Execute(Servicemans);
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
                OpacityForm = 0.1;
                IndicatorVisible = true;
                if(Servicemans.Count > 0) {
                    ///api/NewServiceorderExtensionBases/ServiceOrderByUser?usr_ID=FEF46B07-8D7A-E311-920A-00155D01051D&date=18.11.2020
                    if(Date == DateTime.Parse("01.01.0001 00:00:00"))
                        Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
                    using HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/ServiceOrderByUser?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + Date);
                    var resp = response.Content.ReadAsStringAsync().Result;
                    List<NewServiceorderExtensionBase> _serviceorders = new List<NewServiceorderExtensionBase>();
                    try {
                        _serviceorders = JsonConvert.DeserializeObject<List<NewServiceorderExtensionBase>>(resp).Where(x => x.NewNewServiceman == null).ToList();
                    }
                    catch(Exception ex) {
                        _serviceorders = null;
                    }
                    if(_serviceorders != null) {
                        ServiceOrders.Clear();
                        foreach(NewServiceorderExtensionBase item in _serviceorders) {
                            ServiceOrders.Add(item);
                        }
                    }
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            });
        }
        private RelayCommand _SelectServiceOrderCommand;
        public RelayCommand SelectServiceOrderCommand {
            get => _SelectServiceOrderCommand ??= new RelayCommand(async obj => {
                if(ServiceOrder != null) {
                    ServiceOrderViewModel vm = new ServiceOrderViewModel(ServiceOrder,Servicemans);
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
