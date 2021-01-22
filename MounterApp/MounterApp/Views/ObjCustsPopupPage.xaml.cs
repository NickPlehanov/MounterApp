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
    public partial class ObjCustsPopupPage : PopupPage {
        //public ObjCustsPopupPage() {
        //    InitializeComponent();
        //}
        public ObjCustsPopupViewModel VM { get; private set; }
        public ObjCustsPopupPage(ObjCustsPopupViewModel vm) {
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