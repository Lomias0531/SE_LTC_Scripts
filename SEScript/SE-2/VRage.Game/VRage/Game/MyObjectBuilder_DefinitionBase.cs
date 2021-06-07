using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public abstract class MyObjectBuilder_DefinitionBase : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DefinitionBase owner, in SerializableDefinitionId value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DefinitionBase owner, out SerializableDefinitionId value)
			{
				value = owner.Id;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DefinitionBase owner, in string value)
			{
				owner.DisplayName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DefinitionBase owner, out string value)
			{
				value = owner.DisplayName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DefinitionBase owner, in string value)
			{
				owner.Description = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DefinitionBase owner, out string value)
			{
				value = owner.Description;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DefinitionBase owner, in string[] value)
			{
				owner.Icons = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DefinitionBase owner, out string[] value)
			{
				value = owner.Icons;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DefinitionBase owner, in bool value)
			{
				owner.Public = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DefinitionBase owner, out bool value)
			{
				value = owner.Public;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DefinitionBase owner, in bool value)
			{
				owner.Enabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DefinitionBase owner, out bool value)
			{
				value = owner.Enabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DefinitionBase owner, in bool value)
			{
				owner.AvailableInSurvival = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DefinitionBase owner, out bool value)
			{
				value = owner.AvailableInSurvival;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DefinitionBase owner, in string value)
			{
				owner.DescriptionArgs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DefinitionBase owner, out string value)
			{
				value = owner.DescriptionArgs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DefinitionBase owner, in string[] value)
			{
				owner.DLCs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DefinitionBase owner, out string[] value)
			{
				value = owner.DLCs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DefinitionBase_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DefinitionBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DefinitionBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DefinitionBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DefinitionBase_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DefinitionBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DefinitionBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DefinitionBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DefinitionBase_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DefinitionBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DefinitionBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DefinitionBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DefinitionBase_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DefinitionBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DefinitionBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DefinitionBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		[ProtoMember(1)]
		public SerializableDefinitionId Id;

		[ProtoMember(4)]
		[DefaultValue("")]
		public string DisplayName;

		[ProtoMember(7)]
		[DefaultValue("")]
		public string Description;

		[ProtoMember(10)]
		[DefaultValue(new string[]
		{
			""
		})]
		[XmlElement("Icon")]
		[ModdableContentFile(new string[]
		{
			"dds",
			"png"
		})]
		public string[] Icons;

		[ProtoMember(13)]
		[DefaultValue(true)]
		public bool Public = true;

		[ProtoMember(16)]
		[DefaultValue(true)]
		[XmlAttribute(AttributeName = "Enabled")]
		public bool Enabled = true;

		[ProtoMember(19)]
		[DefaultValue(true)]
		public bool AvailableInSurvival = true;

		[ProtoMember(22)]
		[DefaultValue("")]
		public string DescriptionArgs;

		[ProtoMember(25)]
		[DefaultValue(null)]
		[XmlElement("DLC")]
		public string[] DLCs;
	}
}
