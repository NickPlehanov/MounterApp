using Android.Content;
using Android.Content.PM;
using Android.Icu.Util;
using System;
using System.Collections.Generic;
using System.Text;
[assembly: Xamarin.Forms.Dependency(typeof(MounterApp.Helpers.AppVersions))]
namespace MounterApp.Helpers {
    public class AppVersions : IAppVersion {
        public VersionInfo GetVersionAndBuildNumber() {
            Context context = global::Android.App.Application.Context;
            Android.Content.PM.PackageManager manager = context.PackageManager;
            Android.Content.PM.PackageInfo info = manager.GetPackageInfo(context.PackageName,0);

            return new VersionInfo(info.VersionName,info.VersionCode.ToString());
        }
    }
    public interface IAppVersion {
        VersionInfo GetVersionAndBuildNumber();
    }
    public class VersionInfo {
        public VersionInfo(string versionNumber,string buildNumber) {
            VersionNumber = versionNumber;
            BuildNumber = buildNumber;
        }

        public string VersionNumber {
            get;
        }

        public string BuildNumber {
            get;
        }
    }
}
