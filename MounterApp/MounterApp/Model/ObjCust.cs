using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MounterApp.Model {
    public partial class ObjCust {
        public int RecordId { get; set; }
        public int ObjectId { get; set; }
        public int? OrderNumber { get; set; }
        public int? UserNumber { get; set; }
        public string ObjCustTitle { get; set; }
        public string ObjCustName { get; set; }
        public string ObjCustPhone1 { get; set; }
        public string ObjCustPhone2 { get; set; }
        public string ObjCustPhone3 { get; set; }
        public string ObjCustPhone4 { get; set; }
        public string ObjCustPhone5 { get; set; }
        public string ObjCustAddress { get; set; }
        public int? NordKeyId { get; set; }
        public string Pincode { get; set; }
        public bool? IsVisibleInCabinet { get; set; }
        public bool? ReclosingRequest { get; set; }
        public bool? ReclosingFailure { get; set; }
        public string MyAlarmUserId { get; set; }
        public int? MyAlarmUserRole { get; set; }
        public string MyAlarmUserPhone { get; set; }
        public Guid ObjCustGuid { get; set; }
        public bool? MyAlarmPanicEnabled { get; set; }
        [NotMapped]
        public string UserNumberText {
            get {
                if(!int.TryParse(UserNumber.ToString(),out _))
                    return "Номер пользователя: <не определен>";
                else
                    return string.Format("Номер пользователя: {0}",UserNumber);
            }
        }
        [NotMapped]
        public string ObjCustTitleText {
            get {
                if(string.IsNullOrEmpty(ObjCustTitle))
                    return "Должность: <не указана>";
                else
                    return string.Format("Должность: {0}",ObjCustTitle);
            }
        }
        [NotMapped]
        public string ObjCustNameText {
            get {
                if(string.IsNullOrEmpty(ObjCustName))
                    return "ФИО: <не указаны>";
                else
                    return string.Format("ФИО: {0}",ObjCustName);
            }
        }
        [NotMapped]
        public string ObjCustPhone1Text {
            get {
                if(string.IsNullOrEmpty(ObjCustPhone1))
                    return "Телефон: <не указан>";
                else
                    return string.Format("Телефон: {0}",ObjCustPhone1);
            }
        }

        public virtual A28Object Object { get; set; }
    }
}
