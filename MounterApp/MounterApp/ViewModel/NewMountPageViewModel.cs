using Android.Graphics.Drawables;
using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Xamarin.Forms;
using System.ComponentModel;

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
        public NewMountPageViewModel(Mounts mount,List<NewMounterExtensionBase> mounters) {
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Карточка объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Схема объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Расшлейфовка объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Ответственные объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Вывеска объекта" });
            //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Доп. фото" });
            ImgSrc = "EmptyPhoto.png";

            Mounters = mounters;
            Mount = mount;
            ObjectNumber = Mount.ObjectNumber;
            ObjectAddress = Mount.AddressName;
            ObjectName = Mount.ObjectName;
            ObjectDriveways = Mount.Driveways;
            Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectCard,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectCard)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Карточка объекта")));
            Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectScheme,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectScheme)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Схема объекта")));
            Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectWiring,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectWiring)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Расшлейфовка объекта")));
            Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectListResponsible,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectListResponsible)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Ответственные объекта")));
            Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectSignboard,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectSignboard)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Вывеска объекта")));
        }

        private Mounts _Mount;
        public Mounts Mount {
            get => _Mount;
            set {
                _Mount = value;
                OnPropertyChanged(nameof(Mount));
            }
        }
        private ImageSource _ImgSrc;
        public ImageSource ImgSrc {
            get => _ImgSrc;
            set {
                _ImgSrc = value;
                OnPropertyChanged(nameof(ImgSrc));
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
        private double _ProgressValue;
        public double ProgressValue {
            get => _ProgressValue;
            set {
                _ProgressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
            }
        }
        private bool _IndicatorVisible;
        public bool IndicatorVisible {
            get => _IndicatorVisible;
            set {
                _IndicatorVisible = value;
                OnPropertyChanged(nameof(IndicatorVisible));
            }
        }
        private RelayCommand _SendToServer;
        public RelayCommand SendToServer {
            get => _SendToServer ??= new RelayCommand(async obj => {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += async (s,e) => {
                IndicatorVisible = true;
                if(string.IsNullOrEmpty(ObjectNumberValidationError) && string.IsNullOrWhiteSpace(ObjectNumberValidationError))
                    if(string.IsNullOrEmpty(ObjectNameValidationError) && string.IsNullOrWhiteSpace(ObjectNameValidationError))
                        if(string.IsNullOrEmpty(ObjectAddressValidationError) && string.IsNullOrWhiteSpace(ObjectAddressValidationError)) {
                            bool error = false;
                            ProgressValue = 0.1;
                            //int _id = App.Database.GetCurrentID();
                            Mounts mount = new Mounts() {
                                ObjectNumber = ObjectNumber,
                                ObjectName = ObjectName,
                                AddressName = ObjectAddress,
                                //TODO:получать максимальный из базы и писать +1
                                //ID = App.Database.GetCurrentID(),
                                MounterID = Mounters.FirstOrDefault().NewMounterId,
                                Driveways = ObjectDriveways,
                                State = 0,
                                ObjectCard = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Карточка объекта").Data,
                                ObjectScheme = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Схема объекта").Data,
                                ObjectWiring = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Расшлейфовка объекта").Data,
                                ObjectListResponsible = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Ответственные объекта").Data,
                                ObjectSignboard = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Вывеска объекта").Data
                            };
                            ProgressValue = 0.2;
                            App.Database.UpdateMount(mount);
                            //App.Database.SaveMount(mount);
                            foreach(PhotoCollection ph in Photos.Where(x => x.Data != null)) {
                                using(HttpClient client = new HttpClient()) {
                                    MultipartFormDataContent form = new MultipartFormDataContent();
                                    form.Add(new StreamContent(new MemoryStream(Convert.FromBase64String(ph.Data)))
                                        ,String.Format("file"),String.Format(ObjectNumber + "_" + ph._Types.PhotoTypeName + ".jpeg"));
                                    //form.Add(new StreamContent(ph.File.GetStream()),String.Format("file"),String.Format(ObjectNumber + "_" + ph._Types.PhotoTypeName + ".jpeg"));
                                    HttpResponseMessage response = await client.PostAsync(Resources.BaseAddress + "/api/Common"
                                            ,form);
                                    ProgressValue += 0.1;
                                    if(response.StatusCode.ToString() != "OK") {
                                        await Application.Current.MainPage.DisplayAlert("Ошибка (Фото не было загружено)",response.Content.ReadAsStringAsync().Result,"OK");
                                        error = true;
                                    }
                                }
                            }
                            if(!error) {
                                //mount.State = 1;
                                //App.Database.UpdateMount(mount);
                                ProgressValue = 0.9;
                                if(Mount != null)
                                    App.Database.DeleteMount(Mount.ID);
                                //TODO: очищать форму?
                            }
                        }
            };
                bw.RunWorkerCompleted += (s,e) => {
                    ProgressValue = 1;
                    IndicatorVisible = false;
                };
                bw.RunWorkerAsync();
            });
        }
        private RelayCommand _SaveToDB;
        public RelayCommand SaveToDB {
            get => _SaveToDB ??= new RelayCommand(async obj => {
                Mounts mount = new Mounts() {
                    ObjectNumber = ObjectNumber,
                    ObjectName = ObjectName,
                    AddressName = ObjectAddress,
                    //TODO:получать максимальный из базы и писать +1
                    ID = App.Database.GetCurrentID(),
                    MounterID = Mounters.FirstOrDefault().NewMounterId,
                    Driveways = ObjectDriveways,
                    State = 0,
                    ObjectCard = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Карточка объекта").Data,
                    ObjectScheme = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Схема объекта").Data,
                    ObjectWiring = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Расшлейфовка объекта").Data,
                    ObjectListResponsible = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Ответственные объекта").Data,
                    ObjectSignboard = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Вывеска объекта").Data
                };
                //App.Database.UpdateMount(mount);
                App.Database.SaveMount(mount);
            });
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
                            ImgSrc = "EmptyPhoto.png";
                            SelectedPhoto = null;
                            PhotoNames.Remove(PhotoName);
                        }
                        else
                            await Application.Current.MainPage.DisplayAlert("Ошибка","Такая фотография уже была загружена","OK");
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Ошибка","Выберите тип фотографии","OK");
            });
        }
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                MountsViewModel vm = new MountsViewModel(Mounters);
                App.Current.MainPage = new MountsPage(vm);
            });
        }
        private MediaFile _File;
        public MediaFile File {
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
                    string result = await Application.Current.MainPage.DisplayPromptAsync("Удаление","Вы действительно хотите удалить выбранную строку? Напишите +, для удаления в поле ниже.");
                    if(result.Equals("+")) {
                        Photos.Remove(SelectedPhoto);
                        PhotoNames.Add(SelectedPhoto._Types);
                    }
                }
                SelectedPhoto = null;
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
            //get => _ObjectNumber;
            get {
                if(string.IsNullOrEmpty(_ObjectNumber) || string.IsNullOrWhiteSpace(_ObjectNumber)) {
                    ObjectNumberValidationError = "Номер объекта не может быть пустым";
                    VisibleObjectNumberValidationError = true;
                }
                return _ObjectNumber;
            }
            set {
                _ObjectNumber = value;
                if(string.IsNullOrEmpty(_ObjectNumber) || string.IsNullOrWhiteSpace(_ObjectNumber)) {
                    ObjectNumberValidationError = "Номер объекта не может быть пустым";
                    VisibleObjectNumberValidationError = true;
                }
                else if(!int.TryParse(_ObjectNumber,out _)) {
                    ObjectNumberValidationError = "Номер объекта должен быть числовым значением";
                    VisibleObjectNumberValidationError = true;
                }
                else {
                    ObjectNumberValidationError = "";
                    VisibleObjectNumberValidationError = false;
                }
                OnPropertyChanged(nameof(ObjectNumber));
            }
        }

        private string _ObjectName;
        public string ObjectName {
            //get => _ObjectName;
            get {
                if(string.IsNullOrEmpty(_ObjectName) || string.IsNullOrWhiteSpace(_ObjectName)) {
                    ObjectNameValidationError = "Название объекта не может быть пустым";
                    VisibleObjectNameValidationError = true;
                }
                return _ObjectName;
            }
            set {
                _ObjectName = value;
                if(string.IsNullOrEmpty(_ObjectName) || string.IsNullOrWhiteSpace(_ObjectName)) {
                    ObjectNameValidationError = "Название объекта не может быть пустым";
                    VisibleObjectNameValidationError = true;
                }
                else {
                    ObjectNameValidationError = "";
                    VisibleObjectNameValidationError = false;
                }
                OnPropertyChanged(nameof(ObjectName));
            }
        }

        private string _ObjectAddress;
        public string ObjectAddress {
            //get => _ObjectAddress;
            get {
                if(string.IsNullOrEmpty(_ObjectAddress) || string.IsNullOrWhiteSpace(_ObjectAddress)) {
                    ObjectAddressValidationError = "Адрес объекта не может быть пустым";
                    VisibleObjectAddressValidationError = true;
                }
                return _ObjectAddress;
            }
            set {
                _ObjectAddress = value;
                if(string.IsNullOrEmpty(_ObjectAddress) || string.IsNullOrWhiteSpace(_ObjectAddress)) {
                    ObjectAddressValidationError = "Адрес объекта не может быть пустым";
                    VisibleObjectAddressValidationError = true;
                }
                else {
                    ObjectAddressValidationError = "";
                    VisibleObjectAddressValidationError = false;
                }
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

        private string _ObjectNumberValidationError;
        public string ObjectNumberValidationError {
            get => _ObjectNumberValidationError;
            set {
                _ObjectNumberValidationError = value;
                OnPropertyChanged(nameof(ObjectNumberValidationError));
            }
        }

        private bool _VisibleObjectNumberValidationError;
        public bool VisibleObjectNumberValidationError {
            get => _VisibleObjectNumberValidationError;
            set {
                _VisibleObjectNumberValidationError = value;
                OnPropertyChanged(nameof(VisibleObjectNumberValidationError));
            }
        }

        private string _ObjectNameValidationError;
        public string ObjectNameValidationError {
            get => _ObjectNameValidationError;
            set {
                _ObjectNameValidationError = value;
                OnPropertyChanged(nameof(ObjectNameValidationError));
            }
        }

        private bool _VisibleObjectNameValidationError;
        public bool VisibleObjectNameValidationError {
            get => _VisibleObjectNameValidationError;
            set {
                _VisibleObjectNameValidationError = value;
                OnPropertyChanged(nameof(VisibleObjectNameValidationError));
            }
        }

        private string _ObjectAddressValidationError;
        public string ObjectAddressValidationError {
            get => _ObjectAddressValidationError;
            set {
                _ObjectAddressValidationError = value;
                OnPropertyChanged(nameof(ObjectAddressValidationError));
            }
        }

        private bool _VisibleObjectAddressValidationError;
        public bool VisibleObjectAddressValidationError {
            get => _VisibleObjectAddressValidationError;
            set {
                _VisibleObjectAddressValidationError = value;
                OnPropertyChanged(nameof(VisibleObjectAddressValidationError));
            }
        }

        //private bool _SendButtonEnabled;
        //public bool SendButtonEnabled {
        //    get => _SendButtonEnabled;
        //    set {
        //        _SendButtonEnabled = value;
        //        SendButtonEnabled = false;
        //        if(!string.IsNullOrEmpty(ObjectNumberValidationError) && !string.IsNullOrWhiteSpace(ObjectNumberValidationError))
        //            if(!string.IsNullOrEmpty(ObjectNameValidationError) && !string.IsNullOrWhiteSpace(ObjectNameValidationError))
        //                if(!string.IsNullOrEmpty(ObjectAddressValidationError) && !string.IsNullOrWhiteSpace(ObjectAddressValidationError))
        //                    SendButtonEnabled = true;
        //        OnPropertyChanged(nameof(SendButtonEnabled));
        //    }
        //}

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
