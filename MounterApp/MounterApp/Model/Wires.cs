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
                    return string.Format("№ {0} - {1}",ID.Value.ToString().Trim(),Desc.Trim());
                    //return "№ " + ID.Value.ToString().Trim() + " - " + Desc.Trim();
                else
                    return string.Format("{0}",Desc.Trim());
                //return Desc;
            }
        }
    }
}
