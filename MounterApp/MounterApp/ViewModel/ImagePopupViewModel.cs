using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class ImagePopupViewModel:BaseViewModel {
        /// <summary>
        /// Конструктор принимающий на входе картинку
        /// </summary>
        /// <param name="image"></param>
        public ImagePopupViewModel(ImageSource image) {
            Image = image;
        }
        /// <summary>
        /// Свойство хранящее картинку
        /// </summary>
        private ImageSource _Image;
        public ImageSource Image {
            get => _Image;
            set {
                _Image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
    }
}
