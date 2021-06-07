using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_PhysicalItemDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ESize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in Vector3 value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out Vector3 value)
			{
				value = owner.Size;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMass_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in float value)
			{
				owner.Mass = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out float value)
			{
				value = owner.Mass;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModel_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in string value)
			{
				owner.Model = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out string value)
			{
				value = owner.Model;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModels_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in string[] value)
			{
				owner.Models = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out string[] value)
			{
				value = owner.Models;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EIconSymbol_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in string value)
			{
				owner.IconSymbol = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out string value)
			{
				value = owner.IconSymbol;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVolume_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in float? value)
			{
				owner.Volume = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out float? value)
			{
				value = owner.Volume;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModelVolume_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in float? value)
			{
				owner.ModelVolume = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out float? value)
			{
				value = owner.ModelVolume;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in string value)
			{
				owner.PhysicalMaterial = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out string value)
			{
				value = owner.PhysicalMaterial;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVoxelMaterial_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in string value)
			{
				owner.VoxelMaterial = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out string value)
			{
				value = owner.VoxelMaterial;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanSpawnFromScreen_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in bool value)
			{
				owner.CanSpawnFromScreen = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out bool value)
			{
				value = owner.CanSpawnFromScreen;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnX_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in bool value)
			{
				owner.RotateOnSpawnX = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out bool value)
			{
				value = owner.RotateOnSpawnX;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnY_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in bool value)
			{
				owner.RotateOnSpawnY = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out bool value)
			{
				value = owner.RotateOnSpawnY;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnZ_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in bool value)
			{
				owner.RotateOnSpawnZ = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out bool value)
			{
				value = owner.RotateOnSpawnZ;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EHealth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in int value)
			{
				owner.Health = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out int value)
			{
				value = owner.Health;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieceId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in SerializableDefinitionId? value)
			{
				owner.DestroyedPieceId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out SerializableDefinitionId? value)
			{
				value = owner.DestroyedPieceId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieces_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in int value)
			{
				owner.DestroyedPieces = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out int value)
			{
				value = owner.DestroyedPieces;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EExtraInventoryTooltipLine_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in string value)
			{
				owner.ExtraInventoryTooltipLine = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out string value)
			{
				value = owner.ExtraInventoryTooltipLine;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaxStackAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, MyFixedPoint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in MyFixedPoint value)
			{
				owner.MaxStackAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out MyFixedPoint value)
			{
				value = owner.MaxStackAmount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimalPricePerUnit_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in int value)
			{
				owner.MinimalPricePerUnit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out int value)
			{
				value = owner.MinimalPricePerUnit;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOfferAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in int value)
			{
				owner.MinimumOfferAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out int value)
			{
				value = owner.MinimumOfferAmount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOfferAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in int value)
			{
				owner.MaximumOfferAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out int value)
			{
				value = owner.MaximumOfferAmount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOrderAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in int value)
			{
				owner.MinimumOrderAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out int value)
			{
				value = owner.MinimumOrderAmount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOrderAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in int value)
			{
				owner.MaximumOrderAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out int value)
			{
				value = owner.MaximumOrderAmount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanPlayerOrder_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in bool value)
			{
				owner.CanPlayerOrder = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out bool value)
			{
				value = owner.CanPlayerOrder;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumAcquisitionAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in int value)
			{
				owner.MinimumAcquisitionAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out int value)
			{
				value = owner.MinimumAcquisitionAmount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumAcquisitionAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in int value)
			{
				owner.MaximumAcquisitionAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out int value)
			{
				value = owner.MaximumAcquisitionAmount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_PhysicalItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_PhysicalItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_PhysicalItemDefinition CreateInstance()
			{
				return new MyObjectBuilder_PhysicalItemDefinition();
			}

			MyObjectBuilder_PhysicalItemDefinition IActivator<MyObjectBuilder_PhysicalItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public Vector3 Size;

		[ProtoMember(4)]
		public float Mass;

		[ProtoMember(7)]
		[ModdableContentFile("mwm")]
		public string Model = "Models\\Components\\Sphere.mwm";

		[ProtoMember(10)]
		[ModdableContentFile("mwm")]
		[XmlArrayItem("Model")]
		public string[] Models;

		[ProtoMember(13)]
		[DefaultValue(null)]
		public string IconSymbol;

		[ProtoMember(16)]
		[DefaultValue(null)]
		public float? Volume;

		[ProtoMember(19)]
		[DefaultValue(null)]
		public float? ModelVolume;

		[ProtoMember(22)]
		public string PhysicalMaterial;

		[ProtoMember(25)]
		public string VoxelMaterial;

		[ProtoMember(28)]
		[DefaultValue(true)]
		public bool CanSpawnFromScreen = true;

		[ProtoMember(31)]
		public bool RotateOnSpawnX;

		[ProtoMember(34)]
		public bool RotateOnSpawnY;

		[ProtoMember(37)]
		public bool RotateOnSpawnZ;

		[ProtoMember(40)]
		public int Health = 100;

		[ProtoMember(43)]
		[DefaultValue(null)]
		public SerializableDefinitionId? DestroyedPieceId;

		[ProtoMember(46)]
		public int DestroyedPieces;

		[ProtoMember(49)]
		[DefaultValue(null)]
		public string ExtraInventoryTooltipLine;

		[ProtoMember(52)]
		public MyFixedPoint MaxStackAmount = MyFixedPoint.MaxValue;

		[ProtoMember(55)]
		public int MinimalPricePerUnit = -1;

		[ProtoMember(57)]
		public int MinimumOfferAmount;

		[ProtoMember(59)]
		public int MaximumOfferAmount;

		[ProtoMember(61)]
		public int MinimumOrderAmount;

		[ProtoMember(63)]
		public int MaximumOrderAmount;

		[ProtoMember(65)]
		public bool CanPlayerOrder;

		[ProtoMember(67)]
		public int MinimumAcquisitionAmount = 10;

		[ProtoMember(69)]
		public int MaximumAcquisitionAmount = 100;

		public bool ShouldSerializeIconSymbol()
		{
			return IconSymbol != null;
		}
	}
}
