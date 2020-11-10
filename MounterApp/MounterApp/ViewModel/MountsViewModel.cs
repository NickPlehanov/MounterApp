using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MounterApp.ViewModel {
    public class MountsViewModel : BaseViewModel {
        private ObservableCollection<Mounts> _Mounts = new ObservableCollection<Mounts>();
        public ObservableCollection<Mounts> Mounts {
            get => _Mounts;
            set {
                _Mounts = value;
                OnPropertyChanged(nameof(Mounts));
            }
        }

        private ObservableCollection<Mounts> _NotSendedMounts = new ObservableCollection<Mounts>();
        public ObservableCollection<Mounts> NotSendedMounts {
            get => _NotSendedMounts;
            set {
                _NotSendedMounts = value;
                OnPropertyChanged(nameof(NotSendedMounts));
            }
        }
        public MountsViewModel() {

        }
        public MountsViewModel(List<NewMounterExtensionBase> mounters) {
            Mounters = mounters;
            var _ntMounts = App.Database.GetMounts(Mounters.FirstOrDefault().NewMounterId).ToList();
            if(_ntMounts != null)
                if(_ntMounts.Any())
                    foreach(var item in _ntMounts)
                        NotSendedMounts.Add(item);
        }
        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
        }

        private RelayCommand _ClickCommand;
        public RelayCommand ClickCommand {
            get => _ClickCommand ??= new RelayCommand(async obj => {
                if(Mounters.Count > 0) {
                    bool t = false;
                }
            });
        }
    }
}