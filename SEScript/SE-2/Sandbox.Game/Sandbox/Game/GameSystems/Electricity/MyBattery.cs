using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems.Electricity
{
	[StaticEventOwner]
	public class MyBattery
	{
		private class MyBatteryRegenerationEffect
		{
			public float ChargePerSecond;

			public MyTimeSpan RemainingTime;

			public MyTimeSpan LastRegenTime;

			public void Init(MyObjectBuilder_BatteryRegenerationEffect builder)
			{
				ChargePerSecond = builder.ChargePerSecond;
				RemainingTime = MyTimeSpan.FromMilliseconds(builder.RemainingTimeInMiliseconds);
				LastRegenTime = MyTimeSpan.FromMilliseconds(builder.LastRegenTimeInMiliseconds);
			}

			public MyObjectBuilder_BatteryRegenerationEffect GetObjectBuilder()
			{
				return new MyObjectBuilder_BatteryRegenerationEffect
				{
					ChargePerSecond = ChargePerSecond,
					RemainingTimeInMiliseconds = (long)RemainingTime.Milliseconds,
					LastRegenTimeInMiliseconds = (long)LastRegenTime.Milliseconds
				};
			}
		}

		protected sealed class SyncCapacitySuccess_003C_003ESystem_Int64_0023System_Single : ICallSite<IMyEventOwner, long, float, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in float remainingCapacity, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SyncCapacitySuccess(entityId, remainingCapacity);
			}
		}

		public static readonly string BATTERY_CHARGE_STAT_NAME = "BatteryCharge";

		public static float BATTERY_DEPLETION_MULTIPLIER = 1f;

		private int m_lastUpdateTime;

		private MyEntity m_lastParent;

		public const float EnergyCriticalThresholdCharacter = 0.1f;

		public const float EnergyLowThresholdCharacter = 0.25f;

		public const float EnergyCriticalThresholdShip = 0.05f;

		public const float EnergyLowThresholdShip = 0.125f;

		private const int m_productionUpdateInterval = 100;

		private readonly MyCharacter m_owner;

		private readonly MyStringHash m_resourceSinkGroup = MyStringHash.GetOrCompute("Charging");

		private readonly MyStringHash m_resourceSourceGroup = MyStringHash.GetOrCompute("Battery");

		public float RechargeMultiplier = 1f;

		private Dictionary<int, MyBatteryRegenerationEffect> m_regenEffects = new Dictionary<int, MyBatteryRegenerationEffect>();

		private static List<int> m_tmpRemoveEffects = new List<int>();

		public bool IsEnergyCriticalShip => ResourceSource.RemainingCapacity / 1E-05f < 0.05f;

		public bool IsEnergyLowShip => ResourceSource.RemainingCapacity / 1E-05f < 0.125f;

		public MyCharacter Owner => m_owner;

		public MyResourceSinkComponent ResourceSink
		{
			get;
			private set;
		}

		public MyResourceSourceComponent ResourceSource
		{
			get;
			private set;
		}

		public bool OwnedByLocalPlayer
		{
			get;
			set;
		}

		public MyBattery(MyCharacter owner)
		{
			m_owner = owner;
			ResourceSink = new MyResourceSinkComponent();
			ResourceSource = new MyResourceSourceComponent();
		}

		public void Init(MyObjectBuilder_Battery builder, List<MyResourceSinkInfo> additionalSinks = null, List<MyResourceSourceInfo> additionalSources = null)
		{
			MyResourceSinkInfo myResourceSinkInfo = default(MyResourceSinkInfo);
			myResourceSinkInfo.MaxRequiredInput = 0.0018f;
			myResourceSinkInfo.ResourceTypeId = MyResourceDistributorComponent.ElectricityId;
			myResourceSinkInfo.RequiredInputFunc = Sink_ComputeRequiredPower;
			MyResourceSinkInfo myResourceSinkInfo2 = myResourceSinkInfo;
			if (additionalSinks != null)
			{
				additionalSinks.Insert(0, myResourceSinkInfo2);
				ResourceSink.Init(m_resourceSinkGroup, additionalSinks);
			}
			else
			{
				ResourceSink.Init(m_resourceSinkGroup, myResourceSinkInfo2);
			}
			ResourceSink.TemporaryConnectedEntity = m_owner;
			MyResourceSourceInfo myResourceSourceInfo = default(MyResourceSourceInfo);
			myResourceSourceInfo.ResourceTypeId = MyResourceDistributorComponent.ElectricityId;
			myResourceSourceInfo.DefinedOutput = 0.009f;
			myResourceSourceInfo.ProductionToCapacityMultiplier = 3600f;
			MyResourceSourceInfo myResourceSourceInfo2 = myResourceSourceInfo;
			if (additionalSources != null)
			{
				additionalSources.Insert(0, myResourceSourceInfo2);
				ResourceSource.Init(m_resourceSourceGroup, additionalSources);
			}
			else
			{
				ResourceSource.Init(m_resourceSourceGroup, myResourceSourceInfo2);
			}
			ResourceSource.TemporaryConnectedEntity = m_owner;
			m_lastUpdateTime = MySession.Static.GameplayFrameCounter;
			if (builder == null)
			{
				ResourceSource.SetProductionEnabledByType(MyResourceDistributorComponent.ElectricityId, newProducerEnabled: true);
				ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, 1E-05f);
				ResourceSink.Update();
				return;
			}
			ResourceSource.SetProductionEnabledByType(MyResourceDistributorComponent.ElectricityId, builder.ProducerEnabled);
			if (MySession.Static.SurvivalMode)
			{
				ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, MathHelper.Clamp(builder.CurrentCapacity, 0f, 1E-05f));
			}
			else
			{
				ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, 1E-05f);
			}
			if (builder.RegenEffects != null)
			{
				foreach (MyObjectBuilder_BatteryRegenerationEffect regenEffect in builder.RegenEffects)
				{
					MyBatteryRegenerationEffect myBatteryRegenerationEffect = new MyBatteryRegenerationEffect();
					myBatteryRegenerationEffect.Init(regenEffect);
					m_regenEffects.Add(m_regenEffects.Count, myBatteryRegenerationEffect);
				}
			}
			ResourceSink.Update();
		}

		public MyObjectBuilder_Battery GetObjectBuilder()
		{
			MyObjectBuilder_Battery myObjectBuilder_Battery = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Battery>();
			myObjectBuilder_Battery.ProducerEnabled = ResourceSource.Enabled;
			myObjectBuilder_Battery.CurrentCapacity = ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId);
			myObjectBuilder_Battery.RegenEffects = new List<MyObjectBuilder_BatteryRegenerationEffect>();
			foreach (KeyValuePair<int, MyBatteryRegenerationEffect> regenEffect in m_regenEffects)
			{
				myObjectBuilder_Battery.RegenEffects.Add(regenEffect.Value.GetObjectBuilder());
			}
			return myObjectBuilder_Battery;
		}

		public float Sink_ComputeRequiredPower()
		{
			float num = (1E-05f - ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId)) * 60f / 100f * ResourceSource.ProductionToCapacityMultiplierByType(MyResourceDistributorComponent.ElectricityId);
			float num2 = ResourceSource.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId);
			num2 *= (MySession.Static.CreativeMode ? 0f : 1f);
			return Math.Min(num + num2, 0.0018f);
		}

		public void UpdateOnServer100()
		{
			if (Sync.IsServer)
			{
				MyEntity parent = m_owner.Parent;
				if (m_lastParent != parent)
				{
					ResourceSink.Update();
					m_lastParent = parent;
				}
				if (ResourceSource.HasCapacityRemainingByType(MyResourceDistributorComponent.ElectricityId) || ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) > 0f)
				{
					float num = (float)(MySession.Static.GameplayFrameCounter - m_lastUpdateTime) * 0.0166666675f;
					m_lastUpdateTime = MySession.Static.GameplayFrameCounter;
					float num2 = ResourceSource.ProductionToCapacityMultiplierByType(MyResourceDistributorComponent.ElectricityId);
					float num3 = ResourceSource.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId) / num2;
					float num4 = MyFakes.ENABLE_BATTERY_SELF_RECHARGE ? ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId) : (RechargeMultiplier * ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) / num2);
					float num5 = MySession.Static.CreativeMode ? 0f : (num * num3);
					float num6 = num * num4 - num5;
					float value = ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId) + num6;
					ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, MathHelper.Clamp(value, 0f, 1E-05f));
				}
				if (!ResourceSource.HasCapacityRemainingByType(MyResourceDistributorComponent.ElectricityId))
				{
					ResourceSink.Update();
				}
				UpdateEffects100();
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SyncCapacitySuccess, Owner.EntityId, ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId));
			}
		}

		[Event(null, 219)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void SyncCapacitySuccess(long entityId, float remainingCapacity)
		{
			MyEntities.TryGetEntityById(entityId, out MyCharacter entity);
			entity?.SuitBattery.ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, remainingCapacity);
		}

		public void DebugDepleteBattery()
		{
			ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, 0f);
		}

		private void UpdateEffects100()
		{
			foreach (KeyValuePair<int, MyBatteryRegenerationEffect> regenEffect in m_regenEffects)
			{
				MyBatteryRegenerationEffect value = regenEffect.Value;
				if (value.RemainingTime <= MyTimeSpan.Zero)
				{
					m_tmpRemoveEffects.Add(regenEffect.Key);
				}
				else if (Sync.IsServer)
				{
					UpdateEffect100(value);
				}
			}
			foreach (int tmpRemoveEffect in m_tmpRemoveEffects)
			{
				RemoveEffect(tmpRemoveEffect);
			}
			m_tmpRemoveEffects.Clear();
		}

		private void UpdateEffect100(MyBatteryRegenerationEffect effect)
		{
			MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			MyTimeSpan myTimeSpan2 = myTimeSpan - effect.LastRegenTime;
			if (myTimeSpan2 > MyTimeSpan.Zero)
			{
				_ = effect.RemainingTime.Seconds;
				_ = 0.0;
				float num = ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId);
				float num2 = (float)(9.9999997473787516E-06 * (Math.Min(myTimeSpan2.Seconds, effect.RemainingTime.Seconds) * (double)effect.ChargePerSecond));
				ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, Math.Min(1E-05f, num + num2));
				effect.LastRegenTime = myTimeSpan;
				effect.RemainingTime -= myTimeSpan2;
			}
		}

		public int AddEffect(float chargePerSecond, double miliseconds)
		{
			MyBatteryRegenerationEffect value = new MyBatteryRegenerationEffect
			{
				ChargePerSecond = chargePerSecond,
				RemainingTime = MyTimeSpan.FromMilliseconds(miliseconds),
				LastRegenTime = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds)
			};
			int i;
			for (i = 0; i < m_regenEffects.Count && m_regenEffects.ContainsKey(i); i++)
			{
			}
			m_regenEffects.Add(i, value);
			return i;
		}

		public void RemoveEffect(int id)
		{
			m_regenEffects.Remove(id);
		}

		internal void Consume(MyFixedPoint amount, MyConsumableItemDefinition consumableDef)
		{
			if (consumableDef != null)
			{
				foreach (MyConsumableItemDefinition.StatValue stat in consumableDef.Stats)
				{
					if (string.Equals(stat.Name, BATTERY_CHARGE_STAT_NAME))
					{
						AddEffect((float)amount.ToIntSafe() * stat.Value, stat.Time * 1000f);
					}
				}
			}
		}
	}
}
