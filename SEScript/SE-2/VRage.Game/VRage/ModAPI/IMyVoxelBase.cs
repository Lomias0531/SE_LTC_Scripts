using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace VRage.ModAPI
{
	public interface IMyVoxelBase : IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
	{
		new IMyStorage Storage
		{
			get;
		}

		Vector3D PositionLeftBottomCorner
		{
			get;
		}

		string StorageName
		{
			get;
		}

		bool IsBoxIntersectingBoundingBoxOfThisVoxelMap(ref BoundingBoxD boundingBox);
	}
}
