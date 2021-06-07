using System;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Utils
{
	public class MyCameraShake
	{
		public float MaxShake = 15f;

		public float MaxShakePosX = 0.8f;

		public float MaxShakePosY = 0.2f;

		public float MaxShakePosZ = 0.8f;

		public float MaxShakeDir = 0.2f;

		public float Reduction = 0.6f;

		public float Dampening = 0.8f;

		public float OffConstant = 0.01f;

		public float DirReduction = 0.35f;

		private bool m_shakeEnabled;

		private Vector3 m_shakePos;

		private Vector3 m_shakeDir;

		private float m_currentShakePosPower;

		private float m_currentShakeDirPower;

		public bool ShakeEnabled
		{
			get
			{
				return m_shakeEnabled;
			}
			set
			{
				m_shakeEnabled = value;
			}
		}

		public Vector3 ShakePos => m_shakePos;

		public Vector3 ShakeDir => m_shakeDir;

		public MyCameraShake()
		{
			m_shakeEnabled = false;
			m_currentShakeDirPower = 0f;
			m_currentShakePosPower = 0f;
		}

		public bool ShakeActive()
		{
			return m_shakeEnabled;
		}

		public void AddShake(float shakePower)
		{
			if (!MyUtils.IsZero(shakePower) && !MyUtils.IsZero(MaxShake))
			{
				float num = MathHelper.Clamp(shakePower / MaxShake, 0f, 1f);
				if (m_currentShakePosPower < num)
				{
					m_currentShakePosPower = num;
				}
				if (m_currentShakeDirPower < num * DirReduction)
				{
					m_currentShakeDirPower = num * DirReduction;
				}
				m_shakePos = new Vector3(m_currentShakePosPower * MaxShakePosX, m_currentShakePosPower * MaxShakePosY, m_currentShakePosPower * MaxShakePosZ);
				m_shakeDir = new Vector3(m_currentShakeDirPower * MaxShakeDir, m_currentShakeDirPower * MaxShakeDir, 0f);
				m_shakeEnabled = true;
			}
		}

		public void UpdateShake(float timeStep, out Vector3 outPos, out Vector3 outDir)
		{
			if (!m_shakeEnabled)
			{
				outPos = Vector3.Zero;
				outDir = Vector3.Zero;
				return;
			}
			m_shakePos.X *= MyUtils.GetRandomSign();
			m_shakePos.Y *= MyUtils.GetRandomSign();
			m_shakePos.Z *= MyUtils.GetRandomSign();
			outPos.X = m_shakePos.X * Math.Abs(m_shakePos.X) * Reduction;
			outPos.Y = m_shakePos.Y * Math.Abs(m_shakePos.Y) * Reduction;
			outPos.Z = m_shakePos.Z * Math.Abs(m_shakePos.Z) * Reduction;
			m_shakeDir.X *= MyUtils.GetRandomSign();
			m_shakeDir.Y *= MyUtils.GetRandomSign();
			m_shakeDir.Z *= MyUtils.GetRandomSign();
			outDir.X = m_shakeDir.X * Math.Abs(m_shakeDir.X) * 100f;
			outDir.Y = m_shakeDir.Y * Math.Abs(m_shakeDir.Y) * 100f;
			outDir.Z = m_shakeDir.Z * Math.Abs(m_shakeDir.Z) * 100f;
			outDir *= DirReduction;
			m_currentShakePosPower *= (float)Math.Pow(Dampening, timeStep * 60f);
			m_currentShakeDirPower *= (float)Math.Pow(Dampening, timeStep * 60f);
			if (m_currentShakeDirPower < 0f)
			{
				m_currentShakeDirPower = 0f;
			}
			if (m_currentShakePosPower < 0f)
			{
				m_currentShakePosPower = 0f;
			}
			m_shakePos = new Vector3(m_currentShakePosPower * MaxShakePosX, m_currentShakePosPower * MaxShakePosY, m_currentShakePosPower * MaxShakePosZ);
			m_shakeDir = new Vector3(m_currentShakeDirPower * MaxShakeDir, m_currentShakeDirPower * MaxShakeDir, 0f);
			if (m_currentShakeDirPower < OffConstant && m_currentShakePosPower < OffConstant)
			{
				m_currentShakeDirPower = 0f;
				m_currentShakePosPower = 0f;
				m_shakeEnabled = false;
			}
		}
	}
}
