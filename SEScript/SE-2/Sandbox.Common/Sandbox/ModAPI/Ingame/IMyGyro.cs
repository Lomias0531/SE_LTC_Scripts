using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	public interface IMyGyro : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		float GyroPower
		{
			get;
			set;
		}

		bool GyroOverride
		{
			get;
			set;
		}

		float Yaw
		{
			get;
			set;
		}

		float Pitch
		{
			get;
			set;
		}

		float Roll
		{
			get;
			set;
		}
	}
}
