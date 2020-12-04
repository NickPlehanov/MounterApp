//using Android.Content.Res;
using Android.Hardware;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class MainPageViewModel : BaseViewModel {
        public MainPageViewModel() {
            IndicatorVisible = false;
            OpacityForm = 1;
            PhoneNumber = Application.Current.Properties["Phone"] as string;
        }
        private RelayCommand _AuthCommand;
        public RelayCommand AuthCommand {
            get => _AuthCommand ??= new RelayCommand(async obj => {
                IndicatorVisible = true;
                OpacityForm = 0.1;
                using HttpClient client = new HttpClient();
                string Phone = null;
                if(PhoneNumber.Length == 11) {
                    Phone = PhoneNumber.Substring(1,PhoneNumber.Length - 1);
                }
                try {
                    //Phone = Application.Current.Properties["Phone"].ToString();
                    Application.Current.Properties["Phone"] = PhoneNumber;
                    await Application.Current.SavePropertiesAsync();
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewMounterExtensionBases/phone?phone=" + Phone);
                    var resp = response.Content.ReadAsStringAsync().Result;
                    List<NewMounterExtensionBase> mounters = JsonConvert.DeserializeObject<List<NewMounterExtensionBase>>(resp).Where(x => x.NewIsWorking == true).ToList();
                    response = await client.GetAsync(Resources.BaseAddress + "/api/NewServicemanExtensionBases/phone?phone=" + Phone);
                    resp = response.Content.ReadAsStringAsync().Result;
                    List<NewServicemanExtensionBase> servicemans = JsonConvert.DeserializeObject<List<NewServicemanExtensionBase>>(resp).Where(x => x.NewIswork == true).ToList();
                    if(mounters != null || servicemans != null) {
                        if(mounters.Count > 0 || servicemans.Count > 0) {
                            if(mounters.Count > 1 || servicemans.Count > 1)
                                Message = "Неоднозначно определен сотрудник";
                            else {
                                IndicatorVisible = false;
                                MainMenuPageViewModel vm = new MainMenuPageViewModel(mounters,servicemans);
                                App.Current.MainPage = new MainMenuPage(vm);
                            }
                        }
                        else {
                            Message = "Проверьте правильность ввода номера телефона";
                        }
                    }
                }
                catch (Exception ex) {
                    Message = "Нет подключения к интернету или сетевой адрес недоступен";
                }
                IndicatorVisible = false;
                OpacityForm = 1;
            });
        }
        private bool _IndicatorVisible;
        public bool IndicatorVisible {
            get => _IndicatorVisible;
            set {
                _IndicatorVisible = value;
                OnPropertyChanged(nameof(IndicatorVisible));
            }
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
        private double _OpacityForm;
        public double OpacityForm {
            get => _OpacityForm;
            set {
                _OpacityForm = value;
                OnPropertyChanged(nameof(OpacityForm));
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
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                // vm = new MountsViewModel(Mounters);
                //App.Current.MainPage = new MountsPage(vm);
                System.Environment.Exit(0);
            });
        }
    }
}
