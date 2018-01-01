using System;
using System.Windows;

namespace Gallery.Extensions
{
	/// <summary>
	/// Attached Property (Title) for Page object that allows binding of Page Title
	/// </summary>
	public static class Page
	{
		#region public attached string Title
		public static string GetTitle(System.Windows.Controls.Page element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return element.GetValue(TitleProperty) as string;
		}

		public static void SetTitle(System.Windows.Controls.Page element, string value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TitleProperty, value);
		}

		public static readonly DependencyProperty TitleProperty =
				DependencyProperty.RegisterAttached(
						"Title",
						typeof(string),
						typeof(Page),
						new PropertyMetadata(null, OnTitlePropertyChanged));

		private static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var source = d as System.Windows.Controls.Page;
			source.Title = e.NewValue as string;
		}
		#endregion public attached string Title

	}
}
