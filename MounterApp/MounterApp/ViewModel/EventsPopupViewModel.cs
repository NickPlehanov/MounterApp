using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class EventsPopupViewModel : BaseViewModel {
        public EventsPopupViewModel(NewServiceorderExtensionBase _so,List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            ServiceOrder = _so;
            Servicemans = _servicemans;
            Mounters = _mounters;
            CloseImage = "close.png";
            GetImage = "get.png";
            IndicatorVisible = false;
            OpacityForm = 1;
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
        private ImageSource _CloseImage;
        public ImageSource CloseImage {
            get => _CloseImage;
            set {
                _CloseImage = value;
                OnPropertyChanged(nameof(CloseImage));
            }
        }

        private ImageSource _GetImage;
        public ImageSource GetImage {
            get => _GetImage;
            set {
                _GetImage = value;
                OnPropertyChanged(nameof(GetImage));
            }
        }
        private NewServiceorderExtensionBase _ServiceOrder;
        public NewServiceorderExtensionBase ServiceOrder {
            get => _ServiceOrder;
            set {
                _ServiceOrder = value;
                OnPropertyChanged(nameof(ServiceOrder));
            }
        }
        private List<NewServicemanExtensionBase> _Servicemans;
        public List<NewServicemanExtensionBase> Servicemans {
            get => _Servicemans;
            set {
                _Servicemans = value;
                OnPropertyChanged(nameof(Servicemans));
            }
        }
        private DateTime _StartDate;
        public DateTime StartDate {
            get => _StartDate;
            set {
                if(value == DateTime.Parse("01.01.1900 00:00:00"))
                    _StartDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy")).AddDays(-1);
                else
                    _StartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        private DateTime _EndDate;
        public DateTime EndDate {
            get => _EndDate;
            set {
                if(value == DateTime.Parse("01.01.1900 00:00:00"))
                    _EndDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
                else
                    _EndDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        private ObservableCollection<GetEventsReceivedFromObject_Result> _Events = new ObservableCollection<GetEventsReceivedFromObject_Result>();
        public ObservableCollection<GetEventsReceivedFromObject_Result> Events {
            get => _Events;
            set {
                _Events = value;
                OnPropertyChanged(nameof(Events));
            }
        }

        private RelayCommand _GetEventsCommands;
        public RelayCommand GetEventsCommands {
            get => _GetEventsCommands ??= new RelayCommand(async obj => {
                IndicatorVisible = true;
                OpacityForm = 0.1;
                Events.Clear();
                using HttpClient client = new HttpClient(GetHttpClientHandler());
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/events?objNumber=" + ServiceOrder.NewNumber +
                    "&startDate=" + StartDate +
                    "&endDate=" + EndDate +
                    "&testFiltered=0&doubleFiltered=0"
                    );
                var resp = response.Content.ReadAsStringAsync().Result;
                List<GetEventsReceivedFromObject_Result> _evnts = new List<GetEventsReceivedFromObject_Result>();
                try {
                    _evnts = JsonConvert.DeserializeObject<List<GetEventsReceivedFromObject_Result>>(resp);
                }
                catch(Exception ex) { }
                if(_evnts.Count > 0) {
                    foreach(var item in _evnts)
                        Events.Add(item);
                    //EventsVisible = true;
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            },obj => StartDate <= EndDate);
        }

        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
        }
        private RelayCommand _ExitCommand;
        public RelayCommand ExitCommand {
            get => _ExitCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(true);
                //ServiceOrderViewModel vm = new ServiceOrderViewModel(ServiceOrder,Servicemans,Mounters);
                //App.Current.MainPage = new ServiceOrder(vm);
            });
        }
    }
}
