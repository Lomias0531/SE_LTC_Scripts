using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GasProperties), null)]
	public class MyGasProperties : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyGasProperties_003C_003EActor : IActivator, IActivator<MyGasProperties>
		{
			private sealed override object CreateInstance()
			{
				return new MyGasProperties();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGasProperties CreateInstance()
			{
				return new MyGasProperties();
			}

			MyGasProperties IActivator<MyGasProperties>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float EnergyDensity;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GasProperties myObjectBuilder_GasProperties = builder as MyObjectBuilder_GasProperties;
			EnergyDensity = myObjectBuilder_GasProperties.EnergyDensity;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_GasProperties obj = base.GetObjectBuilder() as MyObjectBuilder_GasProperties;
			obj.EnergyDensity = EnergyDensity;
			return obj;
		}
	}
}
