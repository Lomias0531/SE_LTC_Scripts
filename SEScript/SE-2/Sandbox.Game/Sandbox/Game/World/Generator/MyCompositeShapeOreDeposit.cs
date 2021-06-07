using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using VRage.Game;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World.Generator
{
	internal class MyCompositeShapeOreDeposit
	{
		public readonly MyCsgShapeBase Shape;

		protected readonly MyVoxelMaterialDefinition m_material;

		public virtual void DebugDraw(ref MatrixD translation, Color materialColor)
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_ORES)
			{
				Shape.DebugDraw(ref translation, materialColor);
				MyRenderProxy.DebugDrawText3D((Matrix.CreateTranslation(Shape.Center()) * translation).Translation, m_material.Id.SubtypeName, Color.White, 1f, depthRead: false);
			}
		}

		public MyCompositeShapeOreDeposit(MyCsgShapeBase shape, MyVoxelMaterialDefinition material)
		{
			Shape = shape;
			m_material = material;
		}

		public virtual MyVoxelMaterialDefinition GetMaterialForPosition(ref Vector3 pos, float lodSize)
		{
			return m_material;
		}
	}
}
