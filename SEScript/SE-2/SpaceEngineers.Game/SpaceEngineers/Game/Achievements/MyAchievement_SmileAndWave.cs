using Sandbox.Game.Entities.Character;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace SpaceEngineers.Game.Achievements
{
	public class MyAchievement_SmileAndWave : MySteamAchievementBase
	{
		private const string WAVE_ANIMATION_NAME = "RightHand/Emote";

		private readonly string ANIMATION_CLIP_NAME = "AnimStack::Astronaut_character_wave_final";

		private MyStringId m_waveAnimationId;

		private MyCharacter m_localCharacter;

		public override bool NeedsUpdate => m_localCharacter == null;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_SmileAndWave", null, 0f);
		}

		public override void SessionBeforeStart()
		{
			if (!base.IsAchieved && !MySession.Static.CreativeMode)
			{
				m_waveAnimationId = MyStringId.GetOrCompute("Wave");
				m_localCharacter = null;
			}
		}

		public override void SessionUpdate()
		{
			if (!base.IsAchieved)
			{
				m_localCharacter = MySession.Static.LocalCharacter;
				if (m_localCharacter != null)
				{
					MySession.Static.LocalCharacter.AnimationController.ActionTriggered += AnimationControllerOnActionTriggered;
				}
			}
		}

		private void AnimationControllerOnActionTriggered(MyStringId animationAction)
		{
			if (!(animationAction != m_waveAnimationId))
			{
				Vector3D value = MySession.Static.LocalCharacter.PositionComp.GetPosition();
				long factionId = MySession.Static.Factions.GetPlayerFaction(MySession.Static.LocalPlayerId)?.FactionId ?? 0;
				foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
				{
					if (onlinePlayer.Character != null && onlinePlayer.Character != MySession.Static.LocalCharacter)
					{
						Vector3D value2 = onlinePlayer.Character.PositionComp.GetPosition();
						Vector3D.DistanceSquared(ref value2, ref value, out double result);
						if (result < 25.0)
						{
							long factionId2 = MySession.Static.Factions.GetPlayerFaction(onlinePlayer.Identity.IdentityId)?.FactionId ?? 0;
							if (MySession.Static.Factions.AreFactionsEnemies(factionId, factionId2) && IsPlayerWaving(onlinePlayer.Character) && PlayersLookingFaceToFace(MySession.Static.LocalCharacter, onlinePlayer.Character))
							{
								NotifyAchieved();
								MySession.Static.LocalCharacter.AnimationController.ActionTriggered -= AnimationControllerOnActionTriggered;
								break;
							}
						}
					}
				}
			}
		}

		private bool PlayersLookingFaceToFace(MyCharacter firstCharacter, MyCharacter secondCharacter)
		{
			Vector3D vector = firstCharacter.GetHeadMatrix(includeY: false).Forward;
			Vector3D vector2 = secondCharacter.GetHeadMatrix(includeY: false).Forward;
			Vector3D.Dot(ref vector, ref vector2, out double result);
			return result < -0.5;
		}

		private bool IsPlayerWaving(MyCharacter character)
		{
			MyAnimationController controller = character.AnimationController.Controller;
			for (int i = 0; i < controller.GetLayerCount(); i++)
			{
				MyAnimationStateMachine layerByIndex = controller.GetLayerByIndex(i);
				MyAnimationStateMachineNode myAnimationStateMachineNode;
				MyAnimationTreeNodeTrack myAnimationTreeNodeTrack;
				if (layerByIndex.CurrentNode != null && layerByIndex.CurrentNode.Name != null && layerByIndex.CurrentNode.Name == "RightHand/Emote" && (myAnimationStateMachineNode = (layerByIndex.CurrentNode as MyAnimationStateMachineNode)) != null && (myAnimationTreeNodeTrack = (myAnimationStateMachineNode.RootAnimationNode as MyAnimationTreeNodeTrack)) != null && myAnimationTreeNodeTrack.AnimationClip.Name == ANIMATION_CLIP_NAME)
				{
					return true;
				}
			}
			return false;
		}
	}
}
