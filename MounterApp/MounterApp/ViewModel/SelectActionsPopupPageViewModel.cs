using Android.Widget;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace MounterApp.ViewModel {
    public class SelectActionsPopupPageViewModel : BaseViewModel {
        public SelectActionsPopupPageViewModel(Mounts _mount,List<NewMounterExtensionBase> _mounters,List<NewServicemanExtensionBase> _servicemans) {
            Mount = _mount;
            Mounters = _mounters;
            Servicemans = _servicemans;
            IsPickPhoto = null;
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Карточка объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Схема объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Расшлейфовка объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Ответственные объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Вывеска объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Доп. фото" });
            //ImgSrc = "EmptyPhoto.png";
            VisibleAcceptedLayout = false;
            CameraImage = IconName("camera");
            CollectionImage = IconName("collections");
            CloseImage = IconName("close");
            IsChanged = false;
            Analytics.TrackEvent("Инициализация страницы выбора способа получения фото");
            Counter = 0;
        }

        private List<NewServicemanExtensionBase> _Servicemans;
        public List<NewServicemanExtensionBase> Servicemans {
            get => _Servicemans;
            set {
                _Servicemans = value;
                OnPropertyChanged(nameof(Servicemans));
            }
        }
        private ImageSource _CameraImage;
        public ImageSource CameraImage {
            get => _CameraImage;
            set {
                _CameraImage = value;
                OnPropertyChanged(nameof(CameraImage));
            }
        }

        private ImageSource _CollectionImage;
        public ImageSource CollectionImage {
            get => _CollectionImage;
            set {
                _CollectionImage = value;
                OnPropertyChanged(nameof(CollectionImage));
            }
        }

        private ImageSource _CloseImage;
        public ImageSource CloseImage {
            get => _CloseImage;
            set {
                _CloseImage = value;
                OnPropertyChanged(nameof(CloseImage));
            }
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
        private string _PhotoComment;
        public string PhotoComment {
            get => _PhotoComment;
            set {
                _PhotoComment = value;
                OnPropertyChanged(nameof(PhotoComment));
            }
        }
        private MediaFile _File;
        public MediaFile File {
            get => _File;
            set {
                _File = value;
                OnPropertyChanged(nameof(File));
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
        private RelayCommand _AddNewPhotoCommand;
        public RelayCommand AddNewPhotoCommand {
            get => _AddNewPhotoCommand ??= new RelayCommand(async obj => {
                if(PhotoName != null) {
                    if(File != null) {
                        string tmp = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                        if(!Photos.Any(y => y.Data == tmp)) {
                            Photos.Add(new PhotoCollection(
                                Guid.NewGuid(),
                                Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path)),
                                PhotoComment,
                                    ImageSource.FromStream(() => {
                                        var stream = File.GetStream();
                                        return stream;
                                    }),
                                    PhotoName
                                ));
                            PhotoSource.Add(ImgSrc);
                            switch(PhotoName.PhotoTypeName) {
                                case "Карточка объекта":
                                    Mount.ObjectCard = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    break;
                                case "Схема объекта":
                                    Mount.ObjectScheme = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    break;
                                case "Расшлейфовка объекта":
                                    Mount.ObjectWiring = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    break;
                                case "Ответственные объекта":
                                    Mount.ObjectListResponsible = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    break;
                                case "Вывеска объекта":
                                    Mount.ObjectSignboard = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    break;
                                case "Доп. фото":
                                    Counter++;
                                    if(Mount.ObjectExtra1 == null)
                                        Mount.ObjectExtra1 = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    else if(Mount.ObjectExtra2 == null)
                                        Mount.ObjectExtra2 = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    else if(Mount.ObjectExtra3 == null)
                                        Mount.ObjectExtra3 = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    else if(Mount.ObjectExtra4 == null)
                                        Mount.ObjectExtra4 = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    else if(Mount.ObjectExtra5 == null)
                                        Mount.ObjectExtra5 = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    break;
                            }
                            //ImgSrc = "EmptyPhoto.png";
                            ImgSrc = null;
                            SelectedPhoto = null;
                            if(!PhotoName.PhotoTypeName.Equals("Доп. фото"))
                                PhotoNames.Remove(PhotoName);
                            if(Counter == 5)
                                PhotoNames.Remove(PhotoName);
                            Toast.MakeText(Android.App.Application.Context,"Фото добавлено",ToastLength.Short).Show();
                        }
                        else
                            await Application.Current.MainPage.DisplayAlert("Ошибка","Такая фотография уже была загружена","OK");
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Ошибка","Выберите тип фотографии","OK");
            });
        }

        private int _Counter;
        public int Counter {
            get => _Counter;
            set {
                _Counter = value;
                OnPropertyChanged(nameof(Counter));
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
        private ObservableCollection<ImageSource> _PhotoSource = new ObservableCollection<ImageSource>();
        public ObservableCollection<ImageSource> PhotoSource {
            get => _PhotoSource;
            set {
                _PhotoSource = value;
                OnPropertyChanged(nameof(PhotoSource));
            }
        }
        private ImageSource _ImgSrc;
        public ImageSource ImgSrc {
            get => _ImgSrc;
            set {
                _ImgSrc = value;
                if(_ImgSrc == null)
                    VisibleAcceptedLayout = false;
                else
                    VisibleAcceptedLayout = true;
                OnPropertyChanged(nameof(ImgSrc));
            }
        }

        private bool _VisibleAcceptedLayout;
        public bool VisibleAcceptedLayout {
            get => _VisibleAcceptedLayout;
            set {
                _VisibleAcceptedLayout = value;
                OnPropertyChanged(nameof(VisibleAcceptedLayout));
            }
        }

        private RelayCommand _AcceptCommand;
        public RelayCommand AcceptCommand {
            get => _AcceptCommand ??= new RelayCommand(async obj => {

            });
        }
        private bool? _IsPickPhoto;
        public bool? IsPickPhoto {
            get => _IsPickPhoto;
            set {
                _IsPickPhoto = value;
                OnPropertyChanged(nameof(IsPickPhoto));
            }
        }

        private bool _IsChanged;
        public bool IsChanged {
            get => _IsChanged;
            set {
                _IsChanged = value;
                OnPropertyChanged(nameof(IsChanged));
            }
        }

        private Mounts _Mount;
        public Mounts Mount {
            get => _Mount;
            set {
                _Mount = value;
                OnPropertyChanged(nameof(Mount));
            }
        }

        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
        }

        private RelayCommand _BackPressedCommand;
        public RelayCommand BackPressedCommand {
            get => _BackPressedCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(false);
                NewMountPageViewModel vm = new NewMountPageViewModel(Mount,Mounters,IsChanged,Servicemans);
                App.Current.MainPage = new NewMountpage(vm);
            });
        }

        private RelayCommand _PickPhotoCommand;
        public RelayCommand PickPhotoCommand {
            get => _PickPhotoCommand ??= new RelayCommand(async obj => {
                IsPickPhoto = true;
                GetPhotoCommand.Execute(null);
                Analytics.TrackEvent("Выбор фото из галереи");
                //await App.Current.MainPage.Navigation.PopPopupAsync(true);
                //NewMountPageViewModel vm = new NewMountPageViewModel(Mount,Mounters,IsPickPhoto);
                //App.Current.MainPage = new NewMountpage(vm);
            });
        }

        private RelayCommand _TakePhotoCommand;
        public RelayCommand TakePhotoCommand {
            get => _TakePhotoCommand ??= new RelayCommand(async obj => {
                IsPickPhoto = false;
                GetPhotoCommand.Execute(null);
                Analytics.TrackEvent("Фото камерой");
            });
        }
        private RelayCommand _GetPhotoCommand;
        public RelayCommand GetPhotoCommand {
            get => _GetPhotoCommand ??= new RelayCommand(async obj => {
                if(IsPickPhoto.HasValue) {
                    await CrossMedia.Current.Initialize();
                    if(!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {
                        var perm = await CheckAndRequestPermissionAsync(new Permissions.Camera());
                        if(perm == PermissionStatus.Denied) {
                            Crashes.TrackError(new Exception("Камера недоступна"),
                            new Dictionary<string,string> {
                            {"Error","Камера недоступна" }
                            });
                            await Application.Current.MainPage.DisplayAlert("No Camera",":( No camera available.","OK");
                            return;
                        }
                    }
                    int q = 50;
                    if(Application.Current.Properties.ContainsKey("Quality")) {
                        q = int.Parse(Application.Current.Properties["Quality"].ToString());
                    }
                    if(IsPickPhoto.Value == true) {
                        //await CheckAndRequestPermissionAsync(new StorageRead());
                        File = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions {
                            //Directory = "Sample",
                            //Name = "test.jpg",
                            PhotoSize = PhotoSize.Full,
                            CompressionQuality = q
                        });
                    }
                    else {
                        File = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Full,
                            CompressionQuality = q
                        });
                    }
                    if(File == null)
                        return;

                    ImgSrc = ImageSource.FromStream(() => {
                        var stream = File.GetStream();
                        return stream;
                    });
                    IsChanged = true;
                    //AddNewPhotoCommand.Execute(null);
                }
                else {
                    //SelectActionsPopupPageViewModel vm = new SelectActionsPopupPageViewModel(Mount,Mounters);
                    //await App.Current.MainPage.Navigation.PushPopupAsync(new SelectActionsPopupPage(vm));
                }
            });
        }
    }
}
