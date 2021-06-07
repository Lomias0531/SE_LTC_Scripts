using Sandbox.Game.AI;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.AI;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
	[MyBehaviorDescriptor("Wolf")]
	[BehaviorActionImpl(typeof(MyWolfLogic))]
	public class MyWolfActions : MyAgentActions
	{
		private Vector3D? m_runAwayPos;

		private Vector3D? m_lastTargetedEntityPosition;

		private Vector3D? m_debugTarget;

		private MyWolfTarget WolfTarget => base.AiTargetBase as MyWolfTarget;

		protected MyWolfLogic WolfLogic => base.Bot.AgentLogic as MyWolfLogic;

		public MyWolfActions(MyAnimalBot bot)
			: base(bot)
		{
		}

		protected override MyBehaviorTreeState Idle()
		{
			return MyBehaviorTreeState.RUNNING;
		}

		[MyBehaviorTreeAction("GoToPlayerDefinedTarget", ReturnsRunning = true)]
		protected MyBehaviorTreeState GoToPlayerDefinedTarget()
		{
			if (m_debugTarget != MyAIComponent.Static.DebugTarget)
			{
				m_debugTarget = MyAIComponent.Static.DebugTarget;
				if (!MyAIComponent.Static.DebugTarget.HasValue)
				{
					return MyBehaviorTreeState.FAILURE;
				}
			}
			Vector3D position = base.Bot.Player.Character.PositionComp.GetPosition();
			if (m_debugTarget.HasValue)
			{
				if (Vector3D.Distance(position, m_debugTarget.Value) <= 1.0)
				{
					return MyBehaviorTreeState.SUCCESS;
				}
				Vector3D worldCenter = m_debugTarget.Value;
				MyDestinationSphere end = new MyDestinationSphere(ref worldCenter, 1f);
				if (!MyAIComponent.Static.Pathfinding.FindPathGlobal(position, end, null).GetNextTarget(position, out Vector3D target, out float _, out IMyEntity _))
				{
					return MyBehaviorTreeState.FAILURE;
				}
				if (WolfTarget.TargetPosition != target)
				{
					WolfTarget.SetTargetPosition(target);
				}
				WolfTarget.AimAtTarget();
				WolfTarget.GotoTargetNoPath(0f, resetStuckDetection: false);
			}
			return MyBehaviorTreeState.RUNNING;
		}

		[MyBehaviorTreeAction("Attack", MyBehaviorTreeActionType.INIT)]
		protected void Init_Attack()
		{
			WolfTarget.AimAtTarget();
			((Vector3)(WolfTarget.TargetPosition - base.Bot.AgentEntity.PositionComp.GetPosition())).Normalize();
			WolfTarget.Attack(!WolfLogic.SelfDestructionActivated);
		}

		[MyBehaviorTreeAction("Attack")]
		protected MyBehaviorTreeState Attack()
		{
			if (!WolfTarget.IsAttacking)
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.RUNNING;
		}

		[MyBehaviorTreeAction("Attack", MyBehaviorTreeActionType.POST)]
		protected void Post_Attack()
		{
		}

		[MyBehaviorTreeAction("IsAttacking", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsAttacking()
		{
			if (!WolfTarget.IsAttacking)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("Explode")]
		protected MyBehaviorTreeState Explode()
		{
			WolfLogic.ActivateSelfDestruct();
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("GetTargetWithPriority")]
		protected MyBehaviorTreeState GetTargetWithPriority([BTParam] float radius, [BTInOut] ref MyBBMemoryTarget outTarget, [BTInOut] ref MyBBMemoryInt priority)
		{
			if (WolfLogic.SelfDestructionActivated)
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			if (base.Bot == null)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			if (base.Bot.AgentEntity == null)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			Vector3D translation = base.Bot.Navigation.PositionAndOrientation.Translation;
			BoundingSphereD boundingSphere = new BoundingSphereD(translation, radius);
			if (priority == null)
			{
				priority = new MyBBMemoryInt();
			}
			int num = priority.IntValue;
			if (num <= 0 || base.Bot.Navigation.Stuck)
			{
				num = int.MaxValue;
			}
			MyBehaviorTreeState myBehaviorTreeState = IsTargetValid(ref outTarget);
			if (myBehaviorTreeState == MyBehaviorTreeState.FAILURE)
			{
				num = 7;
				MyBBMemoryTarget.UnsetTarget(ref outTarget);
			}
			if (WolfTarget == null)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			Vector3D? memoryTargetPosition = WolfTarget.GetMemoryTargetPosition(outTarget);
			if (!memoryTargetPosition.HasValue || Vector3D.DistanceSquared(memoryTargetPosition.Value, base.Bot.AgentEntity.PositionComp.GetPosition()) > 160000.0)
			{
				num = 7;
				MyBBMemoryTarget.UnsetTarget(ref outTarget);
			}
			if (memoryTargetPosition.HasValue)
			{
				Vector3D globalPos = memoryTargetPosition.Value;
				MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(globalPos);
				if (closestPlanet != null)
				{
					Vector3D closestSurfacePointGlobal = closestPlanet.GetClosestSurfacePointGlobal(ref globalPos);
					if (Vector3D.DistanceSquared(closestSurfacePointGlobal, globalPos) > 2.25 && Vector3D.DistanceSquared(closestSurfacePointGlobal, base.Bot.AgentEntity.PositionComp.GetPosition()) < 25.0)
					{
						num = 7;
						MyBBMemoryTarget.UnsetTarget(ref outTarget);
					}
				}
			}
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(base.Bot.AgentEntity.ControllerInfo.ControllingIdentityId);
			List<MyEntity> topMostEntitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
			topMostEntitiesInSphere.ShuffleList();
			foreach (MyEntity item in topMostEntitiesInSphere)
			{
				if (item != base.Bot.AgentEntity && !(item is MyVoxelBase) && WolfTarget.IsEntityReachable(item))
				{
					Vector3D globalPos2 = item.PositionComp.GetPosition();
					MyPlanet closestPlanet2 = MyGamePruningStructure.GetClosestPlanet(globalPos2);
					if (closestPlanet2 == null || !(Vector3D.DistanceSquared(closestPlanet2.GetClosestSurfacePointGlobal(ref globalPos2), globalPos2) > 1.0))
					{
						int num2 = 6;
						MyCharacter myCharacter = item as MyCharacter;
						if (myCharacter != null)
						{
							MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
							if ((playerFaction == null || playerFaction2 != playerFaction) && !myCharacter.IsDead)
							{
								num2 = 1;
								if (num2 < num)
								{
									myBehaviorTreeState = MyBehaviorTreeState.SUCCESS;
									num = num2;
									MyBBMemoryTarget.SetTargetEntity(ref outTarget, MyAiTargetEnum.CHARACTER, myCharacter.EntityId);
									m_lastTargetedEntityPosition = myCharacter.PositionComp.GetPosition();
								}
							}
						}
					}
				}
			}
			topMostEntitiesInSphere.Clear();
			priority.IntValue = num;
			if (outTarget.TargetType == MyAiTargetEnum.NO_TARGET)
			{
				myBehaviorTreeState = MyBehaviorTreeState.FAILURE;
			}
			return myBehaviorTreeState;
		}

		[MyBehaviorTreeAction("IsRunningAway", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsRunningAway()
		{
			if (!m_runAwayPos.HasValue)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("RunAway", MyBehaviorTreeActionType.INIT)]
		protected MyBehaviorTreeState RunAway_Init()
		{
			return MyBehaviorTreeState.RUNNING;
		}

		[MyBehaviorTreeAction("RunAway")]
		protected MyBehaviorTreeState RunAway([BTParam] float distance)
		{
			if (!m_runAwayPos.HasValue)
			{
				Vector3D center = base.Bot.Player.Character.PositionComp.GetPosition();
				Vector3D vector3D = MyGravityProviderSystem.CalculateNaturalGravityInPoint(center);
				MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(center);
				if (closestPlanet == null)
				{
					return MyBehaviorTreeState.FAILURE;
				}
				if (m_lastTargetedEntityPosition.HasValue)
				{
					Vector3D globalPos = m_lastTargetedEntityPosition.Value;
					globalPos = closestPlanet.GetClosestSurfacePointGlobal(ref globalPos);
					Vector3D value = center - globalPos;
					Vector3D globalPos2 = center + Vector3D.Normalize(value) * distance;
					m_runAwayPos = closestPlanet.GetClosestSurfacePointGlobal(ref globalPos2);
				}
				else
				{
					vector3D.Normalize();
					Vector3D tangent = Vector3D.CalculatePerpendicularVector(vector3D);
					Vector3D bitangent = Vector3D.Cross(vector3D, tangent);
					tangent.Normalize();
					bitangent.Normalize();
					Vector3D globalPos3 = MyUtils.GetRandomDiscPosition(ref center, distance, distance, ref tangent, ref bitangent);
					if (closestPlanet != null)
					{
						m_runAwayPos = closestPlanet.GetClosestSurfacePointGlobal(ref globalPos3);
					}
					else
					{
						m_runAwayPos = globalPos3;
					}
				}
				base.AiTargetBase.SetTargetPosition(m_runAwayPos.Value);
				AimWithMovement();
			}
			else if (base.Bot.Navigation.Stuck)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			base.AiTargetBase.GotoTargetNoPath(1f, resetStuckDetection: false);
			if (Vector3D.DistanceSquared(m_runAwayPos.Value, base.Bot.Player.Character.PositionComp.GetPosition()) < 100.0)
			{
				WolfLogic.Remove();
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.RUNNING;
		}
	}
}
