using System;

namespace Gallery.Model.MobileMe
{
	public class record
	{
		public string guid { get; set; }
		public string userItemGuid { get; set; }
		public string userOrder { get; set; }
		public string title { get; set; }
		public string type { get; set; }
		public string path { get; set; }
		public string keyImageGuid { get; set; }
		public string keyImagePath { get; set; }
		public string keyImageFileExtension { get; set; }
		public string accessLogin { get; set; }
		public string mobileEmail { get; set; }
		public string url { get; set; }
		public string scrubSpritePath { get; set; }
		public int scrubSpriteFrameCount { get; set; }
		public int scrubSpriteKeyFrameIndex { get; set; }
		public int scrubSpriteFrameHeight { get; set; }
		public int scrubSpriteFrameWidth { get; set; }
		public int viewIdentifier { get; set; }
		public int showCaptions { get; set; }
		public int userOrderIndex { get; set; }
		public int sortOrder { get; set; }
		public int numPhotos { get; set; }
		public int numMovies { get; set; }
		public DateTime updated { get; set; }
		public versionInfo versionInfo { get; set; }
		public bool userHidden { get; set; }
		public bool download { get; set; }
		public bool albumWidget { get; set; }
		public bool addPhoto { get; set; }
		public bool allowMobile { get; set; }
		public bool showMobile { get; set; }

		//Movie properties
		public string posterPath { get; set; }
		public int posterImageHeight { get; set; }
		public int posterImageWidth { get; set; }
		public string mimetype { get; set; }
		public string refMoviePath { get; set; }
		public string webImagePath { get; set; }
		public int webImageHeight { get; set; }
		public int webImageWidth { get; set; }
		public string fileExtension { get; set; }
		public bool addComment { get; set; }
		public string description { get; set; }
		//Mobile
		public string videoPathMobile { get; set; }
		public int videoHeightMobile { get; set; }
		public int videoWidthMobile { get; set; }
		public decimal videoDurationMobile { get; set; }
		//Small
		public string videoPathSmall { get; set; }
		public int videoHeightSmall { get; set; }
		public int videoWidthSmall { get; set; }
		public decimal videoDurationSmall { get; set; }
		//Medium
		public string videoPathMedium { get; set; }
		public int videoHeightMedium { get; set; }
		public int videoWidthMedium { get; set; }
		public decimal videoDurationMedium { get; set; }
	}
}
