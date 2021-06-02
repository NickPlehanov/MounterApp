using MounterApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage {
        public SettingsPageViewModel VM { get; private set; }
        public SettingsPage() {
            InitializeComponent();
        }
        public SettingsPage(SettingsPageViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
        }
        protected override bool OnBackButtonPressed() {
            if (VM.BackPressCommand.CanExecute(null)) {
                VM.BackPressCommand.Execute(null);
                return true;
            }
            else {
                return false;
            }
        }
    }
}