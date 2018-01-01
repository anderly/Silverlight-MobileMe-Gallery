using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Gallery.Contracts;

namespace Gallery
{
	public partial class App : Application
	{
		#region : Public Properties :

		[Import]
		public IGalleryContext GalleryContext { get; set; }

		public Dictionary<string, string> InitParams { get; private set; }

		#endregion : Public Properties :

		#region : Constructor :

		public App()
		{
			InitParams = new Dictionary<string, string>();
			this.Startup += this.Application_Startup;
			this.UnhandledException += this.Application_UnhandledException;

			InitializeComponent();
		}

		#endregion : Constructor :

		#region : Event Handlers :

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			// load the username from the passed initParams
			foreach (var item in e.InitParams)
			{
				InitParams.Add(item.Key, item.Value);
			}
			// Load config settings from web service call
			ConfigSettings.Current.Load(InitParams);
			ConfigSettings.Current.Loaded += delegate
										{
											DeploymentCatalogService.Initialize();
											Bootstrapper.Current.Initialize();
											CompositionInitializer.SatisfyImports(this);
											this.Resources.Add("Globals", GalleryContext);
											//ServiceLocator.Initialize();
											this.RootVisual = new MainPage();
										};
		}

		private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			// If the app is running outside of the debugger then report the exception using
			// a ChildWindow control.
			if (!System.Diagnostics.Debugger.IsAttached)
			{
				// NOTE: This will allow the application to continue running after an exception has been thrown
				// but not handled. 
				// For production applications this error handling should be replaced with something that will 
				// report the error to the website and stop the application.
				e.Handled = true;
				ChildWindow errorWin = new ErrorWindow(e.ExceptionObject);
				errorWin.Show();
			}
		}

		#endregion : Event Handlers :

	} //end public partial class App : Application

} //end namespace Gallery