namespace VRage.Game.ObjectBuilders.Definitions.SessionComponents
{
	public struct MyPlacementSettings
	{
		public MyGridPlacementSettings SmallGrid;

		public MyGridPlacementSettings SmallStaticGrid;

		public MyGridPlacementSettings LargeGrid;

		public MyGridPlacementSettings LargeStaticGrid;

		/// <summary>
		/// Align static grids to corners (false) or centers (true).
		/// You should always set to corners in new games. Center alignment is only for backwards compatibility so that
		/// static grids are correctly aligned with already existing saves.
		/// </summary>
		public bool StaticGridAlignToCenter;

		public MyGridPlacementSettings GetGridPlacementSettings(MyCubeSize cubeSize, bool isStatic)
		{
			switch (cubeSize)
			{
			case MyCubeSize.Large:
				if (!isStatic)
				{
					return LargeGrid;
				}
				return LargeStaticGrid;
			case MyCubeSize.Small:
				if (!isStatic)
				{
					return SmallGrid;
				}
				return SmallStaticGrid;
			default:
				return LargeGrid;
			}
		}

		public MyGridPlacementSettings GetGridPlacementSettings(MyCubeSize cubeSize)
		{
			switch (cubeSize)
			{
			case MyCubeSize.Large:
				return LargeGrid;
			case MyCubeSize.Small:
				return SmallGrid;
			default:
				return LargeGrid;
			}
		}
	}
}
