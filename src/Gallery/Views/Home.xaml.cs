using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Gallery.ViewModels;

namespace Gallery
{
	public partial class Home : Page
	{
		IHomeViewModel _viewModel = null;

		public Home()
		{
			InitializeComponent();
			Loaded += Page_Loaded;
		}

		void Page_Loaded(object sender, RoutedEventArgs e)
		{
			_viewModel = DataContext as IHomeViewModel;
		}

		#region : ScrubSprite Event Handlers :

		void Album_MouseMove(object sender, MouseEventArgs e)
		{
			Image img = sender as Image;
			Point point = e.GetPosition(img);

			RectangleGeometry rg;
			TranslateTransform tt;

			_viewModel.GetScrubbingImage(img, point, out rg, out tt);
			img.Clip = rg;
			img.RenderTransform = tt;
		}

		void Album_MouseLeave(object sender, MouseEventArgs e)
		{
			Image img = sender as Image;

			RectangleGeometry rg;
			TranslateTransform tt;

			_viewModel.GetOriginalImage(img, out rg, out tt);
			img.Clip = rg;
			img.RenderTransform = tt;
		}

		void Movie_MouseMove(object sender, MouseEventArgs e)
		{
			Image img = sender as Image;
			Point point = e.GetPosition(img);

			RectangleGeometry rg;
			TranslateTransform tt;

			_viewModel.GetScrubbingImage(img, point, out rg, out tt);
			img.Clip = rg;
			img.RenderTransform = tt;
		}

		void Movie_MouseLeave(object sender, MouseEventArgs e)
		{
			Image img = sender as Image;

			RectangleGeometry rg;
			TranslateTransform tt;

			_viewModel.GetOriginalImage(img, out rg, out tt);
			img.Clip = rg;
			img.RenderTransform = tt;
		}

		#endregion : ScrubSprite Event Handlers :

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}
	}
}