using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Text;
using Xamarin.Essentials;

namespace MounterApp.Model {
    public partial class NewTest2ExtensionBase {
        public Guid NewTest2Id { get; set; }
        public string NewName { get; set; }
        public string NewAddress { get; set; }
        public Guid? NewAndromedaServiceorder { get; set; }
        public int? NewAutocreatePs { get; set; }
        public string NewAutotechstr { get; set; }
        public int? NewCategory { get; set; }
        public string NewComments { get; set; }
        public string NewContactInfo { get; set; }
        public DateTime? NewDate { get; set; }
        public string NewDatetimeCancredirect { get; set; }
        public DateTime? NewIncome { get; set; }
        public int? NewJobtime { get; set; }
        public DateTime? NewMoved { get; set; }
        public string NewMovedFrom { get; set; }
        public Guid? NewNewServicemanPs { get; set; }
        public int? NewNumber { get; set; }
        public string NewObjName { get; set; }
        public DateTime? NewOutgone { get; set; }
        public int? NewPsReglament { get; set; }
        public int? NewResult { get; set; }
        public int? NewResultid { get; set; }
        public Guid? NewServicemanServiceorderPs { get; set; }
        public string NewStartdate { get; set; }
        public string NewTechconclusion { get; set; }
        public Guid? NewTechniqueEnd { get; set; }
        public string NewTime { get; set; }
        public string NewTimetransfer { get; set; }
        public string NewTransferReason { get; set; }
        public string NewWhoInit { get; set; }
        public int? NewReglamentPs { get; set; }
        public string NewRoute { get; set; }
        public bool? NewPerenosEnd { get; set; }
        public bool? NewStartReglamentCreation { get; set; }
        public string NewAddToYear { get; set; }
        public bool? NewAddToYearBool { get; set; }
        //[NotMapped]
        //public string FullName {
        //    get => NewNumber + Environment.NewLine + NewObjName + Environment.NewLine + NewAddress;
        //    //set {
        //    //    _FullName = value;
        //    //}
        //}
        //[NotMapped]
        //public string FullInfo {
        //    get {
        //        if(NewDate.HasValue)
        //            return string.Format("Дата: {0} {1} Причина: {2}",NewDate.Value.ToShortDateString(),NewTime,string.IsNullOrEmpty(NewName) ? "<не указана>" : NewName);
        //        //return NewDate.Value.ToShortDateString()+" " + NewName;
        //        else
        //            return string.Format("Дата: <не указана>; Причина: {0}",string.IsNullOrEmpty(NewName) ? "<не указана>" : NewName);
        //    }
        //}
        [NotMapped]
        public Color ColorOrder {
            get {
                if(NewIncome.HasValue) {
                    if(NewIncome.Value != null)
                        return Color.Red;
                    else {
                        AppTheme appTheme = AppInfo.RequestedTheme;
                        if(appTheme == AppTheme.Light)
                            return Color.White;
                        else
                            return Color.Black;
                    }
                }
                else {
                    AppTheme appTheme = AppInfo.RequestedTheme;
                    if(appTheme == AppTheme.Light)
                        return Color.White;
                    else
                        return Color.Black;
                }
            }
        }
        [NotMapped]
        public string ControlTime { get; set; }

        public virtual NewAndromedaBase NewAndromedaServiceorderNavigation { get; set; }
        public virtual NewServicemanBase NewNewServicemanPsNavigation { get; set; }
        public virtual NewServicemanBase NewServicemanServiceorderPsNavigation { get; set; }
        public virtual NewServicemanBase NewTechniqueEndNavigation { get; set; }
        public virtual NewTest2Base NewTest2 { get; set; }
    }
    public class NewTest2ExtensionBase_ex : NewTest2ExtensionBase {
        public string ServicemanInfo { get; set; }
        public string ServiceOrderInfo { get; set; }
        public string HeaderServiceOrder { get; set; }
    }
}
