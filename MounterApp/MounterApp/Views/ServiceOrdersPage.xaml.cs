using MounterApp.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceOrdersPage : ContentPage {

        public ServiceOrdersPageViewModel VM { get; private set; }
        public ServiceOrdersPage() {
            InitializeComponent();
        }
        public ServiceOrdersPage(ServiceOrdersPageViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
        }
        protected override bool OnBackButtonPressed() {
            //var vm = (ViewModel)BindingContext;
            if (VM.BackPressCommand.CanExecute(null))  // You can add parameters if any
              {
                VM.BackPressCommand.Execute(null); // You can add parameters if any
                return true;
            }
            else {
                return false;
            }
        }
    }
}