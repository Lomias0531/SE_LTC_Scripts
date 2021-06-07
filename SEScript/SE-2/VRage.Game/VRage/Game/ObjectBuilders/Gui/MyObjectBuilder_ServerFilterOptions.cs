using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Gui
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ServerFilterOptions : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003EAllowedGroups_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value)
			{
				owner.AllowedGroups = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value)
			{
				value = owner.AllowedGroups;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003ESameVersion_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value)
			{
				owner.SameVersion = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value)
			{
				value = owner.SameVersion;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003ESameData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value)
			{
				owner.SameData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value)
			{
				value = owner.SameData;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003EHasPassword_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool? value)
			{
				owner.HasPassword = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool? value)
			{
				value = owner.HasPassword;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003ECreativeMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value)
			{
				owner.CreativeMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value)
			{
				value = owner.CreativeMode;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003ESurvivalMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value)
			{
				owner.SurvivalMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value)
			{
				value = owner.SurvivalMode;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003ECheckPlayer_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value)
			{
				owner.CheckPlayer = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value)
			{
				value = owner.CheckPlayer;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003EPlayerCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in SerializableRange value)
			{
				owner.PlayerCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out SerializableRange value)
			{
				value = owner.PlayerCount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003ECheckMod_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value)
			{
				owner.CheckMod = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value)
			{
				value = owner.CheckMod;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003EModCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in SerializableRange value)
			{
				owner.ModCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out SerializableRange value)
			{
				value = owner.ModCount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003ECheckDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value)
			{
				owner.CheckDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value)
			{
				value = owner.CheckDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003EViewDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in SerializableRange value)
			{
				owner.ViewDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out SerializableRange value)
			{
				value = owner.ViewDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003EAdvanced_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value)
			{
				owner.Advanced = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value)
			{
				value = owner.Advanced;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003EPing_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in int value)
			{
				owner.Ping = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out int value)
			{
				value = owner.Ping;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003EModsExclusive_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value)
			{
				owner.ModsExclusive = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value)
			{
				value = owner.ModsExclusive;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003EMods_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, List<ulong>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in List<ulong> value)
			{
				owner.Mods = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out List<ulong> value)
			{
				value = owner.Mods;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003EFilters_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, SerializableDictionary<byte, string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in SerializableDictionary<byte, string> value)
			{
				owner.Filters = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out SerializableDictionary<byte, string> value)
			{
				value = owner.Filters;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ServerFilterOptions, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ServerFilterOptions, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ServerFilterOptions, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ServerFilterOptions, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ServerFilterOptions owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ServerFilterOptions owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ServerFilterOptions>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ServerFilterOptions();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ServerFilterOptions CreateInstance()
			{
				return new MyObjectBuilder_ServerFilterOptions();
			}

			MyObjectBuilder_ServerFilterOptions IActivator<MyObjectBuilder_ServerFilterOptions>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public bool AllowedGroups;

		[ProtoMember(4)]
		public bool SameVersion;

		[ProtoMember(7)]
		public bool SameData;

		[ProtoMember(10)]
		public bool? HasPassword;

		[ProtoMember(13)]
		public bool CreativeMode;

		[ProtoMember(16)]
		public bool SurvivalMode;

		[ProtoMember(19)]
		public bool CheckPlayer;

		[ProtoMember(22)]
		public SerializableRange PlayerCount;

		[ProtoMember(25)]
		public bool CheckMod;

		[ProtoMember(28)]
		public SerializableRange ModCount;

		[ProtoMember(31)]
		public bool CheckDistance;

		[ProtoMember(34)]
		public SerializableRange ViewDistance;

		[ProtoMember(37)]
		public bool Advanced;

		[ProtoMember(40)]
		public int Ping;

		[ProtoMember(43)]
		public bool ModsExclusive;

		[ProtoMember(46)]
		public List<ulong> Mods;

		[ProtoMember(49)]
		public SerializableDictionary<byte, string> Filters;
	}
}
