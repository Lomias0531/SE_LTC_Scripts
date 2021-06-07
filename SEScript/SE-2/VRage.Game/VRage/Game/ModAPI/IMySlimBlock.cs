using System;
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMySlimBlock : VRage.Game.ModAPI.Ingame.IMySlimBlock, IMyDestroyableObject, IMyDecalProxy
	{
		/// <summary>
		/// Gets the fatblock if there is one
		/// </summary>
		new IMyCubeBlock FatBlock
		{
			get;
		}

		/// <summary>
		/// Gets the grid the slimblock is on
		/// </summary>
		new IMyCubeGrid CubeGrid
		{
			get;
		}

		/// <summary>
		/// The blocks definition (cast to MyCubeBlockDefinition)
		/// </summary>
		new MyDefinitionBase BlockDefinition
		{
			get;
		}

		/// <summary>
		/// Largest part of block
		/// </summary>
		Vector3I Max
		{
			get;
		}

		/// <summary>
		/// Min position in the grid
		/// </summary>
		Vector3I Min
		{
			get;
		}

		/// <summary>
		/// Blocks orientation
		/// </summary>
		MyBlockOrientation Orientation
		{
			get;
		}

		/// <summary>
		/// The blocks that neighbour this block
		/// </summary>
		List<IMySlimBlock> Neighbours
		{
			get;
		}

		/// <summary>
		/// Sets the transparency of the block.
		/// </summary>
		/// <remarks>Not synced.</remarks>
		float Dithering
		{
			get;
			set;
		}

		/// <summary>
		/// Identity ID of the builder of this block.
		/// </summary>
		long BuiltBy
		{
			get;
		}

		void AddNeighbours();

		void ApplyAccumulatedDamage(bool addDirtyParts = true);

		string CalculateCurrentModel(out Matrix orientation);

		/// <summary>
		/// Gets the block center as a Vector3D, relative to grid WorldMatrix
		/// </summary>
		/// <param name="scaledCenter"></param>
		void ComputeScaledCenter(out Vector3D scaledCenter);

		/// <summary>
		/// Gets the half extents for the block
		/// </summary>
		/// <param name="scaledHalfExtents"></param>
		void ComputeScaledHalfExtents(out Vector3 scaledHalfExtents);

		/// <summary>
		/// Gets the world position for the center of the block
		/// </summary>
		/// <param name="worldCenter"></param>
		void ComputeWorldCenter(out Vector3D worldCenter);

		/// <summary>
		/// Repair block deformation
		/// </summary>
		/// <param name="oldDamage"></param>
		/// <param name="maxAllowedBoneMovement"></param>
		void FixBones(float oldDamage, float maxAllowedBoneMovement);

		/// <summary>
		/// Reset built level to 0
		/// </summary>
		/// <param name="outputInventory"></param>
		void FullyDismount(IMyInventory outputInventory);

		[Obsolete("GetCopyObjectBuilder() is deprecated. Call GetObjectBuilder(bool) and pass 'true'.")]
		MyObjectBuilder_CubeBlock GetCopyObjectBuilder();

		/// <summary>
		/// Gets the object builder for the slimblock
		/// </summary>
		/// <param name="copy"><b>true</b> to return a copy</param>
		/// <returns></returns>
		MyObjectBuilder_CubeBlock GetObjectBuilder(bool copy = false);

		void InitOrientation(ref Vector3I forward, ref Vector3I up);

		void InitOrientation(Base6Directions.Direction Forward, Base6Directions.Direction Up);

		void InitOrientation(MyBlockOrientation orientation);

		/// <summary>
		/// Transfer contruction components from inventory to stockpile
		/// </summary>
		/// <param name="toInventory"></param>
		/// <param name="flags"></param>
		void MoveItemsFromConstructionStockpile(IMyInventory toInventory, MyItemFlags flags = MyItemFlags.None);

		void RemoveNeighbours();

		void SetToConstructionSite();

		void SpawnConstructionStockpile();

		/// <summary>
		/// Adds the first component to the stockpile
		/// </summary>
		void SpawnFirstItemInConstructionStockpile();

		void UpdateVisual();

		Vector3 GetColorMask();

		/// ---
		/// <summary>
		/// Decreases the build level of a block
		/// </summary>
		/// <param name="grinderAmount">The integrity amount of change</param>
		/// <param name="outputInventory">The inventory where output components will be sent to</param>
		/// <param name="useDefaultDeconstructEfficiency"></param>
		void DecreaseMountLevel(float grinderAmount, IMyInventory outputInventory, bool useDefaultDeconstructEfficiency = false);

		/// <summary>
		/// Increases the build level of a block
		/// </summary>
		/// <param name="welderMountAmount">The integrity amount of change</param>
		/// <param name="welderOwnerPlayerId">The player id of the entity increasing the mount level</param>
		/// <param name="outputInventory">The inventory where components are taken from</param>
		/// <param name="maxAllowedBoneMovement">Maximum movement of bones</param>
		/// <param name="isHelping">Is this increase helping another player</param>
		/// <param name="share">ShareMode used when block becomes functional</param>
		void IncreaseMountLevel(float welderMountAmount, long welderOwnerPlayerId, IMyInventory outputInventory = null, float maxAllowedBoneMovement = 0f, bool isHelping = false, MyOwnershipShareModeEnum share = MyOwnershipShareModeEnum.Faction);

		/// <summary>
		/// Get the amount of items in the construction stockpile
		/// </summary>
		/// <param name="id">Definition of component in stockpile to check</param>
		/// <returns>Amount of components in the stockpile of this type</returns>
		int GetConstructionStockpileItemAmount(MyDefinitionId id);

		/// <summary>
		/// Move items missing from an inventory into the construction stockpile
		/// </summary>
		/// <param name="fromInventory">The inventory where the components are being taken from</param>
		void MoveItemsToConstructionStockpile(IMyInventory fromInventory);

		/// <summary>
		/// Clears out the construction stockpile and moves the components into a destination inventory
		/// </summary>
		/// <param name="outputInventory">The inventory where the components are moved into</param>
		void ClearConstructionStockpile(IMyInventory outputInventory);

		/// <summary>
		/// Play the construction sound associated with the integrity change
		/// </summary>
		/// <param name="integrityChangeType">Type of integrity change</param>
		/// <param name="deconstruction">Is this deconstruction?</param>
		void PlayConstructionSound(MyIntegrityChangeEnum integrityChangeType, bool deconstruction = false);

		/// <summary>
		/// Can we continue to weld this block?
		/// </summary>
		/// <param name="sourceInventory">Source inventory that is used for components</param>
		/// <returns></returns>
		bool CanContinueBuild(IMyInventory sourceInventory);

		/// <summary>
		/// The AABB of this block
		/// </summary>
		/// <param name="aabb"></param>
		/// <param name="useAABBFromBlockCubes"></param>
		void GetWorldBoundingBox(out BoundingBoxD aabb, bool useAABBFromBlockCubes = false);
	}
}
