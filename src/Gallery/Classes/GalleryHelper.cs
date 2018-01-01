using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows.Browser;
using Gallery.Model;
using Gallery.Model.MobileMe;

namespace Gallery.Classes
{
	public static class GalleryHelper
	{
		private static string _username = "emily_parker";

		public static string GetUsername()
		{
			try
			{
				string url = HtmlPage.Document.DocumentUri.ToString();
				int poundTest = url.LastIndexOf('#');
				if (poundTest > 0)
				{
					string tmpUser = url.Substring(poundTest + 2);
					if (!string.IsNullOrEmpty(tmpUser))
					{
						_username = tmpUser;
					}
				}
			}
			catch (Exception ex)
			{
				 //hide exception
			}
			return _username;
		}

		public static string GetID()
		{
			string url = HtmlPage.Document.DocumentUri.ToString();
			string id = url.Substring(url.LastIndexOf('#')+1);
			return id;
		}

		public static MobileMeGallery GetGallery(Stream data)
		{
			var ser = new DataContractJsonSerializer(typeof(gallery));
			gallery gallery = (gallery)ser.ReadObject(data);

			return gallery.ToMobileMeGallery();
		}

		public static List<Photo> GetAlbumPhotos(Stream data)
		{
			var ser = new DataContractJsonSerializer(typeof(album));
			album album = (album)ser.ReadObject(data);

			return album.ToAlbum().Photos;
		}
	}
}
