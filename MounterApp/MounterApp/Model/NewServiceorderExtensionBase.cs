﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace MounterApp.Model {
    public partial class NewServiceorderExtensionBase {
        public Guid NewServiceorderId { get; set; }
        public string NewName { get; set; } = "";
        public DateTime? NewDate { get; set; }
        public Guid? NewServicemanServiceorder { get; set; }
        public DateTime? NewIncome { get; set; }
        public DateTime? NewOutgone { get; set; }
        public bool? NewListReturn { get; set; }
        public string NewComments { get; set; }
        public Guid? NewNewServiceman { get; set; }
        public int? NewCategory { get; set; }
        public string NewWhoInit { get; set; }
        public string NewAddress { get; set; }
        public Guid? NewAndromedaServiceorder { get; set; }
        public int? NewNumber { get; set; }
        public string NewObjName { get; set; }
        public DateTime? NewMoved { get; set; }
        public string NewTime { get; set; }
        public string NewTransferReason { get; set; }
        public int? NewResult { get; set; }
        public string NewMovedFrom { get; set; }
        public int? NewJobtime { get; set; }
        public int? NewResultId { get; set; }
        public string NewTechConclusion { get; set; }
        public int? NewBatteryAction { get; set; }
        public Guid? NewBatteryServiceorder { get; set; }
        public Guid? NewBatteryInstallServiceorder { get; set; }
        public string NewTimetransfer { get; set; }
        public int? NewAutoCreate { get; set; }
        public string NewStartdate { get; set; }
        public string NewSms { get; set; }
        public string NewSmsPnone { get; set; }
        public string NewContactInfo { get; set; }
        public string NewSendedSms { get; set; }
        public bool? NewIsSendSms { get; set; }
        public string NewCommentNew { get; set; }
        public int? NewDutyCount { get; set; }
        public Guid? NewTechniqueEnd { get; set; }
        public string NewAutotechstr { get; set; }
        public bool? NewOpenOnTablet { get; set; }
        public bool? NewMustRead { get; set; }
        public string NewGoToObject { get; set; }
        public bool? NewOperJobNaryad { get; set; }
        public string NewDatetimeCancredirect { get; set; }
        public int? NewPsReglament { get; set; }
        public bool? NewRemoteProgramming { get; set; }
        public bool? NewOperRemove { get; set; }
        public bool? NewNotOperRemove { get; set; }
        public DateTime? NewDateRemove { get; set; }
        public DateTime? NewDateNotRemove { get; set; }
        public bool? NewMountReturn { get; set; }
        public bool? NewMountedGbr { get; set; }
        public bool? NewDeviceIsBack { get; set; }
        public string NewDeviceIsBackComment { get; set; }
        public bool? NewPerenosEnd { get; set; }
        public bool? NewStartReglamentCreation { get; set; }
        public string NewToUrist { get; set; }
        public int? NewOrderFrom { get; set; }
        public DateTime? NewMovedKc { get; set; }
        public bool? NewAutoset { get; set; }
        public string NewHistoryButton { get; set; }
        public int? NewTimeFrom { get; set; }
        public int? NewTimeTo { get; set; }
        //private string _FullName;
        //[NotMapped]
        //public string FullName {
        //    get => NewNumber + Environment.NewLine + NewObjName + Environment.NewLine + NewAddress;
        //}
        //[NotMapped]
        //public string FullInfo {
        //    get {
        //        if(NewDate.HasValue)
        //            return string.Format("Дата: {0} {1} "+Environment.NewLine+"Причина: {2}",NewDate.Value.ToShortDateString(),NewTime,string.IsNullOrEmpty(NewName) ? "<не указана>" : NewName);
        //        else
        //            return string.Format("Дата: <не указана>" + Environment.NewLine + " Причина: {0}",string.IsNullOrEmpty(NewName) ? "<не указана>" : NewName);
        //    }
        //}
        [NotMapped]
        public Color ColorOrder {
            get {
                if (NewIncome.HasValue) {
                    if (NewIncome.Value != null) {
                        return Color.Red;
                    }
                    else {
                        return Color.White;
                    }
                }
                else {
                    return Color.White;
                }
            }
        }
        [NotMapped]
        public Color FrameColor {
            get {
                if (NewOrderFrom.HasValue) {
                    if (NewOrderFrom.Value == 1)//ВИП клиент
{
                        return Color.Red;
                    }
                    else if (NewOrderFrom.Value == 2)//Клиент
{
                        return Color.Yellow;
                    }
                    else if (NewOrderFrom.Value == 3)//Сотрудник витязя
{
                        return Color.Blue;
                    }
                    else {
                        return Color.FromArgb(51, 37, 97);
                    }
                }
                else {
                    return Color.FromArgb(51, 37, 97);
                }
            }
        }
        [NotMapped]
        public string ControlTime { get; set; }

        public virtual NewAndromedaBase NewAndromedaServiceorderNavigation { get; set; }
        public virtual NewServicemanBase NewNewServicemanNavigation { get; set; }
        public virtual NewServicemanBase NewServicemanServiceorderNavigation { get; set; }
        public virtual NewServiceorderBase NewServiceorder { get; set; }
        public virtual NewServicemanBase NewTechniqueEndNavigation { get; set; }
    }
    public class NewServiceorderExtensionBase_ex : NewServiceorderExtensionBase {
        public string ServicemanInfo { get; set; }
        public string ServiceOrderInfo { get; set; }
        public string HeaderServiceOrder { get; set; }
        public bool IsShowed { get; set; } = false;
    }
}
