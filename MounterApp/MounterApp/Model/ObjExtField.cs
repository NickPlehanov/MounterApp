using System;

namespace MounterApp.Model {
    public partial class ObjExtField {
        public int RecordId { get; set; }
        public int ObjectId { get; set; }
        public int ExtFieldId { get; set; }
        public string ExtFieldValue { get; set; }
        public Guid ExtFiledValueGuid { get; set; }

        public virtual ExtField ExtField { get; set; }
        public virtual Object Object { get; set; }
    }
}
