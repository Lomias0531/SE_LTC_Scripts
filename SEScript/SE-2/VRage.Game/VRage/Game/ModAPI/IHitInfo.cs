using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IHitInfo
	{
		/// <summary>
		/// The position where the raycast hit.
		/// </summary>
		Vector3D Position
		{
			get;
		}

		/// <summary>
		/// The entity that was hit.
		/// </summary>
		IMyEntity HitEntity
		{
			get;
		}

		/// <summary>
		/// The direction vector of the hit surface.
		/// </summary>
		Vector3 Normal
		{
			get;
		}

		/// <summary>
		/// How much of the ray cast distance was traveled before hitting something.
		/// Use this value to multiply your initial distance to get the distance to hit position in a cheaper way.
		/// </summary>
		float Fraction
		{
			get;
		}
	}
}
