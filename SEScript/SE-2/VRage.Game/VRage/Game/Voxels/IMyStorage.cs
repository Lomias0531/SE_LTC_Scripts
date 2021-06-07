using System;
using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;

namespace VRage.Game.Voxels
{
	public interface IMyStorage : VRage.ModAPI.IMyStorage
	{
		/// <summary>
		/// Unique identifier for this storage.
		///
		/// This identifier is unique for any storage available locally.
		/// No assumptions can be made by equal storages on different clients
		/// or the same storage across multiple reloads of  the game.
		/// </summary>
		uint StorageId
		{
			get;
		}

		/// <summary>
		/// Weather this storage is shared by multiple voxel entities.
		///
		/// Shared storages may not be closed directly.
		/// </summary>
		bool Shared
		{
			get;
		}

		/// <summary>
		/// The procedural provider of data for this storage, can be null.
		/// </summary>
		IMyStorageDataProvider DataProvider
		{
			get;
		}

		/// <summary>
		/// Returns true if storage compressed data are cached
		/// </summary>
		bool AreCompressedDataCached
		{
			get;
		}

		/// <summary>
		/// Please use RangeChanged on voxelbase if possible
		/// </summary>
		event Action<Vector3I, Vector3I, MyStorageDataTypeFlags> RangeChanged;

		/// <summary>
		/// Close this storage, unloading it's resources.
		/// </summary>
		void Close();

		/// <summary>
		/// Create a copy of this storage.
		/// </summary>
		/// <returns></returns>
		IMyStorage Copy();

		/// <summary>
		/// Pin the storage.
		///
		/// While the storage is pinned it will not be closed, but calls to pin will mark it to close as soon as all pins are disposed.
		///
		/// The pin might not be valid if the storage was already closed. You should always check once you pin the storage that the pin is valid.
		/// </summary>
		/// <returns>A storage pin. Must be disposed when no longer in use.</returns>
		StoragePin Pin();

		/// <summary>
		/// Unpins the storage, must not be called directly, used by storage pins.
		/// </summary>
		void Unpin();

		/// <summary>
		/// Check for intersection against storage space bounding box.
		///
		/// When <b>lazy</b> is set to true this method can return <b>intersects</b> when the box is actually <b>contained</b>.
		/// </summary>
		/// <param name="box">Query box</param>
		/// <param name="lod"></param>
		/// <param name="exhaustiveContainmentCheck"></param>
		/// <returns>Weather the bounding box is disjoint, intersectiong or contained in the storage volume.</returns>
		ContainmentType Intersect(ref BoundingBoxI box, int lod, bool exhaustiveContainmentCheck = true);

		/// <summary>
		/// Wather a given storage space line intersects the contents of the storage.
		///
		/// If so this method returns an aproximation for the shortest interval of the line that contains all itersections.
		/// </summary>
		/// <param name="line">The queried line.</param>
		/// <returns>Wether there is an intersection.</returns>
		new bool Intersect(ref LineD line);

		/// <summary>
		/// Debug draw the storage at the give position orientation and scale.
		/// </summary>
		/// <param name="worldMatrix">World transform for the debug draw.</param>
		/// <param name="mode">Debug draw mode.</param>
		void DebugDraw(ref MatrixD worldMatrix, MyVoxelDebugDrawMode mode);

		/// <summary>
		/// Sets cache for compressed storage data
		/// </summary>
		/// <param name="data"></param>
		void SetCompressedDataCache(byte[] data);

		/// <summary>
		/// Gets uncompressed voxel data
		/// </summary>
		/// <returns>Uncompressed voxel data</returns>
		byte[] GetVoxelData();

		/// <summary>
		/// Notify to the storage the change of a range of storage data.
		/// </summary>
		/// <remarks>
		/// This method is designed to be used when <see cref="!:VRage.ModAPI.IMyStorage.WriteRange(VRage.Voxels.MyStorageData,VRage.Voxels.MyStorageDataTypeFlags,VRageMath.Vector3I,VRageMath.Vector3I,bool)" /> is invoked with <code>notify = false</code>.
		/// Reducing the number of responses to the RangeChanged event if a large set of changes is issued.
		/// </remarks>
		/// <param name="voxelRangeMin">Minimum change coordinate (inclusive).</param>
		/// <param name="voxelRangeMax">Maximum changed coordinate (inclusive).</param>
		/// <param name="changedData">What types of data have been modified.</param>
		void NotifyChanged(Vector3I voxelRangeMin, Vector3I voxelRangeMax, MyStorageDataTypeFlags changedData);
	}
}
