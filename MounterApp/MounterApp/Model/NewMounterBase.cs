using System;
using System.Collections.Generic;

namespace MounterApp.Model {
    public partial class NewMounterBase {
        public NewMounterBase() {
            NewGuardObjectExtensionBase = new HashSet<NewGuardObjectExtensionBase>();
            NewMountWorksExtensionBaseNewMontajnik0Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewMontajnik1Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewMontajnik2Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewMontajnik3Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewMontajnik4Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewMontajnik5Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewMontajnik6Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewMontajnik7Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewMontajnik8Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewMontajnik9Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewMountBossNavigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewMountDoNavigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewMounterMountWorksNavigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName0Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName10Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName11Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName12Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName13Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName14Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName15Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName16Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName17Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName18Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName19Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName1Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName20Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName2Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName3Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName4Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName5Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName6Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName7Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName8Navigation = new HashSet<NewMountWorksExtensionBase>();
            NewMountWorksExtensionBaseNewPartialName9Navigation = new HashSet<NewMountWorksExtensionBase>();
        }

        public Guid NewMounterId { get; set; }
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

        public virtual NewMounterExtensionBase NewMounterExtensionBase { get; set; }
        public virtual ICollection<NewGuardObjectExtensionBase> NewGuardObjectExtensionBase { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewMontajnik0Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewMontajnik1Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewMontajnik2Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewMontajnik3Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewMontajnik4Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewMontajnik5Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewMontajnik6Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewMontajnik7Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewMontajnik8Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewMontajnik9Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewMountBossNavigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewMountDoNavigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewMounterMountWorksNavigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName0Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName10Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName11Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName12Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName13Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName14Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName15Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName16Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName17Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName18Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName19Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName1Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName20Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName2Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName3Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName4Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName5Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName6Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName7Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName8Navigation { get; set; }
        public virtual ICollection<NewMountWorksExtensionBase> NewMountWorksExtensionBaseNewPartialName9Navigation { get; set; }
    }
}
