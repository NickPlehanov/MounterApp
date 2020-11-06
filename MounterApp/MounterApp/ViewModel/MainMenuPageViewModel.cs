using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MounterApp.ViewModel {
    public class MainMenuPageViewModel: BaseViewModel {
		private ObservableCollection<MountWorksModel> _MountWorks;
		public ObservableCollection<MountWorksModel> MountWorks {
			get => _MountWorks;
			set {
				_MountWorks = value;
				OnPropertyChanged(nameof(MountWorks));
			}
		}
        public MainMenuPageViewModel(List<NewMounterExtensionBase> mounters, List<NewServicemanExtensionBase> servicemans) {
			Mounters = mounters;
			Serviceman = servicemans;
        }
        public MainMenuPageViewModel() {

        }

        private RelayCommand _GetMountworksCommand;
		public RelayCommand GetMountworksCommand {
			get => _GetMountworksCommand ??= new RelayCommand(async obj => { 
				if(Mounters.Count > 0) {
					
                }
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
	}
}
