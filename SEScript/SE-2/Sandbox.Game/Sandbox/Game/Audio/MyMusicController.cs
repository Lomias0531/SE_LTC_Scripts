using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Audio
{
	internal class MyMusicController
	{
		private struct MusicOption
		{
			public MyStringId Category;

			public float Frequency;

			public MusicOption(string category, float frequency)
			{
				Category = MyStringId.GetOrCompute(category);
				Frequency = frequency;
			}
		}

		private enum MusicCategory
		{
			location,
			building,
			danger,
			lightFight,
			heavyFight,
			custom
		}

		private const int METEOR_SHOWER_MUSIC_FREQUENCY = 43200;

		private const int METEOR_SHOWER_CROSSFADE_LENGTH = 2000;

		private const int DEFAULT_NO_MUSIC_TIME_MIN = 2;

		private const int DEFAULT_NO_MUSIC_TIME_MAX = 8;

		private const int FAST_NO_MUSIC_TIME_MIN = 1;

		private const int FAST_NO_MUSIC_TIME_MAX = 4;

		private const int BUILDING_NEED = 7000;

		private const int BUILDING_COOLDOWN = 45000;

		private const int BUILDING_CROSSFADE_LENGTH = 2000;

		private const int FIGHTING_NEED = 100;

		private const int FIGHTING_COOLDOWN_LIGHT = 15;

		private const int FIGHTING_COOLDOWN_HEAVY = 20;

		private const int FIGHTING_CROSSFADE_LENGTH = 2000;

		private static List<MusicOption> m_defaultSpaceCategories = new List<MusicOption>
		{
			new MusicOption("Space", 0.7f),
			new MusicOption("Calm", 0.25f),
			new MusicOption("Mystery", 0.05f)
		};

		private static List<MusicOption> m_defaultPlanetCategory = new List<MusicOption>
		{
			new MusicOption("Planet", 0.8f),
			new MusicOption("Calm", 0.1f),
			new MusicOption("Danger", 0.1f)
		};

		private static MyStringHash m_hashCrossfade = MyStringHash.GetOrCompute("CrossFade");

		private static MyStringHash m_hashFadeIn = MyStringHash.GetOrCompute("FadeIn");

		private static MyStringHash m_hashFadeOut = MyStringHash.GetOrCompute("FadeOut");

		private static MyStringId m_stringIdDanger = MyStringId.GetOrCompute("Danger");

		private static MyStringId m_stringIdBuilding = MyStringId.GetOrCompute("Building");

		private static MyStringId m_stringIdLightFight = MyStringId.GetOrCompute("LightFight");

		private static MyStringId m_stringIdHeavyFight = MyStringId.GetOrCompute("HeavyFight");

		private static MyCueId m_cueEmpty = default(MyCueId);

		public bool Active;

		public bool CanChangeCategoryGlobal = true;

		private bool CanChangeCategoryLocal = true;

		private Dictionary<MyStringId, List<MyCueId>> m_musicCuesAll;

		private Dictionary<MyStringId, List<MyCueId>> m_musicCuesRemaining;

		private List<MusicOption> m_actualMusicOptions = new List<MusicOption>();

		private MyPlanet m_lastVisitedPlanet;

		private MySoundData m_lastMusicData;

		private int m_frameCounter;

		private float m_noMusicTimer;

		private MyRandom m_random = new MyRandom();

		private IMySourceVoice m_musicSourceVoice;

		private int m_lastMeteorShower = int.MinValue;

		private MusicCategory m_currentMusicCategory;

		private int m_meteorShower;

		private int m_building;

		private int m_buildingCooldown;

		private int m_fightLight;

		private int m_fightLightCooldown;

		private int m_fightHeavy;

		private int m_fightHeavyCooldown;

		public static MyMusicController Static
		{
			get;
			set;
		}

		public MyStringId CategoryPlaying
		{
			get;
			private set;
		}

		public MyStringId CategoryLast
		{
			get;
			private set;
		}

		public MyCueId CueIdPlaying
		{
			get;
			private set;
		}

		public float NextMusicTrackIn => m_noMusicTimer;

		public bool CanChangeCategory
		{
			get
			{
				if (CanChangeCategoryGlobal)
				{
					return CanChangeCategoryLocal;
				}
				return false;
			}
		}

		public bool MusicIsPlaying
		{
			get
			{
				if (m_musicSourceVoice != null)
				{
					return m_musicSourceVoice.IsPlaying;
				}
				return false;
			}
		}

		public MyMusicController(Dictionary<MyStringId, List<MyCueId>> musicCues = null)
		{
			CategoryPlaying = MyStringId.NullOrEmpty;
			CategoryLast = MyStringId.NullOrEmpty;
			Active = false;
			if (musicCues == null)
			{
				m_musicCuesAll = new Dictionary<MyStringId, List<MyCueId>>(MyStringId.Comparer);
			}
			else
			{
				m_musicCuesAll = musicCues;
			}
			m_musicCuesRemaining = new Dictionary<MyStringId, List<MyCueId>>(MyStringId.Comparer);
		}

		private void Update_1s()
		{
			if (m_meteorShower > 0)
			{
				m_meteorShower--;
			}
			if (m_buildingCooldown > 0)
			{
				m_buildingCooldown = Math.Max(0, m_buildingCooldown - 1000);
			}
			else if (m_building > 0)
			{
				m_building = Math.Max(0, m_building - 1000);
			}
			if (m_fightHeavyCooldown > 0)
			{
				m_fightHeavyCooldown = Math.Max(0, m_fightHeavyCooldown - 1);
			}
			else if (m_fightHeavy > 0)
			{
				m_fightHeavy = Math.Max(0, m_fightHeavy - 10);
			}
			if (m_fightLightCooldown > 0)
			{
				m_fightLightCooldown = Math.Max(0, m_fightLightCooldown - 1);
			}
			else if (m_fightLight > 0)
			{
				m_fightLight = Math.Max(0, m_fightLight - 10);
			}
		}

		public void Update()
		{
			if (m_frameCounter % 60 == 0)
			{
				Update_1s();
			}
			if (MusicIsPlaying)
			{
				if (MyAudio.Static.Mute)
				{
					MyAudio.Static.Mute = false;
				}
				m_musicSourceVoice.SetVolume((m_lastMusicData != null) ? (MyAudio.Static.VolumeMusic * m_lastMusicData.Volume) : MyAudio.Static.VolumeMusic);
			}
			else if (m_noMusicTimer > 0f)
			{
				m_noMusicTimer -= 0.0166666675f;
			}
			else
			{
				if (CanChangeCategory)
				{
					if (m_fightHeavy >= 100)
					{
						m_currentMusicCategory = MusicCategory.heavyFight;
					}
					else if (m_fightLight >= 100)
					{
						m_currentMusicCategory = MusicCategory.lightFight;
					}
					else if (m_meteorShower > 0)
					{
						m_currentMusicCategory = MusicCategory.danger;
					}
					else if (m_building >= 7000)
					{
						m_currentMusicCategory = MusicCategory.building;
					}
					else
					{
						m_currentMusicCategory = MusicCategory.location;
					}
				}
				switch (m_currentMusicCategory)
				{
				case MusicCategory.building:
					PlayBuildingMusic();
					break;
				case MusicCategory.danger:
					PlayDangerMusic();
					break;
				case MusicCategory.lightFight:
					PlayFightingMusic(light: true);
					break;
				case MusicCategory.heavyFight:
					PlayFightingMusic(light: false);
					break;
				case MusicCategory.custom:
					PlaySpecificMusicCategory(CategoryLast, playAtLeastOnce: false);
					break;
				default:
					CalculateNextCue();
					break;
				}
			}
			m_frameCounter++;
		}

		public void Building(int amount)
		{
			m_building = Math.Min(7000, m_building + amount);
			m_buildingCooldown = Math.Min(45000, m_buildingCooldown + amount * 5);
			if (CanChangeCategory && m_building >= 7000)
			{
				m_noMusicTimer = m_random.Next(1, 4);
				if (m_currentMusicCategory < MusicCategory.building)
				{
					PlayBuildingMusic();
				}
			}
		}

		public void MeteorShowerIncoming()
		{
			int sessionTotalFrames = MyFpsManager.GetSessionTotalFrames();
			if (CanChangeCategory && Math.Abs(m_lastMeteorShower - sessionTotalFrames) >= 43200)
			{
				m_meteorShower = 10;
				m_lastMeteorShower = sessionTotalFrames;
				m_noMusicTimer = m_random.Next(1, 4);
				if (m_currentMusicCategory < MusicCategory.danger)
				{
					PlayDangerMusic();
				}
			}
		}

		public void Fighting(bool heavy, int amount)
		{
			m_fightLight = Math.Min(m_fightLight + amount, 100);
			m_fightLightCooldown = 15;
			if (heavy)
			{
				m_fightHeavy = Math.Min(m_fightHeavy + amount, 100);
				m_fightHeavyCooldown = 20;
			}
			if (CanChangeCategory)
			{
				if (m_fightHeavy >= 100 && m_currentMusicCategory < MusicCategory.heavyFight)
				{
					PlayFightingMusic(light: false);
				}
				else if (m_fightLight >= 100 && m_currentMusicCategory < MusicCategory.lightFight)
				{
					PlayFightingMusic(light: true);
				}
			}
		}

		public void IncreaseCategory(MyStringId category, int amount)
		{
			if (category == m_stringIdLightFight)
			{
				Fighting(heavy: false, amount);
			}
			else if (category == m_stringIdHeavyFight)
			{
				Fighting(heavy: true, amount);
			}
			else if (category == m_stringIdBuilding)
			{
				Building(amount);
			}
			else if (category == m_stringIdDanger)
			{
				MeteorShowerIncoming();
			}
		}

		private void PlayDangerMusic()
		{
			CategoryPlaying = m_stringIdDanger;
			m_currentMusicCategory = MusicCategory.danger;
			if (m_musicSourceVoice != null && m_musicSourceVoice.IsPlaying)
			{
				PlayMusic(CueIdPlaying, m_hashCrossfade, 2000, new MyCueId[1]
				{
					SelectCueFromCategory(m_stringIdDanger)
				}, play: false);
			}
			else
			{
				PlayMusic(SelectCueFromCategory(CategoryPlaying), m_hashFadeIn, 1000, new MyCueId[0]);
			}
			m_noMusicTimer = m_random.Next(2, 8);
		}

		private void PlayBuildingMusic()
		{
			CategoryPlaying = m_stringIdBuilding;
			m_currentMusicCategory = MusicCategory.building;
			if (m_musicSourceVoice != null && m_musicSourceVoice.IsPlaying)
			{
				PlayMusic(CueIdPlaying, m_hashCrossfade, 2000, new MyCueId[1]
				{
					SelectCueFromCategory(m_stringIdBuilding)
				}, play: false);
			}
			else
			{
				PlayMusic(SelectCueFromCategory(CategoryPlaying), m_hashFadeIn, 1000, new MyCueId[0]);
			}
			m_noMusicTimer = m_random.Next(2, 8);
		}

		private void PlayFightingMusic(bool light)
		{
			CategoryPlaying = (light ? m_stringIdLightFight : m_stringIdHeavyFight);
			m_currentMusicCategory = (light ? MusicCategory.lightFight : MusicCategory.heavyFight);
			if (m_musicSourceVoice != null && m_musicSourceVoice.IsPlaying)
			{
				PlayMusic(CueIdPlaying, m_hashCrossfade, 2000, new MyCueId[1]
				{
					SelectCueFromCategory(CategoryPlaying)
				}, play: false);
			}
			else
			{
				PlayMusic(SelectCueFromCategory(CategoryPlaying), m_hashFadeIn, 1000, new MyCueId[0]);
			}
			m_noMusicTimer = m_random.Next(1, 4);
		}

		private void CalculateNextCue()
		{
			if (MySession.Static == null || MySession.Static.LocalCharacter == null)
			{
				return;
			}
			m_noMusicTimer = m_random.Next(2, 8);
			Vector3D position = MySession.Static.LocalCharacter.PositionComp.GetPosition();
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
			MySphericalNaturalGravityComponent mySphericalNaturalGravityComponent = (closestPlanet != null) ? (closestPlanet.Components.Get<MyGravityProviderComponent>() as MySphericalNaturalGravityComponent) : null;
			if (closestPlanet != null && mySphericalNaturalGravityComponent != null && Vector3D.Distance(position, closestPlanet.PositionComp.GetPosition()) <= (double)(mySphericalNaturalGravityComponent.GravityLimit * 0.65f))
			{
				if (closestPlanet != m_lastVisitedPlanet)
				{
					m_lastVisitedPlanet = closestPlanet;
					if (closestPlanet.Generator.MusicCategories != null && closestPlanet.Generator.MusicCategories.Count > 0)
					{
						m_actualMusicOptions.Clear();
						foreach (MyMusicCategory musicCategory in closestPlanet.Generator.MusicCategories)
						{
							m_actualMusicOptions.Add(new MusicOption(musicCategory.Category, musicCategory.Frequency));
						}
					}
					else
					{
						m_actualMusicOptions = m_defaultPlanetCategory;
					}
				}
			}
			else
			{
				m_lastVisitedPlanet = null;
				m_actualMusicOptions = m_defaultSpaceCategories;
			}
			float num = 0f;
			foreach (MusicOption actualMusicOption in m_actualMusicOptions)
			{
				num += Math.Max(actualMusicOption.Frequency, 0f);
			}
			float num2 = (float)m_random.NextDouble() * num;
			MyStringId category = m_actualMusicOptions[0].Category;
			for (int i = 0; i < m_actualMusicOptions.Count; i++)
			{
				if (num2 <= m_actualMusicOptions[i].Frequency)
				{
					category = m_actualMusicOptions[i].Category;
					break;
				}
				num2 -= m_actualMusicOptions[i].Frequency;
			}
			CueIdPlaying = SelectCueFromCategory(category);
			CategoryPlaying = category;
			if (!(CueIdPlaying == m_cueEmpty))
			{
				PlayMusic(CueIdPlaying, MyStringHash.NullOrEmpty);
				m_currentMusicCategory = MusicCategory.location;
			}
		}

		public void PlaySpecificMusicTrack(MyCueId cue, bool playAtLeastOnce)
		{
			if (!cue.IsNull)
			{
				if (m_musicSourceVoice != null && m_musicSourceVoice.IsPlaying)
				{
					PlayMusic(CueIdPlaying, m_hashCrossfade, 2000, new MyCueId[1]
					{
						cue
					}, play: false);
				}
				else
				{
					PlayMusic(cue, m_hashFadeIn, 1000, new MyCueId[0]);
				}
				m_noMusicTimer = m_random.Next(2, 8);
				CanChangeCategoryLocal = !playAtLeastOnce;
				m_currentMusicCategory = MusicCategory.location;
			}
		}

		public void PlaySpecificMusicCategory(MyStringId category, bool playAtLeastOnce)
		{
			if (category.Id != 0)
			{
				CategoryPlaying = category;
				if (m_musicSourceVoice != null && m_musicSourceVoice.IsPlaying)
				{
					PlayMusic(CueIdPlaying, m_hashCrossfade, 2000, new MyCueId[1]
					{
						SelectCueFromCategory(CategoryPlaying)
					}, play: false);
				}
				else
				{
					PlayMusic(SelectCueFromCategory(CategoryPlaying), m_hashFadeIn, 1000, new MyCueId[0]);
				}
				m_noMusicTimer = m_random.Next(2, 8);
				CanChangeCategoryLocal = !playAtLeastOnce;
				m_currentMusicCategory = MusicCategory.custom;
			}
		}

		public void SetSpecificMusicCategory(MyStringId category)
		{
			if (category.Id != 0)
			{
				CategoryPlaying = category;
				m_currentMusicCategory = MusicCategory.custom;
			}
		}

		private void PlayMusic(MyCueId cue, MyStringHash effect, int effectDuration = 2000, MyCueId[] cueIds = null, bool play = true)
		{
			if (MyAudio.Static == null)
			{
				return;
			}
			if (play)
			{
				m_musicSourceVoice = MyAudio.Static.PlayMusicCue(cue, overrideMusicAllowed: true);
			}
			if (m_musicSourceVoice != null)
			{
				if (effect != MyStringHash.NullOrEmpty)
				{
					m_musicSourceVoice = MyAudio.Static.ApplyEffect(m_musicSourceVoice, effect, cueIds, effectDuration, musicEffect: true).OutputSound;
				}
				if (m_musicSourceVoice != null)
				{
					IMySourceVoice musicSourceVoice = m_musicSourceVoice;
					musicSourceVoice.StoppedPlaying = (Action)Delegate.Combine(musicSourceVoice.StoppedPlaying, new Action(MusicStopped));
				}
			}
			m_lastMusicData = MyAudio.Static.GetCue(cue);
		}

		private MyCueId SelectCueFromCategory(MyStringId category)
		{
			if (!m_musicCuesRemaining.ContainsKey(category))
			{
				m_musicCuesRemaining.Add(category, new List<MyCueId>());
			}
			if (m_musicCuesRemaining[category].Count == 0)
			{
				if (!m_musicCuesAll.ContainsKey(category) || m_musicCuesAll[category] == null || m_musicCuesAll[category].Count == 0)
				{
					return m_cueEmpty;
				}
				foreach (MyCueId item in m_musicCuesAll[category])
				{
					m_musicCuesRemaining[category].Add(item);
				}
				m_musicCuesRemaining[category].ShuffleList();
			}
			MyCueId result = m_musicCuesRemaining[category][0];
			m_musicCuesRemaining[category].RemoveAt(0);
			return result;
		}

		public void MusicStopped()
		{
			if (m_musicSourceVoice == null || !m_musicSourceVoice.IsPlaying)
			{
				CategoryLast = CategoryPlaying;
				CategoryPlaying = MyStringId.NullOrEmpty;
				CanChangeCategoryLocal = true;
			}
		}

		public void ClearMusicCues()
		{
			m_musicCuesAll.Clear();
			m_musicCuesRemaining.Clear();
		}

		public void AddMusicCue(MyStringId category, MyCueId cueId)
		{
			if (!m_musicCuesAll.ContainsKey(category))
			{
				m_musicCuesAll.Add(category, new List<MyCueId>());
			}
			m_musicCuesAll[category].Add(cueId);
		}

		public void SetMusicCues(Dictionary<MyStringId, List<MyCueId>> musicCues)
		{
			ClearMusicCues();
			m_musicCuesAll = musicCues;
		}

		public void Unload()
		{
			if (m_musicSourceVoice != null)
			{
				m_musicSourceVoice.Stop();
				m_musicSourceVoice = null;
			}
			Active = false;
			ClearMusicCues();
		}
	}
}
