using MounterApp.ViewModel;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpPopupPage : PopupPage {
        public HelpPopupViewModel VM { get; private set; }
        public HelpPopupPage() {
            InitializeComponent();
        }
        public HelpPopupPage(HelpPopupViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
        }
        private void PopupPage_BackgroundClicked(object sender,EventArgs e) {
            App.Current.MainPage.Navigation.PopPopupAsync();
        }
    }
}