using MounterApp.Model;
using MounterApp.ViewModel;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MountsPage : ContentPage {
        public MountsPage() {
            InitializeComponent();
        }
        public MountsViewModel VM { get; private set; }

        public MountsPage(MountsViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
        }
        public MountsPage(List<NewMounterExtensionBase> _mounters) {
            InitializeComponent();
            this.BindingContext = new MountsPage(_mounters);
        }
        protected override bool OnBackButtonPressed() {
            //var vm = (ViewModel)BindingContext;
            if(VM.BackPressCommand.CanExecute(null))  // You can add parameters if any
              {
                VM.BackPressCommand.Execute(null); // You can add parameters if any
                return true;
            }
            else
                return false;
        }
    }
}