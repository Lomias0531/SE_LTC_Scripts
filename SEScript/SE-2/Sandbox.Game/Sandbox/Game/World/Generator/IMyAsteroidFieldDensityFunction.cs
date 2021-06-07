using VRage.Noise;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public interface IMyAsteroidFieldDensityFunction : IMyModule
	{
		bool ExistsInCell(ref BoundingBoxD bbox);
	}
}
