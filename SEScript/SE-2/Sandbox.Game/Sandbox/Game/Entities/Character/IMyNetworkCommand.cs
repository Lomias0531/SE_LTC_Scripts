namespace Sandbox.Game.Entities.Character
{
	internal interface IMyNetworkCommand
	{
		bool ExecuteBeforeMoveAndRotate
		{
			get;
		}

		void Apply();
	}
}
