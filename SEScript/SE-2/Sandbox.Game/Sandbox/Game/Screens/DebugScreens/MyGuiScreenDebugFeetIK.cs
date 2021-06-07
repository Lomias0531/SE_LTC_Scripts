using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("VRage", "Character feet IK")]
	internal class MyGuiScreenDebugFeetIK : MyGuiScreenDebugBase
	{
		private MyGuiControlSlider belowReachableDistance;

		private MyGuiControlSlider aboveReachableDistance;

		private MyGuiControlSlider verticalChangeUpGain;

		private MyGuiControlSlider verticalChangeDownGain;

		private MyGuiControlSlider ankleHeight;

		private MyGuiControlSlider footWidth;

		private MyGuiControlSlider footLength;

		private MyGuiControlCombobox characterMovementStateCombo;

		private MyGuiControlCheckbox enabledIKState;

		public static bool ikSettingsEnabled;

		private MyFeetIKSettings ikSettings;

		public bool updating;

		public MyRagdollMapper PlayerRagdollMapper => MySession.Static.LocalCharacter.Components.Get<MyCharacterRagdollComponent>()?.RagdollMapper;

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugFeetIK";
		}

		public MyGuiScreenDebugFeetIK()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Character feet IK debug draw", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddCheckBox("Draw IK Settings ", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_SETTINGS));
			AddCheckBox("Draw ankle final position", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_ANKLE_FINALPOS));
			AddCheckBox("Draw raycast lines and foot lines", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_RAYCASTLINE));
			AddCheckBox("Draw bones", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_BONES));
			AddCheckBox("Draw raycast hits", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_RAYCASTHITS));
			AddCheckBox("Draw ankle desired positions", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_ANKLE_DESIREDPOSITION));
			AddCheckBox("Draw closest support position", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_CLOSESTSUPPORTPOSITION));
			AddCheckBox("Draw IK solvers debug", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_IKSOLVERS));
			AddCheckBox("Enable/Disable Feet IK", null, MemberHelper.GetMember(() => MyFakes.ENABLE_FOOT_IK));
			enabledIKState = AddCheckBox("Enable IK for this state", null, MemberHelper.GetMember(() => ikSettingsEnabled));
			belowReachableDistance = AddSlider("Reachable distance below character", 0f, 0f, 2f);
			aboveReachableDistance = AddSlider("Reachable distance above character", 0f, 0f, 2f);
			verticalChangeUpGain = AddSlider("Shift Up Gain", 0.1f, 0f, 1f);
			verticalChangeDownGain = AddSlider("Sift Down Gain", 0.1f, 0f, 1f);
			ankleHeight = AddSlider("Ankle height", 0.1f, 0.001f, 0.3f);
			footWidth = AddSlider("Foot width", 0.1f, 0.001f, 0.3f);
			footLength = AddSlider("Foot length", 0.3f, 0.001f, 0.2f);
			RegisterEvents();
		}

		private void ItemChanged(MyGuiControlSlider slider)
		{
			if (!updating)
			{
				ikSettings.Enabled = enabledIKState.IsChecked;
				ikSettings.BelowReachableDistance = belowReachableDistance.Value;
				ikSettings.AboveReachableDistance = aboveReachableDistance.Value;
				ikSettings.VerticalShiftUpGain = verticalChangeUpGain.Value;
				ikSettings.VerticalShiftDownGain = verticalChangeDownGain.Value;
				ikSettings.FootSize.Y = ankleHeight.Value;
				ikSettings.FootSize.X = footWidth.Value;
				ikSettings.FootSize.Z = footLength.Value;
				MyCharacter localCharacter = MySession.Static.LocalCharacter;
				MyCharacterMovementEnum key = MyCharacterMovementEnum.Standing;
				localCharacter.Definition.FeetIKSettings[key] = ikSettings;
			}
		}

		private void RegisterEvents()
		{
			MyGuiControlSlider myGuiControlSlider = belowReachableDistance;
			myGuiControlSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(myGuiControlSlider.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlSlider myGuiControlSlider2 = aboveReachableDistance;
			myGuiControlSlider2.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(myGuiControlSlider2.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlSlider myGuiControlSlider3 = verticalChangeUpGain;
			myGuiControlSlider3.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(myGuiControlSlider3.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlSlider myGuiControlSlider4 = verticalChangeDownGain;
			myGuiControlSlider4.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(myGuiControlSlider4.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlSlider myGuiControlSlider5 = ankleHeight;
			myGuiControlSlider5.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(myGuiControlSlider5.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlSlider myGuiControlSlider6 = footWidth;
			myGuiControlSlider6.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(myGuiControlSlider6.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlSlider myGuiControlSlider7 = footLength;
			myGuiControlSlider7.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(myGuiControlSlider7.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlCheckbox myGuiControlCheckbox = enabledIKState;
			myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(IsCheckedChanged));
		}

		private void IsCheckedChanged(MyGuiControlCheckbox obj)
		{
			ItemChanged(null);
		}

		private void characterMovementStateCombo_ItemSelected()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			MyCharacterMovementEnum key = MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_MOVEMENT_STATE = (MyCharacterMovementEnum)characterMovementStateCombo.GetSelectedKey();
			if (!localCharacter.Definition.FeetIKSettings.TryGetValue(key, out ikSettings))
			{
				ikSettings = default(MyFeetIKSettings);
				ikSettings.Enabled = false;
				ikSettings.AboveReachableDistance = 0.1f;
				ikSettings.BelowReachableDistance = 0.1f;
				ikSettings.VerticalShiftDownGain = 0.1f;
				ikSettings.VerticalShiftUpGain = 0.1f;
				ikSettings.FootSize = new Vector3(0.1f, 0.1f, 0.2f);
			}
			updating = true;
			enabledIKState.IsChecked = ikSettings.Enabled;
			belowReachableDistance.Value = ikSettings.BelowReachableDistance;
			aboveReachableDistance.Value = ikSettings.AboveReachableDistance;
			verticalChangeUpGain.Value = ikSettings.VerticalShiftUpGain;
			verticalChangeDownGain.Value = ikSettings.VerticalShiftDownGain;
			ankleHeight.Value = ikSettings.FootSize.Y;
			footWidth.Value = ikSettings.FootSize.X;
			footLength.Value = ikSettings.FootSize.Z;
			updating = false;
		}

		private void UnRegisterEvents()
		{
			characterMovementStateCombo.ItemSelected -= characterMovementStateCombo_ItemSelected;
			MyGuiControlSlider myGuiControlSlider = belowReachableDistance;
			myGuiControlSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(myGuiControlSlider.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlSlider myGuiControlSlider2 = aboveReachableDistance;
			myGuiControlSlider2.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(myGuiControlSlider2.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlSlider myGuiControlSlider3 = verticalChangeUpGain;
			myGuiControlSlider3.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(myGuiControlSlider3.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlSlider myGuiControlSlider4 = verticalChangeDownGain;
			myGuiControlSlider4.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(myGuiControlSlider4.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlSlider myGuiControlSlider5 = ankleHeight;
			myGuiControlSlider5.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(myGuiControlSlider5.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlSlider myGuiControlSlider6 = footWidth;
			myGuiControlSlider6.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(myGuiControlSlider6.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlSlider myGuiControlSlider7 = footLength;
			myGuiControlSlider7.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(myGuiControlSlider7.ValueChanged, new Action<MyGuiControlSlider>(ItemChanged));
			MyGuiControlCheckbox myGuiControlCheckbox = enabledIKState;
			myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(myGuiControlCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(IsCheckedChanged));
		}
	}
}
