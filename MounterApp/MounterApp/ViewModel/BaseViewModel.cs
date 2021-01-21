using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;

namespace MounterApp.ViewModel {
    public class BaseViewModel : INotifyPropertyChanged {

        public string IconName(string name) {
            //AppTheme appTheme = AppInfo.RequestedTheme;
            //if(appTheme == AppTheme.Light)
            //    return name+"_white.png";
            //else if(appTheme == AppTheme.Dark)
            //    return name+ ".png";
            //else
            //    return name + ".png";
            return name + ".png";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName) {
            if(this.PropertyChanged != null) {
                this.PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
            }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(prop));
        }

        public HttpClientHandler GetHttpClientHandler() {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender,cert,chain,sslPolicyErrors) => { return true; };
            return clientHandler;
        }
        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : BasePermission {
            var status = await permission.CheckStatusAsync();
            if(status != PermissionStatus.Granted) {
                status = await permission.RequestAsync();
            }

            return status;
        }
    }
}
