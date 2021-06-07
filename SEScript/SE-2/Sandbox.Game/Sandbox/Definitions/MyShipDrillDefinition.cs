using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ShipDrillDefinition), null)]
	public class MyShipDrillDefinition : MyShipToolDefinition
	{
		private class Sandbox_Definitions_MyShipDrillDefinition_003C_003EActor : IActivator, IActivator<MyShipDrillDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipDrillDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipDrillDefinition CreateInstance()
			{
				return new MyShipDrillDefinition();
			}

			MyShipDrillDefinition IActivator<MyShipDrillDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float CutOutOffset;

		public float CutOutRadius;

		public Vector3D ParticleOffset;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ShipDrillDefinition myObjectBuilder_ShipDrillDefinition = builder as MyObjectBuilder_ShipDrillDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_ShipDrillDefinition.ResourceSinkGroup);
			CutOutOffset = myObjectBuilder_ShipDrillDefinition.CutOutOffset;
			CutOutRadius = myObjectBuilder_ShipDrillDefinition.CutOutRadius;
			ParticleOffset = myObjectBuilder_ShipDrillDefinition.ParticleOffset;
		}
	}
}
