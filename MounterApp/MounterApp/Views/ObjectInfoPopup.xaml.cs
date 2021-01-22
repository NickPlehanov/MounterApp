using MounterApp.ViewModel;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ObjectInfoPopup : PopupPage {
        public ObjectInfoViewModel VM { get; private set; }
        public ObjectInfoPopup() {
            InitializeComponent();
            this.HasSystemPadding = false;
        }
        public ObjectInfoPopup(ObjectInfoViewModel vm) {
            InitializeComponent();
            VM = vm;
            BindingContext = VM;
            this.HasSystemPadding = false;
        }
        private void PopupPage_BackgroundClicked(object sender,EventArgs e) {
            App.Current.MainPage.Navigation.PopPopupAsync(false);
        }
    }
}