using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Antenna block interface
	/// </summary>
	public interface IMyRadioAntenna : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Broadcasting/Receiving range
		/// </summary>
		float Radius
		{
			get;
			set;
		}

		/// <summary>
		/// Show shipname on hud
		/// </summary>
		bool ShowShipName
		{
			get;
			set;
		}

		/// <summary>
		/// Returns true if antenna is broadcasting
		/// </summary>
		bool IsBroadcasting
		{
			get;
		}

		/// <summary>
		/// Gets or sets if broadcasting is enabled
		/// </summary>
		bool EnableBroadcasting
		{
			get;
			set;
		}

		/// <summary>
		/// The text displayed in the spawn menu
		/// </summary>
		string HudText
		{
			get;
			set;
		}
	}
}
