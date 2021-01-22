using MounterApp.Helpers;
using Rg.Plugins.Popup.Extensions;

namespace MounterApp.ViewModel {
    public class HelpPopupViewModel : BaseViewModel {
        public HelpPopupViewModel(string msg) {
            Message = msg;
        }

        private string _Message;
        public string Message {
            get => _Message;
            set {
                _Message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand {
            get => _CloseCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(false);
            });
        }
        private RelayCommand _BackPressedCommand;
        public RelayCommand BackPressedCommand {
            get => _BackPressedCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(false);
            });
        }
    }
}
