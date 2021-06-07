using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AgentDefinition), null)]
	public class MyAgentDefinition : MyBotDefinition
	{
		private class Sandbox_Definitions_MyAgentDefinition_003C_003EActor : IActivator, IActivator<MyAgentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAgentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAgentDefinition CreateInstance()
			{
				return new MyAgentDefinition();
			}

			MyAgentDefinition IActivator<MyAgentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string BotModel;

		public string TargetType;

		public bool InventoryContentGenerated;

		public MyDefinitionId InventoryContainerTypeId;

		public bool RemoveAfterDeath;

		public int RespawnTimeMs;

		public int RemoveTimeMs;

		public string FactionTag;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AgentDefinition myObjectBuilder_AgentDefinition = builder as MyObjectBuilder_AgentDefinition;
			BotModel = myObjectBuilder_AgentDefinition.BotModel;
			TargetType = myObjectBuilder_AgentDefinition.TargetType;
			InventoryContentGenerated = myObjectBuilder_AgentDefinition.InventoryContentGenerated;
			if (myObjectBuilder_AgentDefinition.InventoryContainerTypeId.HasValue)
			{
				InventoryContainerTypeId = myObjectBuilder_AgentDefinition.InventoryContainerTypeId.Value;
			}
			RemoveAfterDeath = myObjectBuilder_AgentDefinition.RemoveAfterDeath;
			RespawnTimeMs = myObjectBuilder_AgentDefinition.RespawnTimeMs;
			RemoveTimeMs = myObjectBuilder_AgentDefinition.RemoveTimeMs;
			FactionTag = myObjectBuilder_AgentDefinition.FactionTag;
		}

		public override void AddItems(MyCharacter character)
		{
			character.GetInventory().Clear();
			if (InventoryContentGenerated)
			{
				MyContainerTypeDefinition containerTypeDefinition = MyDefinitionManager.Static.GetContainerTypeDefinition(InventoryContainerTypeId.SubtypeName);
				if (containerTypeDefinition != null)
				{
					character.GetInventory().GenerateContent(containerTypeDefinition);
				}
			}
		}
	}
}
