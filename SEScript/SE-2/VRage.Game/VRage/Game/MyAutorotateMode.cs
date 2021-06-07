namespace VRage.Game
{
	public enum MyAutorotateMode
	{
		/// <summary>
		/// When block has mount points only on one side, it will autorotate so that side is touching the surface.
		/// Otherwise, full range of rotations is allowed.
		/// </summary>
		OneDirection,
		/// <summary>
		/// When block has mount points only on two sides and those sides are opposite each other (eg. Top and Bottom),
		/// it will autorotate so that one of these sides is touching the surface. Otherwise, full range of rotations
		/// is allowed.
		/// </summary>
		OppositeDirections,
		/// <summary>
		/// When block has mountpoint on at least one side, it will autorotate so that this side is touching the surface.
		/// Otherwise, full range of rotations is allowed.
		/// </summary>
		FirstDirection
	}
}
