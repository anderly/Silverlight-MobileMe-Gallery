using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace Gallery.Web.Services
{
	[ServiceContract(Namespace = "urn:silverlightmobileme.codeplex.com")]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
	public class GalleryService
	{
		string galleryUrlFormat = ConfigurationManager.AppSettings["GalleryUrlFormat"];
		string albumUrlFormat = ConfigurationManager.AppSettings["AlbumUrlFormat"];

		[WebGet(UriTemplate = "/{username}",
				ResponseFormat = WebMessageFormat.Json,
				BodyStyle = WebMessageBodyStyle.Bare)]
		[OperationContract]
		public Stream GetGallery(string username)
		{
			try
			{
				var webClient = new WebClient();
				string photocastUrl = string.Format(galleryUrlFormat, username);
				return webClient.OpenRead(photocastUrl);
			}
			catch (Exception ex)
			{
				var e = ex;
				throw new WebFaultException(HttpStatusCode.Unauthorized);
			}
		}

		[WebGet(UriTemplate = "/{username}/{id}",
				ResponseFormat = WebMessageFormat.Json,
				BodyStyle = WebMessageBodyStyle.Bare)]
		[OperationContract]
		public Stream GetAlbum(string username, string id)
		{
			var webClient = new WebClient();
			string albumUrl = string.Format(albumUrlFormat, username, id);
			return webClient.OpenRead(albumUrl);
		}

	}
}