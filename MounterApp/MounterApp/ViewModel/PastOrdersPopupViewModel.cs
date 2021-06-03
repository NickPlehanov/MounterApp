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
    public class PastOrdersPopupViewModel : BaseViewModel {
        /// <summary>
        /// Конструктор окна для просмотр прошлых заявок по объекту
        /// </summary>
        /// <param name="_so">Заявка технику</param>
        /// <param name="_servicemans">Список техников</param>
        /// <param name="_mounters">Список монтажников</param>
        public PastOrdersPopupViewModel(NewServiceorderExtensionBase_ex _so, List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            Mounters = _mounters;
            Servicemans = _servicemans;
            ServiceOrder = _so;
            GetPastServiceOrders.Execute(true);
            //ArrowCirclePastServiceOrders = IconName("arrow_circle_down");
            CloseImage = IconName("close");
            IndicatorVisible = false;
            OpacityForm = 1;
            CallImage = IconName("call");
        }
        /// <summary>
        /// Конструктор окна для просмотр прошлых заявок по объекту
        /// </summary>
        /// <param name="_so">Заявка на ПС</param>
        /// <param name="_servicemans">Список техников</param>
        /// <param name="_mounters">Список монтажников</param>
        public PastOrdersPopupViewModel(NewTest2ExtensionBase_ex _so, List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            Mounters = _mounters;
            Servicemans = _servicemans;
            ServiceOrderFireAlarm = _so;
            GetPastServiceOrders.Execute(false);
            //ArrowCirclePastServiceOrders = IconName("arrow_circle_down");
            CloseImage = IconName("close");
            IndicatorVisible = false;
            OpacityForm = 1;
            CallImage = IconName("call");
        }
        /// <summary>
        /// Заявка на ПС
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
        /// Видимость индикатора
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
        /// Список прошлых заявок
        /// </summary>
        private ObservableCollection<NewServiceorderExtensionBase_ex> _PastServiceOrders = new ObservableCollection<NewServiceorderExtensionBase_ex>();
        public ObservableCollection<NewServiceorderExtensionBase_ex> PastServiceOrders {
            get => _PastServiceOrders;
            set {
                _PastServiceOrders = value;
                OnPropertyChanged(nameof(PastServiceOrders));
            }
        }
        /// <summary>
        /// Получение списка прошлых заявок
        /// Параметр True - Заявка технику
        /// Параметр False - Заявка на ПС
        /// </summary>
        private RelayCommand _GetPastServiceOrders;
        public RelayCommand GetPastServiceOrders {
            get => _GetPastServiceOrders ??= new RelayCommand(async obj => {
                if (obj == null) 
                    return;
                IndicatorVisible = true;
                OpacityForm = 0.1;
                Guid? andr = ServiceOrder != null ? ServiceOrder.NewAndromedaServiceorder : ServiceOrderFireAlarm.NewAndromedaServiceorder;

                bool? b = obj as bool?;
                if (b.Value == true) 
                    PastServiceOrders = await ClientHttp.Get<ObservableCollection<NewServiceorderExtensionBase_ex>>("/api/NewServiceorderExtensionBases/ServiceOrderByObjectNew?Andromeda_ID=" + andr);                

                if (b.Value == false) {
                    var pastFireOrders = await ClientHttp.Get<ObservableCollection<NewTest2ExtensionBase_ex>>("/api/NewServiceOrderForFireAlarmExtensionBase/ServiceOrderByObjectNew?Andromeda_ID=" + andr);
                    foreach (var item in pastFireOrders) 
                        PastServiceOrders.Add(new NewServiceorderExtensionBase_ex() { ServiceOrderInfo = item.ServiceOrderInfo, ServicemanInfo = item.ServicemanInfo });                    
                }                
                IndicatorVisible = false;
                OpacityForm = 1;
            });
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
        /// Звонок технику, кто был на заявке
        /// </summary>
        private RelayCommand _CallServiceman;
        public RelayCommand CallServiceman {
            get => _CallServiceman ??= new RelayCommand(async obj => {
                if (obj != null) {
                    if (!string.IsNullOrEmpty(obj.ToString())) {
                        Uri uri = new Uri("tel:" + obj);
                        await Launcher.OpenAsync(uri);
                    }
                    else 
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Номер телефона не указан", Color.Red, LayoutOptions.EndAndExpand), 4000));                    
                }
                else 
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Номер телефона не указан", Color.Red, LayoutOptions.EndAndExpand), 4000));                
            });
        }
        /// <summary>
        /// Команда закрытия окна
        /// </summary>
        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand {
            get => _CloseCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(false);
            });
        }
        /// <summary>
        /// Открытие/закрытие Expander-а со списком прошлых заявок
        /// </summary>
        //private bool _PastServiceOrdersExpandedState;
        //public bool PastServiceOrdersExpandedState {
        //    get => _PastServiceOrdersExpandedState;
        //    set {
        //        ArrowCirclePastServiceOrders = _PastServiceOrdersExpandedState ? IconName("arrow_circle_up") : (ImageSource)IconName("arrow_circle_down");

        //        _PastServiceOrdersExpandedState = value;
        //        OnPropertyChanged(nameof(PastServiceOrdersExpandedState));
        //    }
        //}
        //private ImageSource _ArrowCirclePastServiceOrders;
        //public ImageSource ArrowCirclePastServiceOrders {
        //    get => _ArrowCirclePastServiceOrders;
        //    set {
        //        _ArrowCirclePastServiceOrders = value;
        //        OnPropertyChanged(nameof(ArrowCirclePastServiceOrders));
        //    }
        //}

        //private RelayCommand _PastServiceOrdersExpanderCommand;
        //public RelayCommand PastServiceOrdersExpanderCommand {
        //    get => _PastServiceOrdersExpanderCommand ??= new RelayCommand(async obj => {
        //        PastServiceOrdersExpandedState = !PastServiceOrdersExpandedState;
        //    });
        //}
    }
}
