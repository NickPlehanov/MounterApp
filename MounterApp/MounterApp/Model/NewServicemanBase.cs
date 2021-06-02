using System;
using System.Collections.Generic;

namespace MounterApp.Model {
    public partial class NewServicemanBase {
        public NewServicemanBase() {
            NewServicemanExtensionBaseNewGenTechNavigation = new HashSet<NewServicemanExtensionBase>();
            NewServiceorderExtensionBaseNewNewServicemanNavigation = new HashSet<NewServiceorderExtensionBase>();
            NewServiceorderExtensionBaseNewServicemanServiceorderNavigation = new HashSet<NewServiceorderExtensionBase>();
            NewServiceorderExtensionBaseNewTechniqueEndNavigation = new HashSet<NewServiceorderExtensionBase>();
        }

        public Guid NewServicemanId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid? OrganizationId { get; set; }
        public int Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? DeletionStateCode { get; set; }
        public byte[] VersionNumber { get; set; }
        public int? ImportSequenceNumber { get; set; }
        public DateTime? OverriddenCreatedOn { get; set; }
        public int? TimeZoneRuleVersionNumber { get; set; }
        public int? UtcconversionTimeZoneCode { get; set; }

        public virtual NewServicemanExtensionBase NewServicemanExtensionBaseNewServiceman { get; set; }
        public virtual ICollection<NewServicemanExtensionBase> NewServicemanExtensionBaseNewGenTechNavigation { get; set; }
        public virtual ICollection<NewServiceorderExtensionBase> NewServiceorderExtensionBaseNewNewServicemanNavigation { get; set; }
        public virtual ICollection<NewServiceorderExtensionBase> NewServiceorderExtensionBaseNewServicemanServiceorderNavigation { get; set; }
        public virtual ICollection<NewServiceorderExtensionBase> NewServiceorderExtensionBaseNewTechniqueEndNavigation { get; set; }
    }
}
