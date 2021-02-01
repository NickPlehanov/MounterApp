﻿using Microsoft.AppCenter.Analytics;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
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
        //readonly ClientHttp http = new ClientHttp();
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
            StartDateVisible = true;
            EndDateVisible = true;
            GetButtonVisible = true;
        }
        public EventsPopupViewModel(string objectNumber, DateTime? dt) {
            CloseImage = IconName("close");
            GetImage = IconName("get");
            IndicatorVisible = false;
            OpacityForm = 1;
            ObjectNumber = objectNumber;
            StartDate = dt.Value.AddHours(-4);
            EndDate = dt.Value.AddHours(4);
            GetEvents2.Execute(null);
            StartDateVisible = false;
            EndDateVisible = false;
            GetButtonVisible = false;
        }

        private string _ObjectNumber;
        public string ObjectNumber {
            get => _ObjectNumber;
            set {
                _ObjectNumber = value;
                OnPropertyChanged(nameof(ObjectNumber));
            }
        }

        private RelayCommand _GetEvents2;
        public RelayCommand GetEvents2 {
            get => _GetEvents2 ??= new RelayCommand(async obj => {
                using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.ConnectionClose = true;
                    client.DefaultRequestHeaders.ExpectContinue = false;
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/events?objNumber=" + ObjectNumber +
                                        "&startDate=" + StartDate +
                                        "&endDate=" + EndDate +
                                        "&testFiltered=0&doubleFiltered=0");
                    if(response.IsSuccessStatusCode) {
                        Events = JsonConvert.DeserializeObject<ObservableCollection<GetEventsReceivedFromObject_Result>>(await response.Content.ReadAsStringAsync());
                    }
                }
            });
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
            StartDateVisible = true;
            EndDateVisible = true;
            GetButtonVisible = true;
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
                    Events.Clear();
                    if((EndDate-StartDate).TotalDays>7) {
                        StartDate = EndDate.AddDays(-7.0);
                        //Toast.MakeText(Android.App.Application.Context,"Просмотр событий более чем за неделю, запрещен!",ToastLength.Long).Show(); 
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Просмотр событий более чем за неделю запрещен",Color.Red,LayoutOptions.EndAndExpand),4000));
                    }
                    List<GetEventsReceivedFromObject_Result> _evnts = new List<GetEventsReceivedFromObject_Result>();
                    string obj_number = ServiceOrder != null ? ServiceOrder.NewNumber.ToString() : ServiceOrderFireAlarm.NewNumber.ToString();
                    //Events = await ClientHttp.GetQuery<ObservableCollection<GetEventsReceivedFromObject_Result>>("/api/Andromeda/events?objNumber=" + obj_number +
                    //                        "&startDate=" + StartDate +
                    //                        "&endDate=" + EndDate +
                    //                        "&testFiltered=0&doubleFiltered=0");
                    using (HttpClient client = new HttpClient(GetHttpClientHandler())) {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.ConnectionClose = true;
                        client.DefaultRequestHeaders.ExpectContinue = false;
                        HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/events?objNumber=" + obj_number +
                                            "&startDate=" + StartDate +
                                            "&endDate=" + EndDate +
                                            "&testFiltered=0&doubleFiltered=0");
                        if(response.IsSuccessStatusCode) {
                            Events= JsonConvert.DeserializeObject<ObservableCollection<GetEventsReceivedFromObject_Result>>(await response.Content.ReadAsStringAsync());
                        }
                    }
                }
                else {
                    //await Application.Current.MainPage.DisplayAlert("Ошибка","Дата начала не может быть больше или равна дате окончания","OK");
                    //Toast.MakeText(Android.App.Application.Context,"Дата начала не может быть больше или равна дате окончания",ToastLength.Long).Show();
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Дата начала не может быть больше или равна дате окончания",Color.Red,LayoutOptions.EndAndExpand),4000));
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            });
        }

        private bool _StartDateVisible;
        public bool StartDateVisible {
            get => _StartDateVisible;
            set {
                _StartDateVisible = value;
                OnPropertyChanged(nameof(StartDateVisible));
            }
        }

        private bool _EndDateVisible;
        public bool EndDateVisible {
            get => _EndDateVisible;
            set {
                _EndDateVisible = value;
                OnPropertyChanged(nameof(EndDateVisible));
            }
        }

        private bool _GetButtonVisible;
        public bool GetButtonVisible {
            get => _GetButtonVisible;
            set {
                _GetButtonVisible = value;
                OnPropertyChanged(nameof(GetButtonVisible));
            }
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
                try {
                    Analytics.TrackEvent("Выход со страницы для запроса событий по объекту",
                    new Dictionary<string,string> {
                    {"ServicemanPhone",Servicemans.First().NewPhone }
                    });
                }
                catch { }
                await App.Current.MainPage.Navigation.PopPopupAsync(false);
            });
        }
    }
}
