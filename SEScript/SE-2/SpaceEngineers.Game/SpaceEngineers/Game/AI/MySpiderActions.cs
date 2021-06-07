using Sandbox.Engine.Physics;
using Sandbox.Game.AI;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.AI;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
	[MyBehaviorDescriptor("Spider")]
	[BehaviorActionImpl(typeof(MySpiderLogic))]
	public class MySpiderActions : MyAgentActions
	{
		private MySpiderTarget SpiderTarget => base.AiTargetBase as MySpiderTarget;

		protected MySpiderLogic SpiderLogic => base.Bot.AgentLogic as MySpiderLogic;

		public MySpiderActions(MyAnimalBot bot)
			: base(bot)
		{
		}

		protected override MyBehaviorTreeState Idle()
		{
			return MyBehaviorTreeState.RUNNING;
		}

		[MyBehaviorTreeAction("Burrow", MyBehaviorTreeActionType.INIT)]
		protected void Init_Burrow()
		{
			SpiderLogic.StartBurrowing();
		}

		[MyBehaviorTreeAction("Burrow")]
		protected MyBehaviorTreeState Burrow()
		{
			if (SpiderLogic.IsBurrowing)
			{
				return MyBehaviorTreeState.RUNNING;
			}
			return MyBehaviorTreeState.NOT_TICKED;
		}

		[MyBehaviorTreeAction("Deburrow", MyBehaviorTreeActionType.INIT)]
		protected void Init_Deburrow()
		{
			SpiderLogic.StartDeburrowing();
		}

		[MyBehaviorTreeAction("Deburrow")]
		protected MyBehaviorTreeState Deburrow()
		{
			if (SpiderLogic.IsDeburrowing)
			{
				return MyBehaviorTreeState.RUNNING;
			}
			return MyBehaviorTreeState.NOT_TICKED;
		}

		[MyBehaviorTreeAction("Teleport", ReturnsRunning = false)]
		protected MyBehaviorTreeState Teleport()
		{
			if (base.Bot.Player.Character.HasAnimation("Deburrow"))
			{
				base.Bot.Player.Character.PlayCharacterAnimation("Deburrow", MyBlendOption.Immediate, MyFrameOption.JustFirstFrame, 0f, 1f, sync: true);
				base.Bot.AgentEntity.DisableAnimationCommands();
			}
			if (!MySpaceBotFactory.GetSpiderSpawnPosition(out MatrixD spawnPosition, base.Bot.Player.GetPosition()))
			{
				return MyBehaviorTreeState.FAILURE;
			}
			Vector3D position = spawnPosition.Translation;
			if (MyPhysics.CastRay(position + 3.0 * base.Bot.AgentEntity.WorldMatrix.Up, position - 3.0 * base.Bot.AgentEntity.WorldMatrix.Up, 9).HasValue)
			{
				return MyBehaviorTreeState.NOT_TICKED;
			}
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(base.Bot.AgentEntity.WorldMatrix.Translation - 3.0 * base.Bot.AgentEntity.WorldMatrix.Up, base.Bot.AgentEntity.WorldMatrix.Translation + 3.0 * base.Bot.AgentEntity.WorldMatrix.Up, 9);
			if (hitInfo.HasValue && ((IHitInfo)hitInfo).HitEntity != base.Bot.AgentEntity)
			{
				hitInfo = MyPhysics.CastRay(base.Bot.AgentEntity.WorldMatrix.Translation - 3.0 * base.Bot.AgentEntity.WorldMatrix.Up, base.Bot.AgentEntity.WorldMatrix.Translation + 3.0 * base.Bot.AgentEntity.WorldMatrix.Up, 9);
				return MyBehaviorTreeState.NOT_TICKED;
			}
			float num = (float)base.Bot.AgentEntity.PositionComp.WorldVolume.Radius;
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
			if (closestPlanet != null)
			{
				closestPlanet.CorrectSpawnLocation(ref position, num);
				spawnPosition.Translation = position;
			}
			else
			{
				Vector3D? vector3D = MyEntities.FindFreePlace(spawnPosition.Translation, num, 20, 5, 0.2f);
				if (vector3D.HasValue)
				{
					spawnPosition.Translation = vector3D.Value;
				}
			}
			base.Bot.AgentEntity.SetPhysicsEnabled(enabled: false);
			base.Bot.AgentEntity.WorldMatrix = spawnPosition;
			base.Bot.AgentEntity.Physics.CharacterProxy.SetForwardAndUp(spawnPosition.Forward, spawnPosition.Up);
			base.Bot.AgentEntity.SetPhysicsEnabled(enabled: true);
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("Attack", MyBehaviorTreeActionType.INIT)]
		protected void Init_Attack()
		{
			SpiderTarget.AimAtTarget();
			((Vector3)(SpiderTarget.TargetPosition - base.Bot.AgentEntity.PositionComp.GetPosition())).Normalize();
			SpiderTarget.Attack();
		}

		[MyBehaviorTreeAction("Attack")]
		protected MyBehaviorTreeState Attack()
		{
			if (!SpiderTarget.IsAttacking)
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
			if (!SpiderTarget.IsAttacking)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("GetTargetWithPriority")]
		protected MyBehaviorTreeState GetTargetWithPriority([BTParam] float radius, [BTInOut] ref MyBBMemoryTarget outTarget, [BTInOut] ref MyBBMemoryInt priority)
		{
			Vector3D translation = base.Bot.Navigation.PositionAndOrientation.Translation;
			BoundingSphereD boundingSphere = new BoundingSphereD(translation, radius);
			if (priority == null)
			{
				priority = new MyBBMemoryInt();
			}
			int num = priority.IntValue;
			if (num <= 0)
			{
				num = int.MaxValue;
			}
			MyBehaviorTreeState myBehaviorTreeState = IsTargetValid(ref outTarget);
			if (myBehaviorTreeState == MyBehaviorTreeState.FAILURE)
			{
				num = 7;
				MyBBMemoryTarget.UnsetTarget(ref outTarget);
			}
			Vector3D? memoryTargetPosition = SpiderTarget.GetMemoryTargetPosition(outTarget);
			if (!memoryTargetPosition.HasValue || Vector3D.Distance(memoryTargetPosition.Value, base.Bot.AgentEntity.PositionComp.GetPosition()) > 400.0)
			{
				num = 7;
				MyBBMemoryTarget.UnsetTarget(ref outTarget);
			}
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(base.Bot.AgentEntity.ControllerInfo.ControllingIdentityId);
			List<MyEntity> topMostEntitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
			topMostEntitiesInSphere.ShuffleList();
			foreach (MyEntity item in topMostEntitiesInSphere)
			{
				if (item != base.Bot.AgentEntity && SpiderTarget.IsEntityReachable(item))
				{
					int num2 = 6;
					MyCharacter myCharacter = item as MyCharacter;
					if (myCharacter != null && myCharacter.ControllerInfo != null)
					{
						MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
						if ((playerFaction == null || playerFaction2 != playerFaction) && !myCharacter.IsDead)
						{
							MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(myCharacter.WorldMatrix.Translation - 3.0 * myCharacter.WorldMatrix.Up, myCharacter.WorldMatrix.Translation + 3.0 * myCharacter.WorldMatrix.Up, 15);
							if (hitInfo.HasValue && ((IHitInfo)hitInfo).HitEntity != myCharacter)
							{
								num2 = 1;
								if (num2 < num)
								{
									myBehaviorTreeState = MyBehaviorTreeState.SUCCESS;
									num = num2;
									MyBBMemoryTarget.SetTargetEntity(ref outTarget, MyAiTargetEnum.CHARACTER, myCharacter.EntityId);
								}
							}
						}
					}
				}
			}
			topMostEntitiesInSphere.Clear();
			priority.IntValue = num;
			return myBehaviorTreeState;
		}
	}
}
