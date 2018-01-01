using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Gallery.Model;

namespace Gallery.ViewModels
{
	public interface IHomeViewModel : IViewModelBase
	{
		string Title { get; }
		string PageTitle { get; }
		string AlbumText { get; set; }
		string MovieText { get; set; }
		ObservableCollection<Album> Albums { get; }
		ObservableCollection<Movie> Movies { get; }

		/// <summary>
		/// Retrieves original image clipping and renderTransform settings
		/// </summary>
		/// <param name="img"></param>
		/// <param name="clip"></param>
		/// <param name="renderTransform"></param>
		void GetOriginalImage(Image img, out RectangleGeometry clip, out TranslateTransform renderTransform);

		/// <summary>
		/// Retrieves scrubbing image clipping and renderTransform settings based on passed Point (mouse coordinates)
		/// </summary>
		/// <param name="img"></param>
		/// <param name="point"></param>
		/// <param name="clip"></param>
		/// <param name="renderTransform"></param>
		void GetScrubbingImage(Image img, Point point, out RectangleGeometry clip, out TranslateTransform renderTransform);
	}
}