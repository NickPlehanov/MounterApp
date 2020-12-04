using MounterApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.ViewModel {
    public class SettingsPageViewModel : BaseViewModel {
        public SettingsPageViewModel(List<NewMounterExtensionBase> mounters,List<NewServicemanExtensionBase> servicemans) {
            Mounters = mounters;
            Servicemans = servicemans;
        }

        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
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
    }
}
