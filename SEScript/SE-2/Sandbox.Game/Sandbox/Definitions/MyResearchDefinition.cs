using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ResearchDefinition), null)]
	public class MyResearchDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyResearchDefinition_003C_003EActor : IActivator, IActivator<MyResearchDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyResearchDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyResearchDefinition CreateInstance()
			{
				return new MyResearchDefinition();
			}

			MyResearchDefinition IActivator<MyResearchDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyDefinitionId> Entries;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ResearchDefinition obj = builder as MyObjectBuilder_ResearchDefinition;
			Entries = new List<MyDefinitionId>();
			foreach (SerializableDefinitionId entry in obj.Entries)
			{
				Entries.Add(entry);
			}
		}
	}
}
