using MounterApp.Model;
using MounterApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewMountpage : ContentPage {
        public NewMountPageViewModel ViewModel { get; private set; }
        public NewMountpage() {
            InitializeComponent();
        }
        public NewMountpage(NewMountPageViewModel vm) {
            InitializeComponent();
            ViewModel = vm;
            this.BindingContext = ViewModel;
        }
        //public NewMountpage(List<NewMounterExtensionBase> _mounters) {
        //    InitializeComponent();
        //    this.BindingContext = new MountsPage(_mounters);
        //}
    }
}