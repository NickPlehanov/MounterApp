using MounterApp.ViewModel;
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
    public partial class SelectActionsPopupPage : PopupPage {
        public SelectActionsPopupPageViewModel VM { get; private set; } 
        public SelectActionsPopupPage() {
            InitializeComponent();
        }
        public SelectActionsPopupPage(SelectActionsPopupPageViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
        }
    }
}