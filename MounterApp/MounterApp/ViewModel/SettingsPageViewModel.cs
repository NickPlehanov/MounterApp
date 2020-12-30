using Android.Widget;
using Microsoft.AppCenter.Analytics;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class SettingsPageViewModel : BaseViewModel {
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
            Analytics.TrackEvent("Инициализация окна настроек приложения");
            AppVersions av = new AppVersions();
            Version = null;
            Version = "Версия приложения: "+av.GetVersionAndBuildNumber().VersionNumber;
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
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

        private ImageSource _ClearImage;
        public ImageSource ClearImage {
            get => _ClearImage;
            set {
                _ClearImage = value;
                OnPropertyChanged(nameof(ClearImage));
            }
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
    }
}
