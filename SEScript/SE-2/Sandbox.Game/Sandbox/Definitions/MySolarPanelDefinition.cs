using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_SolarPanelDefinition), null)]
	public class MySolarPanelDefinition : MyPowerProducerDefinition
	{
		private class Sandbox_Definitions_MySolarPanelDefinition_003C_003EActor : IActivator, IActivator<MySolarPanelDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySolarPanelDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySolarPanelDefinition CreateInstance()
			{
				return new MySolarPanelDefinition();
			}

			MySolarPanelDefinition IActivator<MySolarPanelDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Vector3 PanelOrientation;

		public bool IsTwoSided;

		public float PanelOffset;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_SolarPanelDefinition myObjectBuilder_SolarPanelDefinition = builder as MyObjectBuilder_SolarPanelDefinition;
			PanelOrientation = myObjectBuilder_SolarPanelDefinition.PanelOrientation;
			IsTwoSided = myObjectBuilder_SolarPanelDefinition.TwoSidedPanel;
			PanelOffset = myObjectBuilder_SolarPanelDefinition.PanelOffset;
		}
	}
}
