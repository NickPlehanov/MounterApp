using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MounterApp.Model {
    public class GetEventsReceivedFromObject_Result {
        public DateTime? REDateTime { get; set; }
        public DateTime? RESaveDT { get; set; }
        public long AlarmIndex { get; set; }
        public string RChannelName { get; set; }
        public string EventCode { get; set; }
        public string EventClassName { get; set; }
        public int? ZoneUser { get; set; }
        public int? PartNumber { get; set; }
        public string EventDesc { get; set; }
        [NotMapped]
        public string ZoneUserText {
            get => string.Format("Зона: {0}",ZoneUser);
        }
        [NotMapped]
        public string PartNumberText {
            get => string.Format("Раздел: {0}",PartNumber);
        }
        [NotMapped]
        public string RChannelNameText {
            get => string.Format("Канал: {0}",RChannelName);
        }
        [NotMapped]
        public string EventCodeText {
            get => string.Format("Код: {0}",EventCode);
        }
        [NotMapped]
        public string EventClassNameText {
            get => string.Format("Класс: {0}",EventClassName);
        }
    }
}
