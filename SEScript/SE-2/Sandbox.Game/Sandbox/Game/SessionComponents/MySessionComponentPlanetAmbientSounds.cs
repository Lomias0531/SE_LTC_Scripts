using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage.Audio;
using VRage.Game.Components;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	internal class MySessionComponentPlanetAmbientSounds : MySessionComponentBase
	{
		private IMySourceVoice m_sound;

		private IMyAudioEffect m_effect;

		private readonly MyStringHash m_crossFade = MyStringHash.GetOrCompute("CrossFade");

		private readonly MyStringHash m_fadeIn = MyStringHash.GetOrCompute("FadeIn");

		private readonly MyStringHash m_fadeOut = MyStringHash.GetOrCompute("FadeOut");

		private MyPlanet m_nearestPlanet;

		private long m_nextPlanetRecalculation = -1L;

		private int m_planetRecalculationIntervalInSpace = 300;

		private int m_planetRecalculationIntervalOnPlanet = 300;

		private float m_volumeModifier = 1f;

		private static float m_volumeModifierTarget = 1f;

		private float m_volumeOriginal = 1f;

		private const float VOLUME_CHANGE_SPEED = 0.25f;

		public float VolumeModifierGlobal = 1f;

		private MyPlanetEnvironmentalSoundRule[] m_nearestSoundRules;

		private readonly MyPlanetEnvironmentalSoundRule[] m_emptySoundRules = new MyPlanetEnvironmentalSoundRule[0];

		private MyPlanet Planet
		{
			get
			{
				return m_nearestPlanet;
			}
			set
			{
				SetNearestPlanet(value);
			}
		}

		public override bool IsRequiredByGame
		{
			get
			{
				if (base.IsRequiredByGame)
				{
					if (!MyFakes.ENABLE_PLANETS)
					{
						return MyInput.Static.ENABLE_DEVELOPER_KEYS;
					}
					return true;
				}
				return false;
			}
		}

		public override void LoadData()
		{
			base.LoadData();
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			if (m_sound != null)
			{
				m_sound.Stop();
			}
			m_nearestPlanet = null;
			m_nearestSoundRules = null;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			if (m_volumeModifier != m_volumeModifierTarget)
			{
				if (m_volumeModifier < m_volumeModifierTarget)
				{
					m_volumeModifier = MyMath.Clamp(m_volumeModifier + 0.004166667f, 0f, m_volumeModifierTarget);
				}
				else
				{
					m_volumeModifier = MyMath.Clamp(m_volumeModifier - 0.004166667f, m_volumeModifierTarget, 1f);
				}
				if (m_sound != null && m_sound.IsPlaying)
				{
					m_sound.SetVolume(m_volumeOriginal * m_volumeModifier * VolumeModifierGlobal);
				}
			}
			long num = MySession.Static.GameplayFrameCounter;
			if (num >= m_nextPlanetRecalculation)
			{
				Planet = FindNearestPlanet(MySector.MainCamera.Position);
				if (Planet == null)
				{
					m_nextPlanetRecalculation = num + m_planetRecalculationIntervalInSpace;
				}
				else
				{
					m_nextPlanetRecalculation = num + m_planetRecalculationIntervalOnPlanet;
				}
			}
			if (Planet == null || Planet.Provider == null || (MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.Settings.RealisticSound && !Planet.HasAtmosphere))
			{
				if (m_sound != null)
				{
					m_sound.Stop(force: true);
				}
				return;
			}
			Vector3D value = MySector.MainCamera.Position - Planet.PositionComp.GetPosition();
			double num2 = value.Length();
			float num3 = Planet.Provider.Shape.DistanceToRatio((float)num2);
			if (!(num3 < 0f))
			{
				Vector3D value2 = -value / num2;
				float angleFromEquator = (float)(0.0 - value2.Y);
				float sunAngleFromZenith = MySector.DirectionToSunNormalized.Dot(-value2);
				if (!FindSoundRuleIndex(angleFromEquator, num3, sunAngleFromZenith, m_nearestSoundRules, out int outRuleIndex))
				{
					PlaySound(default(MyCueId));
				}
				else
				{
					PlaySound(new MyCueId(m_nearestSoundRules[outRuleIndex].EnvironmentSound));
				}
			}
		}

		private static MyPlanet FindNearestPlanet(Vector3D worldPosition)
		{
			BoundingBoxD box = new BoundingBoxD(worldPosition, worldPosition);
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(ref box);
			if (closestPlanet != null && (double)closestPlanet.AtmosphereAltitude > Vector3D.Distance(worldPosition, closestPlanet.PositionComp.GetPosition()))
			{
				return null;
			}
			return closestPlanet;
		}

		private void SetNearestPlanet(MyPlanet planet)
		{
			m_nearestPlanet = planet;
			if (m_nearestPlanet != null && m_nearestPlanet.Generator != null)
			{
				m_nearestSoundRules = (m_nearestPlanet.Generator.SoundRules ?? m_emptySoundRules);
			}
		}

		private static bool FindSoundRuleIndex(float angleFromEquator, float height, float sunAngleFromZenith, MyPlanetEnvironmentalSoundRule[] soundRules, out int outRuleIndex)
		{
			outRuleIndex = -1;
			if (soundRules == null)
			{
				return false;
			}
			for (int i = 0; i < soundRules.Length; i++)
			{
				if (soundRules[i].Check(angleFromEquator, height, sunAngleFromZenith))
				{
					outRuleIndex = i;
					return true;
				}
			}
			return false;
		}

		private void PlaySound(MyCueId sound)
		{
			if (m_sound == null || !m_sound.IsPlaying)
			{
				m_sound = MyAudio.Static.PlaySound(sound);
				if (!sound.IsNull)
				{
					m_effect = MyAudio.Static.ApplyEffect(m_sound, m_fadeIn);
				}
				if (m_effect != null)
				{
					m_sound = m_effect.OutputSound;
				}
			}
			else if (m_effect != null && m_effect.Finished && sound.IsNull)
			{
				m_sound.Stop(force: true);
			}
			else if (m_sound.CueEnum != sound)
			{
				if (m_effect != null && !m_effect.Finished)
				{
					m_effect.AutoUpdate = true;
				}
				if (sound.IsNull)
				{
					m_effect = MyAudio.Static.ApplyEffect(m_sound, m_fadeOut, null, 5000f);
				}
				else
				{
					m_effect = MyAudio.Static.ApplyEffect(m_sound, m_crossFade, new MyCueId[1]
					{
						sound
					}, 5000f);
				}
				if (m_effect != null && !m_effect.Finished)
				{
					m_effect.AutoUpdate = true;
					m_sound = m_effect.OutputSound;
				}
			}
			if (m_sound != null)
			{
				m_volumeOriginal = (MyAudio.Static.GetCue(sound)?.Volume ?? 1f);
				m_sound.SetVolume(m_volumeOriginal * m_volumeModifier * VolumeModifierGlobal);
			}
		}

		public static void SetAmbientOn()
		{
			m_volumeModifierTarget = 1f;
		}

		public static void SetAmbientOff()
		{
			m_volumeModifierTarget = 0f;
		}
	}
}
