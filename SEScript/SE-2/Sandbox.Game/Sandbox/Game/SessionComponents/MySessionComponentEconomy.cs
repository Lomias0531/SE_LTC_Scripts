using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 887, typeof(MyObjectBuilder_SessionComponentEconomy), null)]
	public class MySessionComponentEconomy : MySessionComponentBase
	{
		public static readonly float ORE_AROUND_STATION_REMOVAL_RADIUS = 5000f;

		private MyStoreItemsGenerator m_storeItemsGenerator = new MyStoreItemsGenerator();

		private MyMinimalPriceCalculator m_priceCalculator = new MyMinimalPriceCalculator();

		private Dictionary<long, MyDynamicAABBTree> m_stationOreBlockTrees = new Dictionary<long, MyDynamicAABBTree>();

		private Dictionary<long, long> m_analysisPerPlayerCurrency;

		private Dictionary<long, long> m_analysisPerFactionCurrency;

		private HashSet<long> m_stationIds = new HashSet<long>();

		private bool m_stationStoreItemsFirstGeneration;

		private Dictionary<long, List<long>> m_factionFriends;

		private Dictionary<long, string> m_factionFriendTooltips;

		internal MySessionComponentEconomyDefinition EconomyDefinition
		{
			get;
			private set;
		}

		internal MyTimeSpan LastEconomyTick
		{
			get;
			private set;
		}

		internal MyTimeSpan EconomyTick
		{
			get;
			private set;
		}

		public bool GenerateFactionsOnStart
		{
			get;
			private set;
		}

		public long AnalysisTotalCurrency
		{
			get;
			private set;
		}

		public Dictionary<long, long> AnalysisPerPlayerCurrency
		{
			get
			{
				return m_analysisPerPlayerCurrency;
			}
			set
			{
				m_analysisPerPlayerCurrency = value;
			}
		}

		public Dictionary<long, long> AnalysisPerFactionCurrency
		{
			get
			{
				return m_analysisPerFactionCurrency;
			}
			set
			{
				m_analysisPerFactionCurrency = value;
			}
		}

		public long AnalysisCurrencyFaucet
		{
			get;
			private set;
		}

		public long AnalysisCurrencySink
		{
			get;
			private set;
		}

		public long CurrencyGeneratedThisTick
		{
			get;
			set;
		}

		public long CurrencyDestroyedThisTick
		{
			get;
			set;
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyObjectBuilder_SessionComponentEconomy myObjectBuilder_SessionComponentEconomy = sessionComponent as MyObjectBuilder_SessionComponentEconomy;
			if (myObjectBuilder_SessionComponentEconomy != null)
			{
				GenerateFactionsOnStart = myObjectBuilder_SessionComponentEconomy.GenerateFactionsOnStart;
				AnalysisTotalCurrency = myObjectBuilder_SessionComponentEconomy.AnalysisTotalCurrency;
				AnalysisCurrencyFaucet = myObjectBuilder_SessionComponentEconomy.AnalysisCurrencyFaucet;
				AnalysisCurrencySink = myObjectBuilder_SessionComponentEconomy.AnalysisCurrencySink;
				CurrencyGeneratedThisTick = myObjectBuilder_SessionComponentEconomy.CurrencyGeneratedThisTick;
				CurrencyDestroyedThisTick = myObjectBuilder_SessionComponentEconomy.CurrencyDestroyedThisTick;
				if (AnalysisPerPlayerCurrency == null)
				{
					AnalysisPerPlayerCurrency = new Dictionary<long, long>();
				}
				if (myObjectBuilder_SessionComponentEconomy.AnalysisPerPlayerCurrency != null)
				{
					foreach (MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair item in myObjectBuilder_SessionComponentEconomy.AnalysisPerPlayerCurrency)
					{
						AnalysisPerPlayerCurrency.Add(item.Id, item.Balance);
					}
				}
				if (AnalysisPerFactionCurrency == null)
				{
					AnalysisPerFactionCurrency = new Dictionary<long, long>();
				}
				if (myObjectBuilder_SessionComponentEconomy.AnalysisPerFactionCurrency != null)
				{
					foreach (MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair item2 in myObjectBuilder_SessionComponentEconomy.AnalysisPerFactionCurrency)
					{
						AnalysisPerFactionCurrency.Add(item2.Id, item2.Balance);
					}
				}
			}
		}

		public override void InitFromDefinition(MySessionComponentDefinition definition)
		{
			base.InitFromDefinition(definition);
			EconomyDefinition = (definition as MySessionComponentEconomyDefinition);
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_SessionComponentEconomy myObjectBuilder_SessionComponentEconomy = base.GetObjectBuilder() as MyObjectBuilder_SessionComponentEconomy;
			myObjectBuilder_SessionComponentEconomy.GenerateFactionsOnStart = GenerateFactionsOnStart;
			myObjectBuilder_SessionComponentEconomy.AnalysisTotalCurrency = AnalysisTotalCurrency;
			myObjectBuilder_SessionComponentEconomy.AnalysisCurrencyFaucet = AnalysisCurrencyFaucet;
			myObjectBuilder_SessionComponentEconomy.AnalysisCurrencySink = AnalysisCurrencySink;
			myObjectBuilder_SessionComponentEconomy.CurrencyGeneratedThisTick = CurrencyGeneratedThisTick;
			myObjectBuilder_SessionComponentEconomy.CurrencyDestroyedThisTick = CurrencyDestroyedThisTick;
			if (myObjectBuilder_SessionComponentEconomy.AnalysisPerPlayerCurrency == null)
			{
				myObjectBuilder_SessionComponentEconomy.AnalysisPerPlayerCurrency = new MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair>();
			}
			MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair item;
			foreach (KeyValuePair<long, long> item2 in AnalysisPerPlayerCurrency)
			{
				MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair> analysisPerPlayerCurrency = myObjectBuilder_SessionComponentEconomy.AnalysisPerPlayerCurrency;
				item = new MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair
				{
					Id = item2.Key,
					Balance = item2.Value
				};
				analysisPerPlayerCurrency.Add(item);
			}
			if (myObjectBuilder_SessionComponentEconomy.AnalysisPerFactionCurrency == null)
			{
				myObjectBuilder_SessionComponentEconomy.AnalysisPerFactionCurrency = new MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair>();
			}
			foreach (KeyValuePair<long, long> item3 in AnalysisPerFactionCurrency)
			{
				MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair> analysisPerFactionCurrency = myObjectBuilder_SessionComponentEconomy.AnalysisPerFactionCurrency;
				item = new MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair
				{
					Id = item3.Key,
					Balance = item3.Value
				};
				analysisPerFactionCurrency.Add(item);
			}
			return myObjectBuilder_SessionComponentEconomy;
		}

		public override void BeforeStart()
		{
			if (Sync.IsServer)
			{
				if (MySession.Static.Settings.EnableEconomy)
				{
					EconomyTick = MyTimeSpan.FromSeconds(MySession.Static.Settings.EconomyTickInSeconds);
				}
				if (MySession.Static.Settings.EnableEconomy && GenerateFactionsOnStart)
				{
					new MyFactionGenerator().GenerateFactions(EconomyDefinition);
					new MyStationGenerator(EconomyDefinition).GenerateStations(MySession.Static.Factions);
					new MyFactionRelationGenerator(EconomyDefinition).GenerateFactionRelations(MySession.Static.Factions);
					GenerateFactionsOnStart = false;
					m_stationStoreItemsFirstGeneration = true;
					UpdateStations();
					m_stationStoreItemsFirstGeneration = false;
				}
			}
			RemoveOreAroundStations();
		}

		private void CreateTestingData()
		{
			MyObjectBuilder_Ore self = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Ice");
			MyObjectBuilder_Ore self2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Gold");
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				foreach (MyStation station in faction.Value.Stations)
				{
					int num = 0;
					for (int i = 0; i < 5; i++)
					{
						MyStoreItem item = new MyStoreItem(num, self.GetId(), i * 20, i * 100, StoreItemTypes.Offer, 0);
						station.StoreItems.Add(item);
						num++;
					}
					for (int j = 0; j < 5; j++)
					{
						MyStoreItem item2 = new MyStoreItem(num, self2.GetId(), j * 10, j * 100, StoreItemTypes.Offer, 0);
						station.StoreItems.Add(item2);
						num++;
					}
					for (int k = 0; k < 5; k++)
					{
						MyStoreItem item3 = new MyStoreItem(num, self.GetId(), k * 100, k * 100, StoreItemTypes.Order, 0);
						station.StoreItems.Add(item3);
						num++;
					}
					for (int l = 0; l < 5; l++)
					{
						MyStoreItem item4 = new MyStoreItem(num, self2.GetId(), l * 10, l * 100, StoreItemTypes.Order, 0);
						station.StoreItems.Add(item4);
						num++;
					}
					for (int m = 0; m < 5; m++)
					{
						MyStoreItem item5 = new MyStoreItem(num, m * 10, m * 100, StoreItemTypes.Offer, ItemTypes.Hydrogen);
						station.StoreItems.Add(item5);
						num++;
					}
				}
			}
		}

		public override void UpdateAfterSimulation()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			base.UpdateAfterSimulation();
			if (MySession.Static.Settings.EnableEconomy)
			{
				MyTimeSpan myTimeSpan = LastEconomyTick + EconomyTick;
				if ((double)MySandboxGame.TotalGamePlayTimeInMilliseconds > myTimeSpan.Milliseconds)
				{
					UpdateStations();
					UpdateCurrencyAnalysis();
					LastEconomyTick = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
				}
			}
		}

		public void ForceEconomyTick()
		{
			LastEconomyTick = MyTimeSpan.FromMilliseconds(-2147483647.0);
		}

		private void UpdateStations()
		{
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			component.CleanOldContracts();
			Dictionary<long, int> counts = new Dictionary<long, int>();
			Dictionary<long, List<MyContract>> lists = new Dictionary<long, List<MyContract>>();
			component.GetAvailableContractCountsByStation(ref counts, ref lists);
			MyContractGenerator cGen = new MyContractGenerator(EconomyDefinition);
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				foreach (MyStation station in faction.Value.Stations)
				{
					station.Update(faction.Value);
					if (!counts.ContainsKey(station.Id))
					{
						List<MyContract> existingContracts = new List<MyContract>();
						component.CreateContractsForStation(cGen, faction.Value, station, 0, ref existingContracts);
						lists.Add(station.Id, existingContracts);
					}
					else
					{
						List<MyContract> existingContracts2 = lists[station.Id];
						component.CreateContractsForStation(cGen, faction.Value, station, counts[station.Id], ref existingContracts2);
					}
				}
				m_storeItemsGenerator.Update(faction.Value, m_stationStoreItemsFirstGeneration);
			}
		}

		internal int GetDefaultReputationForRelation(MyRelationsBetweenFactions relation)
		{
			switch (relation)
			{
			case MyRelationsBetweenFactions.Enemies:
				return EconomyDefinition.ReputationHostileMid;
			case MyRelationsBetweenFactions.Friends:
				return EconomyDefinition.ReputationFriendlyMid;
			default:
				return EconomyDefinition.ReputationNeutralMid;
			}
		}

		public int GetDefaultReputationPlayer()
		{
			return EconomyDefinition.ReputationPlayerDefault;
		}

		internal int TranslateRelationToReputation(MyRelationsBetweenFactions relation)
		{
			switch (relation)
			{
			case MyRelationsBetweenFactions.Enemies:
				return EconomyDefinition.ReputationHostileMid;
			case MyRelationsBetweenFactions.Friends:
				return EconomyDefinition.ReputationNeutralMid;
			case MyRelationsBetweenFactions.Neutral:
			case MyRelationsBetweenFactions.Allies:
				return EconomyDefinition.ReputationNeutralMid;
			default:
				return 0;
			}
		}

		internal MyRelationsBetweenFactions TranslateReputationToRelationship(int reputation)
		{
			if (reputation < EconomyDefinition.ReputationHostileMin)
			{
				return MyRelationsBetweenFactions.Enemies;
			}
			if (reputation < EconomyDefinition.ReputationNeutralMin)
			{
				return MyRelationsBetweenFactions.Enemies;
			}
			if (reputation < EconomyDefinition.ReputationFriendlyMin)
			{
				return MyRelationsBetweenFactions.Neutral;
			}
			_ = EconomyDefinition.ReputationFriendlyMax;
			return MyRelationsBetweenFactions.Friends;
		}

		internal Tuple<MyRelationsBetweenFactions, int> ValidateReputationConsistency(MyRelationsBetweenFactions relation, int reputation)
		{
			if (TranslateReputationToRelationship(reputation) == relation)
			{
				return new Tuple<MyRelationsBetweenFactions, int>(relation, reputation);
			}
			return new Tuple<MyRelationsBetweenFactions, int>(relation, TranslateRelationToReputation(relation));
		}

		public int ClampReputation(int reputation)
		{
			if (reputation < EconomyDefinition.ReputationHostileMin)
			{
				return EconomyDefinition.ReputationHostileMin;
			}
			if (reputation > EconomyDefinition.ReputationFriendlyMax)
			{
				return EconomyDefinition.ReputationFriendlyMax;
			}
			return reputation;
		}

		public int GetHostileMax()
		{
			return EconomyDefinition.ReputationHostileMin;
		}

		public int GetNeutralMin()
		{
			return EconomyDefinition.ReputationNeutralMin;
		}

		public int GetFriendlyMin()
		{
			return EconomyDefinition.ReputationFriendlyMin;
		}

		public int GetFriendlyMax()
		{
			return EconomyDefinition.ReputationFriendlyMax;
		}

		internal MyFactionCollection.MyReputationModifiers GetReputationModifiers(bool positive = true)
		{
			MyFactionCollection.MyReputationModifiers result;
			if (positive)
			{
				result = default(MyFactionCollection.MyReputationModifiers);
				result.Owner = EconomyDefinition.RepMult_Pos_Owner;
				result.Friend = EconomyDefinition.RepMult_Pos_Friend;
				result.Neutral = EconomyDefinition.RepMult_Pos_Neutral;
				result.Hostile = EconomyDefinition.RepMult_Pos_Enemy;
				return result;
			}
			result = default(MyFactionCollection.MyReputationModifiers);
			result.Owner = EconomyDefinition.RepMult_Neg_Owner;
			result.Friend = EconomyDefinition.RepMult_Neg_Friend;
			result.Neutral = EconomyDefinition.RepMult_Neg_Neutral;
			result.Hostile = EconomyDefinition.RepMult_Neg_Enemy;
			return result;
		}

		public int ConvertPirateReputationToChance(int reputation)
		{
			float num = 900f;
			float num2 = (float)(reputation - GetNeutralMin()) / (float)(GetFriendlyMax() - GetNeutralMin());
			return (int)(100f + num2 * num);
		}

		internal float GetOrdersFriendlyBonus(int relationValue)
		{
			return 1f + EconomyDefinition.OrdersFriendlyBonus * ((float)(relationValue - EconomyDefinition.ReputationFriendlyMin) / (float)(EconomyDefinition.ReputationFriendlyMax - EconomyDefinition.ReputationFriendlyMin));
		}

		internal float GetOffersFriendlyBonus(int relationValue)
		{
			return 1f - EconomyDefinition.OffersFriendlyBonus * ((float)(relationValue - EconomyDefinition.ReputationFriendlyMin) / (float)(EconomyDefinition.ReputationFriendlyMax - EconomyDefinition.ReputationFriendlyMin));
		}

		internal float GetOrdersFriendlyBonusMax()
		{
			return EconomyDefinition.OrdersFriendlyBonus;
		}

		internal float GetOffersFriendlyBonusMax()
		{
			return EconomyDefinition.OffersFriendlyBonus;
		}

		public void AddCurrencyGenerated(long amount)
		{
			CurrencyGeneratedThisTick += amount;
		}

		public void AddCurrencyDestroyed(long amount)
		{
			CurrencyDestroyedThisTick += amount;
		}

		private void UpdateCurrencyAnalysis()
		{
			if (MyBankingSystem.Static != null)
			{
				AnalysisTotalCurrency = MyBankingSystem.Static.OverallBalance;
				MyBankingSystem.Static.GetPerPlayerBalances(ref m_analysisPerPlayerCurrency);
				MyBankingSystem.Static.GetPerFactionBalances(ref m_analysisPerFactionCurrency);
				AnalysisCurrencyFaucet = CurrencyGeneratedThisTick;
				AnalysisCurrencySink = CurrencyDestroyedThisTick;
				CurrencyGeneratedThisTick = 0L;
				CurrencyDestroyedThisTick = 0L;
			}
		}

		internal int GetMinimumItemPrice(SerializableDefinitionId itemDefinitionId)
		{
			int minimalPrice = 0;
			if (!m_priceCalculator.TryGetItemMinimalPrice(itemDefinitionId, out minimalPrice))
			{
				m_priceCalculator.CalculateMinimalPrices(new SerializableDefinitionId[1]
				{
					itemDefinitionId
				});
				if (!m_priceCalculator.TryGetItemMinimalPrice(itemDefinitionId, out minimalPrice))
				{
					return 0;
				}
			}
			return minimalPrice;
		}

		public int GetStoreCreationLimitPerPlayer()
		{
			return 30;
		}

		public static void PrepareDatapad(ref MyObjectBuilder_Datapad datapad, MyFaction faction, MyStation station)
		{
			datapad.Name = string.Format(MyTexts.GetString(MySpaceTexts.Datapad_GPS_Name), faction.Tag);
			string name = string.Format(MyTexts.GetString(MySpaceTexts.Datapad_GPS_Data), faction.Tag);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(MyTexts.GetString($"Datapad_Station_GPS_Content_{MyRandom.Instance.Next(0, 9)}"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(MyGps.ConvertToString(name, station.Position));
			datapad.Data = stringBuilder.ToString();
		}

		public SerializableDefinitionId GetDatapadDefinitionId()
		{
			return EconomyDefinition.DatapadDefinition;
		}

		public bool IsGridStation(long gridId)
		{
			if (!Sync.IsServer)
			{
				MyLog.Default.WriteToLogAndAssert("Checking if grid is station on client. Client has no such information.");
			}
			return m_stationIds.Contains(gridId);
		}

		public void AddStationGrid(long gridId)
		{
			if (!Sync.IsServer)
			{
				MyLog.Default.WriteToLogAndAssert("Adding grid into station collection on client. Client shouldn't do that");
			}
			if (m_stationIds.Contains(gridId))
			{
				MyLog.Default.WriteToLogAndAssert("Grid has already been added to station collection, should not happen again.");
			}
			else
			{
				m_stationIds.Add(gridId);
			}
		}

		public void RemoveStationGrid(long gridId)
		{
			if (!Sync.IsServer)
			{
				MyLog.Default.WriteToLogAndAssert("Removing grid from station collection on client. Client shouldn't do that");
			}
			if (m_stationIds.Contains(gridId))
			{
				m_stationIds.Remove(gridId);
			}
		}

		public MyDynamicAABBTree GetStationBlockTree(long planetId)
		{
			if (!m_stationOreBlockTrees.ContainsKey(planetId))
			{
				m_stationOreBlockTrees.Add(planetId, new MyDynamicAABBTree(Vector3.Zero, 0f));
			}
			return m_stationOreBlockTrees[planetId];
		}

		private void RemoveOreAroundStations()
		{
			Dictionary<long, List<Vector3D>> dictionary = new Dictionary<long, List<Vector3D>>();
			Dictionary<long, MyPlanet> dictionary2 = new Dictionary<long, MyPlanet>();
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				foreach (MyStation station in faction.Value.Stations)
				{
					if (station.Type == MyStationTypeEnum.Outpost)
					{
						MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(station.Position);
						if (closestPlanet != null)
						{
							if (!dictionary2.ContainsKey(closestPlanet.EntityId))
							{
								dictionary2.Add(closestPlanet.EntityId, closestPlanet);
								dictionary.Add(closestPlanet.EntityId, new List<Vector3D>());
							}
							dictionary[closestPlanet.EntityId].Add(station.Position);
						}
					}
				}
			}
			foreach (KeyValuePair<long, List<Vector3D>> item in dictionary)
			{
				MyDynamicAABBTree stationOreBlockTree = GetStationBlockTree(item.Key);
				foreach (Vector3D item2 in item.Value)
				{
					dictionary2[item.Key].AddToStationOreBlockTree(ref stationOreBlockTree, item2, ORE_AROUND_STATION_REMOVAL_RADIUS);
				}
				dictionary2[item.Key].SetStationOreBlockTree(stationOreBlockTree);
			}
		}

		internal string GetFactionFriendTooltip(long factionId)
		{
			if (m_factionFriends == null)
			{
				InitializeFactionFriendCollection();
			}
			if (m_factionFriendTooltips.ContainsKey(factionId))
			{
				return m_factionFriendTooltips[factionId];
			}
			return string.Empty;
		}

		private void InitializeFactionFriendCollection()
		{
			m_factionFriends = new Dictionary<long, List<long>>();
			m_factionFriendTooltips = new Dictionary<long, string>();
			Dictionary<long, StringBuilder> dictionary = new Dictionary<long, StringBuilder>();
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				if (!m_factionFriends.ContainsKey(faction.Value.FactionId) && MySession.Static.Factions.IsNpcFaction(faction.Value.Tag))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append($"{faction.Value.Name}\nFriends:");
					m_factionFriends.Add(faction.Value.FactionId, new List<long>());
					dictionary.Add(faction.Value.FactionId, stringBuilder);
				}
			}
			foreach (KeyValuePair<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> allFactionRelation in MySession.Static.Factions.GetAllFactionRelations())
			{
				if (allFactionRelation.Value.Item1 == MyRelationsBetweenFactions.Friends || allFactionRelation.Value.Item1 == MyRelationsBetweenFactions.Allies)
				{
					MyFaction myFaction = MySession.Static.Factions[allFactionRelation.Key.RelateeId1];
					MyFaction myFaction2 = MySession.Static.Factions[allFactionRelation.Key.RelateeId2];
					if (MySession.Static.Factions.IsNpcFaction(myFaction.Tag) && MySession.Static.Factions.IsNpcFaction(myFaction2.Tag))
					{
						m_factionFriends[myFaction.FactionId].Add(myFaction2.FactionId);
						m_factionFriends[myFaction2.FactionId].Add(myFaction.FactionId);
						dictionary[myFaction.FactionId].Append($"\n   [{myFaction2.Tag}] {myFaction2.Name}");
						dictionary[myFaction2.FactionId].Append($"\n   [{myFaction.Tag}] {myFaction.Name}");
					}
				}
			}
			foreach (KeyValuePair<long, StringBuilder> item in dictionary)
			{
				m_factionFriendTooltips.Add(item.Key, item.Value.ToString());
			}
		}
	}
}
