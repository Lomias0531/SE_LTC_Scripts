using VRage.Game;

namespace Sandbox.Game.EntityComponents
{
	public struct MyResourceSourceInfo
	{
		public MyDefinitionId ResourceTypeId;

		public float DefinedOutput;

		public float ProductionToCapacityMultiplier;

		public bool IsInfiniteCapacity;
	}
}
