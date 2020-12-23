using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MounterApp.InternalModel {
    public class PhotoCollection {
        public PhotoCollection(Guid iD,string comment,string path,MediaFile file,ImageSource imgSrc,PhotoTypes types) {
            ID = iD;
            Comment = comment;
            Path = path;
            File = file;
            ImgSrc = imgSrc;
            _Types = types;
        }
        public PhotoCollection(Guid _id, string _data, string _comment) {
            ID = _id;
            Data = _data;
            Comment = _comment;
        }
        public PhotoCollection(Guid _id,string _data,string _comment,ImageSource _imgsrc,PhotoTypes types) {
            ID = _id;
            Data = _data;
            Comment = _comment;
            ImgSrc = _imgsrc;
            _Types = types;
        }

        public Guid ID { get; set; }
        public string Data { get; set; }
        public string Comment { get; set; }
        public string Path { get; set; }
        public MediaFile File { get; set; }
        public ImageSource ImgSrc { get; set; }
        public PhotoTypes _Types {get;set;}
        public bool IsUse { get; set; }
    }
}
