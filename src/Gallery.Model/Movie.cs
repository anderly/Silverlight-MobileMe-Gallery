using System;

namespace Gallery.Model
{
	public class Movie : GalleryItem
	{
		#region : Public Properties :

		/// <summary>
		/// description
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// PosterPath
		/// </summary>
		public string PosterPath { get; set; }

		/// <summary>
		/// posterImageHeight
		/// </summary>
		public int PosterImageHeight { get; set; }

		/// <summary>
		/// posterImageWidth
		/// </summary>
		public int PosterImageWidth { get; set; }

		/// <summary>
		/// mimetype
		/// </summary>
		public string MimeType { get; set; }

		/// <summary>
		/// refMoviePath
		/// </summary>
		public string RefMoviePath { get; set; }

		/// <summary>
		/// webImagePath
		/// </summary>
		public string WebImagePath { get; set; }

		/// <summary>
		/// webImageHeight
		/// </summary>
		public int WebImageHeight { get; set; }

		/// <summary>
		/// webImageWidth
		/// </summary>
		public int WebImageWidth { get; set; }

		/// <summary>
		/// fileExtension 
		/// </summary>
		public string FileExtension { get; set; }

		/// <summary>
		/// addComment
		/// </summary>
		public bool AddComment { get; set; }

		/// <summary>
		/// VideoDurationDisplay 
		/// </summary>
		public string VideoDurationDisplay
		{
			get
			{
				return GetElapsedTime(VideoDurationMedium);
			}
		}

		#region : Mobile :

		/// <summary>
		/// videoPathMobile
		/// </summary>
		public string VideoPathMobile { get; set; }

		/// <summary>
		/// videoHeightMobile
		/// </summary>
		public int VideoHeightMobile { get; set; }

		/// <summary>
		/// videoWidthMobile
		/// </summary>
		public int VideoWidthMobile { get; set; }

		/// <summary>
		/// videoDurationMobile
		/// </summary>
		public decimal VideoDurationMobile { get; set; }

		#endregion : Mobile :

		#region : Small :

		/// <summary>
		/// videoPathSmall
		/// </summary>
		public string VideoPathSmall { get; set; }

		/// <summary>
		/// videoHeightSmall
		/// </summary>
		public int VideoHeightSmall { get; set; }

		/// <summary>
		/// videoWidthSmall
		/// </summary>
		public int VideoWidthSmall { get; set; }

		/// <summary>
		/// videoDurationSmall
		/// </summary>
		public decimal VideoDurationSmall { get; set; }

		#endregion : Small :

		#region : Medium :

		/// <summary>
		/// videoPathMedium
		/// </summary>
		public string VideoPathMedium { get; set; }
		
		/// <summary>
		/// videoHeightMedium
		/// </summary>
		public int VideoHeightMedium { get; set; }
		
		/// <summary>
		/// videoWidthMedium
		/// </summary>
		public int VideoWidthMedium { get; set; }
		
		/// <summary>
		/// videoDurationMedium
		/// </summary>
		public decimal VideoDurationMedium { get; set; }

		#endregion : Medium :

		#endregion : Public Properties :

		#region : Private Helper Functions :

		private string GetElapsedTime(decimal d)
		{
			string elapsedTime = string.Empty;

			int hh;
			int mm;
			int ss;
			decimal _hh;
			decimal _mm;
			decimal _ss;
			_hh = (d / 3600);
			hh = (int)Decimal.Truncate(_hh);
			_mm = DecimalPart(_hh) * 60;
			mm = (int)Decimal.Truncate(_mm);
			_ss = (DecimalPart(_mm) * 60);
			ss = (int)Decimal.Truncate(_ss);
			elapsedTime = string.Format("{0}:{1}", mm.ToString("00"), ss.ToString("00"));
			return elapsedTime;
		}

		private decimal DecimalPart(decimal d)
		{
			decimal decimalPart;
			decimal wholePart;
			decimal fractionalPart;
			wholePart = Decimal.Truncate(d);
			fractionalPart = (d - wholePart);
			decimalPart = fractionalPart;
			return decimalPart;
		}

		#endregion : Private Helper Functions :

	} //end public class Movie : GalleryItem

} //end namespace Gallery.Model
