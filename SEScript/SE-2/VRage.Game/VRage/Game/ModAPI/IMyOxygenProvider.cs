using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMyOxygenProvider
	{
		float GetOxygenForPosition(Vector3D worldPoint);

		bool IsPositionInRange(Vector3D worldPoint);
	}
}
