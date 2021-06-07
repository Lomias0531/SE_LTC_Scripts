using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMyOxygenProviderSystem
	{
		float GetOxygenInPoint(Vector3D worldPoint);

		void AddOxygenGenerator(IMyOxygenProvider provider);

		void RemoveOxygenGenerator(IMyOxygenProvider provider);
	}
}
