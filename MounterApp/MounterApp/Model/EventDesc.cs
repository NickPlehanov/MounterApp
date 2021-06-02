using System.Collections.Generic;

namespace MounterApp.Model {
    public partial class EventDesc {
        public EventDesc() {
            ObjZone = new HashSet<ObjZone>();
        }

        public int EventDescId { get; set; }
        public string EventDesc1 { get; set; }

        public virtual ICollection<ObjZone> ObjZone { get; set; }
    }
}
