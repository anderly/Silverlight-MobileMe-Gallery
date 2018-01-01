//Copyright 2008 Jordan Knight
//This code is licenced under Creative Commons Licence: http://creativecommons.org/licenses/by-sa/3.0/

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gallery
{
	public class AnimationConfig : IDisposable
	{
		private object context;
		Timeline timeLine;

		Action<AnimationConfig, object> completeAction = null;

		public event EventHandler Complete;

		public AnimationConfig(Timeline timeLine, object context)
		{
			this.context = context;
			this.timeLine = timeLine;
			timeLine.Completed += new EventHandler(timeLine_Completed);
		}

		void timeLine_Completed(object sender, EventArgs e)
		{
			timeLine.Completed -= new EventHandler(timeLine_Completed);

			if (Complete != null)
			{
				Complete(this, EventArgs.Empty);
			}
			if (completeAction != null)
			{
				completeAction(this, context);
			}
		}

		internal Timeline TimeLine
		{
			get
			{
				return timeLine;
			}
		}

		public Action<AnimationConfig, object> CompleteAction
		{
			set
			{
				completeAction = value;
			}
		}


		#region IDisposable Members

		public void Dispose()
		{
			timeLine = null;
		}

		#endregion
	}


}
