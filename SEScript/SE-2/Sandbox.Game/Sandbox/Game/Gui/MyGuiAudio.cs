using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Data.Audio;

namespace Sandbox.Game.GUI
{
	public class MyGuiAudio : IMyGuiAudio
	{
		public static bool HudWarnings;

		private static Dictionary<MyGuiSounds, MySoundPair> m_sounds;

		private static Dictionary<MyGuiSounds, int> m_lastTimePlaying;

		public static IMyGuiAudio Static
		{
			get;
			set;
		}

		static MyGuiAudio()
		{
			m_sounds = new Dictionary<MyGuiSounds, MySoundPair>(Enum.GetValues(typeof(MyGuiSounds)).Length);
			m_lastTimePlaying = new Dictionary<MyGuiSounds, int>();
			Static = new MyGuiAudio();
			foreach (MyGuiSounds value in Enum.GetValues(typeof(MyGuiSounds)))
			{
				m_sounds.Add(value, new MySoundPair(value.ToString(), useLog: false));
			}
		}

		public void PlaySound(GuiSounds sound)
		{
			if (sound != GuiSounds.None)
			{
				PlaySound(GetSound(sound));
			}
		}

		public static IMySourceVoice PlaySound(MyGuiSounds sound)
		{
			if (MyFakes.ENABLE_NEW_SOUNDS && MySession.Static != null && MySession.Static.Settings.RealisticSound && MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.OxygenComponent != null && !MySession.Static.LocalCharacter.OxygenComponent.HelmetEnabled)
			{
				MySoundData cue = MyAudio.Static.GetCue(m_sounds[sound].SoundId);
				if (cue != null && cue.CanBeSilencedByVoid)
				{
					MyCockpit myCockpit = MySession.Static.LocalCharacter.Parent as MyCockpit;
					if ((myCockpit == null || !myCockpit.BlockDefinition.IsPressurized) && MySession.Static.LocalCharacter.EnvironmentOxygenLevel <= 0f)
					{
						return null;
					}
				}
			}
			if (CheckForSynchronizedSounds(sound))
			{
				return MyAudio.Static.PlaySound(m_sounds[sound].SoundId);
			}
			return null;
		}

		private MyGuiSounds GetSound(GuiSounds sound)
		{
			switch (sound)
			{
			case GuiSounds.MouseClick:
				return MyGuiSounds.HudMouseClick;
			case GuiSounds.MouseOver:
				return MyGuiSounds.HudMouseOver;
			case GuiSounds.Item:
				return MyGuiSounds.HudItem;
			default:
				return MyGuiSounds.HudClick;
			}
		}

		internal static MyCueId GetCue(MyGuiSounds sound)
		{
			return m_sounds[sound].SoundId;
		}

		private static bool CheckForSynchronizedSounds(MyGuiSounds sound)
		{
			MySoundData cue = MyAudio.Static.GetCue(m_sounds[sound].SoundId);
			if (cue != null && cue.PreventSynchronization >= 0)
			{
				int sessionTotalFrames = MyFpsManager.GetSessionTotalFrames();
				if (m_lastTimePlaying.TryGetValue(sound, out int value))
				{
					if (Math.Abs(sessionTotalFrames - value) <= cue.PreventSynchronization)
					{
						return false;
					}
					m_lastTimePlaying[sound] = sessionTotalFrames;
				}
				else
				{
					m_lastTimePlaying.Add(sound, sessionTotalFrames);
				}
			}
			return true;
		}
	}
}
