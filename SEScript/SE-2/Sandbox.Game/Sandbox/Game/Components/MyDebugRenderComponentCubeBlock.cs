using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Components
{
	public class MyDebugRenderComponentCubeBlock : MyDebugRenderComponent
	{
		private MyCubeBlock m_cubeBlock;

		public MyDebugRenderComponentCubeBlock(MyCubeBlock cubeBlock)
			: base(cubeBlock)
		{
			m_cubeBlock = cubeBlock;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_CUBE_BLOCK_AABBS)
			{
				Color color = Color.Red;
				_ = Color.Green;
				_ = m_cubeBlock.BlockDefinition.Center;
				Vector3 v = m_cubeBlock.Min * m_cubeBlock.CubeGrid.GridSize - new Vector3(m_cubeBlock.CubeGrid.GridSize / 2f);
				Vector3 v2 = m_cubeBlock.Max * m_cubeBlock.CubeGrid.GridSize + new Vector3(m_cubeBlock.CubeGrid.GridSize / 2f);
				BoundingBoxD localbox = new BoundingBoxD(v, v2);
				MatrixD worldMatrix = m_cubeBlock.CubeGrid.WorldMatrix;
				MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, MySimpleObjectRasterizer.Wireframe, 1, 0.01f);
			}
		}
	}
}
