using System.ComponentModel.Composition;
using Gallery.Contracts;

namespace Gallery
{
	public class Bootstrapper
	{
		private static Bootstrapper _instance;

		private Bootstrapper()
		{
		}

		public static Bootstrapper Current
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Bootstrapper();
				}
				return _instance;
			}
		}

		[Import]
		public IDeploymentCatalogService CatalogService { get; set; }

		public void Initialize()
		{
			CompositionInitializer.SatisfyImports(this);
		}
	}
}
