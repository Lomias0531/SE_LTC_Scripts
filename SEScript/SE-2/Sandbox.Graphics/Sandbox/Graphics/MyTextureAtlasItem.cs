using VRageMath;

namespace Sandbox.Graphics
{
	public struct MyTextureAtlasItem
	{
		public string AtlasTexture;

		public Vector4 UVOffsets;

		public MyTextureAtlasItem(string atlasTex, Vector4 uvOffsets)
		{
			AtlasTexture = atlasTex;
			UVOffsets = uvOffsets;
		}
	}
}
