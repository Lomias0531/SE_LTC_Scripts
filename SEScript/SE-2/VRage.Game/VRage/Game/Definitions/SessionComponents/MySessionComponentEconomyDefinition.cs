using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game.Definitions.SessionComponents
{
	[MyDefinitionType(typeof(MyObjectbuilder_SessionComponentEconomyDefinition), null)]
	public class MySessionComponentEconomyDefinition : MySessionComponentDefinition
	{
		private class VRage_Game_Definitions_SessionComponents_MySessionComponentEconomyDefinition_003C_003EActor : IActivator, IActivator<MySessionComponentEconomyDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySessionComponentEconomyDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySessionComponentEconomyDefinition CreateInstance()
			{
				return new MySessionComponentEconomyDefinition();
			}

			MySessionComponentEconomyDefinition IActivator<MySessionComponentEconomyDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public const int StoreCreationLimitPerPlayer = 30;

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

		public MyDefinitionId PirateId;

		public float OffersFriendlyBonus
		{
			get;
			set;
		}

		public float OrdersFriendlyBonus
		{
			get;
			set;
		}

		public float TransactionFee
		{
			get;
			set;
		}

		public float ListingFee
		{
			get;
			set;
		}

		public SerializableDefinitionId DatapadDefinition
		{
			get;
			set;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectbuilder_SessionComponentEconomyDefinition myObjectbuilder_SessionComponentEconomyDefinition = (MyObjectbuilder_SessionComponentEconomyDefinition)builder;
			if (myObjectbuilder_SessionComponentEconomyDefinition != null)
			{
				PerFactionInitialCurrency = myObjectbuilder_SessionComponentEconomyDefinition.PerFactionInitialCurrency;
				FactionRatio_Miners = myObjectbuilder_SessionComponentEconomyDefinition.FactionRatio_Miners;
				FactionRatio_Traders = myObjectbuilder_SessionComponentEconomyDefinition.FactionRatio_Traders;
				FactionRatio_Builders = myObjectbuilder_SessionComponentEconomyDefinition.FactionRatio_Builders;
				DeepSpaceStationStoreBonus = myObjectbuilder_SessionComponentEconomyDefinition.DeepSpaceStationStoreBonus;
				DeepSpaceStationContractBonus = myObjectbuilder_SessionComponentEconomyDefinition.DeepSpaceStationContractBonus;
				RepMult_Pos_Owner = myObjectbuilder_SessionComponentEconomyDefinition.RepMult_Pos_Owner;
				RepMult_Pos_Friend = myObjectbuilder_SessionComponentEconomyDefinition.RepMult_Pos_Friend;
				RepMult_Pos_Neutral = myObjectbuilder_SessionComponentEconomyDefinition.RepMult_Pos_Neutral;
				RepMult_Pos_Enemy = myObjectbuilder_SessionComponentEconomyDefinition.RepMult_Pos_Enemy;
				RepMult_Neg_Owner = myObjectbuilder_SessionComponentEconomyDefinition.RepMult_Neg_Owner;
				RepMult_Neg_Friend = myObjectbuilder_SessionComponentEconomyDefinition.RepMult_Neg_Friend;
				RepMult_Neg_Neutral = myObjectbuilder_SessionComponentEconomyDefinition.RepMult_Neg_Neutral;
				RepMult_Neg_Enemy = myObjectbuilder_SessionComponentEconomyDefinition.RepMult_Neg_Enemy;
				ReputationHostileMin = myObjectbuilder_SessionComponentEconomyDefinition.ReputationHostileMin;
				ReputationHostileMid = myObjectbuilder_SessionComponentEconomyDefinition.ReputationHostileMid;
				ReputationNeutralMin = myObjectbuilder_SessionComponentEconomyDefinition.ReputationNeutralMin;
				ReputationNeutralMid = myObjectbuilder_SessionComponentEconomyDefinition.ReputationNeutralMid;
				ReputationFriendlyMin = myObjectbuilder_SessionComponentEconomyDefinition.ReputationFriendlyMin;
				ReputationFriendlyMid = myObjectbuilder_SessionComponentEconomyDefinition.ReputationFriendlyMid;
				ReputationFriendlyMax = myObjectbuilder_SessionComponentEconomyDefinition.ReputationFriendlyMax;
				ReputationPlayerDefault = myObjectbuilder_SessionComponentEconomyDefinition.ReputationPlayerDefault;
				ReputationLevelValue = myObjectbuilder_SessionComponentEconomyDefinition.ReputationLevelValue;
				Station_Distance_MinimalFromOtherStation = myObjectbuilder_SessionComponentEconomyDefinition.Station_Distance_MinimalFromOtherStation;
				Station_Rule_Miner_Min_StationM = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Miner_Min_StationM;
				Station_Rule_Miner_Max_StationM = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Miner_Max_StationM;
				Station_Rule_Miner_Min_Outpost = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Miner_Min_Outpost;
				Station_Rule_Miner_Max_Outpost = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Miner_Max_Outpost;
				Station_Rule_Trader_Min_Orbit = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Trader_Min_Orbit;
				Station_Rule_Trader_Max_Orbit = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Trader_Max_Orbit;
				Station_Rule_Trader_Min_Outpost = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Trader_Min_Outpost;
				Station_Rule_Trader_Max_Outpost = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Trader_Max_Outpost;
				Station_Rule_Trader_Min_Deep = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Trader_Min_Deep;
				Station_Rule_Trader_Max_Deep = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Trader_Max_Deep;
				Station_Rule_Builder_Min_Orbit = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Builder_Min_Orbit;
				Station_Rule_Builder_Max_Orbit = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Builder_Max_Orbit;
				Station_Rule_Builder_Min_Outpost = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Builder_Min_Outpost;
				Station_Rule_Builder_Max_Outpost = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Builder_Max_Outpost;
				Station_Rule_Builder_Min_Station = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Builder_Min_Station;
				Station_Rule_Builder_Max_Station = myObjectbuilder_SessionComponentEconomyDefinition.Station_Rule_Builder_Max_Station;
				PirateId = myObjectbuilder_SessionComponentEconomyDefinition.PirateId;
				OffersFriendlyBonus = myObjectbuilder_SessionComponentEconomyDefinition.OffersFriendlyBonus;
				OrdersFriendlyBonus = myObjectbuilder_SessionComponentEconomyDefinition.OrdersFriendlyBonus;
				TransactionFee = myObjectbuilder_SessionComponentEconomyDefinition.TransactionFee;
				ListingFee = myObjectbuilder_SessionComponentEconomyDefinition.ListingFee;
				DatapadDefinition = myObjectbuilder_SessionComponentEconomyDefinition.DatapadDefinition;
			}
		}
	}
}
