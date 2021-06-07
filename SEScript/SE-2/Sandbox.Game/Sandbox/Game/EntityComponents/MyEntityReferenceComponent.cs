using VRage.Game.Components;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
	public class MyEntityReferenceComponent : MyEntityComponentBase
	{
		private class Sandbox_Game_EntityComponents_MyEntityReferenceComponent_003C_003EActor : IActivator, IActivator<MyEntityReferenceComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityReferenceComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityReferenceComponent CreateInstance()
			{
				return new MyEntityReferenceComponent();
			}

			MyEntityReferenceComponent IActivator<MyEntityReferenceComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private int m_references;

		public override string ComponentTypeDebugString => "ReferenceCount";

		public void Ref()
		{
			m_references++;
		}

		public bool Unref()
		{
			m_references--;
			if (m_references <= 0)
			{
				base.Entity.Close();
				return true;
			}
			return false;
		}
	}
}
