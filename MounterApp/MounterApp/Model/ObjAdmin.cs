using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.Model {
    public partial class ObjAdmin {
        public ObjAdmin() {
            Object = new HashSet<A28Object>();
        }
        public int ObjAdminId { get; set; }
        public string AdminName { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPhone { get; set; }
        public Guid AdminGuid { get; set; }
        public DateTime? SentInviteDt { get; set; }
        public DateTime? RegistrationDt { get; set; }
        public string MyAlarmUserId { get; set; }
        public bool RecordDeleted { get; set; }

        public virtual ICollection<A28Object> Object { get; set; }
    }
}
