using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Blocks;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Profiler;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Replication.History
{
	public class MyAnimatedSnapshotSync : IMySnapshotSync
	{
		private struct MyBlend
		{
			public MySnapshotHistory.MyItem Item1;

			public MySnapshotHistory.MyItem Item2;

			public MyTimeSpan Duration;

			public MyTimeSpan TimeStart;
		}

		private readonly MySnapshotHistory m_history = new MySnapshotHistory();

		private MyTimeSpan m_safeMovementCounter;

		private Vector3 m_lastVelocity;

		private readonly MyEntity m_entity;

		private Vector3D m_lastPos;

		private bool m_deactivated;

		private bool m_wasExtrapolating;

		private MyTimeSpan m_lastTimeDelta;

		private MyTimeSpan m_lastTime;

		public static MyTimeSpan TimeShift = MyTimeSpan.FromMilliseconds(64.0);

		private int m_invalidParentCounter;

		private readonly List<MyBlend> m_blends = new List<MyBlend>();

		private readonly List<MyBlend> m_blendsToRemove = new List<MyBlend>();

		public MyAnimatedSnapshotSync(MyEntity entity)
		{
			m_entity = entity;
		}

		public long Update(MyTimeSpan clientTimestamp, MySnapshotSyncSetup setup)
		{
			if ((m_deactivated && !m_history.IsLastActive()) || MyFakes.MULTIPLAYER_SKIP_ANIMATION)
			{
				return -1L;
			}
			m_deactivated = false;
			MyTimeSpan myTimeSpan = clientTimestamp - TimeShift;
			m_history.Get(myTimeSpan, out MySnapshotHistory.MyItem item);
			m_history.PruneTooOld(myTimeSpan);
			if ((item.Valid && !item.Snapshot.Active) || m_history.Empty())
			{
				m_deactivated = true;
			}
			bool skipped;
			MyEntity parent = MySnapshot.GetParent(m_entity, out skipped);
			bool flag = setup.IgnoreParentId || !item.Valid || (parent == null && item.Snapshot.ParentId == 0L) || (parent != null && item.Snapshot.ParentId == parent.EntityId);
			if (!flag)
			{
				m_invalidParentCounter++;
			}
			else
			{
				m_invalidParentCounter = 0;
			}
			bool flag2 = item.Valid && item.Type != MySnapshotHistory.SnapshotType.TooNew && item.Type != MySnapshotHistory.SnapshotType.TooOld && flag;
			if (flag2)
			{
				if (MyFakes.MULTIPLAYER_EXTRAPOLATION_SMOOTHING && setup.ExtrapolationSmoothing)
				{
					m_wasExtrapolating = (item.Type == MySnapshotHistory.SnapshotType.Extrapolation);
					m_lastTimeDelta = myTimeSpan - m_history.GetLastTimestamp();
					m_lastTime = item.Timestamp;
				}
				if (item.Snapshot.Active && m_blends.Count > 0)
				{
					BlendExtrapolation(ref item);
				}
				if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_NETWORK_SYNC)
				{
					MyRenderProxy.DebugDrawAABB(m_entity.PositionComp.WorldAABB, Color.White);
					if (!(m_entity is MyWheel))
					{
						item.Snapshot.GetMatrix(m_entity, out MatrixD mat);
						double milliseconds = (m_history.GetLastTimestamp() - (clientTimestamp - TimeShift)).Milliseconds;
						MyRenderProxy.DebugDrawSphere(mat.Translation, (float)Math.Abs(milliseconds / 32.0), (milliseconds < 0.0) ? Color.Red : Color.Green, 1f, depthRead: false);
						MyRenderProxy.DebugDrawAxis(mat, 1f, depthRead: false);
						if (parent != null)
						{
							MyRenderProxy.DebugDrawArrow3D(mat.Translation, parent.WorldMatrix.Translation, Color.Blue);
						}
					}
				}
				MySnapshotCache.Add(m_entity, ref item.Snapshot, setup, item.Type == MySnapshotHistory.SnapshotType.Reset);
			}
			if (MySnapshotCache.DEBUG_ENTITY_ID == m_entity.EntityId)
			{
				MyStatsGraph.ProfileAdvanced(begin: true);
				MyStatsGraph.Begin("Animation", int.MaxValue, "Update", 92, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyAnimatedSnapshotSync.cs");
				MyStatsGraph.CustomTime("applySnapshot", flag2 ? 1f : 0.5f, "{0}", 0f, "", "Update", 93, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyAnimatedSnapshotSync.cs");
				MyStatsGraph.CustomTime("extrapolating", (item.Type == MySnapshotHistory.SnapshotType.Extrapolation) ? 1f : 0.5f, "{0}", 0f, "", "Update", 94, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyAnimatedSnapshotSync.cs");
				MyStatsGraph.CustomTime("blendsCount", m_blends.Count, "{0}", 0f, "", "Update", 95, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyAnimatedSnapshotSync.cs");
				MyStatsGraph.End(1f, 1f, "{0}", "{0} B", null, "Update", 96, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\History\\MyAnimatedSnapshotSync.cs");
				MyStatsGraph.ProfileAdvanced(begin: false);
			}
			if (!item.Valid)
			{
				return -1L;
			}
			return item.Snapshot.ParentId;
		}

		private void BlendExtrapolation(ref MySnapshotHistory.MyItem item)
		{
			m_blendsToRemove.Clear();
			MySnapshot ss = default(MySnapshot);
			bool flag = true;
			float factor = 1f;
			foreach (MyBlend blend in m_blends)
			{
				MyTimeSpan a = item.Timestamp - blend.TimeStart;
				if (a >= MyTimeSpan.Zero && a < blend.Duration)
				{
					MySnapshotHistory.MyItem item2 = blend.Item1;
					MySnapshotHistory.MyItem item3 = blend.Item2;
					MySnapshotHistory.Lerp(item.Timestamp, ref item2, ref item3, out MySnapshotHistory.MyItem item4);
					if (flag)
					{
						ss = item4.Snapshot;
					}
					else
					{
						item4.Snapshot.Lerp(ref ss, factor, out ss);
					}
					if (ss.ParentId == -1)
					{
						m_blendsToRemove.Add(blend);
						flag = true;
						break;
					}
					double seconds = a.Seconds;
					MyTimeSpan duration = blend.Duration;
					factor = 1f - (float)(seconds / duration.Seconds);
					flag = false;
				}
				else
				{
					m_blendsToRemove.Add(blend);
				}
			}
			if (!flag)
			{
				item.Snapshot.Lerp(ref ss, factor, out ss);
				if (ss.ParentId != -1)
				{
					item.Snapshot = ss;
				}
			}
			foreach (MyBlend item5 in m_blendsToRemove)
			{
				m_blends.Remove(item5);
			}
		}

		public void Read(ref MySnapshot item, MyTimeSpan timeStamp)
		{
			if (m_wasExtrapolating)
			{
				MyTimeSpan myTimeSpan = m_lastTimeDelta;
				if (m_blends.Count > 0)
				{
					myTimeSpan = m_blends[m_blends.Count - 1].Duration - (m_lastTime - m_blends[m_blends.Count - 1].TimeStart);
					if (myTimeSpan < m_lastTimeDelta)
					{
						myTimeSpan = m_lastTimeDelta;
					}
				}
				MyBlend myBlend = default(MyBlend);
				myBlend.TimeStart = m_lastTime;
				myBlend.Duration = myTimeSpan;
				MyBlend item2 = myBlend;
				if (m_history.GetItems(m_history.Count - 2, out item2.Item1, out item2.Item2))
				{
					m_blends.Add(item2);
				}
				m_wasExtrapolating = false;
			}
			m_history.Add(ref item, timeStamp);
			m_history.PruneTooOld(timeStamp - TimeShift);
		}

		public void Reset(bool reinit = false)
		{
			if (reinit)
			{
				m_history.Reset();
				m_blends.Clear();
			}
			m_deactivated = false;
		}

		public void Destroy()
		{
			Reset();
		}
	}
}
