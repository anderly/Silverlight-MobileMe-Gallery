using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.Routing;

namespace Gallery.Web
{
	public class CustomRouteHandler : IRouteHandler
	{
		public CustomRouteHandler(string virtualPath)
		{
			VirtualPath = virtualPath;
		}

		public string VirtualPath { get; private set; }

		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			foreach (var aux in requestContext.RouteData.Values)
			{
				HttpContext.Current.Items[aux.Key] = aux.Value;
			}

			return BuildManager.CreateInstanceFromVirtualPath(
						VirtualPath, typeof(Page)) as IHttpHandler;
		}
	}
}
