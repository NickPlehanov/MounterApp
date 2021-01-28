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
using System.Linq;
using System.Net;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class SettingsPageViewModel : BaseViewModel {
        //readonly ClientHttp http = new ClientHttp();
        public SettingsPageViewModel(List<NewMounterExtensionBase> mounters,List<NewServicemanExtensionBase> servicemans) {
            Mounters = mounters;
            Servicemans = servicemans;
            //bool.TryParse(Application.Current.Properties["AutoEnter"].ToString(),out bool tmp
            if(Application.Current.Properties.ContainsKey("AutoEnter")) {
                if(bool.TryParse(Application.Current.Properties["AutoEnter"].ToString(),out bool tmp))
                    AutoEnter = tmp;
                else
                    AutoEnter = false;
            }
            if(Application.Current.Properties.ContainsKey("Quality"))
                Quality = int.Parse(Application.Current.Properties["Quality"].ToString());
            if(Application.Current.Properties.ContainsKey("Compression"))
                Compression = int.Parse(Application.Current.Properties["Compression"].ToString());
            SaveImage = IconName("save");
            ClearImage = IconName("clear");
            HelpImage = IconName("help");
            Analytics.TrackEvent("Инициализация окна настроек приложения");
            AppVersions av = new AppVersions();
            Version = null;
            Version = "Версия приложения: "+av.GetVersionAndBuildNumber().VersionNumber;
            BuildNumber = "Сборка приложения: "+av.GetVersionAndBuildNumber().BuildNumber;
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
            ReportImage = IconName("report");
        }
        private ImageSource _ReportImage;
        public ImageSource ReportImage {
            get => _ReportImage;
            set {
                _ReportImage = value;
                OnPropertyChanged(nameof(ReportImage));
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

        private string _Version;
        public string Version {
            get => _Version;
            set {
                _Version = value;
                OnPropertyChanged(nameof(Version));
            }
        }

        private string _BuildNumber;
        public string BuildNumber {
            get => _BuildNumber;
            set {
                _BuildNumber = value;
                OnPropertyChanged(nameof(BuildNumber));
            }
        }

        private ImageSource _ClearImage;
        public ImageSource ClearImage {
            get => _ClearImage;
            set {
                _ClearImage = value;
                OnPropertyChanged(nameof(ClearImage));
            }
        }


        private ImageSource _HelpImage;
        public ImageSource HelpImage {
            get => _HelpImage;
            set {
                _HelpImage = value;
                OnPropertyChanged(nameof(HelpImage));
            }
        }
        /// <summary>
        /// Команда открытия popup-окна для записи обращения в ИТ
        /// </summary>
        private RelayCommand _OpenOrdersForItPageCommand;
        public RelayCommand OpenOrdersForItPageCommand {
            get => _OpenOrdersForItPageCommand ??= new RelayCommand(async obj => {
                OrdersForITViewModel vm = new OrdersForITViewModel(Mounters,Servicemans);
                await App.Current.MainPage.Navigation.PushPopupAsync(new OrdersForITPopupPage(vm));
            });
        }

        private RelayCommand _CheckAccessToSecret;
        public RelayCommand CheckAccessToSecret {
            get => _CheckAccessToSecret ??= new RelayCommand(async obj => {
                HttpStatusCode code = await ClientHttp.GetQuery("/api/Common/AccessSecret?phone=" + Application.Current.Properties["Phone"].ToString());
                if (code.Equals(HttpStatusCode.OK))
                    GetEventsObjectInfo.Execute(null);
                //using(HttpClient client = new HttpClient(GetHttpClientHandler())) {                    
                //    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/AccessSecret?phone=" + Application.Current.Properties["Phone"].ToString());
                //    if(response != null) {
                //        if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                //            GetEventsObjectInfo.Execute(null);
                //        }
                //        //if(response.StatusCode.Equals(System.Net.HttpStatusCode.MethodNotAllowed)) {
                //        //    await Application.Current.MainPage.DisplayAlert("Информация"
                //        //        ,"У Вас установлена не актуальная версия приложения, пожалуйста обновите её." + Environment.NewLine + Environment.NewLine + "Если Вы не получили ссылку на новую версию, то сообщите свою почту в ИТ-отдел."
                //        //        ,"OK");
                //        //}
                //    }
                //}
            });
        }
        private RelayCommand _HelpCommand;
        public RelayCommand HelpCommand {
            get => _HelpCommand ??= new RelayCommand(async obj => {
                string msg = " - Автоматический вход, позволяет автоматически заходить на главную страницу, с последним введенным номером" + Environment.NewLine + Environment.NewLine +
                " - Качество фото - от 0 до 100 - повышает или понижает качество отправляемых фото при монтаже" + Environment.NewLine + Environment.NewLine +
                " - Очистить локальную базу монтажей - удаляет из внутренней базы данных историю монтажей" + Environment.NewLine + Environment.NewLine +
                " - Оставить отзыв - позволяет написать в ИТ-отдел пожелание или заявку";
                HelpPopupViewModel vm = new HelpPopupViewModel(msg);
                await App.Current.MainPage.Navigation.PushPopupAsync(new HelpPopupPage(vm));
            });
        }

        private RelayCommand _ClearDatabaseCommand;
        public RelayCommand ClearDatabaseCommand {
            get => _ClearDatabaseCommand ??= new RelayCommand(async obj => {
                bool result = await Application.Current.MainPage.DisplayAlert("Удаление","Вы действительно хотите очистить базу данных?","Да","Нет");
                if(result) {
                    Toast.MakeText(Android.App.Application.Context,"Очищено объектов: " + App.Database.ClearDatabase().ToString(),ToastLength.Long).Show();
                    Analytics.TrackEvent("Очистка локальной базы данных объектов");
                }
                ClearDatabaseCommand.ChangeCanExecute();
            },obj=>App.Database.GetCount()>0);
        }
        private int _Quality;
        public int Quality {
            get => _Quality;
            set {
                _Quality = value;
                OnPropertyChanged(nameof(Quality));
            }
        }

        private int _Compression;
        public int Compression {
            get => _Compression;
            set {
                _Compression = value;
                OnPropertyChanged(nameof(Compression));
            }
        }

        private bool _AutoEnter;
        public bool AutoEnter {
            get => _AutoEnter;
            set {
                _AutoEnter = value;
                OnPropertyChanged(nameof(AutoEnter));
            }
        }

        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                MainMenuPageViewModel vm = new MainMenuPageViewModel(Mounters,Servicemans);
                App.Current.MainPage = new MainMenuPage(vm);
            });
        }

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand {
            get => _SaveCommand ??= new RelayCommand(async obj => {
                Application.Current.Properties["AutoEnter"] = AutoEnter;
                Application.Current.Properties["Quality"] = Quality;
                Application.Current.Properties["Compression"] = Compression;
                await Application.Current.SavePropertiesAsync();
                Toast.MakeText(Android.App.Application.Context,"Настройки успешно сохранены",ToastLength.Long).Show();
                Dictionary<string,string> parameters = new Dictionary<string,string> {
                     { "MounterPhone",Mounters!=null ? Mounters.Count()>0 ? Mounters.First().NewPhone:"":"" },
                     { "ServicemanPhone",Servicemans!=null ? Servicemans.Count()>0 ? Servicemans.First().NewPhone:"":"" },
                     { "AutoEnter",AutoEnter.ToString() },
                     { "Quality",Quality.ToString() },
                     { "Compression",Compression.ToString() },
                     { "Events","Сохранение локальных настроек приложения" }
                };
                Analytics.TrackEvent("Сохранение локальных настроек приложения",parameters);
                BackPressCommand.Execute(null);
            });
        }

        private ImageSource _SaveImage;
        public ImageSource SaveImage {
            get => _SaveImage;
            set {
                _SaveImage = value;
                OnPropertyChanged(nameof(SaveImage));
            }
        }

        private ObservableCollection<string> _ObjectsNumbers;
        public ObservableCollection<string> ObjectsNumbers {
            get => _ObjectsNumbers;
            set {
                _ObjectsNumbers = value;
                OnPropertyChanged(nameof(ObjectsNumbers));
            }
        }

        private RelayCommand _FixWebLinkCommand;
        public RelayCommand FixWebLinkCommand {
            get => _FixWebLinkCommand ??= new RelayCommand(async obj => {
                //using HttpClient client1 = new HttpClient(GetHttpClientHandler());
                ////string obj_number = ServiceOrder != null ? ServiceOrder.NewNumber.ToString() : ServiceOrderFireAlarm.NewNumber.ToString();
                //HttpResponseMessage response = null;
                //string resp = null;
                //List<string> objs = new List<string>();
                //response = await client1.GetAsync(Resources.BaseAddress + "/api/Andromeda/FixWebLink");
                //if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                //    resp = response.Content.ReadAsStringAsync().Result;
                //    try {
                //        objs = JsonConvert.DeserializeObject<List<string>>(resp);
                //    }
                //    catch(Exception ex) {
                //        Crashes.TrackError(new Exception("Ошибка десериализации результата запроса события по объекту(расширенный)"),
                //        new Dictionary<string,string> {
                //                //{"Servicemans",Servicemans.First().NewPhone },
                //                {"ServerResponse",response.Content.ReadAsStringAsync().Result },
                //                {"ErrorMessage",ex.Message },
                //                {"StatusCode",response.StatusCode.ToString() },
                //                {"Response",response.ToString() }
                //        });
                //    }
                //    if(objs.Count > 0) {
                //        string msg = null;
                //        ObjectsNumbers.Clear();
                //        foreach(string item in objs) {
                //            msg += item + Environment.NewLine;
                //            ObjectsNumbers.Add(item);
                //        }
                //        await Application.Current.MainPage.DisplayAlert("Информация",msg,"OK");
                //    }
                //}
                //else {
                //    resp = null;
                //}
            });
        }

        private RelayCommand _GetEventsObjectInfo;
        public RelayCommand GetEventsObjectInfo {
            get => _GetEventsObjectInfo ??= new RelayCommand(async obj => {
                EventsExternalPageViewModel vm = new EventsExternalPageViewModel(Mounters,Servicemans);
                App.Current.MainPage = new EventsExternalPage(vm);
            });
        }
    }
}
