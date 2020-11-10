using MounterApp.Helpers;
using System;
using System.IO;
using Xamarin.Forms;

namespace MounterApp {
    public partial class App : Application {
		public App() {
			InitializeComponent();

			MainPage = new MainPage();
		}
		static LocalDatabase database;
		public static LocalDatabase Database {
            get {
				if(database==null)
					database=new LocalDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"mounts.db3"));
				return database;
			}
        }

		protected override void OnStart() {
		}

		protected override void OnSleep() {
		}

		protected override void OnResume() {
		}
	}
}
