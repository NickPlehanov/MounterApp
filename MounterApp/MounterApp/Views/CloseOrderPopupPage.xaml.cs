using MounterApp.ViewModel;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CloseOrderPopupPage : PopupPage {
        //public CloseOrderPopupPage() {
        //    InitializeComponent();
        //}
        public CloseOrderPopupPageViewModel VM { get; private set; }
        public CloseOrderPopupPage(CloseOrderPopupPageViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
        }

        private void PopupPage_BackgroundClicked(object sender,EventArgs e) {
            App.Current.MainPage.Navigation.PopPopupAsync();
        }
    }
}