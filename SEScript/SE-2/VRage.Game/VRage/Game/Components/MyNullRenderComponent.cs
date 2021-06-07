using VRage.Network;
using VRageMath;

namespace VRage.Game.Components
{
	public class MyNullRenderComponent : MyRenderComponentBase
	{
		private class VRage_Game_Components_MyNullRenderComponent_003C_003EActor : IActivator, IActivator<MyNullRenderComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyNullRenderComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyNullRenderComponent CreateInstance()
			{
				return new MyNullRenderComponent();
			}

			MyNullRenderComponent IActivator<MyNullRenderComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override object ModelStorage
		{
			get;
			set;
		}

		public override void SetRenderObjectID(int index, uint ID)
		{
		}

		public override void ReleaseRenderObjectID(int index)
		{
		}

		public override void AddRenderObjects()
		{
		}

		public override void Draw()
		{
		}

		public override bool IsVisible()
		{
			return false;
		}

		protected override bool CanBeAddedToRender()
		{
			return false;
		}

		public override void InvalidateRenderObjects()
		{
		}

		public override void RemoveRenderObjects()
		{
		}

		public override void UpdateRenderEntity(Vector3 colorMaskHSV)
		{
		}

		protected override void UpdateRenderObjectVisibility(bool visible)
		{
		}
	}
}
