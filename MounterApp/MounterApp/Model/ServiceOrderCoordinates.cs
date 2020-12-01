using System;

namespace MounterApp.Model {
    public partial class ServiceOrderCoordinates {
        public Guid SocId { get; set; }
        public Guid SocServiceOrderId { get; set; }
        public string SocIncomeLatitude { get; set; }
        public string SocIncomeLongitude { get; set; }
        public string SocOutcomeLatitide { get; set; }
        public string SocOutcomeLongitude { get; set; }
    }
}
