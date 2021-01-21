namespace MounterApp.Model {
    public class Info {
        public string Name { get; set; }
        public string Address { get; set; }
        public int ObjectNumber { get; set; }
        public bool IsArm { get; set; }
        public bool IsFire { get; set; }
        public bool IsPanic { get; set; }
        public string ObjTypeName { get; set; }
        public string EventTemplateName { get; set; }
        public int ControlTime { get; set; }
        public string ObjectPassword { get; set; }
        public bool RemoteProgramming { get; set; }
        //public string EventTemplate { get; set; }
        public string DeviceName { get; set; }
    }
}
