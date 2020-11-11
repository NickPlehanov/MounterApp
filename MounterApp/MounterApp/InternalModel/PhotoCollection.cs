using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.InternalModel {
    public class PhotoCollection {
        public PhotoCollection(Guid iD,Guid type,string comment,string path) {
            ID = iD;
            Type = type;
            Comment = comment;
            Path = path;
        }

        public Guid ID { get; set; }
        public Guid Type { get; set; }
        public string Comment { get; set; }
        public string Path { get; set; }
    }
}
