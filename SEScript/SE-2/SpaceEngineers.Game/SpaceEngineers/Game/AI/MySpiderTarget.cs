using Sandbox;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
	[TargetType("Spider")]
	public class MySpiderTarget : MyAiTargetBase
	{
		private int m_attackStart;

		private int m_attackCtr;

		private bool m_attackPerformed;

		private BoundingSphereD m_attackBoundingSphere;

		private static readonly int ATTACK_LENGTH = 1000;

		private static readonly int ATTACK_ACTIVATION = 700;

		private static readonly int ATTACK_DAMAGE_TO_CHARACTER = 35;

		private static readonly int ATTACK_DAMAGE_TO_GRID = 50;

		private static HashSet<MySlimBlock> m_tmpBlocks = new HashSet<MySlimBlock>();

		public bool IsAttacking
		{
			get;
			private set;
		}

		public MySpiderTarget(IMyEntityBot bot)
			: base(bot)
		{
		}

		public void Attack()
		{
			MyCharacter agentEntity = m_bot.AgentEntity;
			if (agentEntity != null)
			{
				IsAttacking = true;
				m_attackPerformed = false;
				m_attackStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				ChooseAttackAnimationAndSound(out string animation, out string sound);
				agentEntity.PlayCharacterAnimation(animation, MyBlendOption.Immediate, MyFrameOption.PlayOnce, 0f, 1f, sync: true);
				agentEntity.DisableAnimationCommands();
				agentEntity.SoundComp.StartSecondarySound(sound, sync: true);
			}
		}

		public override void Update()
		{
			base.Update();
			if (!IsAttacking)
			{
				return;
			}
			int num = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_attackStart;
			if (num > ATTACK_LENGTH)
			{
				IsAttacking = false;
				m_bot.AgentEntity?.EnableAnimationCommands();
			}
			else if (num > 500 && m_bot.AgentEntity.UseNewAnimationSystem && !m_attackPerformed)
			{
				m_bot.AgentEntity.TriggerAnimationEvent("attack");
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseEvent(m_bot.AgentEntity, (MyCharacter x) => x.TriggerAnimationEvent, "attack");
				}
			}
			if (num > 750 && !m_attackPerformed)
			{
				MyCharacter agentEntity = m_bot.AgentEntity;
				if (agentEntity != null)
				{
					Vector3D center = agentEntity.WorldMatrix.Translation + agentEntity.PositionComp.WorldMatrix.Forward * 2.5 + agentEntity.PositionComp.WorldMatrix.Up * 1.0;
					m_attackBoundingSphere = new BoundingSphereD(center, 0.9);
					m_attackPerformed = true;
					List<MyEntity> topMostEntitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref m_attackBoundingSphere);
					foreach (MyEntity item in topMostEntitiesInSphere)
					{
						if (item is MyCharacter && item != agentEntity)
						{
							MyCharacter myCharacter = item as MyCharacter;
							if (!myCharacter.IsSitting)
							{
								BoundingSphereD worldVolume = myCharacter.PositionComp.WorldVolume;
								double num2 = m_attackBoundingSphere.Radius + worldVolume.Radius;
								num2 *= num2;
								if (!(Vector3D.DistanceSquared(m_attackBoundingSphere.Center, worldVolume.Center) > num2))
								{
									myCharacter.DoDamage(ATTACK_DAMAGE_TO_CHARACTER, MyDamageType.Spider, updateSync: true, agentEntity.EntityId);
								}
							}
						}
					}
					topMostEntitiesInSphere.Clear();
				}
			}
		}

		private void ChooseAttackAnimationAndSound(out string animation, out string sound)
		{
			m_attackCtr++;
			MyAiTargetEnum targetType = base.TargetType;
			if ((uint)(targetType - 2) > 1u && targetType == MyAiTargetEnum.CHARACTER)
			{
				MyCharacter myCharacter = base.TargetEntity as MyCharacter;
				if (myCharacter != null && myCharacter.IsDead)
				{
					if (m_attackCtr % 3 == 0)
					{
						animation = "AttackFrontLegs";
						sound = "ArcBotSpiderAttackClaw";
					}
					else
					{
						animation = "AttackBite";
						sound = "ArcBotSpiderAttackBite";
					}
				}
				else if (m_attackCtr % 2 == 0)
				{
					animation = "AttackStinger";
					sound = "ArcBotSpiderAttackSting";
				}
				else
				{
					animation = "AttackBite";
					sound = "ArcBotSpiderAttackBite";
				}
			}
			else
			{
				animation = "AttackFrontLegs";
				sound = "ArcBotSpiderAttackClaw";
			}
		}

		public override bool IsMemoryTargetValid(MyBBMemoryTarget targetMemory)
		{
			if (targetMemory == null)
			{
				return false;
			}
			if (targetMemory.TargetType == MyAiTargetEnum.GRID)
			{
				return false;
			}
			if (targetMemory.TargetType == MyAiTargetEnum.CUBE)
			{
				return false;
			}
			return base.IsMemoryTargetValid(targetMemory);
		}
	}
}
