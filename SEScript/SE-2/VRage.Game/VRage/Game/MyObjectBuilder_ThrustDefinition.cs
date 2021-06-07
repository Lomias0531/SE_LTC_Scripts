using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ThrustDefinition : MyObjectBuilder_CubeBlockDefinition
	{
		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EResourceSinkGroup_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				owner.ResourceSinkGroup = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				value = owner.ResourceSinkGroup;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EFuelConverter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, MyFuelConverterInfo>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MyFuelConverterInfo value)
			{
				owner.FuelConverter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MyFuelConverterInfo value)
			{
				value = owner.FuelConverter;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ESlowdownFactor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.SlowdownFactor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.SlowdownFactor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EThrusterType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				owner.ThrusterType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				value = owner.ThrusterType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EForceMagnitude_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.ForceMagnitude = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.ForceMagnitude;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EMaxPowerConsumption_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.MaxPowerConsumption = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.MaxPowerConsumption;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EMinPowerConsumption_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.MinPowerConsumption = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.MinPowerConsumption;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EFlameDamageLengthScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.FlameDamageLengthScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.FlameDamageLengthScale;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EFlameLengthScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.FlameLengthScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.FlameLengthScale;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EFlameFullColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in Vector4 value)
			{
				owner.FlameFullColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out Vector4 value)
			{
				value = owner.FlameFullColor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EFlameIdleColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in Vector4 value)
			{
				owner.FlameIdleColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out Vector4 value)
			{
				value = owner.FlameIdleColor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EFlamePointMaterial_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				owner.FlamePointMaterial = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				value = owner.FlamePointMaterial;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EFlameLengthMaterial_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				owner.FlameLengthMaterial = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				value = owner.FlameLengthMaterial;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EFlameFlare_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				owner.FlameFlare = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				value = owner.FlameFlare;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EFlameVisibilityDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.FlameVisibilityDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.FlameVisibilityDistance;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EFlameGlareQuerySize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.FlameGlareQuerySize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.FlameGlareQuerySize;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EFlameDamage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.FlameDamage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.FlameDamage;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EMinPlanetaryInfluence_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.MinPlanetaryInfluence = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.MinPlanetaryInfluence;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EMaxPlanetaryInfluence_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.MaxPlanetaryInfluence = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.MaxPlanetaryInfluence;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EEffectivenessAtMaxInfluence_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.EffectivenessAtMaxInfluence = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.EffectivenessAtMaxInfluence;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EEffectivenessAtMinInfluence_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.EffectivenessAtMinInfluence = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.EffectivenessAtMinInfluence;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ENeedsAtmosphereForInfluence_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				owner.NeedsAtmosphereForInfluence = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				value = owner.NeedsAtmosphereForInfluence;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EConsumptionFactorPerG_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.ConsumptionFactorPerG = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.ConsumptionFactorPerG;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPropellerUsesPropellerSystem_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				owner.PropellerUsesPropellerSystem = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				value = owner.PropellerUsesPropellerSystem;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPropellerSubpartEntityName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				owner.PropellerSubpartEntityName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				value = owner.PropellerSubpartEntityName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPropellerRoundsPerSecondOnFullSpeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.PropellerRoundsPerSecondOnFullSpeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.PropellerRoundsPerSecondOnFullSpeed;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPropellerRoundsPerSecondOnIdleSpeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.PropellerRoundsPerSecondOnIdleSpeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.PropellerRoundsPerSecondOnIdleSpeed;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPropellerAccelerationTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.PropellerAccelerationTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.PropellerAccelerationTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPropellerDecelerationTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.PropellerDecelerationTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.PropellerDecelerationTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPropellerMaxVisibleDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				owner.PropellerMaxVisibleDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				value = owner.PropellerMaxVisibleDistance;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EVoxelPlacement_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVoxelPlacement_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, VoxelPlacementOverride?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in VoxelPlacementOverride? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out VoxelPlacementOverride? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ESilenceableByShipSoundSystem_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESilenceableByShipSoundSystem_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ECubeSize_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeSize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, MyCubeSize>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MyCubeSize value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MyCubeSize value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EBlockTopology_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockTopology_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, MyBlockTopology>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MyBlockTopology value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MyBlockTopology value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in SerializableVector3I value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out SerializableVector3I value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EModelOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EModelOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in SerializableVector3 value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out SerializableVector3 value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ECubeDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, PatternDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in PatternDefinition value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out PatternDefinition value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EComponents_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EComponents_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, CubeBlockComponent[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in CubeBlockComponent[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out CubeBlockComponent[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EEffects_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEffects_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, CubeBlockEffectBase[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in CubeBlockEffectBase[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out CubeBlockEffectBase[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ECriticalComponent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECriticalComponent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, CriticalPart>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in CriticalPart value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out CriticalPart value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EMountPoints_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoints_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, MountPoint[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MountPoint[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MountPoint[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EVariants_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVariants_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, Variant[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in Variant[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out Variant[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EEntityComponents_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEntityComponents_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, EntityComponentDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in EntityComponentDefinition[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out EntityComponentDefinition[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPhysicsOption_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPhysicsOption_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, MyPhysicsOption>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MyPhysicsOption value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MyPhysicsOption value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EBuildProgressModels_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressModels_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, List<BuildProgressModel>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in List<BuildProgressModel> value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out List<BuildProgressModel> value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EBlockPairName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockPairName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ECenter_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECenter_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, SerializableVector3I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in SerializableVector3I? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out SerializableVector3I? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EMirroringX_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringX_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EMirroringY_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringY_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EMirroringZ_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringZ_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EDeformationRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDeformationRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EEdgeType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEdgeType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EBuildTimeSeconds_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildTimeSeconds_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EDisassembleRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDisassembleRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EAutorotateMode_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EAutorotateMode_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, MyAutorotateMode>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MyAutorotateMode value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MyAutorotateMode value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EMirroringBlock_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EUseModelIntersection_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUseModelIntersection_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPrimarySound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPrimarySound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EActionSound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EActionSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EBuildType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EBuildMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ECompoundTemplates_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECompoundTemplates_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ECompoundEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECompoundEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ESubBlockDefinitions_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESubBlockDefinitions_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, MySubBlockDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MySubBlockDefinition[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MySubBlockDefinition[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EMultiBlock_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMultiBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ENavigationDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ENavigationDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EGuiVisible_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGuiVisible_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EBlockVariants_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockVariants_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in SerializableDefinitionId[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out SerializableDefinitionId[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EDirection_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDirection_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, MyBlockDirection>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MyBlockDirection value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MyBlockDirection value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ERotation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ERotation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, MyBlockRotation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MyBlockRotation value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MyBlockRotation value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EGeneratedBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneratedBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in SerializableDefinitionId[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out SerializableDefinitionId[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EGeneratedBlockType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneratedBlockType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EMirrored_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirrored_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EDamageEffectId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EDestroyEffect_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroyEffect_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EDestroySound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroySound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ESkeleton_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESkeleton_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, List<BoneInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in List<BoneInfo> value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out List<BoneInfo> value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ERandomRotation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ERandomRotation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EIsAirTight_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EIsAirTight_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EIsStandAlone_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EIsStandAlone_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EHasPhysics_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EHasPhysics_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EUseNeighbourOxygenRooms_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUseNeighbourOxygenRooms_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPoints_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPoints_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EMaxIntegrity_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMaxIntegrity_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EBuildProgressToPlaceGeneratedBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressToPlaceGeneratedBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EDamagedSound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamagedSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ECreateFracturedPieces_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECreateFracturedPieces_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EEmissiveColorPreset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEmissiveColorPreset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EGeneralDamageMultiplier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneralDamageMultiplier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EDamageEffectName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EUsesDeformation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUsesDeformation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EDestroyEffectOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroyEffectOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, Vector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in Vector3? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out Vector3? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPCU_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPCU_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPlaceDecals_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPlaceDecals_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EPhysicalMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EMass_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EMass_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ThrustDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ThrustDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ThrustDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ThrustDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ThrustDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ThrustDefinition CreateInstance()
			{
				return new MyObjectBuilder_ThrustDefinition();
			}

			MyObjectBuilder_ThrustDefinition IActivator<MyObjectBuilder_ThrustDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly Vector4 DefaultThrustColor = new Vector4(Color.CornflowerBlue.ToVector3() * 0.7f, 0.75f);

		[ProtoMember(7)]
		public string ResourceSinkGroup;

		[ProtoMember(10)]
		public MyFuelConverterInfo FuelConverter = new MyFuelConverterInfo();

		[ProtoMember(13)]
		public float SlowdownFactor = 10f;

		[ProtoMember(16)]
		public string ThrusterType = "Ion";

		[ProtoMember(19)]
		public float ForceMagnitude;

		[ProtoMember(22)]
		public float MaxPowerConsumption;

		[ProtoMember(25)]
		public float MinPowerConsumption;

		[ProtoMember(28)]
		public float FlameDamageLengthScale = 0.6f;

		[ProtoMember(31)]
		public float FlameLengthScale = 1.15f;

		[ProtoMember(34)]
		public Vector4 FlameFullColor = DefaultThrustColor;

		[ProtoMember(37)]
		public Vector4 FlameIdleColor = DefaultThrustColor;

		[ProtoMember(40)]
		public string FlamePointMaterial = "EngineThrustMiddle";

		[ProtoMember(43)]
		public string FlameLengthMaterial = "EngineThrustMiddle";

		[ProtoMember(46)]
		public string FlameFlare = "SmallGridSmallThruster";

		[ProtoMember(49)]
		public float FlameVisibilityDistance = 200f;

		[ProtoMember(52)]
		public float FlameGlareQuerySize = 0.5f;

		[ProtoMember(55)]
		public float FlameDamage = 0.5f;

		[ProtoMember(58)]
		public float MinPlanetaryInfluence;

		[ProtoMember(61)]
		public float MaxPlanetaryInfluence = 1f;

		[ProtoMember(64)]
		public float EffectivenessAtMaxInfluence = 1f;

		[ProtoMember(67)]
		public float EffectivenessAtMinInfluence = 1f;

		[ProtoMember(70)]
		public bool NeedsAtmosphereForInfluence;

		[ProtoMember(73)]
		public float ConsumptionFactorPerG;

		[ProtoMember(76)]
		public bool PropellerUsesPropellerSystem;

		[ProtoMember(79)]
		public string PropellerSubpartEntityName = "Propeller";

		[ProtoMember(82)]
		public float PropellerRoundsPerSecondOnFullSpeed = 1.9f;

		[ProtoMember(85)]
		public float PropellerRoundsPerSecondOnIdleSpeed = 0.3f;

		[ProtoMember(88)]
		public float PropellerAccelerationTime = 3f;

		[ProtoMember(91)]
		public float PropellerDecelerationTime = 6f;

		[ProtoMember(94)]
		public float PropellerMaxVisibleDistance = 20f;
	}
}
