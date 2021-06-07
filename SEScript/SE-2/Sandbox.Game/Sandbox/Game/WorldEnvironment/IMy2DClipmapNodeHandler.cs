using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
	public interface IMy2DClipmapNodeHandler
	{
		void Init(IMy2DClipmapManager parent, int x, int y, int lod, ref BoundingBox2D bounds);

		void Close();

		void InitJoin(IMy2DClipmapNodeHandler[] children);

		unsafe void Split(BoundingBox2D* childBoxes, ref IMy2DClipmapNodeHandler[] children);
	}
}
