using System;
using Gallery.Classes.MobileMe;

namespace Gallery.Classes
{
	public class Photo
	{
		#region : Public Properties :	

		public string Title { get; set; }
		public string LargeImagePath { get; set; }	
		public string WebImagePath { get; set; }
		public int WebImageHeight { get; set; }
		public int WebImageWidth { get; set; }
		public DateTime PhotoDate { get; set; }
		public bool UserHidden { get; set; }
		public string UserItemGuid { get; set; }
		public int SortOrder { get; set; }
		public DateTime ModDate { get; set; }
		public string Guid { get; set; }
		public string Content { get; set; }
		public int UserOrderIndex { get; set; }
		public string FileExtension { get; set; }
		public DateTime ArchiveDate { get; set; }
		public versionInfo VersionInfo { get; set; }
		public string Type { get; set; }
		public DateTime Updated { get; set; }
		public string Url { get; set; }
		public string ImageUrl
		{
			get
			{
				if (this.Type == "Album")
				{
					return string.Format("{0}/{1}", this.Url, this.WebImagePath);
				}
				else
				{
					return string.Format("{0}.{1}?derivative=medium&source={2}&type=medium", this.Url, this.FileExtension, this.WebImagePath);
				}
			}
		}
		public int ViewIdentifier { get; set; }
		public string Album { get; set; }

		#endregion : Public Properties :

		#region : Constructor :

		public Photo(photo photo)
		{
			this.Title = photo.title;
			this.LargeImagePath = photo.largeImagePath;
			this.WebImagePath = photo.webImagePath;
			this.WebImageHeight = photo.webImageHeight;
			this.WebImageWidth = photo.webImageWidth;
			this.PhotoDate = photo.photoDate;
			this.UserHidden = photo.userHidden;
			this.UserItemGuid = photo.userItemGuid;
			this.SortOrder = photo.sortOrder;
			this.ModDate = photo.modDate;
			this.Guid = photo.guid;
			this.Content = photo.content;
			this.UserOrderIndex = photo.userOrderIndex;
			this.FileExtension = photo.fileExtension;
			this.ArchiveDate = photo.archiveDate;
			this.VersionInfo = photo.versionInfo;
			this.Type = photo.type;
			this.Updated = photo.updated;
			this.Url = photo.url;
			this.ViewIdentifier = photo.viewIdentifier;
			this.Album = photo.album;
		}

		#endregion : Constructor :

	} //end public class Photo

} //end namespace Gallery.Classes