using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class MountsViewModel : BaseViewModel {
        public MountsViewModel() {

        }
        /// <summary>
        /// Список монтажей. Выполнены и отправлены успешно
        /// </summary>
        private ObservableCollection<Mounts> _Mounts = new ObservableCollection<Mounts>();
        public ObservableCollection<Mounts> Mounts {
            get => _Mounts;
            set {
                _Mounts = value;
                OnPropertyChanged(nameof(Mounts));
            }
        }
        /// <summary>
        /// Список не отправленных монтажей
        /// </summary>
        private ObservableCollection<Mounts> _NotSendedMounts = new ObservableCollection<Mounts>();
        public ObservableCollection<Mounts> NotSendedMounts {
            get => _NotSendedMounts;
            set {
                _NotSendedMounts = value;
                OnPropertyChanged(nameof(NotSendedMounts));
            }
        }
        /// <summary>
        /// Выбраннный неотправленный монтаж
        /// </summary>
        private Mounts _NotSendedMount;
        public Mounts NotSendedMount {
            get => _NotSendedMount;
            set {
                _NotSendedMount = value;
                OnPropertyChanged(nameof(NotSendedMount));
            }
        }
        /// <summary>
        /// Конструктор формы монтажей
        /// </summary>
        /// <param name="mounters">Список монтажников</param>
        /// <param name="servicemans">Список техников</param>
        public MountsViewModel(List<NewMounterExtensionBase> mounters, List<NewServicemanExtensionBase> servicemans) {
            Servicemans = servicemans;
            Mounters = mounters;
            HeaderNotSended = "Неотправленные (0)";
            HeaderGoogle = "Запланированные (0)";
            HeaderHistory = "Отправленные (0)";
            ClearHistoryMounts.Execute(null);
            GetNotSendedMounts.Execute(null);
            GetGoogleMounts.Execute(null);
            GetHistoryMounts.Execute(null);

            Opacity = 1;
            IndicatorVisible = false;
            ArrowCircleGoogle = IconName("arrow_circle_down");
            ArrowCircleNotSended = IconName("arrow_circle_down");
            ArrowCircleHistory = IconName("arrow_circle_down");
            DeleteImage = IconName("clear");

            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        }
        /// <summary>
        /// Заголовок меню отправленных монтажей
        /// </summary>
        private string _HeaderHistory;
        public string HeaderHistory {
            get => _HeaderHistory;
            set {
                _HeaderHistory = value;
                OnPropertyChanged(nameof(HeaderHistory));
            }
        }
        /// <summary>
        /// Получаем неотправленные монтажи из локальной базы данных
        /// </summary>
        private RelayCommand _GetNotSendedMounts;
        public RelayCommand GetNotSendedMounts {
            get => _GetNotSendedMounts ??= new RelayCommand(async obj => {
                NotSendedMounts = new ObservableCollection<Mounts>(App.Database.GetMounts().Where(x => x.State == 0 && x.MounterID == Mounters.FirstOrDefault().NewMounterId).ToList());
                HeaderNotSended = string.Format("Неотправленные ({0})",NotSendedMounts.Count);
                NotSendedMountsExpander = NotSendedMounts.Count > 0;
            });
        }
        /// <summary>
        /// Команда отображения справки
        /// </summary>
        private RelayCommand _HelpCommand;
        public RelayCommand HelpCommand {
            get => _HelpCommand ??= new RelayCommand(async obj => {
                string msg = " - На вкладке неотправленные, протаскивание(свайп) монтажа влево - позволяет открыть его, так же как и простым нажатием, свайп вправо - вызывает команду удаления";
                HelpPopupViewModel vm = new HelpPopupViewModel(msg);
                await App.Current.MainPage.Navigation.PushPopupAsync(new HelpPopupPage(vm));
            });
        }
        /// <summary>
        /// Очищаем историю отправленных монтажей, старше 7 дней
        /// </summary>
        private RelayCommand _ClearHistoryMounts;
        public RelayCommand ClearHistoryMounts {
            get => _ClearHistoryMounts ??= new RelayCommand(async obj => {
                DateTime _dt = DateTime.Now.AddDays(-7);
                var _ntMounts = App.Database.GetMounts().Where(x => x.State == 1 && x.MounterID == Mounters.FirstOrDefault().NewMounterId && x.DateSended.Value.Date<= DateTime.Now.AddDays(-7)).ToList();
                if (_ntMounts != null) 
                    foreach (var item in _ntMounts) 
                                App.Database.DeleteMount(item.ID);
            });
        }
        /// <summary>
        /// Получаем историю отправленных монтажей из локальной базы данных
        /// </summary>
        private RelayCommand _GetHistoryMounts;
        public RelayCommand GetHistoryMounts {
            get => _GetHistoryMounts ??= new RelayCommand(async obj => {
                HistoryMounts = new ObservableCollection<Mounts>(App.Database.GetMounts().Where(x => x.State == 1 && x.MounterID == Mounters.FirstOrDefault().NewMounterId).ToList());
                HeaderHistory = string.Format("Отправленные ({0})", HistoryMounts.Count);
            });
        }
        /// <summary>
        /// Иконка удаления
        /// </summary>
        private ImageSource _DeleteImage;
        public ImageSource DeleteImage {
            get => _DeleteImage;
            set {
                _DeleteImage = value;
                OnPropertyChanged(nameof(DeleteImage));
            }
        }
        /// <summary>
        /// Заголовок меню монтажей гугл
        /// </summary>
        private string _HeaderGoogle;
        public string HeaderGoogle {
            get => _HeaderGoogle;
            set {
                _HeaderGoogle = value;
                OnPropertyChanged(nameof(HeaderGoogle));
            }
        }
        /// <summary>
        /// Заголовок Expander-а неотправленных монтажей
        /// </summary>
        private string _HeaderNotSended;
        public string HeaderNotSended {
            get => _HeaderNotSended;
            set {
                _HeaderNotSended = value;
                OnPropertyChanged(nameof(HeaderNotSended));
            }
        }
        /// <summary>
        /// Значение развернуто/свернуто поля гугл-монтажей
        /// </summary>
        private bool _GoogleMountsExpander;
        public bool GoogleMountsExpander {
            get => _GoogleMountsExpander;
            set {
                _GoogleMountsExpander = value;
                ArrowCircleGoogle = _GoogleMountsExpander ? IconName("arrow_circle_up") : (ImageSource)IconName("arrow_circle_down");

                OnPropertyChanged(nameof(GoogleMountsExpander));
            }
        }
        /// <summary>
        /// Значение развернуто свернуто поля отправленных монтажей
        /// </summary>
        private bool _HistoryMountsExpander;
        public bool HistoryMountsExpander {
            get => _HistoryMountsExpander;
            set {
                _HistoryMountsExpander = value;
                ArrowCircleHistory = _HistoryMountsExpander ? IconName("arrow_circle_up") : (ImageSource)IconName("arrow_circle_down");
                OnPropertyChanged(nameof(HistoryMountsExpander));
            }
        }
        /// <summary>
        /// Значение развернуто свернуто поля не отправленных монтажей
        /// </summary>
        private bool _NotSendedMountsExpander;
        public bool NotSendedMountsExpander {
            get => _NotSendedMountsExpander;
            set {
                _NotSendedMountsExpander = value;
                ArrowCircleNotSended = _NotSendedMountsExpander ? IconName("arrow_circle_up") : (ImageSource)IconName("arrow_circle_down");
                OnPropertyChanged(nameof(NotSendedMountsExpander));
            }
        }
        /// <summary>
        /// Команда открытия/закрытия окна гугл-монтажей
        /// </summary>
        private RelayCommand _GoogleMountsExpanderCommand;
        public RelayCommand GoogleMountsExpanderCommand {
            get => _GoogleMountsExpanderCommand ??= new RelayCommand(async obj => {
                GoogleMountsExpander = !GoogleMountsExpander;
            });
        }
        /// <summary>
        /// Команда открытия/закрытия окна отправленных монтажей
        /// </summary>
        private RelayCommand _HistoryMountsExpanderCommand;
        public RelayCommand HistoryMountsExpanderCommand {
            get => _HistoryMountsExpanderCommand ??= new RelayCommand(async obj => {
                HistoryMountsExpander = !HistoryMountsExpander;
            });
        }
        /// <summary>
        /// Команда открытия/закрытия окна не отправленных монтажей
        /// </summary>
        private RelayCommand _NotSendedMountsExpanderCommand;
        public RelayCommand NotSendedMountsExpanderCommand {
            get => _NotSendedMountsExpanderCommand ??= new RelayCommand(async obj => {
                NotSendedMountsExpander = !NotSendedMountsExpander;
            });
        }
        /// <summary>
        /// Прозрачность формы
        /// </summary>
        private double _Opacity;
        public double Opacity {
            get => _Opacity;
            set {
                _Opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }
        /// <summary>
        /// Видимость индикатора загрузки
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
        /// Получаем гугл монтажи
        /// </summary>
        private RelayCommand _GetGoogleMounts;
        public RelayCommand GetGoogleMounts {
            get => _GetGoogleMounts ??= new RelayCommand(async obj => {
                Opacity = 0.1;
                IndicatorVisible = true;
                var mounter = Mounters.FirstOrDefault();
                if (mounter == null) 
                    return;                

                GoogleMounts = await ClientHttp.Get<ObservableCollection<GoogleMountModel>>("/api/Common?phone=7" + mounter.NewPhone + "&date=" + DateTime.Now.Date + "");
                if (GoogleMounts == null) 
                    return;                

                GoogleMountsExpander = GoogleMounts != null;
                HeaderGoogle = string.Format("Запланированные ({0})", GoogleMounts.Count);

                Opacity = 1;
                IndicatorVisible = false;
            });
        }
        /// <summary>
        /// Иконка на Expander-е запланированных(гугл) монтажей
        /// </summary>
        private ImageSource _ArrowCircleGoogle;
        public ImageSource ArrowCircleGoogle {
            get => _ArrowCircleGoogle;
            set {
                _ArrowCircleGoogle = value;
                OnPropertyChanged(nameof(ArrowCircleGoogle));
            }
        }
        /// <summary>
        /// Иконка на Expander-е не отправленных монтажей
        /// </summary>
        private ImageSource _ArrowCircleNotSended;
        public ImageSource ArrowCircleNotSended {
            get => _ArrowCircleNotSended;
            set {
                _ArrowCircleNotSended = value;
                OnPropertyChanged(nameof(ArrowCircleNotSended));
            }
        }
        /// <summary>
        /// Иконка на Expander-е отправленных монтажей
        /// </summary>
        private ImageSource _ArrowCircleHistory;
        public ImageSource ArrowCircleHistory {
            get => _ArrowCircleHistory;
            set {
                _ArrowCircleHistory = value;
                OnPropertyChanged(nameof(ArrowCircleHistory));
            }
        }
        /// <summary>
        /// Список монтажников, заполняется в конструкторе
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
        /// Команда создания нового монтажа
        /// </summary>
        private RelayCommand _NewMountCommand;
        public RelayCommand NewMountCommand {
            get => _NewMountCommand ??= new RelayCommand(async obj => {
                if (Mounters.Count > 0) {
                    NewMountPageViewModel vm = new NewMountPageViewModel(Mounters, Servicemans);
                    App.Current.MainPage = new NewMountpage(vm);
                }
            });
        }
        /// <summary>
        /// Команда выбора неотправленного монтажа для перехода к нему и редактирования
        /// </summary>
        private RelayCommand _SelectMountCommand;
        public RelayCommand SelectMountCommand {
            get => _SelectMountCommand ??= new RelayCommand(async obj => {
                if (obj != null) {
                    if (int.TryParse(obj.ToString(), out int _id)) {
                        NotSendedMount = NotSendedMounts.FirstOrDefault(x => x.ID == _id);
                        if (NotSendedMount != null) {
                            NewMountPageViewModel vm = new NewMountPageViewModel(NotSendedMount, Mounters, false, Servicemans);
                            App.Current.MainPage = new NewMountpage(vm);
                        }
                    }
                    if (Guid.TryParse(obj.ToString(), out Guid guid)) {
                        GoogleMount = GoogleMounts.First(x => x.id == guid);
                        if (GoogleMount != null) {
                            NotSendedMount = GoogleMount;
                            NewMountPageViewModel vm = new NewMountPageViewModel(NotSendedMount, Mounters, false, Servicemans);
                            App.Current.MainPage = new NewMountpage(vm);
                        }
                    }
                }
            });
        }
        /// <summary>
        /// Удаление выбранного неотправленного монтажа
        /// </summary>
        private RelayCommand _DeleteNotSendedMountCommand;
        public RelayCommand DeleteNotSendedMountCommand {
            get => _DeleteNotSendedMountCommand ??= new RelayCommand(async obj => {
                if (obj != null) {
                    int.TryParse(obj.ToString(), out int _id);
                    NotSendedMount = NotSendedMounts.FirstOrDefault(x => x.ID == _id);
                    if (NotSendedMount != null) {
                        bool result = await Application.Current.MainPage.DisplayAlert("Удаление", "Подтвердите удаление", "Удалить", "Отмена");
                        if (result) {
                            NotSendedMounts.Remove(NotSendedMount);
                            App.Database.DeleteMount(_id);
                            NotSendedMounts = new ObservableCollection<Mounts>(App.Database.GetMounts().Where(x => x.State == 0 && x.MounterID == Mounters.FirstOrDefault().NewMounterId).ToList());
                            HeaderGoogle = string.Format("Неотправленные ({0})", NotSendedMounts.Count);
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Монтаж удален из локальной базы", Color.Green, LayoutOptions.EndAndExpand), 4000));
                        }
                    }
                }
            });
        }
        /// <summary>
        /// Команда получения событий по только что смонтированному объекту
        /// </summary>
        private RelayCommand _GetEvents;
        public RelayCommand GetEvents {
            get => _GetEvents ??= new RelayCommand(async obj => {
                int.TryParse(obj.ToString(), out int _id);
                EventsPopupViewModel vm = new EventsPopupViewModel(HistoryMounts.First(x => x.ID == _id).ObjectNumber, HistoryMounts.First(x => x.ID == _id).DateSended);
                await App.Current.MainPage.Navigation.PushPopupAsync(new EventsPopupPage(vm));
            });
        }
        /// <summary>
        /// Команда выхода с формы
        /// </summary>
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                MainMenuPageViewModel vm = new MainMenuPageViewModel(Mounters, Servicemans);
                App.Current.MainPage = new MainMenuPage(vm);
            });
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
        /// Коллекция запланированных (гугл) монтажей
        /// </summary>
        private ObservableCollection<GoogleMountModel> _GoogleMounts = new ObservableCollection<GoogleMountModel>();
        public ObservableCollection<GoogleMountModel> GoogleMounts {
            get => _GoogleMounts;
            set {
                _GoogleMounts = value;
                OnPropertyChanged(nameof(GoogleMounts));
            }
        }
        /// <summary>
        /// Выбранный запланированный (гугл) монтаж
        /// </summary>
        private GoogleMountModel _GoogleMount;
        public GoogleMountModel GoogleMount {
            get => _GoogleMount;
            set {
                _GoogleMount = value;
                NotSendedMount = new Mounts {
                    MounterID = Mounters.FirstOrDefault().NewMounterId,
                    GoogleComment = _GoogleMount.FullInfo
                };
                OnPropertyChanged(nameof(GoogleMount));
            }
        }
        /// <summary>
        /// Коллекция отправленных монтажей
        /// </summary>
        private ObservableCollection<Mounts> _HistoryMounts = new ObservableCollection<Mounts>();
        public ObservableCollection<Mounts> HistoryMounts {
            get => _HistoryMounts;
            set {
                _HistoryMounts = value;
                OnPropertyChanged(nameof(HistoryMounts));
            }
        }
        /// <summary>
        /// хранчит отправленный монтаж
        /// </summary>
        private Mounts _HistoryMount;
        public Mounts HistoryMount {
            get => _HistoryMount;
            set {
                _HistoryMount = value;
                OnPropertyChanged(nameof(HistoryMount));
            }
        }
    }
}