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
using Rg.Plugins.Popup.Extensions;
using Android.Widget;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;

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
        //public NewMountPageViewModel(Mounts mount,List<NewMounterExtensionBase> mounters,bool IsChanged) {
        //    Mounters = mounters;
        //    Mount = mount;
        //    Mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
        //    //IsPickPhoto = isPickPhoto;
        //    Opacity = 1;
        //    IndicatorVisible = false;
        //    if(!string.IsNullOrEmpty(mount.GoogleComment)) {
        //        GoogleComment = mount.GoogleComment;
        //        VisibleGoogleComment = true;
        //    }
        //    Analytics.TrackEvent("Инициализация страницы заполнения нового монтажа",
        //                new Dictionary<string,string> {
        //                    {"MounterPhone",Mounters.FirstOrDefault().NewPhone },
        //                    {"MountObjectNumber",Mount.ObjectNumber }
        //                });
        //    //IsChanged = false;
        //}
        public NewMountPageViewModel(List<NewMounterExtensionBase> mounters) {
            Mounters = mounters;
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Карточка объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Схема объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Расшлейфовка объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Ответственные объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Вывеска объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Доп. фото" });
            ImgSrc = "EmptyPhoto.png";
            PhotoImage = "media.png";
            SaveImage = "save.png";
            SendImage = "send.png";
            Opacity = 1;
            IndicatorVisible = false;
            IsPickPhoto = null;
            Analytics.TrackEvent("Инициализация страницы заполнения нового монтажа",
                        new Dictionary<string,string> {
                            {"MounterPhone",Mounters.FirstOrDefault().NewPhone }
                        });
            //var mainDisplayInfo = DeviceDisplay.MainDisplayInfo.Height;
            //var height = mainDisplayInfo.Height;
            App.Current.MainPage.HeightRequest= DeviceDisplay.MainDisplayInfo.Height;
        }
        public NewMountPageViewModel(Mounts mount,List<NewMounterExtensionBase> mounters,bool _IsChanged) {
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Карточка объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Схема объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Расшлейфовка объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Ответственные объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Вывеска объекта" });
            PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Доп. фото" });
            ImgSrc = "EmptyPhoto.png";
            PhotoImage = "media.png";
            SaveImage = "save.png";
            SendImage = "send.png";
            IsPickPhoto = null;
            if(!string.IsNullOrEmpty(mount.GoogleComment)) {
                GoogleComment = mount.GoogleComment;
                VisibleGoogleComment = true;
            }

            Mounters = mounters;
            Mount = mount;
            Mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
            ObjectNumber = Mount.ObjectNumber;
            ObjectAddress = Mount.AddressName;
            ObjectName = Mount.ObjectName;
            ObjectDriveways = Mount.Driveways;
            if(!string.IsNullOrEmpty(Mount.ObjectCard))
                Photos.Add(
                    new PhotoCollection(Guid.NewGuid(),Mount.ObjectCard,null,
                    ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectCard)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Карточка объекта")));
            if(!string.IsNullOrEmpty(Mount.ObjectScheme))
                Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectScheme,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectScheme)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Схема объекта")));
            if(!string.IsNullOrEmpty(Mount.ObjectWiring))
                Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectWiring,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectWiring)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Расшлейфовка объекта")));
            if(!string.IsNullOrEmpty(Mount.ObjectListResponsible))
                Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectListResponsible,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectListResponsible)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Ответственные объекта")));
            if(!string.IsNullOrEmpty(Mount.ObjectSignboard))
                Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectSignboard,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectSignboard)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Вывеска объекта")));
            Opacity = 1;
            IndicatorVisible = false;
            Analytics.TrackEvent("Инициализация страницы заполнения нового монтажа",
                        new Dictionary<string,string> {
                            {"MounterPhone",Mounters.FirstOrDefault().NewPhone },
                            {"MountObjectNumber",Mount.ObjectNumber }
                        });
            IsChanged = _IsChanged;
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        }
        private GoogleMountModel _GoogleMount;
        public GoogleMountModel GoogleMount {
            get => _GoogleMount;
            set {
                _GoogleMount = value;
                OnPropertyChanged(nameof(GoogleMount));
            }
        }
        private bool _VisibleGoogleComment;
        public bool VisibleGoogleComment {
            get => _VisibleGoogleComment;
            set {
                _VisibleGoogleComment = value;
                OnPropertyChanged(nameof(VisibleGoogleComment));
            }
        }

        private string _GoogleComment;
        public string GoogleComment {
            get => _GoogleComment;
            set {
                _GoogleComment = value;
                OnPropertyChanged(nameof(GoogleComment));
            }
        }
        private bool? _IsPickPhoto;
        public bool? IsPickPhoto {
            get => _IsPickPhoto;
            set {
                _IsPickPhoto = value;
                OnPropertyChanged(nameof(IsPickPhoto));
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

        private ImageSource _PhotoImage;
        public ImageSource PhotoImage {
            get => _PhotoImage;
            set {
                _PhotoImage = value;
                OnPropertyChanged(nameof(PhotoImage));
            }
        }

        private ImageSource _SaveImage;
        public ImageSource SaveImage {
            get => _SaveImage;
            set {
                _SaveImage = value;
                OnPropertyChanged(nameof(SaveImage));
            }
        }

        private ImageSource _SendImage;
        public ImageSource SendImage {
            get => _SendImage;
            set {
                _SendImage = value;
                OnPropertyChanged(nameof(SendImage));
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

        private double _Opacity;
        public double Opacity {
            get => _Opacity;
            set {
                _Opacity = value;
                OnPropertyChanged(nameof(Opacity));
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

        private string _Path;
        public string Path {
            get => _Path;
            set {
                _Path = value;
                OnPropertyChanged(nameof(Path));
            }
        }
        private string _IndicatorText;
        public string IndicatorText {
            get => _IndicatorText;
            set {
                _IndicatorText = value;
                OnPropertyChanged(nameof(IndicatorText));
            }
        }
        private RelayCommand _SendToServer;
        public RelayCommand SendToServer {
            get => _SendToServer ??= new RelayCommand(async obj => {
                if(string.IsNullOrEmpty(ObjectNumberValidationError) && string.IsNullOrWhiteSpace(ObjectNumberValidationError))
                    if(string.IsNullOrEmpty(ObjectNameValidationError) && string.IsNullOrWhiteSpace(ObjectNameValidationError))
                        if(string.IsNullOrEmpty(ObjectAddressValidationError) && string.IsNullOrWhiteSpace(ObjectAddressValidationError))
                            if(Photos.Count >= 5) {
                                Analytics.TrackEvent("Отправка нового монтажа на сервер",
                                    new Dictionary<string,string> {
                                        {"ObjectNumber",ObjectNumber }
                                    });
                                Opacity = 0.1;
                                IndicatorVisible = true;
                                IndicatorText = "Подождите, идет загрузка...";
                                bool error = false;
                                ProgressValue = 0.1;
                                //TODO:Проверять что монтаж уже есть
                                //if(Mount == null) {
                                    //Mounts mount = new Mounts() {
                                    Mount.ObjectNumber = ObjectNumber;
                                    Mount.ObjectName = ObjectName;
                                    Mount.AddressName = ObjectAddress;
                                    Mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
                                    Mount.Driveways = ObjectDriveways;
                                    Mount.State = 0;
                                    Mount.ObjectCard = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Карточка объекта").Data;
                                    Mount.ObjectScheme = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Схема объекта").Data;
                                    Mount.ObjectWiring = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Расшлейфовка объекта").Data;
                                    Mount.ObjectListResponsible = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Ответственные объекта").Data;
                                    Mount.ObjectSignboard = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Вывеска объекта").Data;
                                    //};
                                    App.Database.SaveUpdateMount(Mount);
                                //}
                                //else {
                                //    Mount.ObjectNumber = ObjectNumber;
                                //    Mount.ObjectName = ObjectName;
                                //    Mount.AddressName = ObjectAddress;
                                //    Mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
                                //    Mount.Driveways = ObjectDriveways;
                                //    Mount.State = 0;
                                //    Mount.ObjectCard = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Карточка объекта").Data;
                                //    Mount.ObjectScheme = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Схема объекта").Data;
                                //    Mount.ObjectWiring = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Расшлейфовка объекта").Data;
                                //    Mount.ObjectListResponsible = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Ответственные объекта").Data;
                                //    Mount.ObjectSignboard = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Вывеска объекта").Data;
                                //}
                                Analytics.TrackEvent("Сохранение монтажа перед отправкой на сервер в локальной базе",
                                    new Dictionary<string,string> {
                                        {"ObjectNumber",ObjectNumber }
                                    });
                                //App.Database.SaveMount(mount);
                                int cnt = 1;
                                string ObjectInfo = " Номер объекта: " + ObjectNumber + Environment.NewLine +
                                " Наименование объекта: " + ObjectName + Environment.NewLine +
                                " Адрес объекта: " + ObjectAddress + Environment.NewLine +
                                " Монтажник: " + Mounters.FirstOrDefault().NewName + Environment.NewLine +
                                " Подъездные пути: " + ObjectDriveways;
                                foreach(PhotoCollection ph in Photos.Where(x => x.Data != null)) {
                                    using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                                        MultipartFormDataContent form = new MultipartFormDataContent();
                                        form.Add(new StreamContent(new MemoryStream(Convert.FromBase64String(ph.Data)))
                                            ,String.Format("file"),String.Format(ObjectNumber + "_" + ph._Types.PhotoTypeName + ".jpeg"));
                                        //form.Add(new StreamContent(ph.File.GetStream()),String.Format("file"),String.Format(ObjectNumber + "_" + ph._Types.PhotoTypeName + ".jpeg"));
                                        //HttpResponseMessage response = await client.PostAsync(Resources.BaseAddress + "/api/Common"                                        
                                        HttpResponseMessage response = await client.PostAsync(Resources.BaseAddress + "/api/Common?ObjectInfo=" + ObjectInfo + ""
                                                ,form);
                                        IndicatorText = "Подождите, идет загрузка..." + Environment.NewLine + "Загружено " + cnt.ToString() + " фото из " + Photos.Count.ToString();
                                        cnt++;
                                        if(response.StatusCode.ToString() == "OK") {
                                            PathToSaveModel path = JsonConvert.DeserializeObject<PathToSaveModel>(response.Content.ReadAsStringAsync().Result);
                                            Path = path.PathToSave.ToString().Replace("C:\\","\\\\SQL-SERVICE\\");
                                            //Path = path.PathToSave.ToString().Replace("C:\\Services\\","\\\\SQL-SERVICE\\");
                                        }
                                        //Path = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                                        if(response.StatusCode.ToString() != "OK") {
                                            await Application.Current.MainPage.DisplayAlert("Ошибка (Фото не было загружено)",response.Content.ReadAsStringAsync().Result,"OK");
                                            error = true;
                                            Crashes.TrackError(new Exception("Ошибка отправки фото на сервер"),
                                                new Dictionary<string,string> {
                                                    {"Error",response.Content.ReadAsStringAsync().Result },
                                                    {"ErrorPhotoNumber",cnt.ToString() }
                                                });
                                        }
                                    }
                                }
                                if(!error) {
                                    if(Mount != null) {
                                        //App.Database.DeleteMount(Mount.ID);
                                        //Mount.State = 1;
                                        //App.Database.UpdateMount(Mount);
                                        App.Database.DeleteMount(Mount.ID);
                                        Analytics.TrackEvent("Удаление монтажа из локальной базы данных");
                                        var _ntMounts = App.Database.GetMounts().Where(x => x.State == 0 && x.MounterID == Mounters.FirstOrDefault().NewMounterId).ToList();
                                    }
                                }
                            }
                            else {
                                await Application.Current.MainPage.DisplayAlert("Ошибка","Не все обязательные фото были сделаны","OK");
                                Analytics.TrackEvent("Ошибка. Количество фото");
                            }
                        else {
                            await Application.Current.MainPage.DisplayAlert("Ошибка","Адрес объекта не может быть пустым","OK");
                            Analytics.TrackEvent("Ошибка. Адрес объекта");
                        }
                    else {
                        await Application.Current.MainPage.DisplayAlert("Ошибка","Название объекта не может быть пустым","OK");
                        Analytics.TrackEvent("Ошибка. Название объекта");
                    }
                else {
                    await Application.Current.MainPage.DisplayAlert("Ошибка","Номер объекта не может быть пустым","OK");
                    Analytics.TrackEvent("Ошибка. Номер объекта");
                }
                Toast.MakeText(Android.App.Application.Context,"Данные сохранены",ToastLength.Long).Show();
                Analytics.TrackEvent("Отправка монтажа на сервер. Успешно",
                new Dictionary<string,string> {
                    {"Mount",Mount.ObjectNumber}
                });
                Analytics.TrackEvent("Отправка события в андромеду и сохранение пути. Успешно",
                new Dictionary<string,string> {
                    {"Mount",Mount.ObjectNumber}
                });
                using HttpClient clientEvents = new HttpClient(GetHttpClientHandler());
                HttpResponseMessage responseEvents = await clientEvents.GetAsync(Resources.BaseAddress + "/api/Andromeda/SendEvents?ObjectNumber=" + ObjectNumber + "");
                var respEvents = responseEvents.Content.ReadAsStringAsync().Result;
                if(responseEvents.StatusCode.ToString() != "Accepted") {
                    Crashes.TrackError(new Exception("Не получен код 6 от Андромеды"),
                    new Dictionary<string,string> {
                        {"ResponseStatusCode",responseEvents.StatusCode.ToString() }
                    });
                    await Application.Current.MainPage.DisplayAlert("Внимание"
                        ,"От сервера не был получен корректный ответ. Доставка обходного до оператора не может быть гарантирована. Рекомендуется уточнить информацию у оператора по номеру объекта"
                        ,"OK");
                }
                else if(responseEvents.StatusCode.ToString() == "Accepted") {
                    responseEvents = await clientEvents.GetAsync(Resources.BaseAddress + "/api/Andromeda/weblink?ObjectNumber=" + ObjectNumber + "&path=" + Path + "");
                    if(responseEvents.StatusCode.ToString() != "Accepted") {
                        Crashes.TrackError(new Exception("Не удачная попытка записи Web-ссылки в андромеду"),
                        new Dictionary<string,string> {
                        {"ResponseStatusCode",responseEvents.StatusCode.ToString() },
                        {"ResponseError",responseEvents.Content.ReadAsStringAsync().Result }
                        });
                        await Application.Current.MainPage.DisplayAlert("Внимание"
                            ,"Операторы получили уведомление о электронном обходном по объекту, но при попытке записи ссылки на обходной к информации по объекту возникла ошибка, сообщите об этом операторам."
                            ,"OK");
                    }
                    else if(responseEvents.StatusCode.ToString() == "Accepted") {
                        Toast.MakeText(Android.App.Application.Context,"Монтаж отправлен, данные получены оператором",ToastLength.Long).Show();
                        Analytics.TrackEvent("Монтаж отправлен, данные получены оператором");
                    }
                }
                Opacity = 1;
                IndicatorVisible = false;
                //BackPressCommand.Execute(null);
                MountsViewModel vm = new MountsViewModel(Mounters,Servicemans);
                App.Current.MainPage = new MountsPage(vm);
            },obj => Photos.Count >= 5);
        }
        private RelayCommand _SaveToDB;
        public RelayCommand SaveToDB {
            get => _SaveToDB ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Сохранение монтажа перед отправкой на сервер в локальной базе",
                                    new Dictionary<string,string> {
                                        {"ObjectNumber",ObjectNumber }
                                    });
                Opacity = 0.1;
                IndicatorVisible = true;
                if(IsChanged) {
                    if(Photos.Count >= 5) {
                        if(Mount == null) {
                            Mounts mount = new Mounts() {
                                ObjectNumber = ObjectNumber,
                                ObjectName = ObjectName,
                                AddressName = ObjectAddress,
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
                            App.Database.SaveMount(mount);
                            Analytics.TrackEvent("Монтаж сохранен в локальной базе");
                            Toast.MakeText(Android.App.Application.Context,"Данные сохранены",ToastLength.Long).Show();
                        }
                        else {
                            Mount.State = 0;
                            Mount.ObjectNumber = ObjectNumber;
                            Mount.ObjectName = ObjectName;
                            Mount.AddressName = ObjectAddress;
                            Mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
                            Mount.Driveways = ObjectDriveways;
                            Mount.ObjectCard = Photos.Any(x => x._Types.PhotoTypeName == "Карточка объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Карточка объекта").Data : "";
                            Mount.ObjectScheme = Photos.Any(x => x._Types.PhotoTypeName == "Схема объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Схема объекта").Data : "";
                            Mount.ObjectWiring = Photos.Any(x => x._Types.PhotoTypeName == "Расшлейфовка объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Расшлейфовка объекта").Data : "";
                            Mount.ObjectListResponsible = Photos.Any(x => x._Types.PhotoTypeName == "Ответственные объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Ответственные объекта").Data : "";
                            Mount.ObjectSignboard = Photos.Any(x => x._Types.PhotoTypeName == "Вывеска объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Вывеска объекта").Data : "";
                            //Mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
                            //App.Database.UpdateMount(Mount);
                            App.Database.SaveUpdateMount(Mount);
                            Analytics.TrackEvent("Монтаж обновлен в локальной базе");
                            Toast.MakeText(Android.App.Application.Context,"Данные сохранены",ToastLength.Long).Show();
                        }
                    }
                    else {
                        bool result = await Application.Current.MainPage.DisplayAlert("Ошибка","Не все обязательные фото были сделаны. Продолжить сохранение?","Да","Нет");
                        if(result) {
                            Mounts mount = new Mounts() {
                                ObjectNumber = ObjectNumber,
                                ObjectName = ObjectName,
                                AddressName = ObjectAddress,
                                ID = App.Database.GetCurrentID(),
                                MounterID = Mounters.FirstOrDefault().NewMounterId,
                                Driveways = ObjectDriveways,
                                State = 0,
                                ObjectCard = Photos.Any(x => x._Types.PhotoTypeName == "Карточка объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Карточка объекта").Data : "",
                                ObjectScheme = Photos.Any(x => x._Types.PhotoTypeName == "Схема объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Схема объекта").Data : "",
                                ObjectWiring = Photos.Any(x => x._Types.PhotoTypeName == "Расшлейфовка объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Расшлейфовка объекта").Data : "",
                                ObjectListResponsible = Photos.Any(x => x._Types.PhotoTypeName == "Ответственные объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Ответственные объекта").Data : "",
                                ObjectSignboard = Photos.Any(x => x._Types.PhotoTypeName == "Вывеска объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Вывеска объекта").Data : ""
                            };
                            App.Database.SaveMount(mount);
                            Analytics.TrackEvent("Монтаж сохранен в локальной базе");
                            Toast.MakeText(Android.App.Application.Context,"Данные сохранены",ToastLength.Long).Show();
                        }
                    }
                }
                MountsViewModel vm = new MountsViewModel(Mounters,Servicemans);
                App.Current.MainPage = new MountsPage(vm);
                IndicatorVisible = false;
            }/*,obj => Photos.Count >= 5*/);
        }

        private List<NewServicemanExtensionBase> _Servicemans;
        public List<NewServicemanExtensionBase> Servicemans {
            get => _Servicemans;
            set {
                _Servicemans = value;
                OnPropertyChanged(nameof(Servicemans));
            }
        }
        //private RelayCommand _AddNewPhotoCommand;
        //public RelayCommand AddNewPhotoCommand {
        //    get => _AddNewPhotoCommand ??= new RelayCommand(async obj => {
        //        if(PhotoName != null) {
        //            if(File != null) {
        //                string tmp = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
        //                if(!Photos.Any(y => y.Data == tmp)) {
        //                    Photos.Add(new PhotoCollection(
        //                        Guid.NewGuid(),
        //                        Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path)),
        //                        PhotoComment,
        //                            ImageSource.FromStream(() => {
        //                                var stream = File.GetStream();
        //                                return stream;
        //                            }),
        //                            PhotoName
        //                        ));
        //                    IsChanged = true;
        //                    PhotoSource.Add(ImgSrc);
        //                    ImgSrc = "EmptyPhoto.png";
        //                    SelectedPhoto = null;
        //                    PhotoNames.Remove(PhotoName);
        //                }
        //                else
        //                    await Application.Current.MainPage.DisplayAlert("Ошибка","Такая фотография уже была загружена","OK");
        //            }
        //        }
        //        else
        //            await Application.Current.MainPage.DisplayAlert("Ошибка","Выберите тип фотографии","OK");
        //    });
        //}
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                SaveToDB.Execute(null);
                //MountsViewModel vm = new MountsViewModel(Mounters);
                //App.Current.MainPage = new MountsPage(vm);
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
                //if(IsPickPhoto.HasValue) {
                //    await CrossMedia.Current.Initialize();
                //    if(!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {
                //        await Application.Current.MainPage.DisplayAlert("No Camera",":( No camera available.","OK");
                //        return;
                //    }
                //    if(IsPickPhoto.Value == true) {
                //        File = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions {
                //            //Directory = "Sample",
                //            //Name = "test.jpg",
                //            PhotoSize = PhotoSize.Small,
                //            CompressionQuality = 50
                //        });
                //    }
                //    else {
                //        File = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions {
                //            Directory = "Sample",
                //            Name = "test.jpg",
                //            PhotoSize = PhotoSize.Small,
                //            CompressionQuality = 50
                //        });
                //    }
                //    if(File == null)
                //        return;

                //    ImgSrc = ImageSource.FromStream(() => {
                //        var stream = File.GetStream();
                //        return stream;
                //    });
                //}
                //else {
                Analytics.TrackEvent("Сохарянем данные по монтажу в памяти приложения, и переходим к добавлению фото, чтобы после не вбивать данные заного",
                new Dictionary<string,string> {
                    {"MounterPhone",Mounters.FirstOrDefault().NewPhone },
                    {"ObjectNumber",ObjectNumber }
                });
                if(Mount == null)
                    Mount = new Mounts();
                Mount.ObjectNumber = ObjectNumber;
                Mount.ObjectName = ObjectName;
                Mount.AddressName = ObjectAddress;
                Mount.Driveways = ObjectDriveways;
                Mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
                SelectActionsPopupPageViewModel vm = new SelectActionsPopupPageViewModel(Mount,Mounters);
                await App.Current.MainPage.Navigation.PushPopupAsync(new SelectActionsPopupPage(vm));
                //}




                //File = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions {
                //    Directory = "Sample",
                //    Name = "test.jpg",
                //    PhotoSize=PhotoSize.Small,
                //    CompressionQuality=50
                //});
                //File = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions {
                //    //Directory = "Sample",
                //    //Name = "test.jpg",
                //    PhotoSize = PhotoSize.Small,
                //    CompressionQuality = 50
                //});

                //if(File == null)
                //    return;

                //ImgSrc = ImageSource.FromStream(() => {
                //    var stream = File.GetStream();
                //    return stream;
                //});
            });
        }

        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand {
            get => _DeleteCommand ??= new RelayCommand(async obj => {
                if(SelectedPhoto != null) {
                    //string result = await Application.Current.MainPage.DisplayPromptAsync("Удаление","Вы действительно хотите удалить выбранную строку? Напишите +, для удаления в поле ниже.");
                    bool result = await Application.Current.MainPage.DisplayAlert("Удаление","Подтвердите удаление","Удалить","Отмена");
                    //if(!string.IsNullOrEmpty(result))
                    //    if(result.Equals("+")) {
                    if(result) {
                        Photos.Remove(SelectedPhoto);
                        PhotoNames.Add(SelectedPhoto._Types);
                        Analytics.TrackEvent("Удаление фото");
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
                if(_ObjectNumber != value)
                    if(_ObjectNumber != null)
                        IsChanged = true;
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
                if(_ObjectName != value)
                    if(_ObjectName != null)
                        IsChanged = true;
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
                if(_ObjectAddress != value)
                    if(_ObjectAddress != null)
                        IsChanged = true;
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
                if(_ObjectDriveways != value)
                    if(_ObjectDriveways != null)
                        IsChanged = true;
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
                if(_Photos != value)
                    if(_Photos != null)
                        IsChanged = true;
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
