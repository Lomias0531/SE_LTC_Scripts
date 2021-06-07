using System;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Laser antenna block interface
	/// </summary>
	public interface IMyLaserAntenna : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Determines whether this particular antenna requires line of sight to function.
		/// </summary>
		bool RequireLoS
		{
			get;
		}

		/// <summary>
		/// get target coordinates
		/// </summary>
		Vector3D TargetCoords
		{
			get;
		}

		/// <summary>
		/// Connection is permanent
		/// </summary>
		bool IsPermanent
		{
			get;
			set;
		}

		/// <summary>
		/// Target is outside movement limits of antenna
		/// </summary>
		[Obsolete("Check the Status property instead.")]
		bool IsOutsideLimits
		{
			get;
		}

		/// <summary>
		/// Gets the current status of this antenna.
		/// </summary>
		MyLaserAntennaStatus Status
		{
			get;
		}

		/// <summary>
		/// Gets or sets the max range of the laser set in terminal
		/// </summary>
		float Range
		{
			get;
			set;
		}

		/// <summary>
		/// Set coordinates of target
		/// </summary>
		/// <param name="coords">GPS coordinates string</param>
		void SetTargetCoords(string coords);

		/// <summary>
		/// Connect to target defined by SetTargetCoords
		/// </summary>
		void Connect();
	}
}
