using Rg.Plugins.Popup.Extensions;

namespace MounterApp.ViewModel {
    public class ActivityIndicatorViewModel : BaseViewModel {
        public ActivityIndicatorViewModel(bool closed) {
            if (closed)
                App.Current.MainPage.Navigation.PopPopupAsync(true);
        }

        private bool _Closed;
        public bool Closed {
            get => _Closed;
            set {
                _Closed = value;
                OnPropertyChanged(nameof(Closed));
            }
        }
    }
}
