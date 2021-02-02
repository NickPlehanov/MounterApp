using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class ObjectInfoViewModel : BaseViewModel {
        //readonly ClientHttp http = new ClientHttp();
        public ObjectInfoViewModel(NewServiceorderExtensionBase _so,List<NewServicemanExtensionBase> _servicemans, List<NewMounterExtensionBase> _mounters) {
            Mounters = _mounters;
            Servicemans = _servicemans;
            ServiceOrder = _so;
            GetWires.Execute(null);
            GetExtFields.Execute(null);
            ArrowCircleWires= IconName("arrow_circle_down");
            ArrowCircleExtFields = IconName("arrow_circle_down");
            CloseImage = IconName("close");
            OpacityForm = 1;
            IndicatorVisible = false;
        }
        public ObjectInfoViewModel(NewTest2ExtensionBase _so,List<NewServicemanExtensionBase> _servicemans,List<NewMounterExtensionBase> _mounters) {
            Mounters = _mounters;
            Servicemans = _servicemans;
            ServiceOrderFireAlarm = _so;            
            GetWires.Execute(null);
            GetExtFields.Execute(null);
            ArrowCircleWires = IconName("arrow_circle_down");
            ArrowCircleExtFields = IconName("arrow_circle_down");
            CloseImage = IconName("close");
            OpacityForm = 1;
            IndicatorVisible = false;
        }

        private bool _IndicatorVisible;
        public bool IndicatorVisible {
            get => _IndicatorVisible;
            set {
                _IndicatorVisible = value;
                OnPropertyChanged(nameof(IndicatorVisible));
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
        private NewServiceorderExtensionBase _ServiceOrder;
        public NewServiceorderExtensionBase ServiceOrder {
            get => _ServiceOrder;
            set {
                _ServiceOrder = value;
                OnPropertyChanged(nameof(ServiceOrder));
            }
        }

        private NewTest2ExtensionBase _ServiceOrderFireAlarm;
        public NewTest2ExtensionBase ServiceOrderFireAlarm {
            get => _ServiceOrderFireAlarm;
            set {
                _ServiceOrderFireAlarm = value;
                OnPropertyChanged(nameof(ServiceOrderFireAlarm));
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
        private ObservableCollection<Wires> _WiresCollection = new ObservableCollection<Wires>();
        public ObservableCollection<Wires> WiresCollection {
            get => _WiresCollection;
            set {
                _WiresCollection = value;
                OnPropertyChanged(nameof(WiresCollection));
            }
        }
        private ObservableCollection<ExtFields> _ExtFields = new ObservableCollection<ExtFields>();
        public ObservableCollection<ExtFields> ExtFields {
            get => _ExtFields;
            set {
                _ExtFields = value;
                OnPropertyChanged(nameof(ExtFields));
            }
        }
        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand {
            get => _CloseCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(true);
                //ServiceOrderViewModel vm = new ServiceOrderViewModel(ServiceOrder,Servicemans,Mounters);
                //App.Current.MainPage = new ServiceOrder(vm);
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
        private RelayCommand _GetWires;
        public RelayCommand GetWires {
            get => _GetWires ??= new RelayCommand(async obj => {
                OpacityForm = 0.1;
                IndicatorVisible = true;
                int? number = null;
                if(ServiceOrder.NewNumber.HasValue)
                    number = ServiceOrder.NewNumber;
                else if (ServiceOrderFireAlarm.NewNumber.HasValue)
                    number = ServiceOrderFireAlarm.NewNumber;
                WiresCollection = await ClientHttp.Get<ObservableCollection<Wires>>("/api/Andromeda/wires?objNumber=" + number);


                //WiresCollection.Clear();
                //using HttpClient client = new HttpClient(GetHttpClientHandler());
                //List<Wires> _wrs = new List<Wires>();
                //string resp = "";
                //if(ServiceOrder != null) {
                //    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/wires?objNumber=" + ServiceOrder.NewNumber);
                //    resp = response.Content.ReadAsStringAsync().Result;
                //}
                //else if(ServiceOrderFireAlarm != null) {
                //    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/wires?objNumber=" + ServiceOrderFireAlarm.NewNumber);
                //    resp = response.Content.ReadAsStringAsync().Result;
                //}
                //try {
                //    if(!string.IsNullOrEmpty(resp))
                //        _wrs = JsonConvert.DeserializeObject<List<Wires>>(resp);
                //}
                //catch { }
                //if(_wrs.Count() > 0) {
                //    foreach(var item in _wrs) {
                //        WiresCollection.Add(item);
                //    }
                //}
                OpacityForm = 1;
                IndicatorVisible = false;
            });
        }
        private RelayCommand _GetExtFields;
        public RelayCommand GetExtFields {
            get => _GetExtFields ??= new RelayCommand(async obj => {
                OpacityForm = 0.1;
                IndicatorVisible = true;
                int? number = 63020;
                if(ServiceOrder.NewNumber.HasValue)
                    number = ServiceOrder.NewNumber;
                else if(ServiceOrderFireAlarm.NewNumber.HasValue)
                    number = ServiceOrderFireAlarm.NewNumber;
                ExtFields = await ClientHttp.Get<ObservableCollection<ExtFields>>("/api/Andromeda/ext?objNumber=" + number);

                //ExtFields.Clear();
                //List<ExtFields> _ext = new List<ExtFields>();
                //string resp = "";

                //using HttpClient client = new HttpClient(GetHttpClientHandler());
                //if(ServiceOrder != null) {
                //    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/ext?objNumber=" + ServiceOrder.NewNumber);
                //    resp = response.Content.ReadAsStringAsync().Result;
                //}
                //else if(ServiceOrderFireAlarm != null) {
                //    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/ext?objNumber=" + ServiceOrderFireAlarm.NewNumber);
                //    resp = response.Content.ReadAsStringAsync().Result;
                //}
                //try {
                //    if(!string.IsNullOrEmpty(resp))
                //        _ext = JsonConvert.DeserializeObject<List<ExtFields>>(resp);
                //}
                //catch { }
                //if(_ext.Count() > 0) {
                //    foreach(var item in _ext) {
                //        ExtFields.Add(item);
                //    }
                //}
                OpacityForm = 1;
                IndicatorVisible = false;
            });
        }
        //private RelayCommand _GetWires;
        //public RelayCommand GetWires {
        //    get => _GetWires ??= new RelayCommand(async obj => {
        //        OpacityForm = 0.1;
        //        IndicatorVisible = true;
        //        WiresCollection.Clear();
        //        using HttpClient client = new HttpClient(GetHttpClientHandler());
        //        List<Wires> _wrs = new List<Wires>();
        //        string resp = "";
        //        if(ServiceOrder != null) {
        //            HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/wires?objNumber=" + ServiceOrder.NewNumber);
        //            resp = response.Content.ReadAsStringAsync().Result;
        //        }
        //        else if(ServiceOrderFireAlarm != null) {
        //            HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/wires?objNumber=" + ServiceOrderFireAlarm.NewNumber);
        //            resp = response.Content.ReadAsStringAsync().Result;
        //        }
        //        try {
        //            if (!string.IsNullOrEmpty(resp))
        //                _wrs = JsonConvert.DeserializeObject<List<Wires>>(resp);
        //        }
        //        catch { }
        //        if(_wrs.Count() > 0) {
        //            foreach(var item in _wrs) {
        //                WiresCollection.Add(item);
        //            }
        //        }
        //        OpacityForm = 1;
        //        IndicatorVisible = false;
        //    });
        //}
        //private RelayCommand _GetExtFields;
        //public RelayCommand GetExtFields {
        //    get => _GetExtFields ??= new RelayCommand(async obj => {
        //        OpacityForm = 0.1;
        //        IndicatorVisible = true;
        //        ExtFields.Clear();
        //        using HttpClient client = new HttpClient(GetHttpClientHandler());
        //        List<ExtFields> _ext = new List<ExtFields>();
        //        string resp = "";
        //        if(ServiceOrder != null) {
        //            HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/ext?objNumber=" + ServiceOrder.NewNumber);
        //             resp = response.Content.ReadAsStringAsync().Result;
        //        }
        //        else if(ServiceOrderFireAlarm != null) {
        //            HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/ext?objNumber=" + ServiceOrder.NewNumber);
        //            resp = response.Content.ReadAsStringAsync().Result;
        //        }
        //        try {
        //            if(!string.IsNullOrEmpty(resp))
        //                _ext = JsonConvert.DeserializeObject<List<ExtFields>>(resp);
        //        }
        //        catch { }
        //        if(_ext.Count() > 0) {
        //            foreach(var item in _ext) {
        //                ExtFields.Add(item);
        //            }
        //        }
        //        OpacityForm = 1;
        //        IndicatorVisible = false;
        //    });
        //}


        private RelayCommand _RefreshCommand;
        public RelayCommand RefreshCommand {
            get => _RefreshCommand ??= new RelayCommand(async obj => {
            });
        }
        private bool _WiresExpandedState;
        public bool WiresExpandedState {
            get => _WiresExpandedState;
            set {
                _WiresExpandedState = value;
                if(_WiresExpandedState)
                    ArrowCircleWires = IconName("arrow_circle_up");
                else
                    ArrowCircleWires = IconName("arrow_circle_down");
                OnPropertyChanged(nameof(WiresExpandedState));
            }
        }

        private bool _ExtFieldsExpandedState;
        public bool ExtFieldsExpandedState {
            get => _ExtFieldsExpandedState;
            set {
                if(_ExtFieldsExpandedState)
                    ArrowCircleExtFields = IconName("arrow_circle_up");
                else
                    ArrowCircleExtFields = IconName("arrow_circle_down");
                _ExtFieldsExpandedState = value;
                OnPropertyChanged(nameof(ExtFieldsExpandedState));
            }
        }

        private RelayCommand _ExtFieldsExpanderCommand;
        public RelayCommand ExtFieldsExpanderCommand {
            get => _ExtFieldsExpanderCommand ??= new RelayCommand(async obj => {
                ExtFieldsExpandedState = !ExtFieldsExpandedState;
            });
        }
        private RelayCommand _WiresExpanderCommand;
        public RelayCommand WiresExpanderCommand {
            get => _WiresExpanderCommand ??= new RelayCommand(async obj => {
                WiresExpandedState = !WiresExpandedState;
            });
        }

        private ImageSource _ArrowCircleWires;
        public ImageSource ArrowCircleWires {
            get => _ArrowCircleWires;
            set {
                _ArrowCircleWires = value;
                OnPropertyChanged(nameof(ArrowCircleWires));
            }
        }

        private ImageSource _ArrowCircleExtFields;
        public ImageSource ArrowCircleExtFields {
            get => _ArrowCircleExtFields;
            set {
                _ArrowCircleExtFields = value;
                OnPropertyChanged(nameof(ArrowCircleExtFields));
            }
        }

        private ImageSource _CloseImage;
        public ImageSource CloseImage {
            get => _CloseImage;
            set {
                _CloseImage = value;
                OnPropertyChanged(nameof(CloseImage));
            }
        }
    }
}
