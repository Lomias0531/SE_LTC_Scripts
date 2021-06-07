using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Engine.Multiplayer
{
	[ProtoContract]
	public struct MyControlSendPasswordHashMsg
	{
		protected class Sandbox_Engine_Multiplayer_MyControlSendPasswordHashMsg_003C_003EPasswordHash_003C_003EAccessor : IMemberAccessor<MyControlSendPasswordHashMsg, byte[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyControlSendPasswordHashMsg owner, in byte[] value)
			{
				owner.PasswordHash = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyControlSendPasswordHashMsg owner, out byte[] value)
			{
				value = owner.PasswordHash;
			}
		}

		private class Sandbox_Engine_Multiplayer_MyControlSendPasswordHashMsg_003C_003EActor : IActivator, IActivator<MyControlSendPasswordHashMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(MyControlSendPasswordHashMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyControlSendPasswordHashMsg CreateInstance()
			{
				return (MyControlSendPasswordHashMsg)(object)default(MyControlSendPasswordHashMsg);
			}

			MyControlSendPasswordHashMsg IActivator<MyControlSendPasswordHashMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public byte[] PasswordHash;
	}
}
