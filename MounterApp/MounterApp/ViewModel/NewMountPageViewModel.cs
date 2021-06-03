using MounterApp.Helpers;
using MounterApp.InternalModel;
using MounterApp.Model;
using MounterApp.Views;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class NewMountPageViewModel : BaseViewModel {        
        public NewMountPageViewModel() {
        }
        /// <summary>
        /// Конструктор окна создания монтажа
        /// </summary>
        /// <param name="mounters">Список монтажников</param>
        /// <param name="servicemans">Список техников</param>
        public NewMountPageViewModel(List<NewMounterExtensionBase> mounters, List<NewServicemanExtensionBase> servicemans) {
            Servicemans = servicemans;
            Mounters = mounters;
            FillPhotoNames.Execute(null);
            SetIcons.Execute(null);
            Opacity = 1;
            IndicatorVisible = false;
            //IsPickPhoto = null;
            SaveFlag = false;
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        }
        /// <summary>
        /// Конструктор окна создания монтажа
        /// </summary>
        /// <param name="mounters">Список монтажников</param>
        /// <param name="servicemans">Список техников</param>
        public NewMountPageViewModel(Mounts mount, List<NewMounterExtensionBase> mounters, bool _IsChanged, List<NewServicemanExtensionBase> servicemans) {
            Servicemans = servicemans;
            FillPhotoNames.Execute(null);
            SetIcons.Execute(null);
            //IsPickPhoto = null;
            if (!string.IsNullOrEmpty(mount.GoogleComment)) {
                GoogleComment = mount.GoogleComment;
                VisibleGoogleComment = true;
            }

            Mounters = mounters;
            FillMount.Execute(mount);

            Opacity = 1;
            IndicatorVisible = false;
            IsChanged = _IsChanged;
            SaveFlag = false;
            App.Current.MainPage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
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
        /// Заполняем коллекцию списком  типов фотографий
        /// TODO: вынести список названий фотографий в отдельную сущность CRM иил параметры приложения и запрашивать с backend-а
        /// </summary>
        private RelayCommand _FillPhotoNames;
        public RelayCommand FillPhotoNames {
            get => _FillPhotoNames ??= new RelayCommand(async obj => {
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Вывеска объекта" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Обходной лист" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Расшлейфовка объекта" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Ответственные объекта" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Акт технич. сост-я 1" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Акт технич. сост-я 2" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Схема объекта" });
                PhotoNames.Add(new PhotoTypes() { PhotoTypeId = Guid.NewGuid(), PhotoTypeName = "Доп. фото" });
            });
        }        
        /// <summary>
        /// Установка иконок на кнопки и Image
        /// </summary>
        private RelayCommand _SetIcons;
        public RelayCommand SetIcons {
            get => _SetIcons ??= new RelayCommand(async obj => {
                ImgSrc = "EmptyPhoto.png";
                PhotoImage = IconName("media");
                SaveImage = IconName("save");
                SendImage = IconName("send");
            });
        }
        /// <summary>
        /// Команда заполнения монтажа, который был передан в конструктор
        /// </summary>
        private RelayCommand _FillMount;
        public RelayCommand FillMount {
            get => _FillMount ??= new RelayCommand(async obj => {
                if (obj is null) 
                    return;
                if (!(obj is Mounts mount))
                    return;

                Mount = mount;
                Mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
                ObjectNumber = Mount.ObjectNumber;
                ObjectAddress = Mount.AddressName;
                ObjectName = Mount.ObjectName;
                ObjectDriveways = Mount.Driveways;

                FillPhotoCollection.Execute(null);
            });
        }
        /// <summary>
        /// Добавление фотографии
        /// </summary>
        /// <param name="obj">Изображение в формате BaseString64</param>
        /// <param name="phototypename">Тип фотографии</param>
        /// <returns></returns>
        private bool AddPhoto(string obj, string phototypename) {
            if (string.IsNullOrEmpty(obj)) 
                return false;

            if (string.IsNullOrEmpty(phototypename)) 
                return false;            

            PhotoCollection photo = new PhotoCollection(
                Guid.NewGuid(),
                obj,
                null,
                ImageSource.FromStream(() => { return new MemoryStream(Convert.FromBase64String(obj)); }),
                PhotoNames.FirstOrDefault(x => x.PhotoTypeName == phototypename)
            );
            Photos.Add(photo);
            return true;
        }
        /// <summary>
        /// Заполненение коллекции фотографий
        /// </summary>
        private RelayCommand _FillPhotoCollection;
        public RelayCommand FillPhotoCollection {
            get => _FillPhotoCollection ??= new RelayCommand(async obj => {
                AddPhoto(Mount.ObjectCard, "Обходной лист");
                AddPhoto(Mount.ObjectScheme, "Схема объекта");
                AddPhoto(Mount.ObjectWiring, "Расшлейфовка объекта");
                AddPhoto(Mount.ObjectListResponsible, "Ответственные объекта");
                AddPhoto(Mount.ObjectSignboard, "Вывеска объекта");
                AddPhoto(Mount.ObjectActTech1, "Акт технич. сост-я 1");
                AddPhoto(Mount.ObjectActTech2, "Акт технич. сост-я 2");
                AddPhoto(Mount.ObjectExtra1, "Доп. фото");
                AddPhoto(Mount.ObjectExtra2, "Доп. фото");
                AddPhoto(Mount.ObjectExtra3, "Доп. фото");
                AddPhoto(Mount.ObjectExtra4, "Доп. фото");
                AddPhoto(Mount.ObjectExtra5, "Доп. фото");
            });
        }
        /// <summary>
        /// список техников
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
        /// Выбранный гугл монтаж
        /// </summary>
        private GoogleMountModel _GoogleMount;
        public GoogleMountModel GoogleMount {
            get => _GoogleMount;
            set {
                _GoogleMount = value;
                OnPropertyChanged(nameof(GoogleMount));
            }
        }
        /// <summary>
        /// Отображение видимости описания монтажа из гугл таблиц
        /// </summary>
        private bool _VisibleGoogleComment;
        public bool VisibleGoogleComment {
            get => _VisibleGoogleComment;
            set {
                _VisibleGoogleComment = value;
                OnPropertyChanged(nameof(VisibleGoogleComment));
            }
        }
        /// <summary>
        /// Описание монтажа из гугл таблицы
        /// </summary>
        private string _GoogleComment;
        public string GoogleComment {
            get => _GoogleComment;
            set {
                _GoogleComment = value;
                OnPropertyChanged(nameof(GoogleComment));
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //private bool? _IsPickPhoto;
        //public bool? IsPickPhoto {
        //    get => _IsPickPhoto;
        //    set {
        //        _IsPickPhoto = value;
        //        OnPropertyChanged(nameof(IsPickPhoto));
        //    }
        //}
        /// <summary>
        /// Текущий монтаж
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
        /// Выбранная фотографии из коллекции фотографий монтажа
        /// </summary>
        private ImageSource _ImgSrc;
        public ImageSource ImgSrc {
            get => _ImgSrc;
            set {
                _ImgSrc = value;
                OnPropertyChanged(nameof(ImgSrc));
            }
        }

        //private ObservableCollection<ImageSource> _PhotoSource = new ObservableCollection<ImageSource>();
        //public ObservableCollection<ImageSource> PhotoSource {
        //    get => _PhotoSource;
        //    set {
        //        _PhotoSource = value;
        //        OnPropertyChanged(nameof(PhotoSource));
        //    }
        //}
        /// <summary>
        /// Иконка фотографии
        /// </summary>
        private ImageSource _PhotoImage;
        public ImageSource PhotoImage {
            get => _PhotoImage;
            set {
                _PhotoImage = value;
                OnPropertyChanged(nameof(PhotoImage));
            }
        }
        /// <summary>
        /// иконка сохранения
        /// </summary>
        private ImageSource _SaveImage;
        public ImageSource SaveImage {
            get => _SaveImage;
            set {
                _SaveImage = value;
                OnPropertyChanged(nameof(SaveImage));
            }
        }
        /// <summary>
        /// иконка отправить
        /// </summary>
        private ImageSource _SendImage;
        public ImageSource SendImage {
            get => _SendImage;
            set {
                _SendImage = value;
                OnPropertyChanged(nameof(SendImage));
            }
        }
        //private double _ProgressValue;
        //public double ProgressValue {
        //    get => _ProgressValue;
        //    set {
        //        _ProgressValue = value;
        //        OnPropertyChanged(nameof(ProgressValue));
        //    }
        //}
        /// <summary>
        /// Видимость индикатора отправки
        /// </summary>
        private bool _IndicatorVisible;
        public bool IndicatorVisible {
            get => _IndicatorVisible;
            set {
                _IndicatorVisible = value;
                OnPropertyChanged(nameof(IndicatorVisible));
            }
        }
        /// <summary>
        /// Прозрачность формы
        /// </summary>
        private double _Opacity;
        public double Opacity {
            get => _Opacity;
            set {
                _Opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }
        /// <summary>
        /// Флаг наличия изменений в монтаже для определения необходимости сохранения
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
        /// Сохраняем путь сохранения монтажа на сервере и записываем его в объект андромеды
        /// </summary>
        private string _Path;
        public string Path {
            get => _Path;
            set {
                _Path = value;
                OnPropertyChanged(nameof(Path));
            }
        }
        /// <summary>
        /// Текст на индикаторе загрузки
        /// </summary>
        private string _IndicatorText;
        public string IndicatorText {
            get => _IndicatorText;
            set {
                _IndicatorText = value;
                OnPropertyChanged(nameof(IndicatorText));
            }
        }
        /// <summary>
        /// Отправка монтажа
        /// </summary>
        private RelayCommand _SendToServer;
        public RelayCommand SendToServer {
            get => _SendToServer ??= new RelayCommand(async obj => {
                if (string.IsNullOrEmpty(ObjectNumberValidationError) && string.IsNullOrWhiteSpace(ObjectNumberValidationError)) {
                    if (string.IsNullOrEmpty(ObjectNameValidationError) && string.IsNullOrWhiteSpace(ObjectNameValidationError)) {
                        if (string.IsNullOrEmpty(ObjectAddressValidationError) && string.IsNullOrWhiteSpace(ObjectAddressValidationError)) {
                            if (Photos.Count >= 7) {
                                //TODO: проверять, что это обязательные фото, а не просто количество
                                Opacity = 0.1;
                                IndicatorVisible = true;
                                IndicatorText = "Подождите, идет загрузка...";
                                SaveToDB.Execute(null);
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine("Номер объекта: " + ObjectNumber);
                                sb.AppendLine("Наименование объекта: " + ObjectName);
                                sb.AppendLine("Адрес объекта: " + ObjectAddress);
                                sb.AppendLine("Монтажник: " + Mounters.FirstOrDefault().NewName);
                                sb.AppendLine("Подъездные пути: " + ObjectDriveways);

                                bool error = false;
                                int cnt = 1;
                                foreach (PhotoCollection ph in Photos.Where(x => x.Data != null)) {
                                    MultipartFormDataContent form = new MultipartFormDataContent {
                                        {
                                            new StreamContent(new MemoryStream(Convert.FromBase64String(ph.Data)))
                                            ,
                                            String.Format("file"),
                                            String.Format(ObjectNumber + "_" + ph._Types.PhotoTypeName + "_" + ph.ID.ToString() + ".jpeg")
                                        }
                                    };
                                    string result = await ClientHttp.PostString("/api/Common?NumberObject=" + ObjectNumber +
                                            "&NameObject=" + ObjectName +
                                            "&AddressObject=" + ObjectAddress +
                                            "&MounterName=" + Mounters.FirstOrDefault().NewName +
                                            "&Driveways=" + ObjectDriveways
                                            , form);
                                    IndicatorText = "Подождите, идет загрузка..." + Environment.NewLine + "Загружено " + cnt.ToString() + " фото из " + Photos.Count.ToString();
                                    cnt++;
                                    if (string.IsNullOrEmpty(result)) {
                                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Ошибка при загрузке фото", Color.Red, LayoutOptions.EndAndExpand), 4000));
                                        error = true;
                                    }
                                    else {
                                        PathToSaveModel path = JsonConvert.DeserializeObject<PathToSaveModel>(result);
                                        Path = path.PathToSave.ToString().Replace("C:\\", "\\\\SQL-SERVICE\\");
                                    }
                                }
                                if (!error) {
                                    if (Mount != null) {
                                        Mount.State = 1;
                                        Mount.DateSended = DateTime.Now;
                                        App.Database.UpdateMount(Mount);
                                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Сохранение в локальной базе. Успешно.", Color.Green, LayoutOptions.EndAndExpand), 4000));
                                        IndicatorText = "Подождите, идет загрузка..." + Environment.NewLine + "Отправка уведомления для операторов";
                                        SendEventsToAndromeda.Execute(null);
                                        IndicatorText = "Подождите, идет загрузка..." + Environment.NewLine + "Запись ссылки на электронный обходной";
                                        WriteWebLink.Execute(null);
                                        WriteCoordinates.Execute(null);
                                        WriteDriveways.Execute(null);
                                        if (IsSuccessSendEvents && IsSuccessWriteWebLink) 
                                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Монтаж отправлен, данные получены оператором", Color.Green, LayoutOptions.EndAndExpand), 4000));
                                        else 
                                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Монтаж отправлен, данные получены оператором", Color.Green, LayoutOptions.EndAndExpand), 4000));
                                        MountsViewModel vm = new MountsViewModel(Mounters, Servicemans);
                                        App.Current.MainPage = new MountsPage(vm);
                                    }
                                }
                            }
                            else 
                                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Не все обязательные фото были сделаны", Color.Red, LayoutOptions.EndAndExpand), 4000));
                        }
                        else 
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Адрес объекта не может быть пустым", Color.Red, LayoutOptions.EndAndExpand), 4000));
                    }
                    else 
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Название объекта не может быть пустым", Color.Red, LayoutOptions.EndAndExpand), 4000));
                }
                else 
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Номер объекта не может быть пустым", Color.Red, LayoutOptions.EndAndExpand), 4000));
                Opacity = 1;
                IndicatorVisible = false;
            }, obj => Photos.Count >= 7);
        }
        /// <summary>
        /// Отправляем на сервер номер объекта и его адрес, по адресу через геокодер яндекса определяем координаты и записываем в базу
        /// </summary>
        private RelayCommand _WriteCoordinates;
        public RelayCommand WriteCoordinates {
            get => _WriteCoordinates ??= new RelayCommand(async obj => {
                HttpStatusCode code = await ClientHttp.Get("/api/Andromeda/coords?ObjectNumber=" + ObjectNumber + "&ObjectAddress=" + ObjectAddress);
            });
        }
        /// <summary>
        /// Записываем подъездные пути к объекту
        /// </summary>
        private RelayCommand _WriteDriveways;
        public RelayCommand WriteDriveways {
            get => _WriteDriveways ??= new RelayCommand(async obj => {
                HttpStatusCode code = await ClientHttp.Get("/api/Andromeda/driveways?ObjectNumber=" + ObjectNumber + "&ObjectAddress=" + ObjectAddress);
            });
        }
        /// <summary>
        /// Запись ссылки на объект в объект андромеды
        /// </summary>
        private RelayCommand _WriteWebLink;
        public RelayCommand WriteWebLink {
            get => _WriteWebLink ??= new RelayCommand(async obj => {
                IsSuccessWriteWebLink = false;
                HttpStatusCode code = await ClientHttp.Get("/api/Andromeda/weblink?ObjectNumber=" + ObjectNumber + "&path=" + Path);
                IsSuccessWriteWebLink = code.Equals(HttpStatusCode.Accepted);
            });
        }
        /// <summary>
        /// Отправка события по объекту
        /// </summary>
        private RelayCommand _SendEventsToAndromeda;
        public RelayCommand SendEventsToAndromeda {
            get => _SendEventsToAndromeda ??= new RelayCommand(async obj => {
                IsSuccessSendEvents = false;
                HttpStatusCode code = await ClientHttp.Get("/api/Andromeda/SendEvents?ObjectNumber=" + ObjectNumber);
                if (!code.Equals(HttpStatusCode.Accepted)) {
                    IsSuccessSendEvents = false;
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("От сервера не был получен корректный ответ. Доставка обходного до оператора не может быть гарантирована.", Color.Red, LayoutOptions.EndAndExpand), 4000));
                }
                else 
                    IsSuccessSendEvents = true;
                
            });
        }
        /// <summary>
        /// Получение монтажа и заполнение объекта
        /// </summary>
        /// <param name="id">Идентификатор монтажа в локальной базе данных</param>
        /// <returns></returns>
        private Mounts GetMount(int? id) {
            Mounts mount = new Mounts {
                ObjectNumber = ObjectNumber,
                ObjectName = ObjectName,
                AddressName = ObjectAddress,
                ID = (int)(id.HasValue ? id : App.Database.GetCurrentID()),
                MounterID = Mounters.FirstOrDefault().NewMounterId,
                Driveways = ObjectDriveways,
                State = 0,
                DateTimeCreated = DateTime.Now,
                ObjectCard = Photos.Any(x => x._Types.PhotoTypeName == "Обходной лист") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Обходной лист").Data : "",
                ObjectScheme = Photos.Any(x => x._Types.PhotoTypeName == "Схема объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Схема объекта").Data : "",
                ObjectWiring = Photos.Any(x => x._Types.PhotoTypeName == "Расшлейфовка объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Расшлейфовка объекта").Data : "",
                ObjectListResponsible = Photos.Any(x => x._Types.PhotoTypeName == "Ответственные объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Ответственные объекта").Data : "",
                ObjectSignboard = Photos.Any(x => x._Types.PhotoTypeName == "Вывеска объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Вывеска объекта").Data : "",
                ObjectActTech1 = Photos.Any(x => x._Types.PhotoTypeName == "Акт технич. сост-я 1") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 1").Data : "",
                ObjectActTech2 = Photos.Any(x => x._Types.PhotoTypeName == "Акт технич. сост-я 2") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 2").Data : ""
            };
            foreach (var item in Photos.Where(x => x._Types.PhotoTypeName == "Доп. фото" && x.IsUse == false)) {
                if (mount.ObjectExtra1 == null) {
                    mount.ObjectExtra1 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                }
                else if (mount.ObjectExtra2 == null) {
                    mount.ObjectExtra2 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                }
                else if (mount.ObjectExtra3 == null) {
                    mount.ObjectExtra3 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                }
                else if (mount.ObjectExtra4 == null) {
                    mount.ObjectExtra4 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                }
                else if (mount.ObjectExtra5 == null) {
                    mount.ObjectExtra5 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
                }

                Photos.FirstOrDefault(x => x.ID == item.ID).IsUse = true;
            }
            Mount = mount;
            return mount;
        }

        private bool _SaveFlag;
        public bool SaveFlag {
            get => _SaveFlag;
            set {
                _SaveFlag = value;
                OnPropertyChanged(nameof(SaveFlag));
            }
        }
        //private RelayCommand _SaveToDB;
        //public RelayCommand SaveToDB {
        //    get => _SaveToDB ??= new RelayCommand(async obj => {
        //        Analytics.TrackEvent("Сохранение монтажа перед отправкой на сервер в локальной базе",
        //                            new Dictionary<string, string> {
        //                                {"ObjectNumber",ObjectNumber }
        //                            });
        //        if (Photos.Count >= 5) {
        //            //App.Database.SaveUpdateMount(GetMount());
        //            if (Mount == null) {
        //                Mounts mount = new Mounts();
        //                mount.ObjectNumber = ObjectNumber;
        //                mount.ObjectName = ObjectName;
        //                mount.AddressName = ObjectAddress;
        //                mount.ID = App.Database.GetCurrentID();
        //                mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
        //                mount.Driveways = ObjectDriveways;
        //                mount.State = 0;
        //                mount.DateTimeCreated = DateTime.Now;
        //                mount.ObjectCard = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Обходной лист").Data;
        //                mount.ObjectScheme = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Схема объекта").Data;
        //                mount.ObjectWiring = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Расшлейфовка объекта").Data;
        //                mount.ObjectListResponsible = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Ответственные объекта").Data;
        //                mount.ObjectSignboard = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Вывеска объекта").Data;
        //                mount.ObjectActTech1 = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 1").Data;
        //                mount.ObjectActTech2 = Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 2").Data;
        //                foreach (var item in Photos.Where(x => x._Types.PhotoTypeName == "Доп. фото" && x.IsUse == false)) {
        //                    if (mount.ObjectExtra1 == null)
        //                        mount.ObjectExtra1 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    else if (mount.ObjectExtra2 == null)
        //                        mount.ObjectExtra2 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    else if (mount.ObjectExtra3 == null)
        //                        mount.ObjectExtra3 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    else if (mount.ObjectExtra4 == null)
        //                        mount.ObjectExtra4 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    else if (mount.ObjectExtra5 == null)
        //                        mount.ObjectExtra5 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    Photos.FirstOrDefault(x => x.ID == item.ID).IsUse = true;
        //                }
        //                App.Database.SaveMount(mount);
        //                Analytics.TrackEvent("Монтаж сохранен в локальной базе");
        //                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Сохранение в локальной базе. Успешно.", Color.Green, LayoutOptions.EndAndExpand), 4000));
        //            }
        //            else {
        //                Mount.State = 0;
        //                Mount.ObjectNumber = ObjectNumber;
        //                Mount.ObjectName = ObjectName;
        //                Mount.AddressName = ObjectAddress;
        //                Mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
        //                Mount.Driveways = ObjectDriveways;
        //                Mount.DateTimeCreated = Mount.DateTimeCreated.HasValue ? Mount.DateTimeCreated : DateTime.Now;
        //                Mount.ObjectCard = Photos.Any(x => x._Types.PhotoTypeName == "Обходной лист") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Обходной лист").Data : "";
        //                Mount.ObjectScheme = Photos.Any(x => x._Types.PhotoTypeName == "Схема объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Схема объекта").Data : "";
        //                Mount.ObjectWiring = Photos.Any(x => x._Types.PhotoTypeName == "Расшлейфовка объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Расшлейфовка объекта").Data : "";
        //                Mount.ObjectListResponsible = Photos.Any(x => x._Types.PhotoTypeName == "Ответственные объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Ответственные объекта").Data : "";
        //                Mount.ObjectSignboard = Photos.Any(x => x._Types.PhotoTypeName == "Вывеска объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Вывеска объекта").Data : "";
        //                Mount.ObjectActTech1 = Photos.Any(x => x._Types.PhotoTypeName == "Акт технич. сост-я 1") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 1").Data : "";
        //                Mount.ObjectActTech2 = Photos.Any(x => x._Types.PhotoTypeName == "Акт технич. сост-я 2") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 2").Data : "";
        //                foreach (var item in Photos.Where(x => x._Types.PhotoTypeName == "Доп. фото" && x.IsUse == false)) {
        //                    if (Mount.ObjectExtra1 == null)
        //                        Mount.ObjectExtra1 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    else if (Mount.ObjectExtra2 == null)
        //                        Mount.ObjectExtra2 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    else if (Mount.ObjectExtra3 == null)
        //                        Mount.ObjectExtra3 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    else if (Mount.ObjectExtra4 == null)
        //                        Mount.ObjectExtra4 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    else if (Mount.ObjectExtra5 == null)
        //                        Mount.ObjectExtra5 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    Photos.FirstOrDefault(x => x.ID == item.ID).IsUse = true;
        //                }
        //                App.Database.SaveUpdateMount(Mount);
        //                Analytics.TrackEvent("Монтаж обновлен в локальной базе");
        //                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Сохранение в локальной базе. Успешно.", Color.Green, LayoutOptions.EndAndExpand), 4000));
        //            }
        //        }
        //        else {
        //            bool result = await Application.Current.MainPage.DisplayAlert("Ошибка", "Не все обязательные фото были сделаны. Продолжить сохранение?", "Да", "Нет");
        //            if (result) {
        //                Mounts mount = new Mounts();
        //                mount.ObjectNumber = ObjectNumber;
        //                mount.ObjectName = ObjectName;
        //                mount.AddressName = ObjectAddress;
        //                mount.ID = App.Database.GetCurrentID();
        //                mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
        //                mount.Driveways = ObjectDriveways;
        //                mount.State = 0;
        //                mount.DateTimeCreated = DateTime.Now;
        //                mount.ObjectCard = Photos.Any(x => x._Types.PhotoTypeName == "Обходной лист") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Обходной лист").Data : "";
        //                mount.ObjectScheme = Photos.Any(x => x._Types.PhotoTypeName == "Схема объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Схема объекта").Data : "";
        //                mount.ObjectWiring = Photos.Any(x => x._Types.PhotoTypeName == "Расшлейфовка объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Расшлейфовка объекта").Data : "";
        //                mount.ObjectListResponsible = Photos.Any(x => x._Types.PhotoTypeName == "Ответственные объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Ответственные объекта").Data : "";
        //                mount.ObjectSignboard = Photos.Any(x => x._Types.PhotoTypeName == "Вывеска объекта") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Вывеска объекта").Data : "";
        //                mount.ObjectActTech1 = Photos.Any(x => x._Types.PhotoTypeName == "Акт технич. сост-я 1") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 1").Data : "";
        //                mount.ObjectActTech2 = Photos.Any(x => x._Types.PhotoTypeName == "Акт технич. сост-я 2") ? Photos.FirstOrDefault(x => x._Types.PhotoTypeName == "Акт технич. сост-я 2").Data : "";
        //                foreach (var item in Photos.Where(x => x._Types.PhotoTypeName == "Доп. фото" && x.IsUse == false)) {
        //                    if (mount.ObjectExtra1 == null)
        //                        mount.ObjectExtra1 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    else if (mount.ObjectExtra2 == null)
        //                        mount.ObjectExtra2 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    else if (mount.ObjectExtra3 == null)
        //                        mount.ObjectExtra3 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    else if (mount.ObjectExtra4 == null)
        //                        mount.ObjectExtra4 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    else if (mount.ObjectExtra5 == null)
        //                        mount.ObjectExtra5 = Photos.FirstOrDefault(x => x.ID == item.ID).Data;
        //                    Photos.FirstOrDefault(x => x.ID == item.ID).IsUse = true;
        //                }
        //                App.Database.SaveMount(mount);
        //                Analytics.TrackEvent("Монтаж сохранен в локальной базе");
        //                await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Сохранение в локальной базе. Успешно.", Color.Green, LayoutOptions.EndAndExpand), 4000));
        //            }
        //        }
        //    });
        //}
        /// <summary>
        /// Команда сохранения монтажа в локальной базе данных
        /// </summary>
        private RelayCommand _SaveToDB;
        public RelayCommand SaveToDB {
            get => _SaveToDB ??= new RelayCommand(async obj => {
                if (Photos.Count >= 5) {
                    if (Mount == null) {
                        App.Database.SaveUpdateMount(GetMount(null));
                        SaveFlag = true;
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Сохранение в локальной базе. Успешно.", Color.Green, LayoutOptions.EndAndExpand), 4000));
                    }
                    else {
                        App.Database.SaveUpdateMount(GetMount(Mount.ID));
                        SaveFlag = true;
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Сохранение в локальной базе. Успешно.", Color.Green, LayoutOptions.EndAndExpand), 4000));
                    }
                }
                else {
                    bool result = await Application.Current.MainPage.DisplayAlert("Ошибка", "Не все обязательные фото были сделаны. Продолжить сохранение?", "Да", "Нет");
                    if (result) {
                        SaveFlag = true;
                        App.Database.SaveUpdateMount(GetMount(null));
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Сохранение в локальной базе. Успешно.", Color.Green, LayoutOptions.EndAndExpand), 4000));
                    }
                }
            });
        }
        /// <summary>
        /// Команда закрытия окна
        /// </summary>
        private RelayCommand _BackPressCommand;
        public RelayCommand BackPressCommand {
            get => _BackPressCommand ??= new RelayCommand(async obj => {
                if (!SaveFlag) {
                    SaveToDB.Execute(null);
                }

                MountsViewModel vm = new MountsViewModel(Mounters, Servicemans);
                App.Current.MainPage = new MountsPage(vm);
            });
        }
        /// <summary>
        /// Подгрузка файла из галереи
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
        /// Команда перехода на форму для создания/выбора фото
        /// </summary>
        private RelayCommand _TakePhotoCommand;
        public RelayCommand TakePhotoCommand {
            get => _TakePhotoCommand ??= new RelayCommand(async obj => {
                if (Mount == null) 
                    Mount = new Mounts();

                Mount.ObjectNumber = ObjectNumber;
                Mount.ObjectName = ObjectName;
                Mount.AddressName = ObjectAddress;
                Mount.Driveways = ObjectDriveways;
                Mount.MounterID = Mounters.FirstOrDefault().NewMounterId;
                SelectActionsPopupPageViewModel vm = new SelectActionsPopupPageViewModel(Mount, Mounters, Servicemans);
                await App.Current.MainPage.Navigation.PushPopupAsync(new SelectActionsPopupPage(vm));
            });
        }
        /// <summary>
        /// Удаление фотографиии
        /// </summary>
        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand {
            get => _DeleteCommand ??= new RelayCommand(async obj => {
                if (SelectedPhoto != null) {
                    bool result = await Application.Current.MainPage.DisplayAlert("Удаление", "Подтвердите удаление", "Удалить", "Отмена");
                    if (result) {
                        Photos.Remove(SelectedPhoto);
                        PhotoNames.Add(SelectedPhoto._Types);
                    }
                }
                SelectedPhoto = null;
            });
        }
        /// <summary>
        /// Список наименований фотографий
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
        /// выбранный тип фотографии
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
        /// Номер объекта с элементами валидации
        /// </summary>
        private string _ObjectNumber;
        public string ObjectNumber {
            get {
                if (string.IsNullOrEmpty(_ObjectNumber) || string.IsNullOrWhiteSpace(_ObjectNumber)) {
                    ObjectNumberValidationError = "Номер объекта не может быть пустым";
                    VisibleObjectNumberValidationError = true;
                }
                return _ObjectNumber;
            }
            set {
                if (_ObjectNumber != value) 
                    if (_ObjectNumber != null) 
                        IsChanged = true;
                _ObjectNumber = value;
                if (string.IsNullOrEmpty(_ObjectNumber) || string.IsNullOrWhiteSpace(_ObjectNumber)) {
                    ObjectNumberValidationError = "Номер объекта не может быть пустым";
                    VisibleObjectNumberValidationError = true;
                }
                else if (!int.TryParse(_ObjectNumber, out _)) {
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
        /// <summary>
        /// Наименование объекта с элементами валидации
        /// </summary>
        private string _ObjectName;
        public string ObjectName {
            get {
                if (string.IsNullOrEmpty(_ObjectName) || string.IsNullOrWhiteSpace(_ObjectName)) {
                    ObjectNameValidationError = "Название объекта не может быть пустым";
                    VisibleObjectNameValidationError = true;
                }
                return _ObjectName;
            }
            set {
                if (_ObjectName != value) 
                    if (_ObjectName != null) 
                        IsChanged = true;

                _ObjectName = value;
                if (string.IsNullOrEmpty(_ObjectName) || string.IsNullOrWhiteSpace(_ObjectName)) {
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
        /// <summary>
        /// Адрес объекта с элементами валидации
        /// </summary>
        private string _ObjectAddress;
        public string ObjectAddress {
            get {
                if (string.IsNullOrEmpty(_ObjectAddress) || string.IsNullOrWhiteSpace(_ObjectAddress)) {
                    ObjectAddressValidationError = "Адрес объекта не может быть пустым";
                    VisibleObjectAddressValidationError = true;
                }
                return _ObjectAddress;
            }
            set {
                if (_ObjectAddress != value) 
                    if (_ObjectAddress != null) 
                        IsChanged = true;

                _ObjectAddress = value;
                if (string.IsNullOrEmpty(_ObjectAddress) || string.IsNullOrWhiteSpace(_ObjectAddress)) {
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
        /// <summary>
        /// Подъездные пути
        /// </summary>
        private string _ObjectDriveways;
        public string ObjectDriveways {
            get => _ObjectDriveways;
            set {
                if (_ObjectDriveways != value) 
                    if (_ObjectDriveways != null) 
                        IsChanged = true;
                _ObjectDriveways = value;
                OnPropertyChanged(nameof(ObjectDriveways));
            }
        }
        ///// <summary>
        ///// Комментарий к фотографии
        ///// </summary>
        //private string _PhotoComment;
        //public string PhotoComment {
        //    get => _PhotoComment;
        //    set {
        //        _PhotoComment = value;
        //        OnPropertyChanged(nameof(PhotoComment));
        //    }
        //}
        /// <summary>
        /// Текстовое поле для отображения ошибки валидации (номер объекта)
        /// </summary>
        private string _ObjectNumberValidationError;
        public string ObjectNumberValidationError {
            get => _ObjectNumberValidationError;
            set {
                _ObjectNumberValidationError = value;
                OnPropertyChanged(nameof(ObjectNumberValidationError));
            }
        }
        /// <summary>
        /// Видимость текстового поля с ошибкой валидации
        /// </summary>
        private bool _VisibleObjectNumberValidationError;
        public bool VisibleObjectNumberValidationError {
            get => _VisibleObjectNumberValidationError;
            set {
                _VisibleObjectNumberValidationError = value;
                OnPropertyChanged(nameof(VisibleObjectNumberValidationError));
            }
        }
        /// <summary>
        /// Текстовое поле для отображения ошибки валидации (наименование объекта)
        /// </summary>
        private string _ObjectNameValidationError;
        public string ObjectNameValidationError {
            get => _ObjectNameValidationError;
            set {
                _ObjectNameValidationError = value;
                OnPropertyChanged(nameof(ObjectNameValidationError));
            }
        }
        /// <summary>
        /// Видимость текстового поля с ошибкой валидации наименование объекта
        /// </summary>
        private bool _VisibleObjectNameValidationError;
        public bool VisibleObjectNameValidationError {
            get => _VisibleObjectNameValidationError;
            set {
                _VisibleObjectNameValidationError = value;
                OnPropertyChanged(nameof(VisibleObjectNameValidationError));
            }
        }
        /// <summary>
        /// Текстовое поле для отображения ошибки валидации (адрес объекта)
        /// </summary>
        private string _ObjectAddressValidationError;
        public string ObjectAddressValidationError {
            get => _ObjectAddressValidationError;
            set {
                _ObjectAddressValidationError = value;
                OnPropertyChanged(nameof(ObjectAddressValidationError));
            }
        }
        /// <summary>
        /// Видимость текстового поля с ошибкой валидации адрес объекта
        /// </summary>
        private bool _VisibleObjectAddressValidationError;
        public bool VisibleObjectAddressValidationError {
            get => _VisibleObjectAddressValidationError;
            set {
                _VisibleObjectAddressValidationError = value;
                OnPropertyChanged(nameof(VisibleObjectAddressValidationError));
            }
        }
        /// <summary>
        /// флаг успешной отправки события в андромеду
        /// </summary>
        private bool _IsSuccessSendEvents;
        public bool IsSuccessSendEvents {
            get => _IsSuccessSendEvents;
            set {
                _IsSuccessSendEvents = value;
                OnPropertyChanged(nameof(IsSuccessSendEvents));
            }
        }
        /// <summary>
        /// флаг успешной записи ссылки на объект
        /// </summary>
        private bool _IsSuccessWriteWebLink;
        public bool IsSuccessWriteWebLink {
            get => _IsSuccessWriteWebLink;
            set {
                _IsSuccessWriteWebLink = value;
                OnPropertyChanged(nameof(IsSuccessWriteWebLink));
            }
        }
        /// <summary>
        /// коллекция фотографий монтажа
        /// </summary>
        private ObservableCollection<PhotoCollection> _Photos = new ObservableCollection<PhotoCollection>();
        public ObservableCollection<PhotoCollection> Photos {
            get => _Photos;
            set {
                if (_Photos != value) 
                    if (_Photos != null) 
                        IsChanged = true;

                _Photos = value;
                OnPropertyChanged(nameof(Photos));
            }
        }
        /// <summary>
        /// Выбранная фотография
        /// </summary>
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
