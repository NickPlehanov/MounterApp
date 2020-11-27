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
    public partial class EventsPopupPage : PopupPage {
        public EventsPopupViewModel VM { get; private set; }
        public EventsPopupPage() {
            InitializeComponent();
        }
        public EventsPopupPage(EventsPopupViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
        }

        private void PopupPage_BackgroundClicked(object sender,EventArgs e) {
            App.Current.MainPage.Navigation.PopPopupAsync();
        }
    }
}