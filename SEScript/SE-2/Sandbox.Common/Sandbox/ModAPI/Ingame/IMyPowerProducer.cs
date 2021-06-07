using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	public interface IMyPowerProducer : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Current output in Megawatts
		/// </summary>
		float CurrentOutput
		{
			get;
		}

		/// <summary>
		/// Maximum output in Megawatts
		/// </summary>
		float MaxOutput
		{
			get;
		}
	}
}
