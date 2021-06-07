using Sandbox.Definitions;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage.Audio;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.FileSystem;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Audio")]
	internal class MyGuiScreenDebugAudio : MyGuiScreenDebugBase
	{
		private const string ALL_CATEGORIES = "_ALL_CATEGORIES_";

		private MyGuiControlCombobox m_categoriesCombo;

		private MyGuiControlCombobox m_cuesCombo;

		private static string m_currentCategorySelectedItem;

		private static int m_currentCueSelectedItem;

		private bool m_canUpdateValues = true;

		private IMySourceVoice m_sound;

		private MySoundData m_currentCue;

		private MyGuiControlSlider m_cueVolumeSlider;

		private MyGuiControlCombobox m_cueVolumeCurveCombo;

		private MyGuiControlSlider m_cueMaxDistanceSlider;

		private MyGuiControlSlider m_cueVolumeVariationSlider;

		private MyGuiControlSlider m_cuePitchVariationSlider;

		private MyGuiControlCheckbox m_soloCheckbox;

		private MyGuiControlButton m_applyVolumeToCategory;

		private MyGuiControlButton m_applyMaxDistanceToCategory;

		private MyGuiControlCombobox m_effects;

		private List<MyGuiControlCombobox> m_cues = new List<MyGuiControlCombobox>();

		private List<MyCueId> m_cueCache = new List<MyCueId>();

		public MyGuiScreenDebugAudio()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Audio FX", Color.Yellow.ToVector4());
			AddShareFocusHint();
			if (!(MyAudio.Static is MyNullAudio))
			{
				m_categoriesCombo = AddCombo();
				List<MyStringId> categories = MyAudio.Static.GetCategories();
				m_categoriesCombo.AddItem(0L, new StringBuilder("_ALL_CATEGORIES_"));
				int num = 1;
				foreach (MyStringId item in categories)
				{
					m_categoriesCombo.AddItem(num++, new StringBuilder(item.ToString()));
				}
				m_categoriesCombo.SortItemsByValueText();
				m_categoriesCombo.ItemSelected += categoriesCombo_OnSelect;
				m_cuesCombo = AddCombo();
				m_cuesCombo.ItemSelected += cuesCombo_OnSelect;
				m_cueVolumeSlider = AddSlider("Volume", 1f, 0f, 1f);
				m_cueVolumeSlider.ValueChanged = CueVolumeChanged;
				m_applyVolumeToCategory = AddButton(new StringBuilder("Apply to category"), OnApplyVolumeToCategorySelected);
				m_applyVolumeToCategory.Enabled = false;
				m_cueVolumeCurveCombo = AddCombo();
				foreach (object value in Enum.GetValues(typeof(MyCurveType)))
				{
					m_cueVolumeCurveCombo.AddItem((int)value, new StringBuilder(value.ToString()));
				}
				m_effects = AddCombo();
				m_effects.AddItem(0L, new StringBuilder(""));
				num = 1;
				foreach (MyAudioEffectDefinition audioEffectDefinition in MyDefinitionManager.Static.GetAudioEffectDefinitions())
				{
					m_effects.AddItem(num++, new StringBuilder(audioEffectDefinition.Id.SubtypeName));
				}
				m_effects.SelectItemByIndex(0);
				m_effects.ItemSelected += effects_ItemSelected;
				m_cueMaxDistanceSlider = AddSlider("Max distance", 0f, 0f, 2000f);
				m_cueMaxDistanceSlider.ValueChanged = MaxDistanceChanged;
				m_applyMaxDistanceToCategory = AddButton(new StringBuilder("Apply to category"), OnApplyMaxDistanceToCategorySelected);
				m_applyMaxDistanceToCategory.Enabled = false;
				m_cueVolumeVariationSlider = AddSlider("Volume variation", 0f, 0f, 10f);
				m_cueVolumeVariationSlider.ValueChanged = VolumeVariationChanged;
				m_cuePitchVariationSlider = AddSlider("Pitch variation", 0f, 0f, 500f);
				m_cuePitchVariationSlider.ValueChanged = PitchVariationChanged;
				m_soloCheckbox = AddCheckBox("Solo", checkedState: false, null);
				m_soloCheckbox.IsCheckedChanged = SoloChanged;
				AddButton(new StringBuilder("Play selected"), OnPlaySelected).CueEnum = GuiSounds.None;
				AddButton(new StringBuilder("Stop selected"), OnStopSelected);
				AddButton(new StringBuilder("Save"), OnSave);
				AddButton(new StringBuilder("Reload"), OnReload);
				if (m_categoriesCombo.GetItemsCount() > 0)
				{
					m_categoriesCombo.SelectItemByIndex(0);
				}
			}
		}

		private void effects_ItemSelected()
		{
			foreach (MyGuiControlCombobox cue in m_cues)
			{
				Controls.Remove(cue);
			}
			m_cues.Clear();
			MyStringHash subtypeId = MyStringHash.TryGet(m_effects.GetSelectedValue().ToString());
			if (MyDefinitionManager.Static.TryGetDefinition(new MyDefinitionId(typeof(MyObjectBuilder_AudioEffectDefinition), subtypeId), out MyAudioEffectDefinition definition))
			{
				for (int i = 0; i < definition.Effect.SoundsEffects.Count - 1; i++)
				{
					MyGuiControlCombobox myGuiControlCombobox = AddCombo();
					UpdateCuesCombo(myGuiControlCombobox);
					m_cues.Add(myGuiControlCombobox);
				}
			}
		}

		private void categoriesCombo_OnSelect()
		{
			m_currentCategorySelectedItem = m_categoriesCombo.GetSelectedValue().ToString();
			m_applyVolumeToCategory.Enabled = (m_currentCategorySelectedItem != "_ALL_CATEGORIES_");
			m_applyMaxDistanceToCategory.Enabled = (m_currentCategorySelectedItem != "_ALL_CATEGORIES_");
			UpdateCuesCombo(m_cuesCombo);
			foreach (MyGuiControlCombobox cue in m_cues)
			{
				UpdateCuesCombo(cue);
			}
		}

		private void UpdateCuesCombo(MyGuiControlCombobox box)
		{
			box.ClearItems();
			long num = 0L;
			foreach (MySoundData cueDefinition in MyAudio.Static.CueDefinitions)
			{
				if (m_currentCategorySelectedItem == "_ALL_CATEGORIES_" || m_currentCategorySelectedItem == cueDefinition.Category.ToString())
				{
					box.AddItem(num, new StringBuilder(cueDefinition.SubtypeId.ToString()));
					num++;
				}
			}
			box.SortItemsByValueText();
			if (box.GetItemsCount() > 0)
			{
				box.SelectItemByIndex(0);
			}
		}

		private void cuesCombo_OnSelect()
		{
			m_currentCueSelectedItem = (int)m_cuesCombo.GetSelectedKey();
			MyCueId cue = new MyCueId(MyStringHash.TryGet(m_cuesCombo.GetSelectedValue().ToString()));
			m_currentCue = MyAudio.Static.GetCue(cue);
			UpdateCueValues();
		}

		private void UpdateCueValues()
		{
			m_canUpdateValues = false;
			m_cueVolumeSlider.Value = m_currentCue.Volume;
			m_cueVolumeCurveCombo.SelectItemByKey((long)m_currentCue.VolumeCurve);
			m_cueMaxDistanceSlider.Value = m_currentCue.MaxDistance;
			m_cueVolumeVariationSlider.Value = m_currentCue.VolumeVariation;
			m_cuePitchVariationSlider.Value = m_currentCue.PitchVariation;
			m_soloCheckbox.IsChecked = (m_currentCue == MyAudio.Static.SoloCue);
			m_canUpdateValues = true;
		}

		private void CueVolumeChanged(MyGuiControlSlider slider)
		{
			if (m_canUpdateValues)
			{
				m_currentCue.Volume = slider.Value;
			}
		}

		private void CueVolumeCurveChanged(MyGuiControlCombobox combobox)
		{
			if (m_canUpdateValues)
			{
				m_currentCue.VolumeCurve = (MyCurveType)combobox.GetSelectedKey();
			}
		}

		private void MaxDistanceChanged(MyGuiControlSlider slider)
		{
			if (m_canUpdateValues)
			{
				m_currentCue.MaxDistance = slider.Value;
			}
		}

		private void VolumeVariationChanged(MyGuiControlSlider slider)
		{
			if (m_canUpdateValues)
			{
				m_currentCue.VolumeVariation = slider.Value;
			}
		}

		private void PitchVariationChanged(MyGuiControlSlider slider)
		{
			if (m_canUpdateValues)
			{
				m_currentCue.PitchVariation = slider.Value;
			}
		}

		private void SoloChanged(MyGuiControlCheckbox checkbox)
		{
			if (m_canUpdateValues)
			{
				if (checkbox.IsChecked)
				{
					MyAudio.Static.SoloCue = m_currentCue;
				}
				else
				{
					MyAudio.Static.SoloCue = null;
				}
			}
		}

		private void OnApplyVolumeToCategorySelected(MyGuiControlButton button)
		{
			m_canUpdateValues = false;
			foreach (MySoundData cueDefinition in MyAudio.Static.CueDefinitions)
			{
				if (m_currentCategorySelectedItem == cueDefinition.Category.ToString())
				{
					cueDefinition.Volume = m_cueVolumeSlider.Value;
				}
			}
			m_canUpdateValues = true;
		}

		private void OnApplyMaxDistanceToCategorySelected(MyGuiControlButton button)
		{
			m_canUpdateValues = false;
			foreach (MySoundData cueDefinition in MyAudio.Static.CueDefinitions)
			{
				if (m_currentCategorySelectedItem == cueDefinition.Category.ToString())
				{
					cueDefinition.MaxDistance = m_cueMaxDistanceSlider.Value;
				}
			}
			m_canUpdateValues = true;
		}

		private void OnPlaySelected(MyGuiControlButton button)
		{
			if (m_sound != null && m_sound.IsPlaying)
			{
				m_sound.Stop(force: true);
			}
			MyCueId cueId = new MyCueId(MyStringHash.TryGet(m_cuesCombo.GetSelectedValue().ToString()));
			m_sound = MyAudio.Static.PlaySound(cueId);
			MyStringHash myStringHash = MyStringHash.TryGet(m_effects.GetSelectedValue().ToString());
			if (myStringHash != MyStringHash.NullOrEmpty)
			{
				foreach (MyGuiControlCombobox cue in m_cues)
				{
					MyCueId item = new MyCueId(MyStringHash.TryGet(cue.GetSelectedValue().ToString()));
					m_cueCache.Add(item);
				}
				IMyAudioEffect myAudioEffect = MyAudio.Static.ApplyEffect(m_sound, myStringHash, m_cueCache.ToArray());
				m_sound = myAudioEffect.OutputSound;
				m_cueCache.Clear();
			}
		}

		private void OnStopSelected(MyGuiControlButton button)
		{
			if (m_sound != null && m_sound.IsPlaying)
			{
				m_sound.Stop(force: true);
			}
		}

		private void OnSave(MyGuiControlButton button)
		{
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = new MyObjectBuilder_Definitions();
			DictionaryValuesReader<MyDefinitionId, MyAudioDefinition> soundDefinitions = MyDefinitionManager.Static.GetSoundDefinitions();
			myObjectBuilder_Definitions.Sounds = new MyObjectBuilder_AudioDefinition[soundDefinitions.Count];
			int num = 0;
			foreach (MyAudioDefinition item in soundDefinitions)
			{
				myObjectBuilder_Definitions.Sounds[num++] = (MyObjectBuilder_AudioDefinition)item.GetObjectBuilder();
			}
			MyObjectBuilderSerializer.SerializeXML(Path.Combine(MyFileSystem.ContentPath, "Data\\Audio.sbc"), compress: false, myObjectBuilder_Definitions);
		}

		private void OnReload(MyGuiControlButton button)
		{
			MyAudio.Static.UnloadData();
			MyDefinitionManager.Static.PreloadDefinitions();
			MyAudio.Static.ReloadData(MyAudioExtensions.GetSoundDataFromDefinitions(), MyAudioExtensions.GetEffectData());
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugAudio";
		}
	}
}
