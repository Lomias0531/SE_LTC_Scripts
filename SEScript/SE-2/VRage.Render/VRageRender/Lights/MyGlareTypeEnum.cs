namespace VRageRender.Lights
{
	public enum MyGlareTypeEnum
	{
		/// <summary>
		/// This is the glare that is dependent on occlusion queries.
		/// Physically, this phenomenon originates in the lens.
		/// </summary>
		Normal,
		/// <summary>
		/// Sun
		/// </summary>
		Distant,
		Directional
	}
}
