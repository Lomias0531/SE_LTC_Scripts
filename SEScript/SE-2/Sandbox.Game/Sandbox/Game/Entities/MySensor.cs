using Sandbox.Game.Components;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
	internal class MySensor : MySensorBase
	{
		private class Sandbox_Game_Entities_MySensor_003C_003EActor : IActivator, IActivator<MySensor>
		{
			private sealed override object CreateInstance()
			{
				return new MySensor();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySensor CreateInstance()
			{
				return new MySensor();
			}

			MySensor IActivator<MySensor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public void InitPhysics()
		{
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.Init(objectBuilder);
		}

		public MySensor()
		{
			base.Render = new MyRenderComponentSensor();
		}
	}
}
