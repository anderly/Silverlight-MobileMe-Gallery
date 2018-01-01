using System;
using System.IO;
using System.Windows;
using Gallery.Contracts;
using Gallery.Model;
using GalleryHelper = Gallery.Classes.GalleryHelper;

namespace Gallery.Services.DesignTime
{
	//[ExportService(ServiceType.DesignTime, typeof(IGalleryContext))]
	//[Export(typeof(IGalleryContext))]
	//[PartCreationPolicy(CreationPolicy.Shared)]
	public class MockGalleryContext : IGalleryContext
	{
		private const string MockGallery = "Gallery.Services.DesignTime.emily_parker.js";
		private const string MockAlbum = "Gallery.Services.DesignTime.100609.js";

		//[ImportingConstructor]
		//public MockGalleryContext([Import]IGalleryContextSettings galleryContextSettings)
		public MockGalleryContext()
		{
			//Settings = galleryContextSettings;
			var s = this.GetType().Assembly.GetManifestResourceStream(MockGallery);
			var sr = new StreamReader(s);
			var script = sr.ReadToEnd();
			Gallery = GalleryHelper.GetGallery(sr.BaseStream);
			IsBusy = false;
			IsLoaded = true;
			OnLoaded();
		}

		#region IGalleryContext Members

		public IGalleryContextSettings Settings { get; set; }

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

		public bool IsBusy { get; set; }

		public double PhotoWidth { get; set; }

		public double PhotoTitleFontSize { get; set; }

		public Visibility PhotoTitleVisibility { get; set; }

		public event EventHandler Loaded = delegate { };
		public event EventHandler AlbumLoaded = delegate { };

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

		public void GetAlbum(string id)
		{
			var s = this.GetType().Assembly.GetManifestResourceStream(MockAlbum);
			var sr = new StreamReader(s);
			var script = sr.ReadToEnd();
			(SelectedItem as Album).Photos = GalleryHelper.GetAlbumPhotos(sr.BaseStream);
			OnAlbumLoaded();
		}

		#endregion
	}
}
