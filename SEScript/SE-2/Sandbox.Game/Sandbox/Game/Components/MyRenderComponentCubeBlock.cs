using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using System;
using VRage.Network;

namespace Sandbox.Game.Components
{
	public class MyRenderComponentCubeBlock : MyRenderComponent
	{
		private class Sandbox_Game_Components_MyRenderComponentCubeBlock_003C_003EActor : IActivator, IActivator<MyRenderComponentCubeBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentCubeBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentCubeBlock CreateInstance()
			{
				return new MyRenderComponentCubeBlock();
			}

			MyRenderComponentCubeBlock IActivator<MyRenderComponentCubeBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected MyCubeBlock m_cubeBlock;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_cubeBlock = (base.Container.Entity as MyCubeBlock);
			NeedsDraw = false;
			base.NeedsDrawFromParent = false;
			NeedForDrawFromParentChanged = (Action)Delegate.Combine(NeedForDrawFromParentChanged, new Action(OnNeedForDrawFromParentChanged));
		}

		public override void InvalidateRenderObjects()
		{
			m_cubeBlock.InvalidateOnMove = false;
		}

		public override void AddRenderObjects()
		{
			this.CalculateBlockDepthBias(m_cubeBlock);
			base.AddRenderObjects();
			UpdateGridParent();
		}

		protected void UpdateGridParent()
		{
			if (MyFakes.MANUAL_CULL_OBJECTS)
			{
				MyCubeGridRenderCell orAddCell = m_cubeBlock.CubeGrid.RenderData.GetOrAddCell(m_cubeBlock.Position * m_cubeBlock.CubeGrid.GridSize);
				if (orAddCell.ParentCullObject == uint.MaxValue)
				{
					orAddCell.RebuildInstanceParts(GetRenderFlags());
				}
				for (int i = 0; i < m_renderObjectIDs.Length; i++)
				{
					SetParent(i, orAddCell.ParentCullObject, base.Entity.PositionComp.LocalMatrix);
				}
			}
		}

		private void OnNeedForDrawFromParentChanged()
		{
			if (m_cubeBlock.SlimBlock != null && m_cubeBlock.CubeGrid != null && m_cubeBlock.CubeGrid.BlocksForDraw.Contains(m_cubeBlock) != base.NeedsDrawFromParent)
			{
				if (base.NeedsDrawFromParent)
				{
					m_cubeBlock.CubeGrid.BlocksForDraw.Add(m_cubeBlock);
				}
				else
				{
					m_cubeBlock.CubeGrid.BlocksForDraw.Remove(m_cubeBlock);
				}
				m_cubeBlock.Render.SetVisibilityUpdates(base.NeedsDrawFromParent);
				m_cubeBlock.CubeGrid.MarkForDraw();
			}
		}
	}
}
