using Sandbox;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
	[TargetType("Wolf")]
	[StaticEventOwner]
	public class MyWolfTarget : MyAiTargetBase
	{
		protected sealed class PlayAttackAnimation_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				PlayAttackAnimation(entityId);
			}
		}

		private int m_attackStart;

		private bool m_attackPerformed;

		private BoundingSphereD m_attackBoundingSphere;

		private static readonly int ATTACK_LENGTH = 1000;

		private static readonly int ATTACK_DAMAGE_TO_CHARACTER = 12;

		private static readonly int ATTACK_DAMAGE_TO_GRID = 8;

		private static HashSet<MySlimBlock> m_tmpBlocks = new HashSet<MySlimBlock>();

		private static MyStringId m_stringIdAttackAction = MyStringId.GetOrCompute("attack");

		public bool IsAttacking
		{
			get;
			private set;
		}

		public MyWolfTarget(IMyEntityBot bot)
			: base(bot)
		{
		}

		public void Attack(bool playSound)
		{
			MyCharacter agentEntity = m_bot.AgentEntity;
			if (agentEntity != null)
			{
				IsAttacking = true;
				m_attackPerformed = false;
				m_attackStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				string animationName = "WolfAttack";
				string cueName = "ArcBotWolfAttack";
				if (!agentEntity.UseNewAnimationSystem)
				{
					agentEntity.PlayCharacterAnimation(animationName, MyBlendOption.Immediate, MyFrameOption.PlayOnce, 0f, 1f, sync: true);
					agentEntity.DisableAnimationCommands();
				}
				agentEntity.SoundComp.StartSecondarySound(cueName, sync: true);
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
			else if (num > 500 && m_bot.AgentEntity.UseNewAnimationSystem)
			{
				m_bot.AgentEntity.AnimationController.TriggerAction(m_stringIdAttackAction);
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseEvent(m_bot.AgentEntity, (MyCharacter x) => x.TriggerAnimationEvent, m_stringIdAttackAction.String);
				}
			}
			if (num > 500 && !m_attackPerformed)
			{
				MyCharacter agentEntity = m_bot.AgentEntity;
				if (agentEntity != null)
				{
					Vector3D center = agentEntity.WorldMatrix.Translation + agentEntity.PositionComp.WorldMatrix.Forward * 1.1000000238418579 + agentEntity.PositionComp.WorldMatrix.Up * 0.44999998807907104;
					m_attackBoundingSphere = new BoundingSphereD(center, 0.5);
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
									myCharacter.DoDamage(ATTACK_DAMAGE_TO_CHARACTER, MyDamageType.Wolf, updateSync: true, agentEntity.EntityId);
								}
							}
						}
					}
					topMostEntitiesInSphere.Clear();
				}
			}
		}

		[Event(null, 151)]
		[Broadcast]
		[Reliable]
		private static void PlayAttackAnimation(long entityId)
		{
			if (MyEntities.EntityExists(entityId))
			{
				(MyEntities.GetEntityById(entityId) as MyCharacter)?.AnimationController.TriggerAction(m_stringIdAttackAction);
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
