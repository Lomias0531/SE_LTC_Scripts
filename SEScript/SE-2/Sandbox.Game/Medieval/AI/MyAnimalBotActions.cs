using Sandbox.Engine.Platform;
using Sandbox.Game.AI;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Diagnostics;
using VRage.Game;
using VRage.Game.AI;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Medieval.AI
{
	[MyBehaviorDescriptor("Animal")]
	[BehaviorActionImpl(typeof(MyAnimalBotLogic))]
	public class MyAnimalBotActions : MyAgentActions
	{
		private MyAnimalBot m_bot;

		private long m_eatTimeInS = 10L;

		private long m_eatCounter;

		private long m_soundCounter;

		private bool m_usingPathfinding;

		private static readonly double COS15 = Math.Cos(MathHelper.ToRadians(15f));

		private MyAnimalBotLogic AnimalLogic => m_bot.BotLogic as MyAnimalBotLogic;

		public MyAnimalBotActions(MyAnimalBot bot)
			: base(bot)
		{
			m_bot = bot;
		}

		[MyBehaviorTreeAction("IdleDanger", ReturnsRunning = false)]
		protected MyBehaviorTreeState IdleDanger()
		{
			m_bot.AgentEntity.SoundComp.StartSecondarySound("BotDeerBark", sync: true);
			return MyBehaviorTreeState.SUCCESS;
		}

		protected override void Init_Idle()
		{
			m_bot.Navigation.StopImmediate(forceUpdate: true);
			m_eatCounter = Stopwatch.GetTimestamp() + m_eatTimeInS * Stopwatch.Frequency;
			if (MyUtils.GetRandomInt(2) == 0)
			{
				long num = MyUtils.GetRandomLong() % (m_eatTimeInS - 2) + 1;
				m_soundCounter = Stopwatch.GetTimestamp() + num * Stopwatch.Frequency;
			}
			else
			{
				m_soundCounter = 0L;
			}
			m_bot.AgentEntity.PlayCharacterAnimation("Idle", MyBlendOption.Immediate, MyFrameOption.Loop, 0.5f);
		}

		protected override MyBehaviorTreeState Idle()
		{
			long timestamp = Stopwatch.GetTimestamp();
			if (m_soundCounter != 0L && m_soundCounter < timestamp)
			{
				if (MyRandom.Instance.NextFloat() > 0.7f)
				{
					m_bot.AgentEntity.SoundComp.StartSecondarySound("BotDeerRoar", sync: true);
				}
				else
				{
					m_bot.AgentEntity.SoundComp.StartSecondarySound("BotDeerBark", sync: true);
				}
				m_soundCounter = 0L;
			}
			if (m_eatCounter > timestamp)
			{
				return MyBehaviorTreeState.RUNNING;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		protected override void Init_GotoTarget()
		{
			if (base.AiTargetBase.HasTarget())
			{
				base.AiTargetBase.GotoTargetNoPath(0f);
				m_bot.Navigation.AimWithMovement();
			}
		}

		[MyBehaviorTreeAction("FindWanderLocation", ReturnsRunning = false)]
		protected MyBehaviorTreeState FindWanderLocation([BTOut] ref MyBBMemoryTarget outTarget)
		{
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("IsHumanInArea", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsHumanInArea([BTParam] int standingRadius, [BTParam] int crouchingRadius, [BTOut] ref MyBBMemoryTarget outTarget)
		{
			MyCharacter foundCharacter = null;
			if (TryFindValidHumanInArea(standingRadius, crouchingRadius, out foundCharacter))
			{
				MyBBMemoryTarget.SetTargetEntity(ref outTarget, MyAiTargetEnum.CHARACTER, foundCharacter.EntityId);
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("AmIBeingFollowed", ReturnsRunning = false)]
		protected MyBehaviorTreeState AmIBeingFollowed([BTIn] ref MyBBMemoryTarget inTarget)
		{
			if (inTarget != null)
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("IsHumanNotInArea", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsHumanNotInArea([BTParam] int standingRadius, [BTParam] int crouchingRadius, [BTOut] ref MyBBMemoryTarget outTarget)
		{
			return InvertState(IsHumanInArea(standingRadius, crouchingRadius, ref outTarget));
		}

		private MyBehaviorTreeState InvertState(MyBehaviorTreeState state)
		{
			switch (state)
			{
			case MyBehaviorTreeState.SUCCESS:
				return MyBehaviorTreeState.FAILURE;
			case MyBehaviorTreeState.FAILURE:
				return MyBehaviorTreeState.SUCCESS;
			default:
				return state;
			}
		}

		private bool TryFindValidHumanInArea(int standingRadius, int crouchingRadius, out MyCharacter foundCharacter)
		{
			Vector3D position = m_bot.AgentEntity.PositionComp.GetPosition();
			Vector3D forward = m_bot.AgentEntity.PositionComp.WorldMatrix.Forward;
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if ((onlinePlayer.Id.SerialId == 0 || MyAIComponent.Static.Bots.GetBotType(onlinePlayer.Id.SerialId) == BotType.HUMANOID) && onlinePlayer.Character != null && !onlinePlayer.Character.MarkedForClose && !onlinePlayer.Character.IsDead)
				{
					Vector3D vector = onlinePlayer.Character.PositionComp.GetPosition() - position;
					vector.Y = 0.0;
					double num = vector.Normalize();
					bool flag = false;
					if (num < (double)standingRadius)
					{
						flag = (Vector3D.Dot(vector, forward) > COS15 || !onlinePlayer.Character.IsCrouching || onlinePlayer.Character.IsSprinting || num < (double)crouchingRadius);
					}
					if (flag)
					{
						foundCharacter = onlinePlayer.Character;
						return true;
					}
				}
			}
			foundCharacter = null;
			return false;
		}

		[MyBehaviorTreeAction("FindRandomSafeLocation", ReturnsRunning = false)]
		protected MyBehaviorTreeState FindRandomSafeLocation([BTIn] ref MyBBMemoryTarget inTargetEnemy, [BTOut] ref MyBBMemoryTarget outTargetLocation)
		{
			if (inTargetEnemy == null || !inTargetEnemy.EntityId.HasValue || Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			MyEntity entity = null;
			if (MyEntities.TryGetEntityById(inTargetEnemy.EntityId.Value, out entity))
			{
				Vector3D position = m_bot.AgentEntity.PositionComp.GetPosition();
				Vector3D vector3D = position - entity.PositionComp.GetPosition();
				vector3D.Normalize();
				Vector3D outPosition = default(Vector3D);
				if (!base.AiTargetBase.GetRandomDirectedPosition(position, vector3D, out outPosition))
				{
					outPosition = position + vector3D * 30.0;
				}
				MyBBMemoryTarget.SetTargetPosition(ref outTargetLocation, outPosition);
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("RunAway", MyBehaviorTreeActionType.INIT)]
		protected void Init_RunAway()
		{
			AnimalLogic.EnableCharacterAvoidance(isTrue: true);
			m_bot.Navigation.AimWithMovement();
			base.AiTargetBase.GotoTargetNoPath(0f);
		}

		[MyBehaviorTreeAction("RunAway")]
		protected MyBehaviorTreeState RunAway()
		{
			if (m_bot.Navigation.Navigating)
			{
				if (m_bot.Navigation.Stuck)
				{
					if (m_usingPathfinding)
					{
						return MyBehaviorTreeState.FAILURE;
					}
					m_usingPathfinding = true;
					AnimalLogic.EnableCharacterAvoidance(isTrue: false);
					base.AiTargetBase.GotoTarget();
					return MyBehaviorTreeState.RUNNING;
				}
				return MyBehaviorTreeState.RUNNING;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("RunAway", MyBehaviorTreeActionType.POST)]
		protected void Post_RunAway()
		{
			m_usingPathfinding = false;
			m_bot.Navigation.StopImmediate(forceUpdate: true);
		}

		[MyBehaviorTreeAction("PlaySound", ReturnsRunning = false)]
		protected MyBehaviorTreeState PlaySound([BTParam] string soundtrack)
		{
			m_bot.AgentEntity.SoundComp.StartSecondarySound(soundtrack, sync: true);
			return MyBehaviorTreeState.SUCCESS;
		}
	}
}
