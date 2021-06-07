using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectbuilder_SessionComponentEconomyDefinition : MyObjectBuilder_SessionComponentDefinition
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EPerFactionInitialCurrency_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in long value)
			{
				owner.PerFactionInitialCurrency = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out long value)
			{
				value = owner.PerFactionInitialCurrency;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EFactionRatio_Miners_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.FactionRatio_Miners = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.FactionRatio_Miners;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EFactionRatio_Traders_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.FactionRatio_Traders = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.FactionRatio_Traders;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EFactionRatio_Builders_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.FactionRatio_Builders = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.FactionRatio_Builders;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EDeepSpaceStationStoreBonus_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.DeepSpaceStationStoreBonus = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.DeepSpaceStationStoreBonus;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EDeepSpaceStationContractBonus_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.DeepSpaceStationContractBonus = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.DeepSpaceStationContractBonus;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003ERepMult_Pos_Owner_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.RepMult_Pos_Owner = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.RepMult_Pos_Owner;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003ERepMult_Pos_Friend_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.RepMult_Pos_Friend = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.RepMult_Pos_Friend;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003ERepMult_Pos_Neutral_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.RepMult_Pos_Neutral = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.RepMult_Pos_Neutral;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003ERepMult_Pos_Enemy_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.RepMult_Pos_Enemy = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.RepMult_Pos_Enemy;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003ERepMult_Neg_Owner_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.RepMult_Neg_Owner = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.RepMult_Neg_Owner;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003ERepMult_Neg_Friend_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.RepMult_Neg_Friend = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.RepMult_Neg_Friend;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003ERepMult_Neg_Neutral_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.RepMult_Neg_Neutral = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.RepMult_Neg_Neutral;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003ERepMult_Neg_Enemy_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.RepMult_Neg_Enemy = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.RepMult_Neg_Enemy;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EReputationHostileMin_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.ReputationHostileMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.ReputationHostileMin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EReputationHostileMid_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.ReputationHostileMid = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.ReputationHostileMid;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EReputationNeutralMin_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.ReputationNeutralMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.ReputationNeutralMin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EReputationNeutralMid_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.ReputationNeutralMid = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.ReputationNeutralMid;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EReputationFriendlyMin_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.ReputationFriendlyMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.ReputationFriendlyMin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EReputationFriendlyMid_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.ReputationFriendlyMid = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.ReputationFriendlyMid;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EReputationFriendlyMax_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.ReputationFriendlyMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.ReputationFriendlyMax;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EReputationPlayerDefault_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.ReputationPlayerDefault = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.ReputationPlayerDefault;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EReputationLevelValue_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.ReputationLevelValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.ReputationLevelValue;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Distance_MinimalFromOtherStation_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in double value)
			{
				owner.Station_Distance_MinimalFromOtherStation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out double value)
			{
				value = owner.Station_Distance_MinimalFromOtherStation;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Miner_Min_StationM_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Miner_Min_StationM = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Miner_Min_StationM;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Miner_Max_StationM_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Miner_Max_StationM = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Miner_Max_StationM;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Miner_Min_Outpost_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Miner_Min_Outpost = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Miner_Min_Outpost;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Miner_Max_Outpost_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Miner_Max_Outpost = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Miner_Max_Outpost;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Trader_Min_Orbit_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Trader_Min_Orbit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Trader_Min_Orbit;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Trader_Max_Orbit_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Trader_Max_Orbit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Trader_Max_Orbit;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Trader_Min_Outpost_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Trader_Min_Outpost = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Trader_Min_Outpost;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Trader_Max_Outpost_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Trader_Max_Outpost = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Trader_Max_Outpost;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Trader_Min_Deep_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Trader_Min_Deep = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Trader_Min_Deep;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Trader_Max_Deep_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Trader_Max_Deep = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Trader_Max_Deep;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Builder_Min_Orbit_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Builder_Min_Orbit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Builder_Min_Orbit;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Builder_Max_Orbit_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Builder_Max_Orbit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Builder_Max_Orbit;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Builder_Min_Outpost_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Builder_Min_Outpost = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Builder_Min_Outpost;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Builder_Max_Outpost_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Builder_Max_Outpost = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Builder_Max_Outpost;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Builder_Min_Station_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Builder_Min_Station = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Builder_Min_Station;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EStation_Rule_Builder_Max_Station_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in int value)
			{
				owner.Station_Rule_Builder_Max_Station = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out int value)
			{
				value = owner.Station_Rule_Builder_Max_Station;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EPirateId_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in SerializableDefinitionId value)
			{
				owner.PirateId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out SerializableDefinitionId value)
			{
				value = owner.PirateId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EOffersFriendlyBonus_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.OffersFriendlyBonus = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.OffersFriendlyBonus;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EOrdersFriendlyBonus_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.OrdersFriendlyBonus = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.OrdersFriendlyBonus;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003ETransactionFee_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.TransactionFee = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.TransactionFee;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EListingFee_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in float value)
			{
				owner.ListingFee = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out float value)
			{
				value = owner.ListingFee;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EDatapadDefinition_003C_003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in SerializableDefinitionId value)
			{
				owner.DatapadDefinition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out SerializableDefinitionId value)
			{
				value = owner.DatapadDefinition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectbuilder_SessionComponentEconomyDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition_003C_003EActor : IActivator, IActivator<MyObjectbuilder_SessionComponentEconomyDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectbuilder_SessionComponentEconomyDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectbuilder_SessionComponentEconomyDefinition CreateInstance()
			{
				return new MyObjectbuilder_SessionComponentEconomyDefinition();
			}

			MyObjectbuilder_SessionComponentEconomyDefinition IActivator<MyObjectbuilder_SessionComponentEconomyDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public long PerFactionInitialCurrency;

		public int FactionRatio_Miners;

		public int FactionRatio_Traders;

		public int FactionRatio_Builders;

		public float DeepSpaceStationStoreBonus;

		public float DeepSpaceStationContractBonus;

		public float RepMult_Pos_Owner;

		public float RepMult_Pos_Friend;

		public float RepMult_Pos_Neutral;

		public float RepMult_Pos_Enemy;

		public float RepMult_Neg_Owner;

		public float RepMult_Neg_Friend;

		public float RepMult_Neg_Neutral;

		public float RepMult_Neg_Enemy;

		public int ReputationHostileMin;

		public int ReputationHostileMid;

		public int ReputationNeutralMin;

		public int ReputationNeutralMid;

		public int ReputationFriendlyMin;

		public int ReputationFriendlyMid;

		public int ReputationFriendlyMax;

		public int ReputationPlayerDefault;

		public int ReputationLevelValue;

		public double Station_Distance_MinimalFromOtherStation;

		public int Station_Rule_Miner_Min_StationM;

		public int Station_Rule_Miner_Max_StationM;

		public int Station_Rule_Miner_Min_Outpost;

		public int Station_Rule_Miner_Max_Outpost;

		public int Station_Rule_Trader_Min_Orbit;

		public int Station_Rule_Trader_Max_Orbit;

		public int Station_Rule_Trader_Min_Outpost;

		public int Station_Rule_Trader_Max_Outpost;

		public int Station_Rule_Trader_Min_Deep;

		public int Station_Rule_Trader_Max_Deep;

		public int Station_Rule_Builder_Min_Orbit;

		public int Station_Rule_Builder_Max_Orbit;

		public int Station_Rule_Builder_Min_Outpost;

		public int Station_Rule_Builder_Max_Outpost;

		public int Station_Rule_Builder_Min_Station;

		public int Station_Rule_Builder_Max_Station;

		public SerializableDefinitionId PirateId;

		public float OffersFriendlyBonus = 0.1f;

		public float OrdersFriendlyBonus = 0.05f;

		public float TransactionFee = 0.02f;

		public float ListingFee = 0.03f;

		public SerializableDefinitionId DatapadDefinition;
	}
}
