using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.EntityComponents
{
	[MyStatLogicDescriptor("SpaceStatEffect")]
	public class MySpaceStatEffect : MyStatLogic
	{
		private static MyStringHash HealthId = MyStringHash.GetOrCompute("Health");

		public static readonly float MAX_REGEN_HEALTH_RATIO = 0.7f;

		private int m_healthEffectId;

		private bool m_effectCreated;

		private MyEntityStat Health
		{
			get
			{
				if (m_stats.TryGetValue(HealthId, out MyEntityStat value))
				{
					return value;
				}
				return null;
			}
		}

		public override void Init(IMyCharacter character, Dictionary<MyStringHash, MyEntityStat> stats, string scriptName)
		{
			base.Init(character, stats, scriptName);
			InitActions();
			MyEntityStat health = Health;
			if (health != null)
			{
				health.OnStatChanged += OnHealthChanged;
			}
		}

		public override void Close()
		{
			MyEntityStat health = Health;
			if (health != null)
			{
				health.OnStatChanged -= OnHealthChanged;
			}
			ClearRegenEffect();
			base.Close();
		}

		public override void Update10()
		{
			base.Update10();
			if (!MySession.Static.Settings.EnableOxygen || !base.EnableAutoHealing)
			{
				return;
			}
			if ((MySession.Static.Settings.EnableOxygenPressurization ? Math.Max(base.Character.EnvironmentOxygenLevel, base.Character.OxygenLevel) : base.Character.EnvironmentOxygenLevel) > MyEffectConstants.MinOxygenLevelForHealthRegeneration)
			{
				if (!m_effectCreated)
				{
					CreateRegenEffect();
				}
			}
			else if (m_effectCreated)
			{
				ClearRegenEffect();
			}
		}

		private void OnHealthChanged(float newValue, float oldValue, object statChangeData)
		{
			MyEntityStat health = Health;
			if (health != null && health.Value - health.MinValue < 0.001f && base.Character != null)
			{
				base.Character.Kill(statChangeData);
			}
		}

		private void CreateRegenEffect()
		{
			MyObjectBuilder_EntityStatRegenEffect myObjectBuilder_EntityStatRegenEffect = new MyObjectBuilder_EntityStatRegenEffect();
			MyEntityStat health = Health;
			if (health != null)
			{
				myObjectBuilder_EntityStatRegenEffect.TickAmount = MyEffectConstants.HealthTick;
				myObjectBuilder_EntityStatRegenEffect.Interval = MyEffectConstants.HealthInterval;
				myObjectBuilder_EntityStatRegenEffect.MaxRegenRatio = MAX_REGEN_HEALTH_RATIO;
				myObjectBuilder_EntityStatRegenEffect.MinRegenRatio = 0f;
				m_healthEffectId = health.AddEffect(myObjectBuilder_EntityStatRegenEffect);
				m_effectCreated = true;
			}
		}

		private void ClearRegenEffect()
		{
			MyEntityStat health = Health;
			if (health != null)
			{
				health.RemoveEffect(m_healthEffectId);
				m_effectCreated = false;
			}
		}

		private void InitActions()
		{
			MyStatAction action = default(MyStatAction);
			string actionId = "MedRoomHeal";
			action.StatId = HealthId;
			action.Cost = MyEffectConstants.MedRoomHeal;
			AddAction(actionId, action);
			action = default(MyStatAction);
			actionId = "GenericHeal";
			action.StatId = HealthId;
			action.Cost = MyEffectConstants.GenericHeal;
			AddAction(actionId, action);
		}
	}
}
