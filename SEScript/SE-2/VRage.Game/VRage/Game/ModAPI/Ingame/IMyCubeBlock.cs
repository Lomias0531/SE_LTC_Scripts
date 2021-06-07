using System;
using VRage.ObjectBuilders;
using VRageMath;

namespace VRage.Game.ModAPI.Ingame
{
	/// <summary>
	/// Basic cube interface
	/// </summary>
	public interface IMyCubeBlock : IMyEntity
	{
		SerializableDefinitionId BlockDefinition
		{
			get;
		}

		bool CheckConnectionAllowed
		{
			get;
		}

		/// <summary>
		/// Grid in which the block is placed
		/// </summary>
		IMyCubeGrid CubeGrid
		{
			get;
		}

		/// <summary>
		/// Definition name
		/// </summary>
		string DefinitionDisplayNameText
		{
			get;
		}

		/// <summary>
		/// Is set in definition
		/// Ratio at which is the block disassembled (grinding) 
		/// </summary>
		float DisassembleRatio
		{
			get;
		}

		/// <summary>
		/// Translated block name
		/// </summary>
		string DisplayNameText
		{
			get;
		}

		/// <summary>
		/// Hacking of the block is in progress
		/// </summary>
		bool IsBeingHacked
		{
			get;
		}

		/// <summary>
		/// True if integrity is above breaking threshold
		/// </summary>
		bool IsFunctional
		{
			get;
		}

		/// <summary>
		/// True if block is able to do its work depening on block type (is functional, powered, enabled, etc...)
		/// </summary>
		bool IsWorking
		{
			get;
		}

		/// <summary>
		/// Maximum coordinates of grid cells occupied by this block
		/// </summary>
		Vector3I Max
		{
			get;
		}

		/// <summary>
		/// Block mass
		/// </summary>
		float Mass
		{
			get;
		}

		/// <summary>
		/// Minimum coordinates of grid cells occupied by this block
		/// </summary>
		Vector3I Min
		{
			get;
		}

		/// <summary>
		/// Order in which were the blocks of same type added to grid
		/// Used in default display name
		/// </summary>
		int NumberInGrid
		{
			get;
		}

		/// <summary>
		/// Returns block orientation in base 6 directions
		/// </summary>
		MyBlockOrientation Orientation
		{
			get;
		}

		/// <summary>
		/// Id of player owning block (not steam Id)
		/// </summary>
		long OwnerId
		{
			get;
		}

		/// <summary>
		/// Position in grid coordinates
		/// </summary>
		Vector3I Position
		{
			get;
		}

		/// <summary>
		/// Tag of faction owning block
		/// </summary>
		string GetOwnerFactionTag();

		[Obsolete("GetPlayerRelationToOwner() is useless ingame. Mods should use the one in ModAPI.IMyCubeBlock")]
		MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner();

		MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long playerId);

		[Obsolete]
		void UpdateIsWorking();

		[Obsolete]
		void UpdateVisual();
	}
}
