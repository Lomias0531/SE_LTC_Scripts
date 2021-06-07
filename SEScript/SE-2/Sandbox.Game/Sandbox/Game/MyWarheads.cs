using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	internal class MyWarheads : MySessionComponentBase
	{
		private static readonly HashSet<MyWarhead> m_warheads;

		private static readonly List<MyWarhead> m_warheadsToExplode;

		public static List<BoundingSphere> DebugWarheadShrinks;

		public static List<BoundingSphere> DebugWarheadGroupSpheres;

		static MyWarheads()
		{
			m_warheads = new HashSet<MyWarhead>();
			m_warheadsToExplode = new List<MyWarhead>();
			DebugWarheadShrinks = new List<BoundingSphere>();
			DebugWarheadGroupSpheres = new List<BoundingSphere>();
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
		}

		protected override void UnloadData()
		{
			m_warheads.Clear();
			m_warheadsToExplode.Clear();
			DebugWarheadShrinks.Clear();
			DebugWarheadGroupSpheres.Clear();
		}

		public static void AddWarhead(MyWarhead warhead)
		{
			if (m_warheads.Add(warhead))
			{
				warhead.OnMarkForClose += warhead_OnClose;
			}
		}

		public static void RemoveWarhead(MyWarhead warhead)
		{
			if (m_warheads.Remove(warhead))
			{
				warhead.OnMarkForClose -= warhead_OnClose;
			}
		}

		public static bool Contains(MyWarhead warhead)
		{
			return m_warheads.Contains(warhead);
		}

		private static void warhead_OnClose(MyEntity obj)
		{
			m_warheads.Remove(obj as MyWarhead);
		}

		public override void UpdateBeforeSimulation()
		{
			int num = 16;
			foreach (MyWarhead warhead in m_warheads)
			{
				if (warhead.Countdown(num))
				{
					warhead.RemainingMS -= num;
					if (warhead.RemainingMS <= 0)
					{
						m_warheadsToExplode.Add(warhead);
					}
				}
			}
			foreach (MyWarhead item in m_warheadsToExplode)
			{
				RemoveWarhead(item);
				if (Sync.IsServer)
				{
					item.Explode();
				}
			}
			m_warheadsToExplode.Clear();
		}

		public override void Draw()
		{
			base.Draw();
			foreach (BoundingSphere debugWarheadShrink in DebugWarheadShrinks)
			{
				MyRenderProxy.DebugDrawSphere(debugWarheadShrink.Center, debugWarheadShrink.Radius, Color.Blue, 1f, depthRead: false);
			}
			foreach (BoundingSphere debugWarheadGroupSphere in DebugWarheadGroupSpheres)
			{
				MyRenderProxy.DebugDrawSphere(debugWarheadGroupSphere.Center, debugWarheadGroupSphere.Radius, Color.Yellow, 1f, depthRead: false);
			}
		}
	}
}
