using Sandbox.Engine.Utils;
using Sandbox.Game.AI;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenBotSettings : MyGuiScreenBase
	{
		public override string GetFriendlyName()
		{
			return "MyGuiScreenBotSettings";
		}

		public MyGuiScreenBotSettings()
		{
			base.Size = new Vector2(650f, 350f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			base.BackgroundColor = MyGuiConstants.SCREEN_BACKGROUND_COLOR;
			RecreateControls(constructor: true);
			base.CanHideOthers = false;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_position = new Vector2(MyGuiManager.GetMaxMouseCoord().X - 0.25f, 0.5f);
			MyLayoutVertical myLayoutVertical = new MyLayoutVertical(this, 35f);
			myLayoutVertical.Advance(20f);
			myLayoutVertical.Add(new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.BotSettingsScreen_Title)), MyAlignH.Center);
			myLayoutVertical.Advance(30f);
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(null, null, null, MyDebugDrawSettings.DEBUG_DRAW_BOTS);
			myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(enableDebuggingCheckBox_IsCheckedChanged));
			myLayoutVertical.Add(new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.BotSettingsScreen_EnableBotsDebugging)), MyAlignH.Left, advance: false);
			myLayoutVertical.Add(myGuiControlCheckbox, MyAlignH.Right);
			myLayoutVertical.Advance(15f);
			MyGuiControlButton leftControl = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.BotSettingsScreen_NextBot), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, nextButton_OnButtonClick);
			MyGuiControlButton rightControl = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.BotSettingsScreen_PreviousBot), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, previousButton_OnButtonClick);
			myLayoutVertical.Add(leftControl, rightControl);
			myLayoutVertical.Advance(30f);
			myLayoutVertical.Add(new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Close), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCloseClicked), MyAlignH.Center);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
		}

		private void enableDebuggingCheckBox_IsCheckedChanged(MyGuiControlCheckbox checkBox)
		{
			MyDebugDrawSettings.DEBUG_DRAW_BOTS = checkBox.IsChecked;
		}

		private void nextButton_OnButtonClick(MyGuiControlButton button)
		{
			MyAIComponent.Static.DebugSelectNextBot();
		}

		private void previousButton_OnButtonClick(MyGuiControlButton button)
		{
			MyAIComponent.Static.DebugSelectPreviousBot();
		}

		private void OnCloseClicked(MyGuiControlButton button)
		{
			CloseScreen();
		}
	}
}
