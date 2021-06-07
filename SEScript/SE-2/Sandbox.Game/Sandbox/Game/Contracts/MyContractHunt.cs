using Sandbox.Definitions;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRageMath;

namespace Sandbox.Game.Contracts
{
	[MyContractDescriptor(typeof(MyObjectBuilder_ContractHunt))]
	public class MyContractHunt : MyContract
	{
		private bool m_targetKilled;

		private bool m_targetKilledDirectly;

		private Vector3D? m_deathLocation;

		public long Target
		{
			get;
			private set;
		}

		public MyTimeSpan TimerNextRemark
		{
			get;
			private set;
		}

		public MyTimeSpan RemarkPeriod
		{
			get;
			private set;
		}

		public float RemarkVariance
		{
			get;
			private set;
		}

		public Vector3D MarkPosition
		{
			get;
			private set;
		}

		public bool IsTargetInWorld
		{
			get;
			private set;
		}

		public double KillRange
		{
			get;
			private set;
		}

		public float KillRangeMultiplier
		{
			get;
			private set;
		}

		public int ReputationLossForTarget
		{
			get;
			private set;
		}

		public double RewardRadius
		{
			get;
			private set;
		}

		public override MyObjectBuilder_Contract GetObjectBuilder()
		{
			MyObjectBuilder_Contract objectBuilder = base.GetObjectBuilder();
			MyObjectBuilder_ContractHunt obj = objectBuilder as MyObjectBuilder_ContractHunt;
			obj.Target = Target;
			obj.TimerNextRemark = TimerNextRemark.Ticks;
			obj.RemarkPeriod = RemarkPeriod.Ticks;
			obj.RemarkVariance = RemarkVariance;
			obj.MarkPosition = MarkPosition;
			obj.IsTargetInWorld = IsTargetInWorld;
			obj.KillRange = KillRange;
			obj.KillRangeMultiplier = KillRangeMultiplier;
			obj.ReputationLossForTarget = ReputationLossForTarget;
			obj.RewardRadius = RewardRadius;
			obj.TargetKilled = m_targetKilled;
			obj.TargetKilledDirectly = m_targetKilledDirectly;
			return objectBuilder;
		}

		public override void Init(MyObjectBuilder_Contract ob)
		{
			base.Init(ob);
			MyObjectBuilder_ContractHunt myObjectBuilder_ContractHunt = ob as MyObjectBuilder_ContractHunt;
			if (myObjectBuilder_ContractHunt != null)
			{
				Target = myObjectBuilder_ContractHunt.Target;
				TimerNextRemark = new MyTimeSpan(myObjectBuilder_ContractHunt.TimerNextRemark);
				RemarkPeriod = new MyTimeSpan(myObjectBuilder_ContractHunt.RemarkPeriod);
				RemarkVariance = myObjectBuilder_ContractHunt.RemarkVariance;
				MarkPosition = myObjectBuilder_ContractHunt.MarkPosition;
				IsTargetInWorld = myObjectBuilder_ContractHunt.IsTargetInWorld;
				KillRange = myObjectBuilder_ContractHunt.KillRange;
				KillRangeMultiplier = myObjectBuilder_ContractHunt.KillRangeMultiplier;
				ReputationLossForTarget = myObjectBuilder_ContractHunt.ReputationLossForTarget;
				RewardRadius = myObjectBuilder_ContractHunt.RewardRadius;
				m_targetKilled = myObjectBuilder_ContractHunt.TargetKilled;
				m_targetKilledDirectly = myObjectBuilder_ContractHunt.TargetKilledDirectly;
			}
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			if (base.State == MyContractStateEnum.Active)
			{
				MyFactionCollection factions = MySession.Static.Factions;
				factions.PlayerKilledByPlayer = (Action<long, long>)Delegate.Combine(factions.PlayerKilledByPlayer, new Action<long, long>(KilledByPlayer));
				MyFactionCollection factions2 = MySession.Static.Factions;
				factions2.PlayerKilledByUnknown = (Action<long>)Delegate.Combine(factions2.PlayerKilledByUnknown, new Action<long>(KilledByUnknown));
			}
		}

		private void KilledByPlayer(long victimId, long killerId)
		{
			if (victimId != Target || !base.Owners.Contains(killerId))
			{
				return;
			}
			m_deathLocation = GetVictimLocation();
			if (m_deathLocation.HasValue)
			{
				m_targetKilled = true;
				m_targetKilledDirectly = true;
				Finish();
				long pirateFactionId = GetPirateFactionId();
				if (pirateFactionId != 0L)
				{
					MySession.Static.Factions.AddFactionPlayerReputation(victimId, pirateFactionId, -ReputationLossForTarget);
				}
			}
		}

		private long GetPirateFactionId()
		{
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component == null)
			{
				return 0L;
			}
			MyFactionDefinition myFactionDefinition = MyDefinitionManager.Static.GetDefinition(component.EconomyDefinition.PirateId) as MyFactionDefinition;
			if (myFactionDefinition == null)
			{
				return 0L;
			}
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				if (faction.Value.Tag == myFactionDefinition.Tag)
				{
					return faction.Value.FactionId;
				}
			}
			return 0L;
		}

		private Vector3D? GetVictimLocation()
		{
			if (!MySession.Static.Players.TryGetPlayerId(Target, out MyPlayer.PlayerId result))
			{
				return null;
			}
			MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
			if (playerById == null || playerById.Controller == null || playerById.Controller.ControlledEntity == null)
			{
				return null;
			}
			return (playerById.Controller.ControlledEntity as MyEntity).PositionComp.GetPosition();
		}

		private void KilledByUnknown(long victimId)
		{
			if (victimId != Target)
			{
				return;
			}
			bool flag = false;
			m_deathLocation = GetVictimLocation();
			if (!m_deathLocation.HasValue)
			{
				return;
			}
			double num = KillRange * KillRange;
			foreach (long owner in base.Owners)
			{
				if (MySession.Static.Players.TryGetPlayerId(owner, out MyPlayer.PlayerId result))
				{
					MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
					if (playerById != null && playerById.Controller != null && playerById.Controller.ControlledEntity != null)
					{
						Vector3D position = (playerById.Controller.ControlledEntity as MyEntity).PositionComp.GetPosition();
						if ((m_deathLocation.Value - position).LengthSquared() <= num)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (flag)
			{
				m_targetKilled = true;
				m_targetKilledDirectly = false;
				Finish();
				if (GetPirateFactionId() != 0L)
				{
					MySession.Static.Factions.AddFactionPlayerReputation(victimId, base.StartFaction, (int)(KillRangeMultiplier * (float)ReputationLossForTarget));
				}
			}
		}

		public override bool CanBeFinished_Internal()
		{
			if (base.CanBeFinished_Internal())
			{
				return m_targetKilled;
			}
			return false;
		}

		protected override bool CanPlayerReceiveReward(long identityId)
		{
			if (!m_deathLocation.HasValue)
			{
				return false;
			}
			if (!MySession.Static.Players.TryGetPlayerId(identityId, out MyPlayer.PlayerId result))
			{
				return false;
			}
			MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
			if (playerById == null || playerById.Controller == null || playerById.Controller.ControlledEntity == null)
			{
				return false;
			}
			MyEntity myEntity = playerById.Controller.ControlledEntity as MyEntity;
			double num = RewardRadius + myEntity.PositionComp.WorldAABB.HalfExtents.Length();
			if ((myEntity.PositionComp.GetPosition() - m_deathLocation.Value).LengthSquared() > num * num)
			{
				return false;
			}
			return true;
		}

		public override MyDefinitionId? GetDefinitionId()
		{
			return new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Hunt");
		}

		protected override MyActivationResults CanActivate_Internal(long playerId)
		{
			MyActivationResults myActivationResults = base.CanActivate_Internal(playerId);
			if (myActivationResults != 0)
			{
				return myActivationResults;
			}
			if (Target == playerId)
			{
				return MyActivationResults.Fail_YouAreTargetOfThisHunt;
			}
			bool flag = false;
			foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Identity.IdentityId == Target)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return MyActivationResults.Fail_TargetNotOnline;
			}
			return MyActivationResults.Success;
		}

		protected override void Activate_Internal(MyTimeSpan timeOfActivation)
		{
			base.Activate_Internal(timeOfActivation);
			foreach (long owner in base.Owners)
			{
				Mark(owner);
			}
			MyFactionCollection factions = MySession.Static.Factions;
			factions.PlayerKilledByPlayer = (Action<long, long>)Delegate.Combine(factions.PlayerKilledByPlayer, new Action<long, long>(KilledByPlayer));
			MyFactionCollection factions2 = MySession.Static.Factions;
			factions2.PlayerKilledByUnknown = (Action<long>)Delegate.Combine(factions2.PlayerKilledByUnknown, new Action<long>(KilledByUnknown));
		}

		protected override void FailFor_Internal(long player, bool abandon = false)
		{
			base.FailFor_Internal(player, abandon);
			Unmark(player);
		}

		protected override void FinishFor_Internal(long player, int rewardeeCount)
		{
			base.FinishFor_Internal(player, rewardeeCount);
			Unmark(player);
		}

		public override void Update(MyTimeSpan currentTime)
		{
			base.Update(currentTime);
			if (TimerNextRemark < currentTime)
			{
				foreach (long owner in base.Owners)
				{
					Unmark(owner);
				}
				ChangeMarkPosition();
				foreach (long owner2 in base.Owners)
				{
					Mark(owner2);
				}
				TimerNextRemark = currentTime + RemarkPeriod;
			}
		}

		private void ChangeMarkPosition()
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(Target);
			if (myIdentity == null || myIdentity.IsDead || myIdentity.Character == null)
			{
				IsTargetInWorld = false;
				return;
			}
			Vector3D position = myIdentity.Character.PositionComp.GetPosition();
			Vector3D vector3D2 = MarkPosition = new BoundingSphereD(position, RemarkVariance).RandomToUniformPointInSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble());
		}

		protected override void CleanUp_Internal()
		{
			base.CleanUp_Internal();
			MyFactionCollection factions = MySession.Static.Factions;
			factions.PlayerKilledByPlayer = (Action<long, long>)Delegate.Remove(factions.PlayerKilledByPlayer, new Action<long, long>(KilledByPlayer));
			MyFactionCollection factions2 = MySession.Static.Factions;
			factions2.PlayerKilledByUnknown = (Action<long>)Delegate.Remove(factions2.PlayerKilledByUnknown, new Action<long>(KilledByUnknown));
		}

		private void Remark(long playerId)
		{
			Unmark(playerId);
			Mark(playerId);
		}

		private void Mark(long playerId)
		{
			MyGps gps = CreateGPS();
			MySession.Static.Gpss.SendAddGps(playerId, ref gps, 0L);
		}

		private void Unmark(long identityId)
		{
			MyGps gpsByContractId = MySession.Static.Gpss.GetGpsByContractId(identityId, base.Id);
			if (gpsByContractId != null)
			{
				MySession.Static.Gpss.SendDelete(identityId, gpsByContractId.Hash);
			}
		}

		private MyGps CreateGPS()
		{
			return new MyGps
			{
				DisplayName = MyTexts.GetString(MyCommonTexts.Contract_Hunt_GpsName),
				Name = MyTexts.GetString(MyCommonTexts.Contract_Hunt_GpsName),
				Description = MyTexts.GetString(MyCommonTexts.Contract_Hunt_GpsDescription),
				Coords = MarkPosition,
				ShowOnHud = true,
				DiscardAt = null,
				GPSColor = Color.DarkOrange,
				ContractId = base.Id
			};
		}

		public override long GetMoneyRewardForPlayer(long playerId)
		{
			return (long)((m_targetKilledDirectly ? 1f : KillRangeMultiplier) * (float)base.GetMoneyRewardForPlayer(playerId));
		}

		public override int GetRepRewardForPlayer(long playerId)
		{
			return (int)((m_targetKilledDirectly ? 1f : KillRangeMultiplier) * (float)base.GetRepRewardForPlayer(playerId));
		}
	}
}
