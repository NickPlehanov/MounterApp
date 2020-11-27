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
    public partial class ObjectInfoPopup : PopupPage {
        public ObjectInfoViewModel VM { get; private set; }
        public ObjectInfoPopup() {
            InitializeComponent();
        }
        public ObjectInfoPopup(ObjectInfoViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
        }
        private void PopupPage_BackgroundClicked(object sender,EventArgs e) {
            App.Current.MainPage.Navigation.PopPopupAsync();
        }
    }
}