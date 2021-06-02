using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class EventsExternalPageViewModel : BaseViewModel {
        /// <summary>
        /// Конструктор для формы получения событий по объекту(пасхалка)
        /// </summary>
        /// <param name="mounters">Список монтажников</param>
        /// <param name="servicemans">Список техников</param>
        public EventsExternalPageViewModel(List<NewMounterExtensionBase> mounters, List<NewServicemanExtensionBase> servicemans) {
            Mounters = mounters;
            Servicemans = servicemans;
            GetImage = IconName("get");
            CloseImage = IconName("close");
        }
        /// <summary>
        /// Свойство для видимости индикатора загрузки
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
        /// Свойство сохраняющее значение для степени прозрачности формы
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
        /// Начальная дата для отбора событий
        /// </summary>
        private DateTime _StartDate;
        public DateTime StartDate {
            get => _StartDate;
            set {
                if (value == DateTime.Parse("01.01.1900 00:00:00"))
                    _StartDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
                else
                    _StartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        /// <summary>
        /// Конечная дата для отбора событий
        /// </summary>
        private DateTime _EndDate;
        public DateTime EndDate {
            get => _EndDate;
            set {
                if (value == DateTime.Parse("01.01.1900 00:00:00"))
                    _EndDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy")).AddDays(1);
                else
                    _EndDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        /// <summary>
        /// Номер объекта, по которому будет производится запрос событий
        /// </summary>
        private string _ObjectNumber;
        public string ObjectNumber {
            get => _ObjectNumber;
            set {
                _ObjectNumber = value;
                GetEventsCommands.ChangeCanExecute();
                OnPropertyChanged(nameof(ObjectNumber));
            }
        }
        /// <summary>
        /// Икнока для кнопки "Получить/Запросить"
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
        /// Иконка для кнопки "Закрыть"
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
        /// Список монтажников
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
        /// Список техников
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
        /// Отслеживаемая коллекция для хранения событий
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
        /// Команда получения списка событий за указанный период (StartDate и EndDate), но номеру объекта (ObjectNumber)
        /// </summary>
        private RelayCommand _GetEventsCommands;
        public RelayCommand GetEventsCommands => _GetEventsCommands ??= new RelayCommand(async obj => {
            //Analytics.TrackEvent("Запрос событий по объекту(расширенный)",
            //new Dictionary<string, string> {
            //        {"StartDate",StartDate.ToShortDateString() },
            //        {"EndDate",EndDate.ToShortDateString() },
            //        {"ObjectNumber",ObjectNumber }
            //});
            if (string.IsNullOrEmpty(ObjectNumber)) {
                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Номер объкта не определен, выполнение запроса событий отменено.", Color.Red, LayoutOptions.EndAndExpand), 4000));
                return;
            }
            if (StartDate <= EndDate) {
                IndicatorVisible = true;
                OpacityForm = 0.1;
                Events = await ClientHttp.Get<ObservableCollection<GetEventsReceivedFromObject_Result>>("/api/Andromeda/events?objNumber=" + ObjectNumber +
                                                        "&startDate=" + StartDate +
                                                        "&endDate=" + EndDate +
                                                        "&testFiltered=0&doubleFiltered=0"
                                                        );
            }
            else {
                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Дата начала не может быть больше или равна дате окончания", Color.Red, LayoutOptions.EndAndExpand), 4000));
            }
            IndicatorVisible = false;
            OpacityForm = 1;
        },obj=> StartDate <= EndDate && !string.IsNullOrEmpty(ObjectNumber));
        /// <summary>
        /// Команда выхода с формы
        /// </summary>
        private RelayCommand _BackPressedCommand;
        public RelayCommand BackPressedCommand {
            get => _BackPressedCommand ??= new RelayCommand(async obj => {
                SettingsPageViewModel vm = new SettingsPageViewModel(Mounters, Servicemans);
                App.Current.MainPage = new SettingsPage(vm);
            });
        }
    }
}
