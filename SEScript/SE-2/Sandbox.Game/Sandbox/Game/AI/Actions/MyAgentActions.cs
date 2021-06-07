using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.AI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.AI.Actions
{
	public abstract class MyAgentActions : MyBotActionsBase
	{
		private string m_animationName;

		private MyRandomLocationSphere m_locationSphere;

		protected MyAgentBot Bot
		{
			get;
			private set;
		}

		public MyAiTargetBase AiTargetBase => Bot.AgentLogic.AiTarget;

		protected MyAgentActions(MyAgentBot bot)
		{
			Bot = bot;
			m_locationSphere = new MyRandomLocationSphere(Vector3D.Zero, 30f, Vector3D.UnitX);
		}

		[MyBehaviorTreeAction("AimWithMovement", ReturnsRunning = false)]
		protected MyBehaviorTreeState AimWithMovement()
		{
			Bot.Navigation.AimWithMovement();
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("GotoTarget", MyBehaviorTreeActionType.INIT)]
		protected virtual void Init_GotoTarget()
		{
			if (AiTargetBase.HasTarget())
			{
				AiTargetBase.GotoTarget();
			}
		}

		[MyBehaviorTreeAction("GotoTarget")]
		protected MyBehaviorTreeState GotoTarget()
		{
			if (!AiTargetBase.HasTarget())
			{
				return MyBehaviorTreeState.FAILURE;
			}
			if (Bot.Navigation.Navigating)
			{
				if (Bot.Navigation.Stuck)
				{
					AiTargetBase.GotoFailed();
					return MyBehaviorTreeState.FAILURE;
				}
				return MyBehaviorTreeState.RUNNING;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("GotoTarget", MyBehaviorTreeActionType.POST)]
		protected void Post_GotoTarget()
		{
			Bot.Navigation.StopImmediate(forceUpdate: true);
		}

		[MyBehaviorTreeAction("GotoTargetNoPathfinding", MyBehaviorTreeActionType.INIT)]
		protected virtual void Init_GotoTargetNoPathfinding()
		{
			if (AiTargetBase.HasTarget())
			{
				AiTargetBase.GotoTargetNoPath(1f);
			}
		}

		[MyBehaviorTreeAction("GotoTargetNoPathfinding")]
		protected MyBehaviorTreeState GotoTargetNoPathfinding([BTParam] float radius, [BTParam] bool resetStuckDetection)
		{
			if (!AiTargetBase.HasTarget())
			{
				return MyBehaviorTreeState.FAILURE;
			}
			if (Bot.Navigation.Navigating)
			{
				if (Bot.Navigation.Stuck)
				{
					AiTargetBase.GotoFailed();
					return MyBehaviorTreeState.FAILURE;
				}
				AiTargetBase.GotoTargetNoPath(radius, resetStuckDetection);
				return MyBehaviorTreeState.RUNNING;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("AimAtTarget", MyBehaviorTreeActionType.INIT)]
		protected void Init_AimAtTarget()
		{
			Init_AimAtTargetCustom();
		}

		[MyBehaviorTreeAction("AimAtTarget")]
		protected MyBehaviorTreeState AimAtTarget()
		{
			return AimAtTargetCustom(2f);
		}

		[MyBehaviorTreeAction("AimAtTarget", MyBehaviorTreeActionType.POST)]
		protected void Post_AimAtTarget()
		{
			Post_AimAtTargetCustom();
		}

		[MyBehaviorTreeAction("AimAtTargetCustom", MyBehaviorTreeActionType.INIT)]
		protected void Init_AimAtTargetCustom()
		{
			if (AiTargetBase.HasTarget())
			{
				AiTargetBase.AimAtTarget();
			}
		}

		[MyBehaviorTreeAction("AimAtTargetCustom")]
		protected MyBehaviorTreeState AimAtTargetCustom([BTParam] float tolerance)
		{
			if (!AiTargetBase.HasTarget())
			{
				return MyBehaviorTreeState.FAILURE;
			}
			if (Bot.Navigation.HasRotation(MathHelper.ToRadians(tolerance)))
			{
				return MyBehaviorTreeState.RUNNING;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("AimAtTargetCustom", MyBehaviorTreeActionType.POST)]
		protected void Post_AimAtTargetCustom()
		{
			Bot.Navigation.StopAiming();
		}

		[MyBehaviorTreeAction("PlayAnimation", ReturnsRunning = false)]
		protected MyBehaviorTreeState PlayAnimation([BTParam] string animationName, [BTParam] bool immediate)
		{
			if (Bot.Player.Character.HasAnimation(animationName))
			{
				m_animationName = animationName;
				Bot.Player.Character.PlayCharacterAnimation(animationName, (!immediate) ? MyBlendOption.WaitForPreviousEnd : MyBlendOption.Immediate, MyFrameOption.PlayOnce, 0f);
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("IsAtTargetPosition", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsAtTargetPosition([BTParam] float radius)
		{
			if (!AiTargetBase.HasTarget())
			{
				return MyBehaviorTreeState.FAILURE;
			}
			if (AiTargetBase.PositionIsNearTarget(Bot.Player.Character.PositionComp.GetPosition(), radius))
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("IsAtTargetPositionCylinder", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsAtTargetPositionCylinder([BTParam] float radius, [BTParam] float height)
		{
			if (!AiTargetBase.HasTarget())
			{
				return MyBehaviorTreeState.FAILURE;
			}
			Vector3D position = Bot.Player.Character.PositionComp.GetPosition();
			AiTargetBase.GetTargetPosition(position, out Vector3D targetPosition, out float _);
			Vector2 value = new Vector2((float)position.X, (float)position.Z);
			Vector2 value2 = new Vector2((float)targetPosition.X, (float)targetPosition.Z);
			if (!(Vector2.Distance(value, value2) <= radius) || !(value.Y < value2.Y) || !(value.Y + height > value2.Y))
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("IsNotAtTargetPosition", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsNotAtTargetPosition([BTParam] float radius)
		{
			if (!AiTargetBase.HasTarget())
			{
				return MyBehaviorTreeState.FAILURE;
			}
			if (AiTargetBase.PositionIsNearTarget(Bot.Player.Character.PositionComp.GetPosition(), radius))
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("IsLookingAtTarget", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsLookingAtTarget()
		{
			if (Bot.Navigation.HasRotation(MathHelper.ToRadians(2f)))
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("SetTarget", ReturnsRunning = false)]
		protected MyBehaviorTreeState SetTarget([BTIn] ref MyBBMemoryTarget inTarget)
		{
			if (inTarget != null)
			{
				if (AiTargetBase.SetTargetFromMemory(inTarget))
				{
					return MyBehaviorTreeState.SUCCESS;
				}
				return MyBehaviorTreeState.FAILURE;
			}
			AiTargetBase.UnsetTarget();
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("ClearTarget", ReturnsRunning = false)]
		protected MyBehaviorTreeState ClearTarget([BTInOut] ref MyBBMemoryTarget inTarget)
		{
			if (inTarget != null)
			{
				inTarget.TargetType = MyAiTargetEnum.NO_TARGET;
				inTarget.Position = null;
				inTarget.EntityId = null;
				inTarget.TreeId = null;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("IsTargetValid", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsTargetValid([BTIn] ref MyBBMemoryTarget inTarget)
		{
			if (inTarget == null)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			if (!AiTargetBase.IsMemoryTargetValid(inTarget))
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("HasPlaceArea", ReturnsRunning = false)]
		protected MyBehaviorTreeState HasTargetArea([BTIn] ref MyBBMemoryTarget inTarget)
		{
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("HasTarget", ReturnsRunning = false)]
		protected MyBehaviorTreeState HasTarget()
		{
			if (AiTargetBase.TargetType != 0)
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("HasNoTarget", ReturnsRunning = false)]
		protected MyBehaviorTreeState HasNoTarget()
		{
			if (HasTarget() != MyBehaviorTreeState.SUCCESS)
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("Stand", ReturnsRunning = false)]
		protected MyBehaviorTreeState Stand()
		{
			Bot.AgentEntity.Stand();
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("SwitchToWalk", ReturnsRunning = false)]
		protected MyBehaviorTreeState SwitchToWalk()
		{
			if (!Bot.AgentEntity.WantsWalk)
			{
				Bot.AgentEntity.SwitchWalk();
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("SwitchToRun", ReturnsRunning = false)]
		protected MyBehaviorTreeState SwitchToRun()
		{
			if (Bot.AgentEntity.WantsWalk)
			{
				Bot.AgentEntity.SwitchWalk();
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("GotoRandomLocation", MyBehaviorTreeActionType.INIT)]
		protected void Init_GotoRandomLocation()
		{
			Vector3D position = Bot.AgentEntity.PositionComp.GetPosition();
			Vector3D axis = MyPerGameSettings.NavmeshPresumesDownwardGravity ? Vector3D.UnitY : ((Vector3D)MyGravityProviderSystem.CalculateTotalGravityInPoint(position));
			axis.Normalize();
			Vector3D randomPerpendicularVector = MyUtils.GetRandomPerpendicularVector(ref axis);
			Vector3D worldCenter = position - randomPerpendicularVector * 15.0;
			AiTargetBase.SetTargetPosition(position + randomPerpendicularVector * 30.0);
			Bot.Navigation.AimAt(null, AiTargetBase.TargetPosition);
			m_locationSphere.Init(ref worldCenter, 30f, randomPerpendicularVector);
			Bot.Navigation.Goto(m_locationSphere);
		}

		private static Vector3D GetRandomPerpendicularVector(ref Vector3D axis)
		{
			Vector3D vector = Vector3D.CalculatePerpendicularVector(axis);
			Vector3D.Cross(ref axis, ref vector, out Vector3D result);
			double randomDouble = MyUtils.GetRandomDouble(0.0, 6.2831859588623047);
			return Math.Cos(randomDouble) * vector + Math.Sin(randomDouble) * result;
		}

		[MyBehaviorTreeAction("GotoRandomLocation")]
		protected MyBehaviorTreeState GotoRandomLocation()
		{
			return GotoTarget();
		}

		[MyBehaviorTreeAction("GotoRandomLocation", MyBehaviorTreeActionType.POST)]
		protected void Post_GotoRandomLocation()
		{
			Post_GotoTarget();
		}

		[MyBehaviorTreeAction("GotoAndAimTarget", MyBehaviorTreeActionType.INIT)]
		protected void Init_GotoAndAimTarget()
		{
			if (AiTargetBase.HasTarget())
			{
				AiTargetBase.GotoTarget();
				AiTargetBase.AimAtTarget();
			}
		}

		[MyBehaviorTreeAction("GotoAndAimTarget")]
		protected MyBehaviorTreeState GotoAndAimTarget()
		{
			if (!AiTargetBase.HasTarget())
			{
				return MyBehaviorTreeState.FAILURE;
			}
			if (Bot.Navigation.Navigating)
			{
				if (Bot.Navigation.Stuck)
				{
					AiTargetBase.GotoFailed();
					return MyBehaviorTreeState.FAILURE;
				}
				Vector3D targetPosition = AiTargetBase.GetTargetPosition(Bot.Navigation.PositionAndOrientation.Translation);
				Vector3D targetPoint = Bot.Navigation.TargetPoint;
				if ((targetPoint - targetPosition).Length() > 0.10000000149011612)
				{
					CheckReplanningOfPath(targetPosition, targetPoint);
				}
				return MyBehaviorTreeState.RUNNING;
			}
			if (Bot.Navigation.HasRotation(MathHelper.ToRadians(2f)))
			{
				return MyBehaviorTreeState.RUNNING;
			}
			if (AiTargetBase.PositionIsNearTarget(Bot.Navigation.PositionAndOrientation.Translation, 2f))
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			AiTargetBase.GotoFailed();
			return MyBehaviorTreeState.FAILURE;
		}

		private void CheckReplanningOfPath(Vector3D targetPos, Vector3D navigationTarget)
		{
			Vector3D vector3D = targetPos - Bot.Navigation.PositionAndOrientation.Translation;
			Vector3D v = navigationTarget - Bot.Navigation.PositionAndOrientation.Translation;
			double num = vector3D.Length();
			double num2 = v.Length();
			if (num != 0.0 && num2 != 0.0)
			{
				double num3 = Math.Acos(vector3D.Dot(v) / (num * num2));
				double num4 = num / num2;
				if (num3 > Math.PI / 5.0 || num4 < 0.8 || (num4 > 1.0 && num2 < 2.0))
				{
					AiTargetBase.GotoTarget();
					AiTargetBase.AimAtTarget();
				}
			}
		}

		[MyBehaviorTreeAction("GotoAndAimTarget", MyBehaviorTreeActionType.POST)]
		protected void Post_GotoAndAimTarget()
		{
			Bot.Navigation.StopImmediate(forceUpdate: true);
			Bot.Navigation.StopAiming();
		}

		[MyBehaviorTreeAction("StopAiming", ReturnsRunning = false)]
		protected MyBehaviorTreeState StopAiming()
		{
			Bot.Navigation.StopAiming();
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("GotoFailed", ReturnsRunning = false)]
		protected MyBehaviorTreeState GotoFailed()
		{
			if (AiTargetBase.HasGotoFailed)
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("ResetGotoFailed", ReturnsRunning = false)]
		protected MyBehaviorTreeState ResetGotoFailed()
		{
			AiTargetBase.HasGotoFailed = false;
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("IsMoving", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsMoving()
		{
			if (!Bot.Navigation.Navigating)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("FindClosestPlaceAreaInRadius", ReturnsRunning = false)]
		protected MyBehaviorTreeState FindClosestPlaceAreaInRadius([BTParam] float radius, [BTParam] string typeName, [BTOut] ref MyBBMemoryTarget outTarget)
		{
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("IsTargetBlock", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsTargetBlock([BTIn] ref MyBBMemoryTarget inTarget)
		{
			if (inTarget.TargetType == MyAiTargetEnum.COMPOUND_BLOCK || inTarget.TargetType == MyAiTargetEnum.CUBE)
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("IsTargetNonBlock", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsTargetNonBlock([BTIn] ref MyBBMemoryTarget inTarget)
		{
			if (inTarget.TargetType == MyAiTargetEnum.COMPOUND_BLOCK || inTarget.TargetType == MyAiTargetEnum.CUBE)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("FindClosestBlock", ReturnsRunning = false)]
		protected MyBehaviorTreeState FindClosestBlock([BTOut] ref MyBBMemoryTarget outBlock)
		{
			if (!AiTargetBase.IsTargetGridOrBlock(AiTargetBase.TargetType))
			{
				outBlock = null;
				return MyBehaviorTreeState.FAILURE;
			}
			MyCubeGrid targetGrid = AiTargetBase.TargetGrid;
			Vector3 value = Vector3D.Transform(Bot.BotEntity.PositionComp.GetPosition(), targetGrid.PositionComp.WorldMatrixNormalizedInv);
			float num = float.MaxValue;
			MySlimBlock mySlimBlock = null;
			foreach (MySlimBlock block in targetGrid.GetBlocks())
			{
				float num2 = Vector3.DistanceSquared((Vector3)block.Position * targetGrid.GridSize, value);
				if (num2 < num)
				{
					mySlimBlock = block;
					num = num2;
				}
			}
			if (mySlimBlock == null)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			MyBBMemoryTarget.SetTargetCube(ref outBlock, mySlimBlock.Position, mySlimBlock.CubeGrid.EntityId);
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("SetAndAimTarget", ReturnsRunning = false)]
		protected MyBehaviorTreeState SetAndAimTarget([BTIn] ref MyBBMemoryTarget inTarget)
		{
			return SetTarget(aim: true, ref inTarget);
		}

		protected MyBehaviorTreeState SetTarget(bool aim, ref MyBBMemoryTarget inTarget)
		{
			if (inTarget != null)
			{
				if (AiTargetBase.SetTargetFromMemory(inTarget))
				{
					if (aim)
					{
						AiTargetBase.AimAtTarget();
					}
					return MyBehaviorTreeState.SUCCESS;
				}
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("FindCharacterInRadius", ReturnsRunning = false)]
		protected MyBehaviorTreeState FindCharacterInRadius([BTParam] int radius, [BTOut] ref MyBBMemoryTarget outCharacter)
		{
			MyCharacter myCharacter = FindCharacterInRadius(radius);
			if (myCharacter != null)
			{
				MyBBMemoryTarget.SetTargetEntity(ref outCharacter, MyAiTargetEnum.CHARACTER, myCharacter.EntityId);
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("IsCharacterInRadius", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsCharacterInRadius([BTParam] int radius)
		{
			MyCharacter myCharacter = FindCharacterInRadius(radius);
			if (myCharacter != null && !myCharacter.IsDead)
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("IsNoCharacterInRadius", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsNoCharacterInRadius([BTParam] int radius)
		{
			MyCharacter myCharacter = FindCharacterInRadius(radius);
			if (myCharacter != null && !myCharacter.IsDead)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		protected MyCharacter FindCharacterInRadius(int radius, bool ignoreReachability = false)
		{
			Vector3D translation = Bot.Navigation.PositionAndOrientation.Translation;
			ICollection<MyPlayer> onlinePlayers = Sync.Players.GetOnlinePlayers();
			MyCharacter result = null;
			double num = 3.4028234663852886E+38;
			foreach (MyPlayer item in onlinePlayers)
			{
				if (item.Id.SerialId != 0)
				{
					MyHumanoidBot myHumanoidBot = MyAIComponent.Static.Bots.TryGetBot<MyHumanoidBot>(item.Id.SerialId);
					if (myHumanoidBot == null || myHumanoidBot.BotDefinition.BehaviorType == "Barbarian")
					{
						continue;
					}
				}
				if (item.Character != null && (ignoreReachability || AiTargetBase.IsEntityReachable(item.Character)) && !item.Character.IsDead)
				{
					double num2 = Vector3D.DistanceSquared(item.Character.PositionComp.GetPosition(), translation);
					if (num2 < (double)(radius * radius) && num2 < num)
					{
						result = item.Character;
						num = num2;
					}
				}
			}
			return result;
		}

		[MyBehaviorTreeAction("HasCharacter", ReturnsRunning = false)]
		protected MyBehaviorTreeState HasCharacter()
		{
			if (Bot.AgentEntity == null)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("CallMoveAndRotate")]
		protected MyBehaviorTreeState CallMoveAndRotate()
		{
			if (Bot.AgentEntity == null)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			Bot.AgentEntity.MoveAndRotate(Vector3.Zero, Vector2.One, 0f);
			return MyBehaviorTreeState.RUNNING;
		}

		[MyBehaviorTreeAction("ClearUnreachableEntities")]
		protected MyBehaviorTreeState ClearUnreachableEntities()
		{
			AiTargetBase.ClearUnreachableEntities();
			return MyBehaviorTreeState.SUCCESS;
		}
	}
}
