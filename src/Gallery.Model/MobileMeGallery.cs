using System;
using System.Collections.Generic;
using System.Linq;

namespace Gallery.Model
{
	public class MobileMeGallery
	{
		#region : Public Properties :

		/// <summary>
		/// title
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// updated
		/// </summary>
		public DateTime Updated { get; set; }

		/// <summary>
		/// apertureUserOrder
		/// </summary>
		public string ApertureUserOrder { get; set; }

		/// <summary>
		/// iMovieUserOrder
		/// </summary>
		public string iMovieUserOrder { get; set; }

		/// <summary>
		/// userOrder
		/// </summary>
		public string UserOrder { get; set; }

		/// <summary>
		/// Gallery Albums
		/// </summary>
		public List<Album> Albums
		{
			get { return (from item in Items where item.Type == GalleryItemType.Album select item as Album).ToList<Album>(); }
		}

		/// <summary>
		/// Gallery Movies
		/// </summary>
		public List<Movie> Movies
		{
			get { return (from item in Items where item.Type == GalleryItemType.Movie select item as Movie).ToList<Movie>(); }
		}

		/// <summary>
		/// Gallery Items
		/// </summary>
		public List<GalleryItem> Items { get; set; }

		#endregion : Public Properties :

	} //end public class Gallery

} //end namespace Gallery.Model
