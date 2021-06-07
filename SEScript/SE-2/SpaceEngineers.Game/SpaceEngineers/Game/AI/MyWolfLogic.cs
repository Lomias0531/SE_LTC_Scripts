using Sandbox;
using Sandbox.Game;
using Sandbox.Game.AI;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
	public class MyWolfLogic : MyAgentLogic
	{
		private static readonly int SELF_DESTRUCT_TIME_MS = 4000;

		private static readonly float EXPLOSION_RADIUS = 4f;

		private static readonly int EXPLOSION_DAMAGE = 7500;

		private static readonly int EXPLOSION_PLAYER_DAMAGE = 0;

		private bool m_selfDestruct;

		private int m_selfDestructStartedInTime;

		private bool m_lastWasAttacking;

		public bool SelfDestructionActivated => m_selfDestruct;

		public MyWolfLogic(MyAnimalBot bot)
			: base(bot)
		{
		}

		public override void Update()
		{
			base.Update();
			if (m_selfDestruct && MySandboxGame.TotalGamePlayTimeInMilliseconds >= m_selfDestructStartedInTime + SELF_DESTRUCT_TIME_MS)
			{
				MyAIComponent.Static.RemoveBot(base.AgentBot.Player.Id.SerialId, removeCharacter: true);
				BoundingSphere b = new BoundingSphere(base.AgentBot.Player.GetPosition(), EXPLOSION_RADIUS);
				MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
				myExplosionInfo.PlayerDamage = EXPLOSION_PLAYER_DAMAGE;
				myExplosionInfo.Damage = EXPLOSION_DAMAGE;
				myExplosionInfo.ExplosionType = MyExplosionTypeEnum.BOMB_EXPLOSION;
				myExplosionInfo.ExplosionSphere = b;
				myExplosionInfo.LifespanMiliseconds = 700;
				myExplosionInfo.HitEntity = base.AgentBot.Player.Character;
				myExplosionInfo.ParticleScale = 0.5f;
				myExplosionInfo.OwnerEntity = base.AgentBot.Player.Character;
				myExplosionInfo.Direction = Vector3.Zero;
				myExplosionInfo.VoxelExplosionCenter = base.AgentBot.Player.Character.PositionComp.GetPosition();
				myExplosionInfo.ExplosionFlags = (MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION);
				myExplosionInfo.VoxelCutoutScale = 0.6f;
				myExplosionInfo.PlaySound = true;
				myExplosionInfo.ApplyForceAndDamage = true;
				myExplosionInfo.ObjectsRemoveDelayInMiliseconds = 40;
				MyExplosionInfo explosionInfo = myExplosionInfo;
				if (base.AgentBot.Player.Character.Physics != null)
				{
					explosionInfo.Velocity = base.AgentBot.Player.Character.Physics.LinearVelocity;
				}
				MyExplosions.AddExplosion(ref explosionInfo);
			}
			MyWolfTarget myWolfTarget = base.AiTarget as MyWolfTarget;
			if (base.AgentBot.Player.Character != null && !base.AgentBot.Player.Character.UseNewAnimationSystem && !myWolfTarget.IsAttacking && !m_lastWasAttacking && myWolfTarget.HasTarget() && !myWolfTarget.PositionIsNearTarget(base.AgentBot.Player.Character.PositionComp.GetPosition(), 1.5f))
			{
				if (base.AgentBot.Navigation.Stuck)
				{
					Vector3D position = base.AgentBot.Player.Character.PositionComp.GetPosition();
					Vector3D vector3D = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
					Vector3D vector3D2 = base.AgentBot.Player.Character.AimedPoint - position;
					Vector3D value = vector3D2 - vector3D * Vector3D.Dot(vector3D2, vector3D) / vector3D.LengthSquared();
					value.Normalize();
					base.AgentBot.Navigation.AimAt(null, position + 100.0 * value);
					base.AgentBot.Player.Character.PlayCharacterAnimation("WolfIdle1", MyBlendOption.Immediate, MyFrameOption.Loop, 0f);
					base.AgentBot.Player.Character.DisableAnimationCommands();
				}
				else
				{
					base.AgentBot.Player.Character.EnableAnimationCommands();
				}
			}
			m_lastWasAttacking = myWolfTarget.IsAttacking;
		}

		public override void Cleanup()
		{
			base.Cleanup();
		}

		public void ActivateSelfDestruct()
		{
			if (!m_selfDestruct)
			{
				m_selfDestructStartedInTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				m_selfDestruct = true;
				string cueName = "ArcBotCyberSelfActDestr";
				base.AgentBot.AgentEntity.SoundComp.StartSecondarySound(cueName, sync: true);
			}
		}

		public void Remove()
		{
			MyAIComponent.Static.RemoveBot(base.AgentBot.Player.Id.SerialId, removeCharacter: true);
		}
	}
}
