using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.AI.Navigation;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.UseObject;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.AI;
using VRage.Groups;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_RemoteControl))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyRemoteControl),
		typeof(Sandbox.ModAPI.Ingame.IMyRemoteControl)
	})]
	public class MyRemoteControl : MyShipController, IMyUsableEntity, Sandbox.ModAPI.IMyRemoteControl, Sandbox.ModAPI.IMyShipController, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyShipController, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, Sandbox.ModAPI.Ingame.IMyRemoteControl
	{
		public class MyAutopilotWaypoint
		{
			public Vector3D Coords;

			public string Name;

			public MyToolbarItem[] Actions;

			public MyAutopilotWaypoint(Vector3D coords, string name, List<MyObjectBuilder_ToolbarItem> actionBuilders, List<int> indexes, MyRemoteControl owner)
			{
				Coords = coords;
				Name = name;
				if (actionBuilders == null)
				{
					return;
				}
				InitActions();
				bool flag = indexes != null && indexes.Count > 0;
				for (int i = 0; i < actionBuilders.Count; i++)
				{
					if (actionBuilders[i] != null)
					{
						if (flag)
						{
							Actions[indexes[i]] = MyToolbarItemFactory.CreateToolbarItem(actionBuilders[i]);
						}
						else
						{
							Actions[i] = MyToolbarItemFactory.CreateToolbarItem(actionBuilders[i]);
						}
					}
				}
			}

			public MyAutopilotWaypoint(Vector3D coords, string name, MyRemoteControl owner)
				: this(coords, name, null, null, owner)
			{
			}

			public MyAutopilotWaypoint(IMyGps gps, MyRemoteControl owner)
				: this(gps.Coords, gps.Name, null, null, owner)
			{
			}

			public MyAutopilotWaypoint(MyObjectBuilder_AutopilotWaypoint builder, MyRemoteControl owner)
				: this(builder.Coords, builder.Name, builder.Actions, builder.Indexes, owner)
			{
			}

			public void InitActions()
			{
				Actions = new MyToolbarItem[9];
			}

			public void SetActions(List<MyObjectBuilder_Toolbar.Slot> actionSlots)
			{
				Actions = new MyToolbarItem[9];
				for (int i = 0; i < actionSlots.Count; i++)
				{
					if (actionSlots[i].Data != null)
					{
						Actions[i] = MyToolbarItemFactory.CreateToolbarItem(actionSlots[i].Data);
					}
				}
			}

			public MyObjectBuilder_AutopilotWaypoint GetObjectBuilder()
			{
				MyObjectBuilder_AutopilotWaypoint myObjectBuilder_AutopilotWaypoint = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_AutopilotWaypoint>();
				myObjectBuilder_AutopilotWaypoint.Coords = Coords;
				myObjectBuilder_AutopilotWaypoint.Name = Name;
				if (Actions != null)
				{
					bool flag = false;
					MyToolbarItem[] actions = Actions;
					for (int i = 0; i < actions.Length; i++)
					{
						if (actions[i] != null)
						{
							flag = true;
						}
					}
					if (flag)
					{
						myObjectBuilder_AutopilotWaypoint.Actions = new List<MyObjectBuilder_ToolbarItem>();
						myObjectBuilder_AutopilotWaypoint.Indexes = new List<int>();
						for (int j = 0; j < Actions.Length; j++)
						{
							MyToolbarItem myToolbarItem = Actions[j];
							if (myToolbarItem != null)
							{
								myObjectBuilder_AutopilotWaypoint.Actions.Add(myToolbarItem.GetObjectBuilder());
								myObjectBuilder_AutopilotWaypoint.Indexes.Add(j);
							}
						}
					}
				}
				return myObjectBuilder_AutopilotWaypoint;
			}
		}

		private struct PlanetCoordInformation
		{
			public MyPlanet Planet;

			public double Elevation;

			public double Height;

			public Vector3D PlanetVector;

			public Vector3D GravityWorld;

			internal void Clear()
			{
				Planet = null;
				Elevation = 0.0;
				Height = 0.0;
				PlanetVector = Vector3D.Up;
				GravityWorld = Vector3D.Down;
			}

			internal bool IsValid()
			{
				return Planet != null;
			}

			internal void Calculate(Vector3D worldPoint)
			{
				Clear();
				MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(worldPoint);
				if (closestPlanet == null)
				{
					return;
				}
				Vector3D planetVector = worldPoint - closestPlanet.PositionComp.GetPosition();
				float gravityLimit = ((MySphericalNaturalGravityComponent)closestPlanet.Components.Get<MyGravityProviderComponent>()).GravityLimit;
				if (!(planetVector.Length() > (double)gravityLimit))
				{
					Planet = closestPlanet;
					PlanetVector = planetVector;
					if (!Vector3D.IsZero(PlanetVector))
					{
						GravityWorld = Vector3D.Normalize(Planet.Components.Get<MyGravityProviderComponent>().GetWorldGravity(worldPoint));
						Vector3 localPos = worldPoint - Planet.WorldMatrix.Translation;
						Vector3D closestSurfacePointLocal = Planet.GetClosestSurfacePointLocal(ref localPos);
						Height = Vector3D.Distance(localPos, closestSurfacePointLocal);
						Elevation = PlanetVector.Length();
						PlanetVector *= 1.0 / Elevation;
					}
				}
			}

			internal double EstimateDistanceToGround(Vector3D worldPoint)
			{
				return 0.0;
			}
		}

		public interface IRemoteControlAutomaticBehaviour
		{
			bool NeedUpdate
			{
				get;
			}

			bool IsActive
			{
				get;
			}

			bool RotateToTarget
			{
				get;
				set;
			}

			bool CollisionAvoidance
			{
				get;
				set;
			}

			float SpeedLimit
			{
				get;
				set;
			}

			int PlayerPriority
			{
				get;
				set;
			}

			float MaxPlayerDistance
			{
				get;
			}

			string CurrentBehavior
			{
				get;
			}

			TargetPrioritization PrioritizationStyle
			{
				get;
				set;
			}

			MyEntity CurrentTarget
			{
				get;
				set;
			}

			List<DroneTarget> TargetList
			{
				get;
			}

			List<MyEntity> WaypointList
			{
				get;
			}

			bool WaypointActive
			{
				get;
			}

			bool CycleWaypoints
			{
				get;
				set;
			}

			Vector3D OriginPoint
			{
				get;
				set;
			}

			float PlayerYAxisOffset
			{
				get;
			}

			float WaypointThresholdDistance
			{
				get;
			}

			bool ResetStuckDetection
			{
				get;
			}

			bool Ambushing
			{
				get;
				set;
			}

			bool Operational
			{
				get;
			}

			void Update();

			void WaypointAdvanced();

			void TargetAdd(DroneTarget target);

			void TargetClear();

			void TargetRemove(MyEntity target);

			void TargetLoseCurrent();

			void WaypointAdd(MyEntity waypoint);

			void WaypointClear();

			void StopWorking();

			void DebugDraw();

			void Load(MyObjectBuilder_AutomaticBehaviour objectBuilder, MyRemoteControl remoteControl);

			MyObjectBuilder_AutomaticBehaviour GetObjectBuilder();
		}

		public struct DetectedObject
		{
			public float Distance;

			public Vector3D Position;

			public bool IsVoxel;

			public DetectedObject(float dist, Vector3D pos, bool voxel)
			{
				Distance = dist;
				Position = pos;
				IsVoxel = voxel;
			}
		}

		private class MyDebugRenderComponentRemoteControl : MyDebugRenderComponent
		{
			private readonly MyRemoteControl m_remote;

			private MyAutopilotWaypoint m_prevWaypoint;

			public MyDebugRenderComponentRemoteControl(MyRemoteControl remote)
				: base(remote)
			{
				m_remote = remote;
			}

			public override void DebugDraw()
			{
			}
		}

		protected sealed class OnAddWaypoints_003C_003EVRageMath_Vector3D_003C_0023_003E_0023System_String_003C_0023_003E : ICallSite<MyRemoteControl, Vector3D[], string[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in Vector3D[] coords, in string[] names, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnAddWaypoints(coords, names);
			}
		}

		protected sealed class OnMoveWaypointsUp_003C_003ESystem_Collections_Generic_List_00601_003CSystem_Int32_003E : ICallSite<MyRemoteControl, List<int>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in List<int> indexes, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnMoveWaypointsUp(indexes);
			}
		}

		protected sealed class OnMoveWaypointsDown_003C_003ESystem_Collections_Generic_List_00601_003CSystem_Int32_003E : ICallSite<MyRemoteControl, List<int>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in List<int> indexes, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnMoveWaypointsDown(indexes);
			}
		}

		protected sealed class OnRemoveWaypoints_003C_003ESystem_Int32_003C_0023_003E : ICallSite<MyRemoteControl, int[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in int[] indexes, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveWaypoints(indexes);
			}
		}

		protected sealed class OnResetWaypoint_003C_003E : ICallSite<MyRemoteControl, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnResetWaypoint();
			}
		}

		protected sealed class OnPasteAutopilotSetup_003C_003ESandbox_Common_ObjectBuilders_MyObjectBuilder_AutopilotClipboard : ICallSite<MyRemoteControl, MyObjectBuilder_AutopilotClipboard, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in MyObjectBuilder_AutopilotClipboard clipboard, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnPasteAutopilotSetup(clipboard);
			}
		}

		protected sealed class ClearWaypoints_Implementation_003C_003E : ICallSite<MyRemoteControl, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ClearWaypoints_Implementation();
			}
		}

		protected sealed class OnAddWaypoint_003C_003EVRageMath_Vector3D_0023System_String : ICallSite<MyRemoteControl, Vector3D, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in Vector3D point, in string name, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnAddWaypoint(point, name);
			}
		}

		protected sealed class OnAddWaypoint_003C_003EVRageMath_Vector3D_0023System_String_0023System_Collections_Generic_List_00601_003CVRage_Game_MyObjectBuilder_ToolbarItem_003E : ICallSite<MyRemoteControl, Vector3D, string, List<MyObjectBuilder_ToolbarItem>, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in Vector3D point, in string name, in List<MyObjectBuilder_ToolbarItem> actionBuilders, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnAddWaypoint(point, name, actionBuilders);
			}
		}

		protected sealed class RequestUseMessage_003C_003EVRage_Game_Entity_UseObject_UseActionEnum_0023System_Int64 : ICallSite<MyRemoteControl, UseActionEnum, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in UseActionEnum useAction, in long usedById, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RequestUseMessage(useAction, usedById);
			}
		}

		protected sealed class UseSuccessCallback_003C_003EVRage_Game_Entity_UseObject_UseActionEnum_0023System_Int64_0023VRage_Game_Entity_UseObject_UseActionResult : ICallSite<MyRemoteControl, UseActionEnum, long, UseActionResult, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in UseActionEnum useAction, in long usedById, in UseActionResult useResult, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.UseSuccessCallback(useAction, usedById, useResult);
			}
		}

		protected sealed class UseFailureCallback_003C_003EVRage_Game_Entity_UseObject_UseActionEnum_0023System_Int64_0023VRage_Game_Entity_UseObject_UseActionResult : ICallSite<MyRemoteControl, UseActionEnum, long, UseActionResult, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in UseActionEnum useAction, in long usedById, in UseActionResult useResult, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.UseFailureCallback(useAction, usedById, useResult);
			}
		}

		protected sealed class RequestRelease_003C_003ESystem_Boolean : ICallSite<MyRemoteControl, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in bool previousClosed, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RequestRelease(previousClosed);
			}
		}

		protected sealed class OnToolbarItemChanged_003C_003ESandbox_Game_Entities_Blocks_ToolbarItem_0023System_Int32_0023System_Int32 : ICallSite<MyRemoteControl, ToolbarItem, int, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRemoteControl @this, in ToolbarItem item, in int index, in int waypointIndex, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnToolbarItemChanged(item, index, waypointIndex);
			}
		}

		protected class m_bindedCamera_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType bindedCamera;
				ISyncType result = bindedCamera = new Sync<long, SyncDirection.BothWays>(P_1, P_2);
				((MyRemoteControl)P_0).m_bindedCamera = (Sync<long, SyncDirection.BothWays>)bindedCamera;
				return result;
			}
		}

		protected class m_autopilotSpeedLimit_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType autopilotSpeedLimit;
				ISyncType result = autopilotSpeedLimit = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyRemoteControl)P_0).m_autopilotSpeedLimit = (Sync<float, SyncDirection.BothWays>)autopilotSpeedLimit;
				return result;
			}
		}

		protected class m_useCollisionAvoidance_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useCollisionAvoidance;
				ISyncType result = useCollisionAvoidance = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyRemoteControl)P_0).m_useCollisionAvoidance = (Sync<bool, SyncDirection.BothWays>)useCollisionAvoidance;
				return result;
			}
		}

		protected class m_autoPilotEnabled_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType autoPilotEnabled;
				ISyncType result = autoPilotEnabled = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyRemoteControl)P_0).m_autoPilotEnabled = (Sync<bool, SyncDirection.BothWays>)autoPilotEnabled;
				return result;
			}
		}

		protected class m_dockingModeEnabled_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType dockingModeEnabled;
				ISyncType result = dockingModeEnabled = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyRemoteControl)P_0).m_dockingModeEnabled = (Sync<bool, SyncDirection.BothWays>)dockingModeEnabled;
				return result;
			}
		}

		protected class m_currentFlightMode_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType currentFlightMode;
				ISyncType result = currentFlightMode = new Sync<FlightMode, SyncDirection.BothWays>(P_1, P_2);
				((MyRemoteControl)P_0).m_currentFlightMode = (Sync<FlightMode, SyncDirection.BothWays>)currentFlightMode;
				return result;
			}
		}

		protected class m_waitForFreeWay_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType waitForFreeWay;
				ISyncType result = waitForFreeWay = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyRemoteControl)P_0).m_waitForFreeWay = (Sync<bool, SyncDirection.BothWays>)waitForFreeWay;
				return result;
			}
		}

		protected class m_currentDirection_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType currentDirection;
				ISyncType result = currentDirection = new Sync<Base6Directions.Direction, SyncDirection.BothWays>(P_1, P_2);
				((MyRemoteControl)P_0).m_currentDirection = (Sync<Base6Directions.Direction, SyncDirection.BothWays>)currentDirection;
				return result;
			}
		}

		protected class m_waypointThresholdDistance_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType waypointThresholdDistance;
				ISyncType result = waypointThresholdDistance = new Sync<float, SyncDirection.FromServer>(P_1, P_2);
				((MyRemoteControl)P_0).m_waypointThresholdDistance = (Sync<float, SyncDirection.FromServer>)waypointThresholdDistance;
				return result;
			}
		}

		protected class m_isMainRemoteControl_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isMainRemoteControl;
				ISyncType result = isMainRemoteControl = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyRemoteControl)P_0).m_isMainRemoteControl = (Sync<bool, SyncDirection.BothWays>)isMainRemoteControl;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyRemoteControl_003C_003EActor : IActivator, IActivator<MyRemoteControl>
		{
			private sealed override object CreateInstance()
			{
				return new MyRemoteControl();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRemoteControl CreateInstance()
			{
				return new MyRemoteControl();
			}

			MyRemoteControl IActivator<MyRemoteControl>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly double MAX_STOPPING_DISTANCE = 3000.0;

		private static readonly double PLANET_REPULSION_RADIUS = 2500.0;

		private static readonly double PLANET_AVOIDANCE_RADIUS = 5000.0;

		private static readonly double PLANET_AVOIDANCE_TOLERANCE = 100.0;

		private const float MAX_TERMINAL_DISTANCE_SQUARED = 10f;

		private long? m_savedPreviousControlledEntityId;

		private IMyControllableEntity m_previousControlledEntity;

		private Sync<long, SyncDirection.BothWays> m_bindedCamera;

		private static MyTerminalControlCombobox<MyRemoteControl> m_cameraList = null;

		private MyCharacter cockpitPilot;

		private List<MyAutopilotWaypoint> m_waypoints;

		private MyAutopilotWaypoint m_currentWaypoint;

		private PlanetCoordInformation m_destinationInfo;

		private PlanetCoordInformation m_currentInfo;

		private Vector3D m_currentWorldPosition;

		private Vector3D m_previousWorldPosition;

		private bool m_rotateBetweenWaypoints;

		private Sync<float, SyncDirection.BothWays> m_autopilotSpeedLimit;

		private readonly Sync<bool, SyncDirection.BothWays> m_useCollisionAvoidance;

		private int m_collisionCtr;

		private Vector3D m_oldCollisionDelta = Vector3D.Zero;

		private MyStuckDetection m_stuckDetection;

		private readonly Sync<bool, SyncDirection.BothWays> m_autoPilotEnabled;

		private readonly Sync<bool, SyncDirection.BothWays> m_dockingModeEnabled;

		private readonly Sync<FlightMode, SyncDirection.BothWays> m_currentFlightMode;

		private readonly Sync<bool, SyncDirection.BothWays> m_waitForFreeWay;

		private bool m_patrolDirectionForward = true;

		private Vector3D m_startPosition;

		private MyToolbar m_actionToolbar;

		private readonly Sync<Base6Directions.Direction, SyncDirection.BothWays> m_currentDirection;

		private Vector3D m_lastDelta = Vector3D.Zero;

		private float m_lastAutopilotSpeedLimit = 2f;

		private int m_collisionAvoidanceFrameSkip;

		private float m_rotateFor;

		private readonly List<DetectedObject> m_detectedObstacles = new List<DetectedObject>();

		private IRemoteControlAutomaticBehaviour m_automaticBehaviour;

		private readonly Sync<float, SyncDirection.FromServer> m_waypointThresholdDistance;

		private static MyObjectBuilder_AutopilotClipboard m_clipboard;

		private static MyGuiControlListbox m_gpsGuiControl;

		private static MyGuiControlListbox m_waypointGuiControl;

		private static readonly Dictionary<Base6Directions.Direction, MyStringId> m_directionNames = new Dictionary<Base6Directions.Direction, MyStringId>
		{
			{
				Base6Directions.Direction.Forward,
				MyCommonTexts.Thrust_Forward
			},
			{
				Base6Directions.Direction.Backward,
				MyCommonTexts.Thrust_Back
			},
			{
				Base6Directions.Direction.Left,
				MyCommonTexts.Thrust_Left
			},
			{
				Base6Directions.Direction.Right,
				MyCommonTexts.Thrust_Right
			},
			{
				Base6Directions.Direction.Up,
				MyCommonTexts.Thrust_Up
			},
			{
				Base6Directions.Direction.Down,
				MyCommonTexts.Thrust_Down
			}
		};

		private static readonly Dictionary<Base6Directions.Direction, Vector3D> m_upVectors = new Dictionary<Base6Directions.Direction, Vector3D>
		{
			{
				Base6Directions.Direction.Forward,
				Vector3D.Up
			},
			{
				Base6Directions.Direction.Backward,
				Vector3D.Up
			},
			{
				Base6Directions.Direction.Left,
				Vector3D.Up
			},
			{
				Base6Directions.Direction.Right,
				Vector3D.Up
			},
			{
				Base6Directions.Direction.Up,
				Vector3D.Right
			},
			{
				Base6Directions.Direction.Down,
				Vector3D.Right
			}
		};

		private bool m_syncing;

		private List<IMyGps> m_selectedGpsLocations;

		private List<MyAutopilotWaypoint> m_selectedWaypoints;

		private readonly StringBuilder m_tempName = new StringBuilder();

		private readonly StringBuilder m_tempTooltip = new StringBuilder();

		private readonly StringBuilder m_tempActions = new StringBuilder();

		private List<MyEntity> m_collisionEntityList;

		private List<MySlimBlock> m_collisionBlockList;

		private readonly Sync<bool, SyncDirection.BothWays> m_isMainRemoteControl;

		private bool m_releaseRequested;

		public IMyControllableEntity PreviousControlledEntity
		{
			get
			{
				if (m_savedPreviousControlledEntityId.HasValue && TryFindSavedEntity())
				{
					m_savedPreviousControlledEntityId = null;
				}
				return m_previousControlledEntity;
			}
			private set
			{
				if (value == m_previousControlledEntity)
				{
					return;
				}
				if (m_previousControlledEntity != null)
				{
					m_previousControlledEntity.Entity.OnMarkForClose -= Entity_OnPreviousMarkForClose;
					MyCockpit myCockpit = m_previousControlledEntity.Entity as MyCockpit;
					if (myCockpit != null && myCockpit.Pilot != null)
					{
						myCockpit.Pilot.OnMarkForClose -= Entity_OnPreviousMarkForClose;
					}
				}
				m_previousControlledEntity = value;
				if (m_previousControlledEntity != null)
				{
					AddPreviousControllerEvents();
				}
				SetEmissiveStateWorking();
			}
		}

		public override MyCharacter Pilot
		{
			get
			{
				MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					return myCharacter;
				}
				return cockpitPilot;
			}
		}

		private new MyRemoteControlDefinition BlockDefinition => (MyRemoteControlDefinition)base.BlockDefinition;

		public MyAutopilotWaypoint CurrentWaypoint
		{
			get
			{
				return m_currentWaypoint;
			}
			set
			{
				m_currentWaypoint = value;
				if (m_currentWaypoint != null)
				{
					m_startPosition = base.WorldMatrix.Translation;
				}
			}
		}

		public bool RotateBetweenWaypoints
		{
			get
			{
				if (!MyFakes.ENABLE_VR_REMOTE_CONTROL_WAYPOINTS_FAST_MOVEMENT)
				{
					return false;
				}
				return m_rotateBetweenWaypoints;
			}
			set
			{
				m_rotateBetweenWaypoints = value;
			}
		}

		public double TargettingAimDelta
		{
			get;
			private set;
		}

		public IRemoteControlAutomaticBehaviour AutomaticBehaviour => m_automaticBehaviour;

		bool Sandbox.ModAPI.Ingame.IMyRemoteControl.IsAutoPilotEnabled => m_autoPilotEnabled.Value;

		protected override ControllerPriority Priority
		{
			get
			{
				if ((bool)m_autoPilotEnabled)
				{
					return ControllerPriority.AutoPilot;
				}
				return ControllerPriority.Secondary;
			}
		}

		public bool IsMainRemoteControl
		{
			get
			{
				return m_isMainRemoteControl;
			}
			set
			{
				m_isMainRemoteControl.Value = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyRemoteControl.SpeedLimit
		{
			get
			{
				return m_autopilotSpeedLimit.Value;
			}
			set
			{
				m_autopilotSpeedLimit.Value = value;
			}
		}

		FlightMode Sandbox.ModAPI.Ingame.IMyRemoteControl.FlightMode
		{
			get
			{
				return m_currentFlightMode.Value;
			}
			set
			{
				m_currentFlightMode.Value = value;
			}
		}

		Base6Directions.Direction Sandbox.ModAPI.Ingame.IMyRemoteControl.Direction
		{
			get
			{
				return m_currentDirection.Value;
			}
			set
			{
				m_currentDirection.Value = value;
			}
		}

		MyWaypointInfo Sandbox.ModAPI.Ingame.IMyRemoteControl.CurrentWaypoint
		{
			get
			{
				if (CurrentWaypoint != null)
				{
					return new MyWaypointInfo(CurrentWaypoint.Name, CurrentWaypoint.Coords);
				}
				return MyWaypointInfo.Empty;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyRemoteControl.WaitForFreeWay
		{
			get
			{
				return m_waitForFreeWay.Value;
			}
			set
			{
				m_waitForFreeWay.Value = value;
			}
		}

		public MyRemoteControl()
		{
			CreateTerminalControls();
			TargettingAimDelta = 0.0;
			m_autoPilotEnabled.ValueChanged += delegate
			{
				OnSetAutoPilotEnabled();
			};
			m_isMainRemoteControl.ValueChanged += delegate
			{
				MainRemoteControlChanged();
			};
		}

		private void FillCameraComboBoxContent(ICollection<MyTerminalControlComboBoxItem> items)
		{
			MyTerminalControlComboBoxItem item = new MyTerminalControlComboBoxItem
			{
				Key = 0L,
				Value = MyCommonTexts.ScreenGraphicsOptions_AntiAliasing_None
			};
			items.Add(item);
			bool flag = false;
			foreach (MyCameraBlock fatBlock in base.CubeGrid.GetFatBlocks<MyCameraBlock>())
			{
				item = new MyTerminalControlComboBoxItem
				{
					Key = fatBlock.EntityId,
					Value = MyStringId.GetOrCompute(fatBlock.CustomName.ToString())
				};
				items.Add(item);
				if (fatBlock.EntityId == (long)m_bindedCamera)
				{
					flag = true;
				}
			}
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(base.CubeGrid);
			if (group != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
				{
					if (node.NodeData != base.CubeGrid)
					{
						foreach (MyCameraBlock fatBlock2 in node.NodeData.GetFatBlocks<MyCameraBlock>())
						{
							item = new MyTerminalControlComboBoxItem
							{
								Key = fatBlock2.EntityId,
								Value = MyStringId.GetOrCompute(fatBlock2.CustomName.ToString())
							};
							items.Add(item);
							if (fatBlock2.EntityId == (long)m_bindedCamera)
							{
								flag = true;
							}
						}
					}
				}
			}
			if (!flag)
			{
				m_bindedCamera.Value = 0L;
			}
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyRemoteControl>())
			{
				base.CreateTerminalControls();
				MyTerminalControlCheckbox<MyRemoteControl> obj = new MyTerminalControlCheckbox<MyRemoteControl>("MainRemoteControl", MySpaceTexts.TerminalControlPanel_Cockpit_MainRemoteControl, MySpaceTexts.TerminalControlPanel_Cockpit_MainRemoteControl)
				{
					Getter = ((MyRemoteControl x) => x.IsMainRemoteControl),
					Setter = delegate(MyRemoteControl x, bool v)
					{
						x.IsMainRemoteControl = v;
					},
					Enabled = ((MyRemoteControl x) => x.IsMainRemoteControlFree()),
					SupportsMultipleBlocks = false
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlButton<MyRemoteControl> obj2 = new MyTerminalControlButton<MyRemoteControl>("Control", MySpaceTexts.ControlRemote, MySpaceTexts.Blank, delegate(MyRemoteControl b)
				{
					b.RequestControl();
				})
				{
					Enabled = ((MyRemoteControl r) => r.CanControl(MySession.Static.ControlledEntity)),
					SupportsMultipleBlocks = false
				};
				MyTerminalAction<MyRemoteControl> myTerminalAction = obj2.EnableAction(MyTerminalActionIcons.TOGGLE);
				if (myTerminalAction != null)
				{
					myTerminalAction.InvalidToolbarTypes = new List<MyToolbarType>
					{
						MyToolbarType.ButtonPanel
					};
					myTerminalAction.ValidForGroups = false;
				}
				MyTerminalControlFactory.AddControl(obj2);
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyRemoteControl>());
				MyTerminalControlOnOffSwitch<MyRemoteControl> obj3 = new MyTerminalControlOnOffSwitch<MyRemoteControl>("AutoPilot", MySpaceTexts.BlockPropertyTitle_AutoPilot, MySpaceTexts.Blank)
				{
					Getter = ((MyRemoteControl x) => x.m_autoPilotEnabled),
					Setter = delegate(MyRemoteControl x, bool v)
					{
						x.SetAutoPilotEnabled(v);
					},
					Enabled = ((MyRemoteControl r) => r.CanEnableAutoPilot())
				};
				obj3.EnableToggleAction();
				obj3.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj3);
				MyTerminalControlOnOffSwitch<MyRemoteControl> obj4 = new MyTerminalControlOnOffSwitch<MyRemoteControl>("CollisionAvoidance", MySpaceTexts.BlockPropertyTitle_CollisionAvoidance, MySpaceTexts.Blank)
				{
					Getter = ((MyRemoteControl x) => x.m_useCollisionAvoidance),
					Setter = delegate(MyRemoteControl x, bool v)
					{
						x.SetCollisionAvoidance(v);
					},
					Enabled = ((MyRemoteControl r) => true)
				};
				obj4.EnableToggleAction();
				obj4.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj4);
				MyTerminalControlOnOffSwitch<MyRemoteControl> obj5 = new MyTerminalControlOnOffSwitch<MyRemoteControl>("DockingMode", MySpaceTexts.BlockPropertyTitle_EnableDockingMode, MySpaceTexts.Blank)
				{
					Getter = ((MyRemoteControl x) => x.m_dockingModeEnabled),
					Setter = delegate(MyRemoteControl x, bool v)
					{
						x.SetDockingMode(v);
					},
					Enabled = ((MyRemoteControl r) => r.IsWorking)
				};
				obj5.EnableToggleAction();
				obj5.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj5);
				MyTerminalControlCombobox<MyRemoteControl> obj6 = new MyTerminalControlCombobox<MyRemoteControl>("CameraList", MySpaceTexts.BlockPropertyTitle_AssignedCamera, MySpaceTexts.Blank)
				{
					ComboBoxContentWithBlock = delegate(MyRemoteControl x, ICollection<MyTerminalControlComboBoxItem> list)
					{
						x.FillCameraComboBoxContent(list);
					},
					Getter = ((MyRemoteControl x) => x.m_bindedCamera),
					Setter = delegate(MyRemoteControl x, long y)
					{
						x.m_bindedCamera.Value = y;
					}
				};
				MyTerminalControlFactory.AddControl(obj6);
				m_cameraList = obj6;
				MyTerminalControlCombobox<MyRemoteControl> myTerminalControlCombobox = new MyTerminalControlCombobox<MyRemoteControl>("FlightMode", MySpaceTexts.BlockPropertyTitle_FlightMode, MySpaceTexts.Blank);
				myTerminalControlCombobox.ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> x)
				{
					FillFlightModeCombo(x);
				};
				myTerminalControlCombobox.Getter = ((MyRemoteControl x) => (long)x.m_currentFlightMode.Value);
				myTerminalControlCombobox.Setter = delegate(MyRemoteControl x, long v)
				{
					x.ChangeFlightMode((FlightMode)v);
				};
				myTerminalControlCombobox.SetSerializerRange((int)MyEnum<FlightMode>.Range.Min, (int)MyEnum<FlightMode>.Range.Max);
				MyTerminalControlFactory.AddControl(myTerminalControlCombobox);
				MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<MyRemoteControl>("Direction", MySpaceTexts.BlockPropertyTitle_ForwardDirection, MySpaceTexts.Blank)
				{
					ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> x)
					{
						FillDirectionCombo(x);
					},
					Getter = ((MyRemoteControl x) => (long)x.m_currentDirection.Value),
					Setter = delegate(MyRemoteControl x, long v)
					{
						x.ChangeDirection((Base6Directions.Direction)v);
					}
				});
				MyTerminalControlSlider<MyRemoteControl> myTerminalControlSlider = new MyTerminalControlSlider<MyRemoteControl>("SpeedLimit", MySpaceTexts.BlockPropertyTitle_RemoteBlockSpeedLimit, MySpaceTexts.BlockPropertyTitle_RemoteBlockSpeedLimit);
				myTerminalControlSlider.SetLimits(0f, 100f);
				myTerminalControlSlider.DefaultValue = 100f;
				myTerminalControlSlider.Getter = ((MyRemoteControl x) => x.m_autopilotSpeedLimit);
				myTerminalControlSlider.Setter = delegate(MyRemoteControl x, float v)
				{
					x.m_autopilotSpeedLimit.Value = v;
				};
				myTerminalControlSlider.Writer = delegate(MyRemoteControl x, StringBuilder sb)
				{
					sb.Append(MyValueFormatter.GetFormatedFloat(x.m_autopilotSpeedLimit, 0));
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlListbox<MyRemoteControl> myTerminalControlListbox = new MyTerminalControlListbox<MyRemoteControl>("WaypointList", MySpaceTexts.BlockPropertyTitle_Waypoints, MySpaceTexts.Blank, multiSelect: true);
				myTerminalControlListbox.ListContent = delegate(MyRemoteControl x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2)
				{
					x.FillWaypointList(list1, list2);
				};
				myTerminalControlListbox.ItemSelected = delegate(MyRemoteControl x, List<MyGuiControlListbox.Item> y)
				{
					x.SelectWaypoint(y);
				};
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					m_waypointGuiControl = (MyGuiControlListbox)((MyGuiControlBlockProperty)myTerminalControlListbox.GetGuiControl()).PropertyControl;
				}
				MyTerminalControlFactory.AddControl(myTerminalControlListbox);
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyRemoteControl>("Open Toolbar", MySpaceTexts.BlockPropertyTitle_AutoPilotToolbarOpen, MySpaceTexts.BlockPropertyPopup_AutoPilotToolbarOpen, delegate(MyRemoteControl self)
				{
					MyToolbarItem[] actions = self.m_selectedWaypoints[0].Actions;
					if (actions != null)
					{
						for (int i = 0; i < actions.Length; i++)
						{
							if (actions[i] != null)
							{
								self.m_actionToolbar.SetItemAtIndex(i, actions[i]);
							}
						}
					}
					self.m_actionToolbar.ItemChanged += self.Toolbar_ItemChanged;
					if (MyGuiScreenToolbarConfigBase.Static == null)
					{
						MyToolbarComponent.CurrentToolbar = self.m_actionToolbar;
						MyGuiScreenBase myGuiScreenBase = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, self, null);
						MyToolbarComponent.AutoUpdate = false;
						myGuiScreenBase.Closed += delegate
						{
							MyToolbarComponent.AutoUpdate = true;
							self.m_actionToolbar.ItemChanged -= self.Toolbar_ItemChanged;
							self.m_actionToolbar.Clear();
						};
						MyGuiSandbox.AddScreen(myGuiScreenBase);
					}
				})
				{
					Enabled = ((MyRemoteControl r) => r.m_selectedWaypoints.Count == 1),
					SupportsMultipleBlocks = false
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyRemoteControl>("RemoveWaypoint", MySpaceTexts.BlockActionTitle_RemoveWaypoint, MySpaceTexts.Blank, delegate(MyRemoteControl b)
				{
					b.RemoveWaypoints();
				})
				{
					Enabled = ((MyRemoteControl r) => r.CanRemoveWaypoints()),
					SupportsMultipleBlocks = false
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyRemoteControl>("MoveUp", MySpaceTexts.BlockActionTitle_MoveWaypointUp, MySpaceTexts.Blank, delegate(MyRemoteControl b)
				{
					b.MoveWaypointsUp();
				})
				{
					Enabled = ((MyRemoteControl r) => r.CanMoveWaypointsUp()),
					SupportsMultipleBlocks = false
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyRemoteControl>("MoveDown", MySpaceTexts.BlockActionTitle_MoveWaypointDown, MySpaceTexts.Blank, delegate(MyRemoteControl b)
				{
					b.MoveWaypointsDown();
				})
				{
					Enabled = ((MyRemoteControl r) => r.CanMoveWaypointsDown()),
					SupportsMultipleBlocks = false
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyRemoteControl>("AddWaypoint", MySpaceTexts.BlockActionTitle_AddWaypoint, MySpaceTexts.Blank, delegate(MyRemoteControl b)
				{
					b.AddWaypoints();
				})
				{
					Enabled = ((MyRemoteControl r) => r.CanAddWaypoints()),
					SupportsMultipleBlocks = false
				});
				MyTerminalControlListbox<MyRemoteControl> myTerminalControlListbox2 = new MyTerminalControlListbox<MyRemoteControl>("GpsList", MySpaceTexts.BlockPropertyTitle_GpsLocations, MySpaceTexts.Blank, multiSelect: true);
				myTerminalControlListbox2.ListContent = delegate(MyRemoteControl x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2)
				{
					x.FillGpsList(list1, list2);
				};
				myTerminalControlListbox2.ItemSelected = delegate(MyRemoteControl x, List<MyGuiControlListbox.Item> y)
				{
					x.SelectGps(y);
				};
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					m_gpsGuiControl = (MyGuiControlListbox)((MyGuiControlBlockProperty)myTerminalControlListbox2.GetGuiControl()).PropertyControl;
				}
				MyTerminalControlFactory.AddControl(myTerminalControlListbox2);
				foreach (KeyValuePair<Base6Directions.Direction, MyStringId> directionName in m_directionNames)
				{
					MyTerminalControlFactory.AddAction(new MyTerminalAction<MyRemoteControl>(MyTexts.Get(directionName.Value).ToString(), MyTexts.Get(directionName.Value), OnAction, null, MyTerminalActionIcons.TOGGLE)
					{
						Enabled = ((MyRemoteControl b) => b.IsWorking),
						ParameterDefinitions = 
						{
							TerminalActionParameter.Get((byte)directionName.Key)
						}
					});
				}
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyRemoteControl>("Reset", MySpaceTexts.BlockActionTitle_WaypointReset, MySpaceTexts.BlockActionTooltip_WaypointReset, delegate(MyRemoteControl b)
				{
					b.ResetWaypoint();
				})
				{
					Enabled = ((MyRemoteControl r) => r.IsWorking),
					SupportsMultipleBlocks = false
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyRemoteControl>("Copy", MySpaceTexts.BlockActionTitle_RemoteCopy, MySpaceTexts.Blank, delegate(MyRemoteControl b)
				{
					b.CopyAutopilotSetup();
				})
				{
					Enabled = ((MyRemoteControl r) => r.IsWorking),
					SupportsMultipleBlocks = false
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyRemoteControl>("Paste", MySpaceTexts.BlockActionTitle_RemotePaste, MySpaceTexts.Blank, delegate(MyRemoteControl b)
				{
					b.PasteAutopilotSetup();
				})
				{
					Enabled = ((MyRemoteControl r) => r.IsWorking && m_clipboard != null),
					SupportsMultipleBlocks = false
				});
			}
		}

		private static void OnAction(MyRemoteControl block, ListReader<TerminalActionParameter> paramteres)
		{
			TerminalActionParameter terminalActionParameter = paramteres.FirstOrDefault();
			if (!terminalActionParameter.IsEmpty)
			{
				block.ChangeDirection((Base6Directions.Direction)terminalActionParameter.Value);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, CalculateRequiredPowerInput);
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			base.NeedsUpdate = (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
			MyObjectBuilder_RemoteControl myObjectBuilder_RemoteControl = (MyObjectBuilder_RemoteControl)objectBuilder;
			m_savedPreviousControlledEntityId = myObjectBuilder_RemoteControl.PreviousControlledEntityId;
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			myResourceSinkComponent.RequiredInputChanged += Receiver_RequiredInputChanged;
			myResourceSinkComponent.Update();
			m_autoPilotEnabled.SetLocalValue(myObjectBuilder_RemoteControl.AutoPilotEnabled);
			m_dockingModeEnabled.SetLocalValue(myObjectBuilder_RemoteControl.DockingModeEnabled);
			m_currentFlightMode.SetLocalValue((FlightMode)myObjectBuilder_RemoteControl.FlightMode);
			m_currentDirection.SetLocalValue((Base6Directions.Direction)myObjectBuilder_RemoteControl.Direction);
			m_waitForFreeWay.SetLocalValue(myObjectBuilder_RemoteControl.WaitForFreeWay);
			m_autopilotSpeedLimit.SetLocalValue(myObjectBuilder_RemoteControl.AutopilotSpeedLimit);
			m_bindedCamera.SetLocalValue(myObjectBuilder_RemoteControl.BindedCamera);
			m_waypointThresholdDistance.SetLocalValue(myObjectBuilder_RemoteControl.WaypointThresholdDistance);
			m_isMainRemoteControl.SetLocalValue(myObjectBuilder_RemoteControl.IsMainRemoteControl);
			m_stuckDetection = new MyStuckDetection(0.03f, 0.01f, base.CubeGrid.PositionComp.WorldAABB);
			if (myObjectBuilder_RemoteControl.Coords == null || myObjectBuilder_RemoteControl.Coords.Count == 0)
			{
				if (myObjectBuilder_RemoteControl.Waypoints == null)
				{
					m_waypoints = new List<MyAutopilotWaypoint>();
					CurrentWaypoint = null;
				}
				else
				{
					m_waypoints = new List<MyAutopilotWaypoint>(myObjectBuilder_RemoteControl.Waypoints.Count);
					for (int i = 0; i < myObjectBuilder_RemoteControl.Waypoints.Count; i++)
					{
						m_waypoints.Add(new MyAutopilotWaypoint(myObjectBuilder_RemoteControl.Waypoints[i], this));
					}
				}
			}
			else
			{
				m_waypoints = new List<MyAutopilotWaypoint>(myObjectBuilder_RemoteControl.Coords.Count);
				for (int j = 0; j < myObjectBuilder_RemoteControl.Coords.Count; j++)
				{
					m_waypoints.Add(new MyAutopilotWaypoint(myObjectBuilder_RemoteControl.Coords[j], myObjectBuilder_RemoteControl.Names[j], this));
				}
				if (myObjectBuilder_RemoteControl.AutoPilotToolbar != null && (FlightMode)m_currentFlightMode == FlightMode.OneWay)
				{
					m_waypoints[m_waypoints.Count - 1].SetActions(myObjectBuilder_RemoteControl.AutoPilotToolbar.Slots);
				}
			}
			if (myObjectBuilder_RemoteControl.CurrentWaypointIndex == -1 || myObjectBuilder_RemoteControl.CurrentWaypointIndex >= m_waypoints.Count)
			{
				CurrentWaypoint = null;
			}
			else
			{
				CurrentWaypoint = m_waypoints[myObjectBuilder_RemoteControl.CurrentWaypointIndex];
			}
			UpdatePlanetWaypointInfo();
			m_actionToolbar = new MyToolbar(MyToolbarType.ButtonPanel, 9, 1);
			m_actionToolbar.DrawNumbers = false;
			m_actionToolbar.Init(null, this);
			m_selectedGpsLocations = new List<IMyGps>();
			m_selectedWaypoints = new List<MyAutopilotWaypoint>();
			RaisePropertiesChangedRemote();
			SetDetailedInfoDirty();
			AddDebugRenderComponent(new MyDebugRenderComponentRemoteControl(this));
			m_useCollisionAvoidance.SetLocalValue(myObjectBuilder_RemoteControl.CollisionAvoidance);
			if (myObjectBuilder_RemoteControl.AutomaticBehaviour != null && myObjectBuilder_RemoteControl.AutomaticBehaviour is MyObjectBuilder_DroneAI)
			{
				MyDroneAI myDroneAI = new MyDroneAI();
				myDroneAI.Load(myObjectBuilder_RemoteControl.AutomaticBehaviour, this);
				SetAutomaticBehaviour(myDroneAI);
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			base.ResourceSink.Update();
			if ((bool)m_autoPilotEnabled)
			{
				base.ControlGroup.GetGroup(base.CubeGrid)?.GroupData.ControlSystem.AddControllerBlock(this);
			}
			if (!base.CubeGrid.IsPreview && base.CubeGrid.GridSystems != null && base.CubeGrid.GridSystems.RadioSystem != null)
			{
				base.CubeGrid.GridSystems.RadioSystem.UpdateRemoteControlInfo();
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if ((bool)m_autoPilotEnabled && !base.CubeGrid.IsPreview)
			{
				base.ControlGroup.GetGroup(base.CubeGrid)?.GroupData.ControlSystem.AddControllerBlock(this);
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			m_previousWorldPosition = m_currentWorldPosition;
			m_currentWorldPosition = base.WorldMatrix.Translation;
			if (m_releaseRequested && Sync.IsServer)
			{
				m_releaseRequested = false;
				if (!base.IsWorking)
				{
					MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.RequestRelease, arg2: false);
				}
			}
			if (m_savedPreviousControlledEntityId.HasValue)
			{
				MySession.Static.Players.UpdatePlayerControllers(base.EntityId);
				if (TryFindSavedEntity())
				{
					m_savedPreviousControlledEntityId = null;
				}
			}
			UpdateAutopilot();
		}

		private bool CanEnableAutoPilot()
		{
			if (m_automaticBehaviour == null && (m_waypoints.Count == 0 || (m_waypoints.Count == 1 && (FlightMode)m_currentFlightMode != FlightMode.OneWay)))
			{
				return false;
			}
			if (base.IsFunctional)
			{
				return m_previousControlledEntity == null;
			}
			return false;
		}

		private static void FillFlightModeCombo(List<MyTerminalControlComboBoxItem> list)
		{
			MyTerminalControlComboBoxItem item = new MyTerminalControlComboBoxItem
			{
				Key = 0L,
				Value = MySpaceTexts.BlockPropertyTitle_FlightMode_Patrol
			};
			list.Add(item);
			item = new MyTerminalControlComboBoxItem
			{
				Key = 1L,
				Value = MySpaceTexts.BlockPropertyTitle_FlightMode_Circle
			};
			list.Add(item);
			item = new MyTerminalControlComboBoxItem
			{
				Key = 2L,
				Value = MySpaceTexts.BlockPropertyTitle_FlightMode_OneWay
			};
			list.Add(item);
		}

		private static void FillDirectionCombo(List<MyTerminalControlComboBoxItem> list)
		{
			foreach (KeyValuePair<Base6Directions.Direction, MyStringId> directionName in m_directionNames)
			{
				list.Add(new MyTerminalControlComboBoxItem
				{
					Key = (long)directionName.Key,
					Value = directionName.Value
				});
			}
		}

		public void SetCollisionAvoidance(bool enabled)
		{
			m_useCollisionAvoidance.Value = enabled;
		}

		public void SetAutoPilotEnabled(bool enabled)
		{
			if (CanEnableAutoPilot() || !enabled)
			{
				if (!enabled)
				{
					ClearMovementControl();
				}
				m_autoPilotEnabled.Value = enabled;
			}
		}

		public bool IsAutopilotEnabled()
		{
			return m_autoPilotEnabled.Value;
		}

		public bool HasWaypoints()
		{
			return m_waypoints.Count > 0;
		}

		public void SetWaypointThresholdDistance(float thresholdDistance)
		{
			m_waypointThresholdDistance.Value = thresholdDistance;
		}

		private void RemoveAutoPilot()
		{
			MyEntityThrustComponent myEntityThrustComponent = base.CubeGrid.Components.Get<MyEntityThrustComponent>();
			if (myEntityThrustComponent != null)
			{
				myEntityThrustComponent.AutoPilotControlThrust = Vector3.Zero;
			}
			base.CubeGrid.GridSystems.GyroSystem.ControlTorque = Vector3.Zero;
			base.ControlGroup.GetGroup(base.CubeGrid)?.GroupData.ControlSystem.RemoveControllerBlock(this);
			if (base.CubeGrid.GridSystems.ControlSystem != null)
			{
				MyRemoteControl myRemoteControl = base.CubeGrid.GridSystems.ControlSystem.GetShipController() as MyRemoteControl;
				if (myRemoteControl == null || !myRemoteControl.m_autoPilotEnabled)
				{
					SetAutopilot(enabled: false);
				}
			}
		}

		private void OnSetAutoPilotEnabled()
		{
			if (!m_autoPilotEnabled)
			{
				RemoveAutoPilot();
				if (m_automaticBehaviour != null)
				{
					m_automaticBehaviour.StopWorking();
				}
			}
			else
			{
				base.ControlGroup.GetGroup(base.CubeGrid)?.GroupData.ControlSystem.AddControllerBlock(this);
				SetAutopilot(enabled: true);
				ResetShipControls();
			}
			base.ResourceSink.Update();
		}

		private void SetAutopilot(bool enabled)
		{
			MyEntityThrustComponent myEntityThrustComponent = base.CubeGrid.Components.Get<MyEntityThrustComponent>();
			if (myEntityThrustComponent != null)
			{
				myEntityThrustComponent.AutopilotEnabled = enabled;
				myEntityThrustComponent.MarkDirty();
			}
			if (base.CubeGrid.GridSystems.GyroSystem != null)
			{
				base.CubeGrid.GridSystems.GyroSystem.AutopilotEnabled = enabled;
				base.CubeGrid.GridSystems.GyroSystem.MarkDirty();
			}
		}

		public void SetDockingMode(bool enabled)
		{
			m_dockingModeEnabled.Value = enabled;
		}

		private void SelectGps(List<MyGuiControlListbox.Item> selection)
		{
			m_selectedGpsLocations.Clear();
			if (selection.Count > 0)
			{
				foreach (MyGuiControlListbox.Item item in selection)
				{
					m_selectedGpsLocations.Add((IMyGps)item.UserData);
				}
			}
			RaisePropertiesChangedRemote();
		}

		private void SelectWaypoint(List<MyGuiControlListbox.Item> selection)
		{
			m_selectedWaypoints.Clear();
			if (selection.Count > 0)
			{
				foreach (MyGuiControlListbox.Item item in selection)
				{
					m_selectedWaypoints.Add((MyAutopilotWaypoint)item.UserData);
				}
			}
			RaisePropertiesChangedRemote();
		}

		private void AddWaypoints()
		{
			if (m_selectedGpsLocations.Count > 0)
			{
				int count = m_selectedGpsLocations.Count;
				Vector3D[] array = new Vector3D[count];
				string[] array2 = new string[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = m_selectedGpsLocations[i].Coords;
					array2[i] = m_selectedGpsLocations[i].Name;
				}
				MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.OnAddWaypoints, array, array2);
				m_selectedGpsLocations.Clear();
			}
		}

		[Event(null, 993)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnAddWaypoints(Vector3D[] coords, string[] names)
		{
			for (int i = 0; i < coords.Length; i++)
			{
				m_waypoints.Add(new MyAutopilotWaypoint(coords[i], names[i], this));
			}
			RaisePropertiesChangedRemote();
		}

		private bool CanMoveItemUp(int index)
		{
			if (index == -1)
			{
				return false;
			}
			for (int num = index - 1; num >= 0; num--)
			{
				if (!m_selectedWaypoints.Contains(m_waypoints[num]))
				{
					return true;
				}
			}
			return false;
		}

		private void MoveWaypointsUp()
		{
			if (m_selectedWaypoints.Count > 0)
			{
				List<int> list = new List<int>(m_selectedWaypoints.Count);
				foreach (MyAutopilotWaypoint selectedWaypoint in m_selectedWaypoints)
				{
					int num = m_waypoints.IndexOf(selectedWaypoint);
					if (CanMoveItemUp(num))
					{
						list.Add(num);
					}
				}
				if (list.Count > 0)
				{
					MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.OnMoveWaypointsUp, list);
				}
			}
		}

		[Event(null, 1044)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnMoveWaypointsUp(List<int> indexes)
		{
			for (int i = 0; i < indexes.Count; i++)
			{
				SwapWaypoints(indexes[i] - 1, indexes[i]);
			}
			RaisePropertiesChangedRemote();
		}

		private bool CanMoveItemDown(int index)
		{
			if (index == -1)
			{
				return false;
			}
			for (int i = index + 1; i < m_waypoints.Count; i++)
			{
				if (!m_selectedWaypoints.Contains(m_waypoints[i]))
				{
					return true;
				}
			}
			return false;
		}

		private void MoveWaypointsDown()
		{
			if (m_selectedWaypoints.Count > 0)
			{
				List<int> list = new List<int>(m_selectedWaypoints.Count);
				foreach (MyAutopilotWaypoint selectedWaypoint in m_selectedWaypoints)
				{
					int num = m_waypoints.IndexOf(selectedWaypoint);
					if (CanMoveItemDown(num))
					{
						list.Add(num);
					}
				}
				if (list.Count > 0)
				{
					MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.OnMoveWaypointsDown, list);
				}
			}
		}

		[Event(null, 1093)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnMoveWaypointsDown(List<int> indexes)
		{
			for (int num = indexes.Count - 1; num >= 0; num--)
			{
				int num2 = indexes[num];
				SwapWaypoints(num2, num2 + 1);
			}
			RaisePropertiesChangedRemote();
		}

		private void SwapWaypoints(int index1, int index2)
		{
			MyAutopilotWaypoint value = m_waypoints[index1];
			MyAutopilotWaypoint value2 = m_waypoints[index2];
			m_waypoints[index1] = value2;
			m_waypoints[index2] = value;
		}

		private void RemoveWaypoints()
		{
			if (m_selectedWaypoints.Count > 0)
			{
				int[] array = new int[m_selectedWaypoints.Count];
				for (int i = 0; i < m_selectedWaypoints.Count; i++)
				{
					MyAutopilotWaypoint item = m_selectedWaypoints[i];
					array[i] = m_waypoints.IndexOf(item);
				}
				Array.Sort(array);
				MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.OnRemoveWaypoints, array);
				m_selectedWaypoints.Clear();
				RaisePropertiesChangedRemote();
			}
		}

		[Event(null, 1134)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveWaypoints(int[] indexes)
		{
			bool flag = false;
			for (int num = indexes.Length - 1; num >= 0; num--)
			{
				int num2 = indexes[num];
				if (num2 > -1 && num2 < m_waypoints.Count)
				{
					MyAutopilotWaypoint myAutopilotWaypoint = m_waypoints[num2];
					m_waypoints.Remove(myAutopilotWaypoint);
					if (CurrentWaypoint == myAutopilotWaypoint)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				AdvanceWaypoint();
			}
			RaisePropertiesChangedRemote();
			if (IsAutopilotEnabled() && !CanEnableAutoPilot())
			{
				SetAutoPilotEnabled(enabled: false);
			}
		}

		public void ChangeFlightMode(FlightMode flightMode)
		{
			if (flightMode != (FlightMode)m_currentFlightMode)
			{
				m_currentFlightMode.Value = flightMode;
			}
			SetAutoPilotEnabled(m_autoPilotEnabled);
		}

		public void SetWaitForFreeWay(bool waitForFreeWay)
		{
			if (waitForFreeWay != (bool)m_waitForFreeWay)
			{
				m_waitForFreeWay.Value = waitForFreeWay;
			}
			SetAutoPilotEnabled(m_autoPilotEnabled);
		}

		public void SetAutoPilotSpeedLimit(float speedLimit)
		{
			m_autopilotSpeedLimit.Value = speedLimit;
		}

		public void ChangeDirection(Base6Directions.Direction direction)
		{
			m_currentDirection.Value = direction;
		}

		private bool CanAddWaypoints()
		{
			if (m_selectedGpsLocations.Count == 0)
			{
				return false;
			}
			_ = m_waypoints.Count;
			return true;
		}

		private bool CanMoveWaypointsUp()
		{
			if (m_selectedWaypoints.Count == 0)
			{
				return false;
			}
			if (m_waypoints.Count == 0)
			{
				return false;
			}
			foreach (MyAutopilotWaypoint selectedWaypoint in m_selectedWaypoints)
			{
				int index = m_waypoints.IndexOf(selectedWaypoint);
				if (CanMoveItemUp(index))
				{
					return true;
				}
			}
			return false;
		}

		private bool CanMoveWaypointsDown()
		{
			if (m_selectedWaypoints.Count == 0)
			{
				return false;
			}
			if (m_waypoints.Count == 0)
			{
				return false;
			}
			foreach (MyAutopilotWaypoint selectedWaypoint in m_selectedWaypoints)
			{
				int index = m_waypoints.IndexOf(selectedWaypoint);
				if (CanMoveItemDown(index))
				{
					return true;
				}
			}
			return false;
		}

		private bool CanRemoveWaypoints()
		{
			return m_selectedWaypoints.Count > 0;
		}

		private void ResetWaypoint()
		{
			MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.OnResetWaypoint);
			if (!Sync.IsServer)
			{
				OnResetWaypoint();
			}
		}

		[Event(null, 1274)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		private void OnResetWaypoint()
		{
			if (m_waypoints.Count > 0)
			{
				CurrentWaypoint = m_waypoints[0];
				m_patrolDirectionForward = true;
				RaisePropertiesChangedRemote();
			}
		}

		private void CopyAutopilotSetup()
		{
			m_clipboard = new MyObjectBuilder_AutopilotClipboard();
			m_clipboard.Direction = (byte)m_currentDirection.Value;
			m_clipboard.FlightMode = (int)m_currentFlightMode.Value;
			m_clipboard.RemoteEntityId = base.EntityId;
			m_clipboard.DockingModeEnabled = m_dockingModeEnabled;
			m_clipboard.Waypoints = new List<MyObjectBuilder_AutopilotWaypoint>(m_waypoints.Count);
			foreach (MyAutopilotWaypoint waypoint in m_waypoints)
			{
				m_clipboard.Waypoints.Add(waypoint.GetObjectBuilder());
			}
			RaisePropertiesChangedRemote();
		}

		private void PasteAutopilotSetup()
		{
			if (m_clipboard != null)
			{
				MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.OnPasteAutopilotSetup, m_clipboard);
			}
		}

		[Event(null, 1308)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnPasteAutopilotSetup(MyObjectBuilder_AutopilotClipboard clipboard)
		{
			if (Sync.IsServer)
			{
				m_currentDirection.Value = (Base6Directions.Direction)clipboard.Direction;
				m_currentFlightMode.Value = (FlightMode)clipboard.FlightMode;
				m_dockingModeEnabled.Value = clipboard.DockingModeEnabled;
			}
			if (clipboard.Waypoints != null)
			{
				m_waypoints = new List<MyAutopilotWaypoint>(clipboard.Waypoints.Count);
				foreach (MyObjectBuilder_AutopilotWaypoint waypoint in clipboard.Waypoints)
				{
					if (waypoint.Actions != null)
					{
						foreach (MyObjectBuilder_ToolbarItem action in waypoint.Actions)
						{
							MyObjectBuilder_ToolbarItemTerminalBlock myObjectBuilder_ToolbarItemTerminalBlock = action as MyObjectBuilder_ToolbarItemTerminalBlock;
							if (myObjectBuilder_ToolbarItemTerminalBlock != null && myObjectBuilder_ToolbarItemTerminalBlock.BlockEntityId == clipboard.RemoteEntityId)
							{
								myObjectBuilder_ToolbarItemTerminalBlock.BlockEntityId = base.EntityId;
							}
						}
					}
					m_waypoints.Add(new MyAutopilotWaypoint(waypoint, this));
				}
			}
			m_selectedWaypoints.Clear();
			RaisePropertiesChangedRemote();
		}

		public void ClearWaypoints()
		{
			MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.ClearWaypoints_Implementation);
			if (!Sync.IsServer)
			{
				ClearWaypoints_Implementation();
			}
		}

		void Sandbox.ModAPI.Ingame.IMyRemoteControl.GetWaypointInfo(List<MyWaypointInfo> waypoints)
		{
			if (waypoints != null)
			{
				waypoints.Clear();
				for (int i = 0; i < m_waypoints.Count; i++)
				{
					MyAutopilotWaypoint myAutopilotWaypoint = m_waypoints[i];
					waypoints.Add(new MyWaypointInfo(myAutopilotWaypoint.Name, myAutopilotWaypoint.Coords));
				}
			}
		}

		[Event(null, 1364)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		private void ClearWaypoints_Implementation()
		{
			m_waypoints.Clear();
			AdvanceWaypoint();
			RaisePropertiesChangedRemote();
		}

		public void AddWaypoint(Vector3D point, string name)
		{
			MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.OnAddWaypoint, point, name);
		}

		public void AddWaypoint(MyWaypointInfo coords)
		{
			MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.OnAddWaypoint, coords.Coords, coords.Name);
		}

		[Event(null, 1382)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnAddWaypoint(Vector3D point, string name)
		{
			m_waypoints.Add(new MyAutopilotWaypoint(point, name, this));
			RaisePropertiesChangedRemote();
		}

		public void AddWaypoint(Vector3D point, string name, List<MyObjectBuilder_ToolbarItem> actionBuilders)
		{
			MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.OnAddWaypoint, point, name, actionBuilders);
		}

		[Event(null, 1394)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnAddWaypoint(Vector3D point, string name, [DynamicItem(typeof(MyObjectBuilderDynamicSerializer), false)] List<MyObjectBuilder_ToolbarItem> actionBuilders)
		{
			m_waypoints.Add(new MyAutopilotWaypoint(point, name, actionBuilders, null, this));
			RaisePropertiesChangedRemote();
		}

		private void FillGpsList(ICollection<MyGuiControlListbox.Item> gpsItemList, ICollection<MyGuiControlListbox.Item> selectedGpsItemList)
		{
			List<IMyGps> list = new List<IMyGps>();
			MySession.Static.Gpss.GetGpsList(MySession.Static.LocalPlayerId, list);
			foreach (IMyGps item2 in list)
			{
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(item2.Name), null, null, item2);
				gpsItemList.Add(item);
				if (m_selectedGpsLocations.Contains(item2))
				{
					selectedGpsItemList.Add(item);
				}
			}
		}

		private void FillWaypointList(ICollection<MyGuiControlListbox.Item> waypoints, ICollection<MyGuiControlListbox.Item> selectedWaypoints)
		{
			foreach (MyAutopilotWaypoint waypoint in m_waypoints)
			{
				m_tempName.Append(waypoint.Name);
				int num = 0;
				m_tempActions.Append("\nActions:");
				if (waypoint.Actions != null)
				{
					MyToolbarItem[] actions = waypoint.Actions;
					foreach (MyToolbarItem myToolbarItem in actions)
					{
						if (myToolbarItem != null)
						{
							m_tempActions.Append("\n");
							myToolbarItem.Update(this, 0L);
							m_tempActions.AppendStringBuilder(myToolbarItem.DisplayName);
							num++;
						}
					}
				}
				m_tempTooltip.AppendStringBuilder(m_tempName);
				m_tempTooltip.Append('\n');
				m_tempTooltip.Append(waypoint.Coords.ToString());
				if (num > 0)
				{
					m_tempName.Append(" [");
					m_tempName.Append(num.ToString());
					if (num > 1)
					{
						m_tempName.Append(" Actions]");
					}
					else
					{
						m_tempName.Append(" Action]");
					}
					m_tempTooltip.AppendStringBuilder(m_tempActions);
				}
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(m_tempName, m_tempTooltip.ToString(), null, waypoint);
				waypoints.Add(item);
				if (m_selectedWaypoints.Contains(waypoint))
				{
					selectedWaypoints.Add(item);
				}
				m_tempName.Clear();
				m_tempTooltip.Clear();
				m_tempActions.Clear();
			}
		}

		private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index)
		{
			if (m_selectedWaypoints.Count == 1)
			{
				SendToolbarItemChanged(ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex)), index.ItemIndex, m_waypoints.IndexOf(m_selectedWaypoints[0]));
			}
		}

		private void RaisePropertiesChangedRemote()
		{
			int num = (m_gpsGuiControl != null) ? m_gpsGuiControl.FirstVisibleRow : 0;
			int num2 = (m_waypointGuiControl != null) ? m_waypointGuiControl.FirstVisibleRow : 0;
			RaisePropertiesChanged();
			if (m_gpsGuiControl != null && num < m_gpsGuiControl.Items.Count)
			{
				m_gpsGuiControl.FirstVisibleRow = num;
			}
			if (m_waypointGuiControl != null && num2 < m_waypointGuiControl.Items.Count)
			{
				m_waypointGuiControl.FirstVisibleRow = num2;
			}
		}

		private void UpdateAutopilot()
		{
			if (!base.IsWorking || !m_autoPilotEnabled)
			{
				return;
			}
			MyShipController shipController = base.CubeGrid.GridSystems.ControlSystem.GetShipController();
			if (shipController == null)
			{
				base.ControlGroup.GetGroup(base.CubeGrid)?.GroupData.ControlSystem.AddControllerBlock(this);
				shipController = base.CubeGrid.GridSystems.ControlSystem.GetShipController();
			}
			if (shipController == this)
			{
				MyEntityThrustComponent myEntityThrustComponent = base.CubeGrid.Components.Get<MyEntityThrustComponent>();
				if (myEntityThrustComponent != null && !myEntityThrustComponent.AutopilotEnabled)
				{
					myEntityThrustComponent.AutopilotEnabled = true;
				}
				if (myEntityThrustComponent != null)
				{
					myEntityThrustComponent.Enabled = base.ControlThrusters;
				}
				if (CurrentWaypoint == null && m_waypoints.Count > 0)
				{
					CurrentWaypoint = m_waypoints[0];
					UpdatePlanetWaypointInfo();
					RaisePropertiesChangedRemote();
					SetDetailedInfoDirty();
				}
				if (CurrentWaypoint != null)
				{
					if ((m_automaticBehaviour == null || !m_automaticBehaviour.Ambushing) && (IsInStoppingDistance() || (m_automaticBehaviour == null && m_stuckDetection.IsStuck)))
					{
						AdvanceWaypoint();
					}
					if (MyFakes.ENABLE_VR_REMOTE_CONTROL_WAYPOINTS_FAST_MOVEMENT)
					{
						MyAutopilotWaypoint myAutopilotWaypoint = null;
						while (CurrentWaypoint != null && CurrentWaypoint != myAutopilotWaypoint && (m_automaticBehaviour == null || !m_automaticBehaviour.IsActive) && IsInStoppingDistance())
						{
							myAutopilotWaypoint = CurrentWaypoint;
							AdvanceWaypoint();
						}
					}
					if (Sync.IsServer && CurrentWaypoint != null && !IsInStoppingDistance() && (bool)m_autoPilotEnabled)
					{
						CalculateDeltaPos(out Vector3D deltaPos, out Vector3D perpDeltaPos, out Vector3D targetDelta, out float autopilotSpeedLimit);
						UpdateGyro(targetDelta, perpDeltaPos, out bool rotating, out bool isLabile);
						if (m_automaticBehaviour == null)
						{
							m_stuckDetection.SetRotating(rotating);
						}
						if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
						{
							MyRenderProxy.DebugDrawLine3D(base.WorldMatrix.Translation, base.WorldMatrix.Translation + deltaPos, Color.Green, Color.GreenYellow, depthRead: false);
							foreach (MyAutopilotWaypoint waypoint in m_waypoints)
							{
								MyRenderProxy.DebugDrawSphere(waypoint.Coords, 3f, Color.GreenYellow, 1f, depthRead: false, smooth: true);
								MyRenderProxy.DebugDrawText3D(waypoint.Coords, waypoint.Name, Color.GreenYellow, 1f, depthRead: false);
							}
						}
						m_rotateFor -= 0.0166666675f;
						if (m_automaticBehaviour != null && m_automaticBehaviour.Ambushing)
						{
							if (myEntityThrustComponent != null)
							{
								myEntityThrustComponent.AutoPilotControlThrust = Vector3.Zero;
							}
						}
						else if ((m_automaticBehaviour == null || !m_automaticBehaviour.IsActive) && rotating && !isLabile && (!MyFakes.ENABLE_NEW_COLLISION_AVOIDANCE || !m_useCollisionAvoidance.Value || Vector3D.DistanceSquared(CurrentWaypoint.Coords, base.WorldMatrix.Translation) < 25.0))
						{
							if (myEntityThrustComponent != null)
							{
								myEntityThrustComponent.AutoPilotControlThrust = Vector3.Zero;
							}
						}
						else
						{
							UpdateThrust(deltaPos, perpDeltaPos, autopilotSpeedLimit);
						}
					}
				}
				else if (Sync.IsServer && m_automaticBehaviour != null && m_automaticBehaviour.IsActive && m_automaticBehaviour.RotateToTarget)
				{
					UpdateGyro(Vector3.Zero, Vector3.Zero, out bool rotating2, out bool isLabile2);
					if (rotating2 && !isLabile2 && myEntityThrustComponent != null)
					{
						myEntityThrustComponent.AutoPilotControlThrust = Vector3.Zero;
					}
				}
				if (m_automaticBehaviour == null)
				{
					m_stuckDetection.Update(base.WorldMatrix.Translation, base.WorldMatrix.Forward, (CurrentWaypoint == null) ? Vector3D.Zero : CurrentWaypoint.Coords);
				}
			}
			if (m_automaticBehaviour != null)
			{
				m_automaticBehaviour.Update();
			}
		}

		private bool IsInStoppingDistance()
		{
			int num = m_waypoints.IndexOf(CurrentWaypoint);
			double num2 = (base.WorldMatrix.Translation - CurrentWaypoint.Coords).LengthSquared();
			double num3 = base.CubeGrid.GridSize * 3f;
			if (m_automaticBehaviour != null && m_automaticBehaviour.IsActive)
			{
				num3 = m_automaticBehaviour.WaypointThresholdDistance;
			}
			else if ((float)m_waypointThresholdDistance > 0f)
			{
				num3 = (float)m_waypointThresholdDistance;
			}
			else if ((bool)m_dockingModeEnabled || ((FlightMode)m_currentFlightMode == FlightMode.OneWay && num == m_waypoints.Count - 1))
			{
				num3 = ((!(base.CubeGrid.GridSize >= 0.5f)) ? ((double)base.CubeGrid.GridSize) : ((double)base.CubeGrid.GridSize * 0.25));
			}
			if (MyFakes.ENABLE_VR_REMOTE_CONTROL_WAYPOINTS_FAST_MOVEMENT)
			{
				if (num2 < num3 * num3)
				{
					return true;
				}
				if ((m_previousWorldPosition - CurrentWaypoint.Coords).LengthSquared() < num3 * num3)
				{
					return true;
				}
				Vector3D direction = base.WorldMatrix.Translation - m_previousWorldPosition;
				double num4 = direction.Normalize();
				if (num4 > 0.01)
				{
					RayD ray = new RayD(m_previousWorldPosition, direction);
					double? num5 = new BoundingSphereD(CurrentWaypoint.Coords, num3).Intersects(ray);
					if (!num5.HasValue)
					{
						return false;
					}
					return num5.Value <= num4;
				}
			}
			return num2 < num3 * num3;
		}

		public void AdvanceWaypoint()
		{
			int num = m_waypoints.IndexOf(CurrentWaypoint);
			MyAutopilotWaypoint currentWaypoint = CurrentWaypoint;
			bool flag = m_autoPilotEnabled;
			if (m_waypoints.Count > 0)
			{
				if ((FlightMode)m_currentFlightMode == FlightMode.Circle)
				{
					num = (num + 1) % m_waypoints.Count;
				}
				else if ((FlightMode)m_currentFlightMode == FlightMode.Patrol)
				{
					if (m_patrolDirectionForward)
					{
						num++;
						if (num >= m_waypoints.Count)
						{
							num = ((m_waypoints.Count != 1) ? (m_waypoints.Count - 2) : 0);
							m_patrolDirectionForward = false;
						}
					}
					else
					{
						num--;
						if (num < 0)
						{
							num = 1;
							if (m_waypoints.Count == 1)
							{
								num = 0;
							}
							m_patrolDirectionForward = true;
						}
					}
				}
				else if ((FlightMode)m_currentFlightMode == FlightMode.OneWay)
				{
					num++;
					if (num >= m_waypoints.Count)
					{
						num = 0;
						base.CubeGrid.GridSystems.GyroSystem.ControlTorque = Vector3.Zero;
						MyEntityThrustComponent myEntityThrustComponent = base.CubeGrid.Components.Get<MyEntityThrustComponent>();
						if (myEntityThrustComponent != null)
						{
							myEntityThrustComponent.AutoPilotControlThrust = Vector3.Zero;
						}
						if (Sync.IsServer && (m_automaticBehaviour == null || !m_automaticBehaviour.IsActive))
						{
							flag = false;
						}
					}
				}
			}
			if (num < 0 || num >= m_waypoints.Count)
			{
				CurrentWaypoint = null;
				if (Sync.IsServer && (m_automaticBehaviour == null || !m_automaticBehaviour.IsActive))
				{
					flag = false;
				}
				UpdatePlanetWaypointInfo();
				RaisePropertiesChangedRemote();
				SetDetailedInfoDirty();
			}
			else
			{
				CurrentWaypoint = m_waypoints[num];
				if (CurrentWaypoint != currentWaypoint || m_waypoints.Count == 1)
				{
					if (Sync.IsServer && currentWaypoint.Actions != null)
					{
						for (int i = 0; i < currentWaypoint.Actions.Length; i++)
						{
							if (currentWaypoint.Actions[i] != null)
							{
								m_actionToolbar.SetItemAtIndex(0, currentWaypoint.Actions[i]);
								m_actionToolbar.UpdateItem(0);
								m_actionToolbar.ActivateItemAtSlot(0);
								MyToolbarItem itemAtSlot = m_actionToolbar.GetItemAtSlot(0);
								if (Sync.IsServer && itemAtSlot != null && itemAtSlot.DisplayName.ToString().Contains("Autopilot"))
								{
									flag = m_autoPilotEnabled.Value;
								}
								if (Sync.IsServer && m_waypoints.Count == 1)
								{
									flag = false;
								}
							}
						}
						m_actionToolbar.Clear();
					}
					UpdatePlanetWaypointInfo();
					RaisePropertiesChangedRemote();
					SetDetailedInfoDirty();
				}
			}
			if (Sync.IsServer && flag != (bool)m_autoPilotEnabled)
			{
				SetAutoPilotEnabled(flag);
			}
			bool force = (m_automaticBehaviour != null && m_automaticBehaviour.IsActive && m_automaticBehaviour.ResetStuckDetection) || CurrentWaypoint != currentWaypoint;
			m_stuckDetection.Reset(force);
			if (m_automaticBehaviour != null)
			{
				m_automaticBehaviour.WaypointAdvanced();
			}
		}

		private void UpdatePlanetWaypointInfo()
		{
			if (CurrentWaypoint == null)
			{
				m_destinationInfo.Clear();
			}
			else
			{
				m_destinationInfo.Calculate(CurrentWaypoint.Coords);
			}
		}

		private Vector3D GetAngleVelocity(QuaternionD q1, QuaternionD q2)
		{
			q1.Conjugate();
			QuaternionD quaternionD = q2 * q1;
			double num = 2.0 * Math.Acos(MathHelper.Clamp(quaternionD.W, -1.0, 1.0));
			if (num > Math.PI)
			{
				num -= Math.PI * 2.0;
			}
			return num * new Vector3D(quaternionD.X, quaternionD.Y, quaternionD.Z) / Math.Sqrt(quaternionD.X * quaternionD.X + quaternionD.Y * quaternionD.Y + quaternionD.Z * quaternionD.Z);
		}

		private MatrixD GetOrientation()
		{
			return MatrixD.CreateWorld(Vector3D.Zero, (Vector3D)Base6Directions.GetVector(m_currentDirection), m_upVectors[m_currentDirection]) * base.WorldMatrix.GetOrientation();
		}

		private void UpdateGyro(Vector3D deltaPos, Vector3D perpDeltaPos, out bool rotating, out bool isLabile)
		{
			isLabile = false;
			rotating = true;
			MyGridGyroSystem gyroSystem = base.CubeGrid.GridSystems.GyroSystem;
			gyroSystem.ControlTorque = Vector3.Zero;
			if (CurrentWaypoint != null && (CurrentWaypoint.Coords - base.WorldMatrix.Translation).Length() < (double)base.CubeGrid.PositionComp.LocalVolume.Radius)
			{
				rotating = false;
				isLabile = true;
				return;
			}
			Vector3D value = base.CubeGrid.Physics.AngularVelocity;
			MatrixD matrix = GetOrientation();
			Matrix matrix2 = base.CubeGrid.PositionComp.WorldMatrixNormalizedInv.GetOrientation();
			Vector3D gravityWorld = m_currentInfo.GravityWorld;
			QuaternionD q = QuaternionD.CreateFromRotationMatrix(matrix);
			Vector3D vector3D;
			QuaternionD q2;
			if (m_currentInfo.IsValid())
			{
				vector3D = perpDeltaPos;
				vector3D.Normalize();
				q2 = QuaternionD.CreateFromForwardUp(vector3D, -gravityWorld);
				isLabile = (Vector3D.Dot(vector3D, matrix.Forward) > 0.95 || Math.Abs(Vector3D.Dot(Vector3D.Normalize(deltaPos), gravityWorld)) > 0.95);
			}
			else
			{
				vector3D = deltaPos;
				vector3D.Normalize();
				q2 = QuaternionD.CreateFromForwardUp(vector3D, matrix.Up);
			}
			if (m_automaticBehaviour != null && m_automaticBehaviour.IsActive && m_automaticBehaviour.RotateToTarget && m_automaticBehaviour.CurrentTarget != null)
			{
				isLabile = false;
				matrix = MatrixD.CreateWorld(Vector3D.Zero, (Vector3D)Base6Directions.GetVector(Base6Directions.Direction.Forward), m_upVectors[Base6Directions.Direction.Forward]) * base.WorldMatrix.GetOrientation();
				q = QuaternionD.CreateFromRotationMatrix(matrix);
				vector3D = m_automaticBehaviour.CurrentTarget.WorldMatrix.Translation - base.WorldMatrix.Translation;
				if (m_automaticBehaviour.CurrentTarget is MyCharacter)
				{
					vector3D += m_automaticBehaviour.CurrentTarget.WorldMatrix.Up * m_automaticBehaviour.PlayerYAxisOffset;
				}
				vector3D.Normalize();
				Vector3D up = m_automaticBehaviour.CurrentTarget.WorldMatrix.Up;
				up.Normalize();
				q2 = QuaternionD.CreateFromForwardUp(up: (!(Math.Abs(Vector3D.Dot(vector3D, up)) >= 0.98)) ? Vector3D.Cross(Vector3D.Cross(vector3D, up), vector3D) : Vector3D.CalculatePerpendicularVector(vector3D), forward: vector3D);
			}
			Vector3D v = GetAngleVelocity(q, q2);
			Vector3D vector3D2 = v * value.Dot(ref v);
			v = Vector3D.Transform(v, matrix2);
			double num2 = TargettingAimDelta = Math.Acos(MathHelper.Clamp(Vector3D.Dot(vector3D, matrix.Forward), -1.0, 1.0));
			if (num2 < 0.03)
			{
				rotating = false;
				return;
			}
			rotating = (rotating && !RotateBetweenWaypoints);
			Vector3D value2 = value - gyroSystem.GetAngularVelocity(-v);
			double num3 = (value / value2).Max();
			double num4 = num2 / vector3D2.Length() * num2;
			if (double.IsNaN(num3) || double.IsInfinity(num4) || num4 > num3)
			{
				if ((bool)m_dockingModeEnabled)
				{
					v /= 4.0;
				}
				gyroSystem.ControlTorque = v;
				gyroSystem.MarkDirty();
			}
			else if (num2 < 0.1 && m_automaticBehaviour != null && m_automaticBehaviour.RotateToTarget && m_automaticBehaviour.CurrentTarget != null)
			{
				gyroSystem.ControlTorque = v / 3.0;
				gyroSystem.MarkDirty();
			}
			if ((bool)m_dockingModeEnabled)
			{
				_ = 0.05;
			}
			else
			{
				_ = 0.25;
			}
		}

		private void CalculateDeltaPos(out Vector3D deltaPos, out Vector3D perpDeltaPos, out Vector3D targetDelta, out float autopilotSpeedLimit)
		{
			autopilotSpeedLimit = m_autopilotSpeedLimit.Value;
			m_currentInfo.Calculate(base.WorldMatrix.Translation);
			Vector3D coords = CurrentWaypoint.Coords;
			Vector3D translation = base.WorldMatrix.Translation;
			targetDelta = coords - translation;
			if ((bool)m_useCollisionAvoidance)
			{
				if (MyFakes.ENABLE_NEW_COLLISION_AVOIDANCE)
				{
					deltaPos = AvoidCollisionsVs2(targetDelta, ref autopilotSpeedLimit);
					targetDelta = deltaPos;
				}
				else
				{
					deltaPos = AvoidCollisions(targetDelta, ref autopilotSpeedLimit);
				}
			}
			else
			{
				deltaPos = targetDelta;
			}
			perpDeltaPos = Vector3D.Reject(targetDelta, m_currentInfo.GravityWorld);
		}

		private void FillListOfDetectedObjects(Vector3D pos, MyEntity parentEntity, ref int listLimit, ref Vector3D shipFront, ref float closestEntityDist, ref MyEntity closestEntity)
		{
			float num = Vector3.DistanceSquared(pos, shipFront);
			if (num < closestEntityDist)
			{
				closestEntityDist = num;
				closestEntity = parentEntity;
			}
			if (m_detectedObstacles.Count == 0)
			{
				m_detectedObstacles.Add(new DetectedObject(num, pos, parentEntity is MyVoxelBase));
			}
			else
			{
				for (int i = 0; i < m_detectedObstacles.Count; i++)
				{
					if (num < m_detectedObstacles[i].Distance)
					{
						if (m_detectedObstacles.Count == listLimit)
						{
							m_detectedObstacles.RemoveAt(listLimit - 1);
						}
						m_detectedObstacles.AddOrInsert(new DetectedObject(num, pos, parentEntity is MyVoxelBase), i);
						break;
					}
				}
			}
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				MyRenderProxy.DebugDrawSphere(pos, 1.5f, Color.Red, 1f, depthRead: false);
			}
		}

		private Vector3D AvoidCollisionsVs2(Vector3D delta, ref float autopilotSpeedLimit)
		{
			if (m_collisionAvoidanceFrameSkip > 0)
			{
				m_collisionAvoidanceFrameSkip--;
				autopilotSpeedLimit = m_lastAutopilotSpeedLimit;
				return m_lastDelta;
			}
			m_collisionAvoidanceFrameSkip = 19;
			MyEntityThrustComponent myEntityThrustComponent = base.CubeGrid.Components.Get<MyEntityThrustComponent>();
			if (myEntityThrustComponent == null)
			{
				return delta;
			}
			bool eNABLE_DEBUG_DRAW = MyDebugDrawSettings.ENABLE_DEBUG_DRAW;
			float num = 1f;
			int listLimit = 5;
			bool flag = true;
			Vector3D vector3D = delta;
			float num2 = base.CubeGrid.GetCurrentMass();
			float num3 = Math.Max(base.CubeGrid.Physics.LinearVelocity.Length(), 1f);
			float num4 = (num3 <= 1f) ? 1f : 1.25f;
			double num5 = myEntityThrustComponent.GetMaxThrustInDirection(m_currentDirection.Value) / num2;
			double num6 = Math.Min((double)num3 / num5 * (double)num3 / 2.0, MAX_STOPPING_DISTANCE);
			float radius = base.CubeGrid.PositionComp.LocalVolume.Radius;
			Vector3D vector3D2 = Vector3D.One;
			if (!Vector3D.IsZero(base.CubeGrid.Physics.LinearVelocity))
			{
				vector3D2 = Vector3D.Normalize(base.CubeGrid.Physics.LinearVelocity);
			}
			Vector3D vector3D3 = vector3D2 * num6;
			Vector3D shipFront = radius * vector3D2 + base.CubeGrid.PositionComp.WorldAABB.Center;
			_ = base.CubeGrid.PositionComp.WorldAABB.Center - radius * vector3D2;
			Vector3D vector3D4 = shipFront + vector3D2 * num6;
			MatrixD worldMatrix = base.CubeGrid.WorldMatrix;
			Vector3D v = Vector3D.Reject(worldMatrix.Up, vector3D2);
			bool flag2 = true;
			if (v.LengthSquared() < 0.25)
			{
				v = Vector3D.Reject(worldMatrix.Right, vector3D2);
				flag2 = false;
			}
			Quaternion quaternion = Quaternion.CreateFromForwardUp(vector3D2, v);
			MatrixD.CreateFromQuaternion(quaternion);
			float radius2 = base.CubeGrid.PositionComp.LocalVolume.Radius;
			Vector3D direction = Vector3D.TransformNormal(vector3D2, base.CubeGrid.PositionComp.WorldMatrixInvScaled);
			Vector3D vector3D5 = Vector3D.Reject(base.CubeGrid.PositionComp.LocalAABB.HalfExtents, direction);
			double x = Math.Abs(vector3D5.X) + Math.Abs(vector3D5.Z);
			double y = Math.Abs(vector3D5.Y) + Math.Abs(vector3D5.Z);
			if (!flag2)
			{
				x = Math.Abs(vector3D5.Y) + Math.Abs(vector3D5.Z);
				y = Math.Abs(vector3D5.Y) + Math.Abs(vector3D5.X);
			}
			Vector3D center = base.CubeGrid.PositionComp.WorldAABB.Center + (radius2 + num3) * vector3D2;
			Vector3D halfExtents = new Vector3D(x, y, radius2 + num3);
			MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(center, halfExtents, quaternion);
			new MyOrientedBoundingBoxD(vector3D4 + num * vector3D3 / 2.0, new Vector3D(base.CubeGrid.PositionComp.LocalVolume.Radius * num4, base.CubeGrid.PositionComp.LocalVolume.Radius * num4, (double)num * num6 / 2.0), quaternion);
			new BoundingBoxD(obb.Center - obb.HalfExtent, obb.Center + obb.HalfExtent);
			if (eNABLE_DEBUG_DRAW)
			{
				MyRenderProxy.DebugDrawOBB(obb, Color.Red, 0.25f, depthRead: false, smooth: false);
			}
			BoundingSphereD sphere = new BoundingSphereD(shipFront + vector3D3 / 2.0, num6 / 2.0);
			List<MyEntity> list = m_collisionEntityList ?? (m_collisionEntityList = new List<MyEntity>());
			List<MySlimBlock> list2 = m_collisionBlockList ?? (m_collisionBlockList = new List<MySlimBlock>());
			MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, list);
			MyEntity closestEntity = null;
			float closestEntityDist = float.MaxValue;
			bool flag3 = false;
			foreach (MyEntity item in list)
			{
				MyCubeGrid myCubeGrid;
				if ((myCubeGrid = (item as MyCubeGrid)) != null)
				{
					if (MyCubeGridGroups.Static.Physical.GetGroup(base.CubeGrid) == MyCubeGridGroups.Static.Physical.GetGroup(myCubeGrid) || (!myCubeGrid.IsStatic && myCubeGrid.PositionComp.LocalVolume.Radius < base.CubeGrid.PositionComp.LocalVolume.Radius * 0.05f))
					{
						continue;
					}
					MatrixD matrix = myCubeGrid.WorldMatrix;
					MatrixD boxTransform = MatrixD.CreateFromTransformScale(Quaternion.Normalize(obb.Orientation), obb.Center, Vector3D.One);
					myCubeGrid.GetBlocksIntersectingOBB(new BoundingBoxD(-obb.HalfExtent, obb.HalfExtent), boxTransform, list2);
					foreach (MySlimBlock item2 in list2)
					{
						item2.GetLocalMatrix(out Matrix localMatrix);
						Vector3 position = localMatrix.Translation;
						Vector3D.Transform(ref position, ref matrix, out Vector3D result);
						FillListOfDetectedObjects(result, myCubeGrid, ref listLimit, ref shipFront, ref closestEntityDist, ref closestEntity);
					}
					list2.Clear();
				}
				if (item is MyCharacter && flag)
				{
					if (item.Physics != null && !item.Physics.IsStatic && item.PositionComp.LocalVolume.Radius < base.CubeGrid.PositionComp.LocalVolume.Radius * 0.05f)
					{
						continue;
					}
					Vector3D point = item.WorldMatrix.Translation;
					if (obb.Contains(ref point))
					{
						FillListOfDetectedObjects(item.WorldMatrix.Translation, item, ref listLimit, ref shipFront, ref closestEntityDist, ref closestEntity);
					}
				}
				if (item is MyVoxelBase)
				{
					flag3 = true;
				}
			}
			m_collisionEntityList.Clear();
			bool flag4 = false;
			if (flag3)
			{
				Vector3D[] array = new Vector3D[8];
				obb.GetCorners(array, 0);
				for (int i = -1; i < 4; i++)
				{
					Vector3D vector3D6 = (i >= 0) ? array[i + 4] : vector3D4;
					MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(shipFront + vector3D2 * 0.1, vector3D6);
					if (hitInfo.HasValue)
					{
						MyEntity myEntity = hitInfo.Value.HkHitInfo.Body.UserObject as MyEntity;
						if (myEntity == null && hitInfo.Value.HkHitInfo.Body.UserObject is MyVoxelPhysicsBody)
						{
							myEntity = (((MyVoxelPhysicsBody)hitInfo.Value.HkHitInfo.Body.UserObject).Entity as MyEntity);
						}
						FillListOfDetectedObjects(hitInfo.Value.Position, myEntity, ref listLimit, ref shipFront, ref closestEntityDist, ref closestEntity);
						if (i == -1)
						{
							flag4 = true;
						}
					}
					if (eNABLE_DEBUG_DRAW)
					{
						MyRenderProxy.DebugDrawLine3D(shipFront, vector3D6, Color.Pink, Color.White, depthRead: false);
					}
				}
			}
			if (closestEntityDist < float.MaxValue)
			{
				m_rotateFor = 3f;
				int num7 = 0;
				Vector3D value = Vector3D.Zero;
				bool flag5 = false;
				if (!flag4)
				{
					for (int j = 0; j < m_detectedObstacles.Count; j++)
					{
						if (j == 4)
						{
							flag5 = true;
							break;
						}
						if (!m_detectedObstacles[j].IsVoxel)
						{
							break;
						}
					}
				}
				if (flag5)
				{
					value = vector3D4;
				}
				else
				{
					for (int k = 0; k < m_detectedObstacles.Count; k++)
					{
						if (k == 0 || m_detectedObstacles[k].Distance - m_detectedObstacles[0].Distance < 225f)
						{
							num7++;
							Vector3D vector = m_detectedObstacles[k].Position - worldMatrix.Translation;
							Vector3D value2 = Vector3D.Cross(Vector3D.Cross(delta, vector), delta);
							Vector3D vector3D7 = m_detectedObstacles[k].Position - Vector3D.Normalize(value2) * radius * num4 * 2.0;
							if (eNABLE_DEBUG_DRAW)
							{
								MyRenderProxy.DebugDrawLine3D(worldMatrix.Translation, vector3D7, Color.White, Color.Tomato, depthRead: false);
							}
							value += vector3D7;
						}
					}
					value /= (double)num7;
				}
				autopilotSpeedLimit = 1f + autopilotSpeedLimit * (float)Math.Sqrt(closestEntityDist) / (float)num6 * 0.5f;
				if (num7 < 5 || flag5)
				{
					delta = value - worldMatrix.Translation;
				}
				else if (closestEntity != null)
				{
					Vector3D center2 = closestEntity.PositionComp.WorldAABB.Center;
					Vector3D vector3D8 = center2 - base.WorldMatrix.Translation;
					if (closestEntity.Physics != null && !closestEntity.Physics.IsStatic && m_waitForFreeWay.Value)
					{
						vector3D8.Normalize();
						delta = -vector3D8 * base.CubeGrid.PositionComp.LocalVolume.Radius;
					}
					else
					{
						Vector3D value3 = Vector3D.Cross(Vector3D.Cross(delta, vector3D8), delta);
						delta = center2 - Vector3D.Normalize(value3) * base.CubeGrid.PositionComp.LocalVolume.Radius * num4 * 2.0 - center2;
						delta *= 2.0;
					}
					autopilotSpeedLimit *= 0.75f;
				}
				Vector3D vector2 = Vector3D.Normalize(delta);
				if ((float)Vector3D.Dot(vector2, Vector3D.Normalize(m_detectedObstacles[0].Position - base.WorldMatrix.Translation)) > 0.5f)
				{
					delta *= -1.0;
				}
				else if ((float)Vector3D.Dot(vector2, Vector3D.Normalize(vector3D)) < -0.5f)
				{
					delta = vector3D;
				}
				if (eNABLE_DEBUG_DRAW)
				{
					MyRenderProxy.DebugDrawLine3D(worldMatrix.Translation, worldMatrix.Translation + delta, Color.Red, Color.Aquamarine, depthRead: false);
				}
			}
			else if (eNABLE_DEBUG_DRAW && m_rotateFor <= 0f)
			{
				MyRenderProxy.DebugDrawLine3D(vector3D4, vector3D4 + vector3D3, Color.Yellow, Color.Green, depthRead: false);
			}
			m_detectedObstacles.Clear();
			if (closestEntityDist == float.MaxValue && m_rotateFor > 1.5f)
			{
				autopilotSpeedLimit = m_lastAutopilotSpeedLimit;
				return m_lastDelta;
			}
			m_lastAutopilotSpeedLimit = autopilotSpeedLimit;
			m_lastDelta = delta;
			return delta;
		}

		private Vector3D AvoidCollisions(Vector3D delta, ref float autopilotSpeedLimit)
		{
			if (m_collisionCtr <= 0)
			{
				m_collisionCtr = 0;
				bool eNABLE_DEBUG_DRAW = MyDebugDrawSettings.ENABLE_DEBUG_DRAW;
				Vector3D centerOfMassWorld = base.CubeGrid.Physics.CenterOfMassWorld;
				double num = base.CubeGrid.PositionComp.WorldVolume.Radius * 1.2999999523162842;
				if (MyFakes.ENABLE_VR_DRONE_COLLISIONS)
				{
					num = base.CubeGrid.PositionComp.WorldVolume.Radius * 1.0;
				}
				Vector3D vector3D = base.CubeGrid.Physics.LinearVelocity;
				double num2 = vector3D.Length();
				double num3 = base.CubeGrid.PositionComp.WorldVolume.Radius * 10.0 + num2 * num2 * 0.05;
				if (MyFakes.ENABLE_VR_DRONE_COLLISIONS)
				{
					num3 = base.CubeGrid.PositionComp.WorldVolume.Radius + num2 * num2 * 0.05;
				}
				BoundingSphereD boundingSphere = new BoundingSphereD(centerOfMassWorld, num3);
				Vector3D vector3D2 = boundingSphere.Center + vector3D * 2.0;
				if (MyFakes.ENABLE_VR_DRONE_COLLISIONS)
				{
					vector3D2 = boundingSphere.Center + vector3D;
				}
				if (eNABLE_DEBUG_DRAW)
				{
					MyRenderProxy.DebugDrawSphere(boundingSphere.Center, (float)num, Color.HotPink, 1f, depthRead: false);
					MyRenderProxy.DebugDrawSphere(boundingSphere.Center + vector3D, 1f, Color.HotPink, 1f, depthRead: false);
					MyRenderProxy.DebugDrawSphere(boundingSphere.Center, (float)num3, Color.White, 1f, depthRead: false);
				}
				Vector3D zero = Vector3D.Zero;
				Vector3D zero2 = Vector3D.Zero;
				int num4 = 0;
				double num5 = 0.0;
				List<MyEntity> topMostEntitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
				if (MyGravityProviderSystem.GetStrongestNaturalGravityWell(centerOfMassWorld, out IMyGravityProvider nearestProvider) > 0.0 && nearestProvider is MyGravityProviderComponent)
				{
					MyEntity item = (MyEntity)((MyGravityProviderComponent)nearestProvider).Entity;
					if (!topMostEntitiesInSphere.Contains(item))
					{
						topMostEntitiesInSphere.Add(item);
					}
				}
				for (int i = 0; i < topMostEntitiesInSphere.Count; i++)
				{
					MyEntity myEntity = topMostEntitiesInSphere[i];
					if (myEntity == base.Parent)
					{
						continue;
					}
					Vector3D vector3D3 = Vector3D.Zero;
					Vector3D zero3 = Vector3D.Zero;
					if (myEntity is MyCubeGrid || myEntity is MyVoxelMap || myEntity is MySkinnedEntity)
					{
						if (MyFakes.ENABLE_VR_DRONE_COLLISIONS && myEntity is MyCubeGrid && (myEntity as MyCubeGrid).IsStatic)
						{
							continue;
						}
						if (myEntity is MyCubeGrid)
						{
							MyCubeGrid node = myEntity as MyCubeGrid;
							if (MyCubeGridGroups.Static.Physical.GetGroup(base.CubeGrid) == MyCubeGridGroups.Static.Physical.GetGroup(node))
							{
								continue;
							}
						}
						BoundingSphereD worldVolume = myEntity.PositionComp.WorldVolume;
						worldVolume.Radius += num;
						Vector3D vector3D4 = worldVolume.Center - boundingSphere.Center;
						if (base.CubeGrid.Physics.LinearVelocity.LengthSquared() > 5f && Vector3D.Dot(delta, base.CubeGrid.Physics.LinearVelocity) < 0.0)
						{
							continue;
						}
						double num6 = vector3D4.Length();
						BoundingSphereD boundingSphereD = new BoundingSphereD(worldVolume.Center + vector3D, worldVolume.Radius + num2);
						if (boundingSphereD.Contains(vector3D2) == ContainmentType.Contains)
						{
							autopilotSpeedLimit = 2f;
							if (eNABLE_DEBUG_DRAW)
							{
								MyRenderProxy.DebugDrawSphere(boundingSphereD.Center, (float)boundingSphereD.Radius, Color.Red, 1f, depthRead: false);
							}
						}
						else if (Vector3D.Dot(vector3D4, vector3D) < 0.0)
						{
							if (eNABLE_DEBUG_DRAW)
							{
								MyRenderProxy.DebugDrawSphere(boundingSphereD.Center, (float)boundingSphereD.Radius, Color.Red, 1f, depthRead: false);
							}
						}
						else if (eNABLE_DEBUG_DRAW)
						{
							MyRenderProxy.DebugDrawSphere(boundingSphereD.Center, (float)boundingSphereD.Radius, Color.DarkOrange, 1f, depthRead: false);
						}
						double d = -0.693 * num6 / (worldVolume.Radius + base.CubeGrid.PositionComp.WorldVolume.Radius + num2);
						double scaleFactor = 2.0 * Math.Exp(d);
						double num7 = Math.Min(1.0, Math.Max(0.0, (0.0 - (boundingSphereD.Center - boundingSphere.Center).Length()) / boundingSphereD.Radius + 2.0));
						num5 = Math.Max(num5, num7);
						Vector3D value = vector3D4 / num6;
						vector3D3 = -value * scaleFactor;
						zero3 = -value * num7;
					}
					else
					{
						if (!(myEntity is MyPlanet))
						{
							continue;
						}
						MyPlanet obj = myEntity as MyPlanet;
						float gravityLimit = ((MySphericalNaturalGravityComponent)obj.Components.Get<MyGravityProviderComponent>()).GravityLimit;
						Vector3D translation = obj.WorldMatrix.Translation;
						Vector3D vector3D5 = translation - centerOfMassWorld;
						double num8 = vector3D5.Length() - (double)gravityLimit;
						if (num8 > PLANET_AVOIDANCE_RADIUS || num8 < 0.0 - PLANET_AVOIDANCE_TOLERANCE)
						{
							continue;
						}
						Vector3D vector3D6 = translation - m_currentWaypoint.Coords;
						if (Vector3D.IsZero(vector3D6))
						{
							vector3D6 = Vector3.Up;
						}
						else
						{
							vector3D6.Normalize();
						}
						Vector3D vector3D7 = translation + vector3D6 * gravityLimit;
						Vector3D vector3D8 = vector3D5;
						vector3D8.Normalize();
						double num9 = (vector3D7 - centerOfMassWorld).LengthSquared();
						if (num9 < PLANET_REPULSION_RADIUS * PLANET_REPULSION_RADIUS)
						{
							double amount = Math.Sqrt(num9) / PLANET_REPULSION_RADIUS;
							Vector3D vector3D9 = centerOfMassWorld - vector3D7;
							if (Vector3D.IsZero(vector3D9))
							{
								vector3D9 = Vector3D.CalculatePerpendicularVector(vector3D6);
							}
							else
							{
								vector3D9 = Vector3D.Reject(vector3D9, vector3D6);
								vector3D9.Normalize();
							}
							vector3D3 = Vector3D.Lerp(vector3D6, vector3D9, amount);
						}
						else
						{
							Vector3D vector3D10 = m_currentWaypoint.Coords - centerOfMassWorld;
							vector3D10.Normalize();
							if (Vector3D.Dot(vector3D10, vector3D8) > 0.0)
							{
								vector3D3 = Vector3D.Reject(vector3D10, vector3D8);
								if (Vector3D.IsZero(vector3D3))
								{
									vector3D3 = Vector3D.CalculatePerpendicularVector(vector3D8);
								}
								else
								{
									vector3D3.Normalize();
								}
							}
						}
						double num10 = (vector3D2 - translation).Length();
						if (num10 < (double)gravityLimit)
						{
							m_autopilotSpeedLimit.Value = 2f;
						}
						double num11 = ((double)gravityLimit + PLANET_AVOIDANCE_RADIUS - num10) / PLANET_AVOIDANCE_RADIUS;
						vector3D3 *= num11;
						zero3 = -vector3D8 * num11;
					}
					zero += vector3D3;
					zero2 += zero3;
					num4++;
				}
				topMostEntitiesInSphere.Clear();
				if (num4 > 0)
				{
					double num12 = delta.Length();
					delta /= num12;
					zero *= (1.0 - num5) * 0.10000000149011612 / (double)num4;
					_ = zero + delta;
					delta += zero + zero2;
					delta *= num12;
					if (eNABLE_DEBUG_DRAW)
					{
						MyRenderProxy.DebugDrawArrow3D(centerOfMassWorld, centerOfMassWorld + delta / num12 * 100.0, Color.Green, Color.Green);
						MyRenderProxy.DebugDrawArrow3D(centerOfMassWorld, centerOfMassWorld + zero2 * 100.0, Color.Red, Color.Red);
						MyRenderProxy.DebugDrawSphere(centerOfMassWorld, 100f, Color.Gray, 0.5f, depthRead: false);
					}
				}
				m_oldCollisionDelta = delta;
				return delta;
			}
			m_collisionCtr--;
			return m_oldCollisionDelta;
		}

		private void UpdateThrust(Vector3D delta, Vector3D perpDelta, double maxSpeed)
		{
			MyEntityThrustComponent myEntityThrustComponent = base.CubeGrid.Components.Get<MyEntityThrustComponent>();
			if (myEntityThrustComponent == null)
			{
				return;
			}
			myEntityThrustComponent.AutoPilotControlThrust = Vector3.Zero;
			if (!myEntityThrustComponent.Enabled)
			{
				return;
			}
			Matrix matrix = base.CubeGrid.PositionComp.WorldMatrixNormalizedInv.GetOrientation();
			Vector3D v = delta;
			v.Normalize();
			Vector3D v2 = base.CubeGrid.Physics.LinearVelocity;
			Vector3 vector = Vector3.Transform(v, matrix);
			Vector3.Transform(v2, matrix);
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(200f, 200f), $"Thrust: {myEntityThrustComponent.AutoPilotControlThrust}", Color.Magenta, 1f);
			}
			myEntityThrustComponent.AutoPilotControlThrust = Vector3.Zero;
			Vector3 zero = Vector3.Zero;
			zero.X = myEntityThrustComponent.GetMaxThrustInDirection((vector.X > 0f) ? Base6Directions.Direction.Left : Base6Directions.Direction.Right);
			zero.Y = myEntityThrustComponent.GetMaxThrustInDirection((vector.Y > 0f) ? Base6Directions.Direction.Down : Base6Directions.Direction.Up);
			zero.Z = myEntityThrustComponent.GetMaxThrustInDirection((vector.Z > 0f) ? Base6Directions.Direction.Backward : Base6Directions.Direction.Forward);
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(200f, 250f), $"Brake: {zero}", Color.Magenta, 1f);
			}
			float num = Math.Abs(vector.X) * zero.X + Math.Abs(vector.Y) * zero.Y + Math.Abs(vector.Z) * zero.Z;
			if (!(v2.Length() > 3.0) || !(v2.Dot(ref v) < 0.0))
			{
				Vector3D v3 = Vector3D.CalculatePerpendicularVector(v);
				Vector3D v4 = Vector3D.Cross(v, v3);
				Vector3D vector3D = v * v2.Dot(ref v);
				Vector3D value = v3 * v2.Dot(ref v3);
				Vector3D value2 = v4 * v2.Dot(ref v4);
				v2.Dot(ref m_currentInfo.GravityWorld);
				Vector3D position = value + value2;
				double num2 = delta.Length() / vector3D.Length();
				double num3 = v2.Length() * (double)base.CubeGrid.Physics.Mass / (double)num;
				if ((bool)m_dockingModeEnabled)
				{
					num3 *= 2.5;
				}
				if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(200f, 300f), $"Time to Target: {num2}", Color.Magenta, 1f);
					MyRenderProxy.DebugDrawText2D(new Vector2(200f, 350f), $"Time to Stop: {num3}", Color.Magenta, 1f);
				}
				if ((double.IsInfinity(num2) || double.IsInfinity(num3) || double.IsNaN(num3) || num2 > num3) && v2.LengthSquared() < maxSpeed * maxSpeed)
				{
					Vector3 autoPilotControlThrust = Vector3D.Transform(delta, matrix) - Vector3D.Transform(position, matrix);
					autoPilotControlThrust.Normalize();
					myEntityThrustComponent.AutoPilotControlThrust = autoPilotControlThrust;
				}
			}
		}

		private void ResetShipControls()
		{
			MyEntityThrustComponent myEntityThrustComponent = base.CubeGrid.Components.Get<MyEntityThrustComponent>();
			if (myEntityThrustComponent != null && myEntityThrustComponent.Enabled)
			{
				myEntityThrustComponent.DampenersEnabled = true;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyRemoteControl.GetNearestPlayer(out Vector3D playerPosition)
		{
			playerPosition = default(Vector3D);
			if (!MySession.Static.Players.IdentityIsNpc(base.OwnerId))
			{
				return false;
			}
			MyPlayer nearestPlayer = GetNearestPlayer();
			if (nearestPlayer == null)
			{
				return false;
			}
			playerPosition = nearestPlayer.Controller.ControlledEntity.Entity.WorldMatrix.Translation;
			return true;
		}

		bool Sandbox.ModAPI.IMyRemoteControl.GetNearestPlayer(out Vector3D playerPosition)
		{
			playerPosition = default(Vector3D);
			MyPlayer nearestPlayer = GetNearestPlayer();
			if (nearestPlayer == null)
			{
				return false;
			}
			playerPosition = nearestPlayer.Controller.ControlledEntity.Entity.WorldMatrix.Translation;
			return true;
		}

		public bool GetNearestPlayer(out MatrixD playerWorldTransform, Vector3 offset)
		{
			playerWorldTransform = MatrixD.Identity;
			if (!MySession.Static.Players.IdentityIsNpc(base.OwnerId))
			{
				return false;
			}
			MyPlayer nearestPlayer = GetNearestPlayer();
			if (nearestPlayer == null)
			{
				return false;
			}
			playerWorldTransform = nearestPlayer.Controller.ControlledEntity.Entity.WorldMatrix;
			Vector3 vector = Vector3.TransformNormal(offset, playerWorldTransform);
			playerWorldTransform.Translation += vector;
			return true;
		}

		public new Vector3D GetNaturalGravity()
		{
			return MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.WorldMatrix.Translation);
		}

		public MyPlayer GetNearestPlayer()
		{
			Vector3D translation = base.WorldMatrix.Translation;
			double num = double.MaxValue;
			MyPlayer result = null;
			foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
			{
				IMyControllableEntity controlledEntity = onlinePlayer.Controller.ControlledEntity;
				if (controlledEntity != null)
				{
					Vector3D translation2 = controlledEntity.Entity.WorldMatrix.Translation;
					double num2 = Vector3D.DistanceSquared(translation, translation2);
					if (num2 < num)
					{
						num = num2;
						result = onlinePlayer;
					}
				}
			}
			return result;
		}

		Vector3D Sandbox.ModAPI.IMyRemoteControl.GetFreeDestination(Vector3D originalDestination, float checkRadius, float shipRadius)
		{
			return GetFreeDestination(originalDestination, checkRadius, shipRadius);
		}

		private Vector3D GetFreeDestination(Vector3D originalDestination, float checkRadius, float shipRadius)
		{
			MyCestmirDebugInputComponent.ClearDebugSpheres();
			MyCestmirDebugInputComponent.ClearDebugPoints();
			MyCestmirDebugInputComponent.AddDebugPoint(base.WorldMatrix.Translation, Color.Green);
			if (shipRadius == 0f)
			{
				shipRadius = (float)base.CubeGrid.PositionComp.WorldVolume.Radius;
			}
			Vector3D vector3D = originalDestination;
			BoundingSphereD boundingSphere = new BoundingSphereD(base.WorldMatrix.Translation, shipRadius + checkRadius);
			Vector3D direction = originalDestination - base.WorldMatrix.Translation;
			double num = direction.Length();
			direction /= num;
			RayD rayD = new RayD(base.WorldMatrix.Translation, direction);
			double num2 = double.MaxValue;
			BoundingSphereD boundingSphereD = default(BoundingSphereD);
			List<MyEntity> topMostEntitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
			for (int i = 0; i < topMostEntitiesInSphere.Count; i++)
			{
				MyEntity myEntity = topMostEntitiesInSphere[i];
				if ((myEntity is MyCubeGrid || myEntity is MyVoxelMap) && myEntity.Parent == null && myEntity != base.Parent)
				{
					BoundingSphereD worldVolume = myEntity.PositionComp.WorldVolume;
					worldVolume.Radius += shipRadius;
					MyCestmirDebugInputComponent.AddDebugSphere(worldVolume.Center, (float)myEntity.PositionComp.WorldVolume.Radius, Color.Plum);
					MyCestmirDebugInputComponent.AddDebugSphere(worldVolume.Center, (float)myEntity.PositionComp.WorldVolume.Radius + shipRadius, Color.Purple);
					double? num3 = rayD.Intersects(worldVolume);
					if (num3.HasValue && num3.Value < num2)
					{
						num2 = num3.Value;
						boundingSphereD = worldVolume;
					}
				}
			}
			if (num2 != double.MaxValue)
			{
				Vector3D vector3D2 = rayD.Position + num2 * rayD.Direction;
				MyCestmirDebugInputComponent.AddDebugSphere(vector3D2, 1f, Color.Blue);
				Vector3D vector3D3 = vector3D2 - boundingSphereD.Center;
				if (Vector3D.IsZero(vector3D3))
				{
					vector3D3 = Vector3D.Up;
				}
				vector3D3.Normalize();
				MyCestmirDebugInputComponent.AddDebugSphere(vector3D2 + vector3D3, 1f, Color.Red);
				Vector3D value = Vector3D.Reject(rayD.Direction, vector3D3);
				value.Normalize();
				value *= Math.Max(20.0, boundingSphereD.Radius * 0.5);
				MyCestmirDebugInputComponent.AddDebugSphere(vector3D2 + value, 1f, Color.LightBlue);
				vector3D = vector3D2 + value;
			}
			else
			{
				vector3D = rayD.Position + rayD.Direction * Math.Min(checkRadius, num);
			}
			topMostEntitiesInSphere.Clear();
			return vector3D;
		}

		private bool TryFindSavedEntity()
		{
			if (m_savedPreviousControlledEntityId.HasValue && MyEntities.TryGetEntityById(m_savedPreviousControlledEntityId.Value, out MyEntity entity))
			{
				m_previousControlledEntity = (IMyControllableEntity)entity;
				if (m_previousControlledEntity != null)
				{
					AddPreviousControllerEvents();
					if (m_previousControlledEntity is MyCockpit)
					{
						cockpitPilot = (m_previousControlledEntity as MyCockpit).Pilot;
					}
					return true;
				}
			}
			return false;
		}

		public bool WasControllingCockpitWhenSaved()
		{
			if (m_savedPreviousControlledEntityId.HasValue && MyEntities.TryGetEntityById(m_savedPreviousControlledEntityId.Value, out MyEntity entity))
			{
				return entity is MyCockpit;
			}
			return false;
		}

		private void AddPreviousControllerEvents()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			m_previousControlledEntity.Entity.OnMarkForClose += Entity_OnPreviousMarkForClose;
			MyTerminalBlock myTerminalBlock = m_previousControlledEntity.Entity as MyTerminalBlock;
			if (myTerminalBlock != null)
			{
				myTerminalBlock.IsWorkingChanged += PreviousCubeBlock_IsWorkingChanged;
				MyCockpit myCockpit = m_previousControlledEntity.Entity as MyCockpit;
				if (myCockpit != null && myCockpit.Pilot != null)
				{
					myCockpit.Pilot.OnMarkForClose += Entity_OnPreviousMarkForClose;
				}
			}
		}

		private void PreviousCubeBlock_IsWorkingChanged(MyCubeBlock obj)
		{
			if (!obj.IsWorking && !obj.Closed && !obj.MarkedForClose)
			{
				MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.RequestRelease, arg2: false);
			}
		}

		private void Entity_OnPreviousMarkForClose(MyEntity obj)
		{
			MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.RequestRelease, arg2: true);
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_RemoteControl myObjectBuilder_RemoteControl = (MyObjectBuilder_RemoteControl)base.GetObjectBuilderCubeBlock(copy);
			if (m_previousControlledEntity != null)
			{
				myObjectBuilder_RemoteControl.PreviousControlledEntityId = m_previousControlledEntity.Entity.EntityId;
			}
			myObjectBuilder_RemoteControl.AutoPilotEnabled = m_autoPilotEnabled;
			myObjectBuilder_RemoteControl.DockingModeEnabled = m_dockingModeEnabled;
			myObjectBuilder_RemoteControl.FlightMode = (int)m_currentFlightMode.Value;
			myObjectBuilder_RemoteControl.Direction = (byte)m_currentDirection.Value;
			myObjectBuilder_RemoteControl.WaitForFreeWay = m_waitForFreeWay.Value;
			myObjectBuilder_RemoteControl.AutopilotSpeedLimit = m_autopilotSpeedLimit;
			myObjectBuilder_RemoteControl.WaypointThresholdDistance = m_waypointThresholdDistance;
			myObjectBuilder_RemoteControl.BindedCamera = m_bindedCamera.Value;
			myObjectBuilder_RemoteControl.IsMainRemoteControl = m_isMainRemoteControl;
			myObjectBuilder_RemoteControl.Waypoints = new List<MyObjectBuilder_AutopilotWaypoint>(m_waypoints.Count);
			foreach (MyAutopilotWaypoint waypoint in m_waypoints)
			{
				myObjectBuilder_RemoteControl.Waypoints.Add(waypoint.GetObjectBuilder());
			}
			if (CurrentWaypoint != null)
			{
				myObjectBuilder_RemoteControl.CurrentWaypointIndex = m_waypoints.IndexOf(CurrentWaypoint);
			}
			else
			{
				myObjectBuilder_RemoteControl.CurrentWaypointIndex = -1;
			}
			myObjectBuilder_RemoteControl.CollisionAvoidance = m_useCollisionAvoidance;
			myObjectBuilder_RemoteControl.AutomaticBehaviour = ((m_automaticBehaviour != null) ? m_automaticBehaviour.GetObjectBuilder() : null);
			return myObjectBuilder_RemoteControl;
		}

		public bool CanControl(IMyControllableEntity controllingEntity)
		{
			if (!CheckPreviousEntity(controllingEntity))
			{
				return false;
			}
			if ((bool)m_autoPilotEnabled)
			{
				return false;
			}
			UpdateIsWorking();
			if (base.IsWorking && PreviousControlledEntity == null)
			{
				return CheckRangeAndAccess(controllingEntity, controllingEntity.ControllerInfo.Controller.Player);
			}
			return false;
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(BlockDefinition.RequiredPowerInput, detailedInfo);
			detailedInfo.Append("\n");
			MyCharacter myCharacter = m_previousControlledEntity as MyCharacter;
			if (myCharacter != null && myCharacter != MySession.Static.LocalCharacter)
			{
				detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.RemoteControlUsedBy));
				detailedInfo.Append(myCharacter.DisplayNameText);
				detailedInfo.Append("\n");
			}
			if ((bool)m_autoPilotEnabled && CurrentWaypoint != null)
			{
				detailedInfo.Append("\n");
				detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.RemoteControlWaypoint));
				detailedInfo.Append(CurrentWaypoint.Name);
				detailedInfo.Append("\n");
				detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.RemoteControlCoord));
				detailedInfo.Append(CurrentWaypoint.Coords);
			}
		}

		protected override void ComponentStack_IsFunctionalChanged()
		{
			base.ComponentStack_IsFunctionalChanged();
			if (!base.IsWorking)
			{
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.RequestRelease, arg2: false);
				}
				if ((bool)m_autoPilotEnabled)
				{
					SetAutoPilotEnabled(enabled: false);
					if (m_automaticBehaviour != null)
					{
						m_automaticBehaviour.StopWorking();
					}
				}
			}
			base.ResourceSink.Update();
			RaisePropertiesChangedRemote();
			SetDetailedInfoDirty();
		}

		private void Receiver_RequiredInputChanged(MyDefinitionId resourceTypeId, MyResourceSinkComponent receiver, float oldRequirement, float newRequirement)
		{
			RaisePropertiesChangedRemote();
			SetDetailedInfoDirty();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			RaisePropertiesChangedRemote();
			SetDetailedInfoDirty();
			if (!base.IsWorking && Sync.IsServer)
			{
				m_releaseRequested = true;
			}
		}

		private float CalculateRequiredPowerInput()
		{
			if ((bool)m_autoPilotEnabled || base.ControllerInfo.Controller != null)
			{
				return BlockDefinition.RequiredPowerInput;
			}
			return 1E-07f;
		}

		public override void ShowTerminal()
		{
			MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, MySession.Static.LocalHumanPlayer.Character, this);
		}

		public void RequestControl()
		{
			if (MyFakes.ENABLE_REMOTE_CONTROL && MySession.Static.ControlledEntity != this)
			{
				if (MyGuiScreenTerminal.IsOpen)
				{
					MyGuiScreenTerminal.Hide();
				}
				if (MySession.Static.ControlledEntity != null)
				{
					RequestUse(UseActionEnum.Manipulate, MySession.Static.ControlledEntity);
				}
			}
		}

		private void AcquireControl()
		{
			AcquireControl(MySession.Static.ControlledEntity);
		}

		private void AcquireControl(IMyControllableEntity previousControlledEntity)
		{
			if (!CheckPreviousEntity(previousControlledEntity))
			{
				return;
			}
			if ((bool)m_autoPilotEnabled)
			{
				SetAutoPilotEnabled(enabled: false);
			}
			PreviousControlledEntity = previousControlledEntity;
			MyShipController myShipController = PreviousControlledEntity as MyShipController;
			if (myShipController != null)
			{
				m_enableFirstPerson = myShipController.EnableFirstPersonView;
				cockpitPilot = myShipController.Pilot;
				if (cockpitPilot != null)
				{
					cockpitPilot.CurrentRemoteControl = this;
				}
			}
			else
			{
				m_enableFirstPerson = true;
				MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					myCharacter.CurrentRemoteControl = this;
				}
			}
			if (MyCubeBuilder.Static.IsActivated)
			{
				MySession.Static.GameFocusManager.Clear();
			}
			SetEmissiveStateWorking();
			if (!Sync.IsServer && previousControlledEntity.ControllerInfo.IsLocallyControlled())
			{
				MyMultiplayer.GetReplicationClient()?.RequestReplicable(previousControlledEntity.Entity.GetTopMostParent().EntityId, 0, add: true);
			}
		}

		private bool CheckPreviousEntity(IMyControllableEntity entity)
		{
			if (entity is MyCharacter)
			{
				return true;
			}
			if (entity is MyCryoChamber)
			{
				return false;
			}
			if (entity is MyCockpit)
			{
				return true;
			}
			return false;
		}

		public void RequestControlFromLoad()
		{
			AcquireControl();
		}

		public void RequestUse(UseActionEnum actionEnum, IMyControllableEntity usedBy)
		{
			MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.RequestUseMessage, actionEnum, usedBy.Entity.EntityId);
		}

		[Event(null, 3198)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void RequestUseMessage(UseActionEnum useAction, long usedById)
		{
			MyEntity entity;
			bool num = MyEntities.TryGetEntityById<MyEntity>(usedById, out entity, allowClosed: false);
			IMyControllableEntity myControllableEntity = entity as IMyControllableEntity;
			UseActionResult useActionResult = UseActionResult.OK;
			if (num && (useActionResult = ((IMyUsableEntity)this).CanUse(useAction, myControllableEntity)) == UseActionResult.OK && CanControl(myControllableEntity))
			{
				UseSuccessCallback(useAction, usedById, useActionResult);
				MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.UseSuccessCallback, useAction, usedById, useActionResult);
				if (MyVisualScriptLogicProvider.RemoteControlChanged != null)
				{
					long playerId = 0L;
					if (base.ControllerInfo != null)
					{
						playerId = base.ControllerInfo.ControllingIdentityId;
					}
					MyVisualScriptLogicProvider.RemoteControlChanged(GotControlled: true, playerId, Name, base.EntityId, base.CubeGrid.Name, base.CubeGrid.EntityId);
				}
			}
			else if (MyEventContext.Current.IsLocallyInvoked)
			{
				UseFailureCallback(useAction, usedById, useActionResult);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.UseFailureCallback, useAction, usedById, useActionResult, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 3235)]
		[Reliable]
		[Broadcast]
		private void UseSuccessCallback(UseActionEnum useAction, long usedById, UseActionResult useResult)
		{
			if (!MyEntities.TryGetEntityById<MyEntity>(usedById, out MyEntity entity, allowClosed: false))
			{
				return;
			}
			IMyControllableEntity myControllableEntity = entity as IMyControllableEntity;
			if (myControllableEntity != null)
			{
				MyRelationsBetweenPlayerAndBlock relations = MyRelationsBetweenPlayerAndBlock.NoOwnership;
				if (this != null && myControllableEntity.ControllerInfo.Controller != null)
				{
					relations = GetUserRelationToOwner(myControllableEntity.ControllerInfo.Controller.Player.Identity.IdentityId);
				}
				if (relations.IsFriendly())
				{
					sync_UseSuccess(useAction, myControllableEntity);
				}
				else
				{
					sync_UseFailed(useAction, useResult, myControllableEntity);
				}
			}
		}

		[Event(null, 3265)]
		[Reliable]
		[Client]
		private void UseFailureCallback(UseActionEnum useAction, long usedById, UseActionResult useResult)
		{
			MyEntities.TryGetEntityById<MyEntity>(usedById, out MyEntity entity, allowClosed: false);
			IMyControllableEntity user = entity as IMyControllableEntity;
			sync_UseFailed(useAction, useResult, user);
		}

		private void sync_UseFailed(UseActionEnum actionEnum, UseActionResult actionResult, IMyControllableEntity user)
		{
			if (user != null && user.ControllerInfo.IsLocallyHumanControlled())
			{
				switch (actionResult)
				{
				case UseActionResult.UsedBySomeoneElse:
					MyHud.Notifications.Add(new MyHudNotification(MyCommonTexts.AlreadyUsedBySomebodyElse, 2500, "Red"));
					break;
				case UseActionResult.MissingDLC:
					MySession.Static.CheckDLCAndNotify(BlockDefinition);
					break;
				case UseActionResult.AccessDenied:
					MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
					break;
				case UseActionResult.Unpowered:
					MyHud.Notifications.Add(new MyHudNotification(MySpaceTexts.BlockIsNotPowered, 2500, "Red"));
					break;
				case UseActionResult.CockpitDamaged:
				{
					MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.Notification_ControllableBlockIsDamaged, 2500, "Red");
					object[] textFormatArguments = new string[1]
					{
						base.DefinitionDisplayNameText
					};
					myHudNotification.SetTextFormatArguments(textFormatArguments);
					MyHud.Notifications.Add(myHudNotification);
					break;
				}
				}
			}
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			if ((bool)m_autoPilotEnabled)
			{
				SetAutopilot(enabled: true);
			}
			if (base.CubeGrid.InScene && base.CubeGrid.GridSystems != null && base.CubeGrid.GridSystems.RadioSystem != null)
			{
				base.CubeGrid.GridSystems.RadioSystem.UpdateRemoteControlInfo();
			}
		}

		public override void OnUnregisteredFromGridSystems()
		{
			base.OnUnregisteredFromGridSystems();
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.RequestRelease, arg2: false);
			}
			if ((bool)m_autoPilotEnabled)
			{
				RemoveAutoPilot();
			}
			if (base.CubeGrid.GridSystems != null && base.CubeGrid.GridSystems.RadioSystem != null)
			{
				base.CubeGrid.GridSystems.RadioSystem.UpdateRemoteControlInfo();
			}
		}

		public override void ForceReleaseControl()
		{
			base.ForceReleaseControl();
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.RequestRelease, arg2: false);
			}
		}

		[Event(null, 3338)]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void RequestRelease(bool previousClosed)
		{
			if (!MyFakes.ENABLE_REMOTE_CONTROL)
			{
				return;
			}
			if (MyVisualScriptLogicProvider.RemoteControlChanged != null)
			{
				MyVisualScriptLogicProvider.RemoteControlChanged(GotControlled: false, (base.ControllerInfo == null) ? 0 : base.ControllerInfo.ControllingIdentityId, Name, base.EntityId, base.CubeGrid.Name, base.CubeGrid.EntityId);
			}
			if (m_previousControlledEntity != null)
			{
				if (m_previousControlledEntity is MyCockpit)
				{
					if (cockpitPilot != null)
					{
						cockpitPilot.CurrentRemoteControl = null;
					}
					MyCockpit myCockpit = m_previousControlledEntity as MyCockpit;
					if (previousClosed || myCockpit.Pilot == null)
					{
						ReturnControl(cockpitPilot);
						return;
					}
				}
				MyCharacter myCharacter = m_previousControlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					myCharacter.CurrentRemoteControl = null;
				}
				ReturnControl(m_previousControlledEntity);
				GetFirstRadioReceiver()?.Clear();
			}
			RefreshTerminal();
			SetEmissiveStateWorking();
			base.ResourceSink.Update();
		}

		private void RefreshTerminal()
		{
			if (Pilot != MySession.Static.LocalCharacter)
			{
				RaisePropertiesChanged();
				RaisePropertiesChangedRemote();
				SetDetailedInfoDirty();
			}
		}

		private void ReturnControl(IMyControllableEntity nextControllableEntity)
		{
			if (base.ControllerInfo.Controller != null)
			{
				this.SwitchControl(nextControllableEntity);
			}
			PreviousControlledEntity = null;
			if (!Sync.IsServer && nextControllableEntity != null && nextControllableEntity.ControllerInfo.IsLocallyControlled())
			{
				MyMultiplayer.GetReplicationClient()?.RequestReplicable(nextControllableEntity.Entity.GetTopMostParent().EntityId, 0, add: false);
			}
		}

		protected void sync_UseSuccess(UseActionEnum actionEnum, IMyControllableEntity user)
		{
			AcquireControl(user);
			if (user.ControllerInfo != null && user.ControllerInfo.Controller != null)
			{
				user.SwitchControl(this);
				RefreshTerminal();
			}
			if ((long)m_bindedCamera != 0L && user == MySession.Static.LocalCharacter)
			{
				if (MyEntities.TryGetEntityById(m_bindedCamera, out MyEntity entity))
				{
					MyCameraBlock myCameraBlock = entity as MyCameraBlock;
					if (myCameraBlock != null)
					{
						myCameraBlock.RequestSetView();
					}
					else if (Sync.IsServer)
					{
						m_bindedCamera.Value = 0L;
					}
				}
				else if (Sync.IsServer)
				{
					m_bindedCamera.Value = 0L;
				}
			}
			base.ResourceSink.Update();
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (!MyFakes.ENABLE_REMOTE_CONTROL)
			{
				return;
			}
			if (m_previousControlledEntity != null)
			{
				if (Sync.IsServer && !RemoteIsInRangeAndPlayerHasAccess())
				{
					MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.RequestRelease, arg2: false);
				}
				GetFirstRadioReceiver()?.UpdateHud(showMyself: true);
			}
			if ((bool)m_autoPilotEnabled)
			{
				ResetShipControls();
			}
		}

		private MyDataReceiver GetFirstRadioReceiver()
		{
			HashSet<MyDataReceiver> output = new HashSet<MyDataReceiver>();
			MyAntennaSystem.Static.GetEntityReceivers(base.CubeGrid, ref output, 0L);
			if (output.Count > 0)
			{
				return output.FirstElement();
			}
			return null;
		}

		private bool RemoteIsInRangeAndPlayerHasAccess()
		{
			if (base.ControllerInfo.Controller == null)
			{
				return false;
			}
			return CheckRangeAndAccess(PreviousControlledEntity, base.ControllerInfo.Controller.Player);
		}

		private bool CheckRangeAndAccess(IMyControllableEntity controlledEntity, MyPlayer player)
		{
			MyTerminalBlock myTerminalBlock = controlledEntity as MyTerminalBlock;
			if (myTerminalBlock == null)
			{
				MyCharacter myCharacter = controlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					return MyAntennaSystem.Static.CheckConnection(myCharacter, base.CubeGrid, player);
				}
				return true;
			}
			MyCubeGrid cubeGrid = myTerminalBlock.SlimBlock.CubeGrid;
			return MyAntennaSystem.Static.CheckConnection(cubeGrid, base.CubeGrid, player);
		}

		protected override void OnOwnershipChanged()
		{
			base.OnOwnershipChanged();
			if (PreviousControlledEntity != null && Sync.IsServer && base.ControllerInfo.Controller != null)
			{
				MyRelationsBetweenPlayerAndBlock userRelationToOwner = GetUserRelationToOwner(base.ControllerInfo.ControllingIdentityId);
				if (userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Enemies || userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Neutral)
				{
					RaiseControlledEntityUsed();
				}
			}
			if (base.CubeGrid.GridSystems != null && base.CubeGrid.GridSystems.RadioSystem != null)
			{
				base.CubeGrid.GridSystems.RadioSystem.UpdateRemoteControlInfo();
			}
		}

		protected override void OnControlledEntity_Used()
		{
			base.OnControlledEntity_Used();
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.RequestRelease, arg2: false);
			}
		}

		public override MatrixD GetHeadMatrix(bool includeY, bool includeX = true, bool forceHeadAnim = false, bool forceHeadBone = false)
		{
			if (m_previousControlledEntity != null)
			{
				return m_previousControlledEntity.GetHeadMatrix(includeY, includeX, forceHeadAnim);
			}
			return MatrixD.Identity;
		}

		public UseActionResult CanUse(UseActionEnum actionEnum, IMyControllableEntity user)
		{
			if (m_previousControlledEntity != null && user != m_previousControlledEntity)
			{
				return UseActionResult.UsedBySomeoneElse;
			}
			return UseActionResult.OK;
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public override bool SetEmissiveStateWorking()
		{
			if (base.IsWorking)
			{
				if (m_previousControlledEntity != null)
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, base.Render.RenderObjectIDs[0]);
				}
				if ((bool)m_autoPilotEnabled && m_automaticBehaviour != null)
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, base.Render.RenderObjectIDs[0]);
				}
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
			}
			return false;
		}

		public override void ShowInventory()
		{
			base.ShowInventory();
			if (m_enableShipControl)
			{
				MyCharacter user = GetUser();
				if (user != null)
				{
					MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, user, this);
				}
			}
		}

		private MyCharacter GetUser()
		{
			if (PreviousControlledEntity != null)
			{
				if (cockpitPilot != null)
				{
					return cockpitPilot;
				}
				MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					return myCharacter;
				}
				return null;
			}
			return null;
		}

		private void SendToolbarItemChanged(ToolbarItem item, int index, int waypointIndex)
		{
			if (!m_syncing)
			{
				MyMultiplayer.RaiseEvent(this, (MyRemoteControl x) => x.OnToolbarItemChanged, item, index, waypointIndex);
			}
		}

		[Event(null, 3652)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnToolbarItemChanged(ToolbarItem item, int index, int waypointIndex)
		{
			m_syncing = true;
			MyToolbarItem myToolbarItem = null;
			if (item.EntityID != 0L)
			{
				MyRemoteControl entity2;
				if (string.IsNullOrEmpty(item.GroupName))
				{
					if (MyEntities.TryGetEntityById(item.EntityID, out MyTerminalBlock entity))
					{
						MyObjectBuilder_ToolbarItemTerminalBlock myObjectBuilder_ToolbarItemTerminalBlock = MyToolbarItemFactory.TerminalBlockObjectBuilderFromBlock(entity);
						myObjectBuilder_ToolbarItemTerminalBlock._Action = item.Action;
						myObjectBuilder_ToolbarItemTerminalBlock.Parameters = item.Parameters;
						myToolbarItem = MyToolbarItemFactory.CreateToolbarItem(myObjectBuilder_ToolbarItemTerminalBlock);
					}
				}
				else if (MyEntities.TryGetEntityById(item.EntityID, out entity2))
				{
					MyCubeGrid cubeGrid = entity2.CubeGrid;
					string groupName = item.GroupName;
					MyBlockGroup myBlockGroup = cubeGrid.GridSystems.TerminalSystem.BlockGroups.Find((MyBlockGroup x) => x.Name.ToString() == groupName);
					if (myBlockGroup != null)
					{
						MyObjectBuilder_ToolbarItemTerminalGroup myObjectBuilder_ToolbarItemTerminalGroup = MyToolbarItemFactory.TerminalGroupObjectBuilderFromGroup(myBlockGroup);
						myObjectBuilder_ToolbarItemTerminalGroup._Action = item.Action;
						myObjectBuilder_ToolbarItemTerminalGroup.BlockEntityId = item.EntityID;
						myObjectBuilder_ToolbarItemTerminalGroup.Parameters = item.Parameters;
						myToolbarItem = MyToolbarItemFactory.CreateToolbarItem(myObjectBuilder_ToolbarItemTerminalGroup);
					}
				}
			}
			MyAutopilotWaypoint myAutopilotWaypoint = m_waypoints[waypointIndex];
			if (myAutopilotWaypoint.Actions == null)
			{
				myAutopilotWaypoint.InitActions();
			}
			myAutopilotWaypoint.Actions[index] = myToolbarItem;
			RaisePropertiesChangedRemote();
			m_syncing = false;
		}

		public void SetAutomaticBehaviour(IRemoteControlAutomaticBehaviour automaticBehaviour)
		{
			m_automaticBehaviour = automaticBehaviour;
		}

		public void RemoveAutomaticBehaviour()
		{
			m_automaticBehaviour = null;
		}

		private void SetMainRemoteControl(bool value)
		{
			if (value && base.CubeGrid.HasMainRemoteControl() && !base.CubeGrid.IsMainRemoteControl(this))
			{
				IsMainRemoteControl = false;
			}
			else
			{
				IsMainRemoteControl = value;
			}
		}

		private void MainRemoteControlChanged()
		{
			if ((bool)m_isMainRemoteControl)
			{
				base.CubeGrid.SetMainRemoteControl(this);
			}
			else if (base.CubeGrid.IsMainRemoteControl(this))
			{
				base.CubeGrid.SetMainRemoteControl(null);
			}
			if (base.CubeGrid.GridSystems != null && base.CubeGrid.GridSystems.RadioSystem != null)
			{
				base.CubeGrid.GridSystems.RadioSystem.UpdateRemoteControlInfo();
			}
		}

		protected bool IsMainRemoteControlFree()
		{
			if (base.CubeGrid.HasMainRemoteControl())
			{
				return base.CubeGrid.IsMainRemoteControl(this);
			}
			return true;
		}
	}
}
