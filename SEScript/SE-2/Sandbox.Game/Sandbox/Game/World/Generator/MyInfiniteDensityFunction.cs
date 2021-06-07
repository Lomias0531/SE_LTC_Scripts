using VRage.Library.Utils;
using VRage.Noise;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	internal class MyInfiniteDensityFunction : IMyAsteroidFieldDensityFunction, IMyModule
	{
		private IMyModule noise;

		public MyInfiniteDensityFunction(MyRandom random, double frequency)
		{
			noise = new MySimplexFast(random.Next(), frequency);
		}

		public bool ExistsInCell(ref BoundingBoxD bbox)
		{
			return true;
		}

		public double GetValue(double x)
		{
			return noise.GetValue(x);
		}

		public double GetValue(double x, double y)
		{
			return noise.GetValue(x, y);
		}

		public double GetValue(double x, double y, double z)
		{
			return noise.GetValue(x, y, z);
		}
	}
}
