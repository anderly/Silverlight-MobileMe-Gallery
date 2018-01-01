using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Gallery.Contracts;
using Gallery.Model;

namespace Gallery
{
	public partial class MovieView : UserControl
	{
		[Import]
		public IGalleryContext GalleryContext { get; set; }

		GridLength _defaultHeight;
		GridLength _defaultWidth;

		Grid ParentLayout;

		public MovieView(Grid parentLayout)
		{
			InitializeComponent();
			CompositionInitializer.SatisfyImports(this);

			ParentLayout = parentLayout;
			_defaultHeight = ParentLayout.RowDefinitions[2].Height;
			_defaultWidth = ParentLayout.ColumnDefinitions[0].Width;

			this.Loaded += Page_Loaded;
			App.Current.Host.Content.FullScreenChanged += Content_FullScreenChanged;
		}

		void Page_Loaded(object sender, RoutedEventArgs e)
		{
			var movie = GalleryContext.SelectedItem as Movie;
			this.DataContext = movie;
			VideoPlayer.Width = movie.VideoWidthMedium;
			VideoPlayer.Height = movie.VideoHeightMedium;
		}

		void Content_FullScreenChanged(object sender, EventArgs e)
		{
			if (App.Current.Host.Content.IsFullScreen == true)
			{
				double targetWidth = App.Current.Host.Content.ActualWidth;
				double targetHeight = App.Current.Host.Content.ActualHeight;

				var sv = this.Parent as ScrollViewer;
				sv.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

				ParentLayout.RowDefinitions[0].Height = new GridLength(0);
				ParentLayout.RowDefinitions[1].Height = new GridLength(0);
				ParentLayout.RowDefinitions[2].Height = new GridLength(targetHeight);

				ParentLayout.ColumnDefinitions[0].Width = new GridLength(targetWidth);
				ParentLayout.Margin = new Thickness(0);
			}

			else
			{
				ScrollViewer sv = this.Parent as ScrollViewer;
				sv.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

				ParentLayout.RowDefinitions[0].Height = new GridLength(60);
				ParentLayout.RowDefinitions[1].Height = new GridLength(50);
				ParentLayout.RowDefinitions[2].Height = _defaultHeight;

				ParentLayout.ColumnDefinitions[0].Width = _defaultWidth;
			}
		}

	}
}
