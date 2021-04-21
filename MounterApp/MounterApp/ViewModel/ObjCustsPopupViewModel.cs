using Android.Widget;
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
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class ObjCustsPopupViewModel : BaseViewModel {
        //readonly ClientHttp http = new ClientHttp();

        public ObjCustsPopupViewModel(NewServiceorderExtensionBase_ex _serviceorder) {
            ServiceOrder = _serviceorder;
            GetCustomers.Execute(null);
            ArrowCircleCustomers = IconName("arrow_circle_down");
            OpacityForm = 1;
            IndicatorVisible = false;
            CloseImage = IconName("close");
            CallImage = IconName("call");
            Analytics.TrackEvent("Инициализация окна списка ответсвенных");
        }
        public ObjCustsPopupViewModel(NewTest2ExtensionBase_ex _serviceorder) {
            ServiceOrderFireAlarm = _serviceorder;
            GetCustomers.Execute(null);
            ArrowCircleCustomers = IconName("arrow_circle_down");
            OpacityForm = 1;
            IndicatorVisible = false;
            CloseImage = IconName("close");
            CallImage = IconName("call");
            Analytics.TrackEvent("Инициализация окна списка ответсвенных");
        }

        private NewTest2ExtensionBase_ex _ServiceOrderFireAlarm;
        public NewTest2ExtensionBase_ex ServiceOrderFireAlarm {
            get => _ServiceOrderFireAlarm;
            set {
                _ServiceOrderFireAlarm = value;
                OnPropertyChanged(nameof(ServiceOrderFireAlarm));
            }
        }
        private NewServiceorderExtensionBase_ex _ServiceOrder;
        public NewServiceorderExtensionBase_ex ServiceOrder {
            get => _ServiceOrder;
            set {
                _ServiceOrder = value;
                OnPropertyChanged(nameof(ServiceOrder));
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

        private bool _CustomersExpandedState;
        public bool CustomersExpandedState {
            get => _CustomersExpandedState;
            set {
                if (_CustomersExpandedState)
                    ArrowCircleCustomers = IconName("arrow_circle_up");
                else
                    ArrowCircleCustomers = IconName("arrow_circle_down");
                _CustomersExpandedState = value;
                OnPropertyChanged(nameof(CustomersExpandedState));
            }
        }

        private ImageSource _ArrowCircleCustomers;
        public ImageSource ArrowCircleCustomers {
            get => _ArrowCircleCustomers;
            set {
                _ArrowCircleCustomers = value;
                OnPropertyChanged(nameof(ArrowCircleCustomers));
            }
        }

        private ImageSource _CallImage;
        public ImageSource CallImage {
            get => _CallImage;
            set {
                _CallImage = value;
                OnPropertyChanged(nameof(CallImage));
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

        private ObservableCollection<ObjCust> _CutomersCollection = new ObservableCollection<ObjCust>();
        public ObservableCollection<ObjCust> CutomersCollection {
            get => _CutomersCollection;
            set {
                _CutomersCollection = value;
                OnPropertyChanged(nameof(CutomersCollection));
            }
        }

        private RelayCommand _CustomersExpanderCommand;
        public RelayCommand CustomersExpanderCommand {
            get => _CustomersExpanderCommand ??= new RelayCommand(async obj => {
                CustomersExpandedState = !CustomersExpandedState;
            });
        }

        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand {
            get => _CloseCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(false);
            });
        }

        private RelayCommand _CallCustomer;
        public RelayCommand CallCustomer {
            get => _CallCustomer ??= new RelayCommand(async obj => {
                if (obj != null)
                    if (!string.IsNullOrEmpty(obj.ToString())) {
                        Analytics.TrackEvent("Звонок клиенту",
                            new Dictionary<string, string> {
                        {"PhoneNumber",obj.ToString() }
                            });
                        Uri uri = new Uri("tel:" + obj);
                        await Launcher.OpenAsync(uri);
                    }
                    else
                        //Toast.MakeText(Android.App.Application.Context,"Номер телефона не указан",ToastLength.Long).Show(); 
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Номер телефона не указан или не корректный", Color.Red, LayoutOptions.EndAndExpand), 4000));
                else
                    //Toast.MakeText(Android.App.Application.Context,"Номер телефона не указан",ToastLength.Long).Show();
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Номер телефона не указан или не корректный", Color.Red, LayoutOptions.EndAndExpand), 4000));
            });
        }

        private RelayCommand _GetCustomers;
        public RelayCommand GetCustomers {
            get => _GetCustomers ??= new RelayCommand(async obj => {
                OpacityForm = 0.1;
                IndicatorVisible = true;
                Analytics.TrackEvent("Получение списка ответственных лиц по объекту");
                List<ObjCust> custs = new List<ObjCust>();
                int? number = null;
                if (ServiceOrder != null)
                    if (ServiceOrder.NewNumber.HasValue)
                        number = ServiceOrder.NewNumber;
                if (ServiceOrderFireAlarm != null)
                    if (ServiceOrderFireAlarm.NewNumber.HasValue)
                        number = ServiceOrderFireAlarm.NewNumber;
                if (number == null)
                    return;
                CutomersCollection = await ClientHttp.Get<ObservableCollection<ObjCust>>("/api/Andromeda/Customer?ObjectNumber=" + number);

                //HttpResponseMessage response = null;
                //using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                //    if(ServiceOrder != null) {
                //        response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/Customer?ObjectNumber=" + ServiceOrder.NewNumber);
                //    }
                //    else if(ServiceOrderFireAlarm != null) {
                //        response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/Customer?ObjectNumber=" + ServiceOrderFireAlarm.NewNumber);
                //    }
                //}
                //if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                //    var resp = response.Content.ReadAsStringAsync().Result;
                //    try {
                //        Analytics.TrackEvent("Попытка десериализации ответа от сервера с ответсвенными лицами");
                //        custs = JsonConvert.DeserializeObject<List<ObjCust>>(resp);
                //    }
                //    catch(Exception ex) {
                //        Crashes.TrackError(new Exception("Ошибка получения списка ответственных лиц по объекту"),
                //        new Dictionary<string,string> {
                //        {"ServiceOrderId",ServiceOrder.NewServiceorderId.ToString() },
                //        {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                //        {"ErrorMessage",ex.Message },
                //        {"StatusCode",response.StatusCode.ToString() },
                //        {"Response",response.ToString() }
                //        });
                //    }
                //}
                //if(custs != null)
                //    foreach(var item in custs) {
                //        CutomersCollection.Add(item);
                //    }
                OpacityForm = 1;
                IndicatorVisible = false;
            });
        }
    }
}
