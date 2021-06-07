using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.Definitions.Animation;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_EmoteDefinition), null)]
	public class MyEmoteDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyEmoteDefinition_003C_003EActor : IActivator, IActivator<MyEmoteDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyEmoteDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEmoteDefinition CreateInstance()
			{
				return new MyEmoteDefinition();
			}

			MyEmoteDefinition IActivator<MyEmoteDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly Dictionary<MyDefinitionId, MyDefinitionId> m_overrides = new Dictionary<MyDefinitionId, MyDefinitionId>();

		public DictionaryReader<MyDefinitionId, MyDefinitionId> Overrides => m_overrides;

		public MyDefinitionId AnimationId
		{
			get;
			private set;
		}

		public string ChatCommand
		{
			get;
			private set;
		}

		public string ChatCommandName
		{
			get;
			private set;
		}

		public string ChatCommandDescription
		{
			get;
			private set;
		}

		public int Priority
		{
			get;
			private set;
		}

		public new bool Public
		{
			get;
			private set;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_EmoteDefinition myObjectBuilder_EmoteDefinition = builder as MyObjectBuilder_EmoteDefinition;
			AnimationId = myObjectBuilder_EmoteDefinition.AnimationId;
			ChatCommand = myObjectBuilder_EmoteDefinition.ChatCommand;
			ChatCommandName = myObjectBuilder_EmoteDefinition.ChatCommandName;
			ChatCommandDescription = myObjectBuilder_EmoteDefinition.ChatCommandDescription;
			Priority = myObjectBuilder_EmoteDefinition.Priority;
			Public = myObjectBuilder_EmoteDefinition.Public;
			if (myObjectBuilder_EmoteDefinition.Overrides != null && myObjectBuilder_EmoteDefinition.Overrides.Length != 0)
			{
				MyObjectBuilder_EmoteDefinition.AnimOverrideDef[] overrides = myObjectBuilder_EmoteDefinition.Overrides;
				for (int i = 0; i < overrides.Length; i++)
				{
					MyObjectBuilder_EmoteDefinition.AnimOverrideDef animOverrideDef = overrides[i];
					m_overrides[animOverrideDef.CharacterId] = animOverrideDef.AnimationId;
				}
			}
		}

		public MyAnimationDefinition GetAnimationForCharacter(MyDefinitionId characterId)
		{
			if (m_overrides.TryGetValue(characterId, out MyDefinitionId value))
			{
				MyAnimationDefinition myAnimationDefinition = MyDefinitionManager.Static.TryGetAnimationDefinition(value.SubtypeName);
				if (myAnimationDefinition != null)
				{
					return myAnimationDefinition;
				}
			}
			return MyDefinitionManager.Static.TryGetAnimationDefinition(AnimationId.SubtypeName);
		}
	}
}
