using System;
using System.Windows;
using Gallery.Model;

namespace Gallery.Contracts
{
	public interface IGalleryContext
	{
		#region : Public Properties :

		IGalleryContextSettings Settings { get; set; }
		MobileMeGallery Gallery { get; set; }
		GalleryItem SelectedItem { get; set; }
		bool IsLoaded { get; set; }
		bool IsSelectedItemLoaded { get; }
		bool IsBusy { get; set; }
		double PhotoWidth { get; set; }
		double PhotoTitleFontSize { get; set; }
		Visibility PhotoTitleVisibility { get; set; }

		#endregion : Public Properties :

		#region : Public Methods and Events :

		event EventHandler Loaded;
		event EventHandler AlbumLoaded;

		#endregion : Public Methods and Events :
	}
}
