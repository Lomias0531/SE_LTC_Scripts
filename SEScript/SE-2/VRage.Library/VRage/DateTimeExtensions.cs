using System;

namespace VRage
{
	public static class DateTimeExtensions
	{
		public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Converts seconds passed since unix epoch into DateTime.
		/// </summary>
		/// <param name="timestamp"></param>
		/// <returns></returns>
		public static DateTime ToDateTimeFromUnixTimestamp(this uint timestamp)
		{
			DateTime epoch = Epoch;
			return epoch.AddSeconds(timestamp).ToUniversalTime();
		}

		/// <summary>
		/// Converts DateTime to seconds passed since 1/1/1970.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static uint ToUnixTimestamp(this DateTime time)
		{
			return (uint)time.Subtract(Epoch).TotalSeconds;
		}
	}
}
