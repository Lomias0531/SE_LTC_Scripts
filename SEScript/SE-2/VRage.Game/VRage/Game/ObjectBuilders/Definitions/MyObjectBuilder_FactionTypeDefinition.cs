using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_FactionTypeDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOffersList_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in SerializableDefinitionId[] value)
			{
				owner.OffersList = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out SerializableDefinitionId[] value)
			{
				value = owner.OffersList;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOrdersList_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in SerializableDefinitionId[] value)
			{
				owner.OrdersList = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out SerializableDefinitionId[] value)
			{
				value = owner.OrdersList;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003ECanSellOxygen_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in bool value)
			{
				owner.CanSellOxygen = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out bool value)
			{
				value = owner.CanSellOxygen;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003ECanSellHydrogen_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in bool value)
			{
				owner.CanSellHydrogen = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out bool value)
			{
				value = owner.CanSellHydrogen;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOfferPriceUpMultiplierMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OfferPriceUpMultiplierMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OfferPriceUpMultiplierMax;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOfferPriceUpMultiplierMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OfferPriceUpMultiplierMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OfferPriceUpMultiplierMin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOfferPriceDownMultiplierMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OfferPriceDownMultiplierMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OfferPriceDownMultiplierMax;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOfferPriceDownMultiplierMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OfferPriceDownMultiplierMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OfferPriceDownMultiplierMin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOfferPriceUpDownPoint_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OfferPriceUpDownPoint = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OfferPriceUpDownPoint;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOfferPriceBellowMinimumMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OfferPriceBellowMinimumMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OfferPriceBellowMinimumMultiplier;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOfferPriceStartingMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OfferPriceStartingMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OfferPriceStartingMultiplier;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOfferMaxUpdateCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in byte value)
			{
				owner.OfferMaxUpdateCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out byte value)
			{
				value = owner.OfferMaxUpdateCount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOrderPriceStartingMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OrderPriceStartingMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OrderPriceStartingMultiplier;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOrderPriceUpMultiplierMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OrderPriceUpMultiplierMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OrderPriceUpMultiplierMax;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOrderPriceUpMultiplierMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OrderPriceUpMultiplierMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OrderPriceUpMultiplierMin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOrderPriceDownMultiplierMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OrderPriceDownMultiplierMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OrderPriceDownMultiplierMax;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOrderPriceDownMultiplierMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OrderPriceDownMultiplierMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OrderPriceDownMultiplierMin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOrderPriceOverMinimumMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OrderPriceOverMinimumMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OrderPriceOverMinimumMultiplier;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOrderPriceUpDownPoint_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.OrderPriceUpDownPoint = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.OrderPriceUpDownPoint;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EOrderMaxUpdateCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in byte value)
			{
				owner.OrderMaxUpdateCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out byte value)
			{
				value = owner.OrderMaxUpdateCount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EMinimumOfferGasAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in int value)
			{
				owner.MinimumOfferGasAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out int value)
			{
				value = owner.MinimumOfferGasAmount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EMaximumOfferGasAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in int value)
			{
				owner.MaximumOfferGasAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out int value)
			{
				value = owner.MaximumOfferGasAmount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EBaseCostProductionSpeedMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value)
			{
				owner.BaseCostProductionSpeedMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value)
			{
				value = owner.BaseCostProductionSpeedMultiplier;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EMinimumOxygenPrice_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in int value)
			{
				owner.MinimumOxygenPrice = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out int value)
			{
				value = owner.MinimumOxygenPrice;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EMinimumHydrogenPrice_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in int value)
			{
				owner.MinimumHydrogenPrice = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out int value)
			{
				value = owner.MinimumHydrogenPrice;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EGridsForSale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string[] value)
			{
				owner.GridsForSale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string[] value)
			{
				value = owner.GridsForSale;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EMaxContractCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in int value)
			{
				owner.MaxContractCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out int value)
			{
				value = owner.MaxContractCount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_FactionTypeDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_FactionTypeDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_FactionTypeDefinition CreateInstance()
			{
				return new MyObjectBuilder_FactionTypeDefinition();
			}

			MyObjectBuilder_FactionTypeDefinition IActivator<MyObjectBuilder_FactionTypeDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlArrayItem("ItemId")]
		[ProtoMember(1)]
		[DefaultValue(null)]
		public SerializableDefinitionId[] OffersList;

		[XmlArrayItem("ItemId")]
		[ProtoMember(3)]
		[DefaultValue(null)]
		public SerializableDefinitionId[] OrdersList;

		[ProtoMember(5)]
		public bool CanSellOxygen;

		[ProtoMember(6)]
		public bool CanSellHydrogen;

		[ProtoMember(7)]
		public float OfferPriceUpMultiplierMax = 1.1f;

		[ProtoMember(9)]
		public float OfferPriceUpMultiplierMin = 1.05f;

		[ProtoMember(11)]
		public float OfferPriceDownMultiplierMax = 0.925f;

		[ProtoMember(13)]
		public float OfferPriceDownMultiplierMin = 0.9f;

		[ProtoMember(15)]
		public float OfferPriceUpDownPoint = 0.5f;

		[ProtoMember(17)]
		public float OfferPriceBellowMinimumMultiplier = 0.85f;

		[ProtoMember(19)]
		public float OfferPriceStartingMultiplier = 1.2f;

		[ProtoMember(21)]
		public byte OfferMaxUpdateCount = 3;

		[ProtoMember(23)]
		public float OrderPriceStartingMultiplier = 0.75f;

		[ProtoMember(25)]
		public float OrderPriceUpMultiplierMax = 1.1f;

		[ProtoMember(27)]
		public float OrderPriceUpMultiplierMin = 1.05f;

		[ProtoMember(29)]
		public float OrderPriceDownMultiplierMax = 0.95f;

		[ProtoMember(31)]
		public float OrderPriceDownMultiplierMin = 0.75f;

		[ProtoMember(33)]
		public float OrderPriceOverMinimumMultiplier = 1.1f;

		[ProtoMember(35)]
		public float OrderPriceUpDownPoint = 0.25f;

		[ProtoMember(37)]
		public byte OrderMaxUpdateCount = 4;

		[ProtoMember(39)]
		public int MinimumOfferGasAmount = 100;

		[ProtoMember(41)]
		public int MaximumOfferGasAmount = 10000;

		[ProtoMember(45)]
		public float BaseCostProductionSpeedMultiplier = 1f;

		[ProtoMember(47)]
		public int MinimumOxygenPrice = 150;

		[ProtoMember(49)]
		public int MinimumHydrogenPrice = 150;

		[XmlArrayItem("PrefabName")]
		[ProtoMember(51)]
		[DefaultValue(null)]
		public string[] GridsForSale;

		[ProtoMember(63)]
		public int MaxContractCount
		{
			get;
			set;
		}
	}
}
