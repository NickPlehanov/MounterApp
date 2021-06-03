using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Views;
using Rg.Plugins.Popup.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class ObjectInfoViewModel : BaseViewModel {
        /// <summary>
        /// Конструктор для окна с информацией об объекте
        /// </summary>
        /// <param name="_so">Заявка технику</param>
        /// <param name="_servicemans">Список техников</param>
        /// <param name="_mounters">Список монтажников</param>
        public ObjectInfoViewModel(NewServiceorderExtensionBase_ex _so, List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            Mounters = _mounters;
            Servicemans = _servicemans;
            ServiceOrder = _so;
            GetWires.Execute(null);
            GetExtFields.Execute(null);
            ArrowCircleWires = IconName("arrow_circle_down");
            ArrowCircleExtFields = IconName("arrow_circle_down");
            CloseImage = IconName("close");
            OpacityForm = 1;
            IndicatorVisible = false;
        }
        /// <summary>
        /// Конструктор для окна с информацией об объекте
        /// </summary>
        /// <param name="_so">Заявка на ПС</param>
        /// <param name="_servicemans">Список техников</param>
        /// <param name="_mounters">Список монтажников</param>
        public ObjectInfoViewModel(NewTest2ExtensionBase_ex _so, List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            Mounters = _mounters;
            Servicemans = _servicemans;
            ServiceOrderFireAlarm = _so;
            GetWires.Execute(null);
            GetExtFields.Execute(null);
            ArrowCircleWires = IconName("arrow_circle_down");
            ArrowCircleExtFields = IconName("arrow_circle_down");
            CloseImage = IconName("close");
            OpacityForm = 1;
            IndicatorVisible = false;
        }
        /// <summary>
        /// Индикатор видимости загрузки
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
        /// Прозрачность формы
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
        /// Заявка технику
        /// </summary>
        private NewServiceorderExtensionBase_ex _ServiceOrder;
        public NewServiceorderExtensionBase_ex ServiceOrder {
            get => _ServiceOrder;
            set {
                _ServiceOrder = value;
                OnPropertyChanged(nameof(ServiceOrder));
            }
        }
        /// <summary>
        /// Заявка на пс
        /// </summary>
        private NewTest2ExtensionBase_ex _ServiceOrderFireAlarm;
        public NewTest2ExtensionBase_ex ServiceOrderFireAlarm {
            get => _ServiceOrderFireAlarm;
            set {
                _ServiceOrderFireAlarm = value;
                OnPropertyChanged(nameof(ServiceOrderFireAlarm));
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
        /// Коллекция шлейфов объекта
        /// </summary>
        private ObservableCollection<Wires> _WiresCollection = new ObservableCollection<Wires>();
        public ObservableCollection<Wires> WiresCollection {
            get => _WiresCollection;
            set {
                _WiresCollection = value;
                OnPropertyChanged(nameof(WiresCollection));
            }
        }
        /// <summary>
        /// Коллекция дополнительных полей, которые подвязаны в менеджере объектов
        /// </summary>
        private ObservableCollection<ExtFields> _ExtFields = new ObservableCollection<ExtFields>();
        public ObservableCollection<ExtFields> ExtFields {
            get => _ExtFields;
            set {
                _ExtFields = value;
                OnPropertyChanged(nameof(ExtFields));
            }
        }
        /// <summary>
        /// Команда закрытия окна
        /// </summary>
        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand {
            get => _CloseCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(true);
            });
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
        /// Команда получения списка шлейфов на объекте
        /// </summary>
        private RelayCommand _GetWires;
        public RelayCommand GetWires {
            get => _GetWires ??= new RelayCommand(async obj => {
                OpacityForm = 0.1;
                IndicatorVisible = true;
                int? number = ServiceOrder != null ? ServiceOrder.NewNumber.HasValue ? ServiceOrder.NewNumber : (int?)null : ServiceOrderFireAlarm != null ? ServiceOrderFireAlarm.NewNumber.HasValue ? ServiceOrderFireAlarm.NewNumber : (int?)null : (int?)null;
                if (number == null) 
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Номер объекта не найден. Информация о шлейфах недоступна", Color.Red, LayoutOptions.EndAndExpand), 4000));
                
                WiresCollection = await ClientHttp.Get<ObservableCollection<Wires>>("/api/Andromeda/wires?objNumber=" + number);
                OpacityForm = 1;
                IndicatorVisible = false;
            });
        }
        /// <summary>
        /// Команда получения списка доп. полей
        /// </summary>
        private RelayCommand _GetExtFields;
        public RelayCommand GetExtFields {
            get => _GetExtFields ??= new RelayCommand(async obj => {
                OpacityForm = 0.1;
                IndicatorVisible = true;
                int? number = ServiceOrder != null ? ServiceOrder.NewNumber.HasValue ? ServiceOrder.NewNumber : (int?)null : ServiceOrderFireAlarm != null ? ServiceOrderFireAlarm.NewNumber.HasValue ? ServiceOrderFireAlarm.NewNumber : (int?)null : (int?)null;

                if (number == null) 
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Номер объекта не найден. Дополнительная информация из андромеды недоступна", Color.Red, LayoutOptions.EndAndExpand), 4000));
                

                ExtFields = await ClientHttp.Get<ObservableCollection<ExtFields>>("/api/Andromeda/ext?objNumber=" + number);

                OpacityForm = 1;
                IndicatorVisible = false;
            });
        }
        /// <summary>
        /// Состояние Expander-а шлейфов
        /// </summary>
        private bool _WiresExpandedState;
        public bool WiresExpandedState {
            get => _WiresExpandedState;
            set {
                _WiresExpandedState = value;
                ArrowCircleWires = _WiresExpandedState ? IconName("arrow_circle_up") : (ImageSource)IconName("arrow_circle_down");

                OnPropertyChanged(nameof(WiresExpandedState));
            }
        }
        /// <summary>
        /// Состояние Expander-а доп. полей
        /// </summary>
        private bool _ExtFieldsExpandedState;
        public bool ExtFieldsExpandedState {
            get => _ExtFieldsExpandedState;
            set {
                _ExtFieldsExpandedState = value;
                ArrowCircleExtFields = _ExtFieldsExpandedState ? IconName("arrow_circle_up") : (ImageSource)IconName("arrow_circle_down");
                OnPropertyChanged(nameof(ExtFieldsExpandedState));
            }
        }
        /// <summary>
        /// команда управления состояние Expander-а для доп. полей
        /// </summary>
        private RelayCommand _ExtFieldsExpanderCommand;
        public RelayCommand ExtFieldsExpanderCommand {
            get => _ExtFieldsExpanderCommand ??= new RelayCommand(async obj => {
                ExtFieldsExpandedState = !ExtFieldsExpandedState;
            });
        }
        /// <summary>
        /// команда управления состояние Expander-а для шлейфов
        /// </summary>
        private RelayCommand _WiresExpanderCommand;
        public RelayCommand WiresExpanderCommand {
            get => _WiresExpanderCommand ??= new RelayCommand(async obj => {
                WiresExpandedState = !WiresExpandedState;
            });
        }
        /// <summary>
        /// Иконка стрелочки на шлейфах
        /// </summary>
        private ImageSource _ArrowCircleWires;
        public ImageSource ArrowCircleWires {
            get => _ArrowCircleWires;
            set {
                _ArrowCircleWires = value;
                OnPropertyChanged(nameof(ArrowCircleWires));
            }
        }
        /// <summary>
        /// Иконка стрелочки на доп полях
        /// </summary>
        private ImageSource _ArrowCircleExtFields;
        public ImageSource ArrowCircleExtFields {
            get => _ArrowCircleExtFields;
            set {
                _ArrowCircleExtFields = value;
                OnPropertyChanged(nameof(ArrowCircleExtFields));
            }
        }
        /// <summary>
        /// Иконка закрытия
        /// </summary>
        private ImageSource _CloseImage;
        public ImageSource CloseImage {
            get => _CloseImage;
            set {
                _CloseImage = value;
                OnPropertyChanged(nameof(CloseImage));
            }
        }
    }
}
