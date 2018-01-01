using System.ComponentModel.Composition;
using Gallery.Contracts;

namespace Gallery.Settings
{
	[Export(typeof(IGalleryContextSettings))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class GalleryContextSettings : IGalleryContextSettings
	{

		private string _username = Config.DefaultUsername;
		private string _serviceUri = Config.ServiceUri;
		private string _galleryUriTemplate = string.Concat(Config.ServiceUri, Config.GalleryUriTemplate);
		private string _albumUriTemplate = string.Concat(Config.ServiceUri, Config.AlbumUriTemplate);

		public GalleryContextSettings()
		{
			_username = ConfigSettings.Current[Config.UsernameKey];
			_serviceUri = ConfigSettings.Current[Config.ServiceUriKey];
			_galleryUriTemplate = string.Concat(_serviceUri, Config.GalleryUriTemplate);
			_albumUriTemplate = string.Concat(_serviceUri, Config.AlbumUriTemplate);
		}

		#region IGalleryContextSettings Members

		public string Username
		{
			get { return _username; }
			set { _username = value; }
		}

		public string ServiceUri
		{
			get
			{

				return _serviceUri;
			}
			set { _serviceUri = value ; }
		}

		public string GalleryUriTemplate
		{
			get { return _galleryUriTemplate ; }
			set { _galleryUriTemplate = value; }
		}

		public string AlbumUriTemplate
		{
			get { return _albumUriTemplate; }
			set { _albumUriTemplate = value; }
		}

		#endregion
	}
}
