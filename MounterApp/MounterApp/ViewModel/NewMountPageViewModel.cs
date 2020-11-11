using Android.Graphics.Drawables;
using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Views;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class NewMountPageViewModel : BaseViewModel {

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
            ImgSrc = "EmptyPhoto.png";
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
                if(PhotoName != null)
                    Photos.Add(new PhotoCollection(Guid.NewGuid(),PhotoName.PhotoTypeId,PhotoComment,File.Path));
                else
                    await Application.Current.MainPage.DisplayAlert("Ошибка","Выберите тип фотографии","OK");
            });
        }
        private Plugin.Media.Abstractions.MediaFile _File;
        public Plugin.Media.Abstractions.MediaFile File {
            get => _File;
            set {
                _File = value;
                OnPropertyChanged(nameof(File));
            }
        }

        private RelayCommand _TakePhotoCommand;
        public RelayCommand TakePhotoCommand {
            get => _TakePhotoCommand ??= new RelayCommand(async obj => {
                //NewMountpage mountpage = new NewMountpage();
                //string result = await Application.Current.MainPage.DisplayPromptAsync("Question 2","What's 5 + 5?",initialValue: "10",maxLength: 2,keyboard: Keyboard.Numeric);
                await CrossMedia.Current.Initialize();

                if(!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {
                    await Application.Current.MainPage.DisplayAlert("No Camera",":( No camera available.","OK");
                    return;
                }

                File = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions {
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if(File == null)
                    return;

                ImgSrc = ImageSource.FromStream(() => {
                    var stream = File.GetStream();
                    return stream;
                });
            });
        }

        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand {
            get => _DeleteCommand ??= new RelayCommand(async obj => {
                if(SelectedPhoto != null) {
                    string result = await Application.Current.MainPage.DisplayPromptAsync("Удаление","Вы действительно хотите удалить выбранную строку? Напишите +, дял удаления в поле ниже.");
                    if(result.Equals("+"))
                        Photos.Remove(SelectedPhoto);
                }
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

        private string _PhotoComment;
        public string PhotoComment {
            get => _PhotoComment;
            set {
                _PhotoComment = value;
                OnPropertyChanged(nameof(PhotoComment));
            }
        }

        private ObservableCollection<PhotoCollection> _Photos = new ObservableCollection<PhotoCollection>();
        public ObservableCollection<PhotoCollection> Photos {
            get => _Photos;
            set {
                _Photos = value;
                OnPropertyChanged(nameof(Photos));
            }
        }

        private PhotoCollection _SelectedPhoto;
        public PhotoCollection SelectedPhoto {
            get => _SelectedPhoto;
            set {
                _SelectedPhoto = value;
                OnPropertyChanged(nameof(SelectedPhoto));
            }
        }
    }
}
