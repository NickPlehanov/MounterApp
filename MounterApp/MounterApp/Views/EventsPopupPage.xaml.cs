﻿using MounterApp.ViewModel;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventsPopupPage : PopupPage {
        public EventsPopupViewModel VM { get; private set; }
        public EventsPopupPage() {
            InitializeComponent();
            this.HasSystemPadding = false;
        }
        public EventsPopupPage(EventsPopupViewModel vm) {
            InitializeComponent();
            VM = vm;
            this.BindingContext = VM;
            this.HasSystemPadding = false;
        }

        private void PopupPage_BackgroundClicked(object sender, EventArgs e) {
            App.Current.MainPage.Navigation.PopPopupAsync(false);
        }
    }
}