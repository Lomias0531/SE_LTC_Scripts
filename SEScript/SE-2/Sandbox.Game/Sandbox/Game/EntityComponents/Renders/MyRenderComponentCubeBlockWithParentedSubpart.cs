using Sandbox.Game.Components;
using VRage.Game.Components;
using VRage.Network;

namespace Sandbox.Game.EntityComponents.Renders
{
	public class MyRenderComponentCubeBlockWithParentedSubpart : MyRenderComponentCubeBlock
	{
		private class Sandbox_Game_EntityComponents_Renders_MyRenderComponentCubeBlockWithParentedSubpart_003C_003EActor : IActivator, IActivator<MyRenderComponentCubeBlockWithParentedSubpart>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentCubeBlockWithParentedSubpart();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentCubeBlockWithParentedSubpart CreateInstance()
			{
				return new MyRenderComponentCubeBlockWithParentedSubpart();
			}

			MyRenderComponentCubeBlockWithParentedSubpart IActivator<MyRenderComponentCubeBlockWithParentedSubpart>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override void AddRenderObjects()
		{
			base.AddRenderObjects();
			UpdateChildren();
		}

		protected void UpdateChildren()
		{
			foreach (MyHierarchyComponentBase child in m_cubeBlock.Hierarchy.Children)
			{
				(child.Entity.Render as MyParentedSubpartRenderComponent)?.UpdateParent();
			}
		}
	}
}
