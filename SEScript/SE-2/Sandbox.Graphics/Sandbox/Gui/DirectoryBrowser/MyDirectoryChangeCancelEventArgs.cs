namespace Sandbox.Gui.DirectoryBrowser
{
	public sealed class MyDirectoryChangeCancelEventArgs : MyCancelEventArgs
	{
		public string From
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public MyGuiControlDirectoryBrowser Browser
		{
			get;
			private set;
		}

		public MyDirectoryChangeCancelEventArgs(string from, string to, MyGuiControlDirectoryBrowser browser)
		{
			From = from;
			To = to;
			Browser = browser;
		}
	}
}
