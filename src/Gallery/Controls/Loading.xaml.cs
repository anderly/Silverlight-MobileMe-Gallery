using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Gallery
{
	public partial class Loading : UserControl
	{
		public Loading()
		{
			// Required to initialize variables
			InitializeComponent();
			
			BeginAnimation();
			this.Loaded += new RoutedEventHandler(Page_Loaded);
		}

		void Page_Loaded(object sender, RoutedEventArgs e)
		{
			if (_modal)
			{
				ModalFill.Visibility = Visibility.Visible;
			}
			else
			{
				ModalFill.Visibility = Visibility.Collapsed;
			}
		}

		public double Opacity
		{
			get { return Background.Opacity; }
			set { Background.Opacity = value; }
		}

		private bool _modal = true;

		public bool Modal
		{
			get { return _modal; }
			set { _modal = value; }
		}

		void BeginAnimation()
		{
			var acm = new AnimationChainManager(false);

			int delay = 0;

			foreach (UIElement child in SpinWheel.Children)
			{
				acm.Add()
					 .DoubleAnimationK() // Create a new Double animation with keyframes
					 .Target(child) // Target the UIElement
					 .Property(Path.OpacityProperty) // Change the opacity of the object.
					 .KeyFrame(1, new TimeSpan(0))// At time 0, have opacity of 100%
					 .KeyFrame(.3, new TimeSpan(0, 0, 0, 1, 0)) // Move to 30% at 1 second
					 .KeyFrame(.3, new TimeSpan(0, 0, 0, 0, 700)) // Wait 700 ms before repeating (this is how long it takes to get back around
					 .Repeat() // Keep doing it forever
					 .Offset(new TimeSpan(0, 0, 0, 0, delay))
					 .Queue();

				delay += 100; // wait 100ms in between each animation, so they start slightly after each other (create the wheel effect)
			}

			acm.Begin(false, false);
		}
	}
}