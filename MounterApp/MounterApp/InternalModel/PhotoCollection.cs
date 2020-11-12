using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.InternalModel {
    public class PhotoCollection {
        public PhotoCollection(Guid iD,Guid type,string comment,string path,MediaFile file) {
            ID = iD;
            Type = type;
            Comment = comment;
            Path = path;
            File = file;
        }

        public Guid ID { get; set; }
        public Guid Type { get; set; }
        public string Comment { get; set; }
        public string Path { get; set; }
        public MediaFile File { get; set; }
    }
}
