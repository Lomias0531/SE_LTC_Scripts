namespace Sandbox
{
	public interface IErrorConsumer
	{
		void OnError(string header, string message, string callstack);
	}
}
