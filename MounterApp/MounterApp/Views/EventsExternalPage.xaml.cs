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
    public partial class EventsExternalPage : ContentPage {

        public EventsExternalPageViewModel VM { get; private set; }
        public EventsExternalPage() {
            InitializeComponent();
        }
        public EventsExternalPage(EventsExternalPageViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
        }
        protected override bool OnBackButtonPressed() {
            //var vm = (ViewModel)BindingContext;
            if(VM.BackPressedCommand.CanExecute(null))  // You can add parameters if any
              {
                VM.BackPressedCommand.Execute(null); // You can add parameters if any
                return true;
            }
            else
                return false;
        }
    }
}