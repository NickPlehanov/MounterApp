
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Android.Content;

namespace MounterApp.Droid {
    [Activity(Label = "MounterApp",Icon = "@drawable/icon",Theme = "@style/MainTheme", MainLauncher = true,ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
		protected override void OnCreate(Bundle savedInstanceState) {
			global::Xamarin.Forms.Forms.SetFlags(new string[] { "Expander_Experimental","SwipeView_Experimental","Brush_Experimental","AppTheme_Experimental" });
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;
			//var serviceToStart = new Intent(this, typeof(NotificationService));
			//StartService(serviceToStart);
			//App.Current.UserAppTheme = OSAppTheme.Light;

			base.OnCreate(savedInstanceState);

			//Plugin.Iconize.Iconize.Init(Resource.Id.toolbar,Resource.Id.sliding_tabs);

			Rg.Plugins.Popup.Popup.Init(this,savedInstanceState);

			Xamarin.Essentials.Platform.Init(this,savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this,savedInstanceState);
			LoadApplication(new App());
		}
		public override void OnRequestPermissionsResult(int requestCode,string[] permissions,[GeneratedEnum] Android.Content.PM.Permission[] grantResults) {
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode,permissions,grantResults);
			base.OnRequestPermissionsResult(requestCode,permissions,grantResults);

			//var serviceToStart = new Intent(this, typeof(NotificationService));
			//StartService(serviceToStart);
		}
		public override void OnBackPressed() {
			if(Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed)) {
				App.Current.MainPage.Navigation.PopPopupAsync(true);
				StopService(new Intent(this, typeof(NotificationService)));
			}
		}

        protected override void OnPostResume() {
            base.OnPostResume();
			var serviceToStart = new Intent(this, typeof(NotificationService));
			StartService(serviceToStart);
		}
    }
}