using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Library.Collections;
using VRage.Network;
using VRageMath;

namespace VRage.Replication
{
	public class MyReplicablesAABB : MyReplicablesBase
	{
		private readonly MyDynamicAABBTreeD m_rootsAABB = new MyDynamicAABBTreeD(Vector3D.One);

		private readonly HashSet<IMyReplicable> m_roots = new HashSet<IMyReplicable>();

		private readonly CacheList<IMyReplicable> m_tmp = new CacheList<IMyReplicable>();

		private readonly Dictionary<IMyReplicable, int> m_proxies = new Dictionary<IMyReplicable, int>();

		public MyReplicablesAABB(Thread mainThread)
			: base(mainThread)
		{
		}

		public override void IterateRoots(Action<IMyReplicable> p)
		{
			using (m_tmp)
			{
				m_rootsAABB.GetAll(m_tmp, clear: false);
				foreach (IMyReplicable item in m_tmp)
				{
					p(item);
				}
			}
		}

		public override void GetReplicablesInBox(BoundingBoxD aabb, List<IMyReplicable> list)
		{
			m_rootsAABB.OverlapAllBoundingBox(ref aabb, list);
		}

		protected override void AddRoot(IMyReplicable replicable)
		{
			m_roots.Add(replicable);
			if (replicable.IsSpatial)
			{
				BoundingBoxD aabb = replicable.GetAABB();
				m_proxies.Add(replicable, m_rootsAABB.AddProxy(ref aabb, replicable, 0u));
				replicable.OnAABBChanged = (Action<IMyReplicable>)Delegate.Combine(replicable.OnAABBChanged, new Action<IMyReplicable>(OnRootMoved));
			}
		}

		private void OnRootMoved(IMyReplicable replicable)
		{
			BoundingBoxD aabb = replicable.GetAABB();
			m_rootsAABB.MoveProxy(m_proxies[replicable], ref aabb, Vector3D.One);
		}

		protected override void RemoveRoot(IMyReplicable replicable)
		{
			if (m_roots.Contains(replicable))
			{
				m_roots.Remove(replicable);
				if (m_proxies.ContainsKey(replicable))
				{
					replicable.OnAABBChanged = (Action<IMyReplicable>)Delegate.Remove(replicable.OnAABBChanged, new Action<IMyReplicable>(OnRootMoved));
					m_rootsAABB.RemoveProxy(m_proxies[replicable]);
					m_proxies.Remove(replicable);
				}
			}
		}

		protected override void AddChild(IMyReplicable replicable, IMyReplicable parent)
		{
			base.AddChild(replicable, parent);
			if (replicable.IsSpatial)
			{
				BoundingBoxD aabb = replicable.GetAABB();
				m_proxies.Add(replicable, m_rootsAABB.AddProxy(ref aabb, replicable, 0u));
				replicable.OnAABBChanged = (Action<IMyReplicable>)Delegate.Combine(replicable.OnAABBChanged, new Action<IMyReplicable>(OnRootMoved));
			}
		}

		protected override void RemoveChild(IMyReplicable replicable, IMyReplicable parent)
		{
			base.RemoveChild(replicable, parent);
			if (m_proxies.ContainsKey(replicable))
			{
				replicable.OnAABBChanged = (Action<IMyReplicable>)Delegate.Remove(replicable.OnAABBChanged, new Action<IMyReplicable>(OnRootMoved));
				m_rootsAABB.RemoveProxy(m_proxies[replicable]);
				m_proxies.Remove(replicable);
			}
		}
	}
}
