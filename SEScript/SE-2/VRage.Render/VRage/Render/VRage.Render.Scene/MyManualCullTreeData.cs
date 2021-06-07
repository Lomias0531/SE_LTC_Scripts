using System.Collections.Generic;
using VRage.Network;
using VRageMath;

namespace VRage.Render.Scene
{
	[GenerateActivator]
	public class MyManualCullTreeData
	{
		public MyActor Actor;

		public MyCullResultsBase All;

		public readonly MyDynamicAABBTree Children = new MyDynamicAABBTree();

		public readonly Dictionary<int, MyBruteCullData> BruteCull = new Dictionary<int, MyBruteCullData>();

		public CullData[] RenderCullData;
	}
}
