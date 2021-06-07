using System;
using System.Runtime.Serialization;

namespace VRage.Scripting
{
	/// <summary>
	/// Exceptions during registration of whitelisted type members
	/// </summary>
	[Serializable]
	public class MyWhitelistException : Exception
	{
		public MyWhitelistException()
		{
		}

		public MyWhitelistException(string message)
			: base(message)
		{
		}

		public MyWhitelistException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected MyWhitelistException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
