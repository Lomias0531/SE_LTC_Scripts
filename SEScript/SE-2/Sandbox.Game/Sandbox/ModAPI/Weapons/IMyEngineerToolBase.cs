using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI.Weapons
{
	public interface IMyEngineerToolBase : VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyHandheldGunObject<MyToolBase>, IMyGunObject<MyToolBase>
	{
	}
}
