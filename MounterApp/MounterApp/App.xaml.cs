using MounterApp.Helpers;
using System;
using System.IO;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

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
			AppCenter.Start("android=f1b532c5-c456-4b0d-b829-5f167beae220;",
				  typeof(Analytics),typeof(Crashes));
		}

		protected override void OnSleep() {
		}

		protected override void OnResume() {
		}
	}
}
