using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game.Components
{
	public class MyNullGameLogicComponent : MyGameLogicComponent
	{
		private class VRage_Game_Components_MyNullGameLogicComponent_003C_003EActor : IActivator, IActivator<MyNullGameLogicComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyNullGameLogicComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyNullGameLogicComponent CreateInstance()
			{
				return new MyNullGameLogicComponent();
			}

			MyNullGameLogicComponent IActivator<MyNullGameLogicComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
		}

		public override void UpdateBeforeSimulation()
		{
		}

		public override void UpdateBeforeSimulation10()
		{
		}

		public override void UpdateBeforeSimulation100()
		{
		}

		public override void UpdateAfterSimulation()
		{
		}

		public override void UpdateAfterSimulation10()
		{
		}

		public override void UpdateAfterSimulation100()
		{
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
		}

		public override void MarkForClose()
		{
		}

		public override void Close()
		{
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			return null;
		}
	}
}
