using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;

namespace VRage.Game.ModAPI
{
	public interface IMyFloatingObject : VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyDestroyableObject
	{
	}
}
