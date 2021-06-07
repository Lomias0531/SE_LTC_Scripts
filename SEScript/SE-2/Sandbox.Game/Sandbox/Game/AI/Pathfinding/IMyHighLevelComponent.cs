namespace Sandbox.Game.AI.Pathfinding
{
	public interface IMyHighLevelComponent
	{
		bool FullyExplored
		{
			get;
		}

		bool Contains(MyNavigationPrimitive primitive);
	}
}
