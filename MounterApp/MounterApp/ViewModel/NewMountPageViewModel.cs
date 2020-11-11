using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Views;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class NewMountPageViewModel :BaseViewModel {

        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
        }
        public NewMountPageViewModel() {

        }
        public NewMountPageViewModel(List<NewMounterExtensionBase> mounters) {
            Mounters = mounters;
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Карточка объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Схема объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Расшлейфовка объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Ответственные объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Вывеска объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Доп. фото" });
        }
        private ImageSource _ImgSrc;
        public ImageSource ImgSrc {
            get => _ImgSrc;
            set {
                _ImgSrc = value;
                OnPropertyChanged(nameof(ImgSrc));
            }
        }
        
        private RelayCommand _AddNewPhotoCommand;
        public RelayCommand AddNewPhotoCommand {
            get => _AddNewPhotoCommand ??= new RelayCommand(async obj => {
                //NewMountpage mountpage = new NewMountpage();
                //string result = await Application.Current.MainPage.DisplayPromptAsync("Question 2","What's 5 + 5?",initialValue: "10",maxLength: 2,keyboard: Keyboard.Numeric);
                await CrossMedia.Current.Initialize();

                if(!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {
                    await Application.Current.MainPage.DisplayAlert("No Camera",":( No camera available.","OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions {
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if(file == null)
                    return;

                await Application.Current.MainPage.DisplayAlert("File Location",file.Path,"OK");

                ImgSrc=ImageSource.FromStream(() => {
                    var stream = file.GetStream();
                    return stream;
                });
                //image.Source = ImageSource.FromStream(() =>
                //{
                //    var stream = file.GetStream();
                //    return stream;
                //});
            });
        }

        private ObservableCollection<PhotoTypes> _PhotoNames = new ObservableCollection<PhotoTypes>();
        public ObservableCollection<PhotoTypes> PhotoNames {
            get => _PhotoNames;
            set {
                _PhotoNames = value;
                OnPropertyChanged(nameof(PhotoNames));
            }
        }

        private PhotoTypes _PhotoName;
        public PhotoTypes PhotoName {
            get => _PhotoName;
            set {
                _PhotoName = value;
                OnPropertyChanged(nameof(PhotoName));
            }
        }

        private string _ObjectNumber;
        public string ObjectNumber {
            get => _ObjectNumber;
            set {
                _ObjectNumber = value;
                OnPropertyChanged(nameof(ObjectNumber));
            }
        }
        private string _ObjectName;
        public string ObjectName {
            get => _ObjectName;
            set {
                _ObjectName = value;
                OnPropertyChanged(nameof(ObjectName));
            }
        }
        private string _ObjectAddress;
        public string ObjectAddress {
            get => _ObjectAddress;
            set {
                _ObjectAddress = value;
                OnPropertyChanged(nameof(ObjectAddress));
            }
        }
        private string _ObjectDriveways;
        public string ObjectDriveways {
            get => _ObjectDriveways;
            set {
                _ObjectDriveways = value;
                OnPropertyChanged(nameof(ObjectDriveways));
            }
        }
    }
}
