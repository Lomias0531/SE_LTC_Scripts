using System;
using VRage.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public abstract class MyStatBase : IMyHudStat
	{
		private float m_currentValue;

		private string m_valueStringCache;

		public MyStringHash Id
		{
			get;
			protected set;
		}

		public float CurrentValue
		{
			get
			{
				return m_currentValue;
			}
			protected set
			{
				if (!m_currentValue.IsEqual(value))
				{
					m_currentValue = value;
					m_valueStringCache = null;
				}
			}
		}

		public virtual float MaxValue => 1f;

		public virtual float MinValue => 0f;

		public abstract void Update();

		public override string ToString()
		{
			return $"{CurrentValue:0.00}";
		}

		public string GetValueString()
		{
			if (m_valueStringCache == null)
			{
				m_valueStringCache = ToString();
			}
			return m_valueStringCache;
		}
	}
}
