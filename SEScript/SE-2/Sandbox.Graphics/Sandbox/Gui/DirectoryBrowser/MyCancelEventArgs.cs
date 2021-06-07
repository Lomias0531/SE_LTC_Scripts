namespace Sandbox.Gui.DirectoryBrowser
{
	public class MyCancelEventArgs
	{
		public bool Cancel
		{
			get;
			set;
		}

		public MyCancelEventArgs()
		{
			Cancel = false;
		}
	}
}
