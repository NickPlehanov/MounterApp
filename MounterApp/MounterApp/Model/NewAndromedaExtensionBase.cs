using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.Model {
    public partial class NewAndromedaExtensionBase {
        public string NewAddress { get; set; }
        public Guid NewAndromedaId { get; set; }
        public string NewCorp { get; set; }
        public string NewName { get; set; }
        public int? NewNumber { get; set; }
        public string NewObjtype { get; set; }
        public string NewPhone { get; set; }
        public string NewSquare { get; set; }
        public string NewStartdate { get; set; }
        public string NewType { get; set; }
        public string NewPhone2 { get; set; }
        public string NewEnddate { get; set; }
        public Guid? NewPost { get; set; }
        public string NewSmsPhone { get; set; }
        public Guid? NewContactAndromeda { get; set; }
        public string NewContactInfo { get; set; }
        public bool? NewRemoteProgramming { get; set; }
        public bool? NewIsToPs { get; set; }
        public int? NewRoute { get; set; }

        public virtual NewAndromedaBase NewAndromeda { get; set; }
    }
}
