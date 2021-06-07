using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GhostCharacterDefinition), null)]
	public class MyGhostCharacterDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyGhostCharacterDefinition_003C_003EActor : IActivator, IActivator<MyGhostCharacterDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGhostCharacterDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGhostCharacterDefinition CreateInstance()
			{
				return new MyGhostCharacterDefinition();
			}

			MyGhostCharacterDefinition IActivator<MyGhostCharacterDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyDefinitionId> LeftHandWeapons = new List<MyDefinitionId>();

		public List<MyDefinitionId> RightHandWeapons = new List<MyDefinitionId>();

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GhostCharacterDefinition myObjectBuilder_GhostCharacterDefinition = builder as MyObjectBuilder_GhostCharacterDefinition;
			if (myObjectBuilder_GhostCharacterDefinition.LeftHandWeapons != null)
			{
				SerializableDefinitionId[] leftHandWeapons = myObjectBuilder_GhostCharacterDefinition.LeftHandWeapons;
				foreach (SerializableDefinitionId v in leftHandWeapons)
				{
					LeftHandWeapons.Add(v);
				}
			}
			if (myObjectBuilder_GhostCharacterDefinition.RightHandWeapons != null)
			{
				SerializableDefinitionId[] leftHandWeapons = myObjectBuilder_GhostCharacterDefinition.RightHandWeapons;
				foreach (SerializableDefinitionId v2 in leftHandWeapons)
				{
					RightHandWeapons.Add(v2);
				}
			}
		}
	}
}
