using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ResearchGroupDefinition), null)]
	public class MyResearchGroupDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyResearchGroupDefinition_003C_003EActor : IActivator, IActivator<MyResearchGroupDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyResearchGroupDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyResearchGroupDefinition CreateInstance()
			{
				return new MyResearchGroupDefinition();
			}

			MyResearchGroupDefinition IActivator<MyResearchGroupDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public SerializableDefinitionId[] Members;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ResearchGroupDefinition myObjectBuilder_ResearchGroupDefinition = builder as MyObjectBuilder_ResearchGroupDefinition;
			if (myObjectBuilder_ResearchGroupDefinition.Members != null)
			{
				Members = new SerializableDefinitionId[myObjectBuilder_ResearchGroupDefinition.Members.Length];
				for (int i = 0; i < myObjectBuilder_ResearchGroupDefinition.Members.Length; i++)
				{
					Members[i] = myObjectBuilder_ResearchGroupDefinition.Members[i];
				}
			}
		}
	}
}
