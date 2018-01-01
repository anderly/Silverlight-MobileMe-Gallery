using System;
using System.Windows;
using System.Windows.Media;
using Gallery.Model.MobileMe;

namespace Gallery.Model
{
	public class GalleryItem
	{
		private const string AlbumSubTitleFormat = "{0} photo{1}{2}";

		#region : Public Properties :

		/// <summary>
		/// guid
		/// </summary>
		public string ID
		{
			get { return this.Url.Substring(this.Url.LastIndexOf('/') + 1); }
		}

		public string NavigateUri
		{
			get
			{
				if (this.Type == GalleryItemType.Album)
				{
					return string.Format("{0}&bgcolor=black&view=grid", this.ID);
				}
				else
				{
					return this.ID;
				}
			}
		}

		/// <summary>
		/// guid
		/// </summary>
		public string Guid { get; set; }

		/// <summary>
		/// userItemGuid
		/// </summary>
		public string UserItemGuid { get; set; }

		/// <summary>
		/// title
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// title
		/// </summary>
		public string DisplayTitle
		{
			get
			{
				string displayTitle = string.Empty;
				if (this.Title.Length <= 22)
				{
					displayTitle = this.Title;
				}
				else
				{
					displayTitle = string.Format("{0}...", this.Title.Substring(0, 21).TrimEnd());
				}
				return displayTitle;
			}
		}

		/// <summary>
		/// title
		/// </summary>
		public string SubTitle
		{
			get
			{
				string subTitle = string.Empty;
				if (this.Type == GalleryItemType.Album)
				{
					var album = this as Album;
					var photoCount = album.PhotoCount;
					var movieCount = album.MovieCount;
					subTitle = string.Format(AlbumSubTitleFormat, photoCount, photoCount > 1 ? "s" : string.Empty, movieCount > 0 ? string.Format(" and {0} movie{1}", movieCount, movieCount > 1 ? "s" : string.Empty) : string.Empty);
				}
				else
				{
					subTitle = ((Movie)this).VideoDurationDisplay;
				}
				return subTitle;
			}
		}

		/// <summary>
		/// type
		/// </summary>
		public GalleryItemType Type { get; set; }

		/// <summary>
		/// url
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// updated
		/// </summary>
		public DateTime Updated { get; set; }

		/// <summary>
		/// sortOrder
		/// </summary>
		public int SortOrder { get; set; }

		/// <summary>
		/// showCaptions
		/// </summary>
		public int ShowCaptions { get; set; }

		/// <summary>
		/// accessLogin
		/// </summary>
		public string AccessLogin { get; set; }

		/// <summary>
		/// scrubSpritePath
		/// </summary>
		public string ScrubSpritePath { get; set; }

		/// <summary>
		/// scrubSpriteFrameCount
		/// </summary>
		public int ScrubSpriteFrameCount { get; set; }

		/// <summary>
		/// scrubSpriteKeyFrameIndex
		/// </summary>
		public int ScrubSpriteKeyFrameIndex { get; set; }

		/// <summary>
		/// scrubSpriteFrameHeight
		/// </summary>
		public int ScrubSpriteFrameHeight { get; set; }

		/// <summary>
		/// ScrubSpriteActualHeight
		/// </summary>
		public int ScrubSpriteActualHeight
		{
			get { return this.ScrubSpriteFrameHeight * this.ScrubSpriteFrameCount; }
		}

		/// <summary>
		/// scrubSpriteFrameWidth
		/// </summary>
		public int ScrubSpriteFrameWidth { get; set; }

		public RectangleGeometry BorderClip
		{
			get
			{
				RectangleGeometry rg = new RectangleGeometry();
				rg.RadiusX = 10;
				rg.RadiusY = 10;
				if (this.Type == GalleryItemType.Album)
				{
					rg.Rect = new Rect(0, 0, this.ScrubSpriteFrameWidth, this.ScrubSpriteFrameHeight);
				}
				else
				{
					rg.Rect = new Rect(0, 0, 160, 100);
				}
				return rg;
			}
		}

		public RectangleGeometry ImageClip
		{
			get
			{
				RectangleGeometry rg = new RectangleGeometry();
				rg.RadiusX = 10;
				rg.RadiusY = 10;
				rg.Rect = new Rect(0, this.ScrubSpriteKeyFrameIndex * this.ScrubSpriteFrameHeight, this.ScrubSpriteFrameWidth, this.ScrubSpriteFrameHeight);
				return rg;
			}
		}

		public RectangleGeometry MovieClip
		{
			get
			{
				RectangleGeometry rg = new RectangleGeometry();
				rg.RadiusX = 10;
				rg.RadiusY = 10;
				rg.Rect = new Rect(0, this.ScrubSpriteKeyFrameIndex * this.ScrubSpriteFrameHeight, 160, 100);
				return rg;
			}
		}

		public TranslateTransform ImageTransform
		{
			get
			{
				TranslateTransform tt = new TranslateTransform();
				tt.X = 0;
				if (this.Type == GalleryItemType.Album)
				{
					tt.Y = this.ImageClip.Rect.Y * -1;
				}
				else
				{
					tt.Y = this.MovieClip.Rect.Y * -1;
				}
				return tt;
			}
		}

		/// <summary>
		/// viewIdentifier
		/// </summary>
		public int ViewIdentifier { get; set; }

		/// <summary>
		/// versionInfo
		/// </summary>
		public versionInfo VersionInfo { get; set; }

		/// <summary>
		/// userHidden
		/// </summary>
		public bool UserHidden { get; set; }

		/// <summary>
		/// download
		/// </summary>
		public bool Download { get; set; }

		#endregion : Public Properties :

	} //end public class GalleryItem

} //end namespace Gallery.Model
