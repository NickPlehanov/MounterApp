using System;

namespace MounterApp.Model {
    public partial class NewTest2Base {
        public Guid NewTest2Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid? OwningUser { get; set; }
        public Guid? OwningBusinessUnit { get; set; }
        public int Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? DeletionStateCode { get; set; }
        public byte[] VersionNumber { get; set; }
        public int? ImportSequenceNumber { get; set; }
        public DateTime? OverriddenCreatedOn { get; set; }
        public int? TimeZoneRuleVersionNumber { get; set; }
        public int? UtcconversionTimeZoneCode { get; set; }

        public virtual SystemUserBase OwningUserNavigation { get; set; }
        public virtual NewTest2ExtensionBase NewTest2ExtensionBase { get; set; }
    }
}
