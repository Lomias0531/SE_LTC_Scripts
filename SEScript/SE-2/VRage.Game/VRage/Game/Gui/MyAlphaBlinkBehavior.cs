using System;
using System.Xml.Serialization;
using VRage.Library.Utils;
using VRageMath;

namespace VRage.Game.GUI
{
	public class MyAlphaBlinkBehavior
	{
		private static readonly MyGameTimer TIMER = new MyGameTimer();

		private int m_intervalLenghtMs = 2000;

		private float m_minAlpha = 0.2f;

		private float m_maxAlpha = 0.8f;

		private float m_currentBlinkAlpha = 1f;

		public float MinAlpha
		{
			get
			{
				return m_minAlpha;
			}
			set
			{
				m_minAlpha = MathHelper.Clamp(value, 0f, 1f);
			}
		}

		public float MaxAlpha
		{
			get
			{
				return m_maxAlpha;
			}
			set
			{
				m_maxAlpha = MathHelper.Clamp(value, 0f, 1f);
			}
		}

		public int IntervalMs
		{
			get
			{
				return m_intervalLenghtMs;
			}
			set
			{
				m_intervalLenghtMs = MathHelper.Clamp(value, 0, int.MaxValue);
			}
		}

		public Vector4? ColorMask
		{
			get;
			set;
		}

		[XmlIgnore]
		public float CurrentBlinkAlpha
		{
			get
			{
				return m_currentBlinkAlpha;
			}
			set
			{
				m_currentBlinkAlpha = MathHelper.Clamp(value, 0f, 1f);
			}
		}

		public bool Blink
		{
			get;
			set;
		}

		/// <summary>
		/// Call update to get new current alpha value.
		/// </summary>
		public virtual void UpdateBlink()
		{
			if (Blink)
			{
				double totalMilliseconds = TIMER.ElapsedTimeSpan.TotalMilliseconds;
				CurrentBlinkAlpha = m_minAlpha + (float)((Math.Cos(totalMilliseconds / (double)m_intervalLenghtMs * Math.PI * 2.0) + 1.0) * 0.5 * (double)(m_maxAlpha - m_minAlpha));
			}
			else
			{
				CurrentBlinkAlpha = 1f;
			}
		}
	}
}
