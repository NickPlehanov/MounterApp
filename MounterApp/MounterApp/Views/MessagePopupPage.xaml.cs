using MounterApp.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace MounterApp.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagePopupPage : PopupPage {
        public MessagePopupPageViewModel VM { get; private set; }
        public int Delay { get; private set; }
        public MessagePopupPage(MessagePopupPageViewModel vm, int delay) {
            InitializeComponent();
            VM = vm;
            Delay = delay;
            this.BindingContext = VM;
            this.HasSystemPadding = false;
        }
        public MessagePopupPage() {
            InitializeComponent();
        }
        protected override void OnAppearing() {
            base.OnAppearing();
            HidePopup();
        }
        private async void HidePopup() {
            //await Task.Delay(4000);
            await Task.Delay(Delay);
            //await PopupNavigation.RemovePageAsync(this);
            if (PopupNavigation.Instance.PopupStack.Any()) {
                await PopupNavigation.Instance.PopAsync(false);
                //return await navigation.PopAllPopupAsync(false);
            }
        }
    }
}