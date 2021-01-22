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
            this.HasSystemPadding = false;
        }
        public HelpPopupPage(HelpPopupViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
            this.HasSystemPadding = false;
        }
        private void PopupPage_BackgroundClicked(object sender,EventArgs e) {
            App.Current.MainPage.Navigation.PopPopupAsync(false);
        }
    }
}