using ProtoBuf;
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
	public class MyObjectBuilder_WeaponItemDefinition : MyObjectBuilder_PhysicalItemDefinition
	{
		[ProtoContract]
		public class PhysicalItemWeaponDefinitionId
		{
			protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EPhysicalItemWeaponDefinitionId_003C_003EType_003C_003EAccessor : IMemberAccessor<PhysicalItemWeaponDefinitionId, MyObjectBuilderType>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref PhysicalItemWeaponDefinitionId owner, in MyObjectBuilderType value)
				{
					owner.Type = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref PhysicalItemWeaponDefinitionId owner, out MyObjectBuilderType value)
				{
					value = owner.Type;
				}
			}

			protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EPhysicalItemWeaponDefinitionId_003C_003ESubtype_003C_003EAccessor : IMemberAccessor<PhysicalItemWeaponDefinitionId, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref PhysicalItemWeaponDefinitionId owner, in string value)
				{
					owner.Subtype = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref PhysicalItemWeaponDefinitionId owner, out string value)
				{
					value = owner.Subtype;
				}
			}

			private class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EPhysicalItemWeaponDefinitionId_003C_003EActor : IActivator, IActivator<PhysicalItemWeaponDefinitionId>
			{
				private sealed override object CreateInstance()
				{
					return new PhysicalItemWeaponDefinitionId();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override PhysicalItemWeaponDefinitionId CreateInstance()
				{
					return new PhysicalItemWeaponDefinitionId();
				}

				PhysicalItemWeaponDefinitionId IActivator<PhysicalItemWeaponDefinitionId>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlIgnore]
			public MyObjectBuilderType Type = typeof(MyObjectBuilder_WeaponDefinition);

			[XmlAttribute]
			[ProtoMember(1)]
			public string Subtype;
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EWeaponDefinitionId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, PhysicalItemWeaponDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in PhysicalItemWeaponDefinitionId value)
			{
				owner.WeaponDefinitionId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out PhysicalItemWeaponDefinitionId value)
			{
				value = owner.WeaponDefinitionId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EShowAmmoCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value)
			{
				owner.ShowAmmoCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value)
			{
				value = owner.ShowAmmoCount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EMass_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMass_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EModels_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModels_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EIconSymbol_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EIconSymbol_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EVolume_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVolume_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in float? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out float? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EModelVolume_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModelVolume_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in float? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out float? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EPhysicalMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EVoxelMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVoxelMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003ECanSpawnFromScreen_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanSpawnFromScreen_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003ERotateOnSpawnX_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnX_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003ERotateOnSpawnY_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnY_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003ERotateOnSpawnZ_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnZ_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EHealth_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EHealth_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EDestroyedPieceId_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieceId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EDestroyedPieces_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieces_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EExtraInventoryTooltipLine_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EExtraInventoryTooltipLine_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EMaxStackAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaxStackAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, MyFixedPoint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in MyFixedPoint value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out MyFixedPoint value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EMinimalPricePerUnit_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimalPricePerUnit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EMinimumOfferAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOfferAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EMaximumOfferAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOfferAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EMinimumOrderAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOrderAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EMaximumOrderAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOrderAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003ECanPlayerOrder_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanPlayerOrder_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EMinimumAcquisitionAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumAcquisitionAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EMaximumAcquisitionAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumAcquisitionAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_WeaponItemDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WeaponItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WeaponItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WeaponItemDefinition CreateInstance()
			{
				return new MyObjectBuilder_WeaponItemDefinition();
			}

			MyObjectBuilder_WeaponItemDefinition IActivator<MyObjectBuilder_WeaponItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(4)]
		public PhysicalItemWeaponDefinitionId WeaponDefinitionId;

		[ProtoMember(7)]
		public bool ShowAmmoCount;
	}
}
