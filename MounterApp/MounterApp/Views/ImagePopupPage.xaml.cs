using MounterApp.ViewModel;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePopupPage : PopupPage {
        public ImagePopupViewModel VM { get; private set; }
        public ImagePopupPage() {
            InitializeComponent();
        }
        private void PopupPage_BackgroundClicked(object sender,EventArgs e) {
            App.Current.MainPage.Navigation.PopPopupAsync(false);
        }
        public ImagePopupPage(ImagePopupViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
            this.HasSystemPadding = false;
        }
    }
}