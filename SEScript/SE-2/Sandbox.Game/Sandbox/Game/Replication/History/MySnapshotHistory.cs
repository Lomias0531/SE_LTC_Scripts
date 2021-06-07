using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Replication.History
{
	public class MySnapshotHistory
	{
		public enum SnapshotType
		{
			Exact,
			TooNew,
			Interpolation,
			Extrapolation,
			TooOld,
			Reset
		}

		public struct MyItem
		{
			public bool Valid;

			public SnapshotType Type;

			public MyTimeSpan Timestamp;

			public MySnapshot Snapshot;

			public override string ToString()
			{
				return "Item timestamp: " + Timestamp;
			}
		}

		public static readonly MyTimeSpan DELAY = MyTimeSpan.FromMilliseconds(100.0);

		private static readonly MyTimeSpan MAX_EXTRAPOLATION_DELAY = MyTimeSpan.FromMilliseconds(5000.0);

		private readonly List<MyItem> m_history = new List<MyItem>();

		public int Count => m_history.Count;

		public bool Empty()
		{
			return m_history.Count == 0;
		}

		public void GetItem(MyTimeSpan clientTimestamp, out MyItem item)
		{
			if (m_history.Count > 0)
			{
				int num = FindIndex(clientTimestamp);
				num--;
				if (num >= 0 && num < m_history.Count)
				{
					item = m_history[num];
					return;
				}
			}
			item = default(MyItem);
		}

		public void Add(ref MySnapshot snapshot, MyTimeSpan timestamp)
		{
			if (FindExact(timestamp) == -1)
			{
				MyItem myItem = default(MyItem);
				myItem.Valid = true;
				myItem.Type = SnapshotType.Exact;
				myItem.Timestamp = timestamp;
				myItem.Snapshot = snapshot;
				MyItem item = myItem;
				int index = FindIndex(timestamp);
				m_history.Insert(index, item);
			}
		}

		public void Get(MyTimeSpan clientTimestamp, out MyItem item)
		{
			if (m_history.Count == 0)
			{
				item = default(MyItem);
				return;
			}
			int num = FindIndex(clientTimestamp);
			if (num < m_history.Count && clientTimestamp == m_history[num].Timestamp)
			{
				item = m_history[num];
				item.Type = SnapshotType.Exact;
			}
			else if (num == 0)
			{
				item = m_history[0];
				if (clientTimestamp == m_history[0].Timestamp)
				{
					item.Type = SnapshotType.Exact;
				}
				else if (clientTimestamp < m_history[0].Timestamp)
				{
					item.Type = SnapshotType.TooNew;
				}
				else
				{
					item.Type = SnapshotType.TooOld;
				}
			}
			else if (num < m_history.Count && m_history.Count > 1)
			{
				Lerp(clientTimestamp, num - 1, out item);
				item.Type = SnapshotType.Interpolation;
			}
			else if (m_history.Count > 1 && clientTimestamp - m_history[m_history.Count - 1].Timestamp < MAX_EXTRAPOLATION_DELAY)
			{
				if (!m_history[m_history.Count - 1].Snapshot.Active)
				{
					item = m_history[m_history.Count - 1];
					item.Timestamp = clientTimestamp;
				}
				else
				{
					int index = m_history.Count - 2;
					Lerp(clientTimestamp, index, out item);
					item.Type = SnapshotType.Extrapolation;
				}
			}
			else
			{
				item = m_history[m_history.Count - 1];
				item.Type = SnapshotType.TooOld;
			}
		}

		public void Prune(MyTimeSpan clientTimestamp, MyTimeSpan delay, int leaveCount = 2)
		{
			MyTimeSpan timestamp = clientTimestamp - delay;
			int num = FindIndex(timestamp);
			m_history.RemoveRange(0, Math.Max(0, num - leaveCount));
		}

		public void PruneTooOld(MyTimeSpan clientTimestamp)
		{
			Prune(clientTimestamp, MAX_EXTRAPOLATION_DELAY);
		}

		private int FindIndex(MyTimeSpan timestamp)
		{
			int i;
			for (i = 0; i < m_history.Count && timestamp > m_history[i].Timestamp; i++)
			{
			}
			return i;
		}

		private int FindExact(MyTimeSpan timestamp)
		{
			int i;
			for (i = 0; i < m_history.Count && timestamp != m_history[i].Timestamp; i++)
			{
			}
			if (i < m_history.Count)
			{
				return i;
			}
			return -1;
		}

		private static float Factor(MyTimeSpan timestamp, ref MyItem item1, ref MyItem item2)
		{
			return (float)(timestamp - item1.Timestamp).Ticks / (float)(item2.Timestamp - item1.Timestamp).Ticks;
		}

		public static void Lerp(MyTimeSpan timestamp, ref MyItem item1, ref MyItem item2, out MyItem item)
		{
			float factor = Factor(timestamp, ref item1, ref item2);
			item = new MyItem
			{
				Valid = true,
				Timestamp = timestamp
			};
			item1.Snapshot.Lerp(ref item2.Snapshot, factor, out item.Snapshot);
		}

		private void Lerp(MyTimeSpan timestamp, int index, out MyItem item)
		{
			if (GetItems(index, out MyItem item2, out MyItem item3))
			{
				Lerp(timestamp, ref item2, ref item3, out item);
			}
			else
			{
				item = new MyItem
				{
					Valid = false
				};
			}
		}

		public bool GetItems(int index, out MyItem item1, out MyItem item2)
		{
			item1 = m_history[index];
			item2 = m_history[index + 1];
			if (item1.Snapshot.ParentId != item2.Snapshot.ParentId || item1.Snapshot.InheritRotation != item2.Snapshot.InheritRotation)
			{
				if (m_history.Count < index + 2)
				{
					index++;
					item1 = item2;
					item2 = m_history[index + 1];
				}
				else if (index > 0)
				{
					index--;
					item2 = item1;
					item1 = m_history[index];
				}
			}
			if (item1.Snapshot.ParentId == item2.Snapshot.ParentId)
			{
				return item1.Snapshot.InheritRotation == item2.Snapshot.InheritRotation;
			}
			return false;
		}

		public void ApplyDeltaPosition(MyTimeSpan timestamp, Vector3D positionDelta)
		{
			for (int i = 0; i < m_history.Count; i++)
			{
				if (timestamp <= m_history[i].Timestamp)
				{
					MyItem value = m_history[i];
					value.Snapshot.Position += positionDelta;
					m_history[i] = value;
				}
			}
		}

		public void ApplyDeltaLinearVelocity(MyTimeSpan timestamp, Vector3 linearVelocityDelta)
		{
			for (int i = 0; i < m_history.Count; i++)
			{
				if (timestamp <= m_history[i].Timestamp)
				{
					MyItem value = m_history[i];
					value.Snapshot.LinearVelocity += linearVelocityDelta;
					m_history[i] = value;
				}
			}
		}

		public void ApplyDeltaAngularVelocity(MyTimeSpan timestamp, Vector3 angularVelocityDelta)
		{
			for (int i = 0; i < m_history.Count; i++)
			{
				if (timestamp <= m_history[i].Timestamp)
				{
					MyItem value = m_history[i];
					value.Snapshot.AngularVelocity += angularVelocityDelta;
					m_history[i] = value;
				}
			}
		}

		public void ApplyDeltaRotation(MyTimeSpan timestamp, Quaternion rotationDelta)
		{
			for (int i = 0; i < m_history.Count; i++)
			{
				if (timestamp <= m_history[i].Timestamp)
				{
					MyItem value = m_history[i];
					value.Snapshot.Rotation = value.Snapshot.Rotation * Quaternion.Inverse(rotationDelta);
					value.Snapshot.Rotation.Normalize();
					m_history[i] = value;
				}
			}
		}

		public void ApplyDelta(MyTimeSpan timestamp, ref MySnapshot delta)
		{
			for (int i = 0; i < m_history.Count; i++)
			{
				if (timestamp <= m_history[i].Timestamp)
				{
					MyItem value = m_history[i];
					value.Snapshot.Add(ref delta);
					m_history[i] = value;
				}
			}
		}

		public void Reset()
		{
			m_history.Clear();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < m_history.Count; i++)
			{
				stringBuilder.Append(string.Concat(m_history[i].Timestamp, " (", m_history[i].Snapshot.Position.ToString("N3"), ") "));
			}
			return stringBuilder.ToString();
		}

		public string ToStringRotation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < m_history.Count; i++)
			{
				stringBuilder.Append(string.Concat(m_history[i].Timestamp, " (", m_history[i].Snapshot.Rotation.ToStringAxisAngle("N3"), ") "));
			}
			return stringBuilder.ToString();
		}

		public string ToStringTimestamps()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < m_history.Count; i++)
			{
				stringBuilder.Append(string.Concat(m_history[i].Timestamp, " "));
			}
			return stringBuilder.ToString();
		}

		public void GetLast(out MyItem item, int index = 0)
		{
			item = ((m_history.Count < index + 1) ? default(MyItem) : m_history[m_history.Count - index - 1]);
		}

		public void GetFirst(out MyItem item)
		{
			item = ((m_history.Count > 0) ? m_history[0] : default(MyItem));
		}

		public bool IsLastActive()
		{
			if (m_history.Count >= 1)
			{
				return m_history[m_history.Count - 1].Snapshot.Active;
			}
			return false;
		}

		public MyTimeSpan GetLastTimestamp()
		{
			return m_history[m_history.Count - 1].Timestamp;
		}

		public long GetLastParentId()
		{
			return m_history[m_history.Count - 1].Snapshot.ParentId;
		}

		public void DebugDrawPos(MyEntity entity, MyTimeSpan timestamp, Color color)
		{
			int i = 0;
			MatrixD? matrixD = null;
			for (; i < m_history.Count; i++)
			{
				if (timestamp <= m_history[i].Timestamp)
				{
					m_history[i].Snapshot.GetMatrix(entity, out MatrixD mat);
					MyRenderProxy.DebugDrawAxis(mat, 0.2f, depthRead: false);
					if (matrixD.HasValue)
					{
						MyRenderProxy.DebugDrawArrow3D(matrixD.Value.Translation, mat.Translation, color);
					}
					matrixD = mat;
				}
			}
		}
	}
}
