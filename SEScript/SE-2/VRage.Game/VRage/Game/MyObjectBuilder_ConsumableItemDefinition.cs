using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ConsumableItemDefinition : MyObjectBuilder_UsableItemDefinition
	{
		[ProtoContract]
		public class StatValue
		{
			protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EStatValue_003C_003EName_003C_003EAccessor : IMemberAccessor<StatValue, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StatValue owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StatValue owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EStatValue_003C_003EValue_003C_003EAccessor : IMemberAccessor<StatValue, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StatValue owner, in float value)
				{
					owner.Value = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StatValue owner, out float value)
				{
					value = owner.Value;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EStatValue_003C_003ETime_003C_003EAccessor : IMemberAccessor<StatValue, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StatValue owner, in float value)
				{
					owner.Time = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StatValue owner, out float value)
				{
					value = owner.Time;
				}
			}

			private class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EStatValue_003C_003EActor : IActivator, IActivator<StatValue>
			{
				private sealed override object CreateInstance()
				{
					return new StatValue();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override StatValue CreateInstance()
				{
					return new StatValue();
				}

				StatValue IActivator<StatValue>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			[XmlAttribute]
			public string Name;

			[ProtoMember(4)]
			[XmlAttribute]
			public float Value;

			[ProtoMember(7)]
			[XmlAttribute]
			public float Time;
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EStats_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, StatValue[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in StatValue[] value)
			{
				owner.Stats = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out StatValue[] value)
			{
				value = owner.Stats;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EUseSound_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_UsableItemDefinition_003C_003EUseSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_UsableItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_UsableItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EMass_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMass_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EModels_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModels_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EIconSymbol_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EIconSymbol_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EVolume_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVolume_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in float? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out float? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EModelVolume_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModelVolume_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in float? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out float? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EPhysicalMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EVoxelMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVoxelMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003ECanSpawnFromScreen_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanSpawnFromScreen_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003ERotateOnSpawnX_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnX_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003ERotateOnSpawnY_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnY_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003ERotateOnSpawnZ_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnZ_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EHealth_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EHealth_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EDestroyedPieceId_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieceId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EDestroyedPieces_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieces_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EExtraInventoryTooltipLine_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EExtraInventoryTooltipLine_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EMaxStackAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaxStackAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, MyFixedPoint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in MyFixedPoint value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out MyFixedPoint value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EMinimalPricePerUnit_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimalPricePerUnit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EMinimumOfferAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOfferAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EMaximumOfferAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOfferAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EMinimumOrderAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOrderAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EMaximumOrderAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOrderAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003ECanPlayerOrder_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanPlayerOrder_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EMinimumAcquisitionAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumAcquisitionAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EMaximumAcquisitionAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumAcquisitionAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConsumableItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConsumableItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConsumableItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ConsumableItemDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ConsumableItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ConsumableItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ConsumableItemDefinition CreateInstance()
			{
				return new MyObjectBuilder_ConsumableItemDefinition();
			}

			MyObjectBuilder_ConsumableItemDefinition IActivator<MyObjectBuilder_ConsumableItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlArrayItem("Stat")]
		[ProtoMember(10)]
		public StatValue[] Stats;
	}
}
