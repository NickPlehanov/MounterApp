using System;

namespace MounterApp.InternalModel {
    public class PhotoTypes {
        public Guid PhotoTypeId { get; set; }
        public string PhotoTypeName { get; set; }
        public bool IsVisible { get; set; } = true;
    }
}
