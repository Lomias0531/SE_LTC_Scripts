using VRage.Game;

namespace Sandbox.Game.GameSystems.Conveyors
{
	public class PullInformation
	{
		public MyInventory Inventory
		{
			get;
			set;
		}

		public long OwnerID
		{
			get;
			set;
		}

		public MyInventoryConstraint Constraint
		{
			get;
			set;
		}

		public MyDefinitionId ItemDefinition
		{
			get;
			set;
		}
	}
}
