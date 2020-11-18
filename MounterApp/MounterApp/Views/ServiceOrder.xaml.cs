﻿using MounterApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceOrder : ContentPage {

        public ServiceOrderViewModel VM { get; private set; }
        public ServiceOrder() {
            InitializeComponent();
        }
        public ServiceOrder(ServiceOrderViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
        }
    }
}