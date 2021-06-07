using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// base block interface, block can be affected by upgrade modules, and you can retrieve upgrade list from <see cref="!:IMyUpgradableBlock" />
	/// </summary>
	public interface IMyCubeBlock : VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity
	{
		/// <summary>
		/// Whether the grid should call the ConnectionAllowed method for this block 
		///             (ConnectionAllowed checks mount points and other per-block requirements)
		/// </summary>
		new bool CheckConnectionAllowed
		{
			get;
			set;
		}

		/// <summary>
		/// Grid in which the block is placed
		/// </summary>
		new IMyCubeGrid CubeGrid
		{
			get;
		}

		/// <summary>
		/// Resource sink (draws power)
		/// </summary>
		/// <remarks>Cast to MyResourceSinkComponent as needed.</remarks>
		MyResourceSinkComponentBase ResourceSink
		{
			get;
			set;
		}

		/// <summary>
		/// Get all values changed by upgrade modules
		/// Should only be used as read-only
		/// </summary>
		Dictionary<string, float> UpgradeValues
		{
			get;
		}

		/// <summary>
		/// Gets the SlimBlock associated with this block
		/// </summary>
		IMySlimBlock SlimBlock
		{
			get;
		}

		event Action<IMyCubeBlock> IsWorkingChanged;

		/// <summary>
		/// Event called when upgrade values are changed
		/// Either upgrades were built or destroyed, or they become damaged or unpowered
		/// </summary>
		event Action OnUpgradeValuesChanged;

		/// <summary>
		///
		/// </summary>
		/// <param name="localMatrix"></param>
		/// <param name="currModel"></param>
		void CalcLocalMatrix(out Matrix localMatrix, out string currModel);

		/// <summary>
		/// Calculates model currently used by block depending on its build progress and other factors
		/// </summary>
		/// <param name="orientation">Model orientation</param>
		/// <returns>Model path</returns>
		string CalculateCurrentModel(out Matrix orientation);

		/// <summary>
		/// Debug only method. Effects may wary through time.
		/// </summary>
		/// <returns></returns>
		new bool DebugDraw();

		/// <summary>
		/// Returns block object builder which can be serialized or added to grid
		/// </summary>
		/// <param name="copy">Set if creating a copy of block</param>
		/// <returns>Block object builder</returns>
		MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false);

		/// <summary>
		/// Relation of local player to the block
		/// </summary>
		/// <returns></returns>
		new MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner();

		/// <summary>
		///
		/// </summary>
		/// <param name="playerId">Id of player to check relation with (not steam id!)</param>
		/// <returns>Relation of defined player to the block</returns>
		new MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long playerId);

		/// <summary>
		/// Reloads block model and interactive objects (doors, terminals, etc...)
		/// </summary>
		void Init();

		/// <summary>
		/// Initializes block state from object builder
		/// </summary>
		/// <param name="builder">Object builder of block (should correspond with block type)</param>
		/// <param name="cubeGrid">Owning grid</param>
		void Init(MyObjectBuilder_CubeBlock builder, IMyCubeGrid cubeGrid);

		/// <summary>
		/// Method called when a block has been built (after adding to the grid).
		/// This is called right after placing the block and it doesn't matter whether
		/// it is fully built (creative mode) or is only construction site.
		/// Note that it is not called for blocks which do not create FatBlock at that moment.
		/// </summary>
		void OnBuildSuccess(long builtBy);

		void OnBuildSuccess(long builtBy, bool instantBuild);

		/// <summary>
		/// Called when block is destroyed before being removed from grid
		/// </summary>
		void OnDestroy();

		/// <summary>
		/// Called when the model referred by the block is changed
		/// </summary>
		void OnModelChange();

		/// <summary>
		/// Called at the end of registration from grid systems (after block has been registered).
		/// </summary>
		void OnRegisteredToGridSystems();

		/// <summary>
		/// Method called when user removes a cube block from grid. Useful when block
		/// has to remove some other attached block (like motors).
		/// </summary>
		void OnRemovedByCubeBuilder();

		/// <summary>
		/// Called at the end of unregistration from grid systems (after block has been unregistered).
		/// </summary>
		void OnUnregisteredFromGridSystems();

		/// <summary>
		/// Gets the name of interactive object intersected by defined line
		/// </summary>
		/// <param name="worldFrom">Line from point in world coordinates</param>
		/// <param name="worldTo">Line to point in world coordinates</param>
		/// <returns>Name of intersected detector (interactive object)</returns>
		string RaycastDetectors(Vector3D worldFrom, Vector3D worldTo);

		/// <summary>
		/// Reloads detectors (interactive objects) in model
		/// </summary>
		/// <param name="refreshNetworks">ie conweyor network</param>
		void ReloadDetectors(bool refreshNetworks = true);

		/// <summary>
		/// Force refresh working state. Call if you change block state that could affect its working status.
		/// </summary>
		new void UpdateIsWorking();

		/// <summary>
		/// Updates block visuals (ie. block emissivity)
		/// </summary>
		new void UpdateVisual();

		/// <summary>
		/// Start or stop dammage effect on cube block
		/// </summary>
		void SetDamageEffect(bool start);

		/// <summary>
		/// Activate block effect listed in definition
		/// </summary>
		/// <param name="effectName"></param>
		/// <param name="stopPrevious"></param>
		/// <returns><b>true</b> if effect was started; <b>false</b> otherwise</returns>
		bool SetEffect(string effectName, bool stopPrevious = false);

		/// <summary>
		/// Activate block effect with parameters listed in definition
		/// </summary>
		/// <param name="effectName"></param>
		/// <param name="parameter"></param>
		/// <param name="stopPrevious"></param>
		/// <param name="ignoreParameter"></param>
		/// <param name="removeSameNameEffects"></param>
		/// <returns><b>true</b> if effect was started; <b>false</b> otherwise</returns>
		bool SetEffect(string effectName, float parameter, bool stopPrevious = false, bool ignoreParameter = false, bool removeSameNameEffects = false);

		/// <summary>
		/// Removes active effect set with SetEffect
		/// </summary>
		/// <param name="effectName"></param>
		/// <param name="exception"></param>
		/// <returns>The number of effects removed</returns>
		int RemoveEffect(string effectName, int exception = -1);

		/// <summary>
		/// Preferred way of registering a block for upgrades
		/// Adding directly to the dictionary can have unintended consequences
		/// when multiple mods are involved.
		/// </summary>
		void AddUpgradeValue(string upgrade, float defaultValue);
	}
}
