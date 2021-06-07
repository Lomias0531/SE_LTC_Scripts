using ProtoBuf;
using System.Collections.Generic;
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
	public class MyObjectBuilder_FactionChatItem : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_FactionChatItem_003C_003EText_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionChatItem owner, in string value)
			{
				owner.Text = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionChatItem owner, out string value)
			{
				value = owner.Text;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionChatItem_003C_003EIdentityIdUniqueNumber_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionChatItem, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionChatItem owner, in long value)
			{
				owner.IdentityIdUniqueNumber = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionChatItem owner, out long value)
			{
				value = owner.IdentityIdUniqueNumber;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionChatItem_003C_003ETimestampMs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionChatItem, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionChatItem owner, in long value)
			{
				owner.TimestampMs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionChatItem owner, out long value)
			{
				value = owner.TimestampMs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionChatItem_003C_003EPlayersToSendToUniqueNumber_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionChatItem, List<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionChatItem owner, in List<long> value)
			{
				owner.PlayersToSendToUniqueNumber = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionChatItem owner, out List<long> value)
			{
				value = owner.PlayersToSendToUniqueNumber;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionChatItem_003C_003EIsAlreadySentTo_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionChatItem, List<bool>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionChatItem owner, in List<bool> value)
			{
				owner.IsAlreadySentTo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionChatItem owner, out List<bool> value)
			{
				value = owner.IsAlreadySentTo;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionChatItem_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionChatItem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionChatItem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionChatItem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionChatItem_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionChatItem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionChatItem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionChatItem_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionChatItem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionChatItem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionChatItem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionChatItem_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionChatItem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionChatItem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_FactionChatItem_003C_003EActor : IActivator, IActivator<MyObjectBuilder_FactionChatItem>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_FactionChatItem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_FactionChatItem CreateInstance()
			{
				return new MyObjectBuilder_FactionChatItem();
			}

			MyObjectBuilder_FactionChatItem IActivator<MyObjectBuilder_FactionChatItem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(37)]
		[XmlAttribute("t")]
		public string Text;

		[ProtoMember(40)]
		[XmlElement(ElementName = "I")]
		public long IdentityIdUniqueNumber;

		[ProtoMember(43)]
		[XmlElement(ElementName = "T")]
		public long TimestampMs;

		[ProtoMember(46)]
		[DefaultValue(null)]
		[XmlElement(ElementName = "PTST")]
		public List<long> PlayersToSendToUniqueNumber;

		[ProtoMember(49)]
		[DefaultValue(null)]
		[XmlElement(ElementName = "IAST")]
		public List<bool> IsAlreadySentTo;
	}
}
