using VRage.Game;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public interface IMyCompositeDeposit : IMyCompositeShape
	{
		MyVoxelMaterialDefinition GetMaterialForPosition(ref Vector3 localPos, float lodVoxelSize);
	}
}
