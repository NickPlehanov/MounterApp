using MounterApp.ViewModel;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityIndicatorPopupPage : PopupPage {
        public ActivityIndicatorViewModel VM { get; private set; }
        public ActivityIndicatorPopupPage() {
            InitializeComponent();
        }
        public ActivityIndicatorPopupPage(ActivityIndicatorViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = vm;
        }
    }
}