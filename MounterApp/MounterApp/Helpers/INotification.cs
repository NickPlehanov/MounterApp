using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.Helpers {
    public interface INotification {
        void CreateNotification(string title, string text);
    }
}
