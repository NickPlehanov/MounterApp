using MounterApp.Helpers;
using Rg.Plugins.Popup.Extensions;

namespace MounterApp.ViewModel {
    public class HelpPopupViewModel : BaseViewModel {
        /// <summary>
        /// Конструктор для окна помощи
        /// </summary>
        /// <param name="msg">Текстовое форматированое сообщение </param>
        public HelpPopupViewModel(string msg) {
            Message = msg;
        }
        /// <summary>
        /// Текстовое сообщение, должно быть заранее форматировано
        /// </summary>
        private string _Message;
        public string Message {
            get => _Message;
            set {
                _Message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        /// <summary>
        /// Команда закрытия окна
        /// </summary>
        private RelayCommand _BackPressedCommand;
        public RelayCommand BackPressedCommand {
            get => _BackPressedCommand ??= new RelayCommand(async obj => {
                await App.Current.MainPage.Navigation.PopPopupAsync(false);
            });
        }
    }
}
