using System;

namespace VRage.Network
{
	public struct MyEventContext
	{
		public struct Token : IDisposable
		{
			private readonly MyEventContext m_oldContext;

			public Token(MyEventContext newContext)
			{
				m_oldContext = m_current;
				m_current = newContext;
			}

			void IDisposable.Dispose()
			{
				m_current = m_oldContext;
			}
		}

		[ThreadStatic]
		private static MyEventContext m_current;

		/// <summary>
		/// Event sender, default(EndpointId) when invoked locally.
		/// </summary>
		public readonly EndpointId Sender;

		/// <summary>
		/// Event sender client data, valid only when invoked remotely on server, otherwise null.
		/// </summary>
		public readonly MyClientStateBase ClientState;

		public static MyEventContext Current => m_current;

		public bool IsLocallyInvoked
		{
			get;
			private set;
		}

		public bool HasValidationFailed
		{
			get;
			private set;
		}

		public bool IsValid
		{
			get;
			private set;
		}

		public static void ValidationFailed()
		{
			m_current.HasValidationFailed = true;
		}

		private MyEventContext(EndpointId sender, MyClientStateBase clientState, bool isInvokedLocally)
		{
			this = default(MyEventContext);
			Sender = sender;
			ClientState = clientState;
			IsLocallyInvoked = isInvokedLocally;
			HasValidationFailed = false;
			IsValid = true;
		}

		public static Token Set(EndpointId endpoint, MyClientStateBase client, bool isInvokedLocally)
		{
			return new Token(new MyEventContext(endpoint, client, isInvokedLocally));
		}
	}
}
