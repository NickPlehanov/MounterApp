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
    public partial class ServiceOrderFireAlarm : ContentPage {

        public ServiceOrderFireAlarmViewModel VM { get; private set; }
        public ServiceOrderFireAlarm() {
            InitializeComponent();
        }
        public ServiceOrderFireAlarm(ServiceOrderFireAlarmViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
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