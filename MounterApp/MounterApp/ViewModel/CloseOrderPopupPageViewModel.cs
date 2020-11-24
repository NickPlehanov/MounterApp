using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace MounterApp.ViewModel {
    public class CloseOrderPopupPageViewModel: BaseViewModel {
        public CloseOrderPopupPageViewModel(NewServiceorderExtensionBase soeb, List<NewServicemanExtensionBase> _servicemans) {
            so = soeb;
            Servicemans = _servicemans;
        }
        private NewServiceorderExtensionBase _so;
        public NewServiceorderExtensionBase so {
            get => _so;
            set {
                _so = value;
                OnPropertyChanged(nameof(so));
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
        //выполнено
        private RelayCommand _ServiceOrderCompletedCommand;
        public RelayCommand ServiceOrderCompletedCommand {
            get => _ServiceOrderCompletedCommand ??= new RelayCommand(async obj => {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/id?id=" + so.NewServiceorderId);
                var resp = response.Content.ReadAsStringAsync().Result;
                NewServiceorderExtensionBase soeb = null;
                try {
                    soeb = JsonConvert.DeserializeObject<NewServiceorderExtensionBase>(resp);
                }
                catch {
                    soeb = null;
                }
                if(soeb != null) {
                    soeb.NewOutgone = DateTime.Now.AddHours(-5);
                    soeb.NewNewServiceman = Servicemans.FirstOrDefault().NewServicemanId;
                    soeb.NewResult = 1;
                    using(HttpClient clientPut = new HttpClient()) {
                        var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
                        //form.Add(new StreamContent(ph.File.GetStream()),String.Format("file"),String.Format(ObjectNumber + "_" + ph._Types.PhotoTypeName + ".jpeg"));
                        HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases",httpContent);
                    }
                }
                //TODO: перейти к странице с общим списком заявок
                await App.Current.MainPage.Navigation.PopPopupAsync();
            });
        }
    }
}
