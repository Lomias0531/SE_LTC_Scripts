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
	public class MyObjectBuilder_GlobalChatItem : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_GlobalChatItem_003C_003EText_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GlobalChatItem owner, in string value)
			{
				owner.Text = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GlobalChatItem owner, out string value)
			{
				value = owner.Text;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalChatItem_003C_003EIdentityIdUniqueNumber_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalChatItem, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GlobalChatItem owner, in long value)
			{
				owner.IdentityIdUniqueNumber = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GlobalChatItem owner, out long value)
			{
				value = owner.IdentityIdUniqueNumber;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalChatItem_003C_003EAuthor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GlobalChatItem owner, in string value)
			{
				owner.Author = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GlobalChatItem owner, out string value)
			{
				value = owner.Author;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalChatItem_003C_003EFont_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GlobalChatItem owner, in string value)
			{
				owner.Font = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GlobalChatItem owner, out string value)
			{
				value = owner.Font;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalChatItem_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalChatItem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GlobalChatItem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GlobalChatItem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalChatItem_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GlobalChatItem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GlobalChatItem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalChatItem_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalChatItem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GlobalChatItem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GlobalChatItem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalChatItem_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GlobalChatItem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GlobalChatItem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_GlobalChatItem_003C_003EActor : IActivator, IActivator<MyObjectBuilder_GlobalChatItem>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_GlobalChatItem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_GlobalChatItem CreateInstance()
			{
				return new MyObjectBuilder_GlobalChatItem();
			}

			MyObjectBuilder_GlobalChatItem IActivator<MyObjectBuilder_GlobalChatItem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(52)]
		[XmlAttribute("t")]
		public string Text;

		[ProtoMember(55)]
		[XmlElement(ElementName = "I")]
		public long IdentityIdUniqueNumber;

		[ProtoMember(58)]
		[XmlAttribute("a")]
		[DefaultValue("")]
		public string Author;

		[ProtoMember(61)]
		[XmlAttribute("f")]
		[DefaultValue("Blue")]
		public string Font;
	}
}
