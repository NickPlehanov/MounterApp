using MounterApp.Helpers;
using MounterApp.InternalModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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

		private RelayCommand _GetMountworksCommand;
		public RelayCommand GetMountworksCommand {
			get => _GetMountworksCommand ??= new RelayCommand(async obj => { 
				Mount
			});
		}
	}
}
