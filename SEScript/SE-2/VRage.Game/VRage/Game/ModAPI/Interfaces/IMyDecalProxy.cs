using VRage.Utils;

namespace VRage.Game.ModAPI.Interfaces
{
	public interface IMyDecalProxy
	{
		/// <param name="hitInfo">Hithinfo on world coordinates</param>
		/// <param name="source"></param>
		/// <param name="customdata"></param>
		/// <param name="decalHandler"></param>
		/// <param name="material"></param>
		void AddDecals(ref MyHitInfo hitInfo, MyStringHash source, object customdata, IMyDecalHandler decalHandler, MyStringHash material);
	}
}
