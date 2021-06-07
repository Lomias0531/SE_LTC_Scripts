using VRage.ModAPI;

namespace VRage.Game.ModAPI
{
	public interface IMyCubeBuilder
	{
		/// <summary>
		///  Returns state of building mode
		/// </summary>
		bool BlockCreationIsActivated
		{
			get;
		}

		/// <summary>
		/// Freezes the built object preview in current position
		/// </summary>
		bool FreezeGizmo
		{
			get;
			set;
		}

		/// <summary>
		/// Shows the delete area preview
		/// </summary>
		bool ShowRemoveGizmo
		{
			get;
			set;
		}

		/// <summary>
		/// Enables synmetry block placing
		/// </summary>
		bool UseSymmetry
		{
			get;
			set;
		}

		/// <summary>
		///
		/// </summary>
		bool UseTransparency
		{
			get;
			set;
		}

		/// <summary>
		/// Is any mode active
		/// </summary>
		bool IsActivated
		{
			get;
		}

		/// <summary>
		/// Activates the building mode
		/// </summary>
		void Activate(MyDefinitionId? blockDefinitionId = null);

		/// <summary>
		/// Adds construction site of block with currently selected definition
		/// </summary>
		/// <param name="buildingEntity"></param>
		bool AddConstruction(IMyEntity buildingEntity);

		/// <summary>
		/// Deactivates all modes
		/// </summary>
		void Deactivate();

		/// <summary>
		/// Deactivates building mode
		/// </summary>
		void DeactivateBlockCreation();

		/// <summary>
		/// Creates new grid 
		/// </summary>
		/// <param name="cubeSize">Grid size</param>
		/// <param name="isStatic">Station = static</param>
		void StartNewGridPlacement(MyCubeSize cubeSize, bool isStatic);

		/// <summary>
		/// Finds grid to build on
		/// </summary>
		/// <returns>found grid</returns>
		IMyCubeGrid FindClosestGrid();
	}
}
