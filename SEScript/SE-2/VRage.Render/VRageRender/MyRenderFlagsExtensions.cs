namespace VRageRender
{
	public static class MyRenderFlagsExtensions
	{
		public static bool HasFlags(this RenderFlags renderFlags, RenderFlags flags)
		{
			return (renderFlags & flags) == flags;
		}
	}
}
