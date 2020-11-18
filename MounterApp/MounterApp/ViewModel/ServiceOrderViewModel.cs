using MounterApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.ViewModel {
    public class ServiceOrderViewModel :BaseViewModel {
        public ServiceOrderViewModel(NewServiceorderExtensionBase _so) {
            ServiceOrderID = _so;
        }
        private NewServiceorderExtensionBase _ServiceOrderID;
        public NewServiceorderExtensionBase ServiceOrderID {
            get => _ServiceOrderID;
            set {
                _ServiceOrderID = value;
                OnPropertyChanged(nameof(ServiceOrderID));
            }
        }
    }
}
