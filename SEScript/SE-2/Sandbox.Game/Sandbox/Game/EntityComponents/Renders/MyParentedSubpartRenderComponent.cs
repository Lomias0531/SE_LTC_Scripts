using Sandbox.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents.Renders
{
	public class MyParentedSubpartRenderComponent : MyRenderComponent
	{
		private class Sandbox_Game_EntityComponents_Renders_MyParentedSubpartRenderComponent_003C_003EActor : IActivator, IActivator<MyParentedSubpartRenderComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyParentedSubpartRenderComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyParentedSubpartRenderComponent CreateInstance()
			{
				return new MyParentedSubpartRenderComponent();
			}

			MyParentedSubpartRenderComponent IActivator<MyParentedSubpartRenderComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			MyEntity obj = (MyEntity)base.Entity;
			obj.InvalidateOnMove = false;
			obj.NeedsWorldMatrix = false;
		}

		public override void AddRenderObjects()
		{
			base.AddRenderObjects();
			UpdateParent();
		}

		public void UpdateParent()
		{
			if (GetRenderObjectID() != uint.MaxValue)
			{
				uint num = base.Entity.Parent.Render.ParentIDs[0];
				if (num != uint.MaxValue)
				{
					GetCullObjectRelativeMatrix(out Matrix relativeMatrix);
					SetParent(0, num, relativeMatrix);
					OnParented();
				}
			}
		}

		public void GetCullObjectRelativeMatrix(out Matrix relativeMatrix)
		{
			relativeMatrix = base.Entity.PositionComp.LocalMatrix * base.Entity.Parent.PositionComp.LocalMatrix;
		}

		public virtual void OnParented()
		{
		}
	}
}
