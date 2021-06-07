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
	public class MyObjectBuilder_AnimationDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EAnimationModel_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value)
			{
				owner.AnimationModel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value)
			{
				value = owner.AnimationModel;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EAnimationModelFPS_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value)
			{
				owner.AnimationModelFPS = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value)
			{
				value = owner.AnimationModelFPS;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EClipIndex_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in int value)
			{
				owner.ClipIndex = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out int value)
			{
				value = owner.ClipIndex;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EInfluenceArea_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value)
			{
				owner.InfluenceArea = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value)
			{
				value = owner.InfluenceArea;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EAllowInCockpit_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in bool value)
			{
				owner.AllowInCockpit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out bool value)
			{
				value = owner.AllowInCockpit;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EAllowWithWeapon_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in bool value)
			{
				owner.AllowWithWeapon = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out bool value)
			{
				value = owner.AllowWithWeapon;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003ESupportedSkeletons_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value)
			{
				owner.SupportedSkeletons = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value)
			{
				value = owner.SupportedSkeletons;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003ELoop_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in bool value)
			{
				owner.Loop = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out bool value)
			{
				value = owner.Loop;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003ELeftHandItem_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in SerializableDefinitionId value)
			{
				owner.LeftHandItem = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out SerializableDefinitionId value)
			{
				value = owner.LeftHandItem;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EAnimationSets_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, AnimationSet[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in AnimationSet[] value)
			{
				owner.AnimationSets = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out AnimationSet[] value)
			{
				value = owner.AnimationSets;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EChatCommand_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value)
			{
				owner.ChatCommand = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value)
			{
				value = owner.ChatCommand;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EChatCommandName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value)
			{
				owner.ChatCommandName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value)
			{
				value = owner.ChatCommandName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EChatCommandDescription_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value)
			{
				owner.ChatCommandDescription = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value)
			{
				value = owner.ChatCommandDescription;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EPriority_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in int value)
			{
				owner.Priority = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out int value)
			{
				value = owner.Priority;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_AnimationDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationDefinition CreateInstance()
			{
				return new MyObjectBuilder_AnimationDefinition();
			}

			MyObjectBuilder_AnimationDefinition IActivator<MyObjectBuilder_AnimationDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(16)]
		[ModdableContentFile("mwm")]
		public string AnimationModel;

		[ProtoMember(19)]
		[ModdableContentFile("mwm")]
		public string AnimationModelFPS;

		[ProtoMember(22)]
		public int ClipIndex;

		[ProtoMember(25)]
		public string InfluenceArea;

		[ProtoMember(28)]
		public bool AllowInCockpit = true;

		[ProtoMember(31)]
		public bool AllowWithWeapon;

		[ProtoMember(34)]
		public string SupportedSkeletons = "Humanoid";

		[ProtoMember(37)]
		public bool Loop;

		[ProtoMember(40)]
		public SerializableDefinitionId LeftHandItem;

		[ProtoMember(43)]
		[DefaultValue(null)]
		[XmlArrayItem("AnimationSet")]
		public AnimationSet[] AnimationSets;

		[ProtoMember(50)]
		public string ChatCommand;

		[ProtoMember(51)]
		public string ChatCommandName;

		[ProtoMember(52)]
		public string ChatCommandDescription;

		[ProtoMember(53)]
		public int Priority;
	}
}
