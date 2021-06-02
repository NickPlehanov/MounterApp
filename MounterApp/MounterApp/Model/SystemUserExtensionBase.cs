using System;
using System.Collections.Generic;

namespace MounterApp.Model {
    public class SystemUserExtensionBase {
        public SystemUserExtensionBase(Guid systemUserId, string newPassword, bool? newIsCurator, bool? newIsHeadmounter, string newTelNumberAuth) {
            SystemUserId = systemUserId;
            NewPassword = newPassword;
            NewIsCurator = newIsCurator;
            NewIsHeadmounter = newIsHeadmounter;
            NewTelNumberAuth = newTelNumberAuth;
        }

        //[JsonProperty("systemUserId")]
        public Guid SystemUserId { get; set; }
        //[JsonProperty("newPassword")]
        public string NewPassword { get; set; }
        //[JsonProperty("newIsCurator")]
        public bool? NewIsCurator { get; set; }
        //[JsonProperty("newIsHeadmounter")]
        public bool? NewIsHeadmounter { get; set; }
        //[JsonProperty("newTelNumberAuth")]
        public string NewTelNumberAuth { get; set; }
    }
    public class SystemUserExtensionBaseContainer {
        public List<SystemUserExtensionBase> ExtensionUser { get; set; }
    }
}
