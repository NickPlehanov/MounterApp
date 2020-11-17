//using Android.Content.Res;
using Android.Hardware;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    class MainPageViewModel : BaseViewModel {
        private RelayCommand _AuthCommand;
        public RelayCommand AuthCommand {
            get => _AuthCommand ??= new RelayCommand(async obj => {
                //ProgressBarVisible = true;
                ProgressValue = 0;
                using HttpClient client = new HttpClient();
                string Phone = null;
                if(PhoneNumber.Length == 11) {
                    Phone = PhoneNumber.Substring(1,PhoneNumber.Length - 1);
                }
                ProgressValue = 0.2;
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewMounterExtensionBases/phone?phone=" + Phone);
                var resp = response.Content.ReadAsStringAsync().Result;
                List<NewMounterExtensionBase> mounters = JsonConvert.DeserializeObject<List<NewMounterExtensionBase>>(resp).Where(x=>x.NewIsWorking==true).ToList();
                ProgressValue = 0.4;
                response = await client.GetAsync(Resources.BaseAddress + "/api/NewServicemanExtensionBases/phone?phone=" + Phone);
                resp = response.Content.ReadAsStringAsync().Result;
                List<NewServicemanExtensionBase> servicemans = JsonConvert.DeserializeObject<List<NewServicemanExtensionBase>>(resp).Where(x => x.NewIswork == true).ToList();
                ProgressValue = 0.8;
                if(mounters != null || servicemans != null) {
                    if(mounters.Count > 0 || servicemans.Count > 0) {
                        if(mounters.Count > 1 || servicemans.Count > 1) {
                            ProgressValue = 1;
                            //ProgressBarVisible = false;
                            Message = "Неоднозначно определен сотрудник";
                        }
                        else {
                            ProgressValue = 1;
                            //ProgressBarVisible = false;
                            MainMenuPageViewModel vm = new MainMenuPageViewModel(mounters,servicemans);
                            App.Current.MainPage = new MainMenuPage(vm);
                        }
                    }
                    else {
                        Message = "Проверьте правильность ввода номера телефона";
                        ProgressValue = 1;
                        //ProgressBarVisible = false;
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
        private bool _ProgressBarVisible;
        public bool ProgressBarVisible {
            get => _ProgressBarVisible;
            set {
                _ProgressBarVisible = value;
                OnPropertyChanged(nameof(ProgressBarVisible));
            }
        }
        private double _ProgressValue;
        public double ProgressValue {
            get => _ProgressValue;
            set {
                _ProgressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
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
