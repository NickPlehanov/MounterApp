using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MounterApp.ViewModel {
    public class MainMenuPageViewModel : BaseViewModel {
        private ObservableCollection<MountWorksModel> _MountWorks;
        public ObservableCollection<MountWorksModel> MountWorks {
            get => _MountWorks;
            set {
                _MountWorks = value;
                OnPropertyChanged(nameof(MountWorks));
            }
        }
        public MainMenuPageViewModel(List<NewMounterExtensionBase> mounters,List<NewServicemanExtensionBase> servicemans) {
            Mounters = mounters;
            Serviceman = servicemans;
        }
        public MainMenuPageViewModel(List<NewMounterExtensionBase> mounters) {
            Mounters = mounters;
        }
        public MainMenuPageViewModel(List<NewServicemanExtensionBase> servicemans) {
            Serviceman = servicemans;
        }
        public MainMenuPageViewModel() {

        }

        private string _Message;
        public string Message {
            get => _Message;
            set {
                _Message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private RelayCommand _GetMountworksCommand;
        public RelayCommand GetMountworksCommand {
            get => _GetMountworksCommand ??= new RelayCommand(async obj => {
                MountsViewModel vm = new MountsViewModel(Mounters);
                App.Current.MainPage = new MountsPage(vm);
            });
        }

        private RelayCommand _GetServiceordersCommand;
        public RelayCommand GetServiceordersCommand {
            get => _GetServiceordersCommand ??= new RelayCommand(async obj => {
                ServiceOrdersPageViewModel vm = new ServiceOrdersPageViewModel(Serviceman);
                App.Current.MainPage = new ServiceOrdersPage(vm);
            });
        }
        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
        }
        private List<NewServicemanExtensionBase> _Serviceman;
        public List<NewServicemanExtensionBase> Serviceman {
            get => _Serviceman;
            set {
                _Serviceman = value;
                OnPropertyChanged(nameof(Serviceman));
            }
        }
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                MainPageViewModel vm = new MainPageViewModel();
                App.Current.MainPage = new MainPage(vm);
                //System.Environment.Exit(0);
            });
        }
    }
}
