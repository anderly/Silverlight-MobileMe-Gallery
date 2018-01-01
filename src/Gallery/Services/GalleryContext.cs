using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using Gallery.Contracts;
using Gallery.Model;
using Gallery.Model.MobileMe;

namespace Gallery.Services
{
	[Export(typeof(IGalleryContext))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class GalleryContext : DependencyObject, IGalleryContext, INotifyPropertyChanged
	{
		private readonly Uri _applicationUriSource = Application.Current.Host.Source;

		#region : Public Properties :

		public IGalleryContextSettings Settings { get; set; }

		public IMessenger Messenger { get; set; }

		public MobileMeGallery Gallery { get; set; }

		public GalleryItem SelectedItem { get; set; }

		public bool IsLoaded { get; set; }

		public bool IsSelectedItemLoaded
		{
			get
			{
				if (SelectedItem == null) return false;
				return (SelectedItem as Album).IsLoaded;
			}
		}

		#endregion : Public Properties :

		#region : Constructor :

		[ImportingConstructor]
		public GalleryContext(IGalleryContextSettings settings, IMessenger messenger)
		{
			IsLoaded = false;
			Settings = settings;
			Messenger = messenger;
			Messenger.Register<string>(this, Messages.SelectAlbumOrMovie, GetGalleryItem);
			Initialize();
		}

		#endregion : Constructor :

		#region : Public Methods and Events :

		public event EventHandler Loaded = delegate { };
		public event EventHandler AlbumLoaded = delegate { };

		#endregion : Public Methods and Events :

		#region : Private Methods :

		protected void OnLoaded()
		{
			var handler = Loaded;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		protected void OnAlbumLoaded()
		{
			var handler = AlbumLoaded;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		private void Initialize()
		{
			var webClient = new WebClient();
			webClient.OpenReadCompleted += GetGalleryCompleted;
			var galleryUri = new Uri(_applicationUriSource, string.Format(Settings.GalleryUriTemplate, Settings.Username));
			if (!Settings.AlbumUriTemplate.StartsWith("../"))
			{
				galleryUri = new Uri(string.Format(Settings.GalleryUriTemplate, Settings.Username));
			}
			webClient.OpenReadAsync(galleryUri);
			IsBusy = true;
		}

		private void GetGalleryItem(string id)
		{
			SelectedItem = Gallery.Items.Where(i => i.ID == id).FirstOrDefault();
			if (SelectedItem is Movie)
			{
				Messenger.Send<GalleryItem>(SelectedItem as GalleryItem, Messages.GalleryItemLoaded);
			}
			else if (IsLoaded)
			{
				GoGetAlbum(id);
			}
			else
			{
				Loaded += (a, b) => GoGetAlbum(id);
			}
		}

		private void GetAlbum(string id)
		{
			if (IsLoaded)
			{
				GoGetAlbum(id);
			}
			else
			{
				Loaded += (a, b) => GoGetAlbum(id);
			}
		}

		private void GoGetAlbum(string id)
		{
			var webClient = new WebClient();
			webClient.OpenReadCompleted += GetAlbumCompleted;

			var albumUri = new Uri(_applicationUriSource, string.Format(Settings.AlbumUriTemplate, Settings.Username, id));
			if (!Settings.AlbumUriTemplate.StartsWith("../"))
			{
				albumUri = new Uri(string.Format(Settings.AlbumUriTemplate, Settings.Username, id));
			}
			webClient.OpenReadAsync(albumUri);
			IsBusy = true;
		}

		private void GetGalleryCompleted(object sender, OpenReadCompletedEventArgs e)
		{
			if (e.Error == null)
			{
				var ser = new DataContractJsonSerializer(typeof(gallery));
				var gal = (gallery)ser.ReadObject(e.Result);
				this.Gallery = gal.ToMobileMeGallery();
				IsBusy = false;
				IsLoaded = true;
				OnLoaded();
				Messenger.Send<MobileMeGallery>(this.Gallery, "GalleryLoaded");
			}
		}

		private void GetAlbumCompleted(object sender, OpenReadCompletedEventArgs e)
		{
			if (e.Error == null)
			{
				var ser = new DataContractJsonSerializer(typeof(album));
				var album = (album)ser.ReadObject(e.Result);
				var selectedAlbum = (SelectedItem as Album);
				var sortedPhotos = (from pic in album.ToAlbum().Photos orderby pic.SortOrder select pic).ToList();
				selectedAlbum.Photos.AddRange(sortedPhotos);
				IsBusy = false;
				selectedAlbum.IsLoaded = true;
				OnAlbumLoaded();
				Messenger.Send<GalleryItem>(selectedAlbum as GalleryItem, Messages.GalleryItemLoaded);
				Messenger.Send<Album>(selectedAlbum, Messages.AlbumLoaded);
			}
		}

		#endregion : Private Methods :

		#region : Dependency Properties :

		public static readonly DependencyProperty IsBusyProperty =
			DependencyProperty.Register(
			"IsBusy", typeof(bool),
			typeof(GalleryContext), null);

		public bool IsBusy
		{
			get { return (bool)GetValue(IsBusyProperty); }
			set { SetValue(IsBusyProperty, value); }
		}

		public static readonly DependencyProperty PhotoWidthProperty =
			DependencyProperty.Register(
				"PhotoWidth",
				typeof(double),
				typeof(GalleryContext),
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
				typeof(GalleryContext),
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
				typeof(GalleryContext),
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
			var globals = d as GalleryContext;
			if (globals.PhotoWidth >= 275)
			{
				globals.PhotoTitleFontSize = 13;
			}
			else if (globals.PhotoWidth >= 125 && globals.PhotoWidth < 275)
			{
				globals.PhotoTitleFontSize = 12;
			}
			else if (globals.PhotoWidth >= 85 && globals.PhotoWidth < 125)
			{
				globals.PhotoTitleFontSize = 11;
			}
			else
			{
				globals.PhotoTitleFontSize = 0;
			}
		}

		private static void OnPhotoTitleFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var globals = d as GalleryContext;
			if (globals.PhotoTitleFontSize == 0)
			{
				globals.PhotoTitleVisibility = Visibility.Collapsed;
			}
			else
			{
				globals.PhotoTitleVisibility = Visibility.Visible;
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

	}
}
