using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_OxygenGeneratorDefinition), null)]
	public class MyOxygenGeneratorDefinition : MyProductionBlockDefinition
	{
		public struct MyGasGeneratorResourceInfo
		{
			public MyDefinitionId Id;

			public float IceToGasRatio;
		}

		private class Sandbox_Definitions_MyOxygenGeneratorDefinition_003C_003EActor : IActivator, IActivator<MyOxygenGeneratorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyOxygenGeneratorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyOxygenGeneratorDefinition CreateInstance()
			{
				return new MyOxygenGeneratorDefinition();
			}

			MyOxygenGeneratorDefinition IActivator<MyOxygenGeneratorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float IceConsumptionPerSecond;

		public MySoundPair GenerateSound;

		public MySoundPair IdleSound;

		public MyStringHash ResourceSourceGroup;

		public List<MyGasGeneratorResourceInfo> ProducedGases;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_OxygenGeneratorDefinition myObjectBuilder_OxygenGeneratorDefinition = builder as MyObjectBuilder_OxygenGeneratorDefinition;
			IceConsumptionPerSecond = myObjectBuilder_OxygenGeneratorDefinition.IceConsumptionPerSecond;
			GenerateSound = new MySoundPair(myObjectBuilder_OxygenGeneratorDefinition.GenerateSound);
			IdleSound = new MySoundPair(myObjectBuilder_OxygenGeneratorDefinition.IdleSound);
			ResourceSourceGroup = MyStringHash.GetOrCompute(myObjectBuilder_OxygenGeneratorDefinition.ResourceSourceGroup);
			ProducedGases = null;
			if (myObjectBuilder_OxygenGeneratorDefinition.ProducedGases != null)
			{
				ProducedGases = new List<MyGasGeneratorResourceInfo>(myObjectBuilder_OxygenGeneratorDefinition.ProducedGases.Count);
				foreach (MyObjectBuilder_GasGeneratorResourceInfo producedGase in myObjectBuilder_OxygenGeneratorDefinition.ProducedGases)
				{
					ProducedGases.Add(new MyGasGeneratorResourceInfo
					{
						Id = producedGase.Id,
						IceToGasRatio = producedGase.IceToGasRatio
					});
				}
			}
		}
	}
}
