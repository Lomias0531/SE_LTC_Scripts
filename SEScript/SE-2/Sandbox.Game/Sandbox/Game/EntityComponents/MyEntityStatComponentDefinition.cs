using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.EntityComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_EntityStatComponentDefinition), null)]
	public class MyEntityStatComponentDefinition : MyComponentDefinitionBase
	{
		private class Sandbox_Game_EntityComponents_MyEntityStatComponentDefinition_003C_003EActor : IActivator, IActivator<MyEntityStatComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityStatComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityStatComponentDefinition CreateInstance()
			{
				return new MyEntityStatComponentDefinition();
			}

			MyEntityStatComponentDefinition IActivator<MyEntityStatComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyDefinitionId> Stats;

		public List<string> Scripts;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_EntityStatComponentDefinition myObjectBuilder_EntityStatComponentDefinition = builder as MyObjectBuilder_EntityStatComponentDefinition;
			Stats = new List<MyDefinitionId>();
			foreach (SerializableDefinitionId stat in myObjectBuilder_EntityStatComponentDefinition.Stats)
			{
				Stats.Add(stat);
			}
			Scripts = myObjectBuilder_EntityStatComponentDefinition.Scripts;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_EntityStatComponentDefinition myObjectBuilder_EntityStatComponentDefinition = base.GetObjectBuilder() as MyObjectBuilder_EntityStatComponentDefinition;
			myObjectBuilder_EntityStatComponentDefinition.Stats = new List<SerializableDefinitionId>();
			foreach (MyDefinitionId stat in Stats)
			{
				myObjectBuilder_EntityStatComponentDefinition.Stats.Add(stat);
			}
			myObjectBuilder_EntityStatComponentDefinition.Scripts = Scripts;
			return myObjectBuilder_EntityStatComponentDefinition;
		}
	}
}
