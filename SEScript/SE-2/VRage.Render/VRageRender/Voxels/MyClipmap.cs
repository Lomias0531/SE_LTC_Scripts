using VRageMath;

namespace VRageRender.Voxels
{
	public static class MyClipmap
	{
		/// <summary>
		/// Control whether clipmaps should be updated.
		/// </summary>
		public static bool EnableUpdate = true;

		/// <summary>
		/// Control whether clipmap debug data is to be drawn on the screen.
		/// </summary>
		public static bool EnableDebugDraw;

		/// <summary>
		/// Count of clipmap lods.
		/// </summary>
		public const int LodCount = 16;

		/// <summary>
		/// Mapping of lod index to it's preferred debug color.
		/// </summary>
		public static readonly Vector4[] LodColors = new Vector4[16]
		{
			new Vector4(1f, 0f, 0f, 1f),
			new Vector4(0f, 1f, 0f, 1f),
			new Vector4(0f, 0f, 1f, 1f),
			new Vector4(1f, 1f, 0f, 1f),
			new Vector4(0f, 1f, 1f, 1f),
			new Vector4(1f, 0f, 1f, 1f),
			new Vector4(0.5f, 0f, 1f, 1f),
			new Vector4(0.5f, 1f, 0f, 1f),
			new Vector4(1f, 0f, 0.5f, 1f),
			new Vector4(0f, 1f, 0.5f, 1f),
			new Vector4(1f, 0.5f, 0f, 1f),
			new Vector4(0f, 0.5f, 1f, 1f),
			new Vector4(0.5f, 1f, 1f, 1f),
			new Vector4(1f, 0.5f, 1f, 1f),
			new Vector4(1f, 1f, 0.5f, 1f),
			new Vector4(0.5f, 0.5f, 1f, 1f)
		};
	}
}
