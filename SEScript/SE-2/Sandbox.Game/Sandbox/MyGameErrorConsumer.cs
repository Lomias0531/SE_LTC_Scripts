namespace Sandbox
{
	public class MyGameErrorConsumer : IErrorConsumer
	{
		public void OnError(string header, string message, string callstack)
		{
			_ = header + ": " + message + "\n\nStack:\n" + callstack;
		}
	}
}
