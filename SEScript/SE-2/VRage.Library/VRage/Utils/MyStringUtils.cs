namespace VRage.Utils
{
	public static class MyStringUtils
	{
		public const string OPEN_SQUARE_BRACKET = "U+005B";

		public const string CLOSED_SQUARE_BRACKET = "U+005D";

		/// <summary>
		/// Converts '[' and ']' into their UTF form to avoid being removed by notification processing system.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string UpdateControlsToNotificationFriendly(this string text)
		{
			return text.Replace("[", "U+005B").Replace("]", "U+005D");
		}

		/// <summary>
		/// Converts '[' and ']' UTF form to the regular characters.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string UpdateControlsFromNotificationFriendly(this string text)
		{
			return text.Replace("U+005B", "[").Replace("U+005D", "]");
		}
	}
}
