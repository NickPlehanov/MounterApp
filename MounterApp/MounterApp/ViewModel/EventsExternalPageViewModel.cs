using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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
    public class EventsExternalPageViewModel : BaseViewModel {
        //readonly ClientHttp http = new ClientHttp();
        public EventsExternalPageViewModel(List<NewMounterExtensionBase> mounters, List<NewServicemanExtensionBase> servicemans) {
            Mounters = mounters;
            Servicemans = servicemans;
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
        private DateTime _StartDate;
        public DateTime StartDate {
            get => _StartDate;
            set {
                if(value == DateTime.Parse("01.01.1900 00:00:00"))
                    _StartDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
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
                    _EndDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy")).AddDays(1);
                else
                    _EndDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        private string _ObjectNumber;
        public string ObjectNumber {
            get => _ObjectNumber;
            set {
                _ObjectNumber = value;
                OnPropertyChanged(nameof(ObjectNumber));
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

        private ImageSource _CloseImage;
        public ImageSource CloseImage {
            get => _CloseImage;
            set {
                _CloseImage = value;
                OnPropertyChanged(nameof(CloseImage));
            }
        }

        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
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

        private ObservableCollection<GetEventsReceivedFromObject_Result> _Events = new ObservableCollection<GetEventsReceivedFromObject_Result>();
        public ObservableCollection<GetEventsReceivedFromObject_Result> Events {
            get => _Events;
            set {
                _Events = value;
                OnPropertyChanged(nameof(Events));
            }
        }

        private RelayCommand _ExitCommand;
        public RelayCommand ExitCommand {
            get => _ExitCommand ??= new RelayCommand(async obj => {
                BackPressedCommand.Execute(null);
            });
        }

        private RelayCommand _GetEventsCommands;
        public RelayCommand GetEventsCommands => _GetEventsCommands ??= new RelayCommand(async obj => {
            Analytics.TrackEvent("Запрос событий по объекту(расширенный)",
            new Dictionary<string,string> {
                    //{"ServicemanPhone",Servicemans.First().NewPhone },
                    {"StartDate",StartDate.ToShortDateString() },
                    {"EndDate",EndDate.ToShortDateString() }
            });
            if(StartDate <= EndDate) {
                IndicatorVisible = true;
                OpacityForm = 0.1;
                //Events = await ClientHttp.GetQuery<ObservableCollection<GetEventsReceivedFromObject_Result>>("/api/Andromeda/events?objNumber=" + ObjectNumber +
                //                        "&startDate=" + StartDate +
                //                        "&endDate=" + EndDate +
                //                        "&testFiltered=0&doubleFiltered=0");




                Events = await ClientHttp.Get<ObservableCollection<GetEventsReceivedFromObject_Result>>("/api/Andromeda/events?objNumber=" + ObjectNumber +
                                        "&startDate=" + StartDate +
                                        "&endDate=" + EndDate +
                                        "&testFiltered=0&doubleFiltered=0"
                                        );
                //Events.Clear();
                //string resp = null;
                //List<GetEventsReceivedFromObject_Result> _evnts = new List<GetEventsReceivedFromObject_Result>();
                //HttpResponseMessage response = null;
                //using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                //    //string obj_number = ServiceOrder != null ? ServiceOrder.NewNumber.ToString() : ServiceOrderFireAlarm.NewNumber.ToString();
                //    Analytics.TrackEvent("Запрос событий по объекту(расширенный)",
                //    new Dictionary<string,string> {
                //                //{"ServicemanPhone",Servicemans.First().NewPhone },
                //                {"StartDate",StartDate.ToShortDateString() },
                //                {"EndDate",EndDate.ToShortDateString() },
                //                {"ObjectNumber",ObjectNumber }
                //    });
                //    response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/events?objNumber=" + ObjectNumber +
                //                        "&startDate=" + StartDate +
                //                        "&endDate=" + EndDate +
                //                        "&testFiltered=0&doubleFiltered=0"
                //                        );
                //    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                //        resp = response.Content.ReadAsStringAsync().Result;
                //        try {
                //            Analytics.TrackEvent("Попытка десериализации результата запроса события по объекту(расширенный)",
                //            new Dictionary<string,string> {
                //                //{"Servicemans",Servicemans.First().NewPhone },
                //                //{"ServicemanPhone",Servicemans.First().NewPhone },
                //                    {"StartDate",StartDate.ToShortDateString() },
                //                    {"EndDate",EndDate.ToShortDateString() },
                //                    {"ObjectNumber",ObjectNumber }
                //            });
                //            _evnts = JsonConvert.DeserializeObject<List<GetEventsReceivedFromObject_Result>>(resp);
                //        }
                //        catch(Exception ex) {
                //            Crashes.TrackError(new Exception("Ошибка десериализации результата запроса события по объекту(расширенный)"),
                //            new Dictionary<string,string> {
                //                //{"Servicemans",Servicemans.First().NewPhone },
                //                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                //                {"ErrorMessage",ex.Message },
                //                {"StatusCode",response.StatusCode.ToString() },
                //                {"Response",response.ToString() }
                //            });
                //        }
                //        if(_evnts.Count > 0) {
                //            foreach(var item in _evnts)
                //                Events.Add(item);
                //        }
                //        else
                //            Crashes.TrackError(new Exception("Запрос событий по объекту. Пустой результат запроса(расширенный)"),
                //                new Dictionary<string,string> {
                //                    //{"ServicemanPhone",Servicemans.First().NewPhone },
                //                    {"StartDate",StartDate.ToShortDateString() },
                //                    {"EndDate",EndDate.ToShortDateString() },
                //                    {"ObjectNumber",ObjectNumber },
                //                    {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                //                    {"StatusCode",response.StatusCode.ToString() },
                //                    {"Response",response.ToString() }
                //                });
                //    }
                //    else {
                //        resp = null;
                //        Crashes.TrackError(new Exception("Запрос событий по объекту. Заявка технику. От сервера не получен корректный ответ(расширенный)"),
                //        new Dictionary<string,string> {
                //                    //{"ServicemanPhone",Servicemans.First().NewPhone },
                //                    {"StartDate",StartDate.ToShortDateString() },
                //                    {"EndDate",EndDate.ToShortDateString() },
                //                    {"ObjectNumber",ObjectNumber },
                //                    {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                //                    {"StatusCode",response.StatusCode.ToString() },
                //                    {"Response",response.ToString() }
                //        });
                //    }
                //}
            }
            else {
                //await Application.Current.MainPage.DisplayAlert("Ошибка","Дата начала не может быть больше или равна дате окончания","OK");
                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Дата начала не может быть больше или равна дате окончания",Color.Red,LayoutOptions.EndAndExpand),4000));
            }
            IndicatorVisible = false;
            OpacityForm = 1;
        });
        private RelayCommand _BackPressedCommand;
        public RelayCommand BackPressedCommand {
            get => _BackPressedCommand ??= new RelayCommand(async obj => {
                SettingsPageViewModel vm = new SettingsPageViewModel(Mounters,Servicemans);
                App.Current.MainPage = new SettingsPage(vm);
            });
        }
    }
}
