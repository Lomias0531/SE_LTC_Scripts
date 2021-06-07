using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PhysicalModelDefinition), typeof(Postprocessor))]
	public class MyPhysicalModelDefinition : MyDefinitionBase
	{
		protected class Postprocessor : MyDefinitionPostprocessor
		{
			public override void AfterLoaded(ref Bundle definitions)
			{
			}

			public override void AfterPostprocess(MyDefinitionSet set, Dictionary<MyStringHash, MyDefinitionBase> definitions)
			{
				foreach (MyPhysicalModelDefinition item in definitions.Values.Cast<MyPhysicalModelDefinition>())
				{
					item.PhysicalMaterial = MyDestructionData.GetPhysicalMaterial(item, item.m_material);
				}
			}
		}

		private class Sandbox_Definitions_MyPhysicalModelDefinition_003C_003EActor : IActivator, IActivator<MyPhysicalModelDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPhysicalModelDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPhysicalModelDefinition CreateInstance()
			{
				return new MyPhysicalModelDefinition();
			}

			MyPhysicalModelDefinition IActivator<MyPhysicalModelDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string Model;

		public MyPhysicalMaterialDefinition PhysicalMaterial;

		public float Mass;

		private string m_material;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PhysicalModelDefinition myObjectBuilder_PhysicalModelDefinition = builder as MyObjectBuilder_PhysicalModelDefinition;
			Model = myObjectBuilder_PhysicalModelDefinition.Model;
			if (GetType() == typeof(MyCubeBlockDefinition) || GetType().IsSubclassOf(typeof(MyCubeBlockDefinition)))
			{
				PhysicalMaterial = MyDestructionData.GetPhysicalMaterial(this, myObjectBuilder_PhysicalModelDefinition.PhysicalMaterial);
			}
			else
			{
				m_material = myObjectBuilder_PhysicalModelDefinition.PhysicalMaterial;
			}
			Mass = myObjectBuilder_PhysicalModelDefinition.Mass;
		}
	}
}
