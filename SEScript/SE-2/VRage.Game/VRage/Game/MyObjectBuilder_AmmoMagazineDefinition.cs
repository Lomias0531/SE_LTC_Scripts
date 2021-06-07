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
	public class MyObjectBuilder_AmmoMagazineDefinition : MyObjectBuilder_PhysicalItemDefinition
	{
		[ProtoContract]
		public class AmmoDefinition
		{
			protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EAmmoDefinition_003C_003EType_003C_003EAccessor : IMemberAccessor<AmmoDefinition, MyObjectBuilderType>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AmmoDefinition owner, in MyObjectBuilderType value)
				{
					owner.Type = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AmmoDefinition owner, out MyObjectBuilderType value)
				{
					value = owner.Type;
				}
			}

			protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EAmmoDefinition_003C_003ESubtype_003C_003EAccessor : IMemberAccessor<AmmoDefinition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AmmoDefinition owner, in string value)
				{
					owner.Subtype = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AmmoDefinition owner, out string value)
				{
					value = owner.Subtype;
				}
			}

			private class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EAmmoDefinition_003C_003EActor : IActivator, IActivator<AmmoDefinition>
			{
				private sealed override object CreateInstance()
				{
					return new AmmoDefinition();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override AmmoDefinition CreateInstance()
				{
					return new AmmoDefinition();
				}

				AmmoDefinition IActivator<AmmoDefinition>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlIgnore]
			public MyObjectBuilderType Type = typeof(MyObjectBuilder_AmmoDefinition);

			[XmlAttribute]
			[ProtoMember(1)]
			public string Subtype;
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003ECapacity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value)
			{
				owner.Capacity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value)
			{
				value = owner.Capacity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003ECategory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, MyAmmoCategoryEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in MyAmmoCategoryEnum value)
			{
				owner.Category = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out MyAmmoCategoryEnum value)
			{
				value = owner.Category;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EAmmoDefinitionId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, AmmoDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in AmmoDefinition value)
			{
				owner.AmmoDefinitionId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out AmmoDefinition value)
			{
				value = owner.AmmoDefinitionId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EMass_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMass_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EModels_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModels_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EIconSymbol_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EIconSymbol_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EVolume_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVolume_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in float? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out float? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EModelVolume_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModelVolume_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in float? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out float? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EPhysicalMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EVoxelMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVoxelMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003ECanSpawnFromScreen_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanSpawnFromScreen_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003ERotateOnSpawnX_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnX_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003ERotateOnSpawnY_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnY_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003ERotateOnSpawnZ_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnZ_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EHealth_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EHealth_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EDestroyedPieceId_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieceId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EDestroyedPieces_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieces_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EExtraInventoryTooltipLine_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EExtraInventoryTooltipLine_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EMaxStackAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaxStackAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, MyFixedPoint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in MyFixedPoint value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out MyFixedPoint value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EMinimalPricePerUnit_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimalPricePerUnit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EMinimumOfferAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOfferAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EMaximumOfferAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOfferAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EMinimumOrderAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOrderAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EMaximumOrderAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOrderAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003ECanPlayerOrder_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanPlayerOrder_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EMinimumAcquisitionAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumAcquisitionAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EMaximumAcquisitionAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumAcquisitionAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AmmoMagazineDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AmmoMagazineDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AmmoMagazineDefinition CreateInstance()
			{
				return new MyObjectBuilder_AmmoMagazineDefinition();
			}

			MyObjectBuilder_AmmoMagazineDefinition IActivator<MyObjectBuilder_AmmoMagazineDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(4)]
		public int Capacity;

		[ProtoMember(7)]
		public MyAmmoCategoryEnum Category;

		[ProtoMember(10)]
		public AmmoDefinition AmmoDefinitionId;
	}
}
