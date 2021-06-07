using VRage.Utils;

namespace VRage.Game.ModAPI.Interfaces
{
	public interface IMyDestroyableObject
	{
		/// <summary>
		/// Gets the integrity (health) of the object
		/// </summary>
		float Integrity
		{
			get;
		}

		/// <summary>
		/// When set to true, it should use MyDamageSystem damage routing.
		/// </summary>
		bool UseDamageSystem
		{
			get;
		}

		void OnDestroy();

		/// <summary>
		/// Applies damage to an object
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="damageSource"></param>
		/// <param name="sync"></param>
		/// <param name="hitInfo"></param>
		/// <param name="attackerId"></param>
		/// <returns></returns>
		bool DoDamage(float damage, MyStringHash damageSource, bool sync, MyHitInfo? hitInfo = null, long attackerId = 0L);
	}
}
