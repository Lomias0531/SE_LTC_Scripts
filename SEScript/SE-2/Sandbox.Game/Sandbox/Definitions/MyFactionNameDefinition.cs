using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_FactionNameDefinition), null)]
	public class MyFactionNameDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyFactionNameDefinition_003C_003EActor : IActivator, IActivator<MyFactionNameDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyFactionNameDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFactionNameDefinition CreateInstance()
			{
				return new MyFactionNameDefinition();
			}

			MyFactionNameDefinition IActivator<MyFactionNameDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyLanguagesEnum LanguageId;

		public MyFactionNameTypeEnum Type;

		public List<string> Names;

		public List<string> Tags;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_FactionNameDefinition myObjectBuilder_FactionNameDefinition = builder as MyObjectBuilder_FactionNameDefinition;
			if (myObjectBuilder_FactionNameDefinition != null)
			{
				LanguageId = myObjectBuilder_FactionNameDefinition.LanguageId;
				Type = myObjectBuilder_FactionNameDefinition.Type;
				Names = myObjectBuilder_FactionNameDefinition.Names;
				Tags = myObjectBuilder_FactionNameDefinition.Tags;
			}
		}
	}
}
