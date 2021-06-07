using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_PlayerChatHistory : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_PlayerChatHistory_003C_003EChat_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerChatHistory, List<MyObjectBuilder_PlayerChatItem>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlayerChatHistory owner, in List<MyObjectBuilder_PlayerChatItem> value)
			{
				owner.Chat = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlayerChatHistory owner, out List<MyObjectBuilder_PlayerChatItem> value)
			{
				value = owner.Chat;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerChatHistory_003C_003EIdentityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerChatHistory, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlayerChatHistory owner, in long value)
			{
				owner.IdentityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlayerChatHistory owner, out long value)
			{
				value = owner.IdentityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerChatHistory_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatHistory, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlayerChatHistory owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlayerChatHistory owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerChatHistory_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatHistory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlayerChatHistory owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlayerChatHistory owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerChatHistory_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatHistory, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlayerChatHistory owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlayerChatHistory owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerChatHistory_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatHistory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlayerChatHistory owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlayerChatHistory owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_PlayerChatHistory_003C_003EActor : IActivator, IActivator<MyObjectBuilder_PlayerChatHistory>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_PlayerChatHistory();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_PlayerChatHistory CreateInstance()
			{
				return new MyObjectBuilder_PlayerChatHistory();
			}

			MyObjectBuilder_PlayerChatHistory IActivator<MyObjectBuilder_PlayerChatHistory>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(10)]
		[XmlArrayItem("PCI")]
		public List<MyObjectBuilder_PlayerChatItem> Chat;

		[ProtoMember(13)]
		[XmlElement(ElementName = "ID")]
		public long IdentityId;
	}
}
