using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	public class MyOxygenProviderSystemHelper : IMyOxygenProviderSystem
	{
		float IMyOxygenProviderSystem.GetOxygenInPoint(Vector3D worldPoint)
		{
			return MyOxygenProviderSystem.GetOxygenInPoint(worldPoint);
		}

		void IMyOxygenProviderSystem.AddOxygenGenerator(IMyOxygenProvider provider)
		{
			MyOxygenProviderSystem.AddOxygenGenerator(provider);
		}

		void IMyOxygenProviderSystem.RemoveOxygenGenerator(IMyOxygenProvider provider)
		{
			MyOxygenProviderSystem.RemoveOxygenGenerator(provider);
		}
	}
}
