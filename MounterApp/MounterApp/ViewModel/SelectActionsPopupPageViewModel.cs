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
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class SelectActionsPopupPageViewModel : BaseViewModel {
        /// <summary>
        /// Конструктор для выбора действия для фотографий монтажа
        /// </summary>
        /// <param name="_mount">Монтаж</param>
        /// <param name="_mounters">Список монтажников</param>
        /// <param name="_servicemans">Список техников</param>
        public SelectActionsPopupPageViewModel(Mounts _mount, List<NewMounterExtensionBase> _mounters, List<NewServicemanExtensionBase> _servicemans) {
            Mount = _mount;
            Mounters = _mounters;
            Servicemans = _servicemans;
            IsPickPhoto = null;
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Вывеска объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Обходной лист" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Расшлейфовка объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Ответственные объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Акт технич. сост-я 1" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Акт технич. сост-я 2" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Схема объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Доп. фото" });
            //ImgSrc = "EmptyPhoto.png";
            VisibleAcceptedLayout = false;
            CameraImage = IconName("camera");
            CollectionImage = IconName("collections");
            CloseImage = IconName("close");
            IsChanged = false;
            Counter = 0;
        }
        /// <summary>
        /// Список техников
        /// </summary>
        private List<NewServicemanExtensionBase> _Servicemans;
        public List<NewServicemanExtensionBase> Servicemans {
            get => _Servicemans;
            set {
                _Servicemans = value;
                OnPropertyChanged(nameof(Servicemans));
            }
        }
        /// <summary>
        /// Иконка камеры
        /// </summary>
        private ImageSource _CameraImage;
        public ImageSource CameraImage {
            get => _CameraImage;
            set {
                _CameraImage = value;
                OnPropertyChanged(nameof(CameraImage));
            }
        }
        /// <summary>
        /// Иконка коллекции
        /// </summary>
        private ImageSource _CollectionImage;
        public ImageSource CollectionImage {
            get => _CollectionImage;
            set {
                _CollectionImage = value;
                OnPropertyChanged(nameof(CollectionImage));
            }
        }
        /// <summary>
        /// Иконка закрытия
        /// </summary>
        private ImageSource _CloseImage;
        public ImageSource CloseImage {
            get => _CloseImage;
            set {
                _CloseImage = value;
                OnPropertyChanged(nameof(CloseImage));
            }
        }
        /// <summary>
        /// Коллекция наименований фотографий для монтажа
        /// </summary>
        private ObservableCollection<PhotoTypes> _PhotoNames = new ObservableCollection<PhotoTypes>();
        public ObservableCollection<PhotoTypes> PhotoNames {
            get => _PhotoNames;
            set {
                _PhotoNames = value;
                OnPropertyChanged(nameof(PhotoNames));
            }
        }
        /// <summary>
        /// Выбранное наименование фотографии
        /// </summary>
        private PhotoTypes _PhotoName;
        public PhotoTypes PhotoName {
            get => _PhotoName;
            set {
                _PhotoName = value;
                OnPropertyChanged(nameof(PhotoName));
            }
        }
        /// <summary>
        /// Комментарий к фотографии
        /// </summary>
        private string _PhotoComment;
        public string PhotoComment {
            get => _PhotoComment;
            set {
                _PhotoComment = value;
                OnPropertyChanged(nameof(PhotoComment));
            }
        }
        /// <summary>
        /// Выбранный из галереи файл
        /// </summary>
        private MediaFile _File;
        public MediaFile File {
            get => _File;
            set {
                _File = value;
                OnPropertyChanged(nameof(File));
            }
        }
        /// <summary>
        /// Коллекция фотографий монтажа
        /// </summary>
        private ObservableCollection<PhotoCollection> _Photos = new ObservableCollection<PhotoCollection>();
        public ObservableCollection<PhotoCollection> Photos {
            get => _Photos;
            set {
                _Photos = value;
                OnPropertyChanged(nameof(Photos));
            }
        }
        /// <summary>
        /// Команда добавления нового фото
        /// </summary>
        private RelayCommand _AddNewPhotoCommand;
        public RelayCommand AddNewPhotoCommand {
            get => _AddNewPhotoCommand ??= new RelayCommand(async obj => {
                if (PhotoName != null) {
                    if (File != null) {
                        //конвертируем фото в Base64
                        string tmp = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                        //если такого фото нет, то добавляем в коллекцию
                        if (!Photos.Any(y => y.Data == tmp)) {
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
                            //определяем тип фотографии и записываем в объект монтажа
                            switch (PhotoName.PhotoTypeName) {
                                case "Обходной лист":
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
                                case "Акт технич. сост-я 1":
                                    Mount.ObjectActTech1 = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    break;
                                case "Акт технич. сост-я 2":
                                    Mount.ObjectActTech2 = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    break;
                                case "Доп. фото":
                                    Counter++;
                                    if (Mount.ObjectExtra1 == null) {
                                        Mount.ObjectExtra1 = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    }
                                    else if (Mount.ObjectExtra2 == null) {
                                        Mount.ObjectExtra2 = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    }
                                    else if (Mount.ObjectExtra3 == null) {
                                        Mount.ObjectExtra3 = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    }
                                    else if (Mount.ObjectExtra4 == null) {
                                        Mount.ObjectExtra4 = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    }
                                    else if (Mount.ObjectExtra5 == null) {
                                        Mount.ObjectExtra5 = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                                    }

                                    break;
                            }
                            ImgSrc = null;
                            SelectedPhoto = null;
                            if (!PhotoName.PhotoTypeName.Equals("Доп. фото")) 
                                PhotoNames.Remove(PhotoName);                            

                            if (Counter == 5) 
                                PhotoNames.Remove(PhotoName);                            
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Фото добавлено", Color.Green, LayoutOptions.EndAndExpand), 500));
                        }
                        else 
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Такая фотография уже была загружена", Color.Red, LayoutOptions.EndAndExpand), 4000));                        
                    }
                }
                else 
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Выберите тип фотографии", Color.Red, LayoutOptions.EndAndExpand), 4000));
            });
        }
        /// <summary>
        /// Счетчик для доп.фото
        /// </summary>
        private int _Counter;
        public int Counter {
            get => _Counter;
            set {
                _Counter = value;
                OnPropertyChanged(nameof(Counter));
            }
        }
        /// <summary>
        /// выбранное фото
        /// </summary>
        private PhotoCollection _SelectedPhoto;
        public PhotoCollection SelectedPhoto {
            get => _SelectedPhoto;
            set {
                _SelectedPhoto = value;
                OnPropertyChanged(nameof(SelectedPhoto));
            }
        }
        /// <summary>
        /// Коллекция фотографий
        /// </summary>
        private ObservableCollection<ImageSource> _PhotoSource = new ObservableCollection<ImageSource>();
        public ObservableCollection<ImageSource> PhotoSource {
            get => _PhotoSource;
            set {
                _PhotoSource = value;
                OnPropertyChanged(nameof(PhotoSource));
            }
        }
        /// <summary>
        /// Фотография
        /// </summary>
        private ImageSource _ImgSrc;
        public ImageSource ImgSrc {
            get => _ImgSrc;
            set {
                _ImgSrc = value;
                if (_ImgSrc == null) 
                    VisibleAcceptedLayout = false;
                else 
                    VisibleAcceptedLayout = true;
                OnPropertyChanged(nameof(ImgSrc));
            }
        }
        /// <summary>
        /// видимость поля подтверждения фотографии
        /// </summary>
        private bool _VisibleAcceptedLayout;
        public bool VisibleAcceptedLayout {
            get => _VisibleAcceptedLayout;
            set {
                _VisibleAcceptedLayout = value;
                OnPropertyChanged(nameof(VisibleAcceptedLayout));
            }
        }
        /// <summary>
        /// Флаг определяющий способ получения фото
        /// true - камера
        /// false - галерея
        /// </summary>
        private bool? _IsPickPhoto;
        public bool? IsPickPhoto {
            get => _IsPickPhoto;
            set {
                _IsPickPhoto = value;
                OnPropertyChanged(nameof(IsPickPhoto));
            }
        }
        /// <summary>
        /// Флаг того что было проведено изменение
        /// </summary>
        private bool _IsChanged;
        public bool IsChanged {
            get => _IsChanged;
            set {
                _IsChanged = value;
                OnPropertyChanged(nameof(IsChanged));
            }
        }
        /// <summary>
        /// монтаж
        /// </summary>
        private Mounts _Mount;
        public Mounts Mount {
            get => _Mount;
            set {
                _Mount = value;
                OnPropertyChanged(nameof(Mount));
            }
        }
        /// <summary>
        /// Список монтажников
        /// </summary>
        private List<NewMounterExtensionBase> _Mounters;
        public List<NewMounterExtensionBase> Mounters {
            get => _Mounters;
            set {
                _Mounters = value;
                OnPropertyChanged(nameof(Mounters));
            }
        }
        /// <summary>
        /// Закрытие окна
        /// </summary>
        private RelayCommand _BackPressedCommand;
        public RelayCommand BackPressedCommand {
            get => _BackPressedCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(false);
                NewMountPageViewModel vm = new NewMountPageViewModel(Mount, Mounters, IsChanged, Servicemans);
                App.Current.MainPage = new NewMountpage(vm);
            });
        }
        /// <summary>
        /// делаем фотографию камерой
        /// </summary>
        private RelayCommand _PickPhotoCommand;
        public RelayCommand PickPhotoCommand {
            get => _PickPhotoCommand ??= new RelayCommand(async obj => {
                IsPickPhoto = true;
                GetPhotoCommand.Execute(null);
            });
        }
        /// <summary>
        /// Получаем фото из галереии
        /// </summary>
        private RelayCommand _TakePhotoCommand;
        public RelayCommand TakePhotoCommand {
            get => _TakePhotoCommand ??= new RelayCommand(async obj => {
                IsPickPhoto = false;
                GetPhotoCommand.Execute(null);
            });
        }
        private RelayCommand _GetPhotoCommand;
        public RelayCommand GetPhotoCommand {
            get => _GetPhotoCommand ??= new RelayCommand(async obj => {
                if (IsPickPhoto.HasValue) {
                    await CrossMedia.Current.Initialize();
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {
                        var perm = await CheckAndRequestPermissionAsync(new Permissions.Camera());
                        if (perm == PermissionStatus.Denied) {
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Камера не доступна", Color.Red, LayoutOptions.EndAndExpand), 4000));
                            return;
                        }
                    }
                    //если у нас возникает проблема с получением параметра качества фото, то просто используем по дефолту = 50
                    int q = 50;
                    if (Application.Current.Properties.ContainsKey("Quality")) 
                        q = int.Parse(Application.Current.Properties["Quality"].ToString());
                    
                    if (IsPickPhoto.Value == true) {
                        File = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions {
                            PhotoSize = PhotoSize.Full,
                            CompressionQuality = q
                        });
                    }
                    else {
                        File = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Full,
                            CompressionQuality = q
                        });
                    }
                    if (File == null) {
                        return;
                    }

                    ImgSrc = ImageSource.FromStream(() => {
                        var stream = File.GetStream();
                        return stream;
                    });
                    IsChanged = true;
                }
            });
        }
    }
}
