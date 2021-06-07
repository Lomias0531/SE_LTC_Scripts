using Sandbox.Engine.Multiplayer;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.GameServices;

namespace Sandbox.Game.Screens
{
	internal class MyGuiScreenScenarioMpServer : MyGuiScreenScenarioMpBase
	{
		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
		}

		protected override void OnStartClicked(MyGuiControlButton sender)
		{
			MySession.Static.Settings.CanJoinRunning = false;
			if (!MySession.Static.Settings.CanJoinRunning)
			{
				MyMultiplayer.Static.SetLobbyType(MyLobbyType.Private);
			}
			MyScenarioSystem.Static.PrepareForStart();
			CloseScreen();
		}
	}
}
