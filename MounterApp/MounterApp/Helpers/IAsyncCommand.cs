using System.Threading.Tasks;
using System.Windows.Input;

namespace MounterApp.Helpers {
    public interface IAsyncCommand : ICommand {
        Task ExecuteAsync(object parameter);
    }
}
