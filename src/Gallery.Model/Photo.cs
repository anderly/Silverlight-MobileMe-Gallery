using System;
using Gallery.Model.MobileMe;

namespace Gallery.Model
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

	} //end public class Photo

} //end namespace Gallery.Model