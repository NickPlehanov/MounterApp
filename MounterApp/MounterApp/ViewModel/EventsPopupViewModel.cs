using Android.Widget;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class EventsPopupViewModel : BaseViewModel {
        /// <summary>
        /// Конструктор popup-окна для отображения событий по оъекту за выбранный период
        /// </summary>
        /// <param name="_so">Заявка технику</param>
        /// <param name="_servicemans">Техники(список)</param>
        /// <param name="_mounters">Монтажники(список)</param>
        public EventsPopupViewModel(NewServiceorderExtensionBase _so,List<NewServicemanExtensionBase> _servicemans,List<NewMounterExtensionBase> _mounters) {
            ServiceOrder = _so;
            Servicemans = _servicemans;
            Mounters = _mounters;
            CloseImage = IconName("close");
            GetImage = IconName("get");
            IndicatorVisible = false;
            OpacityForm = 1;
        }
        /// <summary>
        /// Конструктор popup-окна для отображения событий по оъекту за выбранный период
        /// </summary>
        /// <param name="_so">Заявка на ПС</param>
        /// <param name="_servicemans">Техники(список)</param>
        /// <param name="_mounters">Монтажники(список)</param>
        public EventsPopupViewModel(NewTest2ExtensionBase _so,List<NewServicemanExtensionBase> _servicemans,List<NewMounterExtensionBase> _mounters) {
            ServiceOrderFireAlarm = _so;
            Servicemans = _servicemans;
            Mounters = _mounters;
            CloseImage = IconName("close");
            GetImage = IconName("get");
            IndicatorVisible = false;
            OpacityForm = 1;
        }
        /// <summary>
        /// Объект для хранения заявки на пс, заполняется в конструкторе
        /// </summary>
        private NewTest2ExtensionBase _ServiceOrderFireAlarm;
        public NewTest2ExtensionBase ServiceOrderFireAlarm {
            get => _ServiceOrderFireAlarm;
            set {
                _ServiceOrderFireAlarm = value;
                OnPropertyChanged(nameof(ServiceOrderFireAlarm));
            }
        }
        /// <summary>
        /// Определяет состояние отображения индикатора загрузки
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
        /// Устанавливает прозрачность формы
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
        /// Значок закрытия
        /// </summary>
        private ImageSource _CloseImage;
        public ImageSource CloseImage {
            get => _CloseImage;
            set {
                _CloseImage = value;
                OnPropertyChanged(nameof(CloseImage));
            }
        }
        /// <summary>
        /// Значок загрузки
        /// </summary>
        private ImageSource _GetImage;
        public ImageSource GetImage {
            get => _GetImage;
            set {
                _GetImage = value;
                OnPropertyChanged(nameof(GetImage));
            }
        }
        /// <summary>
        /// Объект хранит заявку технику, заполняется в конструкторе
        /// </summary>
        private NewServiceorderExtensionBase _ServiceOrder;
        public NewServiceorderExtensionBase ServiceOrder {
            get => _ServiceOrder;
            set {
                _ServiceOrder = value;
                OnPropertyChanged(nameof(ServiceOrder));
            }
        }
        /// <summary>
        /// Хранит список техников, заполняется в конструкторе
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
        /// Дата начала диапазона выборки для событий по объекту
        /// </summary>
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
        /// <summary>
        /// Дата окончания диапазона выборки для событий по объекту
        /// </summary>
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
        /// <summary>
        /// Отслеживаемая коллекция для хранения событий, полученных в качестве результата запроса
        /// </summary>
        private ObservableCollection<GetEventsReceivedFromObject_Result> _Events = new ObservableCollection<GetEventsReceivedFromObject_Result>();
        public ObservableCollection<GetEventsReceivedFromObject_Result> Events {
            get => _Events;
            set {
                _Events = value;
                OnPropertyChanged(nameof(Events));
            }
        }
        /// <summary>
        /// Командой получаем список событий за выбранный диапазон
        /// </summary>
        private AsyncCommand _GetEventsCommands;
        public AsyncCommand GetEventsCommands {
            get => _GetEventsCommands ??= new AsyncCommand(async () => {
                IndicatorVisible = true;
                OpacityForm = 0.1;
                Analytics.TrackEvent("Запрос событий по объекту",
                new Dictionary<string,string> {
                    {"ServicemanPhone",Servicemans.First().NewPhone },
                    {"StartDate",StartDate.ToShortDateString() },
                    {"EndDate",EndDate.ToShortDateString() }
                });
                if(StartDate <= EndDate) {
                    //IndicatorVisible = true;
                    //OpacityForm = 0.1;
                    Events.Clear();
                    string resp = null;
                    //if(StartDate < DateTime.Now.AddHours(-7)) {
                    if((EndDate-StartDate).TotalDays>7) {
                        StartDate = EndDate.AddDays(-7.0);
                        Toast.MakeText(Android.App.Application.Context,"Просмотр событий более чем за неделю, запрещен!",ToastLength.Long).Show();
                    }
                    List<GetEventsReceivedFromObject_Result> _evnts = new List<GetEventsReceivedFromObject_Result>();
                    HttpResponseMessage response = null;
                    using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                        string obj_number = ServiceOrder != null ? ServiceOrder.NewNumber.ToString() : ServiceOrderFireAlarm.NewNumber.ToString();
                        Analytics.TrackEvent("Запрос событий по объекту. Заявка технику",
                        new Dictionary<string,string> {
                                {"ServicemanPhone",Servicemans.First().NewPhone },
                                {"StartDate",StartDate.ToShortDateString() },
                                {"EndDate",EndDate.ToShortDateString() },
                                {"ObjectNumber",obj_number }
                        });
                        response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/events?objNumber=" + obj_number +
                                            "&startDate=" + StartDate +
                                            "&endDate=" + EndDate +
                                            "&testFiltered=0&doubleFiltered=0"
                                            );
                        if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                            resp = response.Content.ReadAsStringAsync().Result;
                            try {
                                Analytics.TrackEvent("Попытка десериализации результата запроса события по объекту",
                                new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"ServicemanPhone",Servicemans.First().NewPhone },
                                    {"StartDate",StartDate.ToShortDateString() },
                                    {"EndDate",EndDate.ToShortDateString() },
                                    {"ObjectNumber",obj_number }
                                });
                                _evnts = JsonConvert.DeserializeObject<List<GetEventsReceivedFromObject_Result>>(resp);
                            }
                            catch(Exception ex) {
                                Crashes.TrackError(new Exception("Ошибка десериализации результата запроса события по объекту"),
                                new Dictionary<string,string> {
                                {"Servicemans",Servicemans.First().NewPhone },
                                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                {"ErrorMessage",ex.Message },
                                {"StatusCode",response.StatusCode.ToString() },
                                {"Response",response.ToString() }
                                });
                            }
                            if(_evnts.Count > 0) {
                                foreach(var item in _evnts)
                                    Events.Add(item);
                            }
                            else
                                Crashes.TrackError(new Exception("Запрос событий по объекту. Пустой результат запроса"),
                                    new Dictionary<string,string> {
                                    {"ServicemanPhone",Servicemans.First().NewPhone },
                                    {"StartDate",StartDate.ToShortDateString() },
                                    {"EndDate",EndDate.ToShortDateString() },
                                    {"ObjectNumber",obj_number },
                                    {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                    {"StatusCode",response.StatusCode.ToString() },
                                    {"Response",response.ToString() }
                                    });
                        }
                        else {
                            resp = null;
                            Crashes.TrackError(new Exception("Запрос событий по объекту. Заявка технику. От сервера не получен корректный ответ"),
                            new Dictionary<string,string> {
                                    {"ServicemanPhone",Servicemans.First().NewPhone },
                                    {"StartDate",StartDate.ToShortDateString() },
                                    {"EndDate",EndDate.ToShortDateString() },
                                    {"ObjectNumber",obj_number },
                                    {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                                    {"StatusCode",response.StatusCode.ToString() },
                                    {"Response",response.ToString() }
                            });
                        }
                    }
                }
                else {
                    await Application.Current.MainPage.DisplayAlert("Ошибка","Дата начала не может быть больше или равна дате окончания","OK");
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            });
        }
        /// <summary>
        /// Монтажники список
        /// </summary>
        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
        }
        /// <summary>
        /// Команда закрытия формы
        /// </summary>
        private RelayCommand _ExitCommand;
        public RelayCommand ExitCommand {
            get => _ExitCommand ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Выход со страницы для запроса событий по объекту",
                new Dictionary<string,string> {
                    {"ServicemanPhone",Servicemans.First().NewPhone }
                });
                await App.Current.MainPage.Navigation.PopPopupAsync(true);
            });
        }
    }
}
