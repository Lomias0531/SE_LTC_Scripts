using System;
using System.Collections.Generic;
using VRageMath;

namespace VRageRender.Voxels
{
	/// <summary>
	/// Interface describing a lod controller for a voxel object.
	///
	/// The lod controller is a class responsible for managing the visibility state of voxel objects and their parts.
	/// </summary>
	public interface IMyLodController
	{
		/// <summary>
		/// Provide an enumeration of all voxel cells the controller deems visible.
		/// </summary>
		IEnumerable<IMyVoxelActorCell> Cells
		{
			get;
		}

		/// <summary>
		/// Get the actor bound to this clipmap if any.
		///
		/// This reference may not be thread safe.
		/// </summary>
		IMyVoxelActor Actor
		{
			get;
		}

		/// <summary>
		/// The size in voxels of this voxel object.
		/// </summary>
		Vector3I Size
		{
			get;
		}

		IMyVoxelRenderDataProcessorProvider VoxelRenderDataProcessorProvider
		{
			get;
			set;
		}

		float? SpherizeRadius
		{
			get;
		}

		Vector3D SpherizePosition
		{
			get;
		}

		/// <summary>
		/// Event raised whenever the lod controller finishes updating all cells ton the current view.
		/// </summary>
		event Action<IMyLodController> Loaded;

		/// <summary>
		/// Update cell visibility based on a view frustum.
		///
		/// Cells visibility should be updated based on the view frustum.
		/// </summary>
		///              <param name="view">View matrix relative to the clipmap.</param>
		///              <param name="viewFrustum">The view frustum for the current camera.</param>
		///              <param name="farClipping">Distance to the far clipping plane from the view matrix.</param>
		void Update(ref MatrixD view, BoundingFrustumD viewFrustum, float farClipping);

		/// <summary>
		/// Bind to the cell handler, which is the component responsible for creating cells on behalf of the clipmap.
		/// </summary>
		///
		/// It should only be legal to provide the cell handler once to a clipmap. Any future calls should be met with <see cref="T:System.InvalidOperationException" />
		/// <param name="actor">The cell handler to use for this clipmap.</param>
		void BindToActor(IMyVoxelActor actor);

		/// <summary>
		/// Unload all data for this clipmap, this is only invoked at shutdown.
		/// </summary>
		void Unload();

		/// <summary>
		/// Let the clipmap know the provided range needs to be recalculated.
		/// </summary>
		/// <param name="min">min range in voxels</param>
		/// <param name="max">max range in voxels</param>
		void InvalidateRange(Vector3I min, Vector3I max);

		/// <summary>
		/// Invalidate all data in the clipmap.
		///
		/// This should not trigger recalculation immediately, that should happen upon the next update.
		/// </summary>
		void InvalidateAll();

		/// <summary>
		/// Debug draw the contents of the clipmap.
		///
		/// For debug use only.
		/// </summary>
		///             <param name="cameraMatrix"></param>
		void DebugDraw(ref MatrixD cameraMatrix);
	}
}
