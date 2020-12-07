using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.InternalModel {
    public class GoogleMountModel {
        public Guid id { get; set; }
        public string TimeString { get; set; }
        public string Info { get; set; }
        public string FullInfo {
            get {
                return TimeString + Environment.NewLine + Info;
            }
        }
    }
}
