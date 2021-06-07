using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Contracts;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.Components.Contracts;
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
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 666, typeof(MyObjectBuilder_SessionComponentContractSystem), null)]
	public class MySessionComponentContractSystem : MySessionComponentBase, IMyContractSystem
	{
		private enum MyTransferItemsFromGridResults
		{
			Success,
			Fail_NoAccess,
			Fail_NotEnoughItems,
			Fail_NotEnoughSpace,
			Error_MissingKeyStructures
		}

		private struct MyContractStateChanged
		{
			public long Id;

			public MyContractStateEnum StateOld;

			public MyContractStateEnum StateNew;
		}

		protected sealed class DisplayNotificationToPlayer_003C_003ESandbox_Game_SessionComponents_MyContractNotificationTypes : ICallSite<IMyEventOwner, MyContractNotificationTypes, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyContractNotificationTypes notifType, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DisplayNotificationToPlayer(notifType);
			}
		}

		private static readonly int CONTRACT_CREATION_TRIES_MAX = 20;

		private static readonly MyDefinitionId FactionTypeId_Miner = new MyDefinitionId(typeof(MyObjectBuilder_FactionTypeDefinition), "Miner");

		private static readonly MyDefinitionId FactionTypeId_Trader = new MyDefinitionId(typeof(MyObjectBuilder_FactionTypeDefinition), "Trader");

		private static readonly MyDefinitionId FactionTypeId_Builder = new MyDefinitionId(typeof(MyObjectBuilder_FactionTypeDefinition), "Builder");

		private static readonly MyDefinitionId ContractTypeId_Hauling = new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Deliver");

		private static readonly MyDefinitionId ContractTypeId_Acquisition = new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "ObtainAndDeliver");

		private static readonly MyDefinitionId ContractTypeId_Escort = new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Escort");

		private static readonly MyDefinitionId ContractTypeId_Search = new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Find");

		private static readonly MyDefinitionId ContractTypeId_Bounty = new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Hunt");

		private static readonly MyDefinitionId ContractTypeId_Repair = new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Repair");

		private Dictionary<MyDefinitionId, MyContractTypeBaseStrategy> m_contractTypeStrategies = new Dictionary<MyDefinitionId, MyContractTypeBaseStrategy>();

		private MySessionComponentContractSystemDefinition m_definition;

		private int m_updateTimer;

		private int m_updatePeriod = 100;

		private Queue<MyContractStateChanged> m_pendingContractChanges = new Queue<MyContractStateChanged>();

		private Dictionary<long, MyContract> m_inactiveContracts = new Dictionary<long, MyContract>();

		private Dictionary<long, MyContract> m_activeContracts = new Dictionary<long, MyContract>();

		private MyMinimalPriceCalculator m_minimalPriceCalculator = new MyMinimalPriceCalculator();

		private Dictionary<MyDefinitionId, Dictionary<MyDefinitionId, float>> m_contractChanceCache = new Dictionary<MyDefinitionId, Dictionary<MyDefinitionId, float>>();

		private bool m_isContractChanceCacheInitialized;

		public Func<long, long, bool> CustomFinishCondition
		{
			get;
			set;
		}

		public Func<long, long, MyActivationCustomResults> CustomCanActivateContract
		{
			get;
			set;
		}

		public Func<long, bool> CustomNeedsUpdate
		{
			get;
			set;
		}

		public event MyContractConditionDelegate CustomConditionFinished;

		public event MyContractActivateDelegate CustomActivateContract;

		public event MyContractFailedDelegate CustomFailFor;

		public event MyContractFinishedDelegate CustomFinishFor;

		public event MyContractChangeDelegate CustomFinish;

		public event MyContractChangeDelegate CustomFail;

		public event MyContractChangeDelegate CustomCleanUp;

		public event MyContractChangeDelegate CustomTimeRanOut;

		public event MyContractUpdateDelegate CustomUpdate;

		public int GetContractLimitPerPlayer()
		{
			return 20;
		}

		public int GetContractCreationLimitPerPlayer()
		{
			return 20;
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			m_inactiveContracts.Clear();
			m_activeContracts.Clear();
			MyObjectBuilder_SessionComponentContractSystem myObjectBuilder_SessionComponentContractSystem = sessionComponent as MyObjectBuilder_SessionComponentContractSystem;
			if (myObjectBuilder_SessionComponentContractSystem != null && Sync.IsServer)
			{
				if (myObjectBuilder_SessionComponentContractSystem.InactiveContracts != null)
				{
					foreach (MyObjectBuilder_Contract inactiveContract in myObjectBuilder_SessionComponentContractSystem.InactiveContracts)
					{
						MyContract contract = MyContractFactory.CreateInstance(inactiveContract);
						AddContract(contract);
					}
				}
				if (myObjectBuilder_SessionComponentContractSystem.ActiveContracts != null)
				{
					foreach (MyObjectBuilder_Contract activeContract in myObjectBuilder_SessionComponentContractSystem.ActiveContracts)
					{
						MyContract contract2 = MyContractFactory.CreateInstance(activeContract);
						AddContract(contract2);
					}
				}
			}
		}

		private void UpdateActiveGpss(MyStation station)
		{
			foreach (KeyValuePair<long, MyContract> activeContract in m_activeContracts)
			{
				if (activeContract.Value.ContractCondition != null && activeContract.Value.ContractCondition.StationEndId == station.Id)
				{
					activeContract.Value.ReshareConditionWithAll();
				}
			}
		}

		private float GetContractChance(MyDefinitionId factionTypeId, MyDefinitionId contractTypeId)
		{
			InitializeContractChanceCache();
			if (!m_contractChanceCache.ContainsKey(factionTypeId))
			{
				return 0f;
			}
			Dictionary<MyDefinitionId, float> dictionary = m_contractChanceCache[factionTypeId];
			if (!dictionary.ContainsKey(contractTypeId))
			{
				return 0f;
			}
			return dictionary[contractTypeId];
		}

		private void InitializeContractChanceCache()
		{
			if (!m_isContractChanceCacheInitialized)
			{
				foreach (KeyValuePair<MyDefinitionId, MyContractTypeDefinition> contractTypeDefinition in MyDefinitionManager.Static.GetContractTypeDefinitions())
				{
					if (contractTypeDefinition.Value.ChancesPerFactionType != null)
					{
						foreach (KeyValuePair<SerializableDefinitionId, float> item in contractTypeDefinition.Value.ChancesPerFactionType)
						{
							if (!m_contractChanceCache.ContainsKey(item.Key))
							{
								m_contractChanceCache.Add(item.Key, new Dictionary<MyDefinitionId, float>());
							}
							m_contractChanceCache[item.Key].Add(contractTypeDefinition.Value.Id, item.Value);
						}
					}
				}
				m_isContractChanceCacheInitialized = true;
			}
		}

		private void FailContractsForBlock(long entityId, bool punishOwner = true)
		{
			MyContractBlock startBlock = MyEntities.GetEntityById(entityId, allowClosed: true) as MyContractBlock;
			List<long> list = new List<long>();
			foreach (KeyValuePair<long, MyContract> inactiveContract in m_inactiveContracts)
			{
				if (inactiveContract.Value.StartBlock > 0 && inactiveContract.Value.StartBlock == entityId)
				{
					inactiveContract.Value.RefundRewardOnDelete(startBlock);
					list.Add(inactiveContract.Value.Id);
				}
				else if (inactiveContract.Value.IsPlayerMade && inactiveContract.Value.ContractCondition != null && inactiveContract.Value.ContractCondition.BlockEndId != 0L && inactiveContract.Value.ContractCondition.BlockEndId == entityId)
				{
					MyContractBlock startBlock2 = MyEntities.GetEntityById(inactiveContract.Value.StartBlock) as MyContractBlock;
					inactiveContract.Value.RefundRewardOnDelete(startBlock2);
					list.Add(inactiveContract.Value.Id);
				}
			}
			foreach (long item in list)
			{
				m_inactiveContracts.Remove(item);
			}
			foreach (KeyValuePair<long, MyContract> activeContract in m_activeContracts)
			{
				if (activeContract.Value.IsPlayerMade && activeContract.Value.ContractCondition != null && activeContract.Value.ContractCondition.BlockEndId != 0L && activeContract.Value.ContractCondition.BlockEndId == entityId)
				{
					activeContract.Value.Fail(abandon: false, punishOwner: false);
				}
			}
		}

		public override void InitFromDefinition(MySessionComponentDefinition definition)
		{
			base.InitFromDefinition(definition);
			m_definition = (definition as MySessionComponentContractSystemDefinition);
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_SessionComponentContractSystem myObjectBuilder_SessionComponentContractSystem = base.GetObjectBuilder() as MyObjectBuilder_SessionComponentContractSystem;
			myObjectBuilder_SessionComponentContractSystem.InactiveContracts = new MySerializableList<MyObjectBuilder_Contract>();
			myObjectBuilder_SessionComponentContractSystem.ActiveContracts = new MySerializableList<MyObjectBuilder_Contract>();
			foreach (KeyValuePair<long, MyContract> inactiveContract in m_inactiveContracts)
			{
				myObjectBuilder_SessionComponentContractSystem.InactiveContracts.Add(inactiveContract.Value.GetObjectBuilder());
			}
			foreach (KeyValuePair<long, MyContract> activeContract in m_activeContracts)
			{
				myObjectBuilder_SessionComponentContractSystem.ActiveContracts.Add(activeContract.Value.GetObjectBuilder());
			}
			return myObjectBuilder_SessionComponentContractSystem;
		}

		public override void BeforeStart()
		{
			m_contractTypeStrategies.Add(ContractTypeId_Hauling, new MyContractTypeHaulingStrategy(MySession.Static.GetComponent<MySessionComponentEconomy>().EconomyDefinition));
			m_contractTypeStrategies.Add(ContractTypeId_Acquisition, new MyContractTypeAcquisitionStrategy(MySession.Static.GetComponent<MySessionComponentEconomy>().EconomyDefinition));
			m_contractTypeStrategies.Add(ContractTypeId_Escort, new MyContractTypeEscortStrategy(MySession.Static.GetComponent<MySessionComponentEconomy>().EconomyDefinition));
			m_contractTypeStrategies.Add(ContractTypeId_Search, new MyContractTypeSearchStrategy(MySession.Static.GetComponent<MySessionComponentEconomy>().EconomyDefinition));
			m_contractTypeStrategies.Add(ContractTypeId_Bounty, new MyContractTypeBountyStrategy(MySession.Static.GetComponent<MySessionComponentEconomy>().EconomyDefinition));
			m_contractTypeStrategies.Add(ContractTypeId_Repair, new MyContractTypeRepairStrategy(MySession.Static.GetComponent<MySessionComponentEconomy>().EconomyDefinition));
			foreach (KeyValuePair<long, MyContract> inactiveContract in m_inactiveContracts)
			{
				inactiveContract.Value.BeforeStart();
			}
			foreach (KeyValuePair<long, MyContract> activeContract in m_activeContracts)
			{
				activeContract.Value.BeforeStart();
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (MySession.Static.IsServer)
			{
				m_updateTimer--;
				if (m_updateTimer <= 0)
				{
					m_updateTimer = m_updatePeriod;
					MyTimeSpan currentTime = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
					foreach (KeyValuePair<long, MyContract> activeContract in m_activeContracts)
					{
						if (activeContract.Value.NeedsUpdate)
						{
							activeContract.Value.Update(currentTime);
						}
					}
				}
				while (m_pendingContractChanges.Count > 0)
				{
					MyContractStateChanged state = m_pendingContractChanges.Dequeue();
					ProcessContractStateChanges(state);
				}
			}
		}

		internal void StationGridSpawned(MyStation station)
		{
			UpdateActiveGpss(station);
		}

		internal void CleanOldContracts()
		{
			MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			List<long> list = new List<long>();
			foreach (KeyValuePair<long, MyContract> inactiveContract in m_inactiveContracts)
			{
				if (!inactiveContract.Value.IsPlayerMade && inactiveContract.Value.TicksToDiscard.HasValue)
				{
					inactiveContract.Value.DecreaseTicksToDiscard();
					if (inactiveContract.Value.TicksToDiscard <= 0)
					{
						list.Add(inactiveContract.Key);
					}
				}
			}
			foreach (long item in list)
			{
				m_inactiveContracts.Remove(item);
			}
		}

		internal void CreateContractsForStation(MyContractGenerator cGen, MyFaction faction, MyStation station, int currentContractCount, ref List<MyContract> existingContracts)
		{
			MyFactionTypeDefinition myFactionTypeDefinition = null;
			switch (faction.FactionType)
			{
			default:
				return;
			case MyFactionTypes.None:
			case MyFactionTypes.PlayerMade:
				return;
			case MyFactionTypes.Miner:
				myFactionTypeDefinition = MyDefinitionManager.Static.GetDefinition<MyFactionTypeDefinition>(FactionTypeId_Miner);
				break;
			case MyFactionTypes.Trader:
				myFactionTypeDefinition = MyDefinitionManager.Static.GetDefinition<MyFactionTypeDefinition>(FactionTypeId_Trader);
				break;
			case MyFactionTypes.Builder:
				myFactionTypeDefinition = MyDefinitionManager.Static.GetDefinition<MyFactionTypeDefinition>(FactionTypeId_Builder);
				break;
			}
			if (currentContractCount >= myFactionTypeDefinition.MaxContractCount)
			{
				return;
			}
			int num = myFactionTypeDefinition.MaxContractCount - currentContractCount;
			MyTimeSpan now = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			DictionaryReader<MyDefinitionId, MyContractTypeDefinition> contractTypeDefinitions = MyDefinitionManager.Static.GetContractTypeDefinitions();
			List<float> list = new List<float>();
			int num2 = 0;
			foreach (KeyValuePair<MyDefinitionId, MyContractTypeDefinition> item in contractTypeDefinitions)
			{
				float num3 = (num2 > 0) ? list[num2 - 1] : 0f;
				if (m_contractTypeStrategies.ContainsKey(item.Key))
				{
					MyContractTypeBaseStrategy myContractTypeBaseStrategy = m_contractTypeStrategies[item.Key];
					if (myContractTypeBaseStrategy != null && myContractTypeBaseStrategy.CanBeGenerated(station, faction))
					{
						num3 += GetContractChance(myFactionTypeDefinition.Id, item.Key);
					}
				}
				list.Add(num3);
				num2++;
			}
			float num4 = (list.Count > 0) ? list[list.Count - 1] : 0f;
			int num5 = CONTRACT_CREATION_TRIES_MAX;
			while (num5 > 0 && num > 0)
			{
				float num6 = MyRandom.Instance.NextFloat() * num4;
				MyContract contract = null;
				MyContractCreationResults myContractCreationResults = MyContractCreationResults.Error;
				int num7 = 0;
				foreach (KeyValuePair<MyDefinitionId, MyContractTypeDefinition> item2 in contractTypeDefinitions)
				{
					if (num6 < list[num7])
					{
						myContractCreationResults = m_contractTypeStrategies[item2.Key].GenerateContract(out contract, faction.FactionId, station.Id, m_minimalPriceCalculator, now);
						break;
					}
					num7++;
				}
				if (myContractCreationResults == MyContractCreationResults.Success && CheckContractAgainstOther(contract, existingContracts))
				{
					AddContract(contract);
					existingContracts.Add(contract);
					num--;
				}
				else
				{
					num5--;
				}
			}
		}

		internal bool CheckContractAgainstOther(MyContract testedContract, List<MyContract> contracts)
		{
			if (testedContract is MyContractHunt)
			{
				MyContractHunt myContractHunt = testedContract as MyContractHunt;
				foreach (MyContract contract in contracts)
				{
					MyContractHunt myContractHunt2 = contract as MyContractHunt;
					if (myContractHunt2 != null && myContractHunt.Target == myContractHunt2.Target)
					{
						return false;
					}
				}
				return true;
			}
			if (testedContract is MyContractObtainAndDeliver)
			{
				MyContractObtainAndDeliver myContractObtainAndDeliver = testedContract as MyContractObtainAndDeliver;
				foreach (MyContract contract2 in contracts)
				{
					MyContractObtainAndDeliver myContractObtainAndDeliver2 = contract2 as MyContractObtainAndDeliver;
					if (myContractObtainAndDeliver2 != null)
					{
						MyDefinitionId? myDefinitionId = null;
						MyDefinitionId? myDefinitionId2 = null;
						myDefinitionId = myContractObtainAndDeliver.GetItemId();
						myDefinitionId2 = myContractObtainAndDeliver2.GetItemId();
						if (myDefinitionId.HasValue && myDefinitionId2.HasValue && myDefinitionId.Value == myDefinitionId2.Value)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		internal void GetAvailableContractCountsByStation(ref Dictionary<long, int> counts, ref Dictionary<long, List<MyContract>> lists)
		{
			foreach (KeyValuePair<long, MyContract> inactiveContract in m_inactiveContracts)
			{
				if (inactiveContract.Value.StartStation > 0)
				{
					if (!lists.ContainsKey(inactiveContract.Value.StartStation))
					{
						lists.Add(inactiveContract.Value.StartStation, new List<MyContract>());
					}
					lists[inactiveContract.Value.StartStation].Add(inactiveContract.Value);
					if (!counts.ContainsKey(inactiveContract.Value.StartStation))
					{
						counts.Add(inactiveContract.Value.StartStation, 0);
					}
					counts[inactiveContract.Value.StartStation] = counts[inactiveContract.Value.StartStation] + 1;
				}
			}
		}

		internal void ContractBlockDestroyed(long entityId)
		{
			FailContractsForBlock(entityId, punishOwner: false);
		}

		internal MyContractResults ActivateContract(long identityId, long contractId, long stationId, long blockId)
		{
			if (!m_inactiveContracts.ContainsKey(contractId))
			{
				return MyContractResults.Fail_ContractNotFound_Activation;
			}
			MyContract myContract = m_inactiveContracts[contractId];
			if (myContract.StartStation != stationId && myContract.StartBlock != blockId)
			{
				return MyContractResults.Error_InvalidData;
			}
			switch (myContract.CanActivate(identityId))
			{
			case MyActivationResults.Fail_InsufficientFunds:
				return MyContractResults.Fail_ActivationConditionsNotMet_InsufficientFunds;
			case MyActivationResults.Fail_InsufficientInventorySpace:
				return MyContractResults.Fail_ActivationConditionsNotMet_InsufficientSpace;
			case MyActivationResults.Fail_ContractLimitReachedHard:
				return MyContractResults.Fail_ActivationConditionsNotMet_ContractLimitReachedHard;
			case MyActivationResults.Fail_TargetNotOnline:
				return MyContractResults.Fail_ActivationConditionsNotMet_TargetOffline;
			case MyActivationResults.Fail_YouAreTargetOfThisHunt:
				return MyContractResults.Fail_ActivationConditionsNotMet_YouAreTargetOfThisHunt;
			default:
				return MyContractResults.Fail_ActivationConditionsNotMet;
			case MyActivationResults.Success:
				if (myContract.Activate(identityId, MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds)))
				{
					m_inactiveContracts.Remove(myContract.Id);
					m_activeContracts.Add(myContract.Id, myContract);
					return MyContractResults.Success;
				}
				return MyContractResults.Error_Unknown;
			}
		}

		internal MyContractResults AbandonContract(long identityId, long contractId)
		{
			if (!m_activeContracts.ContainsKey(contractId))
			{
				return MyContractResults.Fail_ContractNotFound_Abandon;
			}
			MyContract myContract = m_activeContracts[contractId];
			if (!myContract.Owners.Contains(identityId))
			{
				return MyContractResults.Error_InvalidData;
			}
			myContract.Abandon(identityId);
			return MyContractResults.Success;
		}

		internal MyContractResults FinishContractCondition(long identityId, MyContract contract, MyContractCondition condition, long targetEntityId)
		{
			if (!m_activeContracts.ContainsKey(condition.ContractId))
			{
				return MyContractResults.Fail_ContractNotFound_Finish;
			}
			if (condition is MyContractConditionDeliverPackage)
			{
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
				if (myIdentity == null)
				{
					MyLog.Default.WriteToLogAndAssert("MyContractBlock - identity not found");
					return MyContractResults.Error_MissingKeyStructure;
				}
				MyCharacter character = myIdentity.Character;
				if (character == null)
				{
					MyLog.Default.WriteToLogAndAssert("MyContractBlock - character not found");
					return MyContractResults.Error_MissingKeyStructure;
				}
				if (character.InventoryCount <= 0)
				{
					MyLog.Default.WriteToLogAndAssert("MyContractBlock - no character inventory");
					return MyContractResults.Error_MissingKeyStructure;
				}
				MyInventoryBase inventoryBase = character.GetInventoryBase();
				List<MyPhysicalInventoryItem> items = inventoryBase.GetItems();
				MyPhysicalInventoryItem? myPhysicalInventoryItem = null;
				foreach (MyPhysicalInventoryItem item in items)
				{
					MyObjectBuilder_Package myObjectBuilder_Package = item.Content as MyObjectBuilder_Package;
					if (myObjectBuilder_Package != null && myObjectBuilder_Package.ContractId == condition.ContractId && myObjectBuilder_Package.ContractConditionId == condition.Id)
					{
						myPhysicalInventoryItem = item;
						break;
					}
				}
				if (!myPhysicalInventoryItem.HasValue)
				{
					return MyContractResults.Fail_FinishConditionsNotMet_MissingPackage;
				}
				inventoryBase.Remove(myPhysicalInventoryItem, 1);
				condition.FinalizeCondition();
				return MyContractResults.Success;
			}
			MyContractConditionDeliverItems myContractConditionDeliverItems;
			if ((myContractConditionDeliverItems = (condition as MyContractConditionDeliverItems)) != null)
			{
				if (targetEntityId == 0L || MyEntities.GetEntityById(targetEntityId) == null)
				{
					return MyContractResults.Fail_FinishConditionsNotMet_IncorrectTargetEntity;
				}
				MyTransferItemsFromGridResults myTransferItemsFromGridResults;
				if (myContractConditionDeliverItems.TransferItems)
				{
					MyStation stationByStationId = MySession.Static.Factions.GetStationByStationId(myContractConditionDeliverItems.StationEndId);
					MyContractBlock myContractBlock = null;
					if (stationByStationId != null)
					{
						MyCubeGrid myCubeGrid = MyEntities.GetEntityById(stationByStationId.StationEntityId) as MyCubeGrid;
						if (myCubeGrid != null)
						{
							myContractBlock = myCubeGrid.GetFirstBlockOfType<MyContractBlock>();
						}
					}
					else
					{
						myContractBlock = (MyEntities.GetEntityById(myContractConditionDeliverItems.BlockEndId) as MyContractBlock);
					}
					if (myContractBlock == null)
					{
						MyLog.Default.WriteToLogAndAssert("Acquisition contract - Cannot transfer as there is nowhere to transfer to (target lacks Contract block)");
						return MyContractResults.Error_MissingKeyStructure;
					}
					myTransferItemsFromGridResults = TransferItemsFromEntity(myContractConditionDeliverItems.ItemType, myContractConditionDeliverItems.ItemAmount, targetEntityId, myContractBlock);
				}
				else
				{
					myTransferItemsFromGridResults = DeleteItemsFromEntity(myContractConditionDeliverItems.ItemType, myContractConditionDeliverItems.ItemAmount, targetEntityId);
				}
				switch (myTransferItemsFromGridResults)
				{
				case MyTransferItemsFromGridResults.Success:
					condition.FinalizeCondition();
					return MyContractResults.Success;
				case MyTransferItemsFromGridResults.Fail_NoAccess:
					return MyContractResults.Fail_CannotAccess;
				case MyTransferItemsFromGridResults.Fail_NotEnoughItems:
					return MyContractResults.Fail_FinishConditionsNotMet_NotEnoughItems;
				case MyTransferItemsFromGridResults.Fail_NotEnoughSpace:
					return MyContractResults.Fail_FinishConditionsNotMet_NotEnoughSpace;
				case MyTransferItemsFromGridResults.Error_MissingKeyStructures:
					return MyContractResults.Error_MissingKeyStructure;
				default:
					return MyContractResults.Error_Unknown;
				}
			}
			MyContractConditionCustom myContractConditionCustom;
			if ((myContractConditionCustom = (condition as MyContractConditionCustom)) != null)
			{
				bool flag = true;
				if (CustomFinishCondition != null)
				{
					flag |= CustomFinishCondition(condition.Id, condition.ContractId);
				}
				if (!flag)
				{
					return MyContractResults.Fail_NotPossible;
				}
				myContractConditionCustom.FinalizeCondition();
				if (this.CustomConditionFinished != null)
				{
					this.CustomConditionFinished(condition.Id, condition.ContractId);
				}
				return MyContractResults.Success;
			}
			return MyContractResults.Error_Unknown;
		}

		internal MyContractCreationResults GenerateCustomContract_Deliver(MyContractBlock startBlock, int rewardMoney, int startingDeposit, int durationInMin, long targetBlockId, out long contractId, out long contractConditionId)
		{
			contractId = 0L;
			contractConditionId = 0L;
			MySessionComponentEconomy mySessionComponentEconomy = MySession.Static?.GetComponent<MySessionComponentEconomy>();
			if (mySessionComponentEconomy == null)
			{
				return MyContractCreationResults.Error_MissingKeyStructure;
			}
			MyContractGenerator myContractGenerator = new MyContractGenerator(mySessionComponentEconomy.EconomyDefinition);
			MyTimeSpan now = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			if (MyBankingSystem.GetBalance(startBlock.OwnerId) < rewardMoney)
			{
				return MyContractCreationResults.Fail_NotEnoughFunds;
			}
			MyContract contract;
			MyContractCreationResults num = myContractGenerator.CreateCustomHaulingContract(out contract, startBlock, rewardMoney, startingDeposit, durationInMin, targetBlockId, now);
			if (num == MyContractCreationResults.Success)
			{
				AddContract(contract);
				contractId = contract.Id;
				contractConditionId = ((contract.ContractCondition != null) ? contract.ContractCondition.Id : 0);
				MyBankingSystem.ChangeBalance(startBlock.OwnerId, -rewardMoney);
			}
			return num;
		}

		internal MyContractCreationResults GenerateCustomContract_ObtainAndDeliver(MyContractBlock startBlock, int rewardMoney, int startingDeposit, int durationInMin, long targetBlockId, MyDefinitionId itemTypeId, int itemAmount, out long contractId, out long contractConditionId)
		{
			contractId = 0L;
			contractConditionId = 0L;
			MySessionComponentEconomy mySessionComponentEconomy = MySession.Static?.GetComponent<MySessionComponentEconomy>();
			if (mySessionComponentEconomy == null)
			{
				return MyContractCreationResults.Error_MissingKeyStructure;
			}
			MyContractGenerator myContractGenerator = new MyContractGenerator(mySessionComponentEconomy.EconomyDefinition);
			MyTimeSpan now = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			if (MyBankingSystem.GetBalance(startBlock.OwnerId) < rewardMoney)
			{
				return MyContractCreationResults.Fail_NotEnoughFunds;
			}
			MyContract contract;
			MyContractCreationResults num = myContractGenerator.CreateCustomAcquisitionContract(out contract, startBlock, rewardMoney, startingDeposit, durationInMin, targetBlockId, itemTypeId, itemAmount, now);
			if (num == MyContractCreationResults.Success)
			{
				AddContract(contract);
				contractId = contract.Id;
				contractConditionId = ((contract.ContractCondition != null) ? contract.ContractCondition.Id : 0);
				MyBankingSystem.ChangeBalance(startBlock.OwnerId, -rewardMoney);
			}
			return num;
		}

		internal MyContractCreationResults GenerateCustomContract_Find(MyContractBlock startBlock, int rewardMoney, int startingDeposit, int durationInMin, long targetGridId, double searchRadius, out long contractId, out long contractConditionId)
		{
			contractId = 0L;
			contractConditionId = 0L;
			MySessionComponentEconomy mySessionComponentEconomy = MySession.Static?.GetComponent<MySessionComponentEconomy>();
			if (mySessionComponentEconomy == null)
			{
				return MyContractCreationResults.Error_MissingKeyStructure;
			}
			MyContractGenerator myContractGenerator = new MyContractGenerator(mySessionComponentEconomy.EconomyDefinition);
			MyTimeSpan now = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			if (MyBankingSystem.GetBalance(startBlock.OwnerId) < rewardMoney)
			{
				return MyContractCreationResults.Fail_NotEnoughFunds;
			}
			MyContract contract;
			MyContractCreationResults num = myContractGenerator.CreateCustomSearchContract(out contract, startBlock, rewardMoney, startingDeposit, durationInMin, targetGridId, searchRadius, now);
			if (num == MyContractCreationResults.Success)
			{
				AddContract(contract);
				contractId = contract.Id;
				contractConditionId = ((contract.ContractCondition != null) ? contract.ContractCondition.Id : 0);
				MyBankingSystem.ChangeBalance(startBlock.OwnerId, -rewardMoney);
			}
			return num;
		}

		internal MyContractCreationResults GenerateCustomContract_Escort(MyContractBlock startBlock, int rewardMoney, int startingDeposit, int durationInMin, Vector3D startPoint, Vector3D endPoint, long owner, out long contractId, out long contractConditionId)
		{
			contractId = 0L;
			contractConditionId = 0L;
			MySessionComponentEconomy mySessionComponentEconomy = MySession.Static?.GetComponent<MySessionComponentEconomy>();
			if (mySessionComponentEconomy == null)
			{
				return MyContractCreationResults.Error_MissingKeyStructure;
			}
			MyContractGenerator myContractGenerator = new MyContractGenerator(mySessionComponentEconomy.EconomyDefinition);
			MyTimeSpan now = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			if (MyBankingSystem.GetBalance(startBlock.OwnerId) < rewardMoney)
			{
				return MyContractCreationResults.Fail_NotEnoughFunds;
			}
			MyContract contract;
			MyContractCreationResults num = myContractGenerator.CreateCustomEscortContract(out contract, startBlock, rewardMoney, startingDeposit, durationInMin, startPoint, endPoint, owner, now);
			if (num == MyContractCreationResults.Success)
			{
				AddContract(contract);
				contractId = contract.Id;
				contractConditionId = ((contract.ContractCondition != null) ? contract.ContractCondition.Id : 0);
				MyBankingSystem.ChangeBalance(startBlock.OwnerId, -rewardMoney);
			}
			return num;
		}

		internal MyContractCreationResults GenerateCustomContract_Hunt(MyContractBlock startBlock, int rewardMoney, int startingDeposit, int durationInMin, long targetIdentityId, out long contractId, out long contractConditionId)
		{
			contractId = 0L;
			contractConditionId = 0L;
			MySessionComponentEconomy mySessionComponentEconomy = MySession.Static?.GetComponent<MySessionComponentEconomy>();
			if (mySessionComponentEconomy == null)
			{
				return MyContractCreationResults.Error_MissingKeyStructure;
			}
			MyContractGenerator myContractGenerator = new MyContractGenerator(mySessionComponentEconomy.EconomyDefinition);
			MyTimeSpan now = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			if (MyBankingSystem.GetBalance(startBlock.OwnerId) < rewardMoney)
			{
				return MyContractCreationResults.Fail_NotEnoughFunds;
			}
			MyContract contract;
			MyContractCreationResults num = myContractGenerator.CreateCustomBountyContract(out contract, startBlock, rewardMoney, startingDeposit, durationInMin, targetIdentityId, now);
			if (num == MyContractCreationResults.Success)
			{
				AddContract(contract);
				contractId = contract.Id;
				contractConditionId = ((contract.ContractCondition != null) ? contract.ContractCondition.Id : 0);
				MyBankingSystem.ChangeBalance(startBlock.OwnerId, -rewardMoney);
			}
			return num;
		}

		internal MyContractCreationResults GenerateCustomContract_Repair(MyContractBlock startBlock, int rewardMoney, int startingDeposit, int durationInMin, long gridId, out long contractId, out long contractConditionId)
		{
			contractId = 0L;
			contractConditionId = 0L;
			MySessionComponentEconomy mySessionComponentEconomy = MySession.Static?.GetComponent<MySessionComponentEconomy>();
			if (mySessionComponentEconomy == null)
			{
				return MyContractCreationResults.Error_MissingKeyStructure;
			}
			MyContractGenerator myContractGenerator = new MyContractGenerator(mySessionComponentEconomy.EconomyDefinition);
			MyTimeSpan now = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			if (MyBankingSystem.GetBalance(startBlock.OwnerId) < rewardMoney)
			{
				return MyContractCreationResults.Fail_NotEnoughFunds;
			}
			MyContract contract;
			MyContractCreationResults num = myContractGenerator.CreateCustomRepairContract(out contract, startBlock, rewardMoney, startingDeposit, durationInMin, gridId, now);
			if (num == MyContractCreationResults.Success)
			{
				AddContract(contract);
				contractId = contract.Id;
				contractConditionId = ((contract.ContractCondition != null) ? contract.ContractCondition.Id : 0);
				MyBankingSystem.ChangeBalance(startBlock.OwnerId, -rewardMoney);
			}
			return num;
		}

		internal MyContractCreationResults GenerateCustomContract_Custom(MyDefinitionId definitionId, string name, string description, MyContractBlock startBlock, int rewardMoney, int startingDeposit, int reputationReward, int failReputationPrice, int durationInMin, out long contractId, out long contractConditionId, MyContractBlock endBlock = null)
		{
			contractId = 0L;
			contractConditionId = 0L;
			MySessionComponentEconomy mySessionComponentEconomy = MySession.Static?.GetComponent<MySessionComponentEconomy>();
			if (mySessionComponentEconomy == null)
			{
				return MyContractCreationResults.Error_MissingKeyStructure;
			}
			MyContractGenerator myContractGenerator = new MyContractGenerator(mySessionComponentEconomy.EconomyDefinition);
			MyTimeSpan now = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			if (MyBankingSystem.GetBalance(startBlock.OwnerId) < rewardMoney)
			{
				return MyContractCreationResults.Fail_NotEnoughFunds;
			}
			MyContract contract;
			MyContractCreationResults num = myContractGenerator.CreateCustomCustomContract(out contract, definitionId, name, description, startBlock, rewardMoney, startingDeposit, reputationReward, failReputationPrice, durationInMin, now, endBlock);
			if (num == MyContractCreationResults.Success)
			{
				AddContract(contract);
				contractId = contract.Id;
				contractConditionId = ((contract.ContractCondition != null) ? contract.ContractCondition.Id : 0);
				MyBankingSystem.ChangeBalance(startBlock.OwnerId, -rewardMoney);
			}
			return num;
		}

		internal bool DeleteCustomContract(MyContractBlock startBlock, long contractId)
		{
			MyContract inactiveContractById = GetInactiveContractById(contractId);
			if (inactiveContractById == null || inactiveContractById.State != 0 || !inactiveContractById.IsPlayerMade || inactiveContractById.StartStation != 0L || inactiveContractById.StartBlock == 0L)
			{
				return false;
			}
			inactiveContractById.RefundRewardOnDelete(startBlock);
			RemoveInactiveContract(contractId);
			return true;
		}

		internal void OnCustomActivateContract(long contractId, long identityId)
		{
			this.CustomActivateContract?.Invoke(contractId, identityId);
		}

		internal void OnCustomFailFor(long contractId, long identityId, bool isAbandon)
		{
			this.CustomFailFor?.Invoke(contractId, identityId, isAbandon);
		}

		internal void OnCustomFinishFor(long contractId, long identityId, int rewardeeCount)
		{
			this.CustomFinishFor?.Invoke(contractId, identityId, rewardeeCount);
		}

		internal void OnCustomFinish(long contractId)
		{
			this.CustomFinish?.Invoke(contractId);
		}

		internal void OnCustomFail(long contractId)
		{
			this.CustomFail?.Invoke(contractId);
		}

		internal void OnCustomCleanUp(long contractId)
		{
			this.CustomCleanUp?.Invoke(contractId);
		}

		internal void OnCustomTimeRanOut(long contractId)
		{
			this.CustomTimeRanOut?.Invoke(contractId);
		}

		internal void OnCustomUpdate(long contractId, MyCustomContractStateEnum state, MyTimeSpan currentTime)
		{
			this.CustomUpdate?.Invoke(contractId, state, currentTime);
		}

		private MyTransferItemsFromGridResults TransferItemsFromEntity(MyDefinitionId id, int amount, long entityId, MyContractBlock targetContractBlock)
		{
			if (targetContractBlock == null)
			{
				return MyTransferItemsFromGridResults.Error_MissingKeyStructures;
			}
			MyEntity entityById = MyEntities.GetEntityById(entityId);
			MyCubeGrid myCubeGrid = entityById as MyCubeGrid;
			MyCharacter myCharacter = entityById as MyCharacter;
			if (myCubeGrid != null)
			{
				List<MyCubeGrid> groupNodes = MyCubeGridGroups.Static.GetGroups(GridLinkTypeEnum.Logical).GetGroupNodes(myCubeGrid);
				if (groupNodes == null || groupNodes.Count == 0)
				{
					return MyTransferItemsFromGridResults.Error_MissingKeyStructures;
				}
				List<MyInventory> list = new List<MyInventory>();
				foreach (MyCubeGrid item in groupNodes)
				{
					foreach (MySlimBlock cubeBlock in item.CubeBlocks)
					{
						if (cubeBlock.FatBlock != null && (cubeBlock.FatBlock is MyCargoContainer || cubeBlock.FatBlock is MyShipConnector || cubeBlock.FatBlock is MyRefinery || cubeBlock.FatBlock is MyAssembler))
						{
							for (int i = 0; i < cubeBlock.FatBlock.InventoryCount; i++)
							{
								list.Add(cubeBlock.FatBlock.GetInventory(i));
							}
						}
					}
				}
				int num = amount;
				foreach (MyInventory item2 in list)
				{
					num -= item2.GetItemAmount(id).ToIntSafe();
					if (num <= 0)
					{
						break;
					}
				}
				if (num > 0)
				{
					return MyTransferItemsFromGridResults.Fail_NotEnoughItems;
				}
				if (!targetContractBlock.CubeGrid.GridSystems.ConveyorSystem.PushGenerateItem(id, amount, targetContractBlock, partialPush: false))
				{
					return MyTransferItemsFromGridResults.Fail_NotEnoughSpace;
				}
				num = amount;
				foreach (MyInventory item3 in list)
				{
					MyFixedPoint itemAmount = item3.GetItemAmount(id);
					if (itemAmount < num)
					{
						item3.RemoveItemsOfType(itemAmount, id);
						num -= itemAmount.ToIntSafe();
					}
					else
					{
						item3.RemoveItemsOfType(num, id);
						num = 0;
					}
					if (num <= 0)
					{
						break;
					}
				}
				return MyTransferItemsFromGridResults.Success;
			}
			if (myCharacter != null)
			{
				if (myCharacter.InventoryCount <= 0)
				{
					return MyTransferItemsFromGridResults.Error_MissingKeyStructures;
				}
				MyInventoryBase inventoryBase = myCharacter.GetInventoryBase();
				if (inventoryBase.GetItemAmount(id) < amount)
				{
					return MyTransferItemsFromGridResults.Fail_NotEnoughItems;
				}
				if (!targetContractBlock.CubeGrid.GridSystems.ConveyorSystem.PushGenerateItem(id, amount, targetContractBlock, partialPush: false))
				{
					return MyTransferItemsFromGridResults.Fail_NotEnoughSpace;
				}
				inventoryBase.RemoveItemsOfType(amount, id);
				return MyTransferItemsFromGridResults.Success;
			}
			return MyTransferItemsFromGridResults.Error_MissingKeyStructures;
		}

		private MyTransferItemsFromGridResults DeleteItemsFromEntity(MyDefinitionId id, int amount, long entityId)
		{
			MyEntity entityById = MyEntities.GetEntityById(entityId);
			MyCubeGrid myCubeGrid = entityById as MyCubeGrid;
			MyCharacter myCharacter = entityById as MyCharacter;
			if (myCubeGrid != null)
			{
				MyCubeGrid myCubeGrid2 = MyEntities.GetEntityById(entityId) as MyCubeGrid;
				if (myCubeGrid2 == null)
				{
					return MyTransferItemsFromGridResults.Error_MissingKeyStructures;
				}
				List<MyCubeGrid> groupNodes = MyCubeGridGroups.Static.GetGroups(GridLinkTypeEnum.Logical).GetGroupNodes(myCubeGrid2);
				if (groupNodes == null || groupNodes.Count == 0)
				{
					return MyTransferItemsFromGridResults.Error_MissingKeyStructures;
				}
				List<MyInventory> list = new List<MyInventory>();
				foreach (MyCubeGrid item in groupNodes)
				{
					foreach (MySlimBlock cubeBlock in item.CubeBlocks)
					{
						if (cubeBlock.FatBlock != null && (cubeBlock.FatBlock is MyCargoContainer || cubeBlock.FatBlock is MyShipConnector || cubeBlock.FatBlock is MyRefinery || cubeBlock.FatBlock is MyAssembler))
						{
							for (int i = 0; i < cubeBlock.FatBlock.InventoryCount; i++)
							{
								list.Add(cubeBlock.FatBlock.GetInventory(i));
							}
						}
					}
				}
				int num = amount;
				foreach (MyInventory item2 in list)
				{
					num -= item2.GetItemAmount(id).ToIntSafe();
					if (num <= 0)
					{
						break;
					}
				}
				if (num > 0)
				{
					return MyTransferItemsFromGridResults.Fail_NotEnoughItems;
				}
				num = amount;
				foreach (MyInventory item3 in list)
				{
					MyFixedPoint itemAmount = item3.GetItemAmount(id);
					if (itemAmount < num)
					{
						item3.RemoveItemsOfType(itemAmount, id);
						num -= itemAmount.ToIntSafe();
					}
					else
					{
						item3.RemoveItemsOfType(num, id);
						num = 0;
					}
					if (num <= 0)
					{
						break;
					}
				}
				return MyTransferItemsFromGridResults.Success;
			}
			if (myCharacter != null)
			{
				if (myCharacter.InventoryCount <= 0)
				{
					return MyTransferItemsFromGridResults.Error_MissingKeyStructures;
				}
				MyInventoryBase inventoryBase = myCharacter.GetInventoryBase();
				if (inventoryBase.GetItemAmount(id) < amount)
				{
					return MyTransferItemsFromGridResults.Fail_NotEnoughItems;
				}
				inventoryBase.RemoveItemsOfType(amount, id);
				return MyTransferItemsFromGridResults.Success;
			}
			return MyTransferItemsFromGridResults.Error_MissingKeyStructures;
		}

		internal MyContractResults FinishContract(long identityId, MyContract contract)
		{
			if (!m_activeContracts.ContainsKey(contract.Id))
			{
				return MyContractResults.Fail_ContractNotFound_Finish;
			}
			if (!contract.Owners.Contains(identityId))
			{
				return MyContractResults.Fail_CannotAccess;
			}
			contract.Finish();
			return MyContractResults.Success;
		}

		internal List<MyObjectBuilder_Contract> GetActiveContractsForPlayer_OB(long identityId)
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			if (myIdentity == null)
			{
				return null;
			}
			List<MyObjectBuilder_Contract> list = new List<MyObjectBuilder_Contract>();
			foreach (long activeContract in myIdentity.ActiveContracts)
			{
				if (m_activeContracts.ContainsKey(activeContract))
				{
					MyContract myContract = m_activeContracts[activeContract];
					if (myContract.State == MyContractStateEnum.Active)
					{
						list.Add(myContract.GetObjectBuilder());
					}
				}
			}
			return list;
		}

		internal List<MyObjectBuilder_Contract> GetAvailableContractsForStation_OB(long stationId)
		{
			List<MyObjectBuilder_Contract> list = new List<MyObjectBuilder_Contract>();
			foreach (KeyValuePair<long, MyContract> inactiveContract in m_inactiveContracts)
			{
				if (inactiveContract.Value.StartStation == stationId && inactiveContract.Value.State == MyContractStateEnum.Inactive)
				{
					list.Add(inactiveContract.Value.GetObjectBuilder());
				}
			}
			return list;
		}

		internal List<MyObjectBuilder_Contract> GetAvailableContractsForBlock_OB(long blockId)
		{
			List<MyObjectBuilder_Contract> list = new List<MyObjectBuilder_Contract>();
			foreach (KeyValuePair<long, MyContract> inactiveContract in m_inactiveContracts)
			{
				if (inactiveContract.Value.StartBlock == blockId && inactiveContract.Value.State == MyContractStateEnum.Inactive)
				{
					list.Add(inactiveContract.Value.GetObjectBuilder());
				}
			}
			return list;
		}

		private void AddContract(MyContract contract)
		{
			AddContract(contract, contract.State);
		}

		private void AddContract(MyContract contract, MyContractStateEnum state)
		{
			if (m_activeContracts.ContainsKey(contract.Id) || m_inactiveContracts.ContainsKey(contract.Id))
			{
				MyLog.Default.WriteToLogAndAssert("ContractSystem - Adding Contract that have already been added!!! There is something seriously wrong.");
			}
			switch (state)
			{
			case MyContractStateEnum.Inactive:
				m_inactiveContracts.Add(contract.Id, contract);
				contract.ContractChangedState = (Action<long, MyContractStateEnum, MyContractStateEnum>)Delegate.Combine(contract.ContractChangedState, new Action<long, MyContractStateEnum, MyContractStateEnum>(ContractChangedState_Callback));
				break;
			case MyContractStateEnum.Active:
			case MyContractStateEnum.Finished:
			case MyContractStateEnum.Failed:
			case MyContractStateEnum.ToBeDisposed:
				m_activeContracts.Add(contract.Id, contract);
				contract.ContractChangedState = (Action<long, MyContractStateEnum, MyContractStateEnum>)Delegate.Combine(contract.ContractChangedState, new Action<long, MyContractStateEnum, MyContractStateEnum>(ContractChangedState_Callback));
				break;
			default:
				MyLog.Default.WriteToLogAndAssert("ContractSystem - Cannot add contract with such state: " + contract.State);
				break;
			}
		}

		private void ContractChangedState_Callback(long id, MyContractStateEnum stateOld, MyContractStateEnum stateNew)
		{
			m_pendingContractChanges.Enqueue(new MyContractStateChanged
			{
				Id = id,
				StateOld = stateOld,
				StateNew = stateNew
			});
		}

		private void ProcessContractStateChanges(MyContractStateChanged state)
		{
			MyContract myContract = null;
			switch (state.StateOld)
			{
			case MyContractStateEnum.Inactive:
				return;
			case MyContractStateEnum.Active:
			case MyContractStateEnum.Finished:
			case MyContractStateEnum.Failed:
			case MyContractStateEnum.ToBeDisposed:
				myContract = m_activeContracts[state.Id];
				RemoveActiveContract(state.Id);
				break;
			default:
				MyLog.Default.WriteToLogAndAssert("ContractSystem - contract in such state (" + state.StateOld.ToString() + " cannot have changed state as it is no longer within any collection and should not call this function.");
				break;
			}
			if (myContract != null)
			{
				MyContractStateEnum stateNew = state.StateNew;
				if ((uint)stateNew > 4u)
				{
					_ = 5;
				}
				else
				{
					AddContract(myContract, state.StateNew);
				}
			}
		}

		private void RemoveContractInternal(long contractId)
		{
			RemoveActiveContract(contractId);
			RemoveInactiveContract(contractId);
		}

		private void RemoveActiveContract(long contractId)
		{
			if (m_activeContracts.ContainsKey(contractId))
			{
				MyContract myContract = m_activeContracts[contractId];
				m_activeContracts.Remove(contractId);
				myContract.ContractChangedState = (Action<long, MyContractStateEnum, MyContractStateEnum>)Delegate.Remove(myContract.ContractChangedState, new Action<long, MyContractStateEnum, MyContractStateEnum>(ContractChangedState_Callback));
			}
		}

		private void RemoveInactiveContract(long contractId)
		{
			if (m_inactiveContracts.ContainsKey(contractId))
			{
				MyContract myContract = m_inactiveContracts[contractId];
				m_inactiveContracts.Remove(contractId);
				myContract.ContractChangedState = (Action<long, MyContractStateEnum, MyContractStateEnum>)Delegate.Remove(myContract.ContractChangedState, new Action<long, MyContractStateEnum, MyContractStateEnum>(ContractChangedState_Callback));
			}
		}

		internal MyContract GetActiveContractById(long contractId)
		{
			if (m_activeContracts.ContainsKey(contractId))
			{
				return m_activeContracts[contractId];
			}
			return null;
		}

		internal MyContract GetInactiveContractById(long contractId)
		{
			if (m_inactiveContracts.ContainsKey(contractId))
			{
				return m_inactiveContracts[contractId];
			}
			return null;
		}

		public void SendNotificationToPlayer(MyContractNotificationTypes notifType, long targetIdentityId)
		{
			if (MySession.Static.LocalPlayerId == targetIdentityId)
			{
				DisplayNotificationToPlayer_Internal(notifType);
				return;
			}
			ulong value = MySession.Static.Players.TryGetSteamId(targetIdentityId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => DisplayNotificationToPlayer, notifType, new EndpointId(value));
		}

		[Event(null, 1518)]
		[Reliable]
		[Client]
		private static void DisplayNotificationToPlayer(MyContractNotificationTypes notifType)
		{
			DisplayNotificationToPlayer_Internal(notifType);
		}

		private static void DisplayNotificationToPlayer_Internal(MyContractNotificationTypes notifType)
		{
			MyHudNotification myHudNotification = null;
			switch (notifType)
			{
			case MyContractNotificationTypes.ContractSuccessful:
				myHudNotification = new MyHudNotification(MySpaceTexts.ContractSystem_Notifications_ContractSuccess, 3500, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
				break;
			case MyContractNotificationTypes.ContractFailed:
				myHudNotification = new MyHudNotification(MySpaceTexts.ContractSystem_Notifications_ContractFailed, 3500, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
				break;
			default:
				myHudNotification = null;
				break;
			}
			if (myHudNotification != null)
			{
				MyHud.Notifications.Add(myHudNotification);
			}
		}

		public MyAddContractResultWrapper AddContract(IMyContract contract)
		{
			MyAddContractResultWrapper myAddContractResultWrapper = default(MyAddContractResultWrapper);
			myAddContractResultWrapper.Success = false;
			myAddContractResultWrapper.ContractId = 0L;
			MyAddContractResultWrapper result = myAddContractResultWrapper;
			if (contract == null)
			{
				return result;
			}
			IMyContractHauling haul;
			IMyContractAcquisition acqui;
			IMyContractSearch search;
			IMyContractEscort escort;
			IMyContractBounty bounty;
			IMyContractRepair repair;
			IMyContractCustom custom;
			if ((haul = (contract as IMyContractHauling)) != null)
			{
				result = AddContract_Hauling(haul);
			}
			else if ((acqui = (contract as IMyContractAcquisition)) != null)
			{
				result = AddContract_Acquisition(acqui);
			}
			else if ((search = (contract as IMyContractSearch)) != null)
			{
				result = AddContract_Search(search);
			}
			else if ((escort = (contract as IMyContractEscort)) != null)
			{
				result = AddContract_Escort(escort);
			}
			else if ((bounty = (contract as IMyContractBounty)) != null)
			{
				result = AddContract_Bounty(bounty);
			}
			else if ((repair = (contract as IMyContractRepair)) != null)
			{
				result = AddContract_Repair(repair);
			}
			else if ((custom = (contract as IMyContractCustom)) != null)
			{
				result = AddContract_Custom(custom);
			}
			if (result.Success && m_inactiveContracts.ContainsKey(result.ContractId))
			{
				MyContract myContract = m_inactiveContracts[result.ContractId];
				if (contract.OnContractAcquired != null)
				{
					myContract.OnContractAcquired = (Action<long>)Delegate.Combine(myContract.OnContractAcquired, contract.OnContractAcquired);
				}
				if (contract.OnContractFailed != null)
				{
					myContract.OnContractFailed = (Action)Delegate.Combine(myContract.OnContractFailed, contract.OnContractFailed);
				}
				if (contract.OnContractSucceeded != null)
				{
					myContract.OnContractSucceeded = (Action)Delegate.Combine(myContract.OnContractSucceeded, contract.OnContractSucceeded);
				}
			}
			return result;
		}

		private MyAddContractResultWrapper AddContract_Hauling(IMyContractHauling haul)
		{
			MyAddContractResultWrapper myAddContractResultWrapper = default(MyAddContractResultWrapper);
			myAddContractResultWrapper.Success = false;
			myAddContractResultWrapper.ContractId = 0L;
			MyAddContractResultWrapper result = myAddContractResultWrapper;
			MyContractBlock myContractBlock = MyEntities.GetEntityById(haul.StartBlockId) as MyContractBlock;
			if (myContractBlock == null)
			{
				return result;
			}
			MyContractCreationResults myContractCreationResults = GenerateCustomContract_Deliver(myContractBlock, haul.MoneyReward, haul.Collateral, haul.Duration, haul.EndBlockId, out result.ContractId, out result.ContractConditionId);
			result.Success = (myContractCreationResults == MyContractCreationResults.Success);
			return result;
		}

		private MyAddContractResultWrapper AddContract_Acquisition(IMyContractAcquisition acqui)
		{
			MyAddContractResultWrapper myAddContractResultWrapper = default(MyAddContractResultWrapper);
			myAddContractResultWrapper.Success = false;
			myAddContractResultWrapper.ContractId = 0L;
			MyAddContractResultWrapper result = myAddContractResultWrapper;
			MyContractBlock myContractBlock = MyEntities.GetEntityById(acqui.StartBlockId) as MyContractBlock;
			if (myContractBlock == null)
			{
				return result;
			}
			MyContractCreationResults myContractCreationResults = GenerateCustomContract_ObtainAndDeliver(myContractBlock, acqui.MoneyReward, acqui.Collateral, acqui.Duration, acqui.EndBlockId, acqui.ItemTypeId, acqui.ItemAmount, out result.ContractId, out result.ContractConditionId);
			result.Success = (myContractCreationResults == MyContractCreationResults.Success);
			return result;
		}

		private MyAddContractResultWrapper AddContract_Search(IMyContractSearch search)
		{
			MyAddContractResultWrapper myAddContractResultWrapper = default(MyAddContractResultWrapper);
			myAddContractResultWrapper.Success = false;
			myAddContractResultWrapper.ContractId = 0L;
			MyAddContractResultWrapper result = myAddContractResultWrapper;
			MyContractBlock myContractBlock = MyEntities.GetEntityById(search.StartBlockId) as MyContractBlock;
			if (myContractBlock == null)
			{
				return result;
			}
			MyContractCreationResults myContractCreationResults = GenerateCustomContract_Find(myContractBlock, search.MoneyReward, search.Collateral, search.Duration, search.TargetGridId, search.SearchRadius, out result.ContractId, out result.ContractConditionId);
			result.Success = (myContractCreationResults == MyContractCreationResults.Success);
			return result;
		}

		private MyAddContractResultWrapper AddContract_Escort(IMyContractEscort escort)
		{
			MyAddContractResultWrapper myAddContractResultWrapper = default(MyAddContractResultWrapper);
			myAddContractResultWrapper.Success = false;
			myAddContractResultWrapper.ContractId = 0L;
			MyAddContractResultWrapper result = myAddContractResultWrapper;
			MyContractBlock myContractBlock = MyEntities.GetEntityById(escort.StartBlockId) as MyContractBlock;
			if (myContractBlock == null)
			{
				return result;
			}
			MyContractCreationResults myContractCreationResults = GenerateCustomContract_Escort(myContractBlock, escort.MoneyReward, escort.Collateral, escort.Duration, escort.Start, escort.End, escort.OwnerIdentityId, out result.ContractId, out result.ContractConditionId);
			result.Success = (myContractCreationResults == MyContractCreationResults.Success);
			return result;
		}

		private MyAddContractResultWrapper AddContract_Bounty(IMyContractBounty bounty)
		{
			MyAddContractResultWrapper myAddContractResultWrapper = default(MyAddContractResultWrapper);
			myAddContractResultWrapper.Success = false;
			myAddContractResultWrapper.ContractId = 0L;
			MyAddContractResultWrapper result = myAddContractResultWrapper;
			MyContractBlock myContractBlock = MyEntities.GetEntityById(bounty.StartBlockId) as MyContractBlock;
			if (myContractBlock == null)
			{
				return result;
			}
			MyContractCreationResults myContractCreationResults = GenerateCustomContract_Hunt(myContractBlock, bounty.MoneyReward, bounty.Collateral, bounty.Duration, bounty.TargetIdentityId, out result.ContractId, out result.ContractConditionId);
			result.Success = (myContractCreationResults == MyContractCreationResults.Success);
			return result;
		}

		private MyAddContractResultWrapper AddContract_Repair(IMyContractRepair repair)
		{
			MyAddContractResultWrapper myAddContractResultWrapper = default(MyAddContractResultWrapper);
			myAddContractResultWrapper.Success = false;
			myAddContractResultWrapper.ContractId = 0L;
			MyAddContractResultWrapper result = myAddContractResultWrapper;
			MyContractBlock myContractBlock = MyEntities.GetEntityById(repair.StartBlockId) as MyContractBlock;
			if (myContractBlock == null)
			{
				return result;
			}
			MyContractCreationResults myContractCreationResults = GenerateCustomContract_Repair(myContractBlock, repair.MoneyReward, repair.Collateral, repair.Duration, repair.GridId, out result.ContractId, out result.ContractConditionId);
			result.Success = (myContractCreationResults == MyContractCreationResults.Success);
			return result;
		}

		private MyAddContractResultWrapper AddContract_Custom(IMyContractCustom custom)
		{
			MyAddContractResultWrapper myAddContractResultWrapper = default(MyAddContractResultWrapper);
			myAddContractResultWrapper.Success = false;
			myAddContractResultWrapper.ContractId = 0L;
			MyAddContractResultWrapper result = myAddContractResultWrapper;
			MyContractBlock myContractBlock = MyEntities.GetEntityById(custom.StartBlockId) as MyContractBlock;
			if (myContractBlock == null)
			{
				return result;
			}
			MyContractBlock myContractBlock2 = null;
			if (custom.EndBlockId.HasValue)
			{
				myContractBlock2 = (MyEntities.GetEntityById(custom.EndBlockId.Value) as MyContractBlock);
				if (myContractBlock2 == null)
				{
					return result;
				}
			}
			MyContractCreationResults myContractCreationResults = GenerateCustomContract_Custom(custom.DefinitionId, custom.Name, custom.Description, myContractBlock, custom.MoneyReward, custom.Collateral, custom.ReputationReward, custom.FailReputationPrice, custom.Duration, out result.ContractId, out result.ContractConditionId, myContractBlock2);
			result.Success = (myContractCreationResults == MyContractCreationResults.Success);
			return result;
		}

		public bool RemoveContract(long contractId)
		{
			if (m_inactiveContracts.ContainsKey(contractId))
			{
				RemoveContractInternal(contractId);
				return true;
			}
			if (m_activeContracts.ContainsKey(contractId))
			{
				m_activeContracts[contractId].Fail();
				return true;
			}
			return false;
		}

		public bool IsContractInInactive(long contractId)
		{
			return m_inactiveContracts.ContainsKey(contractId);
		}

		public bool IsContractActive(long contractId)
		{
			if (!m_activeContracts.ContainsKey(contractId))
			{
				return false;
			}
			return m_activeContracts[contractId].State == MyContractStateEnum.Active;
		}

		public MyCustomContractStateEnum GetContractState(long contractId)
		{
			MyContract value = null;
			if (m_activeContracts.TryGetValue(contractId, out value))
			{
				return MyContractCustom.ConvertContractState(value.State);
			}
			if (m_inactiveContracts.TryGetValue(contractId, out value))
			{
				return MyContractCustom.ConvertContractState(value.State);
			}
			return MyCustomContractStateEnum.Invalid;
		}

		public bool TryFinishCustomContract(long contractId)
		{
			if (!m_activeContracts.ContainsKey(contractId))
			{
				return false;
			}
			MyContract myContract = m_activeContracts[contractId];
			if (myContract.State != MyContractStateEnum.Active)
			{
				return false;
			}
			return myContract.Finish() == MyContractResults.Success;
		}

		public bool TryFailCustomContract(long contractId)
		{
			if (!m_activeContracts.ContainsKey(contractId))
			{
				return false;
			}
			MyContract myContract = m_activeContracts[contractId];
			if (myContract.State != MyContractStateEnum.Active)
			{
				return false;
			}
			myContract.Fail();
			return true;
		}

		public bool TryAbandonCustomContract(long contractId, long playerId)
		{
			if (!m_activeContracts.ContainsKey(contractId))
			{
				return false;
			}
			MyContract myContract = m_activeContracts[contractId];
			if (myContract.State != MyContractStateEnum.Active)
			{
				return false;
			}
			myContract.Abandon(playerId);
			return true;
		}

		public MyDefinitionId? GetContractDefinitionId(long contractId)
		{
			if (m_inactiveContracts.ContainsKey(contractId))
			{
				return m_inactiveContracts[contractId].GetDefinitionId();
			}
			if (m_activeContracts.ContainsKey(contractId))
			{
				return m_activeContracts[contractId].GetDefinitionId();
			}
			return null;
		}
	}
}
