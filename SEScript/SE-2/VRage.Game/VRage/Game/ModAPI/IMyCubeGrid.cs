using System;
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMyCubeGrid : VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.Game.ModAPI.Ingame.IMyCubeGrid
	{
		/// <summary>
		/// List of players with majority of blocks on grid
		/// </summary>
		List<long> BigOwners
		{
			get;
		}

		/// <summary>
		/// List of players with any blocks on grid
		/// </summary>
		List<long> SmallOwners
		{
			get;
		}

		/// <summary>
		/// Gets or sets if this grid is a respawn grid (can be cleaned up automatically when player leaves)
		/// </summary>
		bool IsRespawnGrid
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets if the grid is static (station)
		/// </summary>
		/// <remarks>Be careful not to set it on stations which are embedded in voxels!</remarks>
		new bool IsStatic
		{
			get;
			set;
		}

		/// <summary>
		/// Display name of the grid (as seen in Info terminal tab)
		/// </summary>
		new string CustomName
		{
			get;
			set;
		}

		/// <summary>
		/// X-Axis build symmetry plane
		/// </summary>
		Vector3I? XSymmetryPlane
		{
			get;
			set;
		}

		/// <summary>
		/// Y-Axis build symmetry plane
		/// </summary>
		Vector3I? YSymmetryPlane
		{
			get;
			set;
		}

		/// <summary>
		/// Z-Axis build symmetry plane
		/// </summary>
		Vector3I? ZSymmetryPlane
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets if the symmetry plane is offset from the block center
		/// </summary>
		/// <remarks>True if symmetry plane is at block border; false if center of block</remarks>
		bool XSymmetryOdd
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets if the symmetry plane is offset from the block center
		/// </summary>
		/// <remarks>True if symmetry plane is at block border; false if center of block</remarks>
		bool YSymmetryOdd
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets if the symmetry plane is offset from the block center
		/// </summary>
		/// <remarks>True if symmetry plane is at block border; false if center of block</remarks>
		bool ZSymmetryOdd
		{
			get;
			set;
		}

		/// <summary>
		/// Called when a block is added to the grid
		/// </summary>
		event Action<IMySlimBlock> OnBlockAdded;

		/// <summary>
		/// Called when a block is removed from the grid
		/// </summary>
		event Action<IMySlimBlock> OnBlockRemoved;

		/// <summary>
		/// Called when a block on the grid changes owner
		/// </summary>
		event Action<IMyCubeGrid> OnBlockOwnershipChanged;

		/// <summary>
		/// Called when a grid is taken control of by a player
		/// </summary>
		event Action<IMyCubeGrid> OnGridChanged;

		/// <summary>
		/// Triggered when grid is split
		/// </summary>
		event Action<IMyCubeGrid, IMyCubeGrid> OnGridSplit;

		/// <summary>
		/// Triggered when grid changes to or from static (station)
		/// </summary>
		event Action<IMyCubeGrid, bool> OnIsStaticChanged;

		/// <summary>
		/// Triggered when block integrity changes (construction)
		/// </summary>
		event Action<IMySlimBlock> OnBlockIntegrityChanged;

		/// <summary>
		/// Applies random deformation to given block
		/// </summary>
		/// <param name="block">block to be deformed</param>
		void ApplyDestructionDeformation(IMySlimBlock block);

		/// <summary>
		/// Changes owner of all blocks on grid
		/// Call only on server!
		/// </summary>
		/// <param name="playerId">new owner id</param>
		/// <param name="shareMode">new share mode</param>
		void ChangeGridOwnership(long playerId, MyOwnershipShareModeEnum shareMode);

		/// <summary>
		/// Clears symmetry planes
		/// </summary>
		void ClearSymmetries();

		/// <summary>
		/// Sets given color mask to range of blocks
		/// </summary>
		/// <param name="min">Starting coordinates of collored area</param>
		/// <param name="max">End coordinates of collored area</param>
		/// <param name="newHSV">new color mask (Saturation and Value are offsets)</param>
		void ColorBlocks(Vector3I min, Vector3I max, Vector3 newHSV);

		/// <summary>
		/// Sets given skin to range of blocks
		/// </summary>
		/// <param name="min">Starting coordinates of skinned area</param>
		/// <param name="max">End coordinates of skinned area</param>
		/// <param name="newHSV">new color mask (Saturation and Value are offsets)</param>
		/// <param name="newSkin">subtype of the new skin</param>
		void SkinBlocks(Vector3I min, Vector3I max, Vector3? newHSV, string newSkin);

		/// <summary>
		/// Converts station to ship
		/// </summary>
		[Obsolete("Use IMyCubeGrid.Static instead.")]
		void OnConvertToDynamic();

		/// <summary>
		/// Clamps fractional grid position to nearest cell (prefers neighboring occupied cell before empty) 
		/// </summary>
		/// <param name="cube">Return value</param>
		/// <param name="fractionalGridPosition">Fractional position in grid space</param>
		void FixTargetCube(out Vector3I cube, Vector3 fractionalGridPosition);

		/// <summary>
		/// Gets position of closest cell corner
		/// </summary>
		/// <param name="gridPos">Cell coordinates</param>
		/// <param name="position">Position to find nearest corner to. Grid space</param>
		/// <returns>Fractional position of corner in grid space</returns>
		Vector3 GetClosestCorner(Vector3I gridPos, Vector3 position);

		/// <summary>
		/// Get cube block at given position
		/// </summary>
		/// <param name="pos">Block position</param>
		/// <returns>Block or null if none is present at given position</returns>
		new IMySlimBlock GetCubeBlock(Vector3I pos);

		/// <summary>
		/// Returns point of intersection with line
		/// </summary>
		/// <param name="line">Intersecting line</param>
		/// <param name="distance">Distance of intersection</param>
		/// <param name="intersectedBlock"></param>
		/// <returns>Point of intersection</returns>
		Vector3D? GetLineIntersectionExactAll(ref LineD line, out double distance, out IMySlimBlock intersectedBlock);

		/// <summary>
		/// Same as GetLineIntersectionExactAll just without intersected block
		/// </summary>
		bool GetLineIntersectionExactGrid(ref LineD line, ref Vector3I position, ref double distanceSquared);

		/// <summary>
		/// Finds out if given area has any neighboring block
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		bool IsTouchingAnyNeighbor(Vector3I min, Vector3I max);

		/// <summary>
		/// Determines if merge between grids is possible with given offset
		/// </summary>
		/// <param name="gridToMerge"></param>
		/// <param name="gridOffset">offset to merged grid (in grid space)</param>
		/// <returns></returns>
		bool CanMergeCubes(IMyCubeGrid gridToMerge, Vector3I gridOffset);

		/// <summary>
		/// Transformation matrix that has to be applied to grid blocks to correctly merge it
		/// used because ie. ships can be turned 90 degrees along X axis when being merged
		/// </summary>
		/// <param name="gridToMerge"></param>
		/// <param name="gridOffset"></param>
		/// <returns></returns>
		MatrixI CalculateMergeTransform(IMyCubeGrid gridToMerge, Vector3I gridOffset);

		/// <summary>
		/// Merge used by merge blocks
		/// </summary>
		/// <param name="gridToMerge"></param>
		/// <param name="gridOffset"></param>
		/// <returns></returns>
		IMyCubeGrid MergeGrid_MergeBlock(IMyCubeGrid gridToMerge, Vector3I gridOffset);

		/// <summary>
		/// Returns cell with block intersecting given line
		/// </summary>
		/// <param name="worldStart"></param>
		/// <param name="worldEnd"></param>
		/// <returns></returns>
		Vector3I? RayCastBlocks(Vector3D worldStart, Vector3D worldEnd);

		/// <summary>
		/// Returns list of cells with blocks intersected by line
		/// </summary>
		/// <param name="worldStart"></param>
		/// <param name="worldEnd"></param>
		/// <param name="outHitPositions"></param>
		/// <param name="gridSizeInflate"></param>
		/// <param name="havokWorld">use physics intersection</param>
		void RayCastCells(Vector3D worldStart, Vector3D worldEnd, List<Vector3I> outHitPositions, Vector3I? gridSizeInflate = null, bool havokWorld = false);

		/// <summary>
		/// Remove block at given position
		/// </summary>
		void RazeBlock(Vector3I position);

		/// <summary>
		/// Remove blocks in given area
		/// </summary>
		/// <param name="pos">Starting position</param>
		/// <param name="size">Area extents</param>
		void RazeBlocks(ref Vector3I pos, ref Vector3UByte size);

		/// <summary>
		/// Remove blocks at given positions
		/// </summary>
		void RazeBlocks(List<Vector3I> locations);

		/// <summary>
		/// Removes given block
		/// </summary>
		/// <param name="block"></param>
		/// <param name="updatePhysics">Update grid physics</param>
		void RemoveBlock(IMySlimBlock block, bool updatePhysics = false);

		/// <summary>
		/// Removes block and deformates neighboring blocks
		/// </summary>
		/// <param name="block"></param>
		void RemoveDestroyedBlock(IMySlimBlock block);

		/// <summary>
		/// Refreshes block neighbors (checks connections)
		/// </summary>
		/// <param name="block"></param>
		void UpdateBlockNeighbours(IMySlimBlock block);

		/// <summary>
		/// Converts world coordinates to grid space cell coordinates
		/// </summary>
		/// <param name="coords"></param>
		/// <returns></returns>
		new Vector3I WorldToGridInteger(Vector3D coords);

		/// <summary>
		/// Returns blocks in grid
		/// </summary>
		/// <param name="blocks">List of returned blocks</param>
		/// <param name="collect">Filter - function called on each block telling if it should be added to result</param>
		void GetBlocks(List<IMySlimBlock> blocks, Func<IMySlimBlock, bool> collect = null);

		/// <summary>
		/// Returns blocks inside given sphere (world space)
		/// </summary>
		List<IMySlimBlock> GetBlocksInsideSphere(ref BoundingSphereD sphere);

		void UpdateOwnership(long ownerId, bool isFunctional);

		/// <summary>
		/// Add a cubeblock to the grid
		/// </summary>
		/// <param name="objectBuilder">Object builder of cube to add</param>
		/// <param name="testMerge">test for grid merging</param>
		/// <returns></returns>
		IMySlimBlock AddBlock(MyObjectBuilder_CubeBlock objectBuilder, bool testMerge);

		/// <summary>
		/// Checks if removing a block will cause the grid to split
		/// </summary>
		/// <param name="testBlock"></param>
		/// <returns></returns>
		bool WillRemoveBlockSplitGrid(IMySlimBlock testBlock);

		/// <summary>
		/// Tests if a cubeblock can be added at the specific location
		/// </summary>
		/// <param name="pos"></param>
		/// <returns><b>true</b> if block can be added</returns>
		bool CanAddCube(Vector3I pos);

		/// <summary>
		/// Test if the range of positions are not occupied by any blocks
		/// </summary>
		/// <param name="min">Start position</param>
		/// <param name="max">End position</param>
		/// <returns><b>true</b> if blocks can be added in that range</returns>
		bool CanAddCubes(Vector3I min, Vector3I max);

		/// <summary>
		/// Split grid along a plane
		/// </summary>
		/// <param name="plane"></param>
		/// <returns></returns>
		IMyCubeGrid SplitByPlane(PlaneD plane);

		/// <summary>
		/// Split grid
		/// </summary>
		/// <param name="blocks">List of blocks to split into new grid</param>
		/// <param name="sync">Pass <b>true</b> if on server to sync this to clients.</param>
		/// <returns>New grid</returns>
		/// <remarks>To sync to clients, this must be called on the server with sync = true.</remarks>
		IMyCubeGrid Split(List<IMySlimBlock> blocks, bool sync = true);

		/// <summary>
		/// Determines whether this grid is in the same logical group as the other, meaning they're connected
		/// either mechanically or via blocks like connectors. Be aware that using merge blocks combines grids into one, so this function
		/// will not filter out grids connected that way.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		bool IsInSameLogicalGroupAs(IMyCubeGrid other);

		/// <summary>
		/// <para>
		/// Determines whether this grid is mechanically connected to the other. This is any grid connected
		/// with rotors or pistons or other mechanical devices, but not things like connectors. This will in most
		/// cases constitute your complete construct.
		/// </para>
		/// <para>
		/// Be aware that using merge blocks combines grids into one, so this function will not filter out grids
		/// connected that way. Also be aware that detaching the heads of pistons and rotors will cause this
		/// connection to change.
		/// </para>
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		bool IsSameConstructAs(IMyCubeGrid other);

		bool IsRoomAtPositionAirtight(Vector3I vector3I);
	}
}
