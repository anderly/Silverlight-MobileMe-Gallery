using System;
using System.Collections.Generic;
using System.Linq;
using Gallery.Classes.MobileMe;

namespace Gallery.Classes
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

		#region : Constructors :

		public MobileMeGallery()
		{
		
		}

		public MobileMeGallery(gallery gallery)
		{
			//set gallery settings
			this.Title = gallery.data.title;
			this.Updated = gallery.data.updated;
			this.ApertureUserOrder = gallery.data.apertureUserOrder;
			this.iMovieUserOrder = gallery.data.iMovieUserOrder;
			this.UserOrder = gallery.data.userOrder;

			this.Items = new List<GalleryItem>();

			getAlbums(gallery);
			getMovies(gallery);

		}

		#endregion : Constructors :

		#region : Private Helper Methods :

		private void getAlbums(gallery gallery)
		{
			List<record> albums = (from rec in gallery.records
					  where rec.type == "Album"
					  select rec).ToList();

			albums.ForEach(o =>
			{
				this.Items.Add(new Album(o));
			});

			var items = (from item in this.Albums
							 orderby Updated descending
							 select item).ToList();

			if (items.FirstOrDefault().SortOrder == 0)
			{
				var i = 1;
				items.ForEach(a =>
				{
					a.SortOrder = i;
					i++;
				});
			}

		} //end private void getAlbums(gallery gallery)

		private void getMovies(gallery gallery)
		{
			List<record> movies = (from rec in gallery.records
							 where rec.type == "Movie"
							 select rec).ToList();

			movies.ForEach(o =>
			{
				this.Items.Add(new Movie(o));
			});

			var items = (from item in this.Movies
							 orderby Updated descending
							 select item).ToList();

			if (items.FirstOrDefault().SortOrder == 0)
			{
				var i = 1;
				items.ForEach(a =>
				{
					a.SortOrder = i;
					i++;
				});
			}
			

		} //end private void getMovies(gallery gallery)

		#endregion : Private Helper Methods :

	} //end public class Gallery

} //end namespace Gallery.Classes
