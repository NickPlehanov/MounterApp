using System;

namespace MounterApp.Model {
    public partial class NewMounterExtensionBase {
        public Guid NewMounterId { get; set; }
        public string NewName { get; set; }
        public string NewPhone { get; set; }
        public bool? NewIsWorking { get; set; }
        public bool? NewIsHead { get; set; }

        public virtual NewMounterBase NewMounter { get; set; }
    }
}
