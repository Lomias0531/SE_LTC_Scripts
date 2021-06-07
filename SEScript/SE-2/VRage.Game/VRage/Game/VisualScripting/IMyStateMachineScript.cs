namespace VRage.Game.VisualScripting
{
	public interface IMyStateMachineScript
	{
		string TransitionTo
		{
			get;
			set;
		}

		long OwnerId
		{
			get;
			set;
		}

		[VisualScriptingMember(true, false)]
		void Init();

		[VisualScriptingMember(true, false)]
		void Update();

		[VisualScriptingMember(true, false)]
		void Dispose();

		[VisualScriptingMiscData("Self", "Completes the scripts by setting state to completed.", -10510688)]
		[VisualScriptingMember(true, true)]
		void Complete(string transitionName = "Completed");

		[VisualScriptingMember(false, true)]
		long GetOwnerId();

		[VisualScriptingMember(true, false)]
		void Deserialize();
	}
}
