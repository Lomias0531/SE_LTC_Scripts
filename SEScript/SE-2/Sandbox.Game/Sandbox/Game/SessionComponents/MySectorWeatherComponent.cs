using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 555, typeof(MyObjectBuilder_SectorWeatherComponent), null)]
	public class MySectorWeatherComponent : MySessionComponentBase
	{
		public const float ExtremeFreeze = 0f;

		public const float Freeze = 0.25f;

		public const float Cozy = 0.5f;

		public const float Hot = 0.75f;

		public const float ExtremeHot = 1f;

		private float m_speed;

		private Vector3 m_sunRotationAxis;

		private Vector3 m_baseSunDirection;

		private bool m_enabled;

		public bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				if (m_enabled != value)
				{
					m_enabled = value;
					if (Enabled)
					{
						UpdateSunProperties();
					}
				}
				MySession.Static.Settings.EnableSunRotation = value;
			}
		}

		public float RotationInterval
		{
			get
			{
				return m_speed;
			}
			set
			{
				m_speed = value;
				Enabled = (m_speed != 0f);
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyObjectBuilder_SectorWeatherComponent myObjectBuilder_SectorWeatherComponent = (MyObjectBuilder_SectorWeatherComponent)sessionComponent;
			m_speed = 60f * MySession.Static.Settings.SunRotationIntervalMinutes;
			if (!myObjectBuilder_SectorWeatherComponent.BaseSunDirection.IsZero)
			{
				m_baseSunDirection = myObjectBuilder_SectorWeatherComponent.BaseSunDirection;
			}
			Enabled = MySession.Static.Settings.EnableSunRotation;
		}

		public override void BeforeStart()
		{
			if (Enabled)
			{
				UpdateSunProperties();
			}
		}

		private void UpdateSunProperties()
		{
			if ((double)(Math.Abs(m_baseSunDirection.X) + Math.Abs(m_baseSunDirection.Y) + Math.Abs(m_baseSunDirection.Z)) < 0.001)
			{
				m_baseSunDirection = MySector.SunProperties.BaseSunDirectionNormalized;
				m_sunRotationAxis = MySector.SunProperties.SunRotationAxis;
				if (MySession.Static.ElapsedGameTime.Ticks != 0L)
				{
					float angle = -6.283186f * (float)(MySession.Static.ElapsedGameTime.TotalSeconds / (double)m_speed);
					Vector3 baseSunDirection = Vector3.Transform(m_baseSunDirection, Matrix.CreateFromAxisAngle(m_sunRotationAxis, angle));
					baseSunDirection.Normalize();
					m_baseSunDirection = baseSunDirection;
				}
			}
			else
			{
				m_sunRotationAxis = MySector.SunProperties.SunRotationAxis;
			}
			MySector.SunProperties.SunDirectionNormalized = CalculateSunDirection();
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_SectorWeatherComponent obj = (MyObjectBuilder_SectorWeatherComponent)base.GetObjectBuilder();
			obj.BaseSunDirection = m_baseSunDirection;
			return obj;
		}

		public override void UpdateBeforeSimulation()
		{
			if (Enabled)
			{
				Vector3 vector = MySector.SunProperties.SunDirectionNormalized = CalculateSunDirection();
			}
		}

		private Vector3 CalculateSunDirection()
		{
			float angle = 6.283186f * (float)(MySession.Static.ElapsedGameTime.TotalSeconds / (double)m_speed);
			Vector3 result = Vector3.Transform(m_baseSunDirection, Matrix.CreateFromAxisAngle(m_sunRotationAxis, angle));
			result.Normalize();
			return result;
		}

		public static float GetTemperatureInPoint(Vector3D worldPoint)
		{
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(worldPoint);
			if (closestPlanet == null)
			{
				return 0f;
			}
			float oxygenInPoint = MyOxygenProviderSystem.GetOxygenInPoint(worldPoint);
			if (oxygenInPoint < 0.01f)
			{
				return 0f;
			}
			oxygenInPoint = MathHelper.Saturate(oxygenInPoint / 0.6f);
			float num = (float)Vector3D.Distance(closestPlanet.PositionComp.GetPosition(), worldPoint) / closestPlanet.AverageRadius;
			float num2 = (Vector3.Dot(-MySector.SunProperties.SunDirectionNormalized, Vector3.Normalize(worldPoint - closestPlanet.PositionComp.GetPosition())) + 1f) / 2f;
			num2 = 1f - (float)Math.Pow(1f - num2, 0.5);
			float value = MathHelper.Lerp(0.5f, 0.25f, num2);
			float num3 = 0f;
			if (num < 1f)
			{
				float num4 = 0.8f;
				float amount = MathHelper.Saturate(num / num4);
				return MathHelper.Lerp(1f, value, amount);
			}
			return MathHelper.Lerp(0f, value, oxygenInPoint);
		}

		public static MyTemperatureLevel TemperatureToLevel(float temperature)
		{
			if (temperature < 0.125f)
			{
				return MyTemperatureLevel.ExtremeFreeze;
			}
			if (temperature < 0.375f)
			{
				return MyTemperatureLevel.Freeze;
			}
			if (temperature < 0.625f)
			{
				return MyTemperatureLevel.Cozy;
			}
			if (temperature < 0.875f)
			{
				return MyTemperatureLevel.Hot;
			}
			return MyTemperatureLevel.ExtremeHot;
		}

		public static bool IsOnDarkSide(Vector3D point)
		{
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(point);
			if (closestPlanet == null)
			{
				return false;
			}
			return IsThereNight(closestPlanet, ref point);
		}

		public static bool IsThereNight(MyPlanet planet, ref Vector3D position)
		{
			Vector3D value = position - planet.PositionComp.GetPosition();
			if ((float)value.Length() > planet.MaximumRadius * 1.1f)
			{
				return false;
			}
			Vector3 vector = Vector3.Normalize(value);
			return Vector3.Dot(MySector.DirectionToSunNormalized, vector) < -0.1f;
		}
	}
}
