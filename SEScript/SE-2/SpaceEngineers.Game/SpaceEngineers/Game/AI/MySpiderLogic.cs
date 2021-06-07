using Sandbox;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Game.AI;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using VRage.Game.ModAPI;
using VRage.Network;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
	[StaticEventOwner]
	public class MySpiderLogic : MyAgentLogic
	{
		private bool m_burrowing;

		private bool m_deburrowing;

		private bool m_deburrowAnimationStarted;

		private bool m_deburrowSoundStarted;

		private int m_burrowStart;

		private int m_deburrowStart;

		private Vector3D? m_effectOnPosition;

		private static readonly int BURROWING_TIME = 750;

		private static readonly int BURROWING_FX_START = 300;

		private static readonly int DEBURROWING_TIME = 1800;

		private static readonly int DEBURROWING_ANIMATION_START = 0;

		private static readonly int DEBURROWING_SOUND_START = 0;

		public bool IsBurrowing => m_burrowing;

		public bool IsDeburrowing => m_deburrowing;

		public MySpiderLogic(MyAnimalBot bot)
			: base(bot)
		{
		}

		public override void Update()
		{
			base.Update();
			if (m_burrowing || m_deburrowing)
			{
				UpdateBurrowing();
			}
		}

		public override void Cleanup()
		{
			base.Cleanup();
			DeleteBurrowingParticleFX();
		}

		public void StartBurrowing()
		{
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(base.AgentBot.BotEntity.WorldMatrix.Translation - 3.0 * base.AgentBot.BotEntity.WorldMatrix.Up, base.AgentBot.BotEntity.WorldMatrix.Translation + 3.0 * base.AgentBot.BotEntity.WorldMatrix.Up, 9);
			if (hitInfo.HasValue && ((IHitInfo)hitInfo).HitEntity != base.AgentBot.BotEntity)
			{
				return;
			}
			if (base.AgentBot.AgentEntity.UseNewAnimationSystem)
			{
				base.AgentBot.AgentEntity.TriggerAnimationEvent("burrow");
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseEvent(base.AgentBot.AgentEntity, (MyCharacter x) => x.TriggerAnimationEvent, "burrow");
				}
			}
			else if (base.AgentBot.AgentEntity.HasAnimation("Burrow"))
			{
				base.AgentBot.AgentEntity.PlayCharacterAnimation("Burrow", MyBlendOption.Immediate, MyFrameOption.Default, 0f, 1f, sync: true);
				base.AgentBot.AgentEntity.DisableAnimationCommands();
			}
			base.AgentBot.AgentEntity.SoundComp.StartSecondarySound("ArcBotSpiderBurrowIn", sync: true);
			m_burrowing = true;
			m_burrowStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		public void StartDeburrowing()
		{
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(base.AgentBot.BotEntity.WorldMatrix.Translation - 3.0 * base.AgentBot.BotEntity.WorldMatrix.Up, base.AgentBot.BotEntity.WorldMatrix.Translation + 3.0 * base.AgentBot.BotEntity.WorldMatrix.Up, 9);
			if (hitInfo.HasValue && ((IHitInfo)hitInfo).HitEntity != base.AgentBot.BotEntity)
			{
				return;
			}
			if (base.AgentBot.AgentEntity.UseNewAnimationSystem)
			{
				base.AgentBot.AgentEntity.TriggerAnimationEvent("deburrow");
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseEvent(base.AgentBot.AgentEntity, (MyCharacter x) => x.TriggerAnimationEvent, "deburrow");
				}
			}
			m_deburrowing = true;
			m_deburrowStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			CreateBurrowingParticleFX();
			m_deburrowAnimationStarted = false;
			m_deburrowSoundStarted = false;
		}

		private void UpdateBurrowing()
		{
			if (m_burrowing)
			{
				int num = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_burrowStart;
				if (num > BURROWING_FX_START && !m_effectOnPosition.HasValue)
				{
					CreateBurrowingParticleFX();
				}
				if (num >= BURROWING_TIME)
				{
					m_burrowing = false;
					DeleteBurrowingParticleFX();
					base.AgentBot.AgentEntity.EnableAnimationCommands();
				}
			}
			if (!m_deburrowing)
			{
				return;
			}
			int num2 = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_deburrowStart;
			if (!m_deburrowSoundStarted && num2 >= DEBURROWING_SOUND_START)
			{
				base.AgentBot.AgentEntity.SoundComp.StartSecondarySound("ArcBotSpiderBurrowOut", sync: true);
				m_deburrowSoundStarted = true;
			}
			if (!m_deburrowAnimationStarted && num2 >= DEBURROWING_ANIMATION_START)
			{
				if (base.AgentBot.AgentEntity.HasAnimation("Deburrow"))
				{
					base.AgentBot.AgentEntity.EnableAnimationCommands();
					base.AgentBot.AgentEntity.PlayCharacterAnimation("Deburrow", MyBlendOption.Immediate, MyFrameOption.Default, 0f, 1f, sync: true);
					base.AgentBot.AgentEntity.DisableAnimationCommands();
				}
				m_deburrowAnimationStarted = true;
			}
			if (num2 >= DEBURROWING_TIME)
			{
				m_deburrowing = false;
				DeleteBurrowingParticleFX();
				base.AgentBot.AgentEntity.EnableAnimationCommands();
			}
		}

		private void CreateBurrowingParticleFX()
		{
			Vector3D translation = base.AgentBot.BotEntity.PositionComp.WorldMatrix.Translation;
			translation += base.AgentBot.BotEntity.PositionComp.WorldMatrix.Forward * 0.2;
			m_effectOnPosition = translation;
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				base.AgentBot.AgentEntity.CreateBurrowingParticleFX_Client(translation);
			}
			MyMultiplayer.RaiseEvent(base.AgentBot.AgentEntity, (MyCharacter x) => x.CreateBurrowingParticleFX_Client, translation);
		}

		private void DeleteBurrowingParticleFX()
		{
			if (m_effectOnPosition.HasValue && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyCharacter agentEntity = base.AgentBot.AgentEntity;
				if (agentEntity != null)
				{
					agentEntity.DeleteBurrowingParticleFX_Client(m_effectOnPosition.Value);
					MyMultiplayer.RaiseEvent(base.AgentBot.AgentEntity, (MyCharacter x) => x.DeleteBurrowingParticleFX_Client, m_effectOnPosition.Value);
				}
			}
			m_effectOnPosition = null;
		}
	}
}
