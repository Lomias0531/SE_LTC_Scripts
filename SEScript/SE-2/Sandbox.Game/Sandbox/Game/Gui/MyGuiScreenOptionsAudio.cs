using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Audio;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenOptionsAudio : MyGuiScreenBase
	{
		private class MyGuiScreenOptionsAudioSettings
		{
			public float GameVolume;

			public float MusicVolume;

			public float VoiceChatVolume;

			public bool HudWarnings;

			public bool EnableVoiceChat;

			public bool EnableMuteWhenNotInFocus;

			public bool EnableDynamicMusic;

			public bool EnableReverb;

			public bool ShipSoundsAreBasedOnSpeed;

			public bool EnableDoppler;
		}

		private MyGuiControlSlider m_gameVolumeSlider;

		private MyGuiControlSlider m_musicVolumeSlider;

		private MyGuiControlSlider m_voiceChatVolumeSlider;

		private MyGuiControlCheckbox m_hudWarnings;

		private MyGuiControlCheckbox m_enableVoiceChat;

		private MyGuiControlCheckbox m_enableMuteWhenNotInFocus;

		private MyGuiControlCheckbox m_enableDynamicMusic;

		private MyGuiControlCheckbox m_enableReverb;

		private MyGuiControlCheckbox m_enableDoppler;

		private MyGuiControlCheckbox m_shipSoundsAreBasedOnSpeed;

		private MyGuiScreenOptionsAudioSettings m_settingsOld = new MyGuiScreenOptionsAudioSettings();

		private MyGuiScreenOptionsAudioSettings m_settingsNew = new MyGuiScreenOptionsAudioSettings();

		private bool m_gameAudioPausedWhenOpen;

		private MyGuiControlElementGroup m_elementGroup;

		public MyGuiScreenOptionsAudio()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(183f / 280f, 175f / 262f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			if (constructor)
			{
				base.RecreateControls(constructor);
				m_elementGroup = new MyGuiControlElementGroup();
				m_elementGroup.HighlightChanged += m_elementGroup_HighlightChanged;
				AddCaption(MyCommonTexts.ScreenCaptionAudioOptions, null, new Vector2(0f, 0.003f));
				MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
				myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.83f);
				Controls.Add(myGuiControlSeparatorList);
				MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
				myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.83f);
				Controls.Add(myGuiControlSeparatorList2);
				MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				MyGuiDrawAlignEnum originAlign2 = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				Vector2 value = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
				Vector2 value2 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
				float num = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
				float num2 = 25f;
				float y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 0.5f;
				float num3 = 0.0015f;
				Vector2 value3 = new Vector2(0f, 0.045f);
				float num4 = 0f;
				Vector2 value4 = new Vector2(0f, 0.008f);
				Vector2 value5 = (m_size.Value / 2f - value) * new Vector2(-1f, -1f) + new Vector2(0f, y);
				Vector2 value6 = (m_size.Value / 2f - value) * new Vector2(1f, -1f) + new Vector2(0f, y);
				Vector2 value7 = (m_size.Value / 2f - value2) * new Vector2(0f, 1f);
				Vector2 value8 = new Vector2(value6.X - (num + num3), value6.Y);
				num4 -= 0.045f;
				MyGuiControlLabel control = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.MusicVolume))
				{
					Position = value5 + num4 * value3 + value4,
					OriginAlign = originAlign
				};
				m_musicVolumeSlider = new MyGuiControlSlider(null, 0f, 1f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_MusicVolume), defaultValue: MySandboxGame.Config.MusicVolume)
				{
					Position = value6 + num4 * value3,
					OriginAlign = originAlign2,
					Size = new Vector2(num, 0f)
				};
				m_musicVolumeSlider.ValueChanged = OnMusicVolumeChange;
				num4 += 1.08f;
				MyGuiControlLabel control2 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.GameVolume))
				{
					Position = value5 + num4 * value3 + value4,
					OriginAlign = originAlign
				};
				m_gameVolumeSlider = new MyGuiControlSlider(null, 0f, 1f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_SoundVolume), defaultValue: MySandboxGame.Config.GameVolume)
				{
					Position = value6 + num4 * value3,
					OriginAlign = originAlign2,
					Size = new Vector2(num, 0f)
				};
				m_gameVolumeSlider.ValueChanged = OnGameVolumeChange;
				num4 += 1.08f;
				MyGuiControlLabel control3 = null;
				MyGuiControlLabel control4 = null;
				if (MyPerGameSettings.VoiceChatEnabled)
				{
					control4 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.VoiceChatVolume))
					{
						Position = value5 + num4 * value3 + value4,
						OriginAlign = originAlign
					};
					m_voiceChatVolumeSlider = new MyGuiControlSlider(null, 0f, 1f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_VoiceChatVolume), defaultValue: MySandboxGame.Config.VoiceChatVolume)
					{
						Position = value6 + num4 * value3,
						OriginAlign = originAlign2,
						Size = new Vector2(num, 0f)
					};
					m_voiceChatVolumeSlider.ValueChanged = OnVoiceChatVolumeChange;
					num4 += 1.37f;
					control3 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.EnableVoiceChat))
					{
						Position = value5 + num4 * value3 + value4,
						OriginAlign = originAlign
					};
					m_enableVoiceChat = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_EnableVoiceChat))
					{
						Position = value8 + num4 * value3,
						OriginAlign = originAlign
					};
					m_enableVoiceChat.IsCheckedChanged = VoiceChatChecked;
					num4 += 1f;
				}
				MyGuiControlLabel control5 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.MuteWhenNotInFocus))
				{
					Position = value5 + num4 * value3 + value4,
					OriginAlign = originAlign
				};
				m_enableMuteWhenNotInFocus = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_MuteWhenInactive))
				{
					Position = value8 + num4 * value3,
					OriginAlign = originAlign
				};
				m_enableMuteWhenNotInFocus.IsCheckedChanged = EnableMuteWhenNotInFocusChecked;
				num4 += 1f;
				MyGuiControlLabel control6 = null;
				if (MyPerGameSettings.UseReverbEffect && MyFakes.AUDIO_ENABLE_REVERB)
				{
					control6 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.AudioSettings_EnableReverb))
					{
						Position = value5 + num4 * value3 + value4,
						OriginAlign = originAlign
					};
					m_enableReverb = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipAudioOptionsEnableReverb))
					{
						Position = value8 + num4 * value3,
						OriginAlign = originAlign
					};
					m_enableReverb.IsCheckedChanged = EnableReverbChecked;
					m_enableReverb.Enabled = (MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE);
					m_enableReverb.IsChecked = (MyAudio.Static.EnableReverb && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE);
					num4 += 1f;
				}
				MyGuiControlLabel control7 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.AudioSettings_EnableDoppler))
				{
					Position = value5 + num4 * value3 + value4,
					OriginAlign = originAlign
				};
				m_enableDoppler = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipAudioOptionsEnableDoppler))
				{
					Position = value8 + num4 * value3,
					OriginAlign = originAlign
				};
				m_enableDoppler.IsCheckedChanged = EnableDopplerChecked;
				m_enableDoppler.Enabled = true;
				m_enableDoppler.IsChecked = MyAudio.Static.EnableDoppler;
				num4 += 1f;
				MyGuiControlLabel control8 = null;
				if (MyPerGameSettings.EnableShipSoundSystem)
				{
					control8 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.AudioSettings_ShipSoundsBasedOnSpeed))
					{
						Position = value5 + num4 * value3 + value4,
						OriginAlign = originAlign
					};
					m_shipSoundsAreBasedOnSpeed = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_SpeedBasedSounds))
					{
						Position = value8 + num4 * value3,
						OriginAlign = originAlign
					};
					m_shipSoundsAreBasedOnSpeed.IsCheckedChanged = ShipSoundsAreBasedOnSpeedChecked;
					num4 += 1f;
				}
				MyGuiControlLabel control9 = null;
				if (MyPerGameSettings.UseMusicController)
				{
					control9 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.AudioSettings_UseMusicController))
					{
						Position = value5 + num4 * value3 + value4,
						OriginAlign = originAlign
					};
					m_enableDynamicMusic = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_UseContextualMusic))
					{
						Position = value8 + num4 * value3,
						OriginAlign = originAlign
					};
					num4 += 1f;
				}
				MyGuiControlLabel control10 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.HudWarnings))
				{
					Position = value5 + num4 * value3 + value4,
					OriginAlign = originAlign
				};
				m_hudWarnings = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_HudWarnings))
				{
					Position = value8 + num4 * value3,
					OriginAlign = originAlign
				};
				m_hudWarnings.IsCheckedChanged = HudWarningsChecked;
				num4 += 1f;
				MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkClick);
				myGuiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
				MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancelClick);
				myGuiControlButton2.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
				myGuiControlButton.Position = value7 + new Vector2(0f - num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
				myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
				myGuiControlButton2.Position = value7 + new Vector2(num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
				myGuiControlButton2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
				Controls.Add(control2);
				Controls.Add(m_gameVolumeSlider);
				Controls.Add(control);
				Controls.Add(m_musicVolumeSlider);
				Controls.Add(control10);
				Controls.Add(m_hudWarnings);
				Controls.Add(control5);
				Controls.Add(m_enableMuteWhenNotInFocus);
				if (MyPerGameSettings.UseMusicController)
				{
					Controls.Add(control9);
					Controls.Add(m_enableDynamicMusic);
				}
				if (MyPerGameSettings.EnableShipSoundSystem)
				{
					Controls.Add(control8);
					Controls.Add(m_shipSoundsAreBasedOnSpeed);
				}
				if (MyPerGameSettings.UseReverbEffect && MyFakes.AUDIO_ENABLE_REVERB)
				{
					Controls.Add(control6);
					Controls.Add(m_enableReverb);
				}
				Controls.Add(control7);
				Controls.Add(m_enableDoppler);
				if (MyPerGameSettings.VoiceChatEnabled)
				{
					Controls.Add(control3);
					Controls.Add(m_enableVoiceChat);
					Controls.Add(control4);
					Controls.Add(m_voiceChatVolumeSlider);
				}
				Controls.Add(myGuiControlButton);
				m_elementGroup.Add(myGuiControlButton);
				Controls.Add(myGuiControlButton2);
				m_elementGroup.Add(myGuiControlButton2);
				UpdateFromConfig(m_settingsOld);
				UpdateFromConfig(m_settingsNew);
				UpdateControls(m_settingsOld);
				base.FocusedControl = myGuiControlButton;
				base.CloseButtonEnabled = true;
				m_gameAudioPausedWhenOpen = MyAudio.Static.GameSoundIsPaused;
				if (m_gameAudioPausedWhenOpen)
				{
					MyAudio.Static.ResumeGameSounds();
				}
			}
		}

		private void m_elementGroup_HighlightChanged(MyGuiControlElementGroup obj)
		{
			foreach (MyGuiControlBase item in m_elementGroup)
			{
				if (item.HasFocus && obj.SelectedElement != item)
				{
					base.FocusedControl = obj.SelectedElement;
					break;
				}
			}
		}

		private void VoiceChatChecked(MyGuiControlCheckbox checkbox)
		{
			m_settingsNew.EnableVoiceChat = checkbox.IsChecked;
		}

		private void HudWarningsChecked(MyGuiControlCheckbox obj)
		{
			m_settingsNew.HudWarnings = obj.IsChecked;
		}

		private void EnableMuteWhenNotInFocusChecked(MyGuiControlCheckbox obj)
		{
			m_settingsNew.EnableMuteWhenNotInFocus = obj.IsChecked;
		}

		private void EnableDynamicMusicChecked(MyGuiControlCheckbox obj)
		{
			m_settingsNew.EnableDynamicMusic = obj.IsChecked;
		}

		private void ShipSoundsAreBasedOnSpeedChecked(MyGuiControlCheckbox obj)
		{
			m_settingsNew.ShipSoundsAreBasedOnSpeed = obj.IsChecked;
		}

		private void EnableReverbChecked(MyGuiControlCheckbox obj)
		{
			m_settingsNew.EnableReverb = (MyFakes.AUDIO_ENABLE_REVERB && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE && obj.IsChecked);
		}

		private void EnableDopplerChecked(MyGuiControlCheckbox obj)
		{
			m_settingsNew.EnableDoppler = obj.IsChecked;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenOptionsAudio";
		}

		private void UpdateFromConfig(MyGuiScreenOptionsAudioSettings settings)
		{
			settings.GameVolume = MySandboxGame.Config.GameVolume;
			settings.MusicVolume = MySandboxGame.Config.MusicVolume;
			settings.VoiceChatVolume = MySandboxGame.Config.VoiceChatVolume;
			settings.HudWarnings = MySandboxGame.Config.HudWarnings;
			settings.EnableVoiceChat = MySandboxGame.Config.EnableVoiceChat;
			settings.EnableMuteWhenNotInFocus = MySandboxGame.Config.EnableMuteWhenNotInFocus;
			settings.EnableReverb = (MyFakes.AUDIO_ENABLE_REVERB && MySandboxGame.Config.EnableReverb && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE);
			settings.EnableDynamicMusic = MySandboxGame.Config.EnableDynamicMusic;
			settings.ShipSoundsAreBasedOnSpeed = MySandboxGame.Config.ShipSoundsAreBasedOnSpeed;
			settings.EnableDoppler = MySandboxGame.Config.EnableDoppler;
		}

		private void UpdateControls(MyGuiScreenOptionsAudioSettings settings)
		{
			m_gameVolumeSlider.Value = settings.GameVolume;
			m_musicVolumeSlider.Value = settings.MusicVolume;
			m_voiceChatVolumeSlider.Value = settings.VoiceChatVolume;
			m_hudWarnings.IsChecked = settings.HudWarnings;
			m_enableVoiceChat.IsChecked = settings.EnableVoiceChat;
			m_enableMuteWhenNotInFocus.IsChecked = settings.EnableMuteWhenNotInFocus;
			if (MyFakes.AUDIO_ENABLE_REVERB)
			{
				m_enableReverb.IsChecked = settings.EnableReverb;
			}
			m_enableDynamicMusic.IsChecked = settings.EnableDynamicMusic;
			m_shipSoundsAreBasedOnSpeed.IsChecked = settings.ShipSoundsAreBasedOnSpeed;
			m_enableDoppler.IsChecked = settings.EnableDoppler;
		}

		private void Save()
		{
			MySandboxGame.Config.GameVolume = MyAudio.Static.VolumeGame;
			MySandboxGame.Config.MusicVolume = MyAudio.Static.VolumeMusic;
			MySandboxGame.Config.VoiceChatVolume = m_voiceChatVolumeSlider.Value;
			MySandboxGame.Config.HudWarnings = m_hudWarnings.IsChecked;
			MySandboxGame.Config.EnableVoiceChat = m_enableVoiceChat.IsChecked;
			MySandboxGame.Config.EnableMuteWhenNotInFocus = m_enableMuteWhenNotInFocus.IsChecked;
			MySandboxGame.Config.EnableReverb = (MyFakes.AUDIO_ENABLE_REVERB && m_enableReverb.IsChecked && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE);
			MyAudio.Static.EnableReverb = MySandboxGame.Config.EnableReverb;
			MySandboxGame.Config.EnableDynamicMusic = m_enableDynamicMusic.IsChecked;
			MySandboxGame.Config.ShipSoundsAreBasedOnSpeed = m_shipSoundsAreBasedOnSpeed.IsChecked;
			MySandboxGame.Config.EnableDoppler = m_enableDoppler.IsChecked;
			MyAudio.Static.EnableDoppler = MySandboxGame.Config.EnableDoppler;
			MySandboxGame.Config.Save();
			if (MySession.Static != null && MyGuiScreenGamePlay.Static != null)
			{
				if (MySandboxGame.Config.EnableDynamicMusic && MyMusicController.Static == null)
				{
					MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
					MyMusicController.Static.Active = true;
					MyAudio.Static.MusicAllowed = false;
					MyAudio.Static.StopMusic();
				}
				else if (!MySandboxGame.Config.EnableDynamicMusic && MyMusicController.Static != null)
				{
					MyMusicController.Static.Unload();
					MyMusicController.Static = null;
					MyAudio.Static.MusicAllowed = true;
					MyAudio.Static.PlayMusic(new MyMusicTrack
					{
						TransitionCategory = MyStringId.GetOrCompute("Default")
					});
				}
				if (MyFakes.AUDIO_ENABLE_REVERB && MyAudio.Static != null && MyAudio.Static.EnableReverb != m_enableReverb.IsChecked && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE)
				{
					MyAudio.Static.EnableReverb = m_enableReverb.IsChecked;
				}
			}
		}

		private static void UpdateValues(MyGuiScreenOptionsAudioSettings settings)
		{
			MyAudio.Static.VolumeGame = settings.GameVolume;
			MyAudio.Static.VolumeMusic = settings.MusicVolume;
			MyAudio.Static.VolumeVoiceChat = settings.VoiceChatVolume;
			MyAudio.Static.VolumeHud = MyAudio.Static.VolumeGame;
			MyAudio.Static.EnableVoiceChat = settings.EnableVoiceChat;
			MyGuiAudio.HudWarnings = settings.HudWarnings;
		}

		public void OnOkClick(MyGuiControlButton sender)
		{
			Save();
			CloseScreen();
		}

		public void OnCancelClick(MyGuiControlButton sender)
		{
			UpdateValues(m_settingsOld);
			CloseScreen();
		}

		private void OnGameVolumeChange(MyGuiControlSlider sender)
		{
			m_settingsNew.GameVolume = m_gameVolumeSlider.Value;
			UpdateValues(m_settingsNew);
		}

		private void OnMusicVolumeChange(MyGuiControlSlider sender)
		{
			m_settingsNew.MusicVolume = m_musicVolumeSlider.Value;
			UpdateValues(m_settingsNew);
		}

		private void OnVoiceChatVolumeChange(MyGuiControlSlider sender)
		{
			m_settingsNew.VoiceChatVolume = m_voiceChatVolumeSlider.Value;
			UpdateValues(m_settingsNew);
		}

		public override bool CloseScreen()
		{
			UpdateFromConfig(m_settingsOld);
			UpdateValues(m_settingsOld);
			if (m_gameAudioPausedWhenOpen)
			{
				MyAudio.Static.PauseGameSounds();
			}
			return base.CloseScreen();
		}
	}
}
