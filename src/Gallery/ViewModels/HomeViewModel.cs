using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Gallery.Model;

namespace Gallery.ViewModels
{
	public class HomeViewModel : ViewModel, IHomeViewModel
	{
		#region : Private Fields :

		private string _lastUpdated = string.Empty;
		private string _title = string.Empty;
		private string _albumText = "Albums";
		private string _movieText = "Movies";
		private ObservableCollection<Album> _albums;
		private ObservableCollection<Movie> _movies;

		#endregion : Private Fields :

		#region : Public Properties :

		public string LastUpdated
		{
			get { return _lastUpdated; }
			set
			{
				if (value == _lastUpdated) return;
				_lastUpdated = value;
				this.NotifyPropertyChanged(() => this.LastUpdated);
			}
		}

		public string Title
		{
			get { return _title; }
			set
			{
				if (value == _title) return;
				_title = value;
				this.NotifyPropertyChanged(() => this.Title);
				this.NotifyPropertyChanged(() => this.PageTitle);
			}
		}

		public string PageTitle
		{
			get { return string.Format(Config.MobileMePageTitleFormat, _title); }
		}

		public string AlbumText
		{
			get { return _albumText; }
			set
			{
				if (value == _albumText) return;
				_albumText = value;
				NotifyPropertyChanged(() => this.AlbumText);
			}
		}

		public string MovieText
		{
			get { return _movieText; }
			set
			{
				if (value == _movieText) return;
				_movieText = value;
				NotifyPropertyChanged(() => this.MovieText);
			}
		}

		public ObservableCollection<Album> Albums
		{
			get { return _albums; }
		}

		public ObservableCollection<Movie> Movies
		{
			get { return _movies; }
		}

		#endregion : Public Properties :

		#region : Constructor :

		public HomeViewModel()
		{
			_albums = new ObservableCollection<Album>();
			_movies = new ObservableCollection<Movie>();

			Messenger.Register<MobileMeGallery>(this, Messages.GalleryLoaded, LoadViewModel);
			if (GalleryContext.IsLoaded)
			{
				LoadViewModel(GalleryContext.Gallery);
			}
		}

		#endregion : Constructor :

		#region : Public Methods :

		/// <summary>
		/// Retrieves original image clipping and renderTransform settings
		/// </summary>
		/// <param name="img"></param>
		/// <param name="clip"></param>
		/// <param name="renderTransform"></param>
		public void GetOriginalImage(Image img, out RectangleGeometry clip, out TranslateTransform renderTransform)
		{
			MobileMeGallery gallery = GalleryContext.Gallery;
			var galleryItem = gallery.Items.Where(o => o.ScrubSpritePath == ((BitmapImage)img.Source).UriSource.ToString()).FirstOrDefault();

			RectangleGeometry rg = galleryItem.ImageClip;
			TranslateTransform tt = galleryItem.ImageTransform;

			clip = rg;
			renderTransform = tt;
		}

		/// <summary>
		/// Retrieves scrubbing image clipping and renderTransform settings based on passed Point (mouse coordinates)
		/// </summary>
		/// <param name="img"></param>
		/// <param name="point"></param>
		/// <param name="clip"></param>
		/// <param name="renderTransform"></param>
		public void GetScrubbingImage(Image img, Point point, out RectangleGeometry clip, out TranslateTransform renderTransform)
		{
			MobileMeGallery gallery = GalleryContext.Gallery;
			var galleryItem = gallery.Items.Where(o => o.ScrubSpritePath == ((BitmapImage)img.Source).UriSource.ToString()).FirstOrDefault();

			int scrubSpriteFrameCount = galleryItem.ScrubSpriteFrameCount;
			int scrubSpriteFrameHeight = galleryItem.ScrubSpriteFrameHeight;
			int scrubSpriteFrameWidth = galleryItem.ScrubSpriteFrameWidth;

			int frameWidth = scrubSpriteFrameWidth / scrubSpriteFrameCount;

			// Calculate the desired Y offset based on the X position of the mouse pointer
			double yOffset = Math.Floor(point.X / frameWidth);

			// Set the Image Clip and Image RenderTransform with the new values
			var rg = new RectangleGeometry { RadiusX = 10, RadiusY = 10, Rect = new Rect(0, scrubSpriteFrameHeight * yOffset, scrubSpriteFrameWidth, scrubSpriteFrameHeight) };
			clip = rg;
			renderTransform = new TranslateTransform { X = 0, Y = rg.Rect.Y * -1 };
		}

		#endregion : Public Methods :

		#region : Private Methods :

		private void LoadViewModel(MobileMeGallery gallery)
		{
			LastUpdated = gallery.Updated.ToString("d");
			Title = gallery.Title;

			// Load the albums
			gallery.Albums.OrderByDescending(album => album.SortOrder).ToList().ForEach(_albums.Add);
			// Load the movies
			gallery.Movies.OrderBy(movie => movie.SortOrder).ToList().ForEach(_movies.Add);
			OnLoaded();
		}

		#endregion : Private Methods :

	} //end public class HomeViewModel : ViewModelBase

} //end namespace Gallery.ViewModels
