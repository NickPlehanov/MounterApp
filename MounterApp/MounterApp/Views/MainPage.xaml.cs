using MounterApp.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MounterApp {
	public partial class MainPage : ContentPage {
		public MainPage() {
			InitializeComponent();
		}
		public MainPageViewModel VM { get; private set; }
        public MainPage(MainPageViewModel vm) {
			InitializeComponent();
			VM = vm;
			this.BindingContext = VM;
		}
	}
}
