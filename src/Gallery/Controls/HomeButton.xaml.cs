using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Gallery.Contracts;

namespace Gallery
{
	public partial class HomeButton : UserControl
	{
		[Import]
		IMessenger Messenger { get; set; }

		public HomeButton()
		{
			// Required to initialize variables
			InitializeComponent();
			CompositionInitializer.SatisfyImports(this);
		}

		private void backButton_Click(object sender, RoutedEventArgs e)
		{
			Messenger.Send<Uri>(new Uri("gallery", UriKind.Relative), Messages.Navigate);
		}
	}
}