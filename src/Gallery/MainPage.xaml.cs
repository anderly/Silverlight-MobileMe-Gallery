using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Navigation;
using Gallery.Contracts;

namespace Gallery
{
	public partial class MainPage : UserControl
	{
		[Import]
		public IMessenger Messenger { get; set; }

		public MainPage()
		{
			InitializeComponent();
			CompositionInitializer.SatisfyImports(this);
			Messenger.Register<Uri>(this, Messages.Navigate, NavigateToUri);
		}

		private void NavigateToUri(Uri uri)
		{
			ContentFrame.Navigate(uri);
		}

		// If an error occurs during navigation, show an error window
		private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			e.Handled = true;
			ChildWindow errorWin = new ErrorWindow(e.Uri);
			errorWin.Show();
		}
	}
}