using System.Collections.Generic;

namespace Gallery.Model
{
	public class Album : GalleryItem
	{
		#region : Public Properties :

		/// <summary>
		/// path
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		/// path
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// userOrder
		/// </summary>
		public string UserOrder { get; set; }

		/// <summary>
		/// userOrderIndex
		/// </summary>
		public int UserOrderIndex { get; set; }

		/// <summary>
		/// keyImageGuid
		/// </summary>
		public string KeyImageGuid { get; set; }

		/// <summary>
		/// keyImagePath
		/// </summary>
		public string KeyImagePath { get; set; }

		/// <summary>
		/// keyImageFileExtension
		/// </summary>
		public string KeyImageFileExtension { get; set; }

		public int Count
		{
			get
			{
				return PhotoCount + MovieCount;
			}
		}

		/// <summary>
		/// numPhotos
		/// </summary>
		public int PhotoCount { get; set; }

		/// <summary>
		/// numMovies
		/// </summary>
		public int MovieCount { get; set; }

		/// <summary>
		/// albumWidget
		/// </summary>
		public bool AlbumWidget { get; set; }

		/// <summary>
		/// addPhoto
		/// </summary>
		public bool AddPhoto { get; set; }

		/// <summary>
		/// allowMobile
		/// </summary>
		public bool AllowMobile { get; set; }

		/// <summary>
		/// showMobile
		/// </summary>
		public bool ShowMobile { get; set; }

		/// <summary>
		/// mobileEmail - email post address
		/// </summary>
		public string MobileEmail { get; set; }

		public List<Photo> Photos { get; set; }

		#endregion : Public Properties :

		#region : Constructor :

		public Album()
		{
			IsLoaded = false;
		}

		#endregion : Constructor :

	} //public class Album : GalleryItem

} //end namespace Gallery.Model
