namespace VRage.Utils
{
	public static class MyClipboardHelper
	{
		public static void SetClipboard(string text)
		{
			MyVRage.Platform.Clipboard = text;
		}
	}
}
