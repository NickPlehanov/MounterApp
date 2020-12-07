using Android.Widget;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
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
            SaveImage = "save.png";
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
                MainMenuPageViewModel vm = new MainMenuPageViewModel(Mounters);
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
