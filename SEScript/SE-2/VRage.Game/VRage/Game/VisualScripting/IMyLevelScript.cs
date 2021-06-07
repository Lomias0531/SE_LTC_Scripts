namespace VRage.Game.VisualScripting
{
	public interface IMyLevelScript
	{
		[VisualScriptingMember(true, false)]
		void Dispose();

		[VisualScriptingMember(true, false)]
		void Update();

		[VisualScriptingMember(true, false)]
		void GameStarted();

		[VisualScriptingMember(true, false)]
		void GameFinished();
	}
}
