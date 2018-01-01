using System;
using System.ComponentModel;

namespace Gallery.Contracts
{
	public interface IDeploymentCatalogService
	{
		void AddXap(string uri, Action<AsyncCompletedEventArgs> completedAction = null);
		void RemoveXap(string uri);
	}
}
