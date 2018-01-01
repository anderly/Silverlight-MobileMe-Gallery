using System;
using System.Windows.Input;
using Gallery.Core;
using Gallery.Model;

namespace Gallery.ViewModels
{
	public class ViewerViewModel : ViewModel
	{
		#region : Private Fields :

		private string _pageTitle;
		private string _galleryTitle;
		private string _title;
		private string _subTitle;

		#endregion : Private Fields :

		#region : Properties :

		public string PageTitle
		{
			get { return _pageTitle; }
			set
			{
				_pageTitle = value;
				this.NotifyPropertyChanged(() => this.PageTitle);
			}
		}

		public string GalleryTitle
		{
			get { return _galleryTitle; }
			set
			{
				_galleryTitle = value;
				this.NotifyPropertyChanged(() => this.GalleryTitle);
			}
		}

		public string Title
		{
			get { return _title; }
			set
			{
				_title = value;
				this.NotifyPropertyChanged(() => this.Title);
			}
		}

		public string SubTitle
		{
			get { return _subTitle; }
			set
			{
				_subTitle = value;
				this.NotifyPropertyChanged(() => this.SubTitle);
			}
		}

		public bool OptionsButtonVisible { get; set; }

		#endregion : Properties :

		#region : Commands :

		public ICommand HomeCommand { get; set; }

		#endregion : Commands :

		#region : Constructor :

		public ViewerViewModel()
		{
			Messenger.Register<MobileMeGallery>(this, Messages.GalleryLoaded, GalleryLoaded);
			if (GalleryContext.IsLoaded)
			{
				GalleryLoaded(GalleryContext.Gallery);
			}

			OptionsButtonVisible = false;
			HomeCommand = new DelegateCommand<string>(NavigateHome);
			Messenger.Register<GalleryItem>(this, Messages.GalleryItemLoaded, GalleryItemLoaded);
		}

		#endregion : Constructor :

		#region : Private Methods :

		private void GalleryLoaded(MobileMeGallery gallery)
		{
			GalleryTitle = gallery.Title;
		}

		private void GalleryItemLoaded(GalleryItem galleryItem)
		{
			PageTitle = string.Format(Config.MobileMePageTitleFormat, galleryItem.Title);
			Title = galleryItem.Title;
			if (galleryItem is Album)
			{
				SubTitle = string.Format("({0})", (galleryItem as Album).Count);
			}
		}

		private void NavigateHome(string uri)
		{
			Messenger.Send<Uri>(new Uri(uri, UriKind.Relative), Messages.Navigate);
		}

		#endregion : Private Methods :
	}
}
