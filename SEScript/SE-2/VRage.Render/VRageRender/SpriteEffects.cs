namespace VRageRender
{
	/// <summary>
	/// Defines sprite mirroring options.
	/// </summary>
	/// <remarks>
	/// Description is taken from original XNA <a href="http://msdn.microsoft.com/en-us/library/VRageMath.graphics.spriteeffects.aspx">SpriteEffects</a> class.
	/// </remarks>
	public enum SpriteEffects
	{
		/// <summary>
		/// No rotations specified.
		/// </summary>
		None,
		/// <summary>
		/// Rotate 180 degrees around the Y axis before rendering.
		/// </summary>
		FlipHorizontally,
		/// <summary>
		/// Rotate 180 degrees around the X axis before rendering.
		/// </summary>
		FlipVertically,
		/// <summary>
		/// Rotate 180 degress around both the X and Y axis before rendering.
		/// </summary>
		FlipBoth
	}
}
