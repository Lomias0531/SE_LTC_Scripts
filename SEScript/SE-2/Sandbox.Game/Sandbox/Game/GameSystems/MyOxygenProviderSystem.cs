using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	public static class MyOxygenProviderSystem
	{
		private static List<IMyOxygenProvider> m_oxygenGenerators = new List<IMyOxygenProvider>();

		public static float GetOxygenInPoint(Vector3D worldPoint)
		{
			float num = 0f;
			foreach (IMyOxygenProvider oxygenGenerator in m_oxygenGenerators)
			{
				if (oxygenGenerator.IsPositionInRange(worldPoint))
				{
					num += oxygenGenerator.GetOxygenForPosition(worldPoint);
				}
			}
			return MathHelper.Saturate(num);
		}

		public static void AddOxygenGenerator(IMyOxygenProvider gravityGenerator)
		{
			m_oxygenGenerators.Add(gravityGenerator);
		}

		public static void RemoveOxygenGenerator(IMyOxygenProvider gravityGenerator)
		{
			m_oxygenGenerators.Remove(gravityGenerator);
		}

		public static void ClearOxygenGenerators()
		{
			m_oxygenGenerators.Clear();
		}
	}
}
