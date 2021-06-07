using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game.Definitions.Animation;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyAnimationActivator
	{
		public static void Activate(MyEmoteDefinition emoteDefinition)
		{
			if (emoteDefinition != null)
			{
				MyCharacter myCharacter = (MySession.Static.ControlledEntity is MyCockpit) ? ((MyCockpit)MySession.Static.ControlledEntity).Pilot : MySession.Static.LocalCharacter;
				if (myCharacter != null)
				{
					Activate((myCharacter.Definition != null) ? emoteDefinition.GetAnimationForCharacter(myCharacter.Definition.Id) : MyDefinitionManager.Static.TryGetAnimationDefinition(emoteDefinition.AnimationId.SubtypeName), myCharacter);
				}
			}
		}

		public static void Activate(MyAnimationDefinition animationDefinition)
		{
			if (animationDefinition != null)
			{
				MyCharacter controlledObject = (MySession.Static.ControlledEntity is MyCockpit) ? ((MyCockpit)MySession.Static.ControlledEntity).Pilot : MySession.Static.LocalCharacter;
				Activate(animationDefinition, controlledObject);
			}
		}

		public static void Activate(MyAnimationDefinition animationDefinition, MyCharacter controlledObject)
		{
			if (animationDefinition != null && controlledObject != null && !controlledObject.IsOnLadder)
			{
				if (controlledObject.UseNewAnimationSystem)
				{
					controlledObject.TriggerCharacterAnimationEvent("emote", sync: true, animationDefinition.InfluenceAreas);
					controlledObject.TriggerCharacterAnimationEvent(animationDefinition.Id.SubtypeName, sync: true, animationDefinition.InfluenceAreas);
				}
				else
				{
					controlledObject.AddCommand(new MyAnimationCommand
					{
						AnimationSubtypeName = animationDefinition.Id.SubtypeName,
						BlendTime = 0.2f,
						PlaybackCommand = MyPlaybackCommand.Play,
						FrameOption = ((!animationDefinition.Loop) ? MyFrameOption.PlayOnce : MyFrameOption.Loop),
						TimeScale = 1f
					}, sync: true);
				}
			}
		}
	}
}
