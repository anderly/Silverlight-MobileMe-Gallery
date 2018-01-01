using System.Diagnostics;
using Gallery.Core;

namespace Gallery.Extensions
{
	public static class StringExtensions
	{
		[DebuggerStepThrough]
		public static string FormatWith(this string instance, params object[] args)
		{
			Check.Argument.IsNotNullOrEmpty(instance, "instance");

			return string.Format(Culture.Current, instance, args);
		}
	}
}
