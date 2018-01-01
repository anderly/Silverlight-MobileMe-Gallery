using System;
using System.Collections.Generic;
using System.Linq;
using Gallery.Model.MobileMe;

namespace Gallery.Model
{
	public static class GalleryExtensions
	{
		public static MobileMeGallery ToMobileMeGallery(this gallery gallery)
		{
			var mobileMeGallery = new MobileMeGallery()
			{
				//set gallery settings
				Title = gallery.data.title,
				Updated = gallery.data.updated,
				ApertureUserOrder = gallery.data.apertureUserOrder,
				iMovieUserOrder = gallery.data.iMovieUserOrder,
				UserOrder = gallery.data.userOrder,
				Items = new List<GalleryItem>()
			};

			mobileMeGallery.LoadAlbums(gallery);
			mobileMeGallery.LoadMovies(gallery);

			return mobileMeGallery;
		} //end public static MobileMeGallery ToMobileMeGallery(this gallery gallery)

		private static void LoadAlbums(this MobileMeGallery mobileMeGallery, gallery gallery)
		{
			List<record> albums = (from rec in gallery.records
								   where rec.type == "Album"
								   select rec).ToList();

			//albums.ForEach(o => mobileMeGallery.Items.Add(o.ToGalleryItem().ToAlbum(o)));
			albums.ForEach(o => mobileMeGallery.Items.Add(o.ToAlbum()));

			var items = mobileMeGallery.Albums.OrderByDescending(i => i.Updated).ToList();

			if (items.FirstOrDefault().SortOrder == 0)
			{
				var i = 1;
				items.ForEach(a =>
				{
					a.SortOrder = i;
					i++;
				});
			}

		} //end private static void LoadAlbums(this MobileMeGallery mobileMeGallery, gallery gallery)

		private static void LoadMovies(this MobileMeGallery mobileMeGallery, gallery gallery)
		{
			List<record> movies = (from rec in gallery.records
								   where rec.type == "Movie"
								   select rec).ToList();

			//movies.ForEach(o => mobileMeGallery.Items.Add(o.ToGalleryItem().ToMovie(o)));
			movies.ForEach(o => mobileMeGallery.Items.Add(o.ToMovie()));

			var items = mobileMeGallery.Movies.OrderByDescending(i => i.Updated).ToList();

			if (items.FirstOrDefault().SortOrder == 0)
			{
				var i = 1;
				items.ForEach(a =>
				{
					a.SortOrder = i;
					i++;
				});
			}

		} //end private static void LoadMovies(this MobileMeGallery mobileMeGallery, gallery gallery)

		private static GalleryItem ToGalleryItem(this record record)
		{
			var galleryItem = new GalleryItem
			{
				Guid = record.guid,
				UserItemGuid = record.userItemGuid,
				Title = record.title,
				Type = (GalleryItemType)Enum.Parse(typeof(GalleryItemType), record.type, true),
				Url = record.url,
				Updated = record.updated,
				SortOrder = record.sortOrder,
				ShowCaptions = record.showCaptions,
				AccessLogin = record.accessLogin,
				//ScrubSpritePath = string.Format("{0}/{1}", Url, record.scrubSpritePath),
				ScrubSpriteFrameCount = record.scrubSpriteFrameCount,
				ScrubSpriteKeyFrameIndex = record.scrubSpriteKeyFrameIndex,
				ScrubSpriteFrameHeight = record.scrubSpriteFrameHeight,
				ScrubSpriteFrameWidth = record.scrubSpriteFrameWidth,
				ViewIdentifier = record.viewIdentifier,
				VersionInfo = record.versionInfo,
				UserHidden = record.userHidden,
				Download = record.download
			};
			galleryItem.ScrubSpritePath = string.Format("{0}/{1}", galleryItem.Url, record.scrubSpritePath);

			return galleryItem;
		} //end private static GalleryItem ToGalleryItem(this record record)

		private static void SetCommon(this GalleryItem galleryItem, record record)
		{
			galleryItem.Guid = record.guid;
			galleryItem.UserItemGuid = record.userItemGuid;
			galleryItem.Title = record.title;
			galleryItem.Type = (GalleryItemType)Enum.Parse(typeof(GalleryItemType), record.type, true);
			galleryItem.Url = record.url;
			galleryItem.Updated = record.updated;
			galleryItem.SortOrder = record.sortOrder;
			galleryItem.ShowCaptions = record.showCaptions;
			galleryItem.AccessLogin = record.accessLogin;
			galleryItem.ScrubSpritePath = string.Format("{0}/{1}", galleryItem.Url, record.scrubSpritePath);
			galleryItem.ScrubSpriteFrameCount = record.scrubSpriteFrameCount;
			galleryItem.ScrubSpriteKeyFrameIndex = record.scrubSpriteKeyFrameIndex;
			galleryItem.ScrubSpriteFrameHeight = record.scrubSpriteFrameHeight;
			galleryItem.ScrubSpriteFrameWidth = record.scrubSpriteFrameWidth;
			galleryItem.ViewIdentifier = record.viewIdentifier;
			galleryItem.VersionInfo = record.versionInfo;
			galleryItem.UserHidden = record.userHidden;
			galleryItem.Download = record.download;
		}

		private static Album ToAlbum(this record record)
		{
			var album = new Album
			{
				Path = record.path,
				UserOrder = record.userOrder,
				UserOrderIndex = record.userOrderIndex,
				KeyImageGuid = record.keyImageGuid,
				KeyImagePath = record.keyImagePath,
				KeyImageFileExtension = record.keyImageFileExtension,
				PhotoCount = record.numPhotos,
				MovieCount = record.numMovies,
				AlbumWidget = record.albumWidget,
				AddPhoto = record.addPhoto,
				AllowMobile = record.allowMobile,
				ShowMobile = record.showMobile,
				MobileEmail = record.mobileEmail,
				Photos = new List<Photo>()
			};
			album.SetCommon(record);

			return album;
		} //end private static Album ToAlbum(this GalleryItem galleryItem, record record)

		private static Movie ToMovie(this record record)
		{
			var movie = new Movie
			{
				Description = record.description,
				PosterPath = record.posterPath,
				PosterImageHeight = record.posterImageHeight,
				PosterImageWidth = record.posterImageWidth,
				MimeType = record.mimetype,
				RefMoviePath = record.refMoviePath,
				WebImagePath = record.webImagePath,
				WebImageHeight = record.webImageHeight,
				WebImageWidth = record.webImageWidth,
				FileExtension = record.fileExtension,
				AddComment = record.addComment,
				VideoPathMobile = record.videoPathMobile,
				VideoHeightMobile = record.videoHeightMobile,
				VideoWidthMobile = record.videoWidthMobile,
				VideoDurationMobile = record.videoDurationMobile,
				VideoPathSmall = record.videoPathSmall,
				VideoHeightSmall = record.videoHeightSmall,
				VideoWidthSmall = record.videoWidthSmall,
				VideoDurationSmall = record.videoDurationSmall,
				VideoPathMedium = record.videoPathMedium,
				VideoHeightMedium = record.videoHeightMedium,
				VideoWidthMedium = record.videoWidthMedium,
				VideoDurationMedium = record.videoDurationMedium
			};
			movie.SetCommon(record);
			return movie;
		} //end private static Movie ToMovie(this GalleryItem galleryItem, record record)

		public static Album ToAlbum(this album album)
		{
			var newAlbum = new Album();
			newAlbum.Photos = new List<Photo>();
			album.records.Where(i => i.type == "Photo").ToList().ForEach(item => newAlbum.Photos.Add(item.ToPhoto()));
			return newAlbum;
		} //end private static Album ToAlbum(this album album)

		private static Photo ToPhoto(this photo photo)
		{
			var newPhoto = new Photo
			{
				Title = photo.title,
				LargeImagePath = photo.largeImagePath,
				WebImagePath = photo.webImagePath,
				WebImageHeight = photo.webImageHeight,
				WebImageWidth = photo.webImageWidth,
				PhotoDate = photo.photoDate,
				UserHidden = photo.userHidden,
				UserItemGuid = photo.userItemGuid,
				SortOrder = photo.sortOrder,
				ModDate = photo.modDate,
				Guid = photo.guid,
				Content = photo.content,
				UserOrderIndex = photo.userOrderIndex,
				FileExtension = photo.fileExtension,
				ArchiveDate = photo.archiveDate,
				VersionInfo = photo.versionInfo,
				Type = photo.type,
				Updated = photo.updated,
				Url = photo.url,
				ViewIdentifier = photo.viewIdentifier,
				Album = photo.album
			};

			return newPhoto;
		} //end private static Photo ToPhoto(this photo photo)
	}
}
