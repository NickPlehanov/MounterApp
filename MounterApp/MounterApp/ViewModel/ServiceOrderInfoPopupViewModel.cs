using Microsoft.AppCenter.Analytics;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class ServiceOrderInfoPopupViewModel : BaseViewModel {
        public ServiceOrderInfoPopupViewModel(NewServiceorderExtensionBase_ex so) {
            ServiceOrder = so;
            GetFullInfoAboutOrderCommand.Execute(ServiceOrder);
            VisibleAcceptedLayout = false;
        }
        public ServiceOrderInfoPopupViewModel(NewTest2ExtensionBase_ex fso) {
            FireServiceOrder = fso;
            GetFullInfoAboutOrderCommand.Execute(FireServiceOrder);
            VisibleAcceptedLayout = false;
        }

        private NewServiceorderExtensionBase_ex _ServiceOrder;
        public NewServiceorderExtensionBase_ex ServiceOrder {
            get => _ServiceOrder;
            set {
                _ServiceOrder = value;
                OnPropertyChanged(nameof(ServiceOrder));
            }
        }

        private NewTest2ExtensionBase_ex _FireServiceOrder;
        public NewTest2ExtensionBase_ex FireServiceOrder {
            get => _FireServiceOrder;
            set {
                _FireServiceOrder = value;
                OnPropertyChanged(nameof(FireServiceOrder));
            }
        }

        private string _Info;
        public string Info {
            get => _Info;
            set {
                _Info = value;
                OnPropertyChanged(nameof(Info));
            }
        }

        private ImageSource _EntracePhoto;
        public ImageSource EntracePhoto {
            get => _EntracePhoto;
            set {
                _EntracePhoto = value;
                OnPropertyChanged(nameof(EntracePhoto));
            }
        }

        private string _EntracePhotoText;
        public string EntracePhotoText {
            get => _EntracePhotoText;
            set {
                _EntracePhotoText = value;
                OnPropertyChanged(nameof(EntracePhotoText));
            }
        }

        private ImageSource _SchemePhoto;
        public ImageSource SchemePhoto {
            get => _SchemePhoto;
            set {
                _SchemePhoto = value;
                OnPropertyChanged(nameof(SchemePhoto));
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

        private string _PhotoTypeName;
        public string PhotoTypeName {
            get => _PhotoTypeName;
            set {
                _PhotoTypeName = value;
                OnPropertyChanged(nameof(PhotoTypeName));
            }
        }

        private int? _NumberObject;
        public int? NumberObject {
            get => _NumberObject;
            set {
                _NumberObject = value;
                OnPropertyChanged(nameof(NumberObject));
            }
        }

        private RelayCommand _GetPhotoEntraceCommand;
        public RelayCommand GetPhotoEntraceCommand {
            get => _GetPhotoEntraceCommand ??= new RelayCommand(async obj => {
                using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/download?ObjectNumber=" + obj + "&PhotoType=Вывеска объекта");
                    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                        var result = await response.Content.ReadAsStringAsync();
                        EntracePhoto = ImageSource.FromStream(() => {
                            return new MemoryStream(Convert.FromBase64String(result));
                        });
                    }
                    else {
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Фотография не найдена",Color.Red,LayoutOptions.EndAndExpand),4000));
                    }
                    ShowEntracePhotoCommand.ChangeCanExecute();
                    AddPhotoCommand.ChangeCanExecute();
                }
            });
        }

        private RelayCommand _GetPhotoSchemeCommand;
        public RelayCommand GetPhotoSchemeCommand {
            get => _GetPhotoSchemeCommand ??= new RelayCommand(async obj => {
                using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                    HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + "/api/Common/download?ObjectNumber=" + obj + "&PhotoType=Схема объекта");
                    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)) {
                        var result = await response.Content.ReadAsStringAsync();
                        SchemePhoto = ImageSource.FromStream(() => {
                            return new MemoryStream(Convert.FromBase64String(result));
                        });
                    }
                    else
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Фотография не найдена",Color.Red,LayoutOptions.EndAndExpand),4000));
                    ShowSchemePhotoCommand.ChangeCanExecute();
                    AddPhotoCommand.ChangeCanExecute();
                }
            });
        }

        private RelayCommand _GetFullInfoAboutOrderCommand;
        public RelayCommand GetFullInfoAboutOrderCommand {
            get => _GetFullInfoAboutOrderCommand ??= new RelayCommand(async obj => {
                if(obj != null) {
                    //await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Поиск и загрузка фотографий",Color.Yellow,LayoutOptions.EndAndExpand),4000));
                    NewServiceorderExtensionBase_ex _so = null;
                    NewTest2ExtensionBase_ex _fso = null;
                    if(obj is NewServiceorderExtensionBase_ex)
                        _so = obj as NewServiceorderExtensionBase_ex;
                    if(obj is NewTest2ExtensionBase_ex)
                        _fso = obj as NewTest2ExtensionBase_ex;
                    if(_so != null) {
                        GetPhotoEntraceCommand.Execute(_so.NewNumber);
                        GetPhotoSchemeCommand.Execute(_so.NewNumber);
                        Info = _so.NewName;
                        NumberObject = _so.NewNumber;
                    }
                    if(_fso != null) {
                        GetPhotoEntraceCommand.Execute(_fso.NewNumber);
                        GetPhotoSchemeCommand.Execute(_fso.NewNumber);
                        Info = _fso.NewName;
                        NumberObject = _so.NewNumber;
                    }
                    //ShowEntracePhotoCommand.ChangeCanExecute();
                    //ShowSchemePhotoCommand.ChangeCanExecute();
                }
            });
        }

        private RelayCommand _ShowEntracePhotoCommand;
        public RelayCommand ShowEntracePhotoCommand {
            get => _ShowEntracePhotoCommand ??= new RelayCommand(async obj => {
                ImagePopupViewModel vm = new ImagePopupViewModel(EntracePhoto);
                await App.Current.MainPage.Navigation.PushPopupAsync(new ImagePopupPage(vm));
            },obj => EntracePhoto != null);
        }

        private RelayCommand _ShowSchemePhotoCommand;
        public RelayCommand ShowSchemePhotoCommand {
            get => _ShowSchemePhotoCommand ??= new RelayCommand(async obj => {
                ImagePopupViewModel vm = new ImagePopupViewModel(SchemePhoto);
                await App.Current.MainPage.Navigation.PushPopupAsync(new ImagePopupPage(vm));
            },obj => SchemePhoto != null);
        }

        private RelayCommand _ExitCommand;
        public RelayCommand ExitCommand {
            get => _ExitCommand ??= new RelayCommand(async obj => {
                //try {
                //    Analytics.TrackEvent("Выход со страницы для запроса событий по объекту",
                //    new Dictionary<string,string> {
                //    {"ServicemanPhone",Servicemans.First().NewPhone }
                //    });
                //}
                //catch { }
                await App.Current.MainPage.Navigation.PopPopupAsync(false);
            });
        }

        private RelayCommand _AddPhotoCommand;
        public RelayCommand AddPhotoCommand {
            get => _AddPhotoCommand ??= new RelayCommand(async obj => {
                if(obj != null)
                    if(!string.IsNullOrEmpty(obj as string)) {
                        PhotoTypeName = obj as string;
                        bool result = await Application.Current.MainPage.DisplayAlert("Выбор действия","Укажите источник для фото","Галерея","Камера");
                        if(result)
                            File = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions {
                                PhotoSize = PhotoSize.Full,
                                CompressionQuality = int.Parse(Application.Current.Properties["Quality"].ToString())
                            });
                        else
                            File = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions {
                                Directory = "Sample",
                                Name = "test.jpg",
                                PhotoSize = PhotoSize.Full,
                                CompressionQuality = int.Parse(Application.Current.Properties["Quality"].ToString())
                            });
                        if(File == null)
                            return;
                        else {
                            ImgSrc = ImageSource.FromStream(() => {
                                var stream = File.GetStream();
                                return stream;
                            });
                            VisibleAcceptedLayout = true;
                        }
                    }
            },obj => EntracePhoto == null || SchemePhoto == null);
        }

        private RelayCommand _AcceptAndSendPhotoCommand;
        public RelayCommand AcceptAndSendPhotoCommand {
            get => _AcceptAndSendPhotoCommand ??= new RelayCommand(async obj => {
                if(File != null)
                    if(!string.IsNullOrEmpty(File.Path)) {
                        string tmp = Convert.ToBase64String(System.IO.File.ReadAllBytes(File.Path));
                        MultipartFormDataContent form = new MultipartFormDataContent();
                        //string number = ServiceOrder != null ? ServiceOrder.NewNumber.ToString() : FireServiceOrder.NewNumber.ToString();
                        if(NumberObject.HasValue && tmp != null) {
                            using(HttpClient client = new HttpClient(GetHttpClientHandler())) {
                                form.Add(new StreamContent(new MemoryStream(Convert.FromBase64String(tmp)))
                            ,String.Format("file"),String.Format(NumberObject + "_" + PhotoTypeName + "_" + Guid.NewGuid().ToString() + ".jpeg"));
                                if(form != null) {
                                    HttpResponseMessage response = await client.PostAsync(Resources.BaseAddress + "/api/Common2?ObjectNumber=" + NumberObject,form);
                                    if(response.IsSuccessStatusCode) {
                                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Фото отправлено",Color.Green,LayoutOptions.EndAndExpand),4000));
                                        if(PhotoTypeName.ToLower().Contains("вывеска")) {
                                            if (ServiceOrder!=null)
                                                GetPhotoEntraceCommand.Execute(ServiceOrder.NewNumber);
                                            else if(FireServiceOrder != null)
                                                GetPhotoEntraceCommand.Execute(FireServiceOrder.NewNumber);
                                        }
                                        if(PhotoTypeName.ToLower().Contains("схема")) {
                                            if(ServiceOrder != null)
                                                GetPhotoSchemeCommand.Execute(ServiceOrder.NewNumber);
                                            else if(FireServiceOrder != null)
                                                GetPhotoSchemeCommand.Execute(FireServiceOrder.NewNumber);
                                        }
                                    }
                                    else
                                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Фото не было отправлено. Ошибка сервера",Color.Red,LayoutOptions.EndAndExpand),4000));
                                }
                            }
                        }
                        else
                            await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Фото не было отправлено. Нет номера объекта",Color.Red,LayoutOptions.EndAndExpand),4000));
                        VisibleAcceptedLayout = false;
                    }
            });
        }
    }
}
