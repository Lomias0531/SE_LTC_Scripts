using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Network;
using VRageMath;

namespace VRage.Replication
{
	public class MyReplicablesHierarchy : MyReplicablesBase
	{
		public MyReplicablesHierarchy(Thread mainThread)
			: base(mainThread)
		{
		}

		public override void IterateRoots(Action<IMyReplicable> p)
		{
		}

		public override void GetReplicablesInBox(BoundingBoxD aabb, List<IMyReplicable> list)
		{
		}

		protected override void AddRoot(IMyReplicable replicable)
		{
		}

		protected override void RemoveRoot(IMyReplicable replicable)
		{
		}
	}
}
