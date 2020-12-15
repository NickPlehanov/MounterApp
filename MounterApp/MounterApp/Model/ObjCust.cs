using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.Model {
    public partial class ObjCust {
        public int RecordId { get; set; }
        public int ObjectId { get; set; }
        public int? OrderNumber { get; set; }
        public int? UserNumber { get; set; }
        public string ObjCustTitle { get; set; }
        public string ObjCustName { get; set; }
        public string ObjCustPhone1 { get; set; }
        public string ObjCustPhone2 { get; set; }
        public string ObjCustPhone3 { get; set; }
        public string ObjCustPhone4 { get; set; }
        public string ObjCustPhone5 { get; set; }
        public string ObjCustAddress { get; set; }
        public int? NordKeyId { get; set; }
        public string Pincode { get; set; }
        public bool? IsVisibleInCabinet { get; set; }
        public bool? ReclosingRequest { get; set; }
        public bool? ReclosingFailure { get; set; }
        public string MyAlarmUserId { get; set; }
        public int? MyAlarmUserRole { get; set; }
        public string MyAlarmUserPhone { get; set; }
        public Guid ObjCustGuid { get; set; }
        public bool? MyAlarmPanicEnabled { get; set; }

        public virtual A28Object Object { get; set; }
    }
}
