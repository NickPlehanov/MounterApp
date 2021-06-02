using System.Collections.Generic;

namespace MounterApp.Model {
    public partial class ExtField {
        public ExtField() {
            ObjExtField = new HashSet<ObjExtField>();
        }

        public int ExtFieldId { get; set; }
        public int OrderNumber { get; set; }
        public int ExtFieldType { get; set; }
        public string ExtFieldName { get; set; }
        public string Description { get; set; }
        public bool RecordDeleted { get; set; }

        public virtual ICollection<ObjExtField> ObjExtField { get; set; }
    }
}
