using System;
using System.Collections.Generic;
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
    }
}
