using VRage.Game.Components;
using VRage.Network;

namespace Sandbox.Game.Components
{
	internal class MyFracturePiecePositionComponent : MyPositionComponent
	{
		private class Sandbox_Game_Components_MyFracturePiecePositionComponent_003C_003EActor : IActivator, IActivator<MyFracturePiecePositionComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyFracturePiecePositionComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFracturePiecePositionComponent CreateInstance()
			{
				return new MyFracturePiecePositionComponent();
			}

			MyFracturePiecePositionComponent IActivator<MyFracturePiecePositionComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void UpdateChildren(object source, bool forceUpdateAllChildren)
		{
		}

		protected override void OnWorldPositionChanged(object source, bool updateChildren, bool forceUpdateAllChildren)
		{
			m_worldVolumeDirty = true;
			m_worldAABBDirty = true;
			m_normalizedInvMatrixDirty = true;
			m_invScaledMatrixDirty = true;
			if (base.Entity.Physics != null && base.Entity.Physics.Enabled && base.Entity.Physics != source)
			{
				base.Entity.Physics.OnWorldPositionChanged(source);
			}
			if (base.Container.Entity.Render != null)
			{
				base.Container.Entity.Render.InvalidateRenderObjects();
			}
		}
	}
}
