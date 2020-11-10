using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Views;
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
				//if(Mounters.Count > 0) {
				//	string Phone = Mounters.FirstOrDefault().NewPhone;
				//	if(!string.IsNullOrEmpty(Phone)) {
				//		Message = Phone;
				//		//api/NewMountWorksExtensionBases/phone?phone=8919-120-7433
				//		using HttpClient client = new HttpClient();
				//		//HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewMountWorksExtensionBases/phone?phone=Phone&date=09.11.2020");
				//		HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewMountWorksExtensionBases/phone?phone=8919-120-7433&date=09.11.2020");
				//		var resp = response.Content.ReadAsStringAsync().Result;
				//		List<MountworksTimeInfo> mountworks = JsonConvert.DeserializeObject<List<MountworksTimeInfo>>(resp);
				//	}
				//	else
				//		Message = "Номер телефона не может быть пустым";
				//}

				MountsViewModel vm = new MountsViewModel(Mounters);
				App.Current.MainPage = new MountsPage(vm);
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
