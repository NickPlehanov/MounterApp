using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

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
