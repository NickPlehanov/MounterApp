using MounterApp.Model;
using MounterApp.ViewModel;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage {
        public MainMenuPageViewModel VM { get; private set; }
        public MainMenuPage() {
            InitializeComponent();
        }
        public MainMenuPage(List<NewMounterExtensionBase> _mounters, List<NewServicemanExtensionBase> _servicemans) {
            InitializeComponent();
            this.BindingContext = new MainMenuPage(_mounters, _servicemans);
        }
        public MainMenuPage(MainMenuPageViewModel vm) {
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