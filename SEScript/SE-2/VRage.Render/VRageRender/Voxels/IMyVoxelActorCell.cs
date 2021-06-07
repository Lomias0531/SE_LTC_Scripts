using VRageMath;

namespace VRageRender.Voxels
{
	/// <summary>
	/// Represents a chunk of voxel mesh in a voxel actor.
	/// </summary>
	public interface IMyVoxelActorCell
	{
		/// <summary>
		/// Position and lod of this voxel cell.
		/// </summary>
		Vector3D Offset
		{
			get;
		}

		/// <summary>
		/// Lod index of this cell.
		/// </summary>
		int Lod
		{
			get;
		}

		/// <summary>
		/// Weather this cell is visible or not.
		/// </summary>
		bool Visible
		{
			get;
		}

		/// <summary>
		/// Update the mesh inside this voxel cell.
		/// </summary>
		/// <param name="data">Render data for the cell.</param>
		void UpdateMesh(ref MyVoxelRenderCellData data);

		/// <summary>
		/// Set the visibility of the cell. False means the cell will not be considered at all during rendering.
		/// </summary>
		/// <param name="visible">Wether the cell should be visible or not.</param>
		/// <param name="notify">Whther to raise the cell change event form the voxel actor after this visibility change is complete.</param>
		void SetVisible(bool visible, bool notify = true);
	}
}
