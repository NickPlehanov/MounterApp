using Android.Content.Res;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
	class MainPageViewModel : BaseViewModel {
		private RelayCommand _AuthCommand;
		public RelayCommand AuthCommand {
			get => _AuthCommand ??= new RelayCommand(async obj => {
				using HttpClient client = new HttpClient();
				string baseAddress = "https://ec5fa038b958.ngrok.io";
				string Phone = null;
				if(PhoneNumber.Length == 11) {
					Phone = PhoneNumber.Substring(1, PhoneNumber.Length - 1);
				}
				HttpResponseMessage response = await client.GetAsync(baseAddress + "/api/NewMounterExtensionBases/" + Phone);
				var resp = response.Content.ReadAsStringAsync().Result;
				List<NewMounterExtensionBase> mounters = JsonConvert.DeserializeObject<List<NewMounterExtensionBase>>(resp);

				response = await client.GetAsync(baseAddress + "/api/NewServicemanExtensionBases/" + Phone);
				resp = response.Content.ReadAsStringAsync().Result;
				List<NewServicemanExtensionBase> servicemans = JsonConvert.DeserializeObject<List<NewServicemanExtensionBase>>(resp);
				if (mounters!=null || servicemans != null) {
					if (mounters.Count>0 || servicemans.Count > 0) {
						//await PushAsync(new NavigationPage(new MainMenuPage()));
						var _MainMenuPage = new MainMenuPage();
						if(mounters.Count > 0)
							_MainMenuPage.BindingContext = mounters;
						if(servicemans.Count > 0)
							_MainMenuPage.BindingContext = servicemans;
						App.Current.MainPage = new NavigationPage(new MainMenuPage());
					}
				}
			});
		}

		private string _PhoneNumber;
		public string PhoneNumber {
			get => _PhoneNumber;
			set {
				//if(value.Length == 11) {
				//	_PhoneNumber = value.Substring(1, value.Length - 1);
				//}
				_PhoneNumber = value;
				//else
				OnPropertyChanged(nameof(PhoneNumber));
			}
		}

		private string _Message;
		public string Message {
			get => _Message;
			set {
				_Message = value;
				OnPropertyChanged(nameof(Message));
			}
		}
		//private string ProcessResponse(string response) {
		//	string s = response.Replace(@"\", string.Empty);
		//	return s.Trim().Substring(1, (s.Length) - 2);
		//}
	}
}
