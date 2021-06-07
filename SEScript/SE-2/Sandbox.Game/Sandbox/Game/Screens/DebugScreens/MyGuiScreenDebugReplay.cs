using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Game", "Replay")]
	internal class MyGuiScreenDebugReplay : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugReplay()
		{
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugReplay";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.13f);
			AddButton("Record + replay", OnStartRecordingClick);
			AddButton("Stop recording", OnStopRecordingClick);
			AddButton("Replay", OnStartReplayClick);
			AddButton("Clear all", OnClearClick);
			m_currentPosition.Y += 0.01f;
			AddButton("Add new character", OnAddNewCharacterClick);
		}

		private void OnStartRecordingClick(MyGuiControlButton button)
		{
			MySessionComponentReplay.Static.StartRecording();
			MySessionComponentReplay.Static.StartReplay();
		}

		private void OnStopRecordingClick(MyGuiControlButton button)
		{
			MySessionComponentReplay.Static.StopRecording();
		}

		private void OnStartReplayClick(MyGuiControlButton button)
		{
			MySessionComponentReplay.Static.StartReplay();
		}

		private void OnStopReplayClick(MyGuiControlButton button)
		{
			MySessionComponentReplay.Static.StopReplay();
		}

		private void OnAddNewCharacterClick(MyGuiControlButton button)
		{
			MyCharacterInputComponent.SpawnCharacter();
		}

		private void OnClearClick(MyGuiControlButton button)
		{
			MySessionComponentReplay.Static.DeleteRecordings();
		}
	}
}
