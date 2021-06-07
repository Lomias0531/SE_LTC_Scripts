using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.AdminMenu
{
	[StaticEventOwner]
	internal class MyTrashGeneralTabContainer : MyTabContainer
	{
		private MyGuiControlTextbox m_textboxBlockCount;

		private MyGuiControlTextbox m_textboxDistanceTrash;

		private MyGuiControlTextbox m_textboxLogoutAgeTrash;

		private MyGuiControlCheckbox m_checkboxFixed;

		private MyGuiControlCheckbox m_checkboxStationary;

		private MyGuiControlCheckbox m_checkboxLinear;

		private MyGuiControlCheckbox m_chkeckboxAccelerating;

		private MyGuiControlCheckbox m_checkboxPowered;

		private MyGuiControlCheckbox m_checkboxControlled;

		private MyGuiControlCheckbox m_checkboxWithProduction;

		private MyGuiControlCheckbox m_checkboxMedbay;

		private bool m_showMedbayNotification = true;

		public MyTrashGeneralTabContainer(MyGuiScreenBase parentScreen)
			: base(parentScreen)
		{
			base.Control.Size = new Vector2(base.Control.Size.X, 0.557f);
			Vector2 currentPosition = -base.Control.Size * 0.5f;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			CreateTrashCheckBoxes(ref currentPosition);
			Vector2? size = parentScreen.GetSize();
			currentPosition.Y += 0.045f;
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_WithBlockCount)
			};
			control.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_WithBlockCount_Tooltip));
			base.Control.Controls.Add(control);
			m_textboxBlockCount = AddTextbox(ref currentPosition, MySession.Static.Settings.BlockCountThreshold.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false);
			base.Control.Controls.Add(m_textboxBlockCount);
			m_textboxBlockCount.Size = new Vector2(0.07f, m_textboxBlockCount.Size.Y);
			m_textboxBlockCount.PositionX = currentPosition.X + size.Value.X - m_textboxBlockCount.Size.X - 0.045f;
			m_textboxBlockCount.PositionY = currentPosition.Y - 0.01f;
			m_textboxBlockCount.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_WithBlockCount_Tooltip));
			currentPosition.Y += 0.045f;
			MyGuiControlLabel control2 = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_DistanceFromPlayer)
			};
			control2.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_DistanceFromPlayer_Tooltip));
			base.Control.Controls.Add(control2);
			m_textboxDistanceTrash = AddTextbox(ref currentPosition, MySession.Static.Settings.PlayerDistanceThreshold.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false);
			base.Control.Controls.Add(m_textboxDistanceTrash);
			m_textboxDistanceTrash.Size = new Vector2(0.07f, m_textboxDistanceTrash.Size.Y);
			m_textboxDistanceTrash.PositionX = currentPosition.X + size.Value.X - m_textboxDistanceTrash.Size.X - 0.045f;
			m_textboxDistanceTrash.PositionY = currentPosition.Y - 0.01f;
			m_textboxDistanceTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_DistanceFromPlayer_Tooltip));
			currentPosition.Y += 0.045f;
			MyGuiControlLabel control3 = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PlayerLogoutAge)
			};
			control3.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PlayerLogoutAge_Tooltip));
			base.Control.Controls.Add(control3);
			m_textboxLogoutAgeTrash = AddTextbox(ref currentPosition, MySession.Static.Settings.PlayerInactivityThreshold.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false);
			base.Control.Controls.Add(m_textboxLogoutAgeTrash);
			m_textboxLogoutAgeTrash.Size = new Vector2(0.07f, m_textboxLogoutAgeTrash.Size.Y);
			m_textboxLogoutAgeTrash.PositionX = currentPosition.X + size.Value.X - m_textboxLogoutAgeTrash.Size.X - 0.045f;
			m_textboxLogoutAgeTrash.PositionY = currentPosition.Y - 0.01f;
			m_textboxLogoutAgeTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PlayerLogoutAge_Tooltip));
			currentPosition.Y += 0.045f;
			myGuiControlSeparatorList.AddHorizontal(currentPosition - new Vector2(0.002f, 0f), size.Value.X * 0.73f);
			currentPosition.Y += 0.02f;
			float num = 0.14f;
			Vector2 currentPosition2 = currentPosition + new Vector2(num * 0.5f, 0f);
			MyGuiControlButton control4 = CreateDebugButton(ref currentPosition2, num, (!MySession.Static.Settings.TrashRemovalEnabled) ? MyCommonTexts.ScreenDebugAdminMenu_ResumeTrashButton : MyCommonTexts.ScreenDebugAdminMenu_PauseTrashButton, OnTrashButtonClicked, enabled: true, MyCommonTexts.ScreenDebugAdminMenu_PauseTrashButtonTooltip, increaseSpacing: false, addToControls: false);
			base.Control.Controls.Add(control4);
			base.Control.Controls.Add(myGuiControlSeparatorList);
		}

		protected virtual void CreateTrashCheckBoxes(ref Vector2 currentPosition)
		{
			MyTrashRemovalFlags myTrashRemovalFlags = MyTrashRemovalFlags.Fixed;
			string text = string.Format(MySessionComponentTrash.GetName(myTrashRemovalFlags), string.Empty);
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = text
			};
			m_checkboxFixed = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxFixed.IsChecked = ((MySession.Static.Settings.TrashFlags & myTrashRemovalFlags) == myTrashRemovalFlags);
			m_checkboxFixed.UserData = myTrashRemovalFlags;
			base.Control.Controls.Add(m_checkboxFixed);
			base.Control.Controls.Add(control);
			MyTrashRemovalFlags myTrashRemovalFlags2 = MyTrashRemovalFlags.Stationary;
			text = string.Format(MySessionComponentTrash.GetName(myTrashRemovalFlags2), string.Empty);
			currentPosition.Y += 0.045f;
			control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = text
			};
			m_checkboxStationary = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxStationary.IsChecked = ((MySession.Static.Settings.TrashFlags & myTrashRemovalFlags2) == myTrashRemovalFlags2);
			m_checkboxStationary.UserData = myTrashRemovalFlags2;
			base.Control.Controls.Add(m_checkboxStationary);
			base.Control.Controls.Add(control);
			MyTrashRemovalFlags myTrashRemovalFlags3 = MyTrashRemovalFlags.Linear;
			text = string.Format(MySessionComponentTrash.GetName(myTrashRemovalFlags3), string.Empty);
			currentPosition.Y += 0.045f;
			control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = text
			};
			m_checkboxLinear = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxLinear.IsChecked = ((MySession.Static.Settings.TrashFlags & myTrashRemovalFlags3) == myTrashRemovalFlags3);
			m_checkboxLinear.UserData = myTrashRemovalFlags3;
			base.Control.Controls.Add(m_checkboxLinear);
			base.Control.Controls.Add(control);
			MyTrashRemovalFlags myTrashRemovalFlags4 = MyTrashRemovalFlags.Accelerating;
			text = string.Format(MySessionComponentTrash.GetName(myTrashRemovalFlags4), string.Empty);
			currentPosition.Y += 0.045f;
			control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = text
			};
			m_chkeckboxAccelerating = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_chkeckboxAccelerating.IsChecked = ((MySession.Static.Settings.TrashFlags & myTrashRemovalFlags4) == myTrashRemovalFlags4);
			m_chkeckboxAccelerating.UserData = myTrashRemovalFlags4;
			base.Control.Controls.Add(m_chkeckboxAccelerating);
			base.Control.Controls.Add(control);
			MyTrashRemovalFlags myTrashRemovalFlags5 = MyTrashRemovalFlags.Powered;
			text = string.Format(MySessionComponentTrash.GetName(myTrashRemovalFlags5), string.Empty);
			currentPosition.Y += 0.045f;
			control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = text
			};
			m_checkboxPowered = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxPowered.IsChecked = ((MySession.Static.Settings.TrashFlags & myTrashRemovalFlags5) == myTrashRemovalFlags5);
			m_checkboxPowered.UserData = myTrashRemovalFlags5;
			base.Control.Controls.Add(m_checkboxPowered);
			base.Control.Controls.Add(control);
			MyTrashRemovalFlags myTrashRemovalFlags6 = MyTrashRemovalFlags.Controlled;
			text = string.Format(MySessionComponentTrash.GetName(myTrashRemovalFlags6), string.Empty);
			currentPosition.Y += 0.045f;
			control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = text
			};
			m_checkboxControlled = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxControlled.IsChecked = ((MySession.Static.Settings.TrashFlags & myTrashRemovalFlags6) == myTrashRemovalFlags6);
			m_checkboxControlled.UserData = myTrashRemovalFlags6;
			base.Control.Controls.Add(m_checkboxControlled);
			base.Control.Controls.Add(control);
			MyTrashRemovalFlags myTrashRemovalFlags7 = MyTrashRemovalFlags.WithProduction;
			text = string.Format(MySessionComponentTrash.GetName(myTrashRemovalFlags7), string.Empty);
			currentPosition.Y += 0.045f;
			control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = text
			};
			m_checkboxWithProduction = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxWithProduction.IsChecked = ((MySession.Static.Settings.TrashFlags & myTrashRemovalFlags7) == myTrashRemovalFlags7);
			m_checkboxWithProduction.UserData = myTrashRemovalFlags7;
			base.Control.Controls.Add(m_checkboxWithProduction);
			base.Control.Controls.Add(control);
			MyTrashRemovalFlags myTrashRemovalFlags8 = MyTrashRemovalFlags.WithMedBay;
			text = string.Format(MySessionComponentTrash.GetName(myTrashRemovalFlags8), string.Empty);
			currentPosition.Y += 0.045f;
			control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = text
			};
			m_checkboxMedbay = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxMedbay.IsChecked = ((MySession.Static.Settings.TrashFlags & myTrashRemovalFlags8) == myTrashRemovalFlags8);
			m_checkboxMedbay.UserData = myTrashRemovalFlags8;
			m_checkboxMedbay.IsCheckedChanged = delegate(MyGuiControlCheckbox c)
			{
				OnTrashFlagChanged(c.IsChecked);
			};
			base.Control.Controls.Add(m_checkboxMedbay);
			base.Control.Controls.Add(control);
		}

		private void OnTrashButtonClicked(MyGuiControlButton obj)
		{
			MySession.Static.Settings.TrashRemovalEnabled = !MySession.Static.Settings.TrashRemovalEnabled;
			if (!MySession.Static.Settings.TrashRemovalEnabled)
			{
				obj.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_ResumeTrashButton);
			}
			else
			{
				obj.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PauseTrashButton);
			}
			MyGuiScreenAdminMenu.RecalcTrash();
		}

		private void OnTrashFlagChanged(bool value)
		{
			if (value && m_showMedbayNotification)
			{
				MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_MedbayNotification)));
				m_showMedbayNotification = false;
			}
		}

		internal override bool GetSettings(ref MyGuiScreenAdminMenu.AdminSettings newSettings)
		{
			if (m_textboxBlockCount == null || m_textboxDistanceTrash == null || m_textboxLogoutAgeTrash == null)
			{
				return false;
			}
			int.TryParse(m_textboxBlockCount.Text, out int result);
			float.TryParse(m_textboxDistanceTrash.Text, out float result2);
			float.TryParse(m_textboxLogoutAgeTrash.Text, out float result3);
			int num = 0 | ((MySession.Static.Settings.BlockCountThreshold != result) ? 1 : 0) | ((MySession.Static.Settings.PlayerDistanceThreshold != result2) ? 1 : 0) | ((MySession.Static.Settings.PlayerInactivityThreshold != result3) ? 1 : 0);
			newSettings.blockCount = result;
			newSettings.playerDistance = result2;
			newSettings.playerInactivity = result3;
			newSettings.flags = (m_checkboxFixed.IsChecked ? (newSettings.flags | (MyTrashRemovalFlags)m_checkboxFixed.UserData) : (newSettings.flags & ~(MyTrashRemovalFlags)m_checkboxFixed.UserData));
			newSettings.flags = (m_checkboxLinear.IsChecked ? (newSettings.flags | (MyTrashRemovalFlags)m_checkboxLinear.UserData) : (newSettings.flags & ~(MyTrashRemovalFlags)m_checkboxLinear.UserData));
			newSettings.flags = (m_checkboxMedbay.IsChecked ? (newSettings.flags | (MyTrashRemovalFlags)m_checkboxMedbay.UserData) : (newSettings.flags & ~(MyTrashRemovalFlags)m_checkboxMedbay.UserData));
			newSettings.flags = (m_checkboxPowered.IsChecked ? (newSettings.flags | (MyTrashRemovalFlags)m_checkboxPowered.UserData) : (newSettings.flags & ~(MyTrashRemovalFlags)m_checkboxPowered.UserData));
			newSettings.flags = (m_checkboxStationary.IsChecked ? (newSettings.flags | (MyTrashRemovalFlags)m_checkboxStationary.UserData) : (newSettings.flags & ~(MyTrashRemovalFlags)m_checkboxStationary.UserData));
			newSettings.flags = (m_checkboxWithProduction.IsChecked ? (newSettings.flags | (MyTrashRemovalFlags)m_checkboxWithProduction.UserData) : (newSettings.flags & ~(MyTrashRemovalFlags)m_checkboxWithProduction.UserData));
			newSettings.flags = (m_checkboxControlled.IsChecked ? (newSettings.flags | (MyTrashRemovalFlags)m_checkboxControlled.UserData) : (newSettings.flags & ~(MyTrashRemovalFlags)m_checkboxControlled.UserData));
			newSettings.flags = (m_chkeckboxAccelerating.IsChecked ? (newSettings.flags | (MyTrashRemovalFlags)m_chkeckboxAccelerating.UserData) : (newSettings.flags & ~(MyTrashRemovalFlags)m_chkeckboxAccelerating.UserData));
			return (byte)(num | ((MySession.Static.Settings.TrashFlags != newSettings.flags) ? 1 : 0)) != 0;
		}
	}
}
