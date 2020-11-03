using System;

namespace MounterApp.InternalModel {
	public class MountWorksModel {
		public int ObjectNumber { get; set; }
		public string ObjectName { get; set; }
		public string ObjectAddress { get; set; }
		public int Curator { get; set; }
		public int HeadMounter { get; set; }
		public string PathForGBR { get; set; }
		public byte[] CardsPhoto { get; set; }
		public byte[] CardsSchema { get; set; }
		public byte[] CardsShleifs { get; set; }
		public byte[] CardsPeople { get; set; }
		public byte[] CardsEntrace { get; set; }
		public int TypeWork { get; set; }
		public DateTime? DateMount { get; set; }
		public string Mounter { get; set; } 
	}
}
