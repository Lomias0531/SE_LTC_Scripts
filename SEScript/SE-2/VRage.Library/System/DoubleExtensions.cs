using System.Diagnostics;

namespace System
{
	public static class DoubleExtensions
	{
		/// <summary>
		/// Returns true if double is valid
		/// </summary>
		public static bool IsValid(this double f)
		{
			if (!double.IsNaN(f))
			{
				return !double.IsInfinity(f);
			}
			return false;
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(this double f)
		{
		}
	}
}
