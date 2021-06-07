namespace Sandbox
{
	internal class ErrorInfo
	{
		public string Match;

		public string Caption;

		public string Message;

		public ErrorInfo(string match, string caption, string message)
		{
			Match = match;
			Caption = caption;
			Message = message;
		}
	}
}
