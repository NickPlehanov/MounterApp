using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.Model {
    public partial class ObjZone {
        public int ObjZoneId { get; set; }
        public int ObjectId { get; set; }
        public int? ZoneNumber { get; set; }
        public int? EventDescId { get; set; }
        public string ZoneEquip { get; set; }
        public Guid Guid { get; set; }

        public virtual EventDesc EventDesc { get; set; }
        public virtual Object Object { get; set; }
    }
}
