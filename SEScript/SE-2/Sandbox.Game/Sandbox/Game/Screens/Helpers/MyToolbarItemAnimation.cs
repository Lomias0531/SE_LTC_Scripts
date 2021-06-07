using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemAnimation))]
	internal class MyToolbarItemAnimation : MyToolbarItemDefinition
	{
		public override bool Init(MyObjectBuilder_ToolbarItem objBuilder)
		{
			base.Init(objBuilder);
			base.ActivateOnClick = true;
			base.WantsToBeActivated = true;
			return true;
		}

		public override bool Activate()
		{
			if (Definition == null)
			{
				return false;
			}
			MyAnimationDefinition myAnimationDefinition = (MyAnimationDefinition)Definition;
			bool flag = MySession.Static.ControlledEntity is MyCockpit;
			MyCharacter myCharacter = flag ? ((MyCockpit)MySession.Static.ControlledEntity).Pilot : MySession.Static.LocalCharacter;
			if (myCharacter != null)
			{
				if (myCharacter.UseNewAnimationSystem)
				{
					if (myCharacter.IsOnLadder)
					{
						return true;
					}
					if (flag && !myAnimationDefinition.AllowInCockpit)
					{
						return true;
					}
					string subtypeName = myAnimationDefinition.Id.SubtypeName;
					myCharacter.TriggerCharacterAnimationEvent("emote", sync: true, myAnimationDefinition.InfluenceAreas);
					myCharacter.TriggerCharacterAnimationEvent(subtypeName, sync: true, myAnimationDefinition.InfluenceAreas);
				}
				else
				{
					myCharacter.AddCommand(new MyAnimationCommand
					{
						AnimationSubtypeName = myAnimationDefinition.Id.SubtypeName,
						BlendTime = 0.2f,
						PlaybackCommand = MyPlaybackCommand.Play,
						FrameOption = ((!myAnimationDefinition.Loop) ? MyFrameOption.PlayOnce : MyFrameOption.Loop),
						TimeScale = 1f
					}, sync: true);
				}
			}
			return true;
		}

		public override bool AllowedInToolbarType(MyToolbarType type)
		{
			if (type != 0 && type != MyToolbarType.Ship)
			{
				return type == MyToolbarType.Seat;
			}
			return true;
		}

		public override ChangeInfo Update(MyEntity owner, long playerID = 0L)
		{
			return ChangeInfo.None;
		}
	}
}
