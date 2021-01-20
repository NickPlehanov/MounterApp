using MounterApp.ViewModel;
using Xamarin.Forms;

namespace MounterApp {
    public partial class MainPage : ContentPage {
		public MainPage() {
			InitializeComponent();
			App.Current.UserAppTheme = OSAppTheme.Light;
		}
		public MainPageViewModel VM { get; private set; }
        public MainPage(MainPageViewModel vm) {
			InitializeComponent();
			VM = vm;
			this.BindingContext = VM;
		}
	}
}
