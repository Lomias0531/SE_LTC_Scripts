using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AssemblerDefinition), null)]
	public class MyAssemblerDefinition : MyProductionBlockDefinition
	{
		private class Sandbox_Definitions_MyAssemblerDefinition_003C_003EActor : IActivator, IActivator<MyAssemblerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAssemblerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAssemblerDefinition CreateInstance()
			{
				return new MyAssemblerDefinition();
			}

			MyAssemblerDefinition IActivator<MyAssemblerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private float m_assemblySpeed;

		public float AssemblySpeed => m_assemblySpeed;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AssemblerDefinition myObjectBuilder_AssemblerDefinition = builder as MyObjectBuilder_AssemblerDefinition;
			m_assemblySpeed = myObjectBuilder_AssemblerDefinition.AssemblySpeed;
		}

		protected override void InitializeLegacyBlueprintClasses(MyObjectBuilder_ProductionBlockDefinition ob)
		{
			ob.BlueprintClasses = new string[4]
			{
				"LargeBlocks",
				"SmallBlocks",
				"Components",
				"Tools"
			};
		}
	}
}
