using Sandbox.Game.Multiplayer;
using System;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MyEntityStatEffectType(typeof(MyObjectBuilder_EntityStatRegenEffect))]
	public class MyEntityStatRegenEffect
	{
		protected float m_amount;

		protected float m_interval;

		protected float m_maxRegenRatio;

		protected float m_minRegenRatio;

		protected float m_duration;

		protected int m_lastRegenTime;

		private readonly int m_birthTime;

		private bool m_enabled;

		private MyEntityStat m_parentStat;

		public float Amount
		{
			get
			{
				return m_amount;
			}
			set
			{
				m_amount = value;
			}
		}

		public float AmountLeftOverDuration => m_amount * (float)TicksLeft + PartialEndAmount;

		public int TicksLeft => CalculateTicksBetweenTimes(m_lastRegenTime, DeathTime);

		private float PartialEndAmount
		{
			get
			{
				float num = m_duration / m_interval;
				return (num - (float)Math.Truncate(num)) * m_amount;
			}
		}

		public float Interval
		{
			get
			{
				return m_interval;
			}
			set
			{
				m_interval = value;
			}
		}

		public float Duration => m_duration;

		public int LastRegenTime => m_lastRegenTime;

		public int BirthTime => m_birthTime;

		public int DeathTime
		{
			get
			{
				if (!(Duration >= 0f))
				{
					return int.MaxValue;
				}
				return m_birthTime + (int)(m_duration * 1000f);
			}
		}

		public int AliveTime => MySandboxGame.TotalGamePlayTimeInMilliseconds - BirthTime;

		public bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				m_enabled = value;
			}
		}

		public MyEntityStatRegenEffect()
		{
			m_lastRegenTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_birthTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			Enabled = true;
		}

		public virtual void Init(MyObjectBuilder_Base objectBuilder, MyEntityStat parentStat)
		{
			m_parentStat = parentStat;
			MyObjectBuilder_EntityStatRegenEffect myObjectBuilder_EntityStatRegenEffect = objectBuilder as MyObjectBuilder_EntityStatRegenEffect;
			if (myObjectBuilder_EntityStatRegenEffect != null && !(myObjectBuilder_EntityStatRegenEffect.Interval <= 0f))
			{
				m_amount = myObjectBuilder_EntityStatRegenEffect.TickAmount;
				m_interval = myObjectBuilder_EntityStatRegenEffect.Interval;
				m_maxRegenRatio = myObjectBuilder_EntityStatRegenEffect.MaxRegenRatio;
				m_minRegenRatio = myObjectBuilder_EntityStatRegenEffect.MinRegenRatio;
				m_duration = myObjectBuilder_EntityStatRegenEffect.Duration - myObjectBuilder_EntityStatRegenEffect.AliveTime / 1000f;
				ResetRegenTime();
			}
		}

		public virtual MyObjectBuilder_EntityStatRegenEffect GetObjectBuilder()
		{
			return new MyObjectBuilder_EntityStatRegenEffect
			{
				TickAmount = m_amount,
				Interval = m_interval,
				MaxRegenRatio = m_maxRegenRatio,
				MinRegenRatio = m_minRegenRatio,
				Duration = m_duration,
				AliveTime = AliveTime
			};
		}

		public virtual void Closing()
		{
			if (Sync.IsServer)
			{
				IncreaseByRemainingValue();
			}
		}

		public virtual void Update(float regenAmountMultiplier = 1f)
		{
			if (m_interval <= 0f)
			{
				return;
			}
			bool flag = m_duration == 0f;
			while (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastRegenTime >= 0 || flag)
			{
				if (m_amount > 0f && m_parentStat.Value < m_parentStat.MaxValue * m_maxRegenRatio)
				{
					m_parentStat.Value = MathHelper.Clamp(m_parentStat.Value + m_parentStat.MaxValue * m_amount * regenAmountMultiplier, m_parentStat.Value, m_parentStat.MaxValue * m_maxRegenRatio);
				}
				else if (m_amount < 0f && m_parentStat.Value > Math.Max(m_parentStat.MinValue, m_parentStat.MaxValue * m_minRegenRatio))
				{
					m_parentStat.Value = MathHelper.Clamp(m_parentStat.Value + m_parentStat.MaxValue * m_amount, Math.Max(m_parentStat.MaxValue * m_minRegenRatio, m_parentStat.MinValue), m_parentStat.Value);
				}
				m_lastRegenTime += (int)Math.Round(m_interval * 1000f);
				flag = false;
			}
		}

		public int CalculateTicksBetweenTimes(int startTime, int endTime)
		{
			if (startTime < m_birthTime || startTime >= endTime)
			{
				return 0;
			}
			startTime = Math.Max(startTime, m_lastRegenTime);
			endTime = Math.Min(endTime, DeathTime);
			return Math.Max((int)((double)(endTime - startTime) / Math.Round(m_interval * 1000f)), 0);
		}

		public void SetAmountAndInterval(float amount, float interval, bool increaseByRemaining)
		{
			if (amount != Amount || interval != Interval)
			{
				if (increaseByRemaining)
				{
					IncreaseByRemainingValue();
				}
				Amount = amount;
				Interval = interval;
				ResetRegenTime();
			}
		}

		public void ResetRegenTime()
		{
			m_lastRegenTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + (int)Math.Round(m_interval * 1000f);
		}

		private void IncreaseByRemainingValue()
		{
			if (m_interval <= 0f || !Enabled)
			{
				return;
			}
			float num = 1f - (float)(m_lastRegenTime - MySandboxGame.TotalGamePlayTimeInMilliseconds) / (m_interval * 1000f);
			if (!(num <= 0f))
			{
				if (m_amount > 0f && m_parentStat.Value < m_parentStat.MaxValue)
				{
					m_parentStat.Value = MathHelper.Clamp(m_parentStat.Value + m_amount * num, m_parentStat.MinValue, Math.Max(m_parentStat.MaxValue * m_maxRegenRatio, m_parentStat.MaxValue));
				}
				else if (m_amount < 0f && m_parentStat.Value > m_parentStat.MinValue)
				{
					m_parentStat.Value = MathHelper.Clamp(m_parentStat.Value + m_amount * num, Math.Max(m_parentStat.MaxValue * m_minRegenRatio, m_parentStat.MinValue), m_parentStat.MaxValue);
				}
			}
		}

		public override string ToString()
		{
			return m_parentStat.ToString() + ": (" + m_amount + "/" + m_interval + "/" + m_duration + ")";
		}
	}
}
