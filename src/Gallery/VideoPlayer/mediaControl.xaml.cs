using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.IO;
using System.Text;
using System.Windows.Markup;
using System.Linq;
using System.Xml;
using System.Net;
using System.Windows.Threading;
using System.Windows.Browser;

namespace Gallery
{
	public partial class mediaControl : UserControl
	{
        public delegate TimeSpan TimeLine(TimeSpan position);
        private DispatcherTimer _playTimer = new DispatcherTimer();
        private TimeSpan _duration;
        private TimeSpan _setPosition = TimeSpan.FromSeconds(0.0);

        public delegate void EventHandler(Object sender, EventArgs e);
        public delegate void SoundEventHandler(Object sender, bool isMuted);
        public delegate void TearOffEventHandler(Object sender, TimeSpan position);

        public event EventHandler PlayClicked;
        public event SoundEventHandler SoundChanged;
        public event EventHandler PauseClicked;
        public event EventHandler StopClicked;
        public event EventHandler FullScreenClicked;
        public event EventHandler BufferingStart;
        public event EventHandler BufferingEnd;
        public event EventHandler MediaCompleted;
        public event TearOffEventHandler TearOff;

        private MediaElement _media = null;
        private bool _buffering = false;
        private bool _timeRemaining = false;
        private bool _seekable = true;
        private double _lastVolume = 0;

        public void CenterPlay()
        {
            btnPlay.IsChecked = true;
            btnPlay_Checked(this, null);
        }

        public MediaElement Media
        {
            set
            {
                _media = value;
                if (_media != null)
                {
                    _media.MediaOpened += new RoutedEventHandler(_media_MediaOpened);
                    
                }
            }
        }

        public TimeSpan Position
        {
            set
            {
                _setPosition = value;
            }
        }

        public double Volume
        {
            set
            {
                sliderVolume.Value = value;
            }
        }

        public mediaControl()
        {
            // Required to initialize variables
            InitializeComponent();

            spinner.Visibility = Visibility.Collapsed;

            this.btnPlay.Checked += new RoutedEventHandler(btnPlay_Checked);
            this.btnPlay.Unchecked += new RoutedEventHandler(btnPlay_Unchecked);
            
            this.btnSpeaker.Checked += new RoutedEventHandler(btnSpeaker_Checked);
            this.btnSpeaker.Unchecked += new RoutedEventHandler(btnSpeaker_Unchecked);

            this.btnFullScreen.Click += new RoutedEventHandler(btnFullScreen_Click);

            this.tbCurrentTime.MouseLeftButtonUp += new MouseButtonEventHandler(tbCurrentTime_MouseLeftButtonUp);

            this.sliderTimeline.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderTimeline_ValueChanged);
            this.sliderTimeline.LargeChange = .1;
            this.sliderTimeline.SmallChange = .05;

            this.sliderVolume.LargeChange = .125;
            this.sliderVolume.SmallChange = .125;

            this.sliderTimeline.Maximum = 1;

            this.sliderVolume.Value = .5;
            this.sliderVolume.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderVolume_ValueChanged);

            //this will be used to update the time textblock and the slider
            _playTimer.Interval = TimeSpan.FromMilliseconds(50);
            _playTimer.Tick += new System.EventHandler(_playTimer_Tick);
            _lastVolume = sliderVolume.Value;
        }

        #region Handlers
        void _media_MediaOpened(object sender, RoutedEventArgs e)
        {
            _duration = _media.NaturalDuration.TimeSpan;
            tbTotalTime.Text = string.Format("{0:00}:{1:00}", (_duration.Hours * 60) +_duration.Minutes, _duration.Seconds);

            _media.CurrentStateChanged += new RoutedEventHandler(_media_CurrentStateChanged);
            _media.DownloadProgressChanged += new RoutedEventHandler(_media_DownloadProgressChanged);
            _media.MediaEnded += new RoutedEventHandler(_media_MediaEnded);

            if (_setPosition != TimeSpan.FromSeconds(0.0))
            {
                _media.Position = _setPosition;
                // set slider position
                // this should trigger a position set on the media
                sliderTimeline.Value = _media.Position.TotalSeconds/10;
            }

            // need to check seek state
            _seekable = _media.CanSeek;

            if (!_seekable)
            {
                //sliderTimeline.IsHitTestVisible = _seekable;
                sliderTimeline.Visibility = Visibility.Collapsed;
            }

            if (_media.AutoPlay)
            {
                _playTimer.Start();
                btnPlay.IsChecked = true;
            }

            if (_media.DownloadProgress == 1.0 && _seekable)
            {
                SetProgressSlider(1.0);
            }
        }
        
        void _media_DownloadProgressChanged(object sender, RoutedEventArgs e)
        {
            SetProgressSlider(_media.DownloadProgress);
        }

        private void SetProgressSlider(double sliderPosition)
        {
            // get the scale transform
            Rectangle progress = sliderTimeline.ProgressBar;
            progress.RenderTransform.SetValue(ScaleTransform.ScaleXProperty, sliderPosition);
        }

        void _media_MediaEnded(object sender, RoutedEventArgs e)
        {
            _playTimer.Stop();
            btnPlay.IsChecked = false;
            tbCurrentTime.Text = "00:00";
            this.sliderTimeline.Value = 0;

            if (MediaCompleted != null)
            {
                MediaCompleted(this, null);
            }
        }

        void _media_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (_media.CurrentState == MediaElementState.Playing)
            {
                _playTimer.Start();
                spinner.Visibility = Visibility.Collapsed;

                if (BufferingEnd != null)
                {
                    BufferingEnd(sender, e);
                }
            }
            else if (_media.CurrentState == MediaElementState.Buffering)
            {
                if (spinner.Visibility == Visibility.Collapsed)
                {
                    spinner.Visibility = Visibility.Visible;
                    spinner.Animation.Begin();
                }
                if (BufferingStart != null)
                {
                    BufferingStart(sender, e);
                }
            }
            else
            {
                spinner.Visibility = Visibility.Collapsed;
                if (BufferingEnd != null)
                {
                    BufferingEnd(sender, e);
                }
            }
        }

        void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            OnFullScreenClick();
        }

        void tbCurrentTime_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetCurrentTime();
            if (_timeRemaining)
                _timeRemaining = false;
            else
                _timeRemaining = true;
        }

        void btnSpeaker_Unchecked(object sender, RoutedEventArgs e)
        {
            _media.IsMuted = true;
            _lastVolume = sliderVolume.Value;
            sliderVolume.Value = 0.0;
            OnSoundChanged();
        }

        void btnSpeaker_Checked(object sender, RoutedEventArgs e)
        {
            _media.IsMuted = false;
            sliderVolume.Value = _lastVolume;
            OnSoundChanged();
        }

        void _media_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            if (_buffering && _media.BufferingProgress == 1.0)
            {
                spinner.Visibility = Visibility.Collapsed;
                spinner.Animation.Stop();
                _buffering = false;
            }
            else if (!_buffering && _media.BufferingProgress > 0 && _media.BufferingProgress < 1.0)
            {
                spinner.Visibility = Visibility.Visible;
                spinner.Animation.Begin();
                _buffering = true;
            }

        }

        void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
             _media.Volume = e.NewValue;
        }

        void sliderTimeline_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double tValue = (_media.Position.TotalSeconds/_duration.TotalSeconds);
            if (e.NewValue != tValue)
            {
                _media.Position = TimeSpan.FromSeconds(_duration.TotalSeconds * e.NewValue);
            }
        }

        void _playTimer_Tick(object sender, EventArgs e)
        {
            SetCurrentTime();
            if (_duration.TotalSeconds > 0)
            {
                this.sliderTimeline.ValueChanged -= sliderTimeline_ValueChanged;
                this.sliderTimeline.Value = (_media.Position.TotalSeconds / _duration.TotalSeconds);
                this.sliderTimeline.ValueChanged += sliderTimeline_ValueChanged;
            }
        }

        void btnPlay_Checked(object sender, RoutedEventArgs e)
        {
            _media.Play();
            OnPlayClicked();
        }
        void btnPlay_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_media.CanPause)
            {
                _media.Pause();
                OnPauseClicked();
            }
            else
            {
                _media.Stop();
            }
        }

        public void TearOffPlayer()
        {
            if (_media.CanPause)
            {
                _media.Pause();
                OnTearOff(_media.Position);
            }
        }

        private void OnTearOff(TimeSpan currentPosition)
        {
            if (TearOff != null)
            {
                TearOff(this, currentPosition);
            }
        }
        #endregion

        #region Event helpers
        private void OnSoundChanged()
        {
            if (SoundChanged != null)
            {
                SoundChanged(this,_media.IsMuted);
            }
        }
        private void OnFullScreenClick()
        {
            if (FullScreenClicked != null)
            {
                FullScreenClicked(this, null);
            }
        }
        private void OnBufferingStart()
        {
            if (BufferingStart != null)
            {
                BufferingStart(this, null);
            }
        }
        private void OnBufferingEnd()
        {
            if (BufferingEnd != null)
            {
                BufferingEnd(this, null);
            }
        }

        private void OnMediaCompleted()
        {
            if (MediaCompleted != null)
            {
                MediaCompleted(this, null);
            }
        }

        private void OnStopClicked()
        {
            if (StopClicked != null)
            {
                StopClicked(this, null);
            }
        }
       
        private void OnPlayClicked()
        {
            if (PlayClicked != null)
            {
                PlayClicked(this, null);
            }
        }
        
        private void OnPauseClicked()
        {
            if (PauseClicked != null)
            {
                PauseClicked(this, null);
            }
        }
        #endregion

        #region Private methods
        private void SetCurrentTime()
        {
            if (!_timeRemaining)
            {
                tbCurrentTime.Text = string.Format("{0:00}:{1:00}", (_media.Position.Hours * 60) +_media.Position.Minutes, _media.Position.Seconds);
            }
            else
            {
                TimeSpan remaining = _duration.Subtract(_media.Position);
                tbCurrentTime.Text = string.Format("{0:00}:{1:00}", (remaining.Hours * 60) + remaining.Minutes, remaining.Seconds);
            }
        }
        #endregion
        
        #region Public Methods
        public void Play()
        {
            btnPlay.IsChecked = true;
        }
        public void Stop()
        {
            btnPlay.IsChecked = false;
            _media.Position = new TimeSpan(0);
        }
        public void Seek(TimeSpan time)
        {
            if (_media.CanSeek)
            {
                if (time.TotalSeconds < _duration.TotalSeconds)
                {
                    double percentOfSeconds = time.TotalSeconds / _duration.TotalSeconds;
                    sliderTimeline.Value = percentOfSeconds;

                    _media.Play();
                    OnPlayClicked();
                }
            }
        }
        #endregion
    }
}