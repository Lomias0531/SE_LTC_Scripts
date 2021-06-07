namespace VRage.Game
{
	/// <summary>
	/// Enumeration defining where to get the weapon transform from.
	/// This does not include behavior of arms (anim/ik), which is driven separately by variables SimulateLeftHand and SimulateRightHand.
	/// </summary>
	public enum MyItemPositioningEnum
	{
		/// <summary>
		/// Weapon is placed according to sbc data file.
		/// </summary>
		TransformFromData,
		/// <summary>
		/// Weapon is placed according to animation.
		/// </summary>
		TransformFromAnim
	}
}
