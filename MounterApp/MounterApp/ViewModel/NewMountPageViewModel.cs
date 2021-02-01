using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Views;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using Android.Widget;
using Microsoft.AppCenter.Analytics;
using Xamarin.Essentials;
using System.Net;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using MounterApp.Properties;

namespace MounterApp.ViewModel {
    public class NewMountPageViewModel : BaseViewModel {
        //readonly ClientHttp http = new ClientHttp();
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

        private RelayCommand _FillPhotoNames;
        public RelayCommand FillPhotoNames {
            get => _FillPhotoNames ??= new RelayCommand(async obj => {
                //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Карточка объекта" });
                //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Схема объекта" });
                //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Расшлейфовка объекта" });
                //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Ответственные объекта" });
                //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Вывеска объекта" });
                //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Доп. фото" });

                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Вывеска объекта" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Обходной лист" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Расшлейфовка объекта" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Ответственные объекта" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Акт технич. сост-я 1" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Акт технич. сост-я 2" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Схема объекта" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Доп. фото" });
            });
        }
        public NewMountPageViewModel(List<NewMounterExtensionBase> mounters,List<NewServicemanExtensionBase> servicemans) {
            Servicemans = servicemans;
            Mounters = mounters;
            //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Карточка объекта" });
            //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Схема объекта" });
            //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Расшлейфовка объекта" });
            //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Ответственные объекта" });
            //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Вывеска объекта" });
            //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Доп. фото" });
            FillPhotoNames.Execute(null);
            ImgSrc = "EmptyPhoto.png";
            PhotoImage = IconName("media");
            SaveImage = IconName("save");
            SendImage = IconName("send");
            Opacity = 1;
            IndicatorVisible = false;
            IsPickPhoto = null;
            Analytics.TrackEvent("Инициализация страницы заполнения нового монтажа",
                        new Dictionary<string,string> {
                            {"MounterPhone",Mounters.FirstOrDefault().NewPhone }
                        });
            //var mainDisplayInfo = DeviceDisplay.MainDisplayInfo.Height;
            //var height = mainDisplayInfo.Height;
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        }
        public NewMountPageViewModel(Mounts mount,List<NewMounterExtensionBase> mounters,bool _IsChanged,List<NewServicemanExtensionBase> servicemans) {
            Servicemans = servicemans;
            //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Карточка объекта" });
            //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Схема объекта" });
            //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Расшлейфовка объекта" });
            //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Ответственные объекта" });
            //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Вывеска объекта" });
            //PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(),PhotoTypeName = "Доп. фото" });
            FillPhotoNames.Execute(null);
            ImgSrc = "EmptyPhoto.png";
            PhotoImage = IconName("media");
            SaveImage = IconName("save");
            SendImage = IconName("send");
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
                    ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectCard)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Обходной лист")));
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
            if(!string.IsNullOrEmpty(Mount.ObjectSignboard))
                Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectSignboard,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectActTech1)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Акт технич. сост-я 1")));
            if(!string.IsNullOrEmpty(Mount.ObjectSignboard))
                Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectSignboard,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectActTech2)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Акт технич. сост-я 2")));
            if(!string.IsNullOrEmpty(Mount.ObjectExtra1))
                Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectExtra1,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectExtra1)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Доп. фото")));
            if(!string.IsNullOrEmpty(Mount.ObjectExtra2))
                Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectExtra2,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectExtra2)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Доп. фото")));
            if(!string.IsNullOrEmpty(Mount.ObjectExtra3))
                Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectExtra3,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectExtra3)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Доп. фото")));
            if(!string.IsNullOrEmpty(Mount.ObjectExtra4))
                Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectExtra4,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectExtra4)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Доп. фото")));
            if(!string.IsNullOrEmpty(Mount.ObjectExtra5))
                Photos.Add(
                new PhotoCollection(Guid.NewGuid(),Mount.ObjectExtra5,null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(Mount.ObjectExtra5)); }),PhotoNames.FirstOrDefault(x => x.PhotoTypeName == "Доп. фото")));
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

        private List<NewServicemanExtensionBase> _Servicemans;
        public List<NewServicemanExtensionBase> Servicemans {
            get => _Servicemans;
            set {
                _Servicemans = value;
                OnPropertyChanged(nameof(Servicemans));
            }
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
                            if(Photos.Count >= 7) {
                                //TODO: проверять, что это обязательные фото, а не просто количество
                                Analytics.TrackEvent("Отправка нового монтажа на сервер",
                                    new Dictionary<string,string> {
                                        {"ObjectNumber",ObjectNumber }
                                    });
                                Opacity = 0.1;
                                IndicatorVisible = true;
                                IndicatorText = "Подождите, идет загрузка...";
                                ProgressValue = 0.1;
                                Analytics.TrackEvent("Сохранение монтажа перед отправкой на сервер в локальной базе",
                                    new Dictionary<string,string> {
                                        {"ObjectNumber",ObjectNumber }
                                    });
                                SaveToDB.Execute(null);
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine("Номер объекта: " + ObjectNumber);
                                sb.AppendLine("Наименование объекта: " + ObjectName);
                                sb.AppendLine("Адрес объекта: " + ObjectAddress);
                                sb.AppendLine("Монтажник: " + Mounters.FirstOrDefault().NewName);
                                sb.AppendLine("Подъездные пути: " + ObjectDriveways);

                                bool error = false;
                                int cnt = 1;
                                foreach(PhotoCollection ph in Photos.Where(x => x.Data != null)) {
                                    using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                                        client.Timeout = TimeSpan.FromMinutes(10);
                                        MultipartFormDataContent form = new MultipartFormDataContent();
                                        //client.DefaultRequestHeaders.Clear();
                                        //client.DefaultRequestHeaders.ConnectionClose = true;
                                        //client.DefaultRequestHeaders.ExpectContinue = false;
                                        form.Add(new StreamContent(new MemoryStream(Convert.FromBase64String(ph.Data)))
                                            ,String.Format("file"),String.Format(ObjectNumber + "_" + ph._Types.PhotoTypeName + "_" + ph.ID.ToString() + ".jpeg"));
                                        HttpResponseMessage response = await client.PostAsync(Resources.BaseAddress + "/api/Common?NumberObject=" + ObjectNumber +
                                            "&NameObject=" + ObjectName +
                                            "&AddressObject=" + ObjectAddress +
                                            "&MounterName=" + Mounters.FirstOrDefault().NewName +
                                            "&Driveways=" + ObjectDriveways
                                            ,form);
                                        IndicatorText = "Подождите, идет загрузка..." + Environment.NewLine + "Загружено " + cnt.ToString() + " фото из " + Photos.Count.ToString();
                                        cnt++;
                                        if(response.StatusCode.ToString() == "OK") {
                                            PathToSaveModel path = JsonConvert.DeserializeObject<PathToSaveModel>(response.Content.ReadAsStringAsync().Result);
                                            Path = path.PathToSave.ToString().Replace("C:\\","\\\\SQL-SERVICE\\");
                                        }
                                        if(response.StatusCode.ToString() != "OK") {
                                            //await Application.Current.MainPage.DisplayAlert("Ошибка (Фото не было загружено)",response.Content.ReadAsStringAsync().Result,"OK");
                                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Ошибка при загрузке фото",Color.Red,LayoutOptions.EndAndExpand),4000));
                                            error = true;
                                            Crashes.TrackError(new Exception("Ошибка отправки фото на сервер"),
                                                new Dictionary<string,string> {
                                                    {"Error",response.Content.ReadAsStringAsync().Result },
                                                    {"ErrorResponse",response.ToString() },
                                                    {"ErrorPhotoNumber",cnt.ToString() }
                                                });
                                        }
                                    }
                                }
                                //HttpStatusCode result = await ClientHttp.PostQuery("/api/Common?NumberObject=" + ObjectNumber +
                                //            "&NameObject=" + ObjectName +
                                //            "&AddressObject=" + ObjectAddress +
                                //            "&MounterName=" + Mounters.FirstOrDefault().NewName +
                                //            "&Driveways=" + ObjectDriveways
                                //            ,form);
                                if(!error) {
                                    //if(result.Equals(HttpStatusCode.OK)) { 
                                    if(Mount != null) {
                                        Mount.State = 1;
                                        Mount.DateSended = DateTime.Now;
                                        App.Database.UpdateMount(Mount);
                                        Analytics.TrackEvent("Установка статуса монтажа в локальной базе данных: ОТПРАВЛЕНО",
                                        new Dictionary<string,string> {
                                            {"ObjectNumber",Mount.ObjectNumber.ToString() },
                                            {"DateSended",Mount.DateSended.ToString() }
                                        });
                                        //Toast.MakeText(Android.App.Application.Context,"Данные сохранены",ToastLength.Long).Show();
                                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Сохранение в локальной базе. Успешно.",Color.Green,LayoutOptions.EndAndExpand),4000));
                                        Analytics.TrackEvent("Отправка монтажа на сервер. Успешно",
                                            new Dictionary<string,string> { { "Mount",Mount.ObjectNumber } });
                                        IndicatorText = "Подождите, идет загрузка..." + Environment.NewLine + "Отправка уведомления для операторов";
                                        Analytics.TrackEvent("Отправка события в андромеду",
                                            new Dictionary<string,string> { { "Mount",Mount.ObjectNumber } });
                                        SendEventsToAndromeda.Execute(null);
                                        Analytics.TrackEvent("Запись web-ссылки в Андромеду",
                                            new Dictionary<string,string> { { "Mount",Mount.ObjectNumber } });
                                        IndicatorText = "Подождите, идет загрузка..." + Environment.NewLine + "Запись ссылки на электронный обходной";
                                        WriteWebLink.Execute(null);
                                        WriteCoordinates.Execute(null);
                                        WriteDriveways.Execute(null);
                                        if(IsSuccessSendEvents && IsSuccessWriteWebLink)
                                            //Toast.MakeText(Android.App.Application.Context,"Монтаж отправлен, данные получены оператором",ToastLength.Long).Show();
                                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Монтаж отправлен, данные получены оператором",Color.Green,LayoutOptions.EndAndExpand),4000));
                                        else {
                                            //Toast.MakeText(Android.App.Application.Context,"Монтаж отправлен, данные получены оператором",ToastLength.Long).Show();
                                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Монтаж отправлен, данные получены оператором",Color.Green,LayoutOptions.EndAndExpand),4000));
                                            Analytics.TrackEvent("Ошибка при отправке монтажа, не получилось записать ссылку, координаты, маршрут или отправить событие",
                                            new Dictionary<string,string> {
                                                {"IsSuccessSendEvents",IsSuccessSendEvents.ToString() },
                                                {"IsSuccessWriteWebLink",IsSuccessWriteWebLink.ToString() },
                                                {"ObjectNumber",ObjectNumber.ToString() },
                                                {"MounterPhone",Mounters.FirstOrDefault().NewPhone }
                                            });
                                        }
                                        MountsViewModel vm = new MountsViewModel(Mounters,Servicemans);
                                        App.Current.MainPage = new MountsPage(vm);
                                    }
                                }
                            }
                            else {
                                //await Application.Current.MainPage.DisplayAlert("Ошибка","Не все обязательные фото были сделаны","OK");
                                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Не все обязательные фото были сделаны",Color.Red,LayoutOptions.EndAndExpand),4000));
                                Analytics.TrackEvent("Ошибка. Количество фото");
                            }
                        else {
                            //await Application.Current.MainPage.DisplayAlert("Ошибка","Адрес объекта не может быть пустым","OK");
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Адрес объекта не может быть пустым",Color.Red,LayoutOptions.EndAndExpand),4000));
                            Analytics.TrackEvent("Ошибка. Адрес объекта");
                        }
                    else {
                        //await Application.Current.MainPage.DisplayAlert("Ошибка","Название объекта не может быть пустым","OK");
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Название объекта не может быть пустым",Color.Red,LayoutOptions.EndAndExpand),4000));
                        Analytics.TrackEvent("Ошибка. Название объекта");
                    }
                else {
                    //await Application.Current.MainPage.DisplayAlert("Ошибка","Номер объекта не может быть пустым","OK");
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Номер объекта не может быть пустым",Color.Red,LayoutOptions.EndAndExpand),4000));
                    Analytics.TrackEvent("Ошибка. Номер объекта");
                }
                Opacity = 1;
                IndicatorVisible = false;
            },obj => Photos.Count >= 7);
        }

        private RelayCommand _WriteCoordinates;
        public RelayCommand WriteCoordinates {
            get => _WriteCoordinates ??= new RelayCommand(async obj => {
                //await ClientHttp.GetQuery("/api/Andromeda/coords?ObjectNumber=" + ObjectNumber + "&ObjectAddress=" + ObjectAddress + "");

                using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    HttpResponseMessage responseCoords = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/coords?ObjectNumber=" + ObjectNumber + "&ObjectAddress=" + ObjectAddress + "");
                    if(responseCoords.IsSuccessStatusCode || responseCoords.StatusCode == HttpStatusCode.Accepted) { }
                    else {
                        Crashes.TrackError(new Exception("Не удачная попытка записи координат в андромеду"),
                            new Dictionary<string,string> {
                                    {"ResponseStatusCode",responseCoords.StatusCode.ToString() },
                                    {"ResponseError",responseCoords.Content.ReadAsStringAsync().Result },
                                    {"Response",responseCoords.ToString() },
                                    {"ObjectNumber",ObjectNumber },
                                    {"ObjectAddress",ObjectAddress }
                            });
                    }
                }
            });
        }

        private RelayCommand _WriteDriveways;
        public RelayCommand WriteDriveways {
            get => _WriteDriveways ??= new RelayCommand(async obj => {
                //await ClientHttp.GetQuery("/api/Andromeda/driveways?ObjectNumber=" + ObjectNumber + "&ObjectDriveways=" + ObjectDriveways + "");
                using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    HttpResponseMessage responseCoords = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/driveways?ObjectNumber=" + ObjectNumber + "&ObjectDriveways=" + ObjectDriveways + "");
                    if(responseCoords.IsSuccessStatusCode || responseCoords.StatusCode == HttpStatusCode.Accepted) { }
                    else {
                        Crashes.TrackError(new Exception("Не удачная попытка записи подъездных путей в андромеду"),
                            new Dictionary<string,string> {
                                    {"ResponseStatusCode",responseCoords.StatusCode.ToString() },
                                    {"ResponseError",responseCoords.Content.ReadAsStringAsync().Result },
                                    {"Response",responseCoords.ToString() },
                                    {"ObjectNumber",ObjectNumber },
                                    {"ObjectDriveways",ObjectDriveways }
                            });
                    }
                }
            });
        }

        private RelayCommand _WriteWebLink;
        public RelayCommand WriteWebLink {
            get => _WriteWebLink ??= new RelayCommand(async obj => {
                //HttpStatusCode code = await ClientHttp.GetQuery("/api/Andromeda/weblink?ObjectNumber=" + ObjectNumber + "&path=" + Path + "");
                //IsSuccessWriteWebLink = code.Equals(HttpStatusCode.Accepted);
                IsSuccessWriteWebLink = false;
                using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    HttpResponseMessage responseweblink = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/weblink?ObjectNumber=" + ObjectNumber + "&path=" + Path + "");
                    if(responseweblink.StatusCode.ToString() != "Accepted") {
                        IsSuccessWriteWebLink = false;
                        Crashes.TrackError(new Exception("Не удачная попытка записи Web-ссылки в андромеду"),
                            new Dictionary<string,string> {
                                    {"ResponseStatusCode",responseweblink.StatusCode.ToString() },
                                    {"ResponseError",responseweblink.Content.ReadAsStringAsync().Result },
                                    {"Response",responseweblink.ToString() },
                                    {"ObjectNumber",ObjectNumber },
                                    {"Path",Path }
                            });
                    }
                    else if(responseweblink.IsSuccessStatusCode || responseweblink.StatusCode.ToString() == "Accepted") {
                        IsSuccessWriteWebLink = true;
                        Analytics.TrackEvent("Монтаж отправлен, данные получены оператором",
                            new Dictionary<string,string> {
                            {"ObjectNumber",ObjectNumber },
                            {"Date",DateTime.Now.ToString() }
                        });
                    }
                }
            });
        }
        private RelayCommand _SendEventsToAndromeda;
        public RelayCommand SendEventsToAndromeda {
            get => _SendEventsToAndromeda ??= new RelayCommand(async obj => {
                //HttpStatusCode code = await ClientHttp.GetQuery("/api/Andromeda/SendEvents?ObjectNumber=" + ObjectNumber + "");
                //IsSuccessSendEvents = code.Equals(HttpStatusCode.Accepted);

                IsSuccessSendEvents = false;
                using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    HttpResponseMessage responseEvents = await client.GetAsync(Resources.BaseAddress + "/api/Andromeda/SendEvents?ObjectNumber=" + ObjectNumber + "");
                    if(responseEvents.IsSuccessStatusCode || responseEvents.StatusCode.ToString() == "Accepted") {
                        Analytics.TrackEvent("Получен код 6 от Андромеды",
                        new Dictionary<string,string> {
                            {"ObjectNumber",ObjectNumber },
                            {"Date",DateTime.Now.ToString() }
                        });
                        IsSuccessSendEvents = true;
                    }
                    else {
                        Crashes.TrackError(new Exception("Не получен код 6 от Андромеды"),
                        new Dictionary<string,string> {
                            { "ResponseStatusCode",responseEvents.StatusCode.ToString() }
                        });
                        IsSuccessSendEvents = false;
                        //await Application.Current.MainPage.DisplayAlert("Внимание"
                        //    ,"От сервера не был получен корректный ответ. Доставка обходного до оператора не может быть гарантирована. Рекомендуется уточнить информацию у оператора по номеру объекта"
                        //    ,"OK");
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("От сервера не был получен корректный ответ. Доставка обходного до оператора не может быть гарантирована.",Color.Red,LayoutOptions.EndAndExpand),4000));
                    }
                }
            });
        }
        private RelayCommand _SaveToDB;
        public RelayCommand SaveToDB {
            get => _SaveToDB ??= new RelayCommand(async obj => {
                Analytics.TrackEvent("Сохранение монтажа перед отправкой на сервер в локальной базе",
                                    new Dictionary<string,string> {
                                        {"ObjectNumber",ObjectNumber }
                                    });
                //Opacity = 0.1;
                //IndicatorVisible = true;
                //if(IsChanged) {
                if(Photos.Count >= 5) {
                    if(Mount == null) {
                        Mounts mount = new Mounts();
                        mount.ObjectNumber = ObjectNumber;
                        mount.ObjectName = ObjectName;
                        mount.AddressName = ObjectAddress;
                        mount.ID = App.Database.GetCurrentID();
                        mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
                        mount.Driveways = ObjectDriveways;
                        mount.State = 0;
                        mount.DateTimeCreated = DateTime.Now;
                        mount.ObjectCard = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Обходной лист").Data;
                        mount.ObjectScheme = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Схема объекта").Data;
                        mount.ObjectWiring = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Расшлейфовка объекта").Data;
                        mount.ObjectListResponsible = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Ответственные объекта").Data;
                        mount.ObjectSignboard = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Вывеска объекта").Data;
                        mount.ObjectActTech1 = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 1").Data;
                        mount.ObjectActTech2 = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 2").Data;
                        foreach(var item in Photos.Where(x => x._Types.PhotoTypeName == "Доп. фото" && x.IsUse == false)) {
                            if(mount.ObjectExtra1 == null)
                                mount.ObjectExtra1 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            else if(mount.ObjectExtra2 == null)
                                mount.ObjectExtra2 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            else if(mount.ObjectExtra3 == null)
                                mount.ObjectExtra3 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            else if(mount.ObjectExtra4 == null)
                                mount.ObjectExtra4 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            else if(mount.ObjectExtra5 == null)
                                mount.ObjectExtra5 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            Photos.FirstOrDefault(x => x.ID == item.ID).IsUse = true;
                        }
                        App.Database.SaveMount(mount);
                        Analytics.TrackEvent("Монтаж сохранен в локальной базе");
                        //Toast.MakeText(Android.App.Application.Context,"Данные сохранены",ToastLength.Long).Show(); 
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Сохранение в локальной базе. Успешно.",Color.Green,LayoutOptions.EndAndExpand),4000));
                    }
                    else {
                        Mount.State = 0;
                        Mount.ObjectNumber = ObjectNumber;
                        Mount.ObjectName = ObjectName;
                        Mount.AddressName = ObjectAddress;
                        Mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
                        Mount.Driveways = ObjectDriveways;
                        Mount.DateTimeCreated = Mount.DateTimeCreated.HasValue ? Mount.DateTimeCreated : DateTime.Now;
                        Mount.ObjectCard = Photos.Any(x => x._Types.PhotoTypeName == "Обходной лист") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Обходной лист").Data : "";
                        Mount.ObjectScheme = Photos.Any(x => x._Types.PhotoTypeName == "Схема объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Схема объекта").Data : "";
                        Mount.ObjectWiring = Photos.Any(x => x._Types.PhotoTypeName == "Расшлейфовка объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Расшлейфовка объекта").Data : "";
                        Mount.ObjectListResponsible = Photos.Any(x => x._Types.PhotoTypeName == "Ответственные объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Ответственные объекта").Data : "";
                        Mount.ObjectSignboard = Photos.Any(x => x._Types.PhotoTypeName == "Вывеска объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Вывеска объекта").Data : "";
                        Mount.ObjectActTech1 = Photos.Any(x => x._Types.PhotoTypeName == "Акт технич. сост-я 1") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 1").Data : "";
                        Mount.ObjectActTech2 = Photos.Any(x => x._Types.PhotoTypeName == "Акт технич. сост-я 2") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 2").Data : "";
                        foreach(var item in Photos.Where(x => x._Types.PhotoTypeName == "Доп. фото" && x.IsUse == false)) {
                            if(Mount.ObjectExtra1 == null)
                                Mount.ObjectExtra1 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            else if(Mount.ObjectExtra2 == null)
                                Mount.ObjectExtra2 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            else if(Mount.ObjectExtra3 == null)
                                Mount.ObjectExtra3 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            else if(Mount.ObjectExtra4 == null)
                                Mount.ObjectExtra4 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            else if(Mount.ObjectExtra5 == null)
                                Mount.ObjectExtra5 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            Photos.FirstOrDefault(x => x.ID == item.ID).IsUse = true;
                        }
                        App.Database.SaveUpdateMount(Mount);
                        Analytics.TrackEvent("Монтаж обновлен в локальной базе");
                        //Toast.MakeText(Android.App.Application.Context,"Данные сохранены",ToastLength.Long).Show();
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Сохранение в локальной базе. Успешно.",Color.Green,LayoutOptions.EndAndExpand),4000));
                    }
                }
                else {
                    bool result = await Application.Current.MainPage.DisplayAlert("Ошибка","Не все обязательные фото были сделаны. Продолжить сохранение?","Да","Нет");
                    if(result) {
                        Mounts mount = new Mounts();
                        mount.ObjectNumber = ObjectNumber;
                        mount.ObjectName = ObjectName;
                        mount.AddressName = ObjectAddress;
                        mount.ID = App.Database.GetCurrentID();
                        mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
                        mount.Driveways = ObjectDriveways;
                        mount.State = 0;
                        mount.DateTimeCreated = DateTime.Now;
                        mount.ObjectCard = Photos.Any(x => x._Types.PhotoTypeName == "Обходной лист") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Обходной лист").Data : "";
                        mount.ObjectScheme = Photos.Any(x => x._Types.PhotoTypeName == "Схема объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Схема объекта").Data : "";
                        mount.ObjectWiring = Photos.Any(x => x._Types.PhotoTypeName == "Расшлейфовка объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Расшлейфовка объекта").Data : "";
                        mount.ObjectListResponsible = Photos.Any(x => x._Types.PhotoTypeName == "Ответственные объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Ответственные объекта").Data : "";
                        mount.ObjectSignboard = Photos.Any(x => x._Types.PhotoTypeName == "Вывеска объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Вывеска объекта").Data : "";
                        mount.ObjectActTech1 = Photos.Any(x => x._Types.PhotoTypeName == "Акт технич. сост-я 1") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 1").Data : "";
                        mount.ObjectActTech2 = Photos.Any(x => x._Types.PhotoTypeName == "Акт технич. сост-я 2") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 2").Data : "";
                        foreach(var item in Photos.Where(x => x._Types.PhotoTypeName == "Доп. фото" && x.IsUse == false)) {
                            if(mount.ObjectExtra1 == null)
                                mount.ObjectExtra1 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            else if(mount.ObjectExtra2 == null)
                                mount.ObjectExtra2 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            else if(mount.ObjectExtra3 == null)
                                mount.ObjectExtra3 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            else if(mount.ObjectExtra4 == null)
                                mount.ObjectExtra4 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            else if(mount.ObjectExtra5 == null)
                                mount.ObjectExtra5 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                            Photos.FirstOrDefault(x => x.ID == item.ID).IsUse = true;
                        }
                        App.Database.SaveMount(mount);
                        Analytics.TrackEvent("Монтаж сохранен в локальной базе");
                        //Toast.MakeText(Android.App.Application.Context,"Данные сохранены",ToastLength.Long).Show();
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Сохранение в локальной базе. Успешно.",Color.Green,LayoutOptions.EndAndExpand),4000));
                    }
                }
                //}
                //MountsViewModel vm = new MountsViewModel(Mounters,Servicemans);
                //App.Current.MainPage = new MountsPage(vm);
                //IndicatorVisible = false;
            });
        }

        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                SaveToDB.Execute(null);
                MountsViewModel vm = new MountsViewModel(Mounters,Servicemans);
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
                Analytics.TrackEvent("Сохраняем данные по монтажу в памяти приложения, и переходим к добавлению фото, чтобы после не вбивать данные заного",
                new Dictionary<string,string> {
                    {"MounterPhone",Mounters.FirstOrDefault().NewPhone },
                    {"ObjectNumber",ObjectNumber }
                });
                //SaveToDB.Execute(null);
                if(Mount == null)
                    Mount = new Mounts();
                Mount.ObjectNumber = ObjectNumber;
                Mount.ObjectName = ObjectName;
                Mount.AddressName = ObjectAddress;
                Mount.Driveways = ObjectDriveways;
                Mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
                SelectActionsPopupPageViewModel vm = new SelectActionsPopupPageViewModel(Mount,Mounters,Servicemans);
                await App.Current.MainPage.Navigation.PushPopupAsync(new SelectActionsPopupPage(vm));
            });
        }

        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand {
            get => _DeleteCommand ??= new RelayCommand(async obj => {
                if(SelectedPhoto != null) {
                    bool result = await Application.Current.MainPage.DisplayAlert("Удаление","Подтвердите удаление","Удалить","Отмена");
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


        private bool _IsSuccessSendEvents;
        public bool IsSuccessSendEvents {
            get => _IsSuccessSendEvents;
            set {
                _IsSuccessSendEvents = value;
                OnPropertyChanged(nameof(IsSuccessSendEvents));
            }
        }

        private bool _IsSuccessWriteWebLink;
        public bool IsSuccessWriteWebLink {
            get => _IsSuccessWriteWebLink;
            set {
                _IsSuccessWriteWebLink = value;
                OnPropertyChanged(nameof(IsSuccessWriteWebLink));
            }
        }

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
