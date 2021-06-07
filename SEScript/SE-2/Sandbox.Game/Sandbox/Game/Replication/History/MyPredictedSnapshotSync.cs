using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.World;
using System;
using VRage;
using VRage.Game.Entity;
using VRage.Game.Networking;
using VRage.Library.Utils;
using VRage.Profiler;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Replication.History
{
	public class MyPredictedSnapshotSync : IMySnapshotSync
	{
		private enum ResetType
		{
			NoReset,
			Initial,
			Reset,
			ForceStop
		}

		public static float DELTA_FACTOR = 0.8f;

		public static int SMOOTH_ITERATIONS = 30;

		public static bool POSITION_CORRECTION = true;

		public static bool SMOOTH_POSITION_CORRECTION = true;

		public static float MIN_POSITION_DELTA = 0.005f;

		public static float MAX_POSITION_DELTA = 0.5f;

		public static bool ROTATION_CORRECTION = true;

		public static bool SMOOTH_ROTATION_CORRECTION = true;

		public static float MIN_ROTATION_ANGLE = 0.03490659f;

		public static float MAX_ROTATION_ANGLE = 0.17453295f;

		public static bool LINEAR_VELOCITY_CORRECTION = true;

		public static bool SMOOTH_LINEAR_VELOCITY_CORRECTION = true;

		public static float MIN_LINEAR_VELOCITY_DELTA = 0.01f;

		public static float MAX_LINEAR_VELOCITY_DELTA = 4f;

		public static bool ANGULAR_VELOCITY_CORRECTION = true;

		public static bool SMOOTH_ANGULAR_VELOCITY_CORRECTION = true;

		public static float MIN_ANGULAR_VELOCITY_DELTA = 0.01f;

		public static float MAX_ANGULAR_VELOCITY_DELTA = 0.5f;

		public static float MIN_VELOCITY_CHANGE_TO_RESET = 10f;

		public static bool SKIP_CORRECTIONS_FOR_DEBUG_ENTITY;

		public static float SMOOTH_DISTANCE = 150f;

		public static bool ApplyTrend = true;

		public static bool ForceAnimated = false;

		public readonly MyMovingAverage AverageCorrection = new MyMovingAverage(60);

		private readonly MyEntity m_entity;

		private readonly MySnapshotHistory m_clientHistory = new MySnapshotHistory();

		private readonly MySnapshotHistory m_receivedQueue = new MySnapshotHistory();

		private readonly MySnapshotFlags m_currentFlags = new MySnapshotFlags();

		private bool m_inited;

		private ResetType m_wasReset = ResetType.Initial;

		private int m_animDeltaLinearVelocityIterations;

		private MyTimeSpan m_animDeltaLinearVelocityTimestamp;

		private Vector3 m_animDeltaLinearVelocity;

		private int m_animDeltaPositionIterations;

		private MyTimeSpan m_animDeltaPositionTimestamp;

		private Vector3D m_animDeltaPosition;

		private int m_animDeltaRotationIterations;

		private MyTimeSpan m_animDeltaRotationTimestamp;

		private Quaternion m_animDeltaRotation;

		private int m_animDeltaAngularVelocityIterations;

		private MyTimeSpan m_animDeltaAngularVelocityTimestamp;

		private Vector3 m_animDeltaAngularVelocity;

		private MyTimeSpan m_lastServerTimestamp;

		private MyTimeSpan m_trendStart;

		private Vector3 m_lastServerVelocity;

		private int m_stopSuspected;

		private MyTimeSpan m_debugLastClientTimestamp;

		private MySnapshot m_debugLastClientSnapshot;

		private MySnapshot m_debugLastServerSnapshot;

		private MySnapshot m_debugLastDelta;

		private static float TREND_TIMEOUT = 0.2f;

		public bool Inited => m_inited;

		public MyPredictedSnapshotSync(MyEntity entity)
		{
			m_entity = entity;
		}

		public void Destroy()
		{
			Reset();
		}

		public long Update(MyTimeSpan clientTimestamp, MySnapshotSyncSetup setup)
		{
			if ((MyFakes.MULTIPLAYER_SKIP_PREDICTION_SUBGRIDS && MySnapshot.GetParent(m_entity, out bool _) != null) || MyFakes.MULTIPLAYER_SKIP_PREDICTION)
			{
				Reset();
				m_receivedQueue.Reset();
				return -1L;
			}
			if (!m_entity.InScene)
			{
				return -1L;
			}
			if (m_entity.Parent != null)
			{
				return -1L;
			}
			if (m_entity.Physics == null)
			{
				return -1L;
			}
			if (m_entity.Physics.RigidBody != null && !m_entity.Physics.RigidBody.IsActive && !(setup as MyPredictedSnapshotSyncSetup).UpdateAlways)
			{
				return -1L;
			}
			if (MySnapshotCache.DEBUG_ENTITY_ID == m_entity.EntityId && SKIP_CORRECTIONS_FOR_DEBUG_ENTITY)
			{
				return -1L;
			}
			if (m_inited)
			{
				UpdatePrediction(clientTimestamp, setup);
			}
			else
			{
				InitPrediction(clientTimestamp, setup);
			}
			return -1L;
		}

		private bool InitPrediction(MyTimeSpan clientTimestamp, MySnapshotSyncSetup setup)
		{
			m_receivedQueue.GetItem(clientTimestamp, out MySnapshotHistory.MyItem item);
			if (item.Valid)
			{
				if ((MySnapshot.GetParent(m_entity, out bool _)?.EntityId ?? 0) == item.Snapshot.ParentId)
				{
					MySnapshotCache.Add(m_entity, ref item.Snapshot, setup, reset: true);
					m_inited = true;
					return true;
				}
				m_inited = false;
			}
			return false;
		}

		private bool UpdatePrediction(MyTimeSpan clientTimestamp, MySnapshotSyncSetup setup)
		{
			MyPredictedSnapshotSyncSetup myPredictedSnapshotSyncSetup = setup as MyPredictedSnapshotSyncSetup;
			bool flag = (m_entity.WorldMatrix.Translation - MySector.MainCamera.Position).LengthSquared() < (double)(SMOOTH_DISTANCE * SMOOTH_DISTANCE);
			if (!flag)
			{
				myPredictedSnapshotSyncSetup = myPredictedSnapshotSyncSetup.NotSmoothed;
			}
			if (!myPredictedSnapshotSyncSetup.Smoothing)
			{
				m_animDeltaPositionIterations = (m_animDeltaLinearVelocityIterations = (m_animDeltaRotationIterations = (m_animDeltaAngularVelocityIterations = 0)));
			}
			MySnapshot currentSnapshot = new MySnapshot(m_entity, setup.ApplyPhysicsLocal, setup.InheritRotation);
			MySnapshot mySnapshot = currentSnapshot;
			if (!m_clientHistory.Empty())
			{
				m_clientHistory.GetLast(out MySnapshotHistory.MyItem item);
				if (currentSnapshot.ParentId != item.Snapshot.ParentId || currentSnapshot.InheritRotation != item.Snapshot.InheritRotation)
				{
					Reset();
					m_receivedQueue.GetLast(out MySnapshotHistory.MyItem item2);
					if (item2.Snapshot.ParentId == currentSnapshot.ParentId && item2.Snapshot.InheritRotation == currentSnapshot.InheritRotation)
					{
						currentSnapshot.LinearVelocity = item2.Snapshot.LinearVelocity;
						MySnapshotCache.Add(m_entity, ref currentSnapshot, myPredictedSnapshotSyncSetup, reset: true);
						m_wasReset = ResetType.NoReset;
					}
				}
			}
			m_clientHistory.Add(ref currentSnapshot, clientTimestamp);
			MySnapshotHistory.MyItem serverItem;
			MySnapshotHistory.MyItem myItem = UpdateFromServerQueue(clientTimestamp, myPredictedSnapshotSyncSetup, ref currentSnapshot, out serverItem);
			float num = (float)(m_lastServerTimestamp - serverItem.Timestamp).Seconds;
			bool flag2 = false;
			Vector3 value = Vector3.Zero;
			bool flag3 = false;
			if (myItem.Valid)
			{
				if (myItem.Snapshot.Position != Vector3D.Zero || myItem.Snapshot.Rotation.W != 1f)
				{
					flag2 = true;
				}
				currentSnapshot.Add(ref myItem.Snapshot);
				m_clientHistory.ApplyDelta(myItem.Timestamp, ref myItem.Snapshot);
				value = myItem.Snapshot.Position;
				flag3 = true;
			}
			if (m_animDeltaPositionIterations > 0 || m_animDeltaLinearVelocityIterations > 0 || m_animDeltaRotationIterations > 0 || m_animDeltaAngularVelocityIterations > 0)
			{
				if (m_animDeltaPositionIterations > 0)
				{
					m_clientHistory.ApplyDeltaPosition(m_animDeltaPositionTimestamp, m_animDeltaPosition);
					currentSnapshot.Position += m_animDeltaPosition;
					m_animDeltaPositionIterations--;
					m_currentFlags.ApplyPosition = true;
					value += m_animDeltaPosition;
				}
				if (m_animDeltaLinearVelocityIterations > 0)
				{
					m_clientHistory.ApplyDeltaLinearVelocity(m_animDeltaLinearVelocityTimestamp, m_animDeltaLinearVelocity);
					currentSnapshot.LinearVelocity += m_animDeltaLinearVelocity;
					m_animDeltaLinearVelocityIterations--;
					m_currentFlags.ApplyPhysicsLinear = true;
				}
				if (m_animDeltaAngularVelocityIterations > 0)
				{
					m_clientHistory.ApplyDeltaAngularVelocity(m_animDeltaAngularVelocityTimestamp, m_animDeltaAngularVelocity);
					currentSnapshot.AngularVelocity += m_animDeltaAngularVelocity;
					m_animDeltaAngularVelocityIterations--;
					m_currentFlags.ApplyPhysicsAngular = true;
				}
				if (m_animDeltaRotationIterations > 0)
				{
					m_clientHistory.ApplyDeltaRotation(m_animDeltaRotationTimestamp, m_animDeltaRotation);
					currentSnapshot.Rotation *= Quaternion.Inverse(m_animDeltaRotation);
					currentSnapshot.Rotation.Normalize();
					m_animDeltaRotationIterations--;
					m_currentFlags.ApplyRotation = true;
				}
				flag3 = true;
			}
			if (flag3)
			{
				DebugDraw(ref serverItem, ref currentSnapshot, clientTimestamp, myPredictedSnapshotSyncSetup);
				m_currentFlags.ApplyPhysicsLocal = setup.ApplyPhysicsLocal;
				m_currentFlags.InheritRotation = setup.InheritRotation;
				bool reset = myItem.Type == MySnapshotHistory.SnapshotType.Reset || flag2;
				MySnapshotCache.Add(m_entity, ref currentSnapshot, m_currentFlags, reset);
			}
			AverageCorrection.Enqueue(value.Length());
			if (MySnapshotCache.DEBUG_ENTITY_ID == m_entity.EntityId)
			{
				MyStatsGraph.ProfileAdvanced(begin: true);
				MyStatsGraph.Begin("Prediction", int.MaxValue, "UpdatePrediction", 333, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
				MyStatsGraph.CustomTime("applySnapshot", flag3 ? 1f : 0.5f, "{0}", 0f, "", "UpdatePrediction", 334, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
				MyStatsGraph.CustomTime("smoothing", flag ? 1f : 0.5f, "{0}", 0f, "", "UpdatePrediction", 335, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
				if (myPredictedSnapshotSyncSetup.ApplyPosition)
				{
					MyStatsGraph.CustomTime("Pos", (float)m_debugLastDelta.Position.Length(), "{0}", 0f, "", "UpdatePrediction", 339, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
				}
				if (myPredictedSnapshotSyncSetup.ApplyRotation)
				{
					m_debugLastDelta.Rotation.GetAxisAngle(out Vector3 _, out float angle);
					MyStatsGraph.CustomTime("Rot", Math.Abs(angle), "{0}", 0f, "", "UpdatePrediction", 347, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
				}
				if (myPredictedSnapshotSyncSetup.ApplyPhysicsLinear)
				{
					MyStatsGraph.CustomTime("linVel", m_debugLastDelta.LinearVelocity.Length(), "{0}", 0f, "", "UpdatePrediction", 351, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
				}
				if (myPredictedSnapshotSyncSetup.ApplyPhysicsAngular)
				{
					MyStatsGraph.CustomTime("angVel", Math.Abs(m_debugLastDelta.AngularVelocity.Length()), "{0}", 0f, "", "UpdatePrediction", 353, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
				}
				MyStatsGraph.End(null, 0f, "", "{0} B", null, "UpdatePrediction", 355, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyPredictedSnapshotSync.cs");
				MyStatsGraph.ProfileAdvanced(begin: false);
				if (flag3)
				{
					if (myPredictedSnapshotSyncSetup.ApplyPosition)
					{
						_ = (serverItem.Snapshot.Position - m_debugLastServerSnapshot.Position) / num;
						m_debugLastServerSnapshot = serverItem.Snapshot;
						float num2 = (float)(m_debugLastClientTimestamp - clientTimestamp).Seconds;
						_ = (currentSnapshot.Position - m_debugLastClientSnapshot.Position) / num2;
						m_debugLastClientSnapshot = currentSnapshot;
						m_debugLastClientTimestamp = clientTimestamp;
						mySnapshot.Diff(ref currentSnapshot, out MySnapshot ss);
						ss.Position.Length();
						ss.LinearVelocity.Length();
						myItem.Snapshot.Position.Length();
						m_animDeltaPosition.Length();
						MyCubeGrid myCubeGrid = m_entity as MyCubeGrid;
						if (myCubeGrid != null)
						{
							_ = (MyGridPhysicalHierarchy.Static.GetEntityConnectingToParent(myCubeGrid) as MyPistonBase)?.CurrentPosition;
						}
					}
					else if (!myPredictedSnapshotSyncSetup.ApplyRotation)
					{
						_ = myPredictedSnapshotSyncSetup.ApplyPhysicsAngular;
					}
				}
			}
			m_clientHistory.Prune(m_lastServerTimestamp, MyTimeSpan.Zero, 3);
			return flag3;
		}

		public void Read(ref MySnapshot snapshot, MyTimeSpan timeStamp)
		{
			if (m_entity.Parent != null || m_entity.Physics == null)
			{
				return;
			}
			if (m_entity.Physics.IsInWorld && m_entity.Physics.RigidBody != null && !m_entity.Physics.RigidBody.IsActive && snapshot.Active)
			{
				m_entity.Physics.RigidBody.Activate();
			}
			if (!m_receivedQueue.Empty())
			{
				m_receivedQueue.GetLast(out MySnapshotHistory.MyItem item);
				if (snapshot.ParentId != item.Snapshot.ParentId || snapshot.InheritRotation != item.Snapshot.InheritRotation)
				{
					m_receivedQueue.Reset();
				}
			}
			m_receivedQueue.Add(ref snapshot, timeStamp);
			m_receivedQueue.Prune(timeStamp, MyTimeSpan.Zero, 10);
		}

		public void Reset(bool reinit = false)
		{
			m_clientHistory.Reset();
			m_animDeltaRotationIterations = (m_animDeltaLinearVelocityIterations = (m_animDeltaPositionIterations = (m_animDeltaAngularVelocityIterations = 0)));
			m_lastServerVelocity = Vector3D.PositiveInfinity;
			m_wasReset = ResetType.Reset;
			m_trendStart = MyTimeSpan.FromSeconds(-1.0);
			if (reinit)
			{
				m_inited = false;
			}
		}

		private MySnapshotHistory.MyItem UpdateFromServerQueue(MyTimeSpan clientTimestamp, MyPredictedSnapshotSyncSetup setup, ref MySnapshot currentSnapshot, out MySnapshotHistory.MyItem serverItem)
		{
			m_currentFlags.Init(state: false);
			bool flag = false;
			m_receivedQueue.GetItem(clientTimestamp, out serverItem);
			if (serverItem.Valid)
			{
				if (serverItem.Timestamp != m_lastServerTimestamp)
				{
					m_clientHistory.Get(serverItem.Timestamp, out MySnapshotHistory.MyItem item);
					if (item.Valid && (item.Type == MySnapshotHistory.SnapshotType.Exact || item.Type == MySnapshotHistory.SnapshotType.Interpolation) && serverItem.Snapshot.ParentId == item.Snapshot.ParentId && serverItem.Snapshot.InheritRotation == item.Snapshot.InheritRotation)
					{
						m_lastServerTimestamp = serverItem.Timestamp;
						if (UpdateTrend(setup, ref serverItem, ref item))
						{
							return serverItem;
						}
						MySnapshot ss;
						if (!serverItem.Snapshot.Active && !setup.IsControlled)
						{
							serverItem.Snapshot.Diff(ref currentSnapshot, out ss);
							if (!ss.CheckThresholds(0.0001f, 0.0001f, 0.0001f, 0.0001f))
							{
								serverItem.Valid = false;
								return serverItem;
							}
							Reset(reinit: true);
						}
						else
						{
							serverItem.Snapshot.Diff(ref item.Snapshot, out ss);
						}
						m_debugLastDelta = ss;
						UpdateForceStop(ref ss, ref currentSnapshot, ref serverItem, setup);
						if (m_wasReset != 0)
						{
							m_wasReset = ResetType.NoReset;
							serverItem.Snapshot = ss;
							serverItem.Type = MySnapshotHistory.SnapshotType.Reset;
							return serverItem;
						}
						int num = (int)((float)SMOOTH_ITERATIONS * setup.IterationsFactor);
						bool flag2 = false;
						if (setup.ApplyPosition && POSITION_CORRECTION)
						{
							float num2 = setup.MaxPositionFactor * setup.MaxPositionFactor;
							double num3 = ss.Position.LengthSquared();
							if (num3 > (double)(MAX_POSITION_DELTA * MAX_POSITION_DELTA * num2))
							{
								Vector3D position = ss.Position;
								double scaleFactor = position.Normalize() - (double)(MAX_POSITION_DELTA * (1f - DELTA_FACTOR));
								ss.Position = position * scaleFactor;
								flag2 = true;
								m_animDeltaPositionIterations = 0;
								m_currentFlags.ApplyPosition = true;
							}
							else if (!SMOOTH_POSITION_CORRECTION || !setup.Smoothing)
							{
								m_animDeltaPositionIterations = 0;
							}
							else
							{
								float num4 = MIN_POSITION_DELTA * setup.MinPositionFactor;
								if (num3 > (double)(num4 * num4))
								{
									m_animDeltaPositionIterations = num;
								}
								if (m_animDeltaPositionIterations > 0)
								{
									m_animDeltaPosition = ss.Position / m_animDeltaPositionIterations;
									m_animDeltaPositionTimestamp = serverItem.Timestamp;
								}
								ss.Position = Vector3D.Zero;
							}
						}
						else
						{
							ss.Position = Vector3D.Zero;
							m_animDeltaPositionIterations = 0;
						}
						if (setup.ApplyRotation && ROTATION_CORRECTION)
						{
							ss.Rotation.GetAxisAngle(out Vector3 axis, out float angle);
							if (angle > 3.141593f)
							{
								axis = -axis;
								angle = 6.283186f - angle;
							}
							if (angle > MAX_ROTATION_ANGLE * setup.MaxRotationFactor)
							{
								ss.Rotation = Quaternion.CreateFromAxisAngle(axis, angle - MAX_ROTATION_ANGLE * (1f - DELTA_FACTOR));
								ss.Rotation.Normalize();
								flag2 = true;
								m_animDeltaRotationIterations = 0;
								m_currentFlags.ApplyRotation = true;
							}
							else if (!SMOOTH_ROTATION_CORRECTION || !setup.Smoothing)
							{
								m_animDeltaRotationIterations = 0;
							}
							else
							{
								if (angle > MIN_ROTATION_ANGLE)
								{
									m_animDeltaRotationIterations = num;
								}
								if (m_animDeltaRotationIterations > 0)
								{
									m_animDeltaRotation = Quaternion.CreateFromAxisAngle(axis, angle / (float)m_animDeltaRotationIterations);
									m_animDeltaRotationTimestamp = serverItem.Timestamp;
								}
								ss.Rotation = Quaternion.Identity;
							}
						}
						else
						{
							ss.Rotation = Quaternion.Identity;
							m_animDeltaRotationIterations = 0;
						}
						if (setup.ApplyPhysicsLinear && LINEAR_VELOCITY_CORRECTION)
						{
							float num5 = MIN_LINEAR_VELOCITY_DELTA * MIN_LINEAR_VELOCITY_DELTA;
							float num6 = setup.MinLinearFactor * setup.MinLinearFactor * num5;
							float num7 = ss.LinearVelocity.LengthSquared();
							if (serverItem.Snapshot.LinearVelocity.LengthSquared() == 0f && num7 < num5)
							{
								flag2 = true;
								m_animDeltaLinearVelocityIterations = 0;
								m_currentFlags.ApplyPhysicsLinear = true;
							}
							else if (!SMOOTH_LINEAR_VELOCITY_CORRECTION || !setup.Smoothing)
							{
								if (num7 > MAX_LINEAR_VELOCITY_DELTA * MAX_LINEAR_VELOCITY_DELTA * setup.MaxLinearFactor * setup.MaxLinearFactor)
								{
									ss.LinearVelocity *= DELTA_FACTOR;
									flag2 = true;
									m_animDeltaLinearVelocityIterations = 0;
									m_currentFlags.ApplyPhysicsLinear = true;
								}
								else
								{
									m_animDeltaLinearVelocityIterations = 0;
								}
							}
							else
							{
								if (num7 > num6)
								{
									m_animDeltaLinearVelocityIterations = num;
								}
								if (m_animDeltaLinearVelocityIterations > 0)
								{
									m_animDeltaLinearVelocity = ss.LinearVelocity * DELTA_FACTOR / m_animDeltaLinearVelocityIterations;
									m_animDeltaLinearVelocityTimestamp = serverItem.Timestamp;
								}
								ss.LinearVelocity = Vector3.Zero;
							}
						}
						else
						{
							ss.LinearVelocity = Vector3.Zero;
							m_animDeltaLinearVelocityIterations = 0;
						}
						if (setup.ApplyPhysicsAngular && ANGULAR_VELOCITY_CORRECTION)
						{
							float num8 = ss.AngularVelocity.LengthSquared();
							if (num8 > MAX_ANGULAR_VELOCITY_DELTA * MAX_ANGULAR_VELOCITY_DELTA * setup.MaxAngularFactor * setup.MaxAngularFactor)
							{
								ss.AngularVelocity *= DELTA_FACTOR;
								flag2 = true;
								m_currentFlags.ApplyPhysicsAngular = true;
								m_animDeltaAngularVelocityIterations = 0;
							}
							else if (!SMOOTH_ANGULAR_VELOCITY_CORRECTION || !setup.Smoothing)
							{
								m_animDeltaAngularVelocityIterations = 0;
							}
							else
							{
								if (num8 > MIN_ANGULAR_VELOCITY_DELTA * MIN_ANGULAR_VELOCITY_DELTA * setup.MinAngularFactor * setup.MinAngularFactor)
								{
									m_animDeltaAngularVelocityIterations = num;
								}
								if (m_animDeltaAngularVelocityIterations > 0)
								{
									m_animDeltaAngularVelocity = ss.AngularVelocity * DELTA_FACTOR / m_animDeltaAngularVelocityIterations;
									m_animDeltaAngularVelocityTimestamp = serverItem.Timestamp;
								}
								ss.AngularVelocity = Vector3.Zero;
							}
						}
						else
						{
							ss.AngularVelocity = Vector3.Zero;
							m_animDeltaAngularVelocityIterations = 0;
						}
						if (MyCompilationSymbols.EnableNetworkPositionTracking && flag2)
						{
							_ = m_entity.EntityId;
							_ = MySnapshotCache.DEBUG_ENTITY_ID;
						}
						serverItem.Snapshot = ss;
						serverItem.Valid = flag2;
					}
					else if (item.Type == MySnapshotHistory.SnapshotType.TooNew && !item.Snapshot.Active)
					{
						Reset(reinit: true);
					}
					else
					{
						if (POSITION_CORRECTION && item.Valid && (serverItem.Snapshot.ParentId != item.Snapshot.ParentId || serverItem.Snapshot.InheritRotation != item.Snapshot.InheritRotation))
						{
							if (m_trendStart.Seconds < 0.0)
							{
								m_trendStart = clientTimestamp;
							}
							else if ((clientTimestamp - m_trendStart).Seconds > (double)TREND_TIMEOUT + MySession.Static.MultiplayerPing.Seconds && ApplyTrend)
							{
								Reset(reinit: true);
								serverItem.Valid = false;
								return serverItem;
							}
						}
						else
						{
							m_trendStart = MyTimeSpan.FromSeconds(-1.0);
						}
						serverItem.Valid = false;
						flag = (m_wasReset != ResetType.NoReset);
						if (m_wasReset == ResetType.NoReset && !MyCompilationSymbols.EnableNetworkPositionTracking)
						{
						}
					}
				}
				else
				{
					serverItem.Valid = false;
					flag = (m_wasReset != ResetType.NoReset);
				}
			}
			else if (!m_receivedQueue.Empty())
			{
				flag = true;
			}
			if (flag)
			{
				if (serverItem.Valid && (serverItem.Type == MySnapshotHistory.SnapshotType.Exact || serverItem.Type == MySnapshotHistory.SnapshotType.Interpolation || serverItem.Type == MySnapshotHistory.SnapshotType.Extrapolation))
				{
					_ = MySnapshotCache.DEBUG_ENTITY_ID;
					_ = m_entity.EntityId;
					serverItem.Snapshot.Diff(ref currentSnapshot, out MySnapshot ss2);
					m_currentFlags.Init(setup);
					m_currentFlags.ApplyPhysicsLinear &= LINEAR_VELOCITY_CORRECTION;
					m_currentFlags.ApplyPhysicsAngular &= ANGULAR_VELOCITY_CORRECTION;
					serverItem.Valid = ss2.Active;
					serverItem.Snapshot = ss2;
					serverItem.Type = MySnapshotHistory.SnapshotType.Reset;
					m_debugLastDelta = ss2;
					return serverItem;
				}
				serverItem.Valid = false;
				_ = MySnapshotCache.DEBUG_ENTITY_ID;
				_ = m_entity.EntityId;
			}
			return serverItem;
		}

		private void DebugDraw(ref MySnapshotHistory.MyItem serverItem, ref MySnapshot currentSnapshot, MyTimeSpan clientTimestamp, MyPredictedSnapshotSyncSetup setup)
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_NETWORK_SYNC)
			{
				serverItem.Snapshot.GetMatrix(m_entity, out MatrixD mat);
				MyRenderProxy.DebugDrawAxis(mat, 1f, depthRead: false);
				MatrixD worldMatrix = m_entity.WorldMatrix;
				MyRenderProxy.DebugDrawAxis(worldMatrix, 0.2f, depthRead: false);
				MyRenderProxy.DebugDrawArrow3DDir(worldMatrix.Translation, serverItem.Snapshot.GetLinearVelocity(setup.ApplyPhysicsLocal), Color.White);
				double milliseconds = (serverItem.Timestamp - clientTimestamp).Milliseconds;
				float scale = (float)Math.Abs(milliseconds / 32.0);
				MyRenderProxy.DebugDrawAABB(new BoundingBoxD(mat.Translation - Vector3.One, mat.Translation + Vector3.One), (milliseconds < 0.0) ? Color.Red : Color.Green, 1f, scale, depthRead: false);
				bool skipped;
				MyEntity parent = MySnapshot.GetParent(m_entity, out skipped);
				if (parent != null)
				{
					MyRenderProxy.DebugDrawArrow3D(worldMatrix.Translation, parent.WorldMatrix.Translation, Color.Blue);
				}
				currentSnapshot.GetMatrix(m_entity, out MatrixD mat2, applyPosition: true, applyRotation: false);
				MyRenderProxy.DebugDrawArrow3D(m_entity.WorldMatrix.Translation, mat2.Translation, Color.Goldenrod);
			}
		}

		private void UpdateForceStop(ref MySnapshot delta, ref MySnapshot currentSnapshot, ref MySnapshotHistory.MyItem serverItem, MyPredictedSnapshotSyncSetup setup)
		{
			if (m_lastServerVelocity.IsValid() && setup.ApplyPhysicsLinear && setup.AllowForceStop)
			{
				Vector3 vector = serverItem.Snapshot.LinearVelocity - m_lastServerVelocity;
				m_lastServerVelocity = serverItem.Snapshot.LinearVelocity;
				float num = vector.LengthSquared();
				if (m_stopSuspected > 0)
				{
					float num2 = MIN_VELOCITY_CHANGE_TO_RESET / 2f * (MIN_VELOCITY_CHANGE_TO_RESET / 2f);
					if ((serverItem.Snapshot.LinearVelocity - currentSnapshot.LinearVelocity).LengthSquared() > num2)
					{
						Reset();
						m_wasReset = ResetType.ForceStop;
						serverItem.Snapshot.Diff(ref currentSnapshot, out delta);
						m_stopSuspected = 0;
					}
				}
				if (num > MIN_VELOCITY_CHANGE_TO_RESET * MIN_VELOCITY_CHANGE_TO_RESET)
				{
					m_stopSuspected = 10;
					_ = MyCompilationSymbols.EnableNetworkPositionTracking;
				}
				else if (m_stopSuspected > 0)
				{
					m_stopSuspected--;
				}
			}
			else
			{
				m_lastServerVelocity = serverItem.Snapshot.LinearVelocity;
			}
		}

		private bool UpdateTrend(MyPredictedSnapshotSyncSetup setup, ref MySnapshotHistory.MyItem serverItem, ref MySnapshotHistory.MyItem item)
		{
			if (setup.UserTrend && m_receivedQueue.Count > 1 && POSITION_CORRECTION)
			{
				m_receivedQueue.GetFirst(out MySnapshotHistory.MyItem item2);
				m_receivedQueue.GetLast(out MySnapshotHistory.MyItem item3);
				Vector3 value = Vector3.Sign((item3.Snapshot.Position - item2.Snapshot.Position) / (item3.Timestamp.Seconds - item2.Timestamp.Seconds), 1f);
				m_clientHistory.GetLast(out MySnapshotHistory.MyItem item4);
				Vector3 value2 = Vector3.Sign((item4.Snapshot.Position - item.Snapshot.Position) / (item4.Timestamp.Seconds - item.Timestamp.Seconds), 1f);
				if (value == Vector3.Zero && value != value2)
				{
					if (m_trendStart.Seconds < 0.0)
					{
						m_trendStart = item.Timestamp;
					}
					else if ((item.Timestamp - m_trendStart).Seconds > (item3.Timestamp - m_trendStart).Seconds && ApplyTrend)
					{
						Reset(reinit: true);
						serverItem.Valid = false;
						return true;
					}
				}
				else
				{
					m_trendStart = MyTimeSpan.FromSeconds(-1.0);
				}
			}
			else
			{
				m_trendStart = MyTimeSpan.FromSeconds(-1.0);
			}
			return false;
		}

		public long GetParentId()
		{
			if (m_receivedQueue.Empty())
			{
				return -1L;
			}
			return m_receivedQueue.GetLastParentId();
		}
	}
}
