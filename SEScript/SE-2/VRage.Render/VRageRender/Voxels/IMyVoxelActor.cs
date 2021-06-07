using VRage;
using VRageMath;

namespace VRageRender.Voxels
{
	/// <summary>
	/// Defines a render actor for a voxel entity.
	///
	/// Voxels entities differ from other entities in that they have multi-part meshes,
	/// where each part can be shown at different levels of detail.
	///
	/// These parts are called cells or chunks.
	/// </summary>
	public interface IMyVoxelActor
	{
		/// <summary>
		/// Render Object Id for this actor.
		/// </summary>
		uint Id
		{
			get;
		}

		/// <summary>
		/// Size in voxels of this actor.
		/// </summary>
		Vector3I Size
		{
			get;
			set;
		}

		/// <summary>
		/// Transition mode used for visibility changes to meshes in this clipmap.
		///
		/// This will also affect cell updates.
		/// </summary>
		/// <remarks>
		/// It is an error to change the mode when there is an open batch.
		/// </remarks>
		MyVoxelActorTransitionMode TransitionMode
		{
			get;
			set;
		}

		/// <summary>
		/// Whether this actor is currently batching cell changes.
		/// </summary>
		bool IsBatching
		{
			get;
		}

		/// <summary>
		/// Event fired when a cell is added or removed.
		///
		/// The event is only fired when the operation is done (which could be asynchronous, but not on a different thread as the renderer).
		///
		/// The event is only fired for operations that specify the notify flag = true.
		/// </summary>
		event VisibilityChange CellChange;

		/// <summary>
		/// Event  fired when the actor moves.
		/// </summary>
		event ActionRef<MatrixD> Move;

		/// <summary>
		/// Create a clipmap cell.
		///
		/// Cell add event is fired once the cell is actually visible.
		/// </summary>
		/// <param name="offset">Offset of the mesh.</param>
		/// <param name="lod">Mesh lod, used to calculate scale.</param>
		/// <param name="notify">Whether to raise an event for this operation.</param>
		/// <returns></returns>
		IMyVoxelActorCell CreateCell(Vector3D offset, int lod, bool notify = false);

		/// <summary>
		/// Destroy a voxel cell in this actor.
		///
		/// Once a the cell is no longer visible and the notify flag was set, the CellChange event is raised.
		/// </summary>
		/// <param name="cell">The cell to remove</param>
		/// <param name="notify">Whether to raise an event for this operation.</param>
		void DeleteCell(IMyVoxelActorCell cell, bool notify = false);

		/// <summary>
		/// Begin caching add/remove update operations.
		///
		/// To dispatch them all at once call EndBatch();
		/// </summary>
		/// <param name="switchMode">Optionally switch into this transition mode before beginning the batch.</param>
		void BeginBatch(MyVoxelActorTransitionMode? switchMode = null);

		/// <summary>
		/// Dispatch all queued operations.
		/// </summary>
		/// <seealso cref="M:VRageRender.Voxels.IMyVoxelActor.BeginBatch(System.Nullable{VRageRender.Voxels.MyVoxelActorTransitionMode})" />
		void EndBatch(bool justLoaded);
	}
}
