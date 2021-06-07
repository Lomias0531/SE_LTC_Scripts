using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	public interface IMyReactor : IMyPowerProducer, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		bool UseConveyorSystem
		{
			get;
			set;
		}
	}
}
