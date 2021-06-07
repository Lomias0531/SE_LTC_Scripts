using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using System;
using System.Text;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
	public class MyLaserBroadcaster : MyDataBroadcaster
	{
		protected sealed class ChangeStateText_003C_003ESystem_String : ICallSite<MyLaserBroadcaster, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLaserBroadcaster @this, in string newStateText, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ChangeStateText(newStateText);
			}
		}

		protected sealed class ChangeSuccessfullyContacting_003C_003ESystem_Nullable_00601_003CSystem_Int64_003E : ICallSite<MyLaserBroadcaster, long?, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLaserBroadcaster @this, in long? newContact, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ChangeSuccessfullyContacting(newContact);
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyLaserBroadcaster_003C_003EActor : IActivator, IActivator<MyLaserBroadcaster>
		{
			private sealed override object CreateInstance()
			{
				return new MyLaserBroadcaster();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLaserBroadcaster CreateInstance()
			{
				return new MyLaserBroadcaster();
			}

			MyLaserBroadcaster IActivator<MyLaserBroadcaster>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public StringBuilder StateText = new StringBuilder();

		public MyLaserAntenna RealAntenna => base.Entity as MyLaserAntenna;

		public long? SuccessfullyContacting
		{
			get
			{
				MyLaserAntenna realAntenna = RealAntenna;
				if (realAntenna != null && realAntenna.TargetId.HasValue)
				{
					if (realAntenna.CanLaseTargetCoords)
					{
						return realAntenna.TargetId;
					}
				}
				else
				{
					MyProxyAntenna myProxyAntenna = base.Entity as MyProxyAntenna;
					if (myProxyAntenna != null)
					{
						return myProxyAntenna.SuccessfullyContacting;
					}
				}
				return null;
			}
		}

		public override bool ShowOnHud => false;

		public override void InitProxyObjectBuilder(MyObjectBuilder_ProxyAntenna ob)
		{
			base.InitProxyObjectBuilder(ob);
			ob.IsLaser = true;
			ob.SuccessfullyContacting = SuccessfullyContacting;
			ob.StateText = StateText.ToString();
		}

		public void RaiseChangeSuccessfullyContacting()
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyLaserBroadcaster x) => x.ChangeSuccessfullyContacting, SuccessfullyContacting);
			}
		}

		public void RaiseChangeStateText()
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyLaserBroadcaster x) => x.ChangeStateText, StateText.ToString());
			}
		}

		[Event(null, 71)]
		[Reliable]
		[Broadcast]
		private void ChangeStateText(string newStateText)
		{
			if (base.Entity is MyProxyAntenna)
			{
				StateText.Clear();
				StateText.Append(newStateText);
			}
		}

		[Event(null, 84)]
		[Reliable]
		[Broadcast]
		private void ChangeSuccessfullyContacting(long? newContact)
		{
			MyProxyAntenna myProxyAntenna = base.Entity as MyProxyAntenna;
			if (myProxyAntenna != null)
			{
				myProxyAntenna.SuccessfullyContacting = newContact;
			}
		}
	}
}
