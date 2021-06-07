using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	internal class MyBoxOreDeposit : MyCompositeShapeOreDeposit
	{
		private MyCsgBox m_boxShape;

		public MyBoxOreDeposit(MyCsgShapeBase baseShape, MyVoxelMaterialDefinition material)
			: base(baseShape, material)
		{
			m_boxShape = (MyCsgBox)baseShape;
		}

		public override MyVoxelMaterialDefinition GetMaterialForPosition(ref Vector3 pos, float lodSize)
		{
			List<MyVoxelMaterialDefinition> list = MyDefinitionManager.Static.GetVoxelMaterialDefinitions().ToList();
			float num = 2f * m_boxShape.HalfExtents;
			int index = (int)(MathHelper.Clamp((pos - m_boxShape.Center() + m_boxShape.HalfExtents).X / num, 0f, 1f) * (float)(list.Count - 1));
			return list[index];
		}

		public override void DebugDraw(ref MatrixD translation, Color materialColor)
		{
		}
	}
}
