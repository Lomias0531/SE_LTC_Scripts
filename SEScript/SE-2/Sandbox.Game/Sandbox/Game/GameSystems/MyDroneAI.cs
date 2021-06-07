using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.AI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
	public class MyDroneAI : MyRemoteControl.IRemoteControlAutomaticBehaviour
	{
		private MyRemoteControl m_remoteControl;

		private MyEntity3DSoundEmitter m_soundEmitter;

		private bool m_resetSound;

		private int m_frameCounter;

		public float m_maxPlayerDistance;

		public float m_maxPlayerDistanceSq;

		private bool m_rotateToTarget = true;

		private bool m_canRotateToTarget = true;

		private MyDroneAIData m_currentPreset;

		private bool m_avoidCollisions;

		private bool m_alternativebehaviorSwitched;

		private int m_waypointDelayMs;

		private int m_waypointReachedTimeMs;

		private Vector3D m_returnPosition;

		private int m_lostStartTimeMs;

		private int m_waypointStartTimeMs;

		private int m_lastTargetUpdate;

		private int m_lastWeaponUpdate;

		private bool m_farAwayFromTarget;

		private MyEntity m_currentTarget;

		private List<MyUserControllableGun> m_weapons = new List<MyUserControllableGun>();

		private List<MyFunctionalBlock> m_tools = new List<MyFunctionalBlock>();

		private bool m_shooting;

		private bool m_operational = true;

		private bool m_canSkipWaypoint = true;

		private bool m_cycleWaypoints;

		private List<MyEntity> m_forcedWaypoints = new List<MyEntity>();

		private List<DroneTarget> m_targetsList = new List<DroneTarget>();

		private List<DroneTarget> m_targetsFiltered = new List<DroneTarget>();

		private TargetPrioritization m_prioritizationStyle = TargetPrioritization.PriorityRandom;

		public bool m_loadItems = true;

		private bool m_loadEntities;

		private long m_loadCurrentTarget;

		private List<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable> m_loadTargetList;

		private List<long> m_loadWaypointList;

		private MyWeaponBehavior m_currentWeaponBehavior;

		private List<float> m_weaponBehaviorTimes = new List<float>();

		private List<int> m_weaponBehaviorAssignedRules = new List<int>();

		private List<bool> m_weaponBehaviorWeaponLock = new List<bool>();

		private float m_weaponBehaviorCooldown;

		private bool m_weaponBehaviorActive;

		public bool NeedUpdate
		{
			get;
			private set;
		}

		public bool IsActive
		{
			get;
			private set;
		}

		public bool RotateToTarget
		{
			get
			{
				if (m_canRotateToTarget)
				{
					return m_rotateToTarget;
				}
				return false;
			}
			set
			{
				m_rotateToTarget = value;
			}
		}

		public bool CollisionAvoidance
		{
			get
			{
				return m_avoidCollisions;
			}
			set
			{
				m_avoidCollisions = value;
			}
		}

		public Vector3D OriginPoint
		{
			get
			{
				return m_returnPosition;
			}
			set
			{
				m_returnPosition = value;
			}
		}

		public int PlayerPriority
		{
			get;
			set;
		}

		public TargetPrioritization PrioritizationStyle
		{
			get
			{
				return m_prioritizationStyle;
			}
			set
			{
				m_prioritizationStyle = value;
			}
		}

		public MyEntity CurrentTarget
		{
			get
			{
				return m_currentTarget;
			}
			set
			{
				m_currentTarget = value;
			}
		}

		public string CurrentBehavior
		{
			get
			{
				if (m_currentPreset == null)
				{
					return "";
				}
				return m_currentPreset.Name;
			}
		}

		public List<DroneTarget> TargetList => m_targetsFiltered;

		public List<MyEntity> WaypointList => m_forcedWaypoints;

		public bool WaypointActive => !m_canSkipWaypoint;

		public bool Ambushing
		{
			get;
			set;
		}

		public bool Operational => m_operational;

		public float SpeedLimit
		{
			get;
			set;
		}

		public float MaxPlayerDistance
		{
			get
			{
				return m_maxPlayerDistance;
			}
			private set
			{
				m_maxPlayerDistance = value;
				m_maxPlayerDistanceSq = value * value;
			}
		}

		public float PlayerYAxisOffset
		{
			get;
			private set;
		}

		public float WaypointThresholdDistance
		{
			get;
			private set;
		}

		public bool ResetStuckDetection => IsActive;

		public bool CycleWaypoints
		{
			get
			{
				return m_cycleWaypoints;
			}
			set
			{
				m_cycleWaypoints = value;
			}
		}

		public MyDroneAI()
		{
		}

		public MyDroneAI(MyRemoteControl remoteControl, string presetName, bool activate, List<MyEntity> waypoints, List<DroneTarget> targets, int playerPriority, TargetPrioritization prioritizationStyle, float maxPlayerDistance, bool cycleWaypoints)
		{
			m_remoteControl = remoteControl;
			m_returnPosition = m_remoteControl.PositionComp.GetPosition();
			m_currentPreset = MyDroneAIDataStatic.LoadPreset(presetName);
			Ambushing = false;
			LoadDroneAIData();
			m_lastTargetUpdate = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_lastWeaponUpdate = m_lastTargetUpdate;
			m_waypointReachedTimeMs = m_lastTargetUpdate;
			m_forcedWaypoints = (waypoints ?? new List<MyEntity>());
			m_targetsList = (targets ?? new List<DroneTarget>());
			PlayerPriority = playerPriority;
			m_prioritizationStyle = prioritizationStyle;
			MaxPlayerDistance = maxPlayerDistance;
			m_cycleWaypoints = cycleWaypoints;
			NeedUpdate = activate;
		}

		private void LoadDroneAIData()
		{
			if (m_currentPreset == null)
			{
				return;
			}
			m_avoidCollisions = m_currentPreset.AvoidCollisions;
			m_rotateToTarget = m_currentPreset.RotateToPlayer;
			PlayerYAxisOffset = m_currentPreset.PlayerYAxisOffset;
			WaypointThresholdDistance = m_currentPreset.WaypointThresholdDistance;
			SpeedLimit = m_currentPreset.SpeedLimit;
			if (string.IsNullOrEmpty(m_currentPreset.SoundLoop))
			{
				if (m_soundEmitter != null)
				{
					m_soundEmitter.StopSound(forced: true);
				}
				return;
			}
			if (m_soundEmitter == null)
			{
				m_soundEmitter = new MyEntity3DSoundEmitter(m_remoteControl, useStaticList: true);
			}
			MySoundPair mySoundPair = new MySoundPair(m_currentPreset.SoundLoop);
			if (mySoundPair != MySoundPair.Empty)
			{
				m_soundEmitter.PlaySound(mySoundPair, stopPrevious: true);
			}
		}

		public void Load(MyObjectBuilder_AutomaticBehaviour objectBuilder, MyRemoteControl remoteControl)
		{
			MyObjectBuilder_DroneAI myObjectBuilder_DroneAI = objectBuilder as MyObjectBuilder_DroneAI;
			if (myObjectBuilder_DroneAI != null)
			{
				m_remoteControl = remoteControl;
				m_currentPreset = MyDroneAIDataStatic.LoadPreset(myObjectBuilder_DroneAI.CurrentPreset);
				LoadDroneAIData();
				m_lastTargetUpdate = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				m_lastWeaponUpdate = m_lastTargetUpdate;
				m_waypointReachedTimeMs = m_lastTargetUpdate;
				m_forcedWaypoints = new List<MyEntity>();
				m_loadWaypointList = myObjectBuilder_DroneAI.WaypointList;
				m_targetsList = new List<DroneTarget>();
				m_loadTargetList = myObjectBuilder_DroneAI.TargetList;
				m_currentTarget = null;
				m_loadCurrentTarget = myObjectBuilder_DroneAI.CurrentTarget;
				Ambushing = myObjectBuilder_DroneAI.InAmbushMode;
				m_returnPosition = myObjectBuilder_DroneAI.ReturnPosition;
				PlayerPriority = myObjectBuilder_DroneAI.PlayerPriority;
				m_prioritizationStyle = myObjectBuilder_DroneAI.PrioritizationStyle;
				MaxPlayerDistance = myObjectBuilder_DroneAI.MaxPlayerDistance;
				m_cycleWaypoints = myObjectBuilder_DroneAI.CycleWaypoints;
				m_alternativebehaviorSwitched = myObjectBuilder_DroneAI.AlternativebehaviorSwitched;
				CollisionAvoidance = myObjectBuilder_DroneAI.CollisionAvoidance;
				m_canSkipWaypoint = myObjectBuilder_DroneAI.CanSkipWaypoint;
				if (myObjectBuilder_DroneAI.SpeedLimit != float.MinValue)
				{
					SpeedLimit = myObjectBuilder_DroneAI.SpeedLimit;
				}
				NeedUpdate = myObjectBuilder_DroneAI.NeedUpdate;
				IsActive = myObjectBuilder_DroneAI.IsActive;
				m_loadEntities = true;
			}
		}

		public MyObjectBuilder_AutomaticBehaviour GetObjectBuilder()
		{
			MyObjectBuilder_DroneAI myObjectBuilder_DroneAI = new MyObjectBuilder_DroneAI();
			myObjectBuilder_DroneAI.CollisionAvoidance = CollisionAvoidance;
			myObjectBuilder_DroneAI.CurrentTarget = ((m_currentTarget != null) ? m_currentTarget.EntityId : 0);
			myObjectBuilder_DroneAI.CycleWaypoints = m_cycleWaypoints;
			myObjectBuilder_DroneAI.IsActive = IsActive;
			myObjectBuilder_DroneAI.MaxPlayerDistance = m_maxPlayerDistance;
			myObjectBuilder_DroneAI.NeedUpdate = NeedUpdate;
			myObjectBuilder_DroneAI.InAmbushMode = Ambushing;
			myObjectBuilder_DroneAI.PlayerPriority = PlayerPriority;
			myObjectBuilder_DroneAI.PrioritizationStyle = m_prioritizationStyle;
			myObjectBuilder_DroneAI.TargetList = new List<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable>();
			foreach (DroneTarget targets in m_targetsList)
			{
				if (targets.Target != null)
				{
					myObjectBuilder_DroneAI.TargetList.Add(new MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable(targets.Target.EntityId, targets.Priority));
				}
			}
			myObjectBuilder_DroneAI.WaypointList = new List<long>();
			foreach (MyEntity forcedWaypoint in m_forcedWaypoints)
			{
				if (forcedWaypoint != null)
				{
					myObjectBuilder_DroneAI.WaypointList.Add(forcedWaypoint.EntityId);
				}
			}
			myObjectBuilder_DroneAI.CurrentPreset = m_currentPreset.Name;
			myObjectBuilder_DroneAI.AlternativebehaviorSwitched = m_alternativebehaviorSwitched;
			myObjectBuilder_DroneAI.ReturnPosition = m_returnPosition;
			myObjectBuilder_DroneAI.CanSkipWaypoint = m_canSkipWaypoint;
			myObjectBuilder_DroneAI.SpeedLimit = SpeedLimit;
			return myObjectBuilder_DroneAI;
		}

		public void LoadShipGear()
		{
			m_loadItems = false;
			m_remoteControl.CubeGrid.GetBlocks();
			m_weapons = new List<MyUserControllableGun>();
			m_tools = new List<MyFunctionalBlock>();
			foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.Logical.GetGroupNodes(m_remoteControl.CubeGrid))
			{
				foreach (MySlimBlock block in groupNode.GetBlocks())
				{
					if (block.FatBlock is MyUserControllableGun)
					{
						m_weapons.Add(block.FatBlock as MyUserControllableGun);
					}
					if (block.FatBlock is MyShipToolBase)
					{
						m_tools.Add(block.FatBlock as MyFunctionalBlock);
					}
					if (block.FatBlock is MyShipDrill)
					{
						m_tools.Add(block.FatBlock as MyFunctionalBlock);
					}
				}
			}
		}

		public void LoadEntities()
		{
			m_loadEntities = false;
			if (m_loadWaypointList != null)
			{
				foreach (long loadWaypoint in m_loadWaypointList)
				{
					if (loadWaypoint > 0 && MyEntities.TryGetEntityById(loadWaypoint, out MyEntity entity))
					{
						m_forcedWaypoints.Add(entity);
					}
				}
				m_loadWaypointList.Clear();
			}
			if (m_loadTargetList != null)
			{
				foreach (MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable loadTarget in m_loadTargetList)
				{
					if (loadTarget.TargetId > 0 && MyEntities.TryGetEntityById(loadTarget.TargetId, out MyEntity entity2))
					{
						m_targetsList.Add(new DroneTarget(entity2, loadTarget.Priority));
					}
				}
				m_targetsList.Clear();
			}
			if (m_loadCurrentTarget > 0)
			{
				MyEntities.TryGetEntityById(m_loadCurrentTarget, out MyEntity entity3);
				m_currentTarget = entity3;
			}
		}

		public void StopWorking()
		{
			if (m_soundEmitter != null && m_soundEmitter.IsPlaying)
			{
				m_soundEmitter.StopSound(forced: false);
				m_resetSound = true;
			}
		}

		public void Update()
		{
			m_frameCounter++;
			if (m_resetSound)
			{
				MySoundPair mySoundPair = new MySoundPair(m_currentPreset.SoundLoop);
				if (mySoundPair != MySoundPair.Empty)
				{
					m_soundEmitter.PlaySound(mySoundPair, stopPrevious: true);
				}
				m_resetSound = false;
			}
			if (m_soundEmitter != null && m_frameCounter % 100 == 0)
			{
				m_soundEmitter.Update();
			}
			if (Sync.IsServer)
			{
				if (m_loadItems)
				{
					LoadShipGear();
				}
				if (m_loadEntities)
				{
					LoadEntities();
				}
				if (IsActive || NeedUpdate)
				{
					UpdateWaypoint();
				}
			}
		}

		private void UpdateWaypoint()
		{
			int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (m_currentTarget != null && totalGamePlayTimeInMilliseconds - m_lastTargetUpdate >= 1000)
			{
				m_lastTargetUpdate = totalGamePlayTimeInMilliseconds;
				if (!IsValidTarget(m_currentTarget))
				{
					m_currentTarget = null;
				}
			}
			if ((m_currentTarget == null && totalGamePlayTimeInMilliseconds - m_lastTargetUpdate >= 1000) || totalGamePlayTimeInMilliseconds - m_lostStartTimeMs >= m_currentPreset.LostTimeMs)
			{
				FindNewTarget();
				m_lastTargetUpdate = totalGamePlayTimeInMilliseconds;
				if (m_currentTarget != null)
				{
					m_lostStartTimeMs = totalGamePlayTimeInMilliseconds;
					m_farAwayFromTarget = true;
				}
			}
			if (!Ambushing && m_farAwayFromTarget && totalGamePlayTimeInMilliseconds - m_lastTargetUpdate >= 5000 && m_canSkipWaypoint)
			{
				m_lastTargetUpdate = totalGamePlayTimeInMilliseconds;
				NeedUpdate = true;
			}
			float num = -1f;
			if (m_weaponBehaviorCooldown > 0f)
			{
				m_weaponBehaviorCooldown -= 0.0166666675f;
			}
			if (m_operational && totalGamePlayTimeInMilliseconds - m_lastWeaponUpdate >= 300)
			{
				m_lastWeaponUpdate = totalGamePlayTimeInMilliseconds;
				num = ((m_currentTarget != null) ? Vector3.DistanceSquared(m_currentTarget.PositionComp.GetPosition(), m_remoteControl.PositionComp.GetPosition()) : (-1f));
				if (!m_currentPreset.UsesWeaponBehaviors || m_weaponBehaviorCooldown <= 0f)
				{
					WeaponsUpdate(num);
				}
				m_canRotateToTarget = (num < m_currentPreset.RotationLimitSq && num >= 0f);
			}
			if (!m_operational || m_shooting)
			{
				m_lostStartTimeMs = totalGamePlayTimeInMilliseconds;
			}
			if (!Ambushing && totalGamePlayTimeInMilliseconds - m_waypointReachedTimeMs >= m_currentPreset.WaypointMaxTime && m_canSkipWaypoint)
			{
				NeedUpdate = true;
			}
			if (!Ambushing && m_remoteControl.CurrentWaypoint == null && WaypointList.Count > 0)
			{
				NeedUpdate = true;
			}
			if (!NeedUpdate)
			{
				return;
			}
			IsActive = true;
			if (num < 0f && m_currentTarget != null)
			{
				num = Vector3.DistanceSquared(m_currentTarget.PositionComp.GetPosition(), m_remoteControl.PositionComp.GetPosition());
			}
			m_farAwayFromTarget = (num > m_currentPreset.MaxManeuverDistanceSq);
			m_canRotateToTarget = (num < m_currentPreset.RotationLimitSq && num >= 0f);
			bool needUpdate = NeedUpdate;
			if (m_remoteControl.HasWaypoints())
			{
				m_remoteControl.ClearWaypoints();
			}
			m_remoteControl.SetAutoPilotEnabled(enabled: true);
			NeedUpdate = needUpdate;
			m_canSkipWaypoint = true;
			string name = "Player Vicinity";
			Vector3D pos;
			if (m_forcedWaypoints.Count > 0)
			{
				if (m_cycleWaypoints)
				{
					m_forcedWaypoints.Add(m_forcedWaypoints[0]);
				}
				pos = m_forcedWaypoints[0].PositionComp.GetPosition();
				name = m_forcedWaypoints[0].Name;
				m_forcedWaypoints.RemoveAt(0);
				m_canSkipWaypoint = false;
			}
			else if (m_currentTarget == null)
			{
				pos = m_remoteControl.WorldMatrix.Translation + Vector3.One * 0.01f;
			}
			else if (!m_operational && m_currentPreset.UseKamikazeBehavior)
			{
				if (m_remoteControl.TargettingAimDelta > 0.019999999552965164)
				{
					return;
				}
				pos = m_currentTarget.PositionComp.GetPosition() + m_currentTarget.WorldMatrix.Up * PlayerYAxisOffset * 2.0 - Vector3D.Normalize(m_remoteControl.PositionComp.GetPosition() - m_currentTarget.PositionComp.GetPosition()) * m_currentPreset.KamikazeBehaviorDistance;
			}
			else if (!m_operational && !m_currentPreset.UseKamikazeBehavior)
			{
				pos = m_returnPosition + Vector3.One * 0.01f;
			}
			else if (m_farAwayFromTarget)
			{
				pos = m_currentTarget.PositionComp.GetPosition() + Vector3D.Normalize(m_remoteControl.PositionComp.GetPosition() - m_currentTarget.PositionComp.GetPosition()) * m_currentPreset.PlayerTargetDistance;
				if (m_currentPreset.UsePlanetHover)
				{
					HoverMechanic(ref pos);
				}
			}
			else
			{
				if (totalGamePlayTimeInMilliseconds - m_waypointReachedTimeMs <= m_waypointDelayMs)
				{
					return;
				}
				pos = GetRandomPoint();
				name = "Strafe";
				if (m_currentPreset.UsePlanetHover)
				{
					HoverMechanic(ref pos);
				}
			}
			(pos - m_remoteControl.WorldMatrix.Translation).Normalize();
			m_waypointReachedTimeMs = totalGamePlayTimeInMilliseconds;
			bool flag = m_currentPreset.UseKamikazeBehavior && !m_operational;
			m_remoteControl.ChangeFlightMode(FlightMode.OneWay);
			m_remoteControl.SetAutoPilotSpeedLimit(flag ? 100f : SpeedLimit);
			m_remoteControl.SetCollisionAvoidance(!flag && m_canSkipWaypoint && m_avoidCollisions);
			m_remoteControl.ChangeDirection(Base6Directions.Direction.Forward);
			m_remoteControl.AddWaypoint(pos, name);
			NeedUpdate = false;
			IsActive = true;
		}

		public void DebugDraw()
		{
			if (m_remoteControl.CurrentWaypoint != null)
			{
				MyRenderProxy.DebugDrawSphere((Vector3)m_remoteControl.CurrentWaypoint.Coords, 0.5f, Color.Aquamarine);
			}
			if (m_currentTarget != null)
			{
				MyRenderProxy.DebugDrawSphere((Vector3)m_currentTarget.PositionComp.GetPosition(), 2f, m_canRotateToTarget ? Color.Green : Color.Red);
			}
		}

		private void HoverMechanic(ref Vector3D pos)
		{
			Vector3 v = MyGravityProviderSystem.CalculateNaturalGravityInPoint(pos);
			if (!(v.LengthSquared() > 0f))
			{
				return;
			}
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(pos);
			if (closestPlanet != null)
			{
				Vector3D closestSurfacePointGlobal = closestPlanet.GetClosestSurfacePointGlobal(ref pos);
				float num = (float)Vector3D.Distance(closestSurfacePointGlobal, pos);
				if (Vector3D.DistanceSquared(closestPlanet.PositionComp.GetPosition(), closestSurfacePointGlobal) > Vector3D.DistanceSquared(closestPlanet.PositionComp.GetPosition(), pos))
				{
					num *= -1f;
				}
				if (num < m_currentPreset.PlanetHoverMin)
				{
					pos = closestSurfacePointGlobal - Vector3D.Normalize(v) * m_currentPreset.PlanetHoverMin;
				}
				else if (num > m_currentPreset.PlanetHoverMax)
				{
					pos = closestSurfacePointGlobal - Vector3D.Normalize(v) * m_currentPreset.PlanetHoverMax;
				}
			}
		}

		private Vector3D GetRandomPoint()
		{
			int num = 0;
			MatrixD matrixD = MatrixD.CreateFromDir(Vector3D.Normalize(m_currentTarget.PositionComp.GetPosition() - m_remoteControl.PositionComp.GetPosition()));
			Vector3D vector3D;
			do
			{
				Vector3D value = matrixD.Right * MyUtils.GetRandomFloat(0f - m_currentPreset.Width, m_currentPreset.Width);
				Vector3D value2 = matrixD.Up * MyUtils.GetRandomFloat(0f - m_currentPreset.Height, m_currentPreset.Height);
				Vector3D value3 = matrixD.Forward * MyUtils.GetRandomFloat(0f - m_currentPreset.Depth, m_currentPreset.Depth);
				vector3D = m_remoteControl.PositionComp.GetPosition() + value + value2 + value3;
			}
			while (!((float)(vector3D - m_remoteControl.PositionComp.GetPosition()).LengthSquared() > m_currentPreset.MinStrafeDistanceSq) && ++num < 10);
			return vector3D;
		}

		private bool IsValidTarget(MyEntity target)
		{
			if (target is MyCharacter && !((MyCharacter)target).IsDead)
			{
				return true;
			}
			if (target is MyCubeBlock)
			{
				return ((MyCubeBlock)target).IsFunctional;
			}
			if (target is MyCubeGrid)
			{
				return ((MyCubeGrid)target).IsPowered;
			}
			return false;
		}

		private bool FindNewTarget()
		{
			List<DroneTarget> list = new List<DroneTarget>();
			if (PlayerPriority > 0)
			{
				long ownerId = m_remoteControl.OwnerId;
				foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
				{
					if (MyIDModule.GetRelationPlayerPlayer(ownerId, onlinePlayer.Identity.IdentityId) == MyRelationsBetweenPlayers.Enemies)
					{
						IMyControllableEntity controlledEntity = onlinePlayer.Controller.ControlledEntity;
						if (controlledEntity != null && (!(controlledEntity is MyCharacter) || !((MyCharacter)controlledEntity).IsDead))
						{
							Vector3D translation = controlledEntity.Entity.WorldMatrix.Translation;
							if (Vector3D.DistanceSquared(m_remoteControl.PositionComp.GetPosition(), translation) < (double)m_maxPlayerDistanceSq)
							{
								list.Add(new DroneTarget((MyEntity)controlledEntity, PlayerPriority));
							}
						}
					}
				}
			}
			for (int i = 0; i < m_targetsList.Count; i++)
			{
				if (!IsValidTarget(m_targetsList[i].Target))
				{
					continue;
				}
				MyCubeGrid myCubeGrid;
				if ((myCubeGrid = (m_targetsList[i].Target as MyCubeGrid)) != null)
				{
					ListReader<MyCubeBlock> fatBlocks = myCubeGrid.GetFatBlocks();
					if (fatBlocks.Count > 0)
					{
						int index = MyRandom.Instance.Next(0, fatBlocks.Count);
						MyCubeBlock target = fatBlocks[index];
						if (IsValidTarget(target))
						{
							DroneTarget item = new DroneTarget(target, m_targetsList[i].Priority);
							list.Add(item);
						}
					}
				}
				else
				{
					list.Add(m_targetsList[i]);
				}
			}
			m_targetsFiltered.Clear();
			m_targetsFiltered = list;
			if (list.Count == 0)
			{
				return false;
			}
			bool flag = m_prioritizationStyle == TargetPrioritization.Random;
			switch (m_prioritizationStyle)
			{
			default:
				list.Sort();
				m_currentTarget = list[list.Count - 1].Target;
				return true;
			case TargetPrioritization.ClosestFirst:
			{
				double num4 = double.MaxValue;
				foreach (DroneTarget item2 in list)
				{
					double num5 = Vector3D.DistanceSquared(m_remoteControl.PositionComp.GetPosition(), item2.Target.PositionComp.GetPosition());
					if (num5 < num4)
					{
						num4 = num5;
						m_currentTarget = item2.Target;
					}
				}
				return true;
			}
			case TargetPrioritization.PriorityRandom:
			case TargetPrioritization.Random:
			{
				int num = 0;
				foreach (DroneTarget item3 in list)
				{
					num += (flag ? 1 : Math.Max(0, item3.Priority));
				}
				int num2 = MyUtils.GetRandomInt(0, num) + 1;
				foreach (DroneTarget item4 in list)
				{
					int num3 = flag ? 1 : Math.Max(0, item4.Priority);
					if (num2 <= num3)
					{
						m_currentTarget = item4.Target;
						break;
					}
					num2 -= num3;
				}
				return true;
			}
			}
		}

		public void TargetAdd(DroneTarget target)
		{
			if (!m_targetsList.Contains(target))
			{
				m_targetsList.Add(target);
			}
		}

		public void TargetClear()
		{
			m_targetsList.Clear();
		}

		public void TargetLoseCurrent()
		{
			m_currentTarget = null;
		}

		public void TargetRemove(MyEntity target)
		{
			for (int i = 0; i < m_targetsList.Count; i++)
			{
				if (m_targetsList[i].Target == target)
				{
					m_targetsList.RemoveAt(i);
					i--;
				}
			}
		}

		public void WaypointAdd(MyEntity target)
		{
			if (target != null && !m_forcedWaypoints.Contains(target))
			{
				m_forcedWaypoints.Add(target);
			}
		}

		public void WaypointClear()
		{
			m_forcedWaypoints.Clear();
		}

		public void WaypointAdvanced()
		{
			if (Sync.IsServer)
			{
				m_waypointReachedTimeMs = MySandboxGame.TotalGamePlayTimeInMilliseconds + MyUtils.GetRandomInt(m_currentPreset.WaypointDelayMsMin, m_currentPreset.WaypointDelayMsMax);
				if (!Ambushing && IsActive && (m_remoteControl.CurrentWaypoint != null || m_targetsFiltered.Count > 0 || m_forcedWaypoints.Count > 0))
				{
					NeedUpdate = true;
				}
			}
		}

		private void RaycastCheck(Vector3 from, out bool hitVoxel, out bool hitGrid)
		{
			hitVoxel = false;
			hitGrid = false;
			Vector3 vector = m_currentTarget.WorldMatrix.Translation;
			if (m_currentTarget is MyCharacter)
			{
				vector += m_currentTarget.WorldMatrix.Up * PlayerYAxisOffset;
			}
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(from, vector, 15);
			IMyEntity myEntity = null;
			if (hitInfo.HasValue && hitInfo.Value.HkHitInfo.Body != null && hitInfo.Value.HkHitInfo.Body.UserObject != null && hitInfo.Value.HkHitInfo.Body.UserObject is MyPhysicsBody)
			{
				myEntity = ((MyPhysicsBody)hitInfo.Value.HkHitInfo.Body.UserObject).Entity;
			}
			if (myEntity != null && m_currentTarget != myEntity && m_currentTarget.Parent != myEntity && (m_currentTarget.Parent == null || m_currentTarget.Parent != myEntity.Parent) && !(myEntity is MyMissile) && !(myEntity is MyFloatingObject))
			{
				if (myEntity is MyVoxelBase)
				{
					hitVoxel = true;
				}
				else
				{
					hitGrid = true;
				}
			}
		}

		private void ChangeWeaponBehavior()
		{
			m_currentWeaponBehavior = null;
			m_weaponBehaviorTimes.Clear();
			m_weaponBehaviorAssignedRules.Clear();
			m_weaponBehaviorWeaponLock.Clear();
			if (m_currentTarget == null)
			{
				m_weaponBehaviorCooldown = m_currentPreset.WeaponBehaviorNotFoundDelay;
				return;
			}
			List<int> list = new List<int>();
			int num = 0;
			bool hitVoxel = false;
			bool hitGrid = false;
			Vector3 from = m_remoteControl.CubeGrid.PositionComp.LocalVolume.Radius * m_remoteControl.CubeGrid.WorldMatrix.Forward * 1.1000000238418579 + m_remoteControl.CubeGrid.PositionComp.WorldAABB.Center;
			RaycastCheck(from, out hitVoxel, out hitGrid);
			foreach (MyWeaponBehavior weaponBehavior in m_currentPreset.WeaponBehaviors)
			{
				bool flag = true;
				if (!weaponBehavior.IgnoresVoxels && hitVoxel)
				{
					flag = false;
				}
				if (!weaponBehavior.IgnoresGrids && hitGrid)
				{
					flag = false;
				}
				bool flag2 = false;
				if (flag && weaponBehavior.WeaponRules.Count > 0)
				{
					if (weaponBehavior.RequirementsIsWhitelist || weaponBehavior.Requirements.Count > 0)
					{
						foreach (MyUserControllableGun weapon2 in m_weapons)
						{
							if (weapon2.Enabled && weapon2.IsFunctional && weapon2.IsStationary())
							{
								List<string> requirements = weaponBehavior.Requirements;
								MyObjectBuilderType typeId = weapon2.BlockDefinition.Id.TypeId;
								flag2 = requirements.Contains(typeId.ToString());
								if (!weaponBehavior.RequirementsIsWhitelist)
								{
									flag2 = !flag2;
								}
								if (flag2)
								{
									break;
								}
							}
						}
					}
					else
					{
						flag2 = true;
					}
				}
				if (flag2 && flag && weaponBehavior.WeaponRules.Count > 0)
				{
					int num2 = Math.Max(0, weaponBehavior.Priority);
					list.Add(num2);
					num += num2;
				}
				else
				{
					list.Add(0);
				}
			}
			if (num > 0)
			{
				int num3 = MyUtils.GetRandomInt(0, num) + 1;
				for (int i = 0; i < list.Count; i++)
				{
					if (num3 <= list[i])
					{
						m_currentWeaponBehavior = m_currentPreset.WeaponBehaviors[i];
						break;
					}
					num3 -= list[i];
				}
				if (m_currentWeaponBehavior != null)
				{
					foreach (MyWeaponRule weaponRule in m_currentWeaponBehavior.WeaponRules)
					{
						_ = weaponRule;
						m_weaponBehaviorTimes.Add(-1f);
					}
					foreach (MyUserControllableGun weapon3 in m_weapons)
					{
						m_weaponBehaviorWeaponLock.Add(item: false);
						bool flag3 = false;
						if (m_currentWeaponBehavior.RequirementsIsWhitelist || m_currentWeaponBehavior.Requirements.Count > 0)
						{
							List<string> requirements2 = m_currentWeaponBehavior.Requirements;
							MyObjectBuilderType typeId = weapon3.BlockDefinition.Id.TypeId;
							bool flag2 = requirements2.Contains(typeId.ToString());
							if (!m_currentWeaponBehavior.RequirementsIsWhitelist)
							{
								flag2 = !flag2;
							}
							if (!flag2 || !weapon3.IsStationary())
							{
								m_weaponBehaviorAssignedRules.Add(-1);
								continue;
							}
						}
						int i = 0;
						while (i < m_currentWeaponBehavior.WeaponRules.Count)
						{
							if (!string.IsNullOrEmpty(m_currentWeaponBehavior.WeaponRules[i].Weapon))
							{
								string weapon = m_currentWeaponBehavior.WeaponRules[i].Weapon;
								MyObjectBuilderType typeId = weapon3.BlockDefinition.Id.TypeId;
								if (!weapon.Equals(typeId.ToString()))
								{
									i++;
									continue;
								}
							}
							flag3 = true;
							m_weaponBehaviorAssignedRules.Add(i);
							m_weaponBehaviorTimes[i] = MyUtils.GetRandomFloat(m_currentWeaponBehavior.WeaponRules[i].TimeMin, m_currentWeaponBehavior.WeaponRules[i].TimeMax);
							break;
						}
						if (!flag3)
						{
							m_weaponBehaviorAssignedRules.Add(-1);
						}
					}
					m_weaponBehaviorActive = true;
					return;
				}
			}
			m_weaponBehaviorCooldown = m_currentPreset.WeaponBehaviorNotFoundDelay;
		}

		private void WeaponsUpdate(float distSq)
		{
			m_shooting = false;
			if (m_currentPreset.UsesWeaponBehaviors && m_weaponBehaviorActive)
			{
				bool flag = false;
				for (int i = 0; i < m_weaponBehaviorTimes.Count; i++)
				{
					if (m_weaponBehaviorTimes[i] >= 0f)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					m_weaponBehaviorActive = false;
					m_weaponBehaviorCooldown = MyUtils.GetRandomFloat(m_currentWeaponBehavior.TimeMin, m_currentWeaponBehavior.TimeMax);
					for (int i = 0; i < m_weapons.Count; i++)
					{
						m_weapons[i].SetShooting(shooting: false);
					}
					return;
				}
			}
			bool flag2 = true;
			if (m_currentPreset.UsesWeaponBehaviors && !m_weaponBehaviorActive)
			{
				ChangeWeaponBehavior();
				if (!m_weaponBehaviorActive)
				{
					flag2 = false;
				}
			}
			bool flag3 = m_currentPreset.CanBeDisabled;
			bool flag4 = false;
			bool hitVoxel = false;
			bool hitGrid = false;
			int num = 0;
			if (m_weapons != null && m_weapons.Count > 0)
			{
				for (int i = 0; i < m_weapons.Count; i++)
				{
					if (m_weapons[i].Closed || (m_weapons[i].CubeGrid != m_remoteControl.CubeGrid && !MyCubeGridGroups.Static.Logical.HasSameGroup(m_weapons[i].CubeGrid, m_remoteControl.CubeGrid)))
					{
						m_weapons.RemoveAt(i);
						i--;
					}
					else if (!m_weapons[i].Enabled && m_weapons[i].IsFunctional)
					{
						flag3 = false;
						if (!m_weapons[i].IsStationary())
						{
							flag4 = true;
						}
					}
					else
					{
						if (!m_weapons[i].CanOperate() || !m_weapons[i].CanShoot(out MyGunStatusEnum status) || status != 0)
						{
							continue;
						}
						flag3 = false;
						if (m_currentPreset.UseStaticWeaponry && m_weapons[i].IsStationary())
						{
							if (m_currentPreset.UsesWeaponBehaviors && m_weaponBehaviorActive)
							{
								if (m_weaponBehaviorAssignedRules[i] == -1)
								{
									continue;
								}
								if (m_weaponBehaviorWeaponLock[i])
								{
									m_shooting = flag2;
									continue;
								}
								if (m_weaponBehaviorTimes[m_weaponBehaviorAssignedRules[i]] < 0f)
								{
									m_weapons[i].SetShooting(shooting: false);
									continue;
								}
							}
							if (m_remoteControl.TargettingAimDelta <= 0.05000000074505806 && distSq < m_currentPreset.StaticWeaponryUsageSq && distSq >= 0f && m_canRotateToTarget)
							{
								m_shooting = flag2;
								if (!m_weaponBehaviorActive)
								{
									continue;
								}
								if (m_currentPreset.UsesWeaponBehaviors && (!m_currentWeaponBehavior.WeaponRules[m_weaponBehaviorAssignedRules[i]].CanGoThroughVoxels || !m_currentWeaponBehavior.IgnoresGrids))
								{
									if (num < 10)
									{
										num++;
										RaycastCheck(m_weapons[i].GetWeaponMuzzleWorldPosition(), out hitVoxel, out hitGrid);
									}
									if ((hitVoxel && !m_currentWeaponBehavior.WeaponRules[m_weaponBehaviorAssignedRules[i]].CanGoThroughVoxels) || (hitGrid && !m_currentWeaponBehavior.IgnoresGrids))
									{
										m_weapons[i].SetShooting(shooting: false);
										continue;
									}
								}
								if (m_currentPreset.UsesWeaponBehaviors && m_weaponBehaviorTimes[m_weaponBehaviorAssignedRules[i]] == 0f)
								{
									m_weapons[i].ShootFromTerminal(m_weapons[i].WorldMatrix.Forward);
								}
								else
								{
									m_weapons[i].SetShooting(flag2);
								}
								Ambushing = false;
								if (m_currentPreset.UsesWeaponBehaviors && m_currentWeaponBehavior.WeaponRules[m_weaponBehaviorAssignedRules[i]].FiringAfterLosingSight)
								{
									m_weaponBehaviorWeaponLock[i] = flag2;
								}
							}
							else if (!m_currentPreset.UsesWeaponBehaviors || !m_weaponBehaviorActive || !m_currentWeaponBehavior.WeaponRules[m_weaponBehaviorAssignedRules[i]].FiringAfterLosingSight)
							{
								m_weapons[i].SetShooting(shooting: false);
							}
						}
						if (!m_weapons[i].IsStationary())
						{
							if (Ambushing && m_weapons[i] is MyLargeTurretBase && ((MyLargeTurretBase)m_weapons[i]).IsShooting)
							{
								Ambushing = false;
								m_shooting = true;
							}
							flag4 = true;
						}
					}
				}
			}
			if (m_currentPreset.UsesWeaponBehaviors && m_shooting)
			{
				for (int i = 0; i < m_weaponBehaviorTimes.Count; i++)
				{
					m_weaponBehaviorTimes[i] -= 0.3f;
				}
			}
			if (m_tools != null && m_tools.Count > 0)
			{
				for (int i = 0; i < m_tools.Count; i++)
				{
					if (!m_tools[i].IsFunctional)
					{
						continue;
					}
					flag3 = false;
					if (m_currentPreset.UseTools)
					{
						if (distSq < m_currentPreset.ToolsUsageSq && distSq >= 0f && m_canRotateToTarget)
						{
							m_tools[i].Enabled = true;
							Ambushing = false;
						}
						else
						{
							m_tools[i].Enabled = false;
						}
					}
				}
			}
			m_operational = !flag3;
			if (flag3)
			{
				m_rotateToTarget = true;
				m_weapons.Clear();
				m_tools.Clear();
				if (m_remoteControl.HasWaypoints())
				{
					m_remoteControl.ClearWaypoints();
				}
				NeedUpdate = true;
				m_forcedWaypoints.Clear();
			}
			if (!flag4 && !m_alternativebehaviorSwitched)
			{
				m_rotateToTarget = true;
				if (m_currentPreset.AlternativeBehavior.Length > 0)
				{
					m_currentPreset = MyDroneAIDataStatic.LoadPreset(m_currentPreset.AlternativeBehavior);
					LoadDroneAIData();
				}
				m_alternativebehaviorSwitched = true;
			}
		}

		public static bool SetAIToGrid(MyCubeGrid grid, string behaviour, float activationDistance)
		{
			using (MyFatBlockReader<MyRemoteControl> myFatBlockReader = grid.GetFatBlocks<MyRemoteControl>())
			{
				if (myFatBlockReader.MoveNext())
				{
					MyRemoteControl current = myFatBlockReader.Current;
					MyDroneAI automaticBehaviour = new MyDroneAI(current, behaviour, activate: true, null, null, 1, TargetPrioritization.PriorityRandom, activationDistance, cycleWaypoints: false);
					current.SetAutomaticBehaviour(automaticBehaviour);
					current.SetAutoPilotEnabled(enabled: true);
					return true;
				}
				return false;
			}
		}
	}
}
