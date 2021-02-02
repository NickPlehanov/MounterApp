using Microsoft.AppCenter.Analytics;
using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
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
        }
        public ServiceOrderInfoPopupViewModel(NewTest2ExtensionBase_ex fso) {
            FireServiceOrder = fso;
            GetFullInfoAboutOrderCommand.Execute(FireServiceOrder);
        }

        private NewServiceorderExtensionBase _ServiceOrder;
        public NewServiceorderExtensionBase ServiceOrder {
            get => _ServiceOrder;
            set {
                _ServiceOrder = value;
                OnPropertyChanged(nameof(ServiceOrder));
            }
        }

        private NewTest2ExtensionBase _FireServiceOrder;
        public NewTest2ExtensionBase FireServiceOrder {
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

        private ImageSource _SchemePhoto;
        public ImageSource SchemePhoto {
            get => _SchemePhoto;
            set {
                _SchemePhoto = value;
                OnPropertyChanged(nameof(SchemePhoto));
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
                    else
                        await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Фотография не найдена",Color.Red,LayoutOptions.EndAndExpand),4000));
                    ShowEntracePhotoCommand.ChangeCanExecute();
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
                    }
                    if(_fso != null) {
                        GetPhotoEntraceCommand.Execute(_fso.NewNumber);
                        GetPhotoSchemeCommand.Execute(_fso.NewNumber);
                        Info = _fso.NewName;
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
            },obj=>EntracePhoto!=null);
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
    }
}
