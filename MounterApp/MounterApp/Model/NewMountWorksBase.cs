using System;

namespace MounterApp.Model {
    public partial class NewMountWorksBase {
        public Guid NewMountWorksId { get; set; }
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
        public Guid? TransactionCurrencyId { get; set; }
        public decimal? ExchangeRate { get; set; }

        public virtual NewMountWorksExtensionBase NewMountWorksExtensionBase { get; set; }
    }
}
