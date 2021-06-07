using Sandbox.Game.Entities.Character;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_BotDefinition), null)]
	public class MyBotDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyBotDefinition_003C_003EActor : IActivator, IActivator<MyBotDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyBotDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBotDefinition CreateInstance()
			{
				return new MyBotDefinition();
			}

			MyBotDefinition IActivator<MyBotDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDefinitionId BotBehaviorTree;

		public string BehaviorType;

		public string BehaviorSubtype;

		public MyDefinitionId TypeDefinitionId;

		public bool Commandable;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_BotDefinition myObjectBuilder_BotDefinition = builder as MyObjectBuilder_BotDefinition;
			BotBehaviorTree = new MyDefinitionId(myObjectBuilder_BotDefinition.BotBehaviorTree.Type, myObjectBuilder_BotDefinition.BotBehaviorTree.Subtype);
			BehaviorType = myObjectBuilder_BotDefinition.BehaviorType;
			TypeDefinitionId = new MyDefinitionId(myObjectBuilder_BotDefinition.TypeId, myObjectBuilder_BotDefinition.SubtypeName);
			if (string.IsNullOrWhiteSpace(myObjectBuilder_BotDefinition.BehaviorSubtype))
			{
				BehaviorSubtype = myObjectBuilder_BotDefinition.BehaviorType;
			}
			else
			{
				BehaviorSubtype = myObjectBuilder_BotDefinition.BehaviorSubtype;
			}
			Commandable = myObjectBuilder_BotDefinition.Commandable;
		}

		public virtual void AddItems(MyCharacter character)
		{
		}
	}
}
