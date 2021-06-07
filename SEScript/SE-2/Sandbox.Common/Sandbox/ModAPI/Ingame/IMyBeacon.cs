using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Beacon block interface
	/// </summary>
	public interface IMyBeacon : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Broadcasting range (read-only)
		/// </summary>
		float Radius
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the text to display on the HUD when the beacon is on.
		/// </summary>
		string HudText
		{
			get;
			set;
		}
	}
}
