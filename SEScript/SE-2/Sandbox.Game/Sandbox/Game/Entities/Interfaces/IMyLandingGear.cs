using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.Game.Entities.Interfaces
{
	public interface IMyLandingGear
	{
		bool AutoLock
		{
			get;
		}

		LandingGearMode LockMode
		{
			get;
		}

		event LockModeChangedHandler LockModeChanged;

		void RequestLock(bool enable);

		void ResetAutolock();

		IMyEntity GetAttachedEntity();
	}
}
