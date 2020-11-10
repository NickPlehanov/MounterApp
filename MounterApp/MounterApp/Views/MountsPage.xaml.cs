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
        public MountsViewModel ViewModel { get; private set; }

        public MountsPage(MountsViewModel vm) {
            InitializeComponent();
            ViewModel = vm;
            this.BindingContext = ViewModel;
        }
        public MountsPage(List<NewMounterExtensionBase> _mounters) {
            InitializeComponent();
            this.BindingContext = new MountsPage(_mounters);
        }
    }
}