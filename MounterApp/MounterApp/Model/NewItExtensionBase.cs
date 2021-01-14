using System;

namespace MounterApp.Model {
    public class NewItExtensionBase {
        public Guid NewItId { get; set; }
        public string NewName { get; set; }
        public string NewComment { get; set; }
        public string NewOwner { get; set; }
        public string NewResult { get; set; }
        public bool? NewIsWork { get; set; }
        public bool? NewExecutor { get; set; }
        public bool? NewComplet { get; set; }
        public bool? NewItdepartment { get; set; }
        public int? NewPriority { get; set; }
        public int? NewExecutorv { get; set; }
        public bool? NewCheckTask { get; set; }

        public virtual NewItBase NewIt { get; set; }
    }
}
