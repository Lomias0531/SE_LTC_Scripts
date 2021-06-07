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
	public class MyObjectBuilder_FueledPowerProducerDefinition : MyObjectBuilder_PowerProducerDefinition
	{
		[ProtoContract]
		public class FuelInfo
		{
			protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EFuelInfo_003C_003ERatio_003C_003EAccessor : IMemberAccessor<FuelInfo, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref FuelInfo owner, in float value)
				{
					owner.Ratio = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref FuelInfo owner, out float value)
				{
					value = owner.Ratio;
				}
			}

			protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EFuelInfo_003C_003EId_003C_003EAccessor : IMemberAccessor<FuelInfo, SerializableDefinitionId>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref FuelInfo owner, in SerializableDefinitionId value)
				{
					owner.Id = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref FuelInfo owner, out SerializableDefinitionId value)
				{
					value = owner.Id;
				}
			}

			private class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EFuelInfo_003C_003EActor : IActivator, IActivator<FuelInfo>
			{
				private sealed override object CreateInstance()
				{
					return new FuelInfo();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override FuelInfo CreateInstance()
				{
					return new FuelInfo();
				}

				FuelInfo IActivator<FuelInfo>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(10)]
			public float Ratio = 1f;

			[ProtoMember(13)]
			public SerializableDefinitionId Id;
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EFuelProductionToCapacityMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in float value)
			{
				owner.FuelProductionToCapacityMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out float value)
			{
				value = owner.FuelProductionToCapacityMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EFuelInfos_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, List<FuelInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in List<FuelInfo> value)
			{
				owner.FuelInfos = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out List<FuelInfo> value)
			{
				value = owner.FuelInfos;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EResourceSinkGroup_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				owner.ResourceSinkGroup = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				value = owner.ResourceSinkGroup;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EResourceSourceGroup_003C_003EAccessor : VRage_Game_MyObjectBuilder_PowerProducerDefinition_003C_003EResourceSourceGroup_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PowerProducerDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PowerProducerDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EMaxPowerOutput_003C_003EAccessor : VRage_Game_MyObjectBuilder_PowerProducerDefinition_003C_003EMaxPowerOutput_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_PowerProducerDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_PowerProducerDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EVoxelPlacement_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVoxelPlacement_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, VoxelPlacementOverride?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in VoxelPlacementOverride? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out VoxelPlacementOverride? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ESilenceableByShipSoundSystem_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESilenceableByShipSoundSystem_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ECubeSize_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeSize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyCubeSize>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in MyCubeSize value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out MyCubeSize value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EBlockTopology_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockTopology_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyBlockTopology>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in MyBlockTopology value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out MyBlockTopology value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in SerializableVector3I value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out SerializableVector3I value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EModelOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EModelOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in SerializableVector3 value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out SerializableVector3 value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ECubeDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, PatternDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in PatternDefinition value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out PatternDefinition value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EComponents_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EComponents_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, CubeBlockComponent[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in CubeBlockComponent[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out CubeBlockComponent[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EEffects_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEffects_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, CubeBlockEffectBase[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in CubeBlockEffectBase[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out CubeBlockEffectBase[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ECriticalComponent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECriticalComponent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, CriticalPart>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in CriticalPart value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out CriticalPart value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EMountPoints_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoints_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MountPoint[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in MountPoint[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out MountPoint[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EVariants_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVariants_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, Variant[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in Variant[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out Variant[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EEntityComponents_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEntityComponents_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, EntityComponentDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in EntityComponentDefinition[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out EntityComponentDefinition[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EPhysicsOption_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPhysicsOption_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyPhysicsOption>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in MyPhysicsOption value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out MyPhysicsOption value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EBuildProgressModels_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressModels_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, List<BuildProgressModel>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in List<BuildProgressModel> value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out List<BuildProgressModel> value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EBlockPairName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockPairName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ECenter_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECenter_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, SerializableVector3I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in SerializableVector3I? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out SerializableVector3I? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EMirroringX_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringX_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EMirroringY_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringY_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EMirroringZ_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringZ_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EDeformationRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDeformationRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EEdgeType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEdgeType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EBuildTimeSeconds_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildTimeSeconds_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EDisassembleRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDisassembleRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EAutorotateMode_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EAutorotateMode_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyAutorotateMode>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in MyAutorotateMode value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out MyAutorotateMode value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EMirroringBlock_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EUseModelIntersection_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUseModelIntersection_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EPrimarySound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPrimarySound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EActionSound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EActionSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EBuildType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EBuildMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ECompoundTemplates_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECompoundTemplates_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ECompoundEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECompoundEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ESubBlockDefinitions_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESubBlockDefinitions_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MySubBlockDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in MySubBlockDefinition[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out MySubBlockDefinition[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EMultiBlock_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMultiBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ENavigationDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ENavigationDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EGuiVisible_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGuiVisible_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EBlockVariants_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockVariants_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in SerializableDefinitionId[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out SerializableDefinitionId[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EDirection_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDirection_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyBlockDirection>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in MyBlockDirection value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out MyBlockDirection value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ERotation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ERotation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyBlockRotation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in MyBlockRotation value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out MyBlockRotation value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EGeneratedBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneratedBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in SerializableDefinitionId[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out SerializableDefinitionId[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EGeneratedBlockType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneratedBlockType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EMirrored_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirrored_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EDamageEffectId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EDestroyEffect_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroyEffect_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EDestroySound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroySound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ESkeleton_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESkeleton_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, List<BoneInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in List<BoneInfo> value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out List<BoneInfo> value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ERandomRotation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ERandomRotation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EIsAirTight_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EIsAirTight_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EIsStandAlone_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EIsStandAlone_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EHasPhysics_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EHasPhysics_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EUseNeighbourOxygenRooms_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUseNeighbourOxygenRooms_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EPoints_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPoints_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EMaxIntegrity_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMaxIntegrity_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EBuildProgressToPlaceGeneratedBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressToPlaceGeneratedBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EDamagedSound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamagedSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ECreateFracturedPieces_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECreateFracturedPieces_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EEmissiveColorPreset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEmissiveColorPreset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EGeneralDamageMultiplier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneralDamageMultiplier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EDamageEffectName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EUsesDeformation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUsesDeformation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EDestroyEffectOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroyEffectOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, Vector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in Vector3? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out Vector3? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EPCU_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPCU_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EPlaceDecals_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPlaceDecals_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_CubeBlockDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EPhysicalMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EMass_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EMass_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FueledPowerProducerDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FueledPowerProducerDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_FueledPowerProducerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_FueledPowerProducerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_FueledPowerProducerDefinition CreateInstance()
			{
				return new MyObjectBuilder_FueledPowerProducerDefinition();
			}

			MyObjectBuilder_FueledPowerProducerDefinition IActivator<MyObjectBuilder_FueledPowerProducerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public float FuelProductionToCapacityMultiplier = 3600f;

		[ProtoMember(4)]
		public List<FuelInfo> FuelInfos;

		[ProtoMember(7)]
		public string ResourceSinkGroup;
	}
}
