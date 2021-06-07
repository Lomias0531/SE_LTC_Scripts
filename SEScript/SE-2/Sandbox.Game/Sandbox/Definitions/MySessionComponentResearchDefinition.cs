using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components.Session;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_SessionComponentResearchDefinition), null)]
	public class MySessionComponentResearchDefinition : MySessionComponentDefinition
	{
		private class Sandbox_Definitions_MySessionComponentResearchDefinition_003C_003EActor : IActivator, IActivator<MySessionComponentResearchDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySessionComponentResearchDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySessionComponentResearchDefinition CreateInstance()
			{
				return new MySessionComponentResearchDefinition();
			}

			MySessionComponentResearchDefinition IActivator<MySessionComponentResearchDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool WhitelistMode;

		public List<MyDefinitionId> Researches = new List<MyDefinitionId>();

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_SessionComponentResearchDefinition myObjectBuilder_SessionComponentResearchDefinition = builder as MyObjectBuilder_SessionComponentResearchDefinition;
			WhitelistMode = myObjectBuilder_SessionComponentResearchDefinition.WhitelistMode;
			if (myObjectBuilder_SessionComponentResearchDefinition.Researches != null)
			{
				foreach (SerializableDefinitionId research in myObjectBuilder_SessionComponentResearchDefinition.Researches)
				{
					Researches.Add(research);
				}
			}
		}
	}
}
