using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.Model {
    public partial class SystemUserBase {
        public SystemUserBase() {
            InverseParentSystemUser = new HashSet<SystemUserBase>();
            NewAndromedaBase = new HashSet<NewAndromedaBase>();
            NewGuardObjectBase = new HashSet<NewGuardObjectBase>();
            NewGuardObjectExtensionBaseNewCuratorNavigation = new HashSet<NewGuardObjectExtensionBase>();
            NewGuardObjectExtensionBaseNewCuratorUserMountNavigation = new HashSet<NewGuardObjectExtensionBase>();
            NewGuardObjectExtensionBaseNewInspectorNavigation = new HashSet<NewGuardObjectExtensionBase>();
            NewGuardObjectExtensionBaseNewRetentionNavigation = new HashSet<NewGuardObjectExtensionBase>();
            NewMountWorksExtensionBaseNewCuratorNavigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewSystemuserMountWorksNavigation = new HashSet<NewMountWorksExtensionBase>();
        }

        public Guid SystemUserId { get; set; }
        public int DeletionStateCode { get; set; }
        public Guid? TerritoryId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid BusinessUnitId { get; set; }
        public Guid? ParentSystemUserId { get; set; }
        public string FirstName { get; set; }
        public string Salutation { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PersonalEmailAddress { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public string Title { get; set; }
        public string InternalEmailAddress { get; set; }
        public string JobTitle { get; set; }
        public string MobileAlertEmail { get; set; }
        public int? PreferredEmailCode { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public int? PreferredPhoneCode { get; set; }
        public int? PreferredAddressCode { get; set; }
        public string PhotoUrl { get; set; }
        public string DomainName { get; set; }
        public int? PassportLo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? PassportHi { get; set; }
        public string DisabledReason { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public string EmployeeId { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDisabled { get; set; }
        public string GovernmentId { get; set; }
        public byte[] VersionNumber { get; set; }
        public string Skills { get; set; }
        public bool? DisplayInServiceViews { get; set; }
        public Guid? CalendarId { get; set; }
        public Guid? ActiveDirectoryGuid { get; set; }
        public bool SetupUser { get; set; }
        public Guid? SiteId { get; set; }
        public string WindowsLiveId { get; set; }
        public int IncomingEmailDeliveryMethod { get; set; }
        public int OutgoingEmailDeliveryMethod { get; set; }
        public int? ImportSequenceNumber { get; set; }
        public int AccessMode { get; set; }
        public int? InviteStatusCode { get; set; }
        public bool? IsActiveDirectoryUser { get; set; }
        public DateTime? OverriddenCreatedOn { get; set; }
        public int? UtcconversionTimeZoneCode { get; set; }
        public int? TimeZoneRuleVersionNumber { get; set; }
        public string YomiFullName { get; set; }
        public string YomiLastName { get; set; }
        public string YomiMiddleName { get; set; }
        public string YomiFirstName { get; set; }

        public virtual SystemUserBase ParentSystemUser { get; set; }
        public virtual ICollection<SystemUserBase> InverseParentSystemUser { get; set; }
        public virtual ICollection<NewAndromedaBase> NewAndromedaBase { get; set; }
        public virtual ICollection<NewGuardObjectBase> NewGuardObjectBase { get; set; }
        public virtual ICollection<NewGuardObjectExtensionBase> NewGuardObjectExtensionBaseNewCuratorNavigation { get; set; }
        public virtual ICollection<NewGuardObjectExtensionBase> NewGuardObjectExtensionBaseNewCuratorUserMountNavigation { get; set; }
        public virtual ICollection<NewGuardObjectExtensionBase> NewGuardObjectExtensionBaseNewInspectorNavigation { get; set; }
        public virtual ICollection<NewGuardObjectExtensionBase> NewGuardObjectExtensionBaseNewRetentionNavigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewCuratorNavigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewSystemuserMountWorksNavigation { get; set; }
    }
}
