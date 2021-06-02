using System;
using System.Collections.Generic;

namespace MounterApp.Model {
    public partial class NewGuardObjectBase {
        public NewGuardObjectBase() {
            NewGuardObjectExtensionBaseNewCuratorMountNavigation = new HashSet<NewGuardObjectExtensionBase>();
            NewMountWorksExtensionBase = new HashSet<NewMountWorksExtensionBase>();
        }

        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? DeletionStateCode { get; set; }
        public int? ImportSequenceNumber { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid NewGuardObjectId { get; set; }
        public DateTime? OverriddenCreatedOn { get; set; }
        public Guid? OwningBusinessUnit { get; set; }
        public int Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? TimeZoneRuleVersionNumber { get; set; }
        public int? UtcconversionTimeZoneCode { get; set; }
        public byte[] VersionNumber { get; set; }
        public Guid? OwningUser { get; set; }
        public Guid? TransactionCurrencyId { get; set; }
        public decimal? ExchangeRate { get; set; }

        public virtual SystemUserBase OwningUserNavigation { get; set; }
        public virtual NewGuardObjectExtensionBase NewGuardObjectExtensionBaseNewGuardObject { get; set; }
        public virtual ICollection<NewGuardObjectExtensionBase> NewGuardObjectExtensionBaseNewCuratorMountNavigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBase { get; set; }
    }
}
