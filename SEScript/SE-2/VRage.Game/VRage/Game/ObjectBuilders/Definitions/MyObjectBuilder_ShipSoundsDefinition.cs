using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	public class MyObjectBuilder_ShipSoundsDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EMinWeight_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.MinWeight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.MinWeight;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EAllowSmallGrid_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in bool value)
			{
				owner.AllowSmallGrid = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out bool value)
			{
				value = owner.AllowSmallGrid;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EAllowLargeGrid_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in bool value)
			{
				owner.AllowLargeGrid = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out bool value)
			{
				value = owner.AllowLargeGrid;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EEnginePitchRangeInSemitones_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.EnginePitchRangeInSemitones = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.EnginePitchRangeInSemitones;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EEngineTimeToTurnOn_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.EngineTimeToTurnOn = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.EngineTimeToTurnOn;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EEngineTimeToTurnOff_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.EngineTimeToTurnOff = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.EngineTimeToTurnOff;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003ESpeedUpSoundChangeVolumeTo_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.SpeedUpSoundChangeVolumeTo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.SpeedUpSoundChangeVolumeTo;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003ESpeedDownSoundChangeVolumeTo_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.SpeedDownSoundChangeVolumeTo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.SpeedDownSoundChangeVolumeTo;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003ESpeedUpDownChangeSpeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.SpeedUpDownChangeSpeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.SpeedUpDownChangeSpeed;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EWheelsPitchRangeInSemitones_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.WheelsPitchRangeInSemitones = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.WheelsPitchRangeInSemitones;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EWheelsLowerThrusterVolumeBy_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.WheelsLowerThrusterVolumeBy = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.WheelsLowerThrusterVolumeBy;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EWheelsFullSpeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.WheelsFullSpeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.WheelsFullSpeed;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EWheelsGroundMinVolume_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.WheelsGroundMinVolume = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.WheelsGroundMinVolume;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EThrusterPitchRangeInSemitones_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.ThrusterPitchRangeInSemitones = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.ThrusterPitchRangeInSemitones;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EThrusterCompositionMinVolume_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.ThrusterCompositionMinVolume = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.ThrusterCompositionMinVolume;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EThrusterCompositionChangeSpeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value)
			{
				owner.ThrusterCompositionChangeSpeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value)
			{
				value = owner.ThrusterCompositionChangeSpeed;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EWheelsVolumes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, List<ShipSoundVolumePair>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in List<ShipSoundVolumePair> value)
			{
				owner.WheelsVolumes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out List<ShipSoundVolumePair> value)
			{
				value = owner.WheelsVolumes;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EThrusterVolumes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, List<ShipSoundVolumePair>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in List<ShipSoundVolumePair> value)
			{
				owner.ThrusterVolumes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out List<ShipSoundVolumePair> value)
			{
				value = owner.ThrusterVolumes;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EEngineVolumes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, List<ShipSoundVolumePair>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in List<ShipSoundVolumePair> value)
			{
				owner.EngineVolumes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out List<ShipSoundVolumePair> value)
			{
				value = owner.EngineVolumes;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003ESounds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, List<ShipSound>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in List<ShipSound> value)
			{
				owner.Sounds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out List<ShipSound> value)
			{
				value = owner.Sounds;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ShipSoundsDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ShipSoundsDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ShipSoundsDefinition CreateInstance()
			{
				return new MyObjectBuilder_ShipSoundsDefinition();
			}

			MyObjectBuilder_ShipSoundsDefinition IActivator<MyObjectBuilder_ShipSoundsDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(16)]
		public float MinWeight = 3000f;

		[ProtoMember(19)]
		public bool AllowSmallGrid = true;

		[ProtoMember(22)]
		public bool AllowLargeGrid = true;

		[ProtoMember(25)]
		public float EnginePitchRangeInSemitones = 4f;

		[ProtoMember(28)]
		public float EngineTimeToTurnOn = 4f;

		[ProtoMember(31)]
		public float EngineTimeToTurnOff = 3f;

		[ProtoMember(34)]
		public float SpeedUpSoundChangeVolumeTo = 1f;

		[ProtoMember(37)]
		public float SpeedDownSoundChangeVolumeTo = 1f;

		[ProtoMember(40)]
		public float SpeedUpDownChangeSpeed = 0.2f;

		[ProtoMember(43)]
		public float WheelsPitchRangeInSemitones = 4f;

		[ProtoMember(46)]
		public float WheelsLowerThrusterVolumeBy = 0.33f;

		[ProtoMember(49)]
		public float WheelsFullSpeed = 32f;

		[ProtoMember(52)]
		public float WheelsGroundMinVolume = 0.5f;

		[ProtoMember(55)]
		public float ThrusterPitchRangeInSemitones = 4f;

		[ProtoMember(58)]
		public float ThrusterCompositionMinVolume = 0.4f;

		[ProtoMember(61)]
		public float ThrusterCompositionChangeSpeed = 0.025f;

		[ProtoMember(64)]
		[XmlArrayItem("WheelsVolume")]
		public List<ShipSoundVolumePair> WheelsVolumes;

		[ProtoMember(67)]
		[XmlArrayItem("ThrusterVolume")]
		public List<ShipSoundVolumePair> ThrusterVolumes;

		[ProtoMember(70)]
		[XmlArrayItem("EngineVolume")]
		public List<ShipSoundVolumePair> EngineVolumes;

		[ProtoMember(73)]
		[XmlArrayItem("Sound")]
		public List<ShipSound> Sounds;
	}
}
