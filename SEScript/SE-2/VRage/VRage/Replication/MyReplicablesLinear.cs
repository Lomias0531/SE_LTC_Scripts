using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Collections;
using VRage.Network;
using VRageMath;

namespace VRage.Replication
{
	public class MyReplicablesLinear : MyReplicablesBase
	{
		private const int UPDATE_INTERVAL = 60;

		private readonly HashSet<IMyReplicable> m_roots = new HashSet<IMyReplicable>();

		private readonly MyDistributedUpdater<List<IMyReplicable>, IMyReplicable> m_updateList = new MyDistributedUpdater<List<IMyReplicable>, IMyReplicable>(60);

		public MyReplicablesLinear(Thread mainThread)
			: base(mainThread)
		{
		}

		public override void IterateRoots(Action<IMyReplicable> p)
		{
			foreach (IMyReplicable root in m_roots)
			{
				p(root);
			}
		}

		public override void GetReplicablesInBox(BoundingBoxD aabb, List<IMyReplicable> list)
		{
			throw new NotImplementedException();
		}

		protected override void AddRoot(IMyReplicable replicable)
		{
			m_roots.Add(replicable);
			m_updateList.List.Add(replicable);
		}

		protected override void RemoveRoot(IMyReplicable replicable)
		{
			if (m_roots.Contains(replicable))
			{
				m_roots.Remove(replicable);
				m_updateList.List.Remove(replicable);
			}
		}
	}
}
