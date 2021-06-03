using MounterApp.Helpers;
using MounterApp.Model;
using MounterApp.Properties;
using MounterApp.Views;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace MounterApp.ViewModel {
    public class OrdersForITViewModel : BaseViewModel {
        /// <summary>
        /// Окно отправки заявки ИТ-отделу
        /// </summary>
        /// <param name="mounters">Список монтажников</param>
        /// <param name="servicemans">Список техников</param>
        public OrdersForITViewModel(List<NewMounterExtensionBase> mounters, List<NewServicemanExtensionBase> servicemans) {
            EnableButton = true;
            TextButton = "Закрыть";
            Mounters = mounters;
            Servicemans = servicemans;
            GetUserInfo.Execute(null);
            CloseOrSendImage = IconName("close");
        }
        /// <summary>
        /// Иконка для кнопки закрытия/отправки
        /// </summary>
        private ImageSource _CloseOrSendImage;
        public ImageSource CloseOrSendImage {
            get => _CloseOrSendImage;
            set {
                _CloseOrSendImage = value;
                OnPropertyChanged(nameof(CloseOrSendImage));
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
        /// Переменная для хранения информации о  пользователе
        /// </summary>
        private string _UserInfo;
        public string UserInfo {
            get => _UserInfo;
            set {
                _UserInfo = value;
                OnPropertyChanged(nameof(UserInfo));
            }
        }
        /// <summary>
        /// Определяет текст на кнопке
        /// Закрыть/Отправить
        /// </summary>
        private string _TextButton;
        public string TextButton {
            get => _TextButton;
            set {
                _TextButton = value;
                OnPropertyChanged(nameof(TextButton));
            }
        }
        /// <summary>
        /// Описание проблемы/пожелания
        /// </summary>
        private string _DescriptionProblem;
        public string DescriptionProblem {
            get => _DescriptionProblem;
            set {
                _DescriptionProblem = value;
                if (string.IsNullOrEmpty(_DescriptionProblem)) {
                    TextButton = "Закрыть";
                    CloseOrSendImage = IconName("close");
                }
                else {
                    TextButton = "Отправить";
                    CloseOrSendImage = IconName("send");
                }
                OnPropertyChanged(nameof(DescriptionProblem));
            }
        }
        /// <summary>
        /// Доступность кнопки отправить
        /// </summary>
        private bool _EnableButton;
        public bool EnableButton {
            get => _EnableButton;
            set {
                _EnableButton = value;
                OnPropertyChanged(nameof(EnableButton));
            }
        }
        /// <summary>
        /// Команда получения информации о пользователе, который хочет отправить заявку ИТ-отделу
        /// </summary>
        private RelayCommand _GetUserInfo;
        public RelayCommand GetUserInfo {
            get => _GetUserInfo ??= new RelayCommand(async obj => {
                UserInfo = Mounters != null ?
                    Mounters.FirstOrDefault().NewName + Environment.NewLine + Mounters.FirstOrDefault().NewPhone + Environment.NewLine + Mounters.FirstOrDefault().NewMounterId :
                    Servicemans.FirstOrDefault().NewName + Environment.NewLine + Servicemans.FirstOrDefault().NewPhone + Environment.NewLine + Servicemans.FirstOrDefault().NewServicemanId;
            });
        }
        /// <summary>
        /// Команда закрытия формы по кнопке
        /// </summary>
        private RelayCommand _ExitCommand;
        public RelayCommand ExitCommand {
            get => _ExitCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(false);
            });
        }
        /// <summary>
        /// Команда отправки на сервер объекта Заявка в ИТ отдел
        /// </summary>
        private RelayCommand _SendCommand;
        public RelayCommand SendCommand {
            get => _SendCommand ??= new RelayCommand(async obj => {
                EnableButton = false;
                Guid id = Guid.NewGuid();
                var data = JsonConvert.SerializeObject(new NewItBase() {
                    NewItId = id,
                    CreatedOn = DateTime.Now.AddHours(-5),
                    ModifiedOn = DateTime.Now.AddHours(-5),
                    DeletionStateCode = 0,
                    Statecode = 0,
                    Statuscode = 1
                });

                ExitCommand.Execute(null);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                await ClientHttp.PostStateCode("/api/NewItBases", content);
                var ex_data = JsonConvert.SerializeObject(new NewItExtensionBase() {
                    NewItId = id,
                    NewComment = DescriptionProblem + Environment.NewLine + UserInfo,
                    NewName = "Проблема в мобильном приложении MounterApp"
                });
                StringContent ex_content = new StringContent(ex_data, Encoding.UTF8, "application/json");
                var result = await ClientHttp.PostStateCode("/api/NewItExtensionBases", ex_content);
                if (result.Equals(HttpStatusCode.Accepted))
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Успешно отправлено", Color.Green, LayoutOptions.EndAndExpand), 4000));
                else
                    await App.Current.MainPage.Navigation.PushPopupAsync(new MessagePopupPage(new MessagePopupPageViewModel("Ошибка при отправке", Color.Red, LayoutOptions.EndAndExpand), 4000));
            });
        }
        /// <summary>
        /// Команда выбора варианта действия по кнопке на форме, отправка или простое закрытие формы
        /// </summary>
        private RelayCommand _ChooseCommand;
        public RelayCommand ChooseCommand {
            get => _ChooseCommand ??= new RelayCommand(async obj => {
                if (string.IsNullOrEmpty(DescriptionProblem)) 
                    ExitCommand.Execute(null);                
                else 
                    SendCommand.Execute(null);
                
            });
        }
    }
}
