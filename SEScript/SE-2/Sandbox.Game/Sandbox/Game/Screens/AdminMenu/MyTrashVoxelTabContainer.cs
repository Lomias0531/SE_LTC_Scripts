using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.AdminMenu
{
	internal class MyTrashVoxelTabContainer : MyTabContainer
	{
		private MyGuiControlTextbox m_textboxVoxelPlayerDistanceTrash;

		private MyGuiControlTextbox m_textboxVoxelGridDistanceTrash;

		private MyGuiControlTextbox m_textboxVoxelAgeTrash;

		private MyGuiControlCheckbox m_checkboxRevertMaterials;

		private MyGuiControlCheckbox m_checkboxRevertAsteroids;

		private MyGuiControlCheckbox m_checkboxRevertFloatingPreset;

		public MyTrashVoxelTabContainer(MyGuiScreenBase parentScreen)
			: base(parentScreen)
		{
			base.Control.Size = new Vector2(base.Control.Size.X, 0.32f);
			Vector2 currentPosition = -base.Control.Size * 0.5f;
			Vector2? size = parentScreen.GetSize();
			CreateVoxelTrashCheckBoxes(ref currentPosition);
			currentPosition.Y += 0.045f;
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromPlayer)
			};
			control.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromPlayer_Tooltip));
			base.Control.Controls.Add(control);
			m_textboxVoxelPlayerDistanceTrash = AddTextbox(ref currentPosition, MySession.Static.Settings.VoxelPlayerDistanceThreshold.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false);
			base.Control.Controls.Add(m_textboxVoxelPlayerDistanceTrash);
			m_textboxVoxelPlayerDistanceTrash.Size = new Vector2(0.07f, m_textboxVoxelPlayerDistanceTrash.Size.Y);
			m_textboxVoxelPlayerDistanceTrash.PositionX = currentPosition.X + size.Value.X - m_textboxVoxelPlayerDistanceTrash.Size.X - 0.045f;
			m_textboxVoxelPlayerDistanceTrash.PositionY = currentPosition.Y;
			m_textboxVoxelPlayerDistanceTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromPlayer_Tooltip));
			currentPosition.Y += 0.045f;
			MyGuiControlLabel control2 = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromGrid)
			};
			control2.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromGrid_Tooltip));
			base.Control.Controls.Add(control2);
			m_textboxVoxelGridDistanceTrash = AddTextbox(ref currentPosition, MySession.Static.Settings.VoxelGridDistanceThreshold.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false);
			base.Control.Controls.Add(m_textboxVoxelGridDistanceTrash);
			m_textboxVoxelGridDistanceTrash.Size = new Vector2(0.07f, m_textboxVoxelGridDistanceTrash.Size.Y);
			m_textboxVoxelGridDistanceTrash.PositionX = currentPosition.X + size.Value.X - m_textboxVoxelGridDistanceTrash.Size.X - 0.045f;
			m_textboxVoxelGridDistanceTrash.PositionY = currentPosition.Y;
			m_textboxVoxelGridDistanceTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromGrid_Tooltip));
			currentPosition.Y += 0.045f;
			MyGuiControlLabel control3 = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelAge)
			};
			control3.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelAge_Tooltip));
			base.Control.Controls.Add(control3);
			m_textboxVoxelAgeTrash = AddTextbox(ref currentPosition, MySession.Static.Settings.VoxelAgeThreshold.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false);
			base.Control.Controls.Add(m_textboxVoxelAgeTrash);
			m_textboxVoxelAgeTrash.Size = new Vector2(0.07f, m_textboxVoxelAgeTrash.Size.Y);
			m_textboxVoxelAgeTrash.PositionX = currentPosition.X + size.Value.X - m_textboxVoxelAgeTrash.Size.X - 0.045f;
			m_textboxVoxelAgeTrash.PositionY = currentPosition.Y;
			m_textboxVoxelAgeTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelAge_Tooltip));
			currentPosition.Y += 0.055f;
			float num = 0.14f;
			Vector2 currentPosition2 = currentPosition + new Vector2(num * 0.5f, 0f);
			MyGuiControlButton control4 = CreateDebugButton(ref currentPosition2, num, (!MySession.Static.Settings.VoxelTrashRemovalEnabled) ? MyCommonTexts.ScreenDebugAdminMenu_ResumeTrashButton : MyCommonTexts.ScreenDebugAdminMenu_PauseTrashButton, OnTrashVoxelButtonClicked, enabled: true, MyCommonTexts.ScreenDebugAdminMenu_PauseTrashVoxelButtonTooltip, increaseSpacing: false, addToControls: false);
			base.Control.Controls.Add(control4);
		}

		protected virtual void CreateVoxelTrashCheckBoxes(ref Vector2 currentPosition)
		{
			MyTrashRemovalFlags myTrashRemovalFlags = MyTrashRemovalFlags.RevertMaterials;
			string text = string.Format(MySessionComponentTrash.GetName(myTrashRemovalFlags), string.Empty);
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = text
			};
			m_checkboxRevertMaterials = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxRevertMaterials.IsChecked = ((MySession.Static.Settings.TrashFlags & myTrashRemovalFlags) == myTrashRemovalFlags);
			m_checkboxRevertMaterials.UserData = myTrashRemovalFlags;
			base.Control.Controls.Add(m_checkboxRevertMaterials);
			base.Control.Controls.Add(control);
			MyTrashRemovalFlags myTrashRemovalFlags2 = MyTrashRemovalFlags.RevertAsteroids;
			text = string.Format(MySessionComponentTrash.GetName(myTrashRemovalFlags2), string.Empty);
			currentPosition.Y += 0.045f;
			control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = text
			};
			m_checkboxRevertAsteroids = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxRevertAsteroids.IsChecked = ((MySession.Static.Settings.TrashFlags & myTrashRemovalFlags2) == myTrashRemovalFlags2);
			m_checkboxRevertAsteroids.UserData = myTrashRemovalFlags2;
			base.Control.Controls.Add(m_checkboxRevertAsteroids);
			base.Control.Controls.Add(control);
			MyTrashRemovalFlags myTrashRemovalFlags3 = MyTrashRemovalFlags.RevertWithFloatingsPresent;
			text = string.Format(MySessionComponentTrash.GetName(myTrashRemovalFlags3), string.Empty);
			currentPosition.Y += 0.045f;
			control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = text
			};
			m_checkboxRevertFloatingPreset = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxRevertFloatingPreset.IsChecked = ((MySession.Static.Settings.TrashFlags & myTrashRemovalFlags3) == myTrashRemovalFlags3);
			m_checkboxRevertFloatingPreset.UserData = myTrashRemovalFlags3;
			base.Control.Controls.Add(m_checkboxRevertFloatingPreset);
			base.Control.Controls.Add(control);
		}

		private void OnTrashVoxelButtonClicked(MyGuiControlButton obj)
		{
			MySession.Static.Settings.VoxelTrashRemovalEnabled = !MySession.Static.Settings.VoxelTrashRemovalEnabled;
			if (!MySession.Static.Settings.VoxelTrashRemovalEnabled)
			{
				obj.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_ResumeTrashButton);
			}
			else
			{
				obj.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PauseTrashButton);
			}
			MyGuiScreenAdminMenu.RecalcTrash();
		}

		internal override bool GetSettings(ref MyGuiScreenAdminMenu.AdminSettings settings)
		{
			if (m_textboxVoxelPlayerDistanceTrash == null || m_textboxVoxelGridDistanceTrash == null || m_textboxVoxelAgeTrash == null)
			{
				return false;
			}
			float.TryParse(m_textboxVoxelPlayerDistanceTrash.Text, out float result);
			float.TryParse(m_textboxVoxelGridDistanceTrash.Text, out float result2);
			int.TryParse(m_textboxVoxelAgeTrash.Text, out int result3);
			int num = 0 | ((MySession.Static.Settings.VoxelPlayerDistanceThreshold != result) ? 1 : 0) | ((MySession.Static.Settings.VoxelGridDistanceThreshold != result2) ? 1 : 0) | ((MySession.Static.Settings.VoxelAgeThreshold != result3) ? 1 : 0);
			settings.voxelDistanceFromPlayer = result;
			settings.voxelDistanceFromGrid = result2;
			settings.voxelAge = result3;
			settings.flags = (m_checkboxRevertMaterials.IsChecked ? (settings.flags | (MyTrashRemovalFlags)m_checkboxRevertMaterials.UserData) : (settings.flags & ~(MyTrashRemovalFlags)m_checkboxRevertMaterials.UserData));
			settings.flags = (m_checkboxRevertAsteroids.IsChecked ? (settings.flags | (MyTrashRemovalFlags)m_checkboxRevertAsteroids.UserData) : (settings.flags & ~(MyTrashRemovalFlags)m_checkboxRevertAsteroids.UserData));
			settings.flags = (m_checkboxRevertFloatingPreset.IsChecked ? (settings.flags | (MyTrashRemovalFlags)m_checkboxRevertFloatingPreset.UserData) : (settings.flags & ~(MyTrashRemovalFlags)m_checkboxRevertFloatingPreset.UserData));
			return (byte)(num | ((MySession.Static.Settings.TrashFlags != settings.flags) ? 1 : 0)) != 0;
		}
	}
}
