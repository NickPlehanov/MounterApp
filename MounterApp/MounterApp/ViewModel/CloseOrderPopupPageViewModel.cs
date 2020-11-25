using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Properties;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class CloseOrderPopupPageViewModel : BaseViewModel {
        public CloseOrderPopupPageViewModel(NewServiceorderExtensionBase soeb,List<NewServicemanExtensionBase> _servicemans) {
            so = soeb;
            Servicemans = _servicemans;
            GetResults.Execute(null);
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
        private bool _ReasonVisibility;
        public bool ReasonVisibility {
            get => _ReasonVisibility;
            set {
                _ReasonVisibility = value;
                OnPropertyChanged(nameof(ReasonVisibility));
            }
        }
        private MetadataModel _SelectedReason;
        public MetadataModel SelectedReason {
            get => _SelectedReason;
            set {
                _SelectedReason = value;
                OnPropertyChanged(nameof(SelectedReason));
            }
        }
        private MetadataModel _SelectedResult;
        public MetadataModel SelectedResult {
            get => _SelectedResult;
            set {
                _SelectedResult = value;
                OnPropertyChanged(nameof(SelectedResult));
                if(SelectedResult.Value != 1) {
                    ReasonVisibility = true;
                    GetReasons.Execute(null);
                }
                else
                    ReasonVisibility = false;
                if(SelectedResult.Value == 2) {
                    TransferLayoutVisibility = true;
                }
                if(SelectedResult.Value == 3) {
                    TransferLayoutVisibility = false;
                }
            }
        }
        private ObservableCollection<MetadataModel> _Results = new ObservableCollection<MetadataModel>();
        public ObservableCollection<MetadataModel> Results {
            get => _Results;
            set {
                _Results = value;
                OnPropertyChanged(nameof(Results));
            }
        }
        private ObservableCollection<MetadataModel> _Reasons = new ObservableCollection<MetadataModel>();
        public ObservableCollection<MetadataModel> Reasons {
            get => _Reasons;
            set {
                _Reasons = value;
                OnPropertyChanged(nameof(_Reasons));
            }
        }
        private string _ReasonComment;
        public string ReasonComment {
            get => _ReasonComment;
            set {
                _ReasonComment = value;
                OnPropertyChanged(nameof(ReasonComment));
            }
        }
        private RelayCommand _GetReasons;
        public RelayCommand GetReasons {
            get => _GetReasons ??= new RelayCommand(async obj => {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/metadata?ColumnName=New_resultid&ObjectName=New_serviceorder");
                var resp = response.Content.ReadAsStringAsync().Result;
                List<MetadataModel> mm = new List<MetadataModel>();
                try {
                    mm = JsonConvert.DeserializeObject<List<MetadataModel>>(resp);
                }
                catch {
                    mm = null;
                }
                if(mm != null) {
                    foreach(MetadataModel item in mm)
                        Reasons.Add(item);
                }
            });
        }
        private RelayCommand _GetResults;
        public RelayCommand GetResults {
            get => _GetResults ??= new RelayCommand(async obj => {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/metadata?ColumnName=New_result&ObjectName=New_serviceorder");
                var resp = response.Content.ReadAsStringAsync().Result;
                List<MetadataModel> mm = new List<MetadataModel>();
                try {
                    mm = JsonConvert.DeserializeObject<List<MetadataModel>>(resp);
                }
                catch {
                    mm = null;
                }
                if(mm != null) {
                    foreach(MetadataModel item in mm)
                        Results.Add(item);
                }
            });
        }
        private bool _IsTransfer;
        public bool IsTransfer {
            get => _IsTransfer;
            set {
                _IsTransfer = value;
                OnPropertyChanged(nameof(IsTransfer));
            }
        }
        private bool _IsCancel;
        public bool IsCancel {
            get => _IsCancel;
            set {
                _IsCancel = value;
                OnPropertyChanged(nameof(IsCancel));
            }
        }
        private DateTime _TransferDate;
        public DateTime TransferDate {
            get => _TransferDate;
            set {
                if(value == DateTime.Parse("01.01.2010 00:00:00"))
                    _TransferDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy")).AddDays(1);
                else
                    _TransferDate = value;
                OnPropertyChanged(nameof(TransferDate));
            }
        }
        private TimeSpan _TransferTime;
        public TimeSpan TransferTime {
            get => _TransferTime;
            set {
                _TransferTime = value;
                OnPropertyChanged(nameof(TransferTime));
            }
        }
        private bool _TransferLayoutVisibility;
        public bool TransferLayoutVisibility {
            get => _TransferLayoutVisibility;
            set {
                _TransferLayoutVisibility = value;
                OnPropertyChanged(nameof(TransferLayoutVisibility));
            }
        }
        //выполнено
        //private RelayCommand _ServiceOrderCompletedCommand;
        //public RelayCommand ServiceOrderCompletedCommand {
        //    get => _ServiceOrderCompletedCommand ??= new RelayCommand(async obj => {
        //        using HttpClient client = new HttpClient();
        //        HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases/id?id=" + so.NewServiceorderId);
        //        var resp = response.Content.ReadAsStringAsync().Result;
        //        NewServiceorderExtensionBase soeb = null;
        //        try {
        //            soeb = JsonConvert.DeserializeObject<NewServiceorderExtensionBase>(resp);
        //        }
        //        catch {
        //            soeb = null;
        //        }
        //        if(soeb != null) {
        //            soeb.NewOutgone = DateTime.Now.AddHours(-5);
        //            soeb.NewNewServiceman = Servicemans.FirstOrDefault().NewServicemanId;
        //            soeb.NewResult = 1;
        //            using(HttpClient clientPut = new HttpClient()) {
        //                var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
        //                //form.Add(new StreamContent(ph.File.GetStream()),String.Format("file"),String.Format(ObjectNumber + "_" + ph._Types.PhotoTypeName + ".jpeg"));
        //                HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases",httpContent);
        //            }
        //        }
        //        //TODO: перейти к странице с общим списком заявок
        //        await App.Current.MainPage.Navigation.PopPopupAsync();
        //    });
        //}
        //private RelayCommand _CloseCommand;
        //public RelayCommand CloseCommand {
        //    get => _CloseCommand ??= new RelayCommand(async obj => {
        //        await App.Current.MainPage.Navigation.PopPopupAsync();
        //    });
        //}
        //Перенос
        //private RelayCommand _ServiceOrderTransferCommand;
        //public RelayCommand ServiceOrderTransferCommand {
        //    get => _ServiceOrderTransferCommand ??= new RelayCommand(async obj => {
        //        ReasonVisibility = true;
        //        IsTransfer = true;
        //        IsCancel = false;
        //    });
        //}
        ////Отмена
        //private RelayCommand _ServiceOrderCancelCommand;
        //public RelayCommand ServiceOrderCancelCommand {
        //    get => _ServiceOrderCancelCommand ??= new RelayCommand(async obj => {
        //        ReasonVisibility = true;
        //        IsTransfer = false;
        //        IsCancel = true;
        //    });
        //}
        private RelayCommand _CloseServiceOrderCommand;
        public RelayCommand CloseServiceOrderCommand {
            get => _CloseServiceOrderCommand ??= new RelayCommand(async obj => {
                if(SelectedReason != null && !string.IsNullOrEmpty(ReasonComment)) {
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
                        soeb.NewResult = SelectedResult.Value;
                        soeb.NewResultId = SelectedReason.Value;
                        soeb.NewTransferReason = ReasonComment;
                        if(SelectedResult.Value == 2) {
                            soeb.NewMoved = new DateTime(TransferDate.Year,TransferDate.Month,TransferDate.Day,TransferTime.Hours,TransferTime.Minutes,TransferTime.Seconds);
                        }
                        using(HttpClient clientPut = new HttpClient()) {
                            var httpContent = new StringContent(JsonConvert.SerializeObject(soeb),Encoding.UTF8,"application/json");
                            //form.Add(new StreamContent(ph.File.GetStream()),String.Format("file"),String.Format(ObjectNumber + "_" + ph._Types.PhotoTypeName + ".jpeg"));
                            HttpResponseMessage responsePut = await clientPut.PutAsync(Resources.BaseAddress + "/api/NewServiceorderExtensionBases",httpContent);
                        }
                    }
                    await App.Current.MainPage.Navigation.PopPopupAsync(true);
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Ошибка","Не выбрана причина отмены(переноса) или не указан комментарий к причине","OK");
            });
        }
    }
}
