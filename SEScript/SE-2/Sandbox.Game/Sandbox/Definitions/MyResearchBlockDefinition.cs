using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ResearchBlockDefinition), null)]
	public class MyResearchBlockDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyResearchBlockDefinition_003C_003EActor : IActivator, IActivator<MyResearchBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyResearchBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyResearchBlockDefinition CreateInstance()
			{
				return new MyResearchBlockDefinition();
			}

			MyResearchBlockDefinition IActivator<MyResearchBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string[] UnlockedByGroups;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ResearchBlockDefinition myObjectBuilder_ResearchBlockDefinition = builder as MyObjectBuilder_ResearchBlockDefinition;
			if (myObjectBuilder_ResearchBlockDefinition.UnlockedByGroups != null)
			{
				UnlockedByGroups = new string[myObjectBuilder_ResearchBlockDefinition.UnlockedByGroups.Length];
				for (int i = 0; i < myObjectBuilder_ResearchBlockDefinition.UnlockedByGroups.Length; i++)
				{
					UnlockedByGroups[i] = myObjectBuilder_ResearchBlockDefinition.UnlockedByGroups[i];
				}
			}
		}
	}
}
