using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	public interface IMyOreDetector : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		float Range
		{
			get;
		}

		bool BroadcastUsingAntennas
		{
			get;
			set;
		}
	}
}
