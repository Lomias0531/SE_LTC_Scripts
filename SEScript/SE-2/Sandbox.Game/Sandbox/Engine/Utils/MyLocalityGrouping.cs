using System;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	public class MyLocalityGrouping
	{
		public enum GroupingMode
		{
			ContainsCenter,
			Overlaps
		}

		private struct InstanceInfo
		{
			public Vector3 Position;

			public float Radius;

			public int EndTimeMs;
		}

		private class InstanceInfoComparer : IComparer<InstanceInfo>
		{
			public int Compare(InstanceInfo x, InstanceInfo y)
			{
				return x.EndTimeMs - y.EndTimeMs;
			}
		}

		public GroupingMode Mode;

		private SortedSet<InstanceInfo> m_instances = new SortedSet<InstanceInfo>(new InstanceInfoComparer());

		private int TimeMs => MySandboxGame.TotalGamePlayTimeInMilliseconds;

		public MyLocalityGrouping(GroupingMode mode)
		{
			Mode = mode;
		}

		public bool AddInstance(TimeSpan lifeTime, Vector3 position, float radius, bool removeOld = true)
		{
			if (removeOld)
			{
				RemoveOld();
			}
			foreach (InstanceInfo instance in m_instances)
			{
				float num = (Mode == GroupingMode.ContainsCenter) ? Math.Max(radius, instance.Radius) : (radius + instance.Radius);
				if (Vector3.DistanceSquared(position, instance.Position) < num * num)
				{
					return false;
				}
			}
			m_instances.Add(new InstanceInfo
			{
				EndTimeMs = TimeMs + (int)lifeTime.TotalMilliseconds,
				Position = position,
				Radius = radius
			});
			return true;
		}

		public void RemoveOld()
		{
			int timeMs = TimeMs;
			while (m_instances.Count > 0 && m_instances.Min.EndTimeMs < timeMs)
			{
				m_instances.Remove(m_instances.Min);
			}
		}

		public void Clear()
		{
			m_instances.Clear();
		}
	}
}
