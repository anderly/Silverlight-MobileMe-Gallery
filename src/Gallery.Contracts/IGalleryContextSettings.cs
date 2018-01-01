namespace Gallery.Contracts
{
	public interface IGalleryContextSettings
	{
		string Username { get; set; }
		string ServiceUri { get; set; }
		string GalleryUriTemplate { get; set; }
		string AlbumUriTemplate { get; set; }
	}
}
