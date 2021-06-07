using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Contracts
{
	[StaticEventOwner]
	public abstract class MyContract
	{
		private enum MyContractLogType
		{
			ACCEPT,
			FINISH,
			FAIL,
			ABANDON
		}

		protected sealed class CreateParticleEffectOnEntityEvent_003C_003ESystem_String_0023System_Int64_0023System_Boolean : ICallSite<IMyEventOwner, string, long, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string name, in long targetEntity, in bool offset, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateParticleEffectOnEntityEvent(name, targetEntity, offset);
			}
		}

		private MyContractStateEnum m_state;

		private int m_unfinishedConditionCount;

		private MyTimeSpan? m_lastTimeUpdate;

		public Action<long> OnContractAcquired;

		public Action OnContractFailed;

		public Action OnContractSucceeded;

		public Action<long, MyContractStateEnum, MyContractStateEnum> ContractChangedState
		{
			get;
			set;
		}

		public long Id
		{
			get;
			private set;
		}

		public bool IsPlayerMade
		{
			get;
			private set;
		}

		public MyContractStateEnum State
		{
			get
			{
				return m_state;
			}
			set
			{
				if (m_state != value)
				{
					MyContractStateEnum state = m_state;
					m_state = value;
					ContractChangedState.InvokeIfNotNull(Id, state, value);
				}
			}
		}

		public List<long> Owners
		{
			get;
			private set;
		} = new List<long>();


		public List<long> AllConditions
		{
			get;
			private set;
		} = new List<long>();


		public MyContractCondition ContractCondition
		{
			get;
			private set;
		}

		public long RewardMoney
		{
			get;
			private set;
		}

		public int RewardReputation
		{
			get;
			private set;
		}

		public long StartingDeposit
		{
			get;
			private set;
		}

		public int FailReputationPrice
		{
			get;
			private set;
		}

		public long StartFaction
		{
			get;
			private set;
		}

		public long StartStation
		{
			get;
			private set;
		}

		public long StartBlock
		{
			get;
			private set;
		}

		public MyTimeSpan Creation
		{
			get;
			private set;
		}

		public MyTimeSpan? RemainingTime
		{
			get;
			private set;
		}

		public int? TicksToDiscard
		{
			get;
			private set;
		}

		public bool NeedsUpdate => NeedsUpdate_Internal();

		public bool CanBeShared => CanBeShared_Internal();

		public bool CanBeFinished => CanBeFinished_Internal();

		public bool CanBeFinishedInTerminal => CountRemainingConditions() > 0;

		public bool IsTimeLimited => RemainingTime.HasValue;

		public MyActivationResults CanActivate(long identityId)
		{
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			if (component.GetContractLimitPerPlayer() <= myIdentity.ActiveContracts.Count)
			{
				return MyActivationResults.Fail_ContractLimitReachedHard;
			}
			return CanActivate_Internal(identityId);
		}

		public bool Activate(long playerId, MyTimeSpan timeOfActivation)
		{
			if (CanActivate_Internal(playerId) != 0)
			{
				return false;
			}
			if (m_state != 0)
			{
				MyLog.Default.WriteToLogAndAssert("Contract - Cannot activate other than inactive contract\nCurrent state: " + m_state);
			}
			State = MyContractStateEnum.Active;
			Owners.Add(playerId);
			MySession.Static.Players.TryGetIdentity(playerId)?.ActiveContracts.Add(Id);
			Activate_Internal(timeOfActivation);
			if (OnContractAcquired != null)
			{
				OnContractAcquired(playerId);
			}
			LogContract(MyContractLogType.ACCEPT);
			if (MyVisualScriptLogicProvider.ContractAccepted != null)
			{
				MyDefinitionId? definitionId = GetDefinitionId();
				if (definitionId.HasValue)
				{
					MyVisualScriptLogicProvider.ContractAccepted(Id, definitionId.Value, playerId, IsPlayerMade, StartBlock, StartFaction, StartStation);
				}
				else
				{
					MyLog.Default.WriteToLogAndAssert($"Contract definition not found. Something is really wrong. Contract type '{GetType().ToString()}', contract id '{Id}'.");
				}
			}
			return true;
		}

		public bool Share(long playerId)
		{
			if (!CanBeShared)
			{
				return false;
			}
			foreach (long owner in Owners)
			{
				if (owner == playerId)
				{
					return false;
				}
			}
			Owners.Add(playerId);
			MySession.Static.Players.TryGetIdentity(playerId)?.ActiveContracts.Add(Id);
			Share_Internal(playerId);
			return true;
		}

		public MyContractResults Finish()
		{
			if (!CanBeFinished)
			{
				return MyContractResults.Fail_FinishConditionsNotMet;
			}
			if (m_state != MyContractStateEnum.Active)
			{
				MyLog.Default.WriteToLogAndAssert("Contract - Cannot finish Contract that is not active\nCurrent state: " + m_state);
			}
			State = MyContractStateEnum.Finished;
			LogContract(MyContractLogType.FINISH);
			int num = 0;
			foreach (long owner in Owners)
			{
				if (CanPlayerReceiveReward(owner))
				{
					num++;
				}
			}
			if (num == 0)
			{
				MyLog.Default.WriteToLogAndAssert("No one to receive contract reward, is it correct?");
			}
			Finish_Internal();
			long acceptingPlayerId = (Owners.Count >= 1) ? Owners[0] : 0;
			while (Owners.Count > 0)
			{
				long num2 = Owners[0];
				Owners.RemoveAtFast(0);
				MySession.Static.Players.TryGetIdentity(num2)?.ActiveContracts.Remove(Id);
				MySession.Static.GetComponent<MySessionComponentContractSystem>().SendNotificationToPlayer(MyContractNotificationTypes.ContractSuccessful, num2);
				FinishFor_Internal(num2, num);
			}
			if (m_unfinishedConditionCount > 0)
			{
				MyLog.Default.WriteToLogAndAssert("MyContract - Not all conditions have been fulfilled, but contract still finished. Should this be happening?");
			}
			if (OnContractSucceeded != null)
			{
				OnContractSucceeded();
			}
			if (MyVisualScriptLogicProvider.ContractFinished != null)
			{
				MyDefinitionId? definitionId = GetDefinitionId();
				if (definitionId.HasValue)
				{
					MyVisualScriptLogicProvider.ContractFinished(Id, definitionId.Value, acceptingPlayerId, IsPlayerMade, StartBlock, StartFaction, StartStation);
				}
				else
				{
					MyLog.Default.WriteToLogAndAssert($"Contract definition not found. Something is really wrong. Contract type '{GetType().ToString()}', contract id '{Id}'.");
				}
			}
			return MyContractResults.Success;
		}

		protected virtual void Finish_Internal()
		{
		}

		protected virtual void Fail_Internal()
		{
		}

		public bool Abandon(long playerId)
		{
			bool result = FailFor(playerId, abandon: true);
			if (Owners.Count <= 0)
			{
				Fail(abandon: true);
			}
			if (MyVisualScriptLogicProvider.ContractAbandoned != null)
			{
				MyDefinitionId? definitionId = GetDefinitionId();
				if (definitionId.HasValue)
				{
					MyVisualScriptLogicProvider.ContractAbandoned(Id, definitionId.Value, playerId, IsPlayerMade, StartBlock, StartFaction, StartStation);
					return result;
				}
				MyLog.Default.WriteToLogAndAssert($"Contract definition not found. Something is really wrong. Contract type '{GetType().ToString()}', contract id '{Id}'.");
			}
			return result;
		}

		public bool IsOwnerOfCondition(MyContractCondition cond)
		{
			return ContractCondition == cond;
		}

		public void Fail(bool abandon = false, bool punishOwner = true)
		{
			if (m_state != MyContractStateEnum.Active)
			{
				MyLog.Default.WriteToLogAndAssert("Contract - Cannot fail Contract that is not active\nCurrent state: " + m_state);
			}
			State = MyContractStateEnum.Failed;
			LogContract(abandon ? MyContractLogType.ABANDON : MyContractLogType.FAIL);
			if (IsPlayerMade && StartBlock != 0L)
			{
				MyCubeBlock myCubeBlock = MyEntities.GetEntityById(StartBlock) as MyCubeBlock;
				if (myCubeBlock != null && myCubeBlock.OwnerId != 0L)
				{
					if (StartingDeposit > 0)
					{
						if (punishOwner)
						{
							MyBankingSystem.ChangeBalance(myCubeBlock.OwnerId, StartingDeposit);
						}
						else if (Owners.Count > 0)
						{
							MyBankingSystem.ChangeBalance(Owners[0], StartingDeposit);
						}
					}
					if (RewardMoney > 0)
					{
						MyBankingSystem.ChangeBalance(myCubeBlock.OwnerId, RewardMoney);
					}
				}
			}
			if (!IsPlayerMade && StartingDeposit > 0 && punishOwner)
			{
				MySession.Static.GetComponent<MySessionComponentEconomy>()?.AddCurrencyDestroyed(StartingDeposit);
			}
			Fail_Internal();
			long acceptingPlayerId = (Owners.Count >= 1) ? Owners[0] : 0;
			while (Owners.Count > 0)
			{
				long player = Owners[0];
				FailFor(player);
			}
			if (OnContractFailed != null)
			{
				OnContractFailed();
			}
			if (!abandon && MyVisualScriptLogicProvider.ContractFailed != null)
			{
				MyDefinitionId? definitionId = GetDefinitionId();
				if (definitionId.HasValue)
				{
					MyVisualScriptLogicProvider.ContractFailed(Id, definitionId.Value, acceptingPlayerId, IsPlayerMade, StartBlock, StartFaction, StartStation, abandon);
				}
				else
				{
					MyLog.Default.WriteToLogAndAssert($"Contract definition not found. Something is really wrong. Contract type '{GetType().ToString()}', contract id '{Id}'.");
				}
			}
		}

		protected bool FailFor(long player, bool abandon = false)
		{
			int num = 0;
			using (List<long>.Enumerator enumerator = Owners.GetEnumerator())
			{
				while (enumerator.MoveNext() && enumerator.Current != player)
				{
					num++;
				}
			}
			if (num == Owners.Count)
			{
				return false;
			}
			Owners.RemoveAtFast(num);
			MySession.Static.Players.TryGetIdentity(player)?.ActiveContracts.Remove(Id);
			if (StartFaction > 0 && Sync.IsServer && MySession.Static != null)
			{
				MySession.Static.Factions.AddFactionPlayerReputation(player, StartFaction, -FailReputationPrice);
			}
			FailFor_Internal(player, abandon);
			MySession.Static.GetComponent<MySessionComponentContractSystem>().SendNotificationToPlayer(MyContractNotificationTypes.ContractFailed, player);
			return true;
		}

		protected void CleanUp()
		{
			if (m_state != MyContractStateEnum.Finished && m_state != MyContractStateEnum.Failed && m_state != MyContractStateEnum.ToBeDisposed)
			{
				MyLog.Default.WriteToLogAndAssert("Contract - Cannot cleanup Contract that is not finished/failed/marked-for-cleanup\nCurrent state: " + m_state);
			}
			CleanUp_Internal();
		}

		protected virtual void Activate_Internal(MyTimeSpan timeOfActivation)
		{
			long identifierId = Owners[0];
			if (StartingDeposit > 0)
			{
				MyBankingSystem.ChangeBalance(identifierId, -StartingDeposit);
			}
			ActivateCondition();
		}

		protected virtual void FinishFor_Internal(long player, int rewardeeCount)
		{
			if (Sync.IsServer && MySession.Static != null)
			{
				float num = 0f;
				if (rewardeeCount > 0)
				{
					num = 1f / (float)rewardeeCount;
				}
				if (CanPlayerReceiveReward(player))
				{
					if (StartFaction > 0)
					{
						MySession.Static.Factions.AddFactionPlayerReputation(player, StartFaction, (int)(num * (float)GetRepRewardForPlayer(player)));
					}
					long amount = (long)(num * (float)GetMoneyRewardForPlayer(player));
					MyBankingSystem.ChangeBalance(player, amount);
					if (!IsPlayerMade)
					{
						MySession.Static.GetComponent<MySessionComponentEconomy>()?.AddCurrencyGenerated(amount);
					}
				}
				if (StartingDeposit > 0)
				{
					MyBankingSystem.ChangeBalance(player, StartingDeposit);
				}
			}
			CleanConditionForPlayer(player);
		}

		internal void DecreaseTicksToDiscard()
		{
			if (TicksToDiscard.HasValue)
			{
				TicksToDiscard--;
			}
		}

		protected virtual bool CanPlayerReceiveReward(long player)
		{
			return true;
		}

		protected virtual void FailFor_Internal(long player, bool abandon = false)
		{
			CleanConditionForPlayer(player);
		}

		private void ActivateCondition()
		{
			if (ContractCondition != null)
			{
				if (ContractCondition is MyContractConditionDeliverPackage)
				{
					ActivateConditionDeliverPackage(ContractCondition);
				}
				else if (ContractCondition is MyContractConditionDeliverItems)
				{
					ActivateConditionDeliverItems(ContractCondition);
				}
			}
		}

		private void ShareConditionWithPlayer(long identityId)
		{
			if (ContractCondition != null)
			{
				if (ContractCondition is MyContractConditionDeliverPackage)
				{
					ShareConditionWithPlayerDeliverPackage(identityId, ContractCondition);
				}
				else if (ContractCondition is MyContractConditionDeliverItems)
				{
					ShareConditionWithPlayerDeliverItems(identityId, ContractCondition);
				}
			}
		}

		private void CleanConditionForPlayer(long identityId)
		{
			if (ContractCondition != null)
			{
				if (ContractCondition is MyContractConditionDeliverPackage)
				{
					CleanConditionForPlayerDeliverPackage(identityId, ContractCondition);
				}
				else if (ContractCondition is MyContractConditionDeliverItems)
				{
					CleanConditionForPlayerDeliverItems(identityId, ContractCondition);
				}
			}
		}

		public void ReshareConditionWithAll()
		{
			foreach (long owner in Owners)
			{
				CleanConditionForPlayer(owner);
				ShareConditionWithPlayer(owner);
			}
		}

		protected virtual void Share_Internal(long identityId)
		{
		}

		protected virtual void CleanUp_Internal()
		{
			State = MyContractStateEnum.Disposed;
		}

		protected virtual MyActivationResults CanActivate_Internal(long playerId)
		{
			return CheckPlayerFunds(playerId);
		}

		protected virtual bool NeedsUpdate_Internal()
		{
			if (State != MyContractStateEnum.Disposed)
			{
				if (!RemainingTime.HasValue && State != MyContractStateEnum.Failed && State != MyContractStateEnum.Finished)
				{
					return State == MyContractStateEnum.ToBeDisposed;
				}
				return true;
			}
			return false;
		}

		protected virtual bool CanBeShared_Internal()
		{
			return false;
		}

		public virtual bool CanBeFinished_Internal()
		{
			return m_unfinishedConditionCount <= 0;
		}

		public virtual void TimeRanOut_Internal()
		{
			if (m_state == MyContractStateEnum.Active)
			{
				Fail();
			}
		}

		public virtual long GetMoneyRewardForPlayer(long playerId)
		{
			return RewardMoney;
		}

		public virtual int GetRepRewardForPlayer(long playerId)
		{
			return RewardReputation;
		}

		public virtual MyObjectBuilder_Contract GetObjectBuilder()
		{
			MyObjectBuilder_Contract myObjectBuilder_Contract = MyContractFactory.CreateObjectBuilder(this);
			myObjectBuilder_Contract.Id = Id;
			myObjectBuilder_Contract.IsPlayerMade = IsPlayerMade;
			myObjectBuilder_Contract.State = m_state;
			myObjectBuilder_Contract.Owners = new MySerializableList<long>(Owners);
			myObjectBuilder_Contract.RewardMoney = RewardMoney;
			myObjectBuilder_Contract.RewardReputation = RewardReputation;
			myObjectBuilder_Contract.StartingDeposit = StartingDeposit;
			myObjectBuilder_Contract.FailReputationPrice = FailReputationPrice;
			myObjectBuilder_Contract.StartFaction = StartFaction;
			myObjectBuilder_Contract.StartStation = StartStation;
			myObjectBuilder_Contract.StartBlock = StartBlock;
			myObjectBuilder_Contract.Creation = Creation.Ticks;
			myObjectBuilder_Contract.TicksToDiscard = TicksToDiscard;
			if (RemainingTime.HasValue)
			{
				myObjectBuilder_Contract.RemainingTimeInS = RemainingTime.Value.Seconds;
			}
			if (ContractCondition != null)
			{
				myObjectBuilder_Contract.ContractCondition = ContractCondition.GetObjectBuilder();
			}
			return myObjectBuilder_Contract;
		}

		public virtual void Init(MyObjectBuilder_Contract ob)
		{
			Id = ob.Id;
			IsPlayerMade = ob.IsPlayerMade;
			m_state = ob.State;
			Owners = ob.Owners;
			RewardMoney = ob.RewardMoney;
			RewardReputation = ob.RewardReputation;
			StartingDeposit = ob.StartingDeposit;
			FailReputationPrice = ob.FailReputationPrice;
			StartFaction = ob.StartFaction;
			StartStation = ob.StartStation;
			StartBlock = ob.StartBlock;
			Creation = new MyTimeSpan(ob.Creation);
			TicksToDiscard = ob.TicksToDiscard;
			if (ob.RemainingTimeInS.HasValue)
			{
				RemainingTime = MyTimeSpan.FromSeconds(ob.RemainingTimeInS.Value);
			}
			if (ob.ContractCondition != null)
			{
				ContractCondition = MyContractConditionFactory.CreateInstance(ob.ContractCondition);
				if (!ob.ContractCondition.IsFinished)
				{
					m_unfinishedConditionCount++;
				}
			}
		}

		public virtual void BeforeStart()
		{
		}

		public void RecalculateUnfinishedCondiCount()
		{
			m_unfinishedConditionCount = ((!ContractCondition.IsFinished) ? 1 : 0);
		}

		public virtual void Update(MyTimeSpan currentTime)
		{
			if (State == MyContractStateEnum.Failed || State == MyContractStateEnum.Finished)
			{
				CleanUp();
			}
			if (IsTimeLimited && State == MyContractStateEnum.Active)
			{
				UpdateTime(currentTime);
				CheckTimeOut();
			}
		}

		private void UpdateTime(MyTimeSpan currentTime)
		{
			if (RemainingTime.HasValue)
			{
				if (!m_lastTimeUpdate.HasValue)
				{
					m_lastTimeUpdate = currentTime;
					return;
				}
				MyTimeSpan value = currentTime - m_lastTimeUpdate.Value;
				RemainingTime -= value;
				m_lastTimeUpdate = currentTime;
			}
		}

		private void CheckTimeOut()
		{
			if (RemainingTime.HasValue && RemainingTime.Value <= MyTimeSpan.Zero)
			{
				TimeRanOut_Internal();
			}
		}

		public int CountRemainingConditions()
		{
			return m_unfinishedConditionCount;
		}

		public virtual string ToDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"Name: {GetType().ToString()}");
			return stringBuilder.ToString();
		}

		private MyActivationResults CheckPlayerFunds(long playerId)
		{
			if (StartingDeposit <= 0)
			{
				return MyActivationResults.Success;
			}
			MyBankingSystem component = MySession.Static.GetComponent<MyBankingSystem>();
			if (component == null)
			{
				return MyActivationResults.Error;
			}
			if (!component.TryGetAccountInfo(playerId, out MyAccountInfo account))
			{
				return MyActivationResults.Error;
			}
			if (account.Balance < StartingDeposit)
			{
				return MyActivationResults.Fail_InsufficientFunds;
			}
			return MyActivationResults.Success;
		}

		public virtual MyDefinitionId? GetDefinitionId()
		{
			return null;
		}

		public MyContractTypeDefinition GetDefinition()
		{
			MyDefinitionId? definitionId = GetDefinitionId();
			if (!definitionId.HasValue)
			{
				return null;
			}
			return MyDefinitionManager.Static.GetContractType(definitionId.Value.SubtypeName);
		}

		private bool ActivateConditionDeliverPackage(MyContractCondition cond)
		{
			MyContractConditionDeliverPackage myContractConditionDeliverPackage = ContractCondition as MyContractConditionDeliverPackage;
			if (myContractConditionDeliverPackage == null)
			{
				return false;
			}
			long identityId = Owners[0];
			MyObjectBuilder_Package myObjectBuilder_Package = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Package>("Package");
			myObjectBuilder_Package.ContractConditionId = myContractConditionDeliverPackage.Id;
			myObjectBuilder_Package.ContractId = Id;
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			if (myIdentity == null)
			{
				return false;
			}
			MyCharacter character = myIdentity.Character;
			if (character == null)
			{
				return false;
			}
			if (character.InventoryCount <= 0)
			{
				return false;
			}
			character.GetInventoryBase().AddItems(1, myObjectBuilder_Package);
			bool flag = true;
			foreach (long owner in Owners)
			{
				flag &= ShareConditionWithPlayerDeliverPackage(owner, cond);
			}
			return flag;
		}

		private bool ActivateConditionDeliverItems(MyContractCondition cond)
		{
			bool flag = true;
			foreach (long owner in Owners)
			{
				flag &= ShareConditionWithPlayerDeliverItems(owner, cond);
			}
			return flag;
		}

		private bool ShareConditionWithPlayerDeliverPackage(long identityId, MyContractCondition cond)
		{
			MyContractConditionDeliverPackage myContractConditionDeliverPackage = cond as MyContractConditionDeliverPackage;
			if (myContractConditionDeliverPackage == null)
			{
				return false;
			}
			Vector3D coords = Vector3D.Zero;
			long entityId = 0L;
			MyStation stationByStationId = MySession.Static.Factions.GetStationByStationId(myContractConditionDeliverPackage.StationEndId);
			if (stationByStationId != null)
			{
				coords = stationByStationId.Position;
				entityId = 0L;
				if (stationByStationId.StationEntityId != 0L)
				{
					MyCubeGrid myCubeGrid = MyEntities.GetEntityById(stationByStationId.StationEntityId) as MyCubeGrid;
					if (myCubeGrid != null)
					{
						MyContractBlock firstBlockOfType = myCubeGrid.GetFirstBlockOfType<MyContractBlock>();
						if (firstBlockOfType != null)
						{
							coords = firstBlockOfType.PositionComp.GetPosition();
							entityId = firstBlockOfType.EntityId;
						}
					}
				}
			}
			else
			{
				MyEntity entityById = MyEntities.GetEntityById(myContractConditionDeliverPackage.BlockEndId);
				if (entityById != null)
				{
					coords = entityById.PositionComp.GetPosition();
					entityId = entityById.EntityId;
				}
			}
			MyGps gps = new MyGps();
			gps.DisplayName = MyTexts.GetString(MyCommonTexts.Contract_Delivery_GpsName);
			gps.Name = MyTexts.GetString(MyCommonTexts.Contract_Delivery_GpsName);
			gps.Description = MyTexts.GetString(MyCommonTexts.Contract_Delivery_GpsDescription);
			gps.Coords = coords;
			gps.ShowOnHud = true;
			gps.DiscardAt = null;
			gps.GPSColor = Color.DarkOrange;
			gps.ContractId = Id;
			gps.SetEntityId(entityId);
			MySession.Static.Gpss.SendAddGps(identityId, ref gps, entityId);
			return true;
		}

		private bool ShareConditionWithPlayerDeliverItems(long identityId, MyContractCondition cond)
		{
			MyContractConditionDeliverItems myContractConditionDeliverItems = cond as MyContractConditionDeliverItems;
			if (myContractConditionDeliverItems == null)
			{
				return false;
			}
			Vector3D coords = Vector3D.Zero;
			long entityId = 0L;
			MyStation stationByStationId = MySession.Static.Factions.GetStationByStationId(myContractConditionDeliverItems.StationEndId);
			if (stationByStationId != null)
			{
				coords = stationByStationId.Position;
				entityId = 0L;
				MyCubeGrid myCubeGrid = MyEntities.GetEntityById(stationByStationId.StationEntityId) as MyCubeGrid;
				if (myCubeGrid != null)
				{
					MyContractBlock firstBlockOfType = myCubeGrid.GetFirstBlockOfType<MyContractBlock>();
					if (firstBlockOfType != null)
					{
						coords = firstBlockOfType.PositionComp.GetPosition();
						entityId = firstBlockOfType.EntityId;
					}
				}
			}
			else
			{
				MyEntity entityById = MyEntities.GetEntityById(myContractConditionDeliverItems.BlockEndId);
				if (entityById != null)
				{
					coords = entityById.PositionComp.GetPosition();
					entityId = entityById.EntityId;
				}
			}
			MyGps gps = new MyGps();
			gps.DisplayName = MyTexts.GetString(MyCommonTexts.Contract_ObtainAndDelivery_GpsName);
			gps.Name = MyTexts.GetString(MyCommonTexts.Contract_ObtainAndDelivery_GpsName);
			gps.Description = MyTexts.GetString(MyCommonTexts.Contract_ObtainAndDelivery_GpsDescription);
			gps.Coords = coords;
			gps.ShowOnHud = true;
			gps.DiscardAt = null;
			gps.GPSColor = Color.DarkOrange;
			gps.ContractId = Id;
			gps.SetEntityId(entityId);
			MySession.Static.Gpss.SendAddGps(identityId, ref gps, entityId);
			return true;
		}

		private void CleanConditionForPlayerDeliverPackage(long identityId, MyContractCondition cond)
		{
			if (cond is MyContractConditionDeliverPackage)
			{
				MyGps gpsByContractId = MySession.Static.Gpss.GetGpsByContractId(identityId, Id);
				if (gpsByContractId != null)
				{
					MySession.Static.Gpss.SendDelete(identityId, gpsByContractId.Hash);
				}
			}
		}

		private void CleanConditionForPlayerDeliverItems(long identityId, MyContractCondition cond)
		{
			if (cond is MyContractConditionDeliverItems)
			{
				MyGps gpsByContractId = MySession.Static.Gpss.GetGpsByContractId(identityId, Id);
				if (gpsByContractId != null)
				{
					MySession.Static.Gpss.SendDelete(identityId, gpsByContractId.Hash);
				}
			}
		}

		protected void CreateParticleEffectOnEntity(string name, long targetEntity, bool offset)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CreateParticleEffectOnEntityEvent, name, targetEntity, offset);
			if (!Sync.IsDedicated)
			{
				CreateParticleEffectOnEntityEvent(name, targetEntity, offset);
			}
		}

		[Event(null, 923)]
		[Reliable]
		[Broadcast]
		private static void CreateParticleEffectOnEntityEvent(string name, long targetEntity, bool offset)
		{
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(targetEntity) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				double scaleFactor = myCubeGrid.PositionComp.WorldAABB.HalfExtents.AbsMax() * 2.0;
				MatrixD worldMatrix = myCubeGrid.PositionComp.WorldMatrix;
				Vector3D worldPosition = worldMatrix.Translation;
				if (offset)
				{
					worldPosition += scaleFactor * worldMatrix.Forward;
				}
				MatrixD effectMatrix = (offset ? (-1f) : 1f) * MatrixD.Identity;
				if (offset)
				{
					effectMatrix.Translation = scaleFactor * MatrixD.Identity.Forward;
				}
				MyParticlesManager.TryCreateParticleEffect(name, ref effectMatrix, ref worldPosition, myCubeGrid.Render.GetRenderObjectID(), out MyParticleEffect _);
			}
		}

		private void LogContract(MyContractLogType logType)
		{
			string text = string.Empty;
			if (StartFaction != 0L)
			{
				IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(StartFaction);
				if (myFaction != null)
				{
					text = myFaction.Tag;
				}
			}
			ulong num = 0uL;
			if (Owners.Count > 0)
			{
				num = MySession.Static.Players.TryGetSteamId(Owners[0]);
			}
			ulong num2 = 0uL;
			if (StartBlock != 0L)
			{
				MyCubeBlock myCubeBlock = MyEntities.GetEntityById(StartBlock) as MyCubeBlock;
				if (myCubeBlock != null)
				{
					num2 = MySession.Static.Players.TryGetSteamId(myCubeBlock.OwnerId);
				}
			}
			string text2 = string.Empty;
			int num3 = 0;
			MyContractConditionDeliverItems myContractConditionDeliverItems;
			if ((myContractConditionDeliverItems = (ContractCondition as MyContractConditionDeliverItems)) != null)
			{
				MyObjectBuilderType typeId = myContractConditionDeliverItems.ItemType.TypeId;
				text2 = $"{typeId.ToString()}_{myContractConditionDeliverItems.ItemType.SubtypeName}";
				num3 = myContractConditionDeliverItems.ItemAmount;
			}
			string msg = $"CONTRACT LEGEND,change,id,type,playerMade,reputation,currency,factionTag,ownerId,playerId,ItemType,ItemAmount";
			string msg2 = $"CONTRACT,{logType.ToString()},{Id},{GetType().ToString()},{IsPlayerMade},{RewardReputation},{RewardMoney},{text},{num2},{num},{text2},{num3}";
			MyLog.Default.WriteLine(msg);
			MyLog.Default.WriteLine(msg2);
		}

		internal void RefundRewardOnDelete(MyContractBlock startBlock)
		{
			if (State == MyContractStateEnum.Inactive && IsPlayerMade && startBlock != null && startBlock.EntityId == StartBlock)
			{
				MyBankingSystem.ChangeBalance(startBlock.OwnerId, RewardMoney);
			}
		}
	}
}
