namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Base interface for all message providers.
	/// </summary>
	public interface IMyMessageProvider
	{
		/// <summary>
		/// Determines whether there is a message pending to be accepted in this message provider or not.
		/// There may be multiple messages pending in single message provider. In such case the flag will stay raised until the last message is consumed.
		/// </summary>
		bool HasPendingMessage
		{
			get;
		}

		/// <summary>
		/// Indicates number of max messages waiting in queue before the oldest one will be dropped to make space for new one.
		/// </summary>
		int MaxWaitingMessages
		{
			get;
		}

		/// <summary>
		/// Accepts first message from pending message queue for this message provider.
		/// Messages are guaranteed to be dequeued and returned by this method in order they arrived.
		/// </summary>
		/// <returns>First message from pending message queue, default(<see cref="T:Sandbox.ModAPI.Ingame.MyIGCMessage" />) if there are no messages pending to be accepted.</returns>
		MyIGCMessage AcceptMessage();

		/// <summary>
		/// Disables registered message callback.
		/// </summary>
		void DisableMessageCallback();

		/// <summary>
		/// Whenever given message provider obtains new message respective programmable block gets called with provided argument.
		/// Each raised callback argument will be called only once per simulation tick no matter how many messages are there pending to be accepted.
		/// At most a single callback will be invoked each tick
		/// ==&gt; If there are messages pending in multiple message providers with registered callback, one will be randomly picked and invoked. Rest will be deferred to the next tick and follow the same process.
		/// In case you don't consume all messages pending in given message provider, the callback will not be raise again in following tick unless new message arrives.
		/// </summary>
		/// <param name="argument"></param>
		void SetMessageCallback(string argument = "");
	}
}
