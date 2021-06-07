using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
	public class MyGuiScreenScenarioWaitForPlayers : MyGuiScreenBase
	{
		private MyGuiControlLabel m_timeOutLabel;

		private MyGuiControlButton m_leaveButton;

		private StringBuilder m_tmpStringBuilder = new StringBuilder();

		public MyGuiScreenScenarioWaitForPlayers()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			base.Size = new Vector2(800f, 330f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			base.CloseButtonEnabled = false;
			m_closeOnEsc = false;
			RecreateControls(constructor: true);
			base.CanHideOthers = false;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyStringId.GetOrCompute("Waiting for other players"));
			MyGuiControlLabel control = new MyGuiControlLabel(null, null, "Game will start when all players join the world", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_timeOutLabel = new MyGuiControlLabel(null, null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_leaveButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, text: new StringBuilder("Leave"), size: new Vector2(190f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, toolTip: null, textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OnLeaveClicked);
			MyLayoutTable myLayoutTable = new MyLayoutTable(this);
			myLayoutTable.SetColumnWidths(60f, 680f, 60f);
			myLayoutTable.SetRowHeights(110f, 65f, 65f, 65f, 65f, 65f);
			myLayoutTable.Add(control, MyAlignH.Center, MyAlignV.Center, 1, 1);
			myLayoutTable.Add(m_timeOutLabel, MyAlignH.Center, MyAlignV.Center, 2, 1);
			myLayoutTable.Add(m_leaveButton, MyAlignH.Center, MyAlignV.Center, 3, 1);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenBattleWaitingConnectedPlayers";
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape))
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.MainMenu));
			}
		}

		public override bool Update(bool hasFocus)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(0.0);
			_ = MyScenarioSystem.Static.ServerPreparationStartTime;
			timeSpan = DateTime.UtcNow - MyScenarioSystem.Static.ServerPreparationStartTime;
			timeSpan = TimeSpan.FromSeconds(MyScenarioSystem.LoadTimeout) - timeSpan;
			if (timeSpan.TotalMilliseconds < 0.0)
			{
				timeSpan = TimeSpan.FromSeconds(0.0);
			}
			string value = timeSpan.ToString("mm\\:ss");
			m_tmpStringBuilder.Clear().Append("Timeout: ").Append(value);
			m_timeOutLabel.Text = m_tmpStringBuilder.ToString();
			return base.Update(hasFocus);
		}

		private void OnLeaveClicked(MyGuiControlButton sender)
		{
			CloseScreen();
			MySessionLoader.UnloadAndExitToMenu();
		}
	}
}
