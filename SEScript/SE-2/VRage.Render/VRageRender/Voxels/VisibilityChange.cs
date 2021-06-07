using VRageMath;

namespace VRageRender.Voxels
{
	/// <summary>
	/// Signature for the event fired once the visibility of a voxel actor cell changes.
	/// </summary>
	/// <param name="offset">The cell's offset.</param>
	/// <param name="lod">The lod of the cell.</param>
	/// <param name="visible">Whether the cell if visible after the change.</param>
	public delegate void VisibilityChange(Vector3D offset, int lod, bool visible);
}
