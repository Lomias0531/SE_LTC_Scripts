using Sandbox.Definitions;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Common;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MyFactoryTag(typeof(MyObjectBuilder_EntityStat), true)]
	public class MyEntityStat
	{
		public delegate void StatChangedDelegate(float newValue, float oldValue, object statChangeData);

		protected float m_currentValue;

		private float m_lastSyncValue;

		protected float m_minValue;

		protected float m_maxValue;

		protected float m_defaultValue;

		private bool m_syncFlag;

		private Dictionary<int, MyEntityStatRegenEffect> m_effects = new Dictionary<int, MyEntityStatRegenEffect>();

		private static List<int> m_tmpRemoveEffects = new List<int>();

		private int m_updateCounter;

		private float m_statRegenLeft;

		private float m_regenAmountMultiplier = 1f;

		private float m_regenAmountMultiplierDuration;

		private int m_regenAmountMultiplierTimeStart;

		private int m_regenAmountMultiplierTimeAlive;

		private bool m_regenAmountMultiplierActive;

		private MyStringHash m_statId;

		public MyEntityStatDefinition StatDefinition;

		public float Value
		{
			get
			{
				return m_currentValue;
			}
			set
			{
				SetValue(value, null);
			}
		}

		public float CurrentRatio => Value / (MaxValue - MinValue);

		public float MinValue => m_minValue;

		public float MaxValue => m_maxValue;

		public float DefaultValue => m_defaultValue;

		public bool ShouldSync => m_syncFlag;

		public float StatRegenLeft
		{
			get
			{
				return m_statRegenLeft;
			}
			set
			{
				m_statRegenLeft = value;
			}
		}

		public MyStringHash StatId => m_statId;

		public event StatChangedDelegate OnStatChanged;

		public virtual void Init(MyObjectBuilder_Base objectBuilder)
		{
			MyObjectBuilder_EntityStat myObjectBuilder_EntityStat = (MyObjectBuilder_EntityStat)objectBuilder;
			MyDefinitionManager.Static.TryGetDefinition(new MyDefinitionId(myObjectBuilder_EntityStat.TypeId, myObjectBuilder_EntityStat.SubtypeId), out MyEntityStatDefinition definition);
			StatDefinition = definition;
			m_maxValue = definition.MaxValue;
			m_minValue = definition.MinValue;
			m_currentValue = myObjectBuilder_EntityStat.Value * m_maxValue;
			m_defaultValue = definition.DefaultValue;
			m_lastSyncValue = m_currentValue;
			m_statId = MyStringHash.GetOrCompute(definition.Name);
			m_regenAmountMultiplier = myObjectBuilder_EntityStat.StatRegenAmountMultiplier;
			m_regenAmountMultiplierDuration = myObjectBuilder_EntityStat.StatRegenAmountMultiplierDuration;
			m_regenAmountMultiplierTimeStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_regenAmountMultiplierTimeAlive = 0;
			m_regenAmountMultiplierActive = (m_regenAmountMultiplierDuration > 0f);
			ClearEffects();
			if (myObjectBuilder_EntityStat.Effects != null)
			{
				MyObjectBuilder_EntityStatRegenEffect[] effects = myObjectBuilder_EntityStat.Effects;
				foreach (MyObjectBuilder_EntityStatRegenEffect objectBuilder2 in effects)
				{
					AddEffect(objectBuilder2);
				}
			}
		}

		public virtual MyObjectBuilder_EntityStat GetObjectBuilder()
		{
			MyObjectBuilder_EntityStat myObjectBuilder_EntityStat = new MyObjectBuilder_EntityStat();
			MyEntityStatDefinition myEntityStatDefinition = MyDefinitionManager.Static.GetDefinition(new MyDefinitionId(myObjectBuilder_EntityStat.TypeId, StatDefinition.Id.SubtypeId)) as MyEntityStatDefinition;
			myObjectBuilder_EntityStat.SubtypeName = StatDefinition.Id.SubtypeName;
			if (myEntityStatDefinition != null)
			{
				myObjectBuilder_EntityStat.Value = m_currentValue / ((myEntityStatDefinition.MaxValue != 0f) ? myEntityStatDefinition.MaxValue : 1f);
				myObjectBuilder_EntityStat.MaxValue = m_maxValue / ((myEntityStatDefinition.MaxValue != 0f) ? myEntityStatDefinition.MaxValue : 1f);
			}
			else
			{
				myObjectBuilder_EntityStat.Value = m_currentValue / m_maxValue;
				myObjectBuilder_EntityStat.MaxValue = 1f;
			}
			if (m_regenAmountMultiplierActive)
			{
				myObjectBuilder_EntityStat.StatRegenAmountMultiplier = m_regenAmountMultiplier;
				myObjectBuilder_EntityStat.StatRegenAmountMultiplierDuration = m_regenAmountMultiplierDuration;
			}
			myObjectBuilder_EntityStat.Effects = null;
			if (m_effects != null && m_effects.Count > 0)
			{
				int num = m_effects.Count;
				foreach (KeyValuePair<int, MyEntityStatRegenEffect> effect in m_effects)
				{
					if (effect.Value.Duration < 0f)
					{
						num--;
					}
				}
				if (num > 0)
				{
					myObjectBuilder_EntityStat.Effects = new MyObjectBuilder_EntityStatRegenEffect[num];
					int num2 = 0;
					{
						foreach (KeyValuePair<int, MyEntityStatRegenEffect> effect2 in m_effects)
						{
							if (effect2.Value.Duration >= 0f)
							{
								myObjectBuilder_EntityStat.Effects[num2++] = effect2.Value.GetObjectBuilder();
							}
						}
						return myObjectBuilder_EntityStat;
					}
				}
			}
			return myObjectBuilder_EntityStat;
		}

		public void ApplyRegenAmountMultiplier(float amountMultiplier = 1f, float duration = 2f)
		{
			m_regenAmountMultiplier = amountMultiplier;
			m_regenAmountMultiplierDuration = duration;
			m_regenAmountMultiplierTimeStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_regenAmountMultiplierActive = (duration > 0f);
		}

		public void ResetRegenAmountMultiplier()
		{
			m_regenAmountMultiplier = 1f;
			m_regenAmountMultiplierActive = false;
		}

		private void UpdateRegenAmountMultiplier()
		{
			if (m_regenAmountMultiplierActive)
			{
				m_regenAmountMultiplierTimeAlive = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_regenAmountMultiplierTimeStart;
				if ((float)m_regenAmountMultiplierTimeAlive >= m_regenAmountMultiplierDuration * 1000f)
				{
					m_regenAmountMultiplier = 1f;
					m_regenAmountMultiplierDuration = 0f;
					m_regenAmountMultiplierActive = false;
				}
			}
		}

		public float GetEfficiencyMultiplier(float multiplier, float threshold)
		{
			if (!(CurrentRatio < threshold))
			{
				return 1f;
			}
			return multiplier;
		}

		public int AddEffect(float amount, float interval, float duration = -1f, float minRegenRatio = 0f, float maxRegenRatio = 1f)
		{
			MyObjectBuilder_EntityStatRegenEffect objectBuilder = new MyObjectBuilder_EntityStatRegenEffect
			{
				TickAmount = amount,
				Interval = interval,
				Duration = duration,
				MinRegenRatio = minRegenRatio,
				MaxRegenRatio = maxRegenRatio
			};
			return AddEffect(objectBuilder);
		}

		public int AddEffect(MyObjectBuilder_EntityStatRegenEffect objectBuilder)
		{
			MyEntityStatRegenEffect myEntityStatRegenEffect = MyEntityStatEffectFactory.CreateInstance(objectBuilder);
			myEntityStatRegenEffect.Init(objectBuilder, this);
			int i;
			for (i = 0; i < m_effects.Count && m_effects.ContainsKey(i); i++)
			{
			}
			m_effects.Add(i, myEntityStatRegenEffect);
			return i;
		}

		public virtual void Update()
		{
			m_syncFlag = false;
			UpdateRegenAmountMultiplier();
			foreach (KeyValuePair<int, MyEntityStatRegenEffect> effect in m_effects)
			{
				MyEntityStatRegenEffect value = effect.Value;
				if (value.Duration >= 0f && (float)value.AliveTime >= value.Duration * 1000f)
				{
					m_tmpRemoveEffects.Add(effect.Key);
				}
				if (Sync.IsServer && value.Enabled)
				{
					if (m_regenAmountMultiplierActive)
					{
						value.Update(m_regenAmountMultiplier);
					}
					else
					{
						value.Update();
					}
				}
			}
			foreach (int tmpRemoveEffect in m_tmpRemoveEffects)
			{
				RemoveEffect(tmpRemoveEffect);
			}
			m_tmpRemoveEffects.Clear();
			if ((m_updateCounter++ % 10 == 0 || (double)Math.Abs(Value - MinValue) <= 0.001) && m_lastSyncValue != m_currentValue)
			{
				m_syncFlag = true;
				m_lastSyncValue = m_currentValue;
			}
		}

		private void SetValue(float newValue, object statChangeData)
		{
			float currentValue = m_currentValue;
			m_currentValue = MathHelper.Clamp(newValue, MinValue, MaxValue);
			if (this.OnStatChanged != null && newValue != currentValue)
			{
				this.OnStatChanged(newValue, currentValue, statChangeData);
			}
		}

		public bool RemoveEffect(int id)
		{
			MyEntityStatRegenEffect value = null;
			if (m_effects.TryGetValue(id, out value))
			{
				value.Closing();
			}
			return m_effects.Remove(id);
		}

		public void ClearEffects()
		{
			foreach (KeyValuePair<int, MyEntityStatRegenEffect> effect in m_effects)
			{
				effect.Value.Closing();
			}
			m_effects.Clear();
		}

		public bool TryGetEffect(int id, out MyEntityStatRegenEffect outEffect)
		{
			return m_effects.TryGetValue(id, out outEffect);
		}

		public DictionaryReader<int, MyEntityStatRegenEffect> GetEffects()
		{
			return m_effects;
		}

		public MyEntityStatRegenEffect GetEffect(int id)
		{
			MyEntityStatRegenEffect value = null;
			if (!m_effects.TryGetValue(id, out value))
			{
				return null;
			}
			return value;
		}

		public override string ToString()
		{
			return m_statId.ToString();
		}

		public void Increase(float amount, object statChangeData)
		{
			SetValue(Value + amount, statChangeData);
		}

		public void Decrease(float amount, object statChangeData)
		{
			SetValue(Value - amount, statChangeData);
		}

		public float CalculateRegenLeftForLongestEffect()
		{
			MyEntityStatRegenEffect myEntityStatRegenEffect = null;
			m_statRegenLeft = 0f;
			foreach (KeyValuePair<int, MyEntityStatRegenEffect> effect in m_effects)
			{
				if (effect.Value.Duration > 0f)
				{
					m_statRegenLeft += effect.Value.AmountLeftOverDuration;
					if (myEntityStatRegenEffect == null || effect.Value.DeathTime > myEntityStatRegenEffect.DeathTime)
					{
						myEntityStatRegenEffect = effect.Value;
					}
				}
			}
			if (myEntityStatRegenEffect == null)
			{
				return m_statRegenLeft;
			}
			foreach (KeyValuePair<int, MyEntityStatRegenEffect> effect2 in m_effects)
			{
				if (effect2.Value.Duration < 0f)
				{
					m_statRegenLeft += effect2.Value.Amount * (float)effect2.Value.CalculateTicksBetweenTimes(myEntityStatRegenEffect.LastRegenTime, myEntityStatRegenEffect.DeathTime);
				}
			}
			return m_statRegenLeft;
		}
	}
}
