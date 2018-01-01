using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Gallery.Contracts;

namespace Gallery
{
	[Export(typeof(IDeploymentCatalogService))]
	public class DeploymentCatalogService : IDeploymentCatalogService
	{
		private static AggregateCatalog _aggregateCatalog;
		private static CompositionContainer _container;

		Dictionary<string, DeploymentCatalog> _catalogs;

		public DeploymentCatalogService()
		{
			_catalogs = new Dictionary<string, DeploymentCatalog>();
		}

		public static void Initialize()
		{
			_aggregateCatalog = new AggregateCatalog();
			_aggregateCatalog.Catalogs.Add(new DeploymentCatalog());
			_container = CompositionHost.Initialize(_aggregateCatalog);
		}

		public void AddXap(string uri, Action<AsyncCompletedEventArgs> completedAction = null)
		{
			DeploymentCatalog catalog;
			if (!_catalogs.TryGetValue(uri, out catalog))
			{
				catalog = new DeploymentCatalog(uri);
				if (completedAction != null)
					catalog.DownloadCompleted += (s, e) => completedAction(e);
				else
					catalog.DownloadCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(catalog_DownloadCompleted);

				catalog.DownloadAsync();
				_catalogs[uri] = catalog;
			}
			_aggregateCatalog.Catalogs.Add(catalog);
		}

		void catalog_DownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			if (e.Error != null)
				throw e.Error;
		}

		public void RemoveXap(string uri)
		{
			DeploymentCatalog catalog;
			if (_catalogs.TryGetValue(uri, out catalog))
			{
				_aggregateCatalog.Catalogs.Remove(catalog);
			}
		}

		public static T GetInstance<T>()
		{

			return _container.GetExportedValue<T>();

		}
	}

}
