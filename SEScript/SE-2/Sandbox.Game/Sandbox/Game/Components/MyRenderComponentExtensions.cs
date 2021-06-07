using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using System;
using VRage.Game.Components;
using VRage.Game.Models;
using VRageMath;

namespace Sandbox.Game.Components
{
	public static class MyRenderComponentExtensions
	{
		public unsafe static void CalculateBlockDepthBias(this MyRenderComponent renderComponent, MyCubeBlock block)
		{
			if (block.Hierarchy == null || block.Hierarchy.Parent == null)
			{
				return;
			}
			MyCompoundCubeBlock myCompoundCubeBlock = block.Hierarchy.Parent.Entity as MyCompoundCubeBlock;
			if (myCompoundCubeBlock == null)
			{
				return;
			}
			bool* ptr = stackalloc bool[64];
			foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
			{
				if (block2.FatBlock != null && block2.FatBlock != block)
				{
					MyRenderComponentBase render = block2.FatBlock.Render;
					if (render != null)
					{
						ptr[(int)render.DepthBias] = true;
					}
				}
			}
			int num = 0;
			MyModel myModel = renderComponent.ModelStorage as MyModel;
			if (myModel != null)
			{
				Vector3 position = myModel.BoundingSphere.Center;
				MatrixI matrix = new MatrixI(block.SlimBlock.Orientation);
				Vector3 result = default(Vector3);
				Vector3.Transform(ref position, ref matrix, out result);
				if (result.LengthSquared() > 0.5f)
				{
					num = ((Math.Abs(result.X) > Math.Abs(result.Y)) ? ((!(Math.Abs(result.X) > Math.Abs(result.Z))) ? ((result.Z > 0f) ? 10 : 12) : ((result.X > 0f) ? 2 : 4)) : ((!(Math.Abs(result.Z) > Math.Abs(result.Y))) ? ((result.Y > 0f) ? 6 : 8) : ((result.Z > 0f) ? 10 : 12)));
				}
			}
			int num2 = num;
			while (true)
			{
				if (num2 < 64)
				{
					if (!ptr[num2])
					{
						break;
					}
					num2++;
					continue;
				}
				return;
			}
			renderComponent.DepthBias = (byte)num2;
		}
	}
}
