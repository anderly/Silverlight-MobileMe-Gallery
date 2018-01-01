using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Gallery.Classes.MobileMe;

namespace Gallery.Classes
{
	public class Album : GalleryItem
	{
		#region : Public Properties :

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

		#region : Constructors :

		public Album()
		{

		} //end public Album()

		public Album(record album) : base(album)
		{
			this.Path = album.path;
			this.UserOrder = album.userOrder;
			this.UserOrderIndex = album.userOrderIndex;
			this.KeyImageGuid = album.keyImageGuid;
			this.KeyImagePath = album.keyImagePath;
			this.KeyImageFileExtension = album.keyImageFileExtension;
			this.PhotoCount = album.numPhotos;
			this.MovieCount = album.numMovies;
			this.AlbumWidget = album.albumWidget;
			this.AddPhoto = album.addPhoto;
			this.AllowMobile = album.allowMobile;
			this.ShowMobile = album.showMobile;
			this.MobileEmail = album.mobileEmail;

			this.Photos = new List<Photo>();

		} //end public Album(record album)

		#endregion : Constructors :

		public void GetPhotos(Stream data)
		{
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(album));
			album album = (album)ser.ReadObject(data);

			album.records.Where(i => i.type == "Photo").ToList().ForEach(item => this.Photos.Add(new Photo(item)));
		}

	} //public class Album : GalleryItem

} //end namespace Gallery.Classes
