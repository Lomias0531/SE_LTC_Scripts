namespace VRageRender.Textures
{
	/// <summary>
	/// Reresent loading quality for textures.
	/// This works only for dds textures with mipmaps. Other textures will retains their original properties.
	/// </summary>
	public enum TextureQuality
	{
		/// <summary>
		/// Full quality.
		/// </summary>
		Full,
		/// <summary>
		/// 1/2 quality.
		/// </summary>
		Half,
		/// <summary>
		/// 1/4 quality
		/// </summary>
		OneFourth,
		/// <summary>
		/// 1/8 quality
		/// </summary>
		OneEighth,
		/// <summary>
		/// 1/16 quality
		/// </summary>
		OneSixteenth
	}
}
