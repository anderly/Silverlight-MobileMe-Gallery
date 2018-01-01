using System.Web;
using System.ServiceModel.Activation;
using System.Web.Routing;
using Gallery.Web.Services;

namespace Gallery.Web
{
	public class Global : HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			//routes.Add("GalleryService", new Route(
			//    "GalleryService",
			//    new CustomRouteHandler("~/Services/GalleryService.svc")
			//));
			routes.Add("Default", new Route(
				"{username}",
				new CustomRouteHandler("~/Default.aspx")
			));
			RouteTable.Routes.Add(new ServiceRoute("GalleryService", new WebServiceHostFactory(), typeof(GalleryService)));
			RouteTable.Routes.Add(new ServiceRoute("ConfigService", new WebServiceHostFactory(), typeof(ConfigService)));
		}

		protected void Application_Start()
		{
			RegisterRoutes(RouteTable.Routes);
		}
	}
}