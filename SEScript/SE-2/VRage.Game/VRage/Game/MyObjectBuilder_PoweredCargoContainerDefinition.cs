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
	public class MyObjectBuilder_PoweredCargoContainerDefinition : MyObjectBuilder_CargoContainerDefinition
	{
		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EResourceSinkGroup_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				owner.ResourceSinkGroup = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				value = owner.ResourceSinkGroup;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ERequiredPowerInput_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in float value)
			{
				owner.RequiredPowerInput = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out float value)
			{
				value = owner.RequiredPowerInput;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EInventorySize_003C_003EAccessor : VRage_Game_MyObjectBuilder_CargoContainerDefinition_003C_003EInventorySize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_CargoContainerDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_CargoContainerDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EVoxelPlacement_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVoxelPlacement_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, VoxelPlacementOverride?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in VoxelPlacementOverride? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out VoxelPlacementOverride? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ESilenceableByShipSoundSystem_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESilenceableByShipSoundSystem_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ECubeSize_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeSize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, MyCubeSize>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in MyCubeSize value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out MyCubeSize value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EBlockTopology_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockTopology_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, MyBlockTopology>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in MyBlockTopology value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out MyBlockTopology value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in SerializableVector3I value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out SerializableVector3I value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EModelOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EModelOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in SerializableVector3 value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out SerializableVector3 value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ECubeDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, PatternDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in PatternDefinition value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out PatternDefinition value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EComponents_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EComponents_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, CubeBlockComponent[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in CubeBlockComponent[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out CubeBlockComponent[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EEffects_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEffects_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, CubeBlockEffectBase[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in CubeBlockEffectBase[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out CubeBlockEffectBase[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ECriticalComponent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECriticalComponent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, CriticalPart>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in CriticalPart value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out CriticalPart value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EMountPoints_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoints_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, MountPoint[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in MountPoint[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out MountPoint[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EVariants_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVariants_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, Variant[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in Variant[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out Variant[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EEntityComponents_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEntityComponents_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, EntityComponentDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in EntityComponentDefinition[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out EntityComponentDefinition[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EPhysicsOption_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPhysicsOption_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, MyPhysicsOption>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in MyPhysicsOption value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out MyPhysicsOption value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EBuildProgressModels_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressModels_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, List<BuildProgressModel>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in List<BuildProgressModel> value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out List<BuildProgressModel> value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EBlockPairName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockPairName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ECenter_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECenter_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, SerializableVector3I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in SerializableVector3I? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out SerializableVector3I? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EMirroringX_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringX_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EMirroringY_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringY_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EMirroringZ_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringZ_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EDeformationRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDeformationRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EEdgeType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEdgeType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EBuildTimeSeconds_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildTimeSeconds_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EDisassembleRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDisassembleRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EAutorotateMode_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EAutorotateMode_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, MyAutorotateMode>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in MyAutorotateMode value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out MyAutorotateMode value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EMirroringBlock_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EUseModelIntersection_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUseModelIntersection_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EPrimarySound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPrimarySound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EActionSound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EActionSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EBuildType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EBuildMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ECompoundTemplates_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECompoundTemplates_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ECompoundEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECompoundEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ESubBlockDefinitions_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESubBlockDefinitions_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, MySubBlockDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in MySubBlockDefinition[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out MySubBlockDefinition[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EMultiBlock_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMultiBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ENavigationDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ENavigationDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EGuiVisible_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGuiVisible_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EBlockVariants_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockVariants_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in SerializableDefinitionId[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out SerializableDefinitionId[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EDirection_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDirection_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, MyBlockDirection>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in MyBlockDirection value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out MyBlockDirection value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ERotation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ERotation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, MyBlockRotation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in MyBlockRotation value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out MyBlockRotation value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EGeneratedBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneratedBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in SerializableDefinitionId[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out SerializableDefinitionId[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EGeneratedBlockType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneratedBlockType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EMirrored_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirrored_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EDamageEffectId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EDestroyEffect_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroyEffect_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EDestroySound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroySound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ESkeleton_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESkeleton_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, List<BoneInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in List<BoneInfo> value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out List<BoneInfo> value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ERandomRotation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ERandomRotation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EIsAirTight_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EIsAirTight_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EIsStandAlone_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EIsStandAlone_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EHasPhysics_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EHasPhysics_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EUseNeighbourOxygenRooms_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUseNeighbourOxygenRooms_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EPoints_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPoints_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EMaxIntegrity_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMaxIntegrity_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EBuildProgressToPlaceGeneratedBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressToPlaceGeneratedBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EDamagedSound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamagedSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ECreateFracturedPieces_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECreateFracturedPieces_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EEmissiveColorPreset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEmissiveColorPreset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EGeneralDamageMultiplier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneralDamageMultiplier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EDamageEffectName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EUsesDeformation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUsesDeformation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EDestroyEffectOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroyEffectOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, Vector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in Vector3? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out Vector3? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EPCU_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPCU_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EPlaceDecals_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPlaceDecals_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EPhysicalMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EMass_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EMass_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PoweredCargoContainerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PoweredCargoContainerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_PoweredCargoContainerDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_PoweredCargoContainerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_PoweredCargoContainerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_PoweredCargoContainerDefinition CreateInstance()
			{
				return new MyObjectBuilder_PoweredCargoContainerDefinition();
			}

			MyObjectBuilder_PoweredCargoContainerDefinition IActivator<MyObjectBuilder_PoweredCargoContainerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string ResourceSinkGroup;

		[ProtoMember(4)]
		public float RequiredPowerInput;
	}
}
