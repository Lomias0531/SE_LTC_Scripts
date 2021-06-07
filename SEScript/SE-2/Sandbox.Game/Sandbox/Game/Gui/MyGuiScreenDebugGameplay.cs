using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Game;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Gameplay")]
	[StaticEventOwner]
	internal class MyGuiScreenDebugGameplay : MyGuiScreenDebugBase
	{
		[Serializable]
		public struct MyStationDebugDrawStructure
		{
			protected class Sandbox_Game_Gui_MyGuiScreenDebugGameplay_003C_003EMyStationDebugDrawStructure_003C_003EStart_003C_003EAccessor : IMemberAccessor<MyStationDebugDrawStructure, SerializableVector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStationDebugDrawStructure owner, in SerializableVector3D value)
				{
					owner.Start = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStationDebugDrawStructure owner, out SerializableVector3D value)
				{
					value = owner.Start;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenDebugGameplay_003C_003EMyStationDebugDrawStructure_003C_003EEnd_003C_003EAccessor : IMemberAccessor<MyStationDebugDrawStructure, SerializableVector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStationDebugDrawStructure owner, in SerializableVector3D value)
				{
					owner.End = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStationDebugDrawStructure owner, out SerializableVector3D value)
				{
					value = owner.End;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenDebugGameplay_003C_003EMyStationDebugDrawStructure_003C_003ETypeId_003C_003EAccessor : IMemberAccessor<MyStationDebugDrawStructure, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStationDebugDrawStructure owner, in int value)
				{
					owner.TypeId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStationDebugDrawStructure owner, out int value)
				{
					value = owner.TypeId;
				}
			}

			public SerializableVector3D Start;

			public SerializableVector3D End;

			public int TypeId;
		}

		protected sealed class ForceClusterReorderRequest_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ForceClusterReorderRequest();
			}
		}

		protected sealed class UpdateEconomyRequest_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateEconomyRequest();
			}
		}

		protected sealed class RequestStationPositions_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestStationPositions();
			}
		}

		protected sealed class DrawStationsClient_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Gui_MyGuiScreenDebugGameplay_003C_003EMyStationDebugDrawStructure_003E : ICallSite<IMyEventOwner, List<MyStationDebugDrawStructure>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<MyStationDebugDrawStructure> stations, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DrawStationsClient(stations);
			}
		}

		protected sealed class SetDepletionMultiplier_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float multiplier, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetDepletionMultiplier(multiplier);
			}
		}

		protected sealed class SetDepletionMultiplierSuccess_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float multiplier, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetDepletionMultiplierSuccess(multiplier);
			}
		}

		protected sealed class SetFuelConsumptionMultiplier_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float multiplier, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetFuelConsumptionMultiplier(multiplier);
			}
		}

		protected sealed class SetFuelConsumptionMultiplierSuccess_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float multiplier, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetFuelConsumptionMultiplierSuccess(multiplier);
			}
		}

		private const float TWO_BUTTON_XOFFSET = 0.05f;

		public MyGuiScreenDebugGameplay()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Gameplay", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddVerticalSpacing(0.01f * m_scale);
			AddCheckBox("Debris enabled", null, MemberHelper.GetMember(() => MyFakes.ENABLE_DEBRIS));
			AddCheckBox("Drill rocks enabled", null, MemberHelper.GetMember(() => MyFakes.ENABLE_DRILL_ROCKS));
			AddCheckBox("Revoke Game Inventory Items", null, MemberHelper.GetMember(() => MySessionComponentGameInventory.DEBUG_REVOKE_ITEM_OWNERSHIP));
			AddVerticalSpacing(0.02f);
			if (MySession.Static?.LocalCharacter != null)
			{
				MyCharacter character = MySession.Static.LocalCharacter;
				AddLabel("Local Character", Color.Yellow, 1f);
				AddVerticalSpacing();
				MyCharacterStatComponent stats = character.StatComp;
				if (stats != null)
				{
					AddSlider("Health", stats.Health.MinValue, stats.Health.MaxValue, stats.Health.MaxValue, () => stats.Health.Value, delegate(float value)
					{
						stats.Health.Value = value;
					});
				}
				MyCharacterOxygenComponent oxygen = character.OxygenComponent;
				if (oxygen != null)
				{
					MyDefinitionId oxygenId = MyCharacterOxygenComponent.OxygenId;
					AddSlider("Oxygen", 0f, 100f, 100f, () => oxygen.GetGasFillLevel(MyCharacterOxygenComponent.OxygenId) * 100f, delegate(float value)
					{
						oxygen.UpdateStoredGasLevel(ref oxygenId, value * 0.01f);
					});
					MyDefinitionId hydrogenId = MyCharacterOxygenComponent.HydrogenId;
					AddSlider("Hydrogen", 0f, 100f, 100f, () => oxygen.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId) * 100f, delegate(float value)
					{
						oxygen.UpdateStoredGasLevel(ref hydrogenId, value * 0.01f);
					});
				}
				MyBattery energy = character.SuitBattery;
				if (energy != null)
				{
					AddSlider("Energy", 0f, 100f, 100f, () => energy.ResourceSource.RemainingCapacity / 1E-05f * 100f, delegate(float value)
					{
						energy.ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, value * 1E-07f);
					});
				}
				AddVerticalSpacing(0.02f);
				AddButton("Set toolbar to shared", delegate
				{
					MySession.Static.SharedToolbar = character.ControlSteamId;
				});
			}
			else
			{
				AddLabel("No Character present", Color.Orange, 1f);
			}
			AddVerticalSpacing(0.02f);
			AddSlider("Battery Depletion Multiplier", 0f, 100f, 1f, () => MyBattery.BATTERY_DEPLETION_MULTIPLIER, SetDepletionMultiplierLocal);
			AddSlider("Reactor Fuel Consumption Multiplier", 0f, 100f, 1f, () => MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER, SetFuelConsumptionMultiplierLocal);
			AddVerticalSpacing(0.02f);
			MyCubeGrid grid = (MySession.Static?.ControlledEntity?.Entity as MyCubeGrid) ?? (MySession.Static?.ControlledEntity?.Entity as MyCockpit)?.CubeGrid;
			if (grid?.Physics != null)
			{
				AddLabel("Controlled Ship", Color.Yellow, 1f);
				AddVerticalSpacing();
				Vector2 currentPosition = m_currentPosition;
				m_buttonXOffset = -0.05f;
				AddButton("Stop", delegate
				{
					grid.Physics.LinearVelocity = Vector3.Zero;
				});
				m_currentPosition = currentPosition;
				m_buttonXOffset = 0.05f;
				Vector3 velocityVector = (!Vector3.IsZero(grid.Physics.LinearVelocity)) ? Vector3.Normalize(grid.Physics.LinearVelocity) : Vector3.Forward;
				AddButton("Max Speed", delegate
				{
					SetMaxSpeed(grid, velocityVector);
				});
				if (grid.GridSizeEnum == MyCubeSize.Large)
				{
					currentPosition = m_currentPosition;
					m_buttonXOffset = -0.05f;
					AddButton("Recharge Jump Drives", delegate
					{
						RechargeJumpDrives(grid);
					});
					m_currentPosition = currentPosition;
					m_buttonXOffset = 0.05f;
					AddButton("Discharge Jump Drives", delegate
					{
						DischargeJumpDrives(grid);
					});
				}
				m_buttonXOffset = 0f;
			}
			else
			{
				AddLabel("No Ship is being controlled", Color.Orange, 1f);
			}
			AddVerticalSpacing(0.02f);
			AddSlider("Container Drop Multiplier", 1f, 10f, 1f, GetRespawnTimeMultiplier, SetRespawnTimeMultiplier);
			AddVerticalSpacing();
			AddButton("Trigger Meteor Shower", delegate
			{
				TriggerMeteorShower();
			});
			AddButton("Trigger Cargo Ship", delegate
			{
				TriggerCargoShip();
			});
			AddVerticalSpacing(0.02f);
			AddButton("Force cluster reorder", delegate
			{
				ForceClusterReorder();
			});
			AddButton("Draw positions of stations", delegate
			{
				DrawStations();
			});
			AddButton("Force economy update", delegate
			{
				UpdateEconomy();
			});
		}

		private void ForceClusterReorder()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ForceClusterReorderRequest);
		}

		[Event(null, 154)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void ForceClusterReorderRequest()
		{
			ForceClusterReorderInternal();
		}

		private static void ForceClusterReorderInternal()
		{
			MyFakes.FORCE_CLUSTER_REORDER = true;
		}

		private void UpdateEconomy()
		{
			if (MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer)
			{
				UpdateEconomyInternal();
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => UpdateEconomyRequest);
			}
		}

		[Event(null, 177)]
		[Reliable]
		[Server]
		private static void UpdateEconomyRequest()
		{
			UpdateEconomyInternal();
		}

		private static void UpdateEconomyInternal()
		{
			MySession.Static.GetComponent<MySessionComponentEconomy>()?.ForceEconomyTick();
		}

		private static void DrawStations()
		{
			if (MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer)
			{
				if (MySession.Static.Factions != null)
				{
					foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
					{
						foreach (MyStation station in faction.Value.Stations)
						{
							switch (station.Type)
							{
							case MyStationTypeEnum.MiningStation:
								MyRenderProxy.DebugDrawSphere(station.Position, 150f, Color.Red, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
								MyRenderProxy.DebugDrawLine3D(station.Position, Vector3D.Zero, Color.Red, Color.Red, depthRead: false, persistent: true);
								break;
							case MyStationTypeEnum.OrbitalStation:
							{
								Vector3D pointTo = MyGamePruningStructure.GetClosestPlanet(station.Position)?.PositionComp.GetPosition() ?? Vector3D.Zero;
								MyRenderProxy.DebugDrawSphere(station.Position, 150f, Color.CornflowerBlue, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
								MyRenderProxy.DebugDrawLine3D(station.Position, pointTo, Color.CornflowerBlue, Color.CornflowerBlue, depthRead: false, persistent: true);
								break;
							}
							case MyStationTypeEnum.Outpost:
							{
								Vector3D pointTo2 = MyGamePruningStructure.GetClosestPlanet(station.Position)?.PositionComp.GetPosition() ?? Vector3D.Zero;
								MyRenderProxy.DebugDrawSphere(station.Position, 150f, Color.Yellow, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
								MyRenderProxy.DebugDrawLine3D(station.Position, pointTo2, Color.Yellow, Color.Yellow, depthRead: false, persistent: true);
								break;
							}
							case MyStationTypeEnum.SpaceStation:
							{
								Color color = (!station.IsDeepSpaceStation) ? Color.Green : Color.Purple;
								MyRenderProxy.DebugDrawSphere(station.Position, 150f, color, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
								MyRenderProxy.DebugDrawLine3D(station.Position, Vector3D.Zero, color, color, depthRead: false, persistent: true);
								break;
							}
							}
						}
					}
				}
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RequestStationPositions);
			}
		}

		[Event(null, 265)]
		[Reliable]
		[Server]
		private static void RequestStationPositions()
		{
			if ((MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer) && MySession.Static.Factions != null)
			{
				List<MyStationDebugDrawStructure> list = new List<MyStationDebugDrawStructure>();
				foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
				{
					foreach (MyStation station in faction.Value.Stations)
					{
						MyStationDebugDrawStructure item = default(MyStationDebugDrawStructure);
						item.Start = Vector3D.Zero;
						item.End = station.Position;
						switch (station.Type)
						{
						case MyStationTypeEnum.MiningStation:
							item.TypeId = 0;
							break;
						case MyStationTypeEnum.OrbitalStation:
						{
							item.TypeId = 1;
							MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(station.Position);
							if (closestPlanet != null)
							{
								item.Start = closestPlanet.PositionComp.GetPosition();
							}
							break;
						}
						case MyStationTypeEnum.Outpost:
						{
							item.TypeId = 2;
							MyPlanet closestPlanet2 = MyGamePruningStructure.GetClosestPlanet(station.Position);
							if (closestPlanet2 != null)
							{
								item.Start = closestPlanet2.PositionComp.GetPosition();
							}
							break;
						}
						case MyStationTypeEnum.SpaceStation:
							if (station.IsDeepSpaceStation)
							{
								item.TypeId = 3;
							}
							else
							{
								item.TypeId = 4;
							}
							break;
						}
						list.Add(item);
					}
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => DrawStationsClient, list, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 323)]
		[Reliable]
		[Client]
		private static void DrawStationsClient(List<MyStationDebugDrawStructure> stations)
		{
			foreach (MyStationDebugDrawStructure station in stations)
			{
				Color color;
				switch (station.TypeId)
				{
				case 0:
					color = Color.Red;
					break;
				case 1:
					color = Color.CornflowerBlue;
					break;
				case 2:
					color = Color.Yellow;
					break;
				case 3:
					color = Color.Purple;
					break;
				case 4:
					color = Color.Green;
					break;
				default:
					color = Color.Pink;
					break;
				}
				MyRenderProxy.DebugDrawSphere(station.End, 150f, color, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
				MyRenderProxy.DebugDrawLine3D(station.End, station.Start, color, color, depthRead: false, persistent: true);
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugGameplay";
		}

		private static void SetMaxSpeed(MyCubeGrid grid, Vector3 direction)
		{
			if (grid?.Physics != null)
			{
				if (grid.GridSizeEnum == MyCubeSize.Large)
				{
					grid.Physics.LinearVelocity = direction * MySector.EnvironmentDefinition.LargeShipMaxSpeed;
				}
				else
				{
					grid.Physics.LinearVelocity = direction * MySector.EnvironmentDefinition.SmallShipMaxSpeed;
				}
			}
		}

		private static void RechargeJumpDrives(MyCubeGrid grid)
		{
			if (grid != null)
			{
				foreach (MyJumpDrive fatBlock in grid.GetFatBlocks<MyJumpDrive>())
				{
					fatBlock.SetStoredPower(1f);
				}
			}
		}

		private static void DischargeJumpDrives(MyCubeGrid grid)
		{
			if (grid != null)
			{
				foreach (MyJumpDrive fatBlock in grid.GetFatBlocks<MyJumpDrive>())
				{
					fatBlock.SetStoredPower(0f);
				}
			}
		}

		private static void SetRespawnTimeMultiplier(float multiplier)
		{
		}

		private static float GetRespawnTimeMultiplier()
		{
			return (1f / MySession.Static?.GetComponent<MySessionComponentContainerDropSystem>()?.GetRespawnTimeMultiplier()) ?? 1f;
		}

		private static void TriggerMeteorShower()
		{
			MyGlobalEventBase myGlobalEventBase = MyGlobalEventFactory.CreateEvent(new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "MeteorWave"));
			MyGlobalEvents.RemoveGlobalEvent(myGlobalEventBase);
			myGlobalEventBase.SetActivationTime(TimeSpan.Zero);
			MyGlobalEvents.AddGlobalEvent(myGlobalEventBase);
		}

		private static void TriggerCargoShip()
		{
			MyGlobalEventBase eventById = MyGlobalEvents.GetEventById(new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "SpawnCargoShip"));
			MyGlobalEvents.RemoveGlobalEvent(eventById);
			eventById.SetActivationTime(TimeSpan.Zero);
			MyGlobalEvents.AddGlobalEvent(eventById);
			MyHud.Notifications.Add(new MyHudNotificationDebug("Cargo ship will spawn soon™", 5000));
		}

		private static void SetDepletionMultiplierLocal(float multiplier)
		{
			MyBattery.BATTERY_DEPLETION_MULTIPLIER = multiplier;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetDepletionMultiplier, MyBattery.BATTERY_DEPLETION_MULTIPLIER);
		}

		[Event(null, 429)]
		[Reliable]
		[Server]
		private static void SetDepletionMultiplier(float multiplier)
		{
			MyBattery.BATTERY_DEPLETION_MULTIPLIER = multiplier;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetDepletionMultiplierSuccess, MyBattery.BATTERY_DEPLETION_MULTIPLIER);
		}

		[Event(null, 437)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void SetDepletionMultiplierSuccess(float multiplier)
		{
			MyBattery.BATTERY_DEPLETION_MULTIPLIER = multiplier;
		}

		private static void SetFuelConsumptionMultiplierLocal(float multiplier)
		{
			MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER = multiplier;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetFuelConsumptionMultiplier, MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER);
		}

		[Event(null, 450)]
		[Reliable]
		[Server]
		private static void SetFuelConsumptionMultiplier(float multiplier)
		{
			MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER = multiplier;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetFuelConsumptionMultiplierSuccess, MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER);
		}

		[Event(null, 458)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void SetFuelConsumptionMultiplierSuccess(float multiplier)
		{
			MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER = multiplier;
		}
	}
}
