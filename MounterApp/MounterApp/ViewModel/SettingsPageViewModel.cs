using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class SettingsPageViewModel : BaseViewModel {
        public SettingsPageViewModel(List<NewMounterExtensionBase> mounters, List<NewServicemanExtensionBase> servicemans) {
            Mounters = mounters;
            Servicemans = servicemans;
            if (Application.Current.Properties.ContainsKey("AutoEnter")) {
                if (bool.TryParse(Application.Current.Properties["AutoEnter"].ToString(), out bool tmp)) {
                    AutoEnter = tmp;
                }
                else
                    AutoEnter = false;
            }
            if (Application.Current.Properties.ContainsKey("Quality"))
                Quality = int.Parse(Application.Current.Properties["Quality"].ToString());


            if (Application.Current.Properties.ContainsKey("TimeToPush"))
                TimeToPush = int.Parse(Application.Current.Properties["TimeToPush"].ToString());


            if (Application.Current.Properties.ContainsKey("AutoUpdateTime"))
                AutoUpdateTime = double.TryParse(Application.Current.Properties["AutoUpdateTime"].ToString(), out _) ? double.Parse(Application.Current.Properties["AutoUpdateTime"].ToString()) : 0;

            else {
                AutoUpdateTime = 0;
                Application.Current.SavePropertiesAsync();
            }
            SaveImage = IconName("save");
            ClearImage = IconName("clear");
            HelpImage = IconName("help");
            ReportImage = IconName("report");
            GetImage = IconName("get");
            AppVersions av = new AppVersions();
            Version = null;
            Version = "Версия приложения: " + av.GetVersionAndBuildNumber().VersionNumber;
            BuildNumber = "Сборка приложения: " + av.GetVersionAndBuildNumber().BuildNumber;
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
            IsChanged = false;
        }
        private ImageSource _ReportImage;
        public ImageSource ReportImage {
            get => _ReportImage;
            set {
                _ReportImage = value;
                OnPropertyChanged(nameof(ReportImage));
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

        private double? _AutoUpdateTime;
        public double? AutoUpdateTime {
            get => _AutoUpdateTime;
            set {
                if (_AutoUpdateTime != value) {
                    IsChanged = true;
                }

                _AutoUpdateTime = value;
                OnPropertyChanged(nameof(AutoUpdateTime));
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
        private RelayCommand _OpenOrdersForItPageCommand;
        public RelayCommand OpenOrdersForItPageCommand {
            get => _OpenOrdersForItPageCommand ??= new RelayCommand(async obj => {
                OrdersForITViewModel vm = new OrdersForITViewModel(Mounters, Servicemans);
                await App.Current.MainPage.Navigation.PushPopupAsync(new OrdersForITPopupPage(vm));
            });
        }

        private RelayCommand _CheckAccessToSecret;
        public RelayCommand CheckAccessToSecret {
            get => _CheckAccessToSecret ??= new RelayCommand(async obj => {
                HttpStatusCode code = await ClientHttp.Get("/api/Common/AccessSecret?phone=" + Application.Current.Properties["Phone"].ToString());
                if (code.Equals(HttpStatusCode.OK)) {
                    GetEventsObjectInfo.Execute(null);
                }
            });
        }
        private RelayCommand _HelpCommand;
        public RelayCommand HelpCommand {
            get => _HelpCommand ??= new RelayCommand(async obj => {
                string msg = " - Автоматический вход, позволяет автоматически заходить на главную страницу, с последним введенным номером" + Environment.NewLine + Environment.NewLine +
                " - Качество фото - от 0 до 100 - повышает или понижает качество отправляемых фото при монтаже" + Environment.NewLine + Environment.NewLine +
                " - Время автоматического обновления заявок - от 0 до 60 - определяет переодичность обновления списка заявок. Внимание данный пункт меню определяет, работу PUSH-уведомлений" + Environment.NewLine + Environment.NewLine +
                " - Время уведомления для повременных заявок - от 0 до 60 - определяет период, за который будут отправляться PUSH - уведомления о приближении времени повременной заявки или истечении крайней границы" + Environment.NewLine + Environment.NewLine +
                " - Очистить локальную базу монтажей - удаляет из внутренней базы данных историю монтажей" + Environment.NewLine + Environment.NewLine +
                " - Оставить отзыв - позволяет написать в ИТ-отдел пожелание или заявку" + Environment.NewLine + Environment.NewLine +
                " - Скачать - позволяет скачать актуальную(или предыдущую) версию приложения" + Environment.NewLine + Environment.NewLine;
                HelpPopupViewModel vm = new HelpPopupViewModel(msg);
                await App.Current.MainPage.Navigation.PushPopupAsync(new HelpPopupPage(vm));
            });
        }

        private RelayCommand _ClearDatabaseCommand;
        public RelayCommand ClearDatabaseCommand {
            get => _ClearDatabaseCommand ??= new RelayCommand(async obj => {
                bool result = await Application.Current.MainPage.DisplayAlert("Удаление", "Вы действительно хотите очистить базу данных?", "Да", "Нет");
                if (result) {
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Очищено объектов: " + App.Database.ClearDatabase().ToString(), Color.Green, LayoutOptions.EndAndExpand), 4000));
                }
                ClearDatabaseCommand.ChangeCanExecute();
            }, obj => App.Database.GetCount() > 0);
        }
        private int _Quality;
        public int Quality {
            get => _Quality;
            set {
                if (_Quality != value) {
                    IsChanged = true;
                }

                _Quality = value;
                OnPropertyChanged(nameof(Quality));
            }
        }

        private int _TimeToPush;
        public int TimeToPush {
            get => _TimeToPush;
            set {
                _TimeToPush = value;
                OnPropertyChanged(nameof(TimeToPush));
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
                if (_AutoEnter != value) {
                    IsChanged = true;
                }

                _AutoEnter = value;
                OnPropertyChanged(nameof(AutoEnter));
            }
        }

        private bool _IsChanged;
        public bool IsChanged {
            get => _IsChanged;
            set {
                _IsChanged = value;
                OnPropertyChanged(nameof(IsChanged));
            }
        }

        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                if (IsChanged) {
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание", "Внесены изменения в настройки, Сохранить?", "Да", "Нет");
                    if (result) {
                        SaveCommand.Execute(null);
                    }

                }
                App.Current.MainPage = new MainMenuPage(new MainMenuPageViewModel(Mounters, Servicemans));
            });
        }

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand {
            get => _SaveCommand ??= new RelayCommand(async obj => {
                Application.Current.Properties["AutoEnter"] = AutoEnter;
                Application.Current.Properties["Quality"] = Quality;
                Application.Current.Properties["AutoUpdateTime"] = AutoUpdateTime;
                Application.Current.Properties["TimeToPush"] = TimeToPush;
                await Application.Current.SavePropertiesAsync();
                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Настройки успешно сохранены", Color.Green, LayoutOptions.EndAndExpand), 4000));
                Dictionary<string, string> parameters = new Dictionary<string, string> {
                     { "MounterPhone",Mounters!=null ? Mounters.Count()>0 ? Mounters.First().NewPhone:"":"" },
                     { "ServicemanPhone",Servicemans!=null ? Servicemans.Count()>0 ? Servicemans.First().NewPhone:"":"" },
                     { "AutoEnter",AutoEnter.ToString() },
                     { "Quality",Quality.ToString() },
                     { "Compression",Compression.ToString() },
                     { "Events","Сохранение локальных настроек приложения" }
                };
                IsChanged = false;
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
            });
        }

        private RelayCommand _GetEventsObjectInfo;
        public RelayCommand GetEventsObjectInfo {
            get => _GetEventsObjectInfo ??= new RelayCommand(async obj => {
                EventsExternalPageViewModel vm = new EventsExternalPageViewModel(Mounters, Servicemans);
                App.Current.MainPage = new EventsExternalPage(vm);
            });
        }

        private RelayCommand _DownloadAppCommand;
        public RelayCommand DownloadAppCommand {
            get => _DownloadAppCommand ??= new RelayCommand(async obj => {
                var link = await ClientHttp.GetString("/api/Common/GetAppDownload");
                if (string.IsNullOrEmpty(link)) {
                    return;
                }

                await Browser.OpenAsync(new Uri(link), BrowserLaunchMode.SystemPreferred);
            });
        }
    }
}
