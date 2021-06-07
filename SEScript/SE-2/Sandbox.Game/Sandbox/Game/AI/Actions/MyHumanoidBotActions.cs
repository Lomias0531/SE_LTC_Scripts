using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.AI;
using VRage.Library.Utils;
using VRage.ObjectBuilders;

namespace Sandbox.Game.AI.Actions
{
	public abstract class MyHumanoidBotActions : MyAgentActions
	{
		private MyTimeSpan m_reservationTimeOut;

		private const int RESERVATION_WAIT_TIMEOUT_SECONDS = 3;

		protected new MyHumanoidBot Bot => base.Bot as MyHumanoidBot;

		public MyHumanoidBotActions(MyHumanoidBot humanoidBot)
			: base(humanoidBot)
		{
		}

		[MyBehaviorTreeAction("PlaySound", ReturnsRunning = false)]
		protected MyBehaviorTreeState PlaySound([BTParam] string soundName)
		{
			Bot.HumanoidEntity.SoundComp.StartSecondarySound(soundName, sync: true);
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("EquipItem", ReturnsRunning = false)]
		protected MyBehaviorTreeState EquipItem([BTParam] string itemName)
		{
			if (string.IsNullOrEmpty(itemName))
			{
				return MyBehaviorTreeState.FAILURE;
			}
			MyCharacter humanoidEntity = Bot.HumanoidEntity;
			if (humanoidEntity.CurrentWeapon != null && humanoidEntity.CurrentWeapon.DefinitionId.SubtypeName == itemName)
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			MyObjectBuilder_PhysicalGunObject myObjectBuilder_PhysicalGunObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PhysicalGunObject>(itemName);
			MyDefinitionId id = myObjectBuilder_PhysicalGunObject.GetId();
			if (humanoidEntity.GetInventory().ContainItems(1, myObjectBuilder_PhysicalGunObject) || !humanoidEntity.WeaponTakesBuilderFromInventory(id))
			{
				humanoidEntity.SwitchToWeapon(id);
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		private void ReservationHandler(ref MyAiTargetManager.ReservedEntityData reservedEntity, bool success)
		{
			if (Bot != null && Bot.HumanoidLogic != null && Bot.Player != null && Bot.Player.Id.SerialId == reservedEntity.ReserverId.SerialId)
			{
				MyHumanoidBotLogic humanoidLogic = Bot.HumanoidLogic;
				humanoidLogic.ReservationStatus = MyReservationStatus.FAILURE;
				if (success && reservedEntity.EntityId == humanoidLogic.ReservationEntityData.EntityId && (reservedEntity.Type != MyReservedEntityType.ENVIRONMENT_ITEM || reservedEntity.LocalId == humanoidLogic.ReservationEntityData.LocalId) && (reservedEntity.Type != MyReservedEntityType.VOXEL || !(reservedEntity.GridPos != humanoidLogic.ReservationEntityData.GridPos)))
				{
					humanoidLogic.ReservationStatus = MyReservationStatus.SUCCESS;
				}
			}
		}

		private void AreaReservationHandler(ref MyAiTargetManager.ReservedAreaData reservedArea, bool success)
		{
			if (Bot != null && Bot.HumanoidLogic != null && Bot.Player != null && Bot.Player.Id.SerialId == reservedArea.ReserverId.SerialId)
			{
				MyHumanoidBotLogic humanoidLogic = Bot.HumanoidLogic;
				humanoidLogic.ReservationStatus = MyReservationStatus.FAILURE;
				if (success && reservedArea.WorldPosition == humanoidLogic.ReservationAreaData.WorldPosition && reservedArea.Radius == humanoidLogic.ReservationAreaData.Radius)
				{
					humanoidLogic.ReservationStatus = MyReservationStatus.SUCCESS;
				}
			}
		}

		[MyBehaviorTreeAction("TryReserveEntity")]
		protected MyBehaviorTreeState TryReserveEntity([BTIn] ref MyBBMemoryTarget inTarget, [BTParam] int timeMs)
		{
			if (Bot == null || Bot.Player == null)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			MyHumanoidBotLogic humanoidLogic = Bot.HumanoidLogic;
			if (inTarget != null && inTarget.EntityId.HasValue && inTarget.TargetType != MyAiTargetEnum.POSITION && inTarget.TargetType != 0)
			{
				switch (humanoidLogic.ReservationStatus)
				{
				case MyReservationStatus.NONE:
				{
					MyAiTargetManager.ReservedEntityData reservationEntityData;
					switch (inTarget.TargetType)
					{
					case MyAiTargetEnum.GRID:
					case MyAiTargetEnum.CUBE:
					case MyAiTargetEnum.CHARACTER:
					case MyAiTargetEnum.ENTITY:
						humanoidLogic.ReservationStatus = MyReservationStatus.WAITING;
						reservationEntityData = new MyAiTargetManager.ReservedEntityData
						{
							Type = MyReservedEntityType.ENTITY,
							EntityId = inTarget.EntityId.Value,
							ReservationTimer = timeMs,
							ReserverId = new MyPlayer.PlayerId(Bot.Player.Id.SteamId, Bot.Player.Id.SerialId)
						};
						humanoidLogic.ReservationEntityData = reservationEntityData;
						MyAiTargetManager.OnReservationResult += ReservationHandler;
						MyAiTargetManager.Static.RequestEntityReservation(humanoidLogic.ReservationEntityData.EntityId, humanoidLogic.ReservationEntityData.ReservationTimer, Bot.Player.Id.SerialId);
						break;
					case MyAiTargetEnum.ENVIRONMENT_ITEM:
						humanoidLogic.ReservationStatus = MyReservationStatus.WAITING;
						reservationEntityData = new MyAiTargetManager.ReservedEntityData
						{
							Type = MyReservedEntityType.ENVIRONMENT_ITEM,
							EntityId = inTarget.EntityId.Value,
							LocalId = inTarget.TreeId.Value,
							ReservationTimer = timeMs,
							ReserverId = new MyPlayer.PlayerId(Bot.Player.Id.SteamId, Bot.Player.Id.SerialId)
						};
						humanoidLogic.ReservationEntityData = reservationEntityData;
						MyAiTargetManager.OnReservationResult += ReservationHandler;
						MyAiTargetManager.Static.RequestEnvironmentItemReservation(humanoidLogic.ReservationEntityData.EntityId, humanoidLogic.ReservationEntityData.LocalId, humanoidLogic.ReservationEntityData.ReservationTimer, Bot.Player.Id.SerialId);
						break;
					case MyAiTargetEnum.VOXEL:
						humanoidLogic.ReservationStatus = MyReservationStatus.WAITING;
						reservationEntityData = new MyAiTargetManager.ReservedEntityData
						{
							Type = MyReservedEntityType.VOXEL,
							EntityId = inTarget.EntityId.Value,
							GridPos = inTarget.VoxelPosition,
							ReservationTimer = timeMs,
							ReserverId = new MyPlayer.PlayerId(Bot.Player.Id.SteamId, Bot.Player.Id.SerialId)
						};
						humanoidLogic.ReservationEntityData = reservationEntityData;
						MyAiTargetManager.OnReservationResult += ReservationHandler;
						MyAiTargetManager.Static.RequestVoxelPositionReservation(humanoidLogic.ReservationEntityData.EntityId, humanoidLogic.ReservationEntityData.GridPos, humanoidLogic.ReservationEntityData.ReservationTimer, Bot.Player.Id.SerialId);
						break;
					default:
						humanoidLogic.ReservationStatus = MyReservationStatus.FAILURE;
						break;
					}
					m_reservationTimeOut = MySandboxGame.Static.TotalTime + MyTimeSpan.FromSeconds(3.0);
					break;
				}
				case MyReservationStatus.WAITING:
					if (m_reservationTimeOut < MySandboxGame.Static.TotalTime)
					{
						humanoidLogic.ReservationStatus = MyReservationStatus.FAILURE;
					}
					break;
				}
			}
			switch (humanoidLogic.ReservationStatus)
			{
			case MyReservationStatus.WAITING:
				return MyBehaviorTreeState.RUNNING;
			case MyReservationStatus.SUCCESS:
				return MyBehaviorTreeState.SUCCESS;
			default:
				return MyBehaviorTreeState.FAILURE;
			}
		}

		[MyBehaviorTreeAction("TryReserveEntity", MyBehaviorTreeActionType.POST)]
		protected void Post_TryReserveEntity()
		{
			if (Bot != null && Bot.HumanoidLogic != null)
			{
				MyHumanoidBotLogic humanoidLogic = Bot.HumanoidLogic;
				if (humanoidLogic.ReservationStatus != 0)
				{
					MyAiTargetManager.OnReservationResult -= ReservationHandler;
				}
				humanoidLogic.ReservationStatus = MyReservationStatus.NONE;
			}
		}

		[MyBehaviorTreeAction("TryReserveArea")]
		protected MyBehaviorTreeState TryReserveAreaAroundEntity([BTParam] string areaName, [BTParam] float radius, [BTParam] int timeMs)
		{
			MyHumanoidBotLogic humanoidLogic = Bot.HumanoidLogic;
			MyBehaviorTreeState result = MyBehaviorTreeState.FAILURE;
			if (humanoidLogic != null)
			{
				switch (humanoidLogic.ReservationStatus)
				{
				case MyReservationStatus.NONE:
					humanoidLogic.ReservationStatus = MyReservationStatus.WAITING;
					humanoidLogic.ReservationAreaData = new MyAiTargetManager.ReservedAreaData
					{
						WorldPosition = Bot.HumanoidEntity.WorldMatrix.Translation,
						Radius = radius,
						ReservationTimer = MyTimeSpan.FromMilliseconds(timeMs),
						ReserverId = new MyPlayer.PlayerId(Bot.Player.Id.SteamId, Bot.Player.Id.SerialId)
					};
					MyAiTargetManager.OnAreaReservationResult += AreaReservationHandler;
					MyAiTargetManager.Static.RequestAreaReservation(areaName, Bot.HumanoidEntity.WorldMatrix.Translation, radius, timeMs, Bot.Player.Id.SerialId);
					m_reservationTimeOut = MySandboxGame.Static.TotalTime + MyTimeSpan.FromSeconds(3.0);
					humanoidLogic.ReservationStatus = MyReservationStatus.WAITING;
					result = MyBehaviorTreeState.RUNNING;
					break;
				case MyReservationStatus.SUCCESS:
					result = MyBehaviorTreeState.SUCCESS;
					break;
				case MyReservationStatus.FAILURE:
					result = MyBehaviorTreeState.FAILURE;
					break;
				case MyReservationStatus.WAITING:
					result = ((!(m_reservationTimeOut < MySandboxGame.Static.TotalTime)) ? MyBehaviorTreeState.RUNNING : MyBehaviorTreeState.FAILURE);
					break;
				}
			}
			return result;
		}

		[MyBehaviorTreeAction("TryReserveArea", MyBehaviorTreeActionType.POST)]
		protected void Post_TryReserveArea()
		{
			if (Bot.HumanoidLogic != null)
			{
				MyHumanoidBotLogic humanoidLogic = Bot.HumanoidLogic;
				if (humanoidLogic.ReservationStatus != 0)
				{
					MyAiTargetManager.OnAreaReservationResult -= AreaReservationHandler;
				}
				humanoidLogic.ReservationStatus = MyReservationStatus.NONE;
			}
		}

		[MyBehaviorTreeAction("IsInReservedArea", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsInReservedArea([BTParam] string areaName)
		{
			if (MyAiTargetManager.Static.IsInReservedArea(areaName, Bot.HumanoidEntity.WorldMatrix.Translation))
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("IsNotInReservedArea", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsNotInReservedArea([BTParam] string areaName)
		{
			if (MyAiTargetManager.Static.IsInReservedArea(areaName, Bot.HumanoidEntity.WorldMatrix.Translation))
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}
	}
}
