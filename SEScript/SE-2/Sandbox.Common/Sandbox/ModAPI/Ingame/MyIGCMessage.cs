namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Provides info about received message along with received data
	/// </summary>
	public struct MyIGCMessage
	{
		/// <summary>
		/// The data received in message.
		/// </summary>
		public readonly object Data;

		/// <summary>
		/// Tag designing type of this message.
		/// </summary>
		public readonly string Tag;

		/// <summary>
		/// Source/Author of this message.
		/// </summary>
		public readonly long Source;

		public MyIGCMessage(object data, string tag, long source)
		{
			Tag = tag;
			Data = data;
			Source = source;
		}

		public TData As<TData>()
		{
			return (TData)Data;
		}
	}
}
