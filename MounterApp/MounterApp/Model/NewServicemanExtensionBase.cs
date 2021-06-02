using System;

namespace MounterApp.Model {
    public partial class NewServicemanExtensionBase {
        public Guid NewServicemanId { get; set; }
        public string NewName { get; set; }
        public string NewPhone { get; set; }
        public string NewPassword { get; set; }
        public int? NewDinner { get; set; }
        public bool? NewDinnerNew { get; set; }
        public int? NewTechniqueType { get; set; }
        public int? NewCategory { get; set; }
        public Guid? NewGenTech { get; set; }
        public int? NewRoute1 { get; set; }
        public int? NewRoute2 { get; set; }
        public int? NewRoute3 { get; set; }
        public bool? NewIswork { get; set; }

        public virtual NewServicemanBase NewGenTechNavigation { get; set; }
        public virtual NewServicemanBase NewServiceman { get; set; }
    }
}
