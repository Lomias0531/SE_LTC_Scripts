using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens
{
	internal class MyGuiScreenScenarioMpClient : MyGuiScreenScenarioMpBase
	{
		public void MySyncScenario_InfoAnswer(bool gameAlreadyRunning, bool canJoinGame)
		{
			if (canJoinGame)
			{
				m_startButton.Enabled = gameAlreadyRunning;
			}
			else
			{
				MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.GuiScenarioCannotJoinCaption), messageText: MyTexts.Get(MySpaceTexts.GuiScenarioCannotJoin), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate
				{
					Canceling();
				}, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
			}
		}

		public MyGuiScreenScenarioMpClient()
		{
			m_startButton.Enabled = false;
			MySyncScenario.InfoAnswer += MySyncScenario_InfoAnswer;
			MySyncScenario.AskInfo();
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_canJoinRunning.Enabled = false;
		}

		protected override void OnStartClicked(MyGuiControlButton sender)
		{
			MySyncScenario.OnPrepareScenarioFromLobby(-1L);
			CloseScreen();
		}

		protected override void OnClosed()
		{
			MySyncScenario.InfoAnswer -= MySyncScenario_InfoAnswer;
			base.OnClosed();
		}
	}
}
