using System.Collections.Generic;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ModAPI.Ingame
{
	/// <summary>
	/// basic block interface
	/// </summary>
	public interface IMySlimBlock
	{
		/// <summary>
		/// Block definition ID
		/// </summary>
		SerializableDefinitionId BlockDefinition
		{
			get;
		}

		/// <summary>
		/// Current accumlated damage, pending application
		/// </summary>
		float AccumulatedDamage
		{
			get;
		}

		/// <summary>
		/// Build integrity (of components)
		/// </summary>
		float BuildIntegrity
		{
			get;
		}

		/// <summary>
		/// Ratio of BuildIntegrity and MaxIntegrity
		/// </summary>
		float BuildLevelRatio
		{
			get;
		}

		/// <summary>
		/// BuildIntegrity - Integrity
		/// </summary>
		float CurrentDamage
		{
			get;
		}

		/// <summary>
		///
		/// </summary>
		float DamageRatio
		{
			get;
		}

		/// <summary>
		/// Gets the fatblock if there is one
		/// </summary>
		IMyCubeBlock FatBlock
		{
			get;
		}

		/// <summary>
		/// If this block is deformed (bones deformed)
		/// </summary>
		bool HasDeformation
		{
			get;
		}

		/// <summary>
		/// Gets if component stack is empty
		/// </summary>
		bool IsDestroyed
		{
			get;
		}

		/// <summary>
		/// Integrity is at maximum
		/// </summary>
		bool IsFullIntegrity
		{
			get;
		}

		/// <summary>
		/// Gets if component stack is empty
		/// </summary>
		bool IsFullyDismounted
		{
			get;
		}

		/// <summary>
		/// Maximum deformation of block
		/// </summary>
		float MaxDeformation
		{
			get;
		}

		/// <summary>
		/// The maximum integrity of block
		/// </summary>
		float MaxIntegrity
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
		/// Fatblock owner, if present; otherwise grid owner
		/// </summary>
		long OwnerId
		{
			get;
		}

		/// <summary>
		/// Gets if sub parts are shown
		/// </summary>
		bool ShowParts
		{
			get;
		}

		/// <summary>
		/// A component stockpile has been allocated
		/// </summary>
		bool StockpileAllocated
		{
			get;
		}

		/// <summary>
		/// The component stockpile is empty (no build components)
		/// </summary>
		bool StockpileEmpty
		{
			get;
		}

		/// <summary>
		/// Grid relative position of block
		/// </summary>
		Vector3I Position
		{
			get;
		}

		/// <summary>
		/// Gets the grid the slimblock is on
		/// </summary>
		IMyCubeGrid CubeGrid
		{
			get;
		}

		/// <summary>
		/// Gets the color of the block
		/// </summary>
		Vector3 ColorMaskHSV
		{
			get;
		}

		/// <summary>
		/// Gets the skin of the block
		/// </summary>
		MyStringHash SkinSubtypeId
		{
			get;
		}

		/// <summary>
		/// Gets the list of missing components for this block
		/// </summary>
		/// <param name="addToDictionary"></param>
		void GetMissingComponents(Dictionary<string, int> addToDictionary);
	}
}
