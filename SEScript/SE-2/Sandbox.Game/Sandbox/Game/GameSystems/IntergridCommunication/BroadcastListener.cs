using Sandbox.ModAPI.Ingame;
using System;

namespace Sandbox.Game.GameSystems.IntergridCommunication
{
	internal class BroadcastListener : MyMessageListener, IMyBroadcastListener, IMyMessageProvider
	{
		public string Tag
		{
			get;
			private set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public BroadcastListener(MyIntergridCommunicationContext context, string tag)
			: base(context)
		{
			Tag = tag;
		}

		public override void SetMessageCallback(string argument)
		{
			if (!IsActive)
			{
				throw new Exception("Callbacks are not supported for disabled broadcast listeners!");
			}
			base.SetMessageCallback(argument);
		}

		public override MyIGCMessage AcceptMessage()
		{
			MyIGCMessage result = base.AcceptMessage();
			if (!IsActive && !base.HasPendingMessage)
			{
				base.Context.DisposeBroadcastListener(this, keepIfHavingPendingMessages: false);
			}
			return result;
		}
	}
}
