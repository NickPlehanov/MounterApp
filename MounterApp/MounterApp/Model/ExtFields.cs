namespace MounterApp.Model {
    public class ExtFields {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string FullInfo {
            get => FieldName + " :" + FieldValue;
        }
    }
}
