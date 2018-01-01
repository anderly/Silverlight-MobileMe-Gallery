using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Gallery.Contracts;
using Gallery.Model;

namespace Gallery.Views
{
	public partial class Viewer : Page
	{
		#region : Public Properties :

		[Import]
		public IGalleryContext GalleryContext { get; set; }

		[Import]
		public IMessenger Messenger { get; set; }

		#endregion : Public Properties :

		#region : Constructor :

		public Viewer()
		{
			CompositionInitializer.SatisfyImports(this);
			InitializeComponent();
			Messenger.Register<MobileMeGallery>(this, Messages.GalleryLoaded, GalleryLoaded);
			Messenger.Register<GalleryItem>(this, Messages.GalleryItemLoaded, GalleryItemLoaded);
		}

		#endregion : Constructor :

		#region : Private Methods :

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (GalleryContext.IsLoaded)
			{
				GalleryLoaded(GalleryContext.Gallery);
			}
		}

		private void ViewerLoaded()
		{
			//VisualStateManager.GoToState(GridViewButton, "Pressed", false);
		}

		private void GalleryLoaded(MobileMeGallery gallery)
		{
			string id = string.Empty;
			if (this.NavigationContext.QueryString.ContainsKey("id"))
			{
				id = this.NavigationContext.QueryString["id"];
				Messenger.Send<string>(id, Messages.SelectAlbumOrMovie);
			}
		}

		private void GalleryItemLoaded(GalleryItem selectedItem)
		{
			//var selectedItem = GalleryContext.SelectedItem;
			DownloadButton.IsEnabled = selectedItem.Download;
			if (selectedItem is Album)
			{
				if (this.NavigationContext.QueryString.ContainsKey("bgcolor"))
				{
					string color = this.NavigationContext.QueryString["bgcolor"];
					switch (color)
					{
						case "black":
							//ContentViewer.Background.SetValue(ScrollViewer.BackgroundProperty, new SolidColorBrush(Color.FromArgb(0xFF, 0x0, 0x0, 0x0)));
							ColorBlack.IsChecked = true;
							break;
						case "dkgrey":
							//ContentViewer.Background.SetValue(ScrollViewer.BackgroundProperty, new SolidColorBrush(Color.FromArgb(0xFF, 0x44, 0x44, 0x44)));
							ColorDarkGray.IsChecked = true;
							break;
						case "ltgrey":
							//ContentViewer.Background.SetValue(ScrollViewer.BackgroundProperty, new SolidColorBrush(Color.FromArgb(0xFF, 0x77, 0x77, 0x77)));
							ColorLightGray.IsChecked = true;
							break;
						case "white":
							//ContentViewer.Background.SetValue(ScrollViewer.BackgroundProperty, new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF)));
							ColorWhite.IsChecked = true;
							break;
						default:
							ColorBlack.IsChecked = true;
							break;
					}
				}
				var album = selectedItem as Album;
				ContentViewer.Content = new AlbumView();
				UploadButton.IsEnabled = album.AddPhoto;
				SendToAlbumButton.IsEnabled = album.AllowMobile;
			}
			else
			{
				ContentViewer.Content = new MovieView(LayoutRoot);
				SubscribeButton.Visibility = Visibility.Collapsed;
				UploadButton.Visibility = Visibility.Collapsed;
				SendToAlbumButton.Visibility = Visibility.Collapsed;
				Row_4.Height = new GridLength(0);
			}
		}

		private void OptionsButton_Click(object sender, RoutedEventArgs e)
		{
			if (OptionsBar.Height != 0)
			{
				HideOptionsStoryboard.Begin();
				OptionsButton.Content = Config.OptionsButtonTextShow;
			}
			else
			{
				ShowOptionsStoryboard.Begin();
				OptionsButton.Content = Config.OptionsButtonTextHide;
			}
		}

		private void ImageSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			GalleryContext.PhotoWidth = e.NewValue;
		}

		#endregion : Private Methods :

	} //end public partial class Viewer : Page

} //end namespace Gallery.Views
