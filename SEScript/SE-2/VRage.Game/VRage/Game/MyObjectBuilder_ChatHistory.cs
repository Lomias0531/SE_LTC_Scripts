using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	public class MyObjectBuilder_ChatHistory : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_ChatHistory_003C_003EIdentityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ChatHistory, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ChatHistory owner, in long value)
			{
				owner.IdentityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ChatHistory owner, out long value)
			{
				value = owner.IdentityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ChatHistory_003C_003EPlayerChatHistory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ChatHistory, List<MyObjectBuilder_PlayerChatHistory>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ChatHistory owner, in List<MyObjectBuilder_PlayerChatHistory> value)
			{
				owner.PlayerChatHistory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ChatHistory owner, out List<MyObjectBuilder_PlayerChatHistory> value)
			{
				value = owner.PlayerChatHistory;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ChatHistory_003C_003EGlobalChatHistory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ChatHistory, MyObjectBuilder_GlobalChatHistory>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ChatHistory owner, in MyObjectBuilder_GlobalChatHistory value)
			{
				owner.GlobalChatHistory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ChatHistory owner, out MyObjectBuilder_GlobalChatHistory value)
			{
				value = owner.GlobalChatHistory;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ChatHistory_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ChatHistory, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ChatHistory owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ChatHistory owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ChatHistory_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ChatHistory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ChatHistory owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ChatHistory owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ChatHistory_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ChatHistory, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ChatHistory owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ChatHistory owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ChatHistory_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ChatHistory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ChatHistory owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ChatHistory owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ChatHistory_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ChatHistory>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ChatHistory();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ChatHistory CreateInstance()
			{
				return new MyObjectBuilder_ChatHistory();
			}

			MyObjectBuilder_ChatHistory IActivator<MyObjectBuilder_ChatHistory>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public long IdentityId;

		[ProtoMember(4)]
		public List<MyObjectBuilder_PlayerChatHistory> PlayerChatHistory;

		[ProtoMember(7)]
		public MyObjectBuilder_GlobalChatHistory GlobalChatHistory;
	}
}
