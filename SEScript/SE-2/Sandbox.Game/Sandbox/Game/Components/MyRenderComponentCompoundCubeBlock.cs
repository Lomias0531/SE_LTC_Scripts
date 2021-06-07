using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentCompoundCubeBlock : MyRenderComponentCubeBlock
	{
		private class Sandbox_Game_Components_MyRenderComponentCompoundCubeBlock_003C_003EActor : IActivator, IActivator<MyRenderComponentCompoundCubeBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentCompoundCubeBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentCompoundCubeBlock CreateInstance()
			{
				return new MyRenderComponentCompoundCubeBlock();
			}

			MyRenderComponentCompoundCubeBlock IActivator<MyRenderComponentCompoundCubeBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override void InvalidateRenderObjects()
		{
			base.InvalidateRenderObjects();
			foreach (MySlimBlock block in (m_cubeBlock as MyCompoundCubeBlock).GetBlocks())
			{
				if (block.FatBlock != null && (block.FatBlock.Render.Visible || block.FatBlock.Render.CastShadows) && block.FatBlock.InScene && block.FatBlock.InvalidateOnMove)
				{
					uint[] renderObjectIDs = block.FatBlock.Render.RenderObjectIDs;
					foreach (uint id in renderObjectIDs)
					{
						MatrixD worldMatrix = block.FatBlock.WorldMatrix;
						MyRenderProxy.UpdateRenderObject(id, worldMatrix);
					}
				}
			}
		}

		public override void AddRenderObjects()
		{
			InvalidateRenderObjects();
		}
	}
}
