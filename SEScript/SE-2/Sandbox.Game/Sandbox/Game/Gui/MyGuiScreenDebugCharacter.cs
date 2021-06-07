using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions.Animation;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("VRage", "Character")]
	internal class MyGuiScreenDebugCharacter : MyGuiScreenDebugBase
	{
		private MyGuiControlCombobox m_animationComboA;

		private MyGuiControlCombobox m_animationComboB;

		private MyGuiControlSlider m_blendSlider;

		private MyGuiControlCombobox m_animationCombo;

		private MyGuiControlCheckbox m_loopCheckbox;

		public MyGuiScreenDebugCharacter()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Render Character", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			if (MySession.Static == null || MySession.Static.ControlledEntity == null || !(MySession.Static.ControlledEntity is MyCharacter))
			{
				AddLabel("None active character", Color.Yellow.ToVector4(), 1.2f);
				return;
			}
			MyCharacter playerCharacter = MySession.Static.LocalCharacter;
			if (constructor)
			{
				MyAnimationControllerDefinition definition = MyDefinitionManagerBase.Static.GetDefinition<MyAnimationControllerDefinition>("Debug");
				if (definition == null)
				{
					return;
				}
				playerCharacter.AnimationController.Clear();
				playerCharacter.AnimationController.InitFromDefinition(definition, forceReloadMwm: true);
				if (playerCharacter.AnimationController.ReloadBonesNeeded != null)
				{
					playerCharacter.AnimationController.ReloadBonesNeeded();
				}
			}
			AddSlider("Max slope", playerCharacter.Definition.MaxSlope, 0f, 89f, delegate(MyGuiControlSlider slider)
			{
				playerCharacter.Definition.MaxSlope = slider.Value;
			});
			AddLabel(playerCharacter.Model.AssetName, Color.Yellow.ToVector4(), 1.2f);
			AddLabel("Animation A:", Color.Yellow.ToVector4(), 1.2f);
			m_animationComboA = AddCombo();
			ListReader<MyAnimationDefinition> animationDefinitions = MyDefinitionManager.Static.GetAnimationDefinitions();
			int num = 0;
			foreach (MyAnimationDefinition item in animationDefinitions)
			{
				m_animationComboA.AddItem(num++, new StringBuilder(item.Id.SubtypeName));
			}
			m_animationComboA.SelectItemByIndex(0);
			AddLabel("Animation B:", Color.Yellow.ToVector4(), 1.2f);
			m_animationComboB = AddCombo();
			num = 0;
			foreach (MyAnimationDefinition item2 in animationDefinitions)
			{
				m_animationComboB.AddItem(num++, new StringBuilder(item2.Id.SubtypeName));
			}
			m_animationComboB.SelectItemByIndex(0);
			m_blendSlider = AddSlider("Blend time", 0.5f, 0f, 3f);
			AddButton(new StringBuilder("Play A->B"), OnPlayBlendButtonClick);
			m_currentPosition.Y += 0.01f;
			m_animationCombo = AddCombo();
			num = 0;
			foreach (MyAnimationDefinition item3 in animationDefinitions)
			{
				m_animationCombo.AddItem(num++, new StringBuilder(item3.Id.SubtypeName));
			}
			m_animationCombo.SortItemsByValueText();
			m_animationCombo.SelectItemByIndex(0);
			m_loopCheckbox = AddCheckBox("Loop", checkedState: false, null);
			m_currentPosition.Y += 0.02f;
			foreach (string key in playerCharacter.Definition.BoneSets.Keys)
			{
				MyGuiControlCheckbox myGuiControlCheckbox = AddCheckBox(key, checkedState: false, null);
				myGuiControlCheckbox.UserData = key;
				if (key == "Body")
				{
					myGuiControlCheckbox.IsChecked = true;
				}
			}
			AddButton(new StringBuilder("Play animation"), OnPlayButtonClick);
			AddCheckBox("Draw damage and hit hapsules", () => MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE, delegate(bool s)
			{
				MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE = s;
			});
			m_currentPosition.Y += 0.01f;
			AddSlider("Gravity mult", MyPerGameSettings.CharacterGravityMultiplier, 0f, 5f, delegate(MyGuiControlSlider slider)
			{
				MyPerGameSettings.CharacterGravityMultiplier = slider.Value;
			});
		}

		private void OnPlayBlendButtonClick(MyGuiControlButton sender)
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			localCharacter.PlayCharacterAnimation(m_animationComboA.GetSelectedKey().ToString(), MyBlendOption.Immediate, MyFrameOption.PlayOnce, m_blendSlider.Value);
			localCharacter.PlayCharacterAnimation(m_animationComboB.GetSelectedKey().ToString(), MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, m_blendSlider.Value);
		}

		private void OnPlayButtonClick(MyGuiControlButton sender)
		{
			if (MySession.Static.LocalCharacter.UseNewAnimationSystem)
			{
				MySession.Static.LocalCharacter.TriggerCharacterAnimationEvent("play", sync: false);
				MySession.Static.LocalCharacter.TriggerCharacterAnimationEvent(m_animationCombo.GetSelectedValue().ToString(), sync: false);
			}
			else
			{
				MySession.Static.LocalCharacter.PlayCharacterAnimation(m_animationCombo.GetSelectedValue().ToString(), MyBlendOption.Immediate, (!m_loopCheckbox.IsChecked) ? MyFrameOption.PlayOnce : MyFrameOption.Loop, m_blendSlider.Value);
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugCharacter";
		}

		public override bool CloseScreen()
		{
			if (MySession.Static != null)
			{
				_ = MySession.Static.LocalCharacter;
			}
			return base.CloseScreen();
		}
	}
}
