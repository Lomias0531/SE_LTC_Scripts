namespace VRage.Game.ObjectBuilders.Definitions.SessionComponents
{
	public struct MyGridPlacementSettings
	{
		public SnapMode SnapMode;

		public float SearchHalfExtentsDeltaRatio;

		public float SearchHalfExtentsDeltaAbsolute;

		public VoxelPlacementSettings? VoxelPlacement;

		/// <summary>
		/// When min. allowed penetration is not met, block may still be placed if it is touching static grid and this property is true.
		/// </summary>
		public bool CanAnchorToStaticGrid;

		public bool EnablePreciseRotationWhenSnapped;
	}
}
