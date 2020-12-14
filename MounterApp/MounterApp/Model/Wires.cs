using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.Model {
    public class Wires {
        public int? ID { get; set; }
        public string Desc { get; set; }
        public string FullInfo {
            get {
                if(ID.HasValue)
                    return "№ " + ID.Value.ToString().Trim() + " - " + Desc.Trim();
                else
                    return Desc;
            }
        }
    }
}
