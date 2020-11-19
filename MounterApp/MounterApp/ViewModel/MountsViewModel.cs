using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MounterApp.ViewModel {
    public class MountsViewModel : BaseViewModel {
        private ObservableCollection<Mounts> _Mounts = new ObservableCollection<Mounts>();
        public ObservableCollection<Mounts> Mounts {
            get => _Mounts;
            set {
                _Mounts = value;
                OnPropertyChanged(nameof(Mounts));
            }
        }

        private ObservableCollection<Mounts> _NotSendedMounts = new ObservableCollection<Mounts>();
        public ObservableCollection<Mounts> NotSendedMounts {
            get => _NotSendedMounts;
            set {
                _NotSendedMounts = value;
                OnPropertyChanged(nameof(NotSendedMounts));
            }
        }
        private Mounts _NotSendedMount;
        public Mounts NotSendedMount {
            get => _NotSendedMount;
            set {
                _NotSendedMount = value;
                OnPropertyChanged(nameof(NotSendedMount));
            }
        }
        public MountsViewModel() {

        }
        public MountsViewModel(List<NewMounterExtensionBase> mounters) {
            Mounters = mounters;
            HeaderNotSended = "Неотправленные монтажи (0)";
            var _ntMounts = App.Database.GetMounts(Mounters.FirstOrDefault().NewMounterId).Where(x => x.State == 0).ToList();
            if(_ntMounts != null)
                if(_ntMounts.Any()) {
                    foreach(var item in _ntMounts)
                        NotSendedMounts.Add(item);
                    HeaderNotSended = "Неотправленные монтажи (" + _ntMounts.Count.ToString() + ")";
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

        private RelayCommand _NewMountCommand;
        public RelayCommand NewMountCommand {
            get => _NewMountCommand ??= new RelayCommand(async obj => {
                if(Mounters.Count > 0) {
                    //NewMountPageViewModel vm = new NewMountPageViewModel();
                    NewMountPageViewModel vm = new NewMountPageViewModel(Mounters);
                    App.Current.MainPage = new NewMountpage(vm);
                }
            });
        }
        private RelayCommand _SelectMountCommand;
        public RelayCommand SelectMountCommand {
            get => _SelectMountCommand ??= new RelayCommand(async obj => {
                if(NotSendedMount != null) {
                    NewMountPageViewModel vm = new NewMountPageViewModel(NotSendedMount,Mounters);
                    App.Current.MainPage = new NewMountpage(vm);
                }
            });
        }

        private string _HeaderNotSended;
        public string HeaderNotSended {
            get => _HeaderNotSended;
            set {
                _HeaderNotSended = value;
                OnPropertyChanged(nameof(HeaderNotSended));
            }
        }
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                MainMenuPageViewModel vm = new MainMenuPageViewModel(Mounters);
                App.Current.MainPage = new MainMenuPage(vm);
            });
        }
    }
}