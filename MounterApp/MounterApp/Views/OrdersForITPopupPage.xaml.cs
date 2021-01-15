using MounterApp.ViewModel;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersForITPopupPage : PopupPage {
        public OrdersForITViewModel VM  { get; private set; }
        public OrdersForITPopupPage() {
            InitializeComponent();
        }
        public OrdersForITPopupPage(OrdersForITViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
        }
        private async void PopupPage_BackgroundClicked(object sender,EventArgs e) {
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
    }
}