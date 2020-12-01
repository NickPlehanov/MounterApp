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
    public partial class PastOrdersPopupPage : PopupPage {
        public PastOrdersPopupViewModel VM { get; private set; }
        public PastOrdersPopupPage(PastOrdersPopupViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
        }
    }
}