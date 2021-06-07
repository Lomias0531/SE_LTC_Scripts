using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenVoxelHandSetting : MyGuiScreenBase
	{
		private static readonly Vector2 SCREEN_SIZE = new Vector2(0.37f, 1.2f);

		private static readonly float HIDDEN_PART_RIGHT = 0.04f;

		private Vector2 m_controlPadding = new Vector2(0.02f, 0.02f);

		private MyGuiControlLabel m_labelSettings;

		private MyGuiControlLabel m_labelSnapToVoxel;

		private MyGuiControlCheckbox m_checkSnapToVoxel;

		private MyGuiControlLabel m_labelProjectToVoxel;

		private MyGuiControlCheckbox m_projectToVoxel;

		private MyGuiControlLabel m_labelFreezePhysics;

		private MyGuiControlCheckbox m_freezePhysicsCheck;

		private MyGuiControlLabel m_labelShowGizmos;

		private MyGuiControlCheckbox m_showGizmos;

		private MyGuiControlLabel m_labelTransparency;

		private MyGuiControlSlider m_sliderTransparency;

		private MyGuiControlLabel m_labelZoom;

		private MyGuiControlSlider m_sliderZoom;

		private MyGuiControlVoxelHandSettings m_voxelControl;

		public MyGuiScreenVoxelHandSetting()
			: base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - SCREEN_SIZE.X * 0.5f + HIDDEN_PART_RIGHT, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity, SCREEN_SIZE)
		{
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = false;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			float num = -0.465f;
			float num2 = (SCREEN_SIZE.Y - 1f) / 2f;
			AddCaption(MyTexts.Get(MyCommonTexts.VoxelHandSettingScreen_Caption).ToString(), Color.White.ToVector4(), m_controlPadding + new Vector2(0f - HIDDEN_PART_RIGHT, num2 - 0.03f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.44f), m_size.Value.X * 0.73f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.048f), m_size.Value.X * 0.73f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, -0.235f), m_size.Value.X * 0.73f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, -0.394f), m_size.Value.X * 0.73f);
			Controls.Add(myGuiControlSeparatorList);
			num += 0.042f;
			m_checkSnapToVoxel = new MyGuiControlCheckbox
			{
				Position = new Vector2(0.12f, num),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP
			};
			m_checkSnapToVoxel.IsChecked = MySessionComponentVoxelHand.Static.SnapToVoxel;
			MyGuiControlCheckbox checkSnapToVoxel = m_checkSnapToVoxel;
			checkSnapToVoxel.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkSnapToVoxel.IsCheckedChanged, new Action<MyGuiControlCheckbox>(SnapToVoxel_Changed));
			num += 0.01f;
			m_labelSnapToVoxel = new MyGuiControlLabel
			{
				Position = new Vector2(-0.15f, num),
				TextEnum = MyCommonTexts.VoxelHandSettingScreen_HandSnapToVoxel,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			num += 0.036f;
			m_projectToVoxel = new MyGuiControlCheckbox
			{
				Position = new Vector2(0.12f, num),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP
			};
			m_projectToVoxel.IsChecked = MySessionComponentVoxelHand.Static.ProjectToVoxel;
			MyGuiControlCheckbox projectToVoxel = m_projectToVoxel;
			projectToVoxel.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(projectToVoxel.IsCheckedChanged, new Action<MyGuiControlCheckbox>(ProjectToVoxel_Changed));
			num += 0.01f;
			m_labelProjectToVoxel = new MyGuiControlLabel
			{
				Position = new Vector2(-0.15f, num),
				TextEnum = MyCommonTexts.VoxelHandSettingScreen_HandProjectToVoxel,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			num += 0.036f;
			m_freezePhysicsCheck = new MyGuiControlCheckbox
			{
				Position = new Vector2(0.12f, num),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP
			};
			m_freezePhysicsCheck.IsChecked = MySessionComponentVoxelHand.Static.FreezePhysics;
			MyGuiControlCheckbox freezePhysicsCheck = m_freezePhysicsCheck;
			freezePhysicsCheck.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(freezePhysicsCheck.IsCheckedChanged, new Action<MyGuiControlCheckbox>(FreezePhysics_Changed));
			num += 0.01f;
			m_labelFreezePhysics = new MyGuiControlLabel
			{
				Position = new Vector2(-0.15f, num),
				TextEnum = MyCommonTexts.VoxelHandSettingScreen_FreezePhysics,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			num += 0.036f;
			m_showGizmos = new MyGuiControlCheckbox
			{
				Position = new Vector2(0.12f, num),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP
			};
			m_showGizmos.IsChecked = MySessionComponentVoxelHand.Static.ShowGizmos;
			MyGuiControlCheckbox showGizmos = m_showGizmos;
			showGizmos.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(showGizmos.IsCheckedChanged, new Action<MyGuiControlCheckbox>(ShowGizmos_Changed));
			num += 0.01f;
			m_labelShowGizmos = new MyGuiControlLabel
			{
				Position = new Vector2(-0.15f, num),
				TextEnum = MyCommonTexts.VoxelHandSettingScreen_HandShowGizmos,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			num += 0.045f;
			m_labelTransparency = new MyGuiControlLabel
			{
				Position = new Vector2(-0.15f, num),
				TextEnum = MyCommonTexts.VoxelHandSettingScreen_HandTransparency,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			num += 0.027f;
			m_sliderTransparency = new MyGuiControlSlider
			{
				Position = new Vector2(-0.15f, num),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_sliderTransparency.Size = new Vector2(0.263f, 0.1f);
			m_sliderTransparency.MinValue = 0f;
			m_sliderTransparency.MaxValue = 1f;
			m_sliderTransparency.Value = 1f - MySessionComponentVoxelHand.Static.ShapeColor.ToVector4().W;
			MyGuiControlSlider sliderTransparency = m_sliderTransparency;
			sliderTransparency.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderTransparency.ValueChanged, new Action<MyGuiControlSlider>(BrushTransparency_ValueChanged));
			num += 0.057f;
			m_labelZoom = new MyGuiControlLabel
			{
				Position = new Vector2(-0.15f, num),
				TextEnum = MyCommonTexts.VoxelHandSettingScreen_HandDistance,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			num += 0.027f;
			m_sliderZoom = new MyGuiControlSlider
			{
				Position = new Vector2(-0.15f, num),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_sliderZoom.Size = new Vector2(0.263f, 0.1f);
			m_sliderZoom.MaxValue = MySessionComponentVoxelHand.MAX_BRUSH_ZOOM;
			m_sliderZoom.Value = MySessionComponentVoxelHand.Static.GetBrushZoom();
			m_sliderZoom.MinValue = MySessionComponentVoxelHand.MIN_BRUSH_ZOOM;
			m_sliderZoom.Enabled = !MySessionComponentVoxelHand.Static.ProjectToVoxel;
			MyGuiControlSlider sliderZoom = m_sliderZoom;
			sliderZoom.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderZoom.ValueChanged, new Action<MyGuiControlSlider>(BrushZoom_ValueChanged));
			m_voxelControl = new MyGuiControlVoxelHandSettings();
			m_voxelControl.Position = new Vector2(-0.05f, 0.17f);
			m_voxelControl.Item = (MyToolbarComponent.CurrentToolbar.SelectedItem as MyToolbarItemVoxelHand);
			m_voxelControl.UpdateFromBrush(MySessionComponentVoxelHand.Static.CurrentShape);
			StringBuilder text;
			if (MyInput.Static.IsJoystickLastUsed)
			{
				StringBuilder stringBuilder = null;
				StringBuilder stringBuilder2 = null;
				StringBuilder stringBuilder3 = null;
				stringBuilder = new StringBuilder(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.PRIMARY_TOOL_ACTION));
				stringBuilder2 = new StringBuilder(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.SECONDARY_TOOL_ACTION));
				stringBuilder3 = new StringBuilder(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_MATERIAL_SELECT));
				text = new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.VoxelHands_Description_Gamepad), stringBuilder, stringBuilder2, stringBuilder3));
			}
			else
			{
				StringBuilder output = null;
				StringBuilder output2 = null;
				StringBuilder output3 = null;
				StringBuilder output4 = null;
				MyInput.Static.GetGameControl(MyControlsSpace.PRIMARY_TOOL_ACTION).AppendBoundButtonNames(ref output, ", ", MyInput.Static.GetUnassignedName());
				MyInput.Static.GetGameControl(MyControlsSpace.SECONDARY_TOOL_ACTION).AppendBoundButtonNames(ref output2, ", ", MyInput.Static.GetUnassignedName());
				MyInput.Static.GetGameControl(MyControlsSpace.SWITCH_LEFT).AppendBoundButtonNames(ref output3, ", ", MyInput.Static.GetUnassignedName());
				MyInput.Static.GetGameControl(MyControlsSpace.SWITCH_RIGHT).AppendBoundButtonNames(ref output4, ", ", MyInput.Static.GetUnassignedName());
				text = new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.VoxelHands_Description), output, output2, output3, output4));
			}
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(new Vector2(-0.15f, 0.252f), new Vector2(0.275f, 0.125f));
			myGuiControlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlMultilineText.Text = text;
			Controls.Add(myGuiControlMultilineText);
			Vector2 value = new Vector2(-0.083f, 0.36f);
			Vector2 value2 = new Vector2(0.134f, 0.038f);
			float usableWidth = 0.265f;
			MyGuiControlButton myGuiControlButton = CreateButton(usableWidth, MyTexts.Get(MyCommonTexts.Close), OKButtonClicked, enabled: true, MySpaceTexts.ToolTipNewsletter_Close, 0.8f);
			myGuiControlButton.Position = value + new Vector2(0f, 2f) * value2;
			myGuiControlButton.PositionX += value2.X / 2f;
			myGuiControlButton.ShowTooltipWhenDisabled = true;
			Controls.Add(myGuiControlButton);
			Controls.Add(m_labelSnapToVoxel);
			Controls.Add(m_checkSnapToVoxel);
			Controls.Add(m_labelShowGizmos);
			Controls.Add(m_showGizmos);
			Controls.Add(m_labelProjectToVoxel);
			Controls.Add(m_projectToVoxel);
			Controls.Add(m_labelFreezePhysics);
			Controls.Add(m_freezePhysicsCheck);
			Controls.Add(m_labelTransparency);
			Controls.Add(m_sliderTransparency);
			Controls.Add(m_labelZoom);
			Controls.Add(m_sliderZoom);
			Controls.Add(m_voxelControl);
		}

		private MyGuiControlButton CreateButton(float usableWidth, StringBuilder text, Action<MyGuiControlButton> onClick, bool enabled = true, MyStringId? tooltip = null, float textScale = 1f)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, text, textScale, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			myGuiControlButton.Size = new Vector2(usableWidth, 0.034f);
			myGuiControlButton.Position += new Vector2(-0.02f, 0f);
			if (tooltip.HasValue)
			{
				myGuiControlButton.SetToolTip(tooltip.Value);
			}
			return myGuiControlButton;
		}

		private void SnapToVoxel_Changed(MyGuiControlCheckbox sender)
		{
			MySessionComponentVoxelHand.Static.SnapToVoxel = m_checkSnapToVoxel.IsChecked;
		}

		private void ShowGizmos_Changed(MyGuiControlCheckbox sender)
		{
			MySessionComponentVoxelHand.Static.ShowGizmos = m_showGizmos.IsChecked;
		}

		private void ProjectToVoxel_Changed(MyGuiControlCheckbox sender)
		{
			MySessionComponentVoxelHand.Static.ProjectToVoxel = m_projectToVoxel.IsChecked;
			m_sliderZoom.Enabled = !m_projectToVoxel.IsChecked;
		}

		private void FreezePhysics_Changed(MyGuiControlCheckbox sender)
		{
			MySessionComponentVoxelHand.Static.FreezePhysics = sender.IsChecked;
		}

		private void BrushTransparency_ValueChanged(MyGuiControlSlider sender)
		{
			MySessionComponentVoxelHand.Static.ShapeColor.A = (byte)((1f - m_sliderTransparency.Value) * 255f);
		}

		private void BrushZoom_ValueChanged(MyGuiControlSlider sender)
		{
			MySessionComponentVoxelHand.Static.SetBrushZoom(m_sliderZoom.Value);
		}

		private void OKButtonClicked(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.VOXEL_HAND_SETTINGS))
			{
				CloseScreen();
			}
			base.HandleInput(receivedFocusInThisUpdate);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenVoxelHandSetting";
		}
	}
}
