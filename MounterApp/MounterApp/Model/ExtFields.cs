using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.Model {
    public class ExtFields {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string FullInfo {
            get => FieldName + " :" + FieldValue;
        }
    }
}
