using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	public class MyObjectBuilder_PlayerChatItem : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_PlayerChatItem_003C_003EText_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlayerChatItem owner, in string value)
			{
				owner.Text = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlayerChatItem owner, out string value)
			{
				value = owner.Text;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerChatItem_003C_003EIdentityIdUniqueNumber_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerChatItem, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlayerChatItem owner, in long value)
			{
				owner.IdentityIdUniqueNumber = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlayerChatItem owner, out long value)
			{
				value = owner.IdentityIdUniqueNumber;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerChatItem_003C_003ETimestampMs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerChatItem, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlayerChatItem owner, in long value)
			{
				owner.TimestampMs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlayerChatItem owner, out long value)
			{
				value = owner.TimestampMs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerChatItem_003C_003ESent_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerChatItem, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlayerChatItem owner, in bool value)
			{
				owner.Sent = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlayerChatItem owner, out bool value)
			{
				value = owner.Sent;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerChatItem_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatItem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlayerChatItem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlayerChatItem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerChatItem_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlayerChatItem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlayerChatItem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerChatItem_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatItem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlayerChatItem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlayerChatItem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerChatItem_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlayerChatItem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlayerChatItem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_PlayerChatItem_003C_003EActor : IActivator, IActivator<MyObjectBuilder_PlayerChatItem>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_PlayerChatItem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_PlayerChatItem CreateInstance()
			{
				return new MyObjectBuilder_PlayerChatItem();
			}

			MyObjectBuilder_PlayerChatItem IActivator<MyObjectBuilder_PlayerChatItem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(25)]
		[XmlAttribute("t")]
		public string Text;

		[ProtoMember(28)]
		[XmlElement(ElementName = "I")]
		public long IdentityIdUniqueNumber;

		[ProtoMember(31)]
		[XmlElement(ElementName = "T")]
		public long TimestampMs;

		[ProtoMember(34)]
		[DefaultValue(true)]
		[XmlElement(ElementName = "S")]
		public bool Sent = true;
	}
}
