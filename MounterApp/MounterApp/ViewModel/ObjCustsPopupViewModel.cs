using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class ObjCustsPopupViewModel : BaseViewModel {
        /// <summary>
        /// Конструктор формы отображения списка ответсвенных лиц по заявке технику
        /// </summary>
        /// <param name="_serviceorder">Заявка технику</param>
        public ObjCustsPopupViewModel(NewServiceorderExtensionBase_ex _serviceorder) {
            ServiceOrder = _serviceorder;
            GetCustomers.Execute(null);
            ArrowCircleCustomers = IconName("arrow_circle_down");
            OpacityForm = 1;
            IndicatorVisible = false;
            CloseImage = IconName("close");
            CallImage = IconName("call");
        }
        /// <summary>
        /// Конструктор формы отображения списка ответсвенных лиц по заявке технику(ПС)
        /// </summary>
        /// <param name="_serviceorder">Заявка на ПС</param>
        public ObjCustsPopupViewModel(NewTest2ExtensionBase_ex _serviceorder) {
            ServiceOrderFireAlarm = _serviceorder;
            GetCustomers.Execute(null);
            ArrowCircleCustomers = IconName("arrow_circle_down");
            OpacityForm = 1;
            IndicatorVisible = false;
            CloseImage = IconName("close");
            CallImage = IconName("call");
        }
        /// <summary>
        /// Объект: Заявка на ПС
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
        /// Объект: Заявка технику
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
        /// Прозрачность окна
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
        /// состояние Expander-а со списком ответсвенных лиц
        /// </summary>
        private bool _CustomersExpandedState;
        public bool CustomersExpandedState {
            get => _CustomersExpandedState;
            set {
                ArrowCircleCustomers = _CustomersExpandedState ? IconName("arrow_circle_up") : (ImageSource)IconName("arrow_circle_down");
                _CustomersExpandedState = value;
                OnPropertyChanged(nameof(CustomersExpandedState));
            }
        }
        /// <summary>
        /// Иконка стрелочки на Expander-е
        /// </summary>
        private ImageSource _ArrowCircleCustomers;
        public ImageSource ArrowCircleCustomers {
            get => _ArrowCircleCustomers;
            set {
                _ArrowCircleCustomers = value;
                OnPropertyChanged(nameof(ArrowCircleCustomers));
            }
        }
        /// <summary>
        /// Иконка звонка
        /// </summary>
        private ImageSource _CallImage;
        public ImageSource CallImage {
            get => _CallImage;
            set {
                _CallImage = value;
                OnPropertyChanged(nameof(CallImage));
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
        /// <summary>
        /// Коллекция ответсвенных лиц по объекту
        /// </summary>
        private ObservableCollection<ObjCust> _CutomersCollection = new ObservableCollection<ObjCust>();
        public ObservableCollection<ObjCust> CutomersCollection {
            get => _CutomersCollection;
            set {
                _CutomersCollection = value;
                OnPropertyChanged(nameof(CutomersCollection));
            }
        }
        /// <summary>
        /// Команда открытия/закрытия Expander-а со списком ответсвенных лиц
        /// </summary>
        private RelayCommand _CustomersExpanderCommand;
        public RelayCommand CustomersExpanderCommand {
            get => _CustomersExpanderCommand ??= new RelayCommand(async obj => {
                CustomersExpandedState = !CustomersExpandedState;
            });
        }
        /// <summary>
        /// команда закрытия окна
        /// </summary>
        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand {
            get => _CloseCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(false);
            });
        }
        /// <summary>
        /// Команда звонка ответсвенному
        /// </summary>
        private RelayCommand _CallCustomer;
        public RelayCommand CallCustomer {
            get => _CallCustomer ??= new RelayCommand(async obj => {
                if (obj != null) {
                    if (!string.IsNullOrEmpty(obj.ToString())) {
                        Uri uri = new Uri("tel:" + obj);
                        await Launcher.OpenAsync(uri);
                    }
                    else 
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Номер телефона не указан или не корректный", Color.Red, LayoutOptions.EndAndExpand), 4000));                    
                }
                else 
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Номер телефона не указан или не корректный", Color.Red, LayoutOptions.EndAndExpand), 4000));
            });
        }
        /// <summary>
        /// Получение списка ответсвенных лиц
        /// </summary>
        private RelayCommand _GetCustomers;
        public RelayCommand GetCustomers {
            get => _GetCustomers ??= new RelayCommand(async obj => {
                OpacityForm = 0.1;
                IndicatorVisible = true;
                //List<ObjCust> custs = new List<ObjCust>();
                //int ? number = null;
                //if (ServiceOrder != null) {
                //    if (ServiceOrder.NewNumber.HasValue) 
                //        number = ServiceOrder.NewNumber;

                //}

                //if (ServiceOrderFireAlarm != null) {
                //    if (ServiceOrderFireAlarm.NewNumber.HasValue) 
                //        number = ServiceOrderFireAlarm.NewNumber;

                //}
                int? number = ServiceOrder != null ? ServiceOrder.NewNumber.HasValue ? ServiceOrder.NewNumber : (int?)null : ServiceOrderFireAlarm != null ? ServiceOrderFireAlarm.NewNumber.HasValue ? ServiceOrderFireAlarm.NewNumber : (int?)null : (int?)null;
                if (number == null) 
                    return;
                

                CutomersCollection = await ClientHttp.Get<ObservableCollection<ObjCust>>("/api/Andromeda/Customer?ObjectNumber=" + number);
                OpacityForm = 1;
                IndicatorVisible = false;
            });
        }
    }
}
