using System;
using System.Collections.Generic;

namespace MounterApp.Model {
    public partial class A28Object {
        public A28Object() {
            ObjCust = new HashSet<ObjCust>();
            ObjExtField = new HashSet<ObjExtField>();
            ObjZone = new HashSet<ObjZone>();
        }

        public int ObjectId { get; set; }
        public int ObjectNumber { get; set; }
        public int EventTemplateId { get; set; }
        public int ObjTypeId { get; set; }
        public string Name { get; set; }
        public string Contract { get; set; }
        public string ObjectPassword { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public string Address { get; set; }
        public bool ArmScheduleEarlyArm { get; set; }
        public bool ArmScheduleControlArm { get; set; }
        public bool ArmScheduleLaterArm { get; set; }
        public bool ArmScheduleEarlyDisarm { get; set; }
        public bool ArmScheduleControlDisarm { get; set; }
        public bool ArmScheduleLaterDisarm { get; set; }
        public int ArmScheduleDeviation { get; set; }
        public bool LschedEnable { get; set; }
        public DateTime LschedStart { get; set; }
        public DateTime LschedStop { get; set; }
        public bool Disable { get; set; }
        public DateTime DisableDt { get; set; }
        public bool AutoEnable { get; set; }
        public DateTime AutoEnableDt { get; set; }
        public int ControlTime { get; set; }
        public bool ManualDisarm { get; set; }
        public string MapFileName { get; set; }
        public string WebLink { get; set; }
        public bool IsArm { get; set; }
        public bool IsFire { get; set; }
        public bool IsPanic { get; set; }
        public bool? CtuseCommon { get; set; }
        public bool? CtignoreSystemEvent { get; set; }
        public bool? IsTestFilter { get; set; }
        public bool? IsDoubleFilter { get; set; }
        public bool IsUseEpaf { get; set; }
        public int? DeviceTypeId { get; set; }
        public bool? IsRadioChannel { get; set; }
        public bool? IsPhoneChannel { get; set; }
        public bool? IsEthernetChannel { get; set; }
        public bool? IsGsmDtmfchannel { get; set; }
        public bool? IsGsmGprschannel { get; set; }
        public bool? IsGsmCsdchannel { get; set; }
        public int? RadioTransId { get; set; }
        public string ActionNumber { get; set; }
        public string ChannelPhonePhone { get; set; }
        public int? ChannelEthernetProviderId { get; set; }
        public int? ChannelGsmOperator1Id { get; set; }
        public int? ChannelGsmOperator2Id { get; set; }
        public string ChannelGsmPhone1 { get; set; }
        public string ChannelGsmPhone2 { get; set; }
        public string DeviceSim1 { get; set; }
        public string DeviceSim2 { get; set; }
        public int? WarnSignalLevel { get; set; }
        public int? AlarmSignalLevel { get; set; }
        public int? Xcoord { get; set; }
        public int? Ycoord { get; set; }
        public bool? CoordVisible { get; set; }
        public int? MountingCompanyId { get; set; }
        public int? SecurityCompanyId { get; set; }
        public int? ServiceCompanyId { get; set; }
        public int? MonitoringStateId { get; set; }
        public DateTime? StartMonitoringDate { get; set; }
        public DateTime? StopMonitoringDate { get; set; }
        public DateTime? StateChangeDate { get; set; }
        public int? SecurityDogovorId { get; set; }
        public int? ReactionDogovorId { get; set; }
        public int? PsecurityCompanyId { get; set; }
        public int? ReactionCompanyId { get; set; }
        public int? MountingPersonId { get; set; }
        public int? PserviceCompanyId { get; set; }
        public int? TariffId { get; set; }
        public decimal? Price { get; set; }
        public int? SecurityStateId { get; set; }
        public Guid Guid { get; set; }
        public bool? IsManCaused { get; set; }
        public bool? IsAccessControl { get; set; }
        public bool? IsServiceAvailable { get; set; }
        public bool? IsVitalFunction { get; set; }
        public string CommentForOperator { get; set; }
        public string CommentForGuard { get; set; }
        public string CustomersComment { get; set; }
        public bool RecordDeleted { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string TransmitterId { get; set; }
        public string JupiterDeviceId { get; set; }
        public string JupiterCypherKey { get; set; }
        public string JupiterParams { get; set; }
        public int? ObjAdminId { get; set; }
        public string JupiterArmDisarmPassword { get; set; }
        public int? DeviceCsn { get; set; }
        public int? DeviceArmAllowed { get; set; }
        public int? DeviceHwversion { get; set; }
        public int? DeviceSwversion { get; set; }
        public decimal? MoneyBalance { get; set; }
        public int? DisableReason { get; set; }
        public decimal? ContractPrice { get; set; }
        public int RegionId { get; set; }
        public string DeviceImei { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool? IsSecureAtm { get; set; }

        public virtual ObjAdmin ObjAdmin { get; set; }
        public virtual ICollection<ObjCust> ObjCust { get; set; }
        public virtual ICollection<ObjExtField> ObjExtField { get; set; }
        public virtual ICollection<ObjZone> ObjZone { get; set; }
    }
}
