using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data.Audio;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[XmlType("Sound")]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public sealed class MyObjectBuilder_AudioDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003ESoundData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, MySoundData>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in MySoundData value)
			{
				owner.SoundData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out MySoundData value)
			{
				value = owner.SoundData;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003ECategory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in string value)
			{
				owner.Category = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out string value)
			{
				value = owner.Category;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EVolumeCurve_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, MyCurveType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in MyCurveType value)
			{
				owner.VolumeCurve = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out MyCurveType value)
			{
				value = owner.VolumeCurve;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EMaxDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in float value)
			{
				owner.MaxDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out float value)
			{
				value = owner.MaxDistance;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EUpdateDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in float value)
			{
				owner.UpdateDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out float value)
			{
				value = owner.UpdateDistance;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EVolume_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in float value)
			{
				owner.Volume = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out float value)
			{
				value = owner.Volume;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EVolumeVariation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in float value)
			{
				owner.VolumeVariation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out float value)
			{
				value = owner.VolumeVariation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EPitchVariation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in float value)
			{
				owner.PitchVariation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out float value)
			{
				value = owner.PitchVariation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EPitch_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in float value)
			{
				owner.Pitch = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out float value)
			{
				value = owner.Pitch;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EPreventSynchronization_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in int value)
			{
				owner.PreventSynchronization = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out int value)
			{
				value = owner.PreventSynchronization;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EDynamicMusicCategory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in string value)
			{
				owner.DynamicMusicCategory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out string value)
			{
				value = owner.DynamicMusicCategory;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EDynamicMusicAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in int value)
			{
				owner.DynamicMusicAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out int value)
			{
				value = owner.DynamicMusicAmount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EModifiableByHelmetFilters_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value)
			{
				owner.ModifiableByHelmetFilters = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value)
			{
				value = owner.ModifiableByHelmetFilters;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EAlwaysUseOneMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value)
			{
				owner.AlwaysUseOneMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value)
			{
				value = owner.AlwaysUseOneMode;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003ECanBeSilencedByVoid_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value)
			{
				owner.CanBeSilencedByVoid = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value)
			{
				value = owner.CanBeSilencedByVoid;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EStreamSound_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value)
			{
				owner.StreamSound = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value)
			{
				value = owner.StreamSound;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EDisablePitchEffects_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value)
			{
				owner.DisablePitchEffects = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value)
			{
				value = owner.DisablePitchEffects;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003ESoundLimit_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in int value)
			{
				owner.SoundLimit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out int value)
			{
				value = owner.SoundLimit;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003ELoopable_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value)
			{
				owner.Loopable = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value)
			{
				value = owner.Loopable;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EAlternative2D_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in string value)
			{
				owner.Alternative2D = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out string value)
			{
				value = owner.Alternative2D;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EUseOcclusion_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value)
			{
				owner.UseOcclusion = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value)
			{
				value = owner.UseOcclusion;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EWaves_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, List<MyAudioWave>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in List<MyAudioWave> value)
			{
				owner.Waves = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out List<MyAudioWave> value)
			{
				value = owner.Waves;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EDistantSounds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, List<DistantSound>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in List<DistantSound> value)
			{
				owner.DistantSounds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out List<DistantSound> value)
			{
				value = owner.DistantSounds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003ETransitionCategory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in string value)
			{
				owner.TransitionCategory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out string value)
			{
				value = owner.TransitionCategory;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EMusicCategory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in string value)
			{
				owner.MusicCategory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out string value)
			{
				value = owner.MusicCategory;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003ERealisticFilter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in string value)
			{
				owner.RealisticFilter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out string value)
			{
				value = owner.RealisticFilter;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003ERealisticVolumeChange_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in float value)
			{
				owner.RealisticVolumeChange = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out float value)
			{
				value = owner.RealisticVolumeChange;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AudioDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AudioDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_AudioDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AudioDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AudioDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AudioDefinition CreateInstance()
			{
				return new MyObjectBuilder_AudioDefinition();
			}

			MyObjectBuilder_AudioDefinition IActivator<MyObjectBuilder_AudioDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlIgnore]
		public MySoundData SoundData = new MySoundData();

		[ProtoMember(1)]
		public string Category
		{
			get
			{
				return SoundData.Category.ToString();
			}
			set
			{
				SoundData.Category = MyStringId.GetOrCompute(value);
			}
		}

		[ProtoMember(4)]
		[DefaultValue(MyCurveType.Custom_1)]
		public MyCurveType VolumeCurve
		{
			get
			{
				return SoundData.VolumeCurve;
			}
			set
			{
				SoundData.VolumeCurve = value;
			}
		}

		[ProtoMember(7)]
		public float MaxDistance
		{
			get
			{
				return SoundData.MaxDistance;
			}
			set
			{
				SoundData.MaxDistance = value;
			}
		}

		[ProtoMember(10)]
		public float UpdateDistance
		{
			get
			{
				return SoundData.UpdateDistance;
			}
			set
			{
				SoundData.UpdateDistance = value;
			}
		}

		[ProtoMember(13)]
		[DefaultValue(1f)]
		public float Volume
		{
			get
			{
				return SoundData.Volume;
			}
			set
			{
				SoundData.Volume = value;
			}
		}

		[ProtoMember(16)]
		[DefaultValue(0f)]
		public float VolumeVariation
		{
			get
			{
				return SoundData.VolumeVariation;
			}
			set
			{
				SoundData.VolumeVariation = value;
			}
		}

		[ProtoMember(19)]
		[DefaultValue(0f)]
		public float PitchVariation
		{
			get
			{
				return SoundData.PitchVariation;
			}
			set
			{
				SoundData.PitchVariation = value;
			}
		}

		[ProtoMember(22)]
		[DefaultValue(0f)]
		public float Pitch
		{
			get
			{
				return SoundData.Pitch;
			}
			set
			{
				SoundData.Pitch = value;
			}
		}

		[ProtoMember(25)]
		[DefaultValue(-1)]
		public int PreventSynchronization
		{
			get
			{
				return SoundData.PreventSynchronization;
			}
			set
			{
				SoundData.PreventSynchronization = value;
			}
		}

		[ProtoMember(28)]
		public string DynamicMusicCategory
		{
			get
			{
				return SoundData.DynamicMusicCategory.ToString();
			}
			set
			{
				SoundData.DynamicMusicCategory = MyStringId.GetOrCompute(value);
			}
		}

		[ProtoMember(31)]
		public int DynamicMusicAmount
		{
			get
			{
				return SoundData.DynamicMusicAmount;
			}
			set
			{
				SoundData.DynamicMusicAmount = value;
			}
		}

		[ProtoMember(34)]
		[DefaultValue(true)]
		public bool ModifiableByHelmetFilters
		{
			get
			{
				return SoundData.ModifiableByHelmetFilters;
			}
			set
			{
				SoundData.ModifiableByHelmetFilters = value;
			}
		}

		[ProtoMember(37)]
		[DefaultValue(false)]
		public bool AlwaysUseOneMode
		{
			get
			{
				return SoundData.AlwaysUseOneMode;
			}
			set
			{
				SoundData.AlwaysUseOneMode = value;
			}
		}

		[ProtoMember(40)]
		[DefaultValue(true)]
		public bool CanBeSilencedByVoid
		{
			get
			{
				return SoundData.CanBeSilencedByVoid;
			}
			set
			{
				SoundData.CanBeSilencedByVoid = value;
			}
		}

		[ProtoMember(43)]
		[DefaultValue(false)]
		public bool StreamSound
		{
			get
			{
				return SoundData.StreamSound;
			}
			set
			{
				SoundData.StreamSound = value;
			}
		}

		[ProtoMember(46)]
		[DefaultValue(false)]
		public bool DisablePitchEffects
		{
			get
			{
				return SoundData.DisablePitchEffects;
			}
			set
			{
				SoundData.DisablePitchEffects = value;
			}
		}

		[ProtoMember(49)]
		[DefaultValue(0)]
		public int SoundLimit
		{
			get
			{
				return SoundData.SoundLimit;
			}
			set
			{
				SoundData.SoundLimit = value;
			}
		}

		[ProtoMember(52)]
		[DefaultValue(false)]
		public bool Loopable
		{
			get
			{
				return SoundData.Loopable;
			}
			set
			{
				SoundData.Loopable = value;
			}
		}

		[ProtoMember(55)]
		public string Alternative2D
		{
			get
			{
				return SoundData.Alternative2D;
			}
			set
			{
				SoundData.Alternative2D = value;
			}
		}

		[ProtoMember(58)]
		[DefaultValue(false)]
		public bool UseOcclusion
		{
			get
			{
				return SoundData.UseOcclusion;
			}
			set
			{
				SoundData.UseOcclusion = value;
			}
		}

		[ProtoMember(61)]
		public List<MyAudioWave> Waves
		{
			get
			{
				return SoundData.Waves;
			}
			set
			{
				SoundData.Waves = value;
			}
		}

		[ProtoMember(64)]
		public List<DistantSound> DistantSounds
		{
			get
			{
				return SoundData.DistantSounds;
			}
			set
			{
				SoundData.DistantSounds = value;
			}
		}

		[ProtoMember(67)]
		[DefaultValue("")]
		public string TransitionCategory
		{
			get
			{
				return SoundData.MusicTrack.TransitionCategory.ToString();
			}
			set
			{
				SoundData.MusicTrack.TransitionCategory = MyStringId.GetOrCompute(value);
			}
		}

		[ProtoMember(70)]
		[DefaultValue("")]
		public string MusicCategory
		{
			get
			{
				return SoundData.MusicTrack.MusicCategory.ToString();
			}
			set
			{
				SoundData.MusicTrack.MusicCategory = MyStringId.GetOrCompute(value);
			}
		}

		[ProtoMember(73)]
		[DefaultValue("")]
		public string RealisticFilter
		{
			get
			{
				return SoundData.RealisticFilter.String;
			}
			set
			{
				SoundData.RealisticFilter = MyStringHash.GetOrCompute(value);
			}
		}

		[ProtoMember(76)]
		[DefaultValue(1f)]
		public float RealisticVolumeChange
		{
			get
			{
				return SoundData.RealisticVolumeChange;
			}
			set
			{
				SoundData.RealisticVolumeChange = value;
			}
		}
	}
}
