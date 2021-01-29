using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class ImagePopupViewModel:BaseViewModel {
        public ImagePopupViewModel(ImageSource image) {
            Image = image;
        }


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
