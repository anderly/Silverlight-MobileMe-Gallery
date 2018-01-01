using System.Diagnostics;
using System.Globalization;


namespace Gallery.Core
{

	public static class Culture
	{
		public static CultureInfo Current
		{
			[DebuggerStepThrough]
			get
			{
				return CultureInfo.CurrentUICulture;
			}
		}

		public static CultureInfo Invariant
		{
			[DebuggerStepThrough]
			get
			{
				return CultureInfo.InvariantCulture;
			}
		}
	}
}
