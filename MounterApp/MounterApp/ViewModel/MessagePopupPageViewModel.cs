using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class MessagePopupPageViewModel : BaseViewModel {
        public MessagePopupPageViewModel(string _alertMessage, System.Drawing.Color bgColor, LayoutOptions layoutOptions) {
            AlertMessage = _alertMessage;
            BackgroundColor = bgColor;
            VerticalOptionsProperty = layoutOptions;
        }

        private string _AlertMessage;
        public string AlertMessage {
            get => _AlertMessage;
            set {
                _AlertMessage = value;
                OnPropertyChanged(nameof(AlertMessage));
            }
        }

        private System.Drawing.Color _BackgroundColor;
        public System.Drawing.Color BackgroundColor {
            get => _BackgroundColor;
            set {
                _BackgroundColor = value;
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }

        private LayoutOptions _VerticalOptionsProperty;
        public LayoutOptions VerticalOptionsProperty {
            get => _VerticalOptionsProperty;
            set {
                _VerticalOptionsProperty = value;
                OnPropertyChanged(nameof(VerticalOptionsProperty));
            }
        }
    }
}
