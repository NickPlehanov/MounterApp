using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
	public class BaseViewModel : INotifyPropertyChanged, INavigation {
		public IReadOnlyList<Page> ModalStack => throw new System.NotImplementedException();

		public IReadOnlyList<Page> NavigationStack => throw new System.NotImplementedException();

		public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName) {
            if(this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

		public void InsertPageBefore(Page page, Page before) {
			throw new System.NotImplementedException();
		}

		public Task<Page> PopAsync() {
			return App.Current.MainPage.Navigation.PopAsync();
		}

		public Task<Page> PopAsync(bool animated) {
			return App.Current.MainPage.Navigation.PopAsync(true);
		}

		public Task<Page> PopModalAsync() {
			return App.Current.MainPage.Navigation.PopModalAsync();
		}

		public Task<Page> PopModalAsync(bool animated) {
			return App.Current.MainPage.Navigation.PopModalAsync(true);
		}

		public Task PopToRootAsync() {
			return App.Current.MainPage.Navigation.PopToRootAsync();
		}

		public Task PopToRootAsync(bool animated) {
			return App.Current.MainPage.Navigation.PopToRootAsync(true);
		}

		public Task PushAsync(Page page) {
			return App.Current.MainPage.Navigation.PushAsync(page);
		}

		public Task PushAsync(Page page, bool animated) {
			return App.Current.MainPage.Navigation.PushAsync(page,true);
		}

		public Task PushModalAsync(Page page) {
			return App.Current.MainPage.Navigation.PushModalAsync(page);
		}

		public Task PushModalAsync(Page page, bool animated) {
			return App.Current.MainPage.Navigation.PushModalAsync(page,true);
		}

		public void RemovePage(Page page) {
			App.Current.MainPage.Navigation.RemovePage(page);
		}
	}
}
