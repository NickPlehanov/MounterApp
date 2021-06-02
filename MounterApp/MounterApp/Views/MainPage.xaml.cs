using MounterApp.Helpers;
using MounterApp.ViewModel;
using System;
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
        private void SendNotification(object sender, EventArgs e) {
            DependencyService.Get<INotification>().CreateNotification("title", "text");
        }
    }
}
