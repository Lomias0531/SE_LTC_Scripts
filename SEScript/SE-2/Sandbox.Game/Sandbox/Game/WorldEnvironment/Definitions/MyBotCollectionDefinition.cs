using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_BotCollectionDefinition), null)]
	public class MyBotCollectionDefinition : MyDefinitionBase
	{
		private class Sandbox_Game_WorldEnvironment_Definitions_MyBotCollectionDefinition_003C_003EActor : IActivator, IActivator<MyBotCollectionDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyBotCollectionDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBotCollectionDefinition CreateInstance()
			{
				return new MyBotCollectionDefinition();
			}

			MyBotCollectionDefinition IActivator<MyBotCollectionDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDiscreteSampler<MyDefinitionId> Bots;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_BotCollectionDefinition myObjectBuilder_BotCollectionDefinition = builder as MyObjectBuilder_BotCollectionDefinition;
			if (myObjectBuilder_BotCollectionDefinition != null)
			{
				List<MyDefinitionId> list = new List<MyDefinitionId>();
				List<float> list2 = new List<float>();
				for (int i = 0; i < myObjectBuilder_BotCollectionDefinition.Bots.Length; i++)
				{
					MyObjectBuilder_BotCollectionDefinition.BotDefEntry botDefEntry = myObjectBuilder_BotCollectionDefinition.Bots[i];
					list.Add(botDefEntry.Id);
					list2.Add(botDefEntry.Probability);
				}
				Bots = new MyDiscreteSampler<MyDefinitionId>(list, list2);
			}
		}
	}
}
