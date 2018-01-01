using System;

namespace Gallery.Model.MobileMe
{
	public class photo
	{
		public string largeImagePath { get; set; }
		public string webImagePath { get; set; }
		public string title { get; set; }
		public int webImageHeight { get; set; }
		public int webImageWidth { get; set; }
		public DateTime photoDate { get; set; }
		public bool userHidden { get; set; }
		public string userItemGuid { get; set; }
		public int sortOrder { get; set; }
		public DateTime modDate { get; set; }
		public string guid { get; set; }
		public string content { get; set; }
		public int userOrderIndex { get; set; }
		public string fileExtension { get; set; }
		public DateTime archiveDate { get; set; }
		public versionInfo versionInfo { get; set; }
		public string type { get; set; }
		public DateTime updated { get; set; }
		public string url { get; set; }
		public int viewIdentifier { get; set; }
		public string album { get; set; }
	}
}
