using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Gallery.Contracts;
using Gallery.Core;
using Gallery.Model;

namespace Gallery
{
	public partial class VideoPlayer : UserControl
	{
		#region : Private Fields :

		//VideoPlayer;component/video.wmv
		private DispatcherTimer _Timer = new DispatcherTimer();
		private double _defaultPlayerWidth = 0.0;
		private double _defaultPlayerHeight = 0.0;
		private DateTime _lastClick = DateTime.Now;

		private bool _autoHideControls = false;

		#endregion : Private Fields :

		#region : Public Properties :

		[Import]
		public IGalleryContext GalleryContext { get; set; }

		#endregion : Public Properties :

		#region : Constructor :

		public VideoPlayer()
		{
			InitializeComponent();
			CompositionInitializer.SatisfyImports(this);
			//this timer will be used to hide the controls when a user has moused away
			_Timer.Interval = new TimeSpan(0, 0, 5);
			_Timer.Tick += _Timer_Tick;
			Loaded += Page_Loaded;
			Application.Current.Host.Content.FullScreenChanged += Content_FullScreenChanged;
		}

		#endregion : Constructor :

		#region : Handlers :

		void Page_Loaded(object sender, RoutedEventArgs e)
		{
			LayoutRoot2.MouseEnter += LayoutRoot2_MouseEnter;
			LayoutRoot.MouseLeftButtonUp += LayoutRoot_MouseLeftButtonUp;
			controlsContainer.MouseLeave += controlsContainer_MouseLeave;

			#region : Additional Startup Params :

			if (Application.Current.Resources.Contains("autostart"))
			{
				mediaPlayer.AutoPlay = Convert.ToBoolean(Application.Current.Resources["autostart"]);
			}
			if (Application.Current.Resources.Contains("thumbnail"))
			{
				ImageSource imgsrc = new System.Windows.Media.Imaging.BitmapImage(Application.Current.Resources["thumbnail"] as Uri);
				ThumbnailImage.SetValue(Image.SourceProperty, imgsrc);
				Thumbnail.Visibility = Visibility.Visible;
			}
			if (Application.Current.Resources.Contains("muted"))
			{
				mediaPlayer.IsMuted = Convert.ToBoolean(Application.Current.Resources["muted"]);
			}
			if (Application.Current.Resources.Contains("autohide"))
			{
				_autoHideControls = Convert.ToBoolean(Application.Current.Resources["autohide"]);
			}
			if (Application.Current.Resources.Contains("canscrub"))
			{
				mediaControls.sliderTimeline.IsHitTestVisible = Convert.ToBoolean(Application.Current.Resources["canscrub"]);
			}

			#endregion

			var movie = GalleryContext.SelectedItem as Movie;
			Check.Argument.IsNotNull(movie, "Movie");
			mediaPlayer.Source = new Uri(string.Format("{0}/{1}", movie.Url, movie.VideoPathMedium));

			mediaControls.Media = mediaPlayer;

			_defaultPlayerWidth = Width;
			_defaultPlayerHeight = Height;

			RepositionCenterPlay();

			mediaControls.FullScreenClicked += mediaControls_FullScreenClicked;
			mediaControls.PlayClicked += mediaControls_PlayClicked;
			mediaControls.PauseClicked += mediaControls_PauseClicked;
			mediaControls.StopClicked += mediaControls_StopClicked;
			mediaControls.BufferingStart += mediaControls_BufferingStart;
			mediaControls.BufferingEnd += mediaControls_BufferingEnd;
			mediaControls.MediaCompleted += mediaControls_MediaCompleted;
			mediaControls.TearOff += mediaControls_TearOff;

			controlsContainer.Visibility = Visibility.Visible;
			controlsIn.Begin();

			// check to see if a position was specified in query string
			if (HtmlPage.Document.QueryString.ContainsKey("p"))
			{
				var startTime = TimeSpan.FromSeconds(Convert.ToDouble(HtmlPage.Document.QueryString["p"]));
				mediaControls.Position = startTime;
			}

		}

		void mediaControls_TearOff(object sender, TimeSpan position)
		{
			// popup here.
			string pos = position.TotalSeconds.ToString();
			var jsObj = (ScriptObject)HtmlPage.Window.GetProperty("tearWin");
			jsObj.InvokeSelf(pos);
			//HtmlPage.Window.Navigate(new Uri("http://localhost:21945/VidMinTester/tear.htm?p=" + pos), "_blank");
		}

		void LayoutRoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			// The the last click was within 500ms...
			if ((DateTime.Now - _lastClick).TotalMilliseconds < 500)
			{
				// Toggle fullscreen
				Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;

				_lastClick = DateTime.Now.AddSeconds(-1);
			}
			else
			{

				// Store the last click time
				_lastClick = DateTime.Now;
			}
		}
		void mediaControls_MediaCompleted(object sender, EventArgs e)
		{
			Thumbnail.Visibility = Visibility.Visible;
		}
		void mediaControls_BufferingEnd(object sender, EventArgs e)
		{
			//BigBuffer.Animation.Stop();
			Loading.Visibility = Visibility.Collapsed;
		}
		void mediaControls_BufferingStart(object sender, EventArgs e)
		{
			//BigBuffer.Animation.Begin();
			Loading.Visibility = Visibility.Visible;
		}
		void mediaControls_StopClicked(object sender, EventArgs e)
		{
			PlayButton.Visibility = Visibility.Visible;
		}
		void mediaControls_PauseClicked(object sender, EventArgs e)
		{
			PlayButton.Visibility = Visibility.Visible;
		}
		void mediaControls_PlayClicked(object sender, EventArgs e)
		{
			Thumbnail.Visibility = Visibility.Collapsed;
			PlayButton.Visibility = Visibility.Collapsed;
		}
		void Content_FullScreenChanged(object sender, EventArgs e)
		{
			if (Application.Current.Host.Content.IsFullScreen == true)
			{
				var targetWidth = (double)Application.Current.Host.Content.ActualWidth;
				var targetHeight = (double)Application.Current.Host.Content.ActualHeight;

				Width = targetWidth;
				Height = targetHeight;

				mediaPlayer.Width = targetWidth;
				mediaPlayer.Height = targetHeight;
			}

			else
			{
				Width = _defaultPlayerWidth;
				Height = _defaultPlayerHeight;

				mediaPlayer.Width = _defaultPlayerWidth;
				mediaPlayer.Height = _defaultPlayerHeight;
			}

			RepositionCenterPlay();
		}
		void mediaControls_FullScreenClicked(object sender, EventArgs e)
		{
			if (Application.Current.Host.Content.IsFullScreen)
			{
				Application.Current.Host.Content.IsFullScreen = false;
			}
			else
			{
				Application.Current.Host.Content.IsFullScreen = true;
			}
		}
		void controlsContainer_MouseLeave(object sender, MouseEventArgs e)
		{
			if (_autoHideControls)
			{
				_Timer.Start();
			}
		}
		void _Timer_Tick(object sender, EventArgs e)
		{
			controlsOut.Completed += new EventHandler(controlsOut_Completed);
			controlsOut.Begin();
		}
		void controlsOut_Completed(object sender, EventArgs e)
		{
			controlsContainer.Visibility = Visibility.Collapsed;
		}
		void LayoutRoot2_MouseEnter(object sender, MouseEventArgs e)
		{
			if (_autoHideControls)
			{
				if (controlsContainer.Visibility == Visibility.Collapsed)
				{
					controlsContainer.Visibility = Visibility.Visible;
					controlsIn.Begin();
				}
				_Timer.Stop();
			}
		}
		//private void PlayIcon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		//{
		//    mediaControls.CenterPlay();
		//}

		#endregion : Handlers :

		private void RepositionCenterPlay()
		{
			var newLeft = (Application.Current.Host.Content.ActualWidth / 2) - 50;
			var newTop = (Application.Current.Host.Content.ActualHeight / 2) - 50;

			PlayButton.SetValue(Canvas.LeftProperty, newLeft);
			PlayButton.SetValue(Canvas.TopProperty, newTop);
		}

		[ScriptableMember]
		public void SeekPlayback(string time)
		{
			var tsTime = TimeSpan.Parse(time);
			mediaControls.Seek(tsTime);
		}

		[ScriptableMember]
		public void TearOffPlayer()
		{
			mediaControls.TearOffPlayer();
		}

		private void PlayButton_Click(object sender, RoutedEventArgs e)
		{
			mediaControls.CenterPlay();

			var timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 500) };
			timer.Tick += delegate
							{
								VisualStateManager.GoToState(PlayButton, "Normal", true);
								timer.Stop();
							};
			timer.Start();

		}
	}
}
