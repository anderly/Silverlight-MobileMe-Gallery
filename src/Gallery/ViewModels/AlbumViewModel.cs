using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Gallery.Model;

namespace Gallery.ViewModels
{
	public class AlbumViewModel : ViewModel
	{
		#region : Private Fields :

		private string _title = string.Empty;
		private string _albumText = "Albums";
		private string _movieText = "Movies";
		private readonly ObservableCollection<Photo> _photos;

		#endregion : Private Fields :

		#region : Public Properties :

		public string Title
		{
			get { return _title; }
			set
			{
				if (value == _title) return;
				_title = value;
				NotifyPropertyChanged(() => this.Title);
				NotifyPropertyChanged(() => this.PageTitle);
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

		public ObservableCollection<Photo> Photos
		{
			get { return _photos; }
		}

		#endregion : Public Properties :

		#region : Constructor :

		public AlbumViewModel()
		{
			_photos = new ObservableCollection<Photo>();

			Messenger.Register<Album>(this, Messages.AlbumLoaded, LoadViewModel);
			// Not required since it appears that Messenger will notify caller of missed events
			//if (GalleryContext.SelectedItem != null)
			//{
			//    LoadViewModel(GalleryContext.SelectedItem as Album);
			//}
		}

		#endregion : Constructor :

		#region : Dependency Properties :

		public static readonly DependencyProperty PhotoWidthProperty =
			DependencyProperty.Register(
				"PhotoWidth",
				typeof(double),
				typeof(AlbumViewModel),
				new PropertyMetadata(new PropertyChangedCallback(OnPhotoWidthPropertyChanged))
			);

		public double PhotoWidth
		{
			get { return (double)GetValue(PhotoWidthProperty); }
			set { SetValue(PhotoWidthProperty, value); }
		}

		public static readonly DependencyProperty PhotoTitleFontSizeProperty =
			DependencyProperty.Register(
				"PhotoTitleFontSize",
				typeof(double),
				typeof(AlbumViewModel),
				new PropertyMetadata(new PropertyChangedCallback(OnPhotoTitleFontSizePropertyChanged))
			);

		public double PhotoTitleFontSize
		{
			get { return (double)GetValue(PhotoTitleFontSizeProperty); }
			set { if (value > 0) { SetValue(PhotoTitleFontSizeProperty, value); } }
		}

		public static readonly DependencyProperty PhotoTitleVisibilityProperty =
			DependencyProperty.Register(
				"PhotoTitleVisibility",
				typeof(Visibility),
				typeof(AlbumViewModel),
				null
			);
		public Visibility PhotoTitleVisibility
		{
			get { return (Visibility)GetValue(PhotoTitleVisibilityProperty); }
			set { SetValue(PhotoTitleVisibilityProperty, value); }
		}

		#endregion : Dependency Properties :

		#region : Static Event Handlers :

		private static void OnPhotoWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var avm = d as AlbumViewModel;
			if (avm.PhotoWidth >= 275)
			{
				avm.PhotoTitleFontSize = 13;
			}
			else if (avm.PhotoWidth >= 125 && avm.PhotoWidth < 275)
			{
				avm.PhotoTitleFontSize = 12;
			}
			else if (avm.PhotoWidth >= 85 && avm.PhotoWidth < 125)
			{
				avm.PhotoTitleFontSize = 11;
			}
			else
			{
				avm.PhotoTitleFontSize = 0;
			}
		}

		private static void OnPhotoTitleFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var avm = d as AlbumViewModel;
			if (avm.PhotoTitleFontSize == 0)
			{
				avm.PhotoTitleVisibility = Visibility.Collapsed;
			}
			else
			{
				avm.PhotoTitleVisibility = Visibility.Visible;
			}
		}

		#endregion : Static Event Handlers :

		#region : INotifyPropertyChanged Members :

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != PropertyChanged)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion : INotifyPropertyChanged Members :

		#region : Private Methods :

		private void LoadViewModel(Album album)
		{
			MobileMeGallery gallery = GalleryContext.Gallery;

			Title = gallery.Title;

			album.Photos.ForEach(p => _photos.Add(p));

			OnLoaded();
		}

		#endregion : Private Methods :

	} //end public class AlbumViewModel : ViewModelBase

} //end namespace Gallery.ViewModels
