﻿using MounterApp.Model;
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