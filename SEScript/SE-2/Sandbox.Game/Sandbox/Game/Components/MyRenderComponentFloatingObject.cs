using Sandbox.Game.Entities;
using VRage.Network;
using VRage.Utils;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentFloatingObject : MyRenderComponent
	{
		private class Sandbox_Game_Components_MyRenderComponentFloatingObject_003C_003EActor : IActivator, IActivator<MyRenderComponentFloatingObject>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentFloatingObject();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentFloatingObject CreateInstance()
			{
				return new MyRenderComponentFloatingObject();
			}

			MyRenderComponentFloatingObject IActivator<MyRenderComponentFloatingObject>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyFloatingObject m_floatingObject;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_floatingObject = (base.Container.Entity as MyFloatingObject);
		}

		public override void AddRenderObjects()
		{
			if (m_floatingObject.VoxelMaterial == null)
			{
				base.AddRenderObjects();
			}
			else if (m_renderObjectIDs[0] == uint.MaxValue)
			{
				SetRenderObjectID(0, MyRenderProxy.CreateRenderVoxelDebris("Voxel debris", base.Model.AssetName, base.Container.Entity.PositionComp.WorldMatrix, 5f, 8f, MyUtils.GetRandomFloat(0f, 2f), m_floatingObject.VoxelMaterial.Index, FadeIn));
			}
		}
	}
}
