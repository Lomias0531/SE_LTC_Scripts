using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_SchematicItemDefinition), null)]
	public class MySchematicItemDefinition : MyUsableItemDefinition
	{
		private class Sandbox_Definitions_MySchematicItemDefinition_003C_003EActor : IActivator, IActivator<MySchematicItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySchematicItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySchematicItemDefinition CreateInstance()
			{
				return new MySchematicItemDefinition();
			}

			MySchematicItemDefinition IActivator<MySchematicItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDefinitionId Research;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_SchematicItemDefinition myObjectBuilder_SchematicItemDefinition = builder as MyObjectBuilder_SchematicItemDefinition;
			Research = myObjectBuilder_SchematicItemDefinition.Research.Value;
		}
	}
}
