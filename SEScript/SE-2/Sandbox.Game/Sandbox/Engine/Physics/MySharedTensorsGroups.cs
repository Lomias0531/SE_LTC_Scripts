using Sandbox.Game.Entities;
using System.Diagnostics;
using System.Threading;
using VRage.Collections;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
	internal class MySharedTensorsGroups : MyGroups<MyCubeGrid, MySharedTensorData>, IMySceneComponent
	{
		private static MySharedTensorsGroups m_static;

		private static MySharedTensorsGroups Static => m_static;

		public void Load()
		{
			m_static = this;
		}

		public void Unload()
		{
			m_static = null;
		}

		[DebuggerStepThrough]
		[Conditional("DEBUG")]
		private static void AssertThread()
		{
			_ = MySandboxGame.Static.UpdateThread;
			_ = Thread.CurrentThread;
		}

		public static void Link(MyCubeGrid parent, MyCubeGrid child, MyCubeBlock linkingBlock)
		{
			Static.CreateLink(linkingBlock.EntityId, parent, child);
		}

		public static bool BreakLinkIfExists(MyCubeGrid parent, MyCubeGrid child, MyCubeBlock linkingBlock)
		{
			return Static.BreakLink(linkingBlock.EntityId, parent, child);
		}

		public static void MarkGroupDirty(MyCubeGrid grid)
		{
			Static.GetGroup(grid)?.GroupData.MarkDirty();
		}

		public static HashSetReader<Node> GetGridsInSameGroup(MyCubeGrid groupRepresentative)
		{
			return Static.GetGroup(groupRepresentative)?.Nodes ?? default(HashSetReader<Node>);
		}

		public MySharedTensorsGroups()
			: base(supportOphrans: false, (MajorGroupComparer)null)
		{
		}
	}
}
