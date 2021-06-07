using Havok;
using Sandbox.Game.Entities.Cube;
using System.Collections.Generic;
using VRage;
using VRage.Library.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Physics
{
	internal class DisableGridTOIsOptimizer : PhysicsStepOptimizerBase
	{
		public static DisableGridTOIsOptimizer Static;

		private HashSet<MyGridPhysics> m_optimizedGrids = new HashSet<MyGridPhysics>();

		public DisableGridTOIsOptimizer()
		{
			Static = this;
		}

		public override void Unload()
		{
			Static = null;
		}

		public override void EnableOptimizations(List<MyTuple<HkWorld, MyTimeSpan>> timings)
		{
			PhysicsStepOptimizerBase.ForEverySignificantWorld(timings, delegate(HkWorld world)
			{
				PhysicsStepOptimizerBase.ForEveryActivePhysicsBodyOfType(world, delegate(MyGridPhysics body)
				{
					body.ConsiderDisablingTOIs();
				});
			});
		}

		public override void DisableOptimizations()
		{
			while (m_optimizedGrids.Count > 0)
			{
				m_optimizedGrids.FirstElement().DisableTOIOptimization();
			}
		}

		public void Register(MyGridPhysics grid)
		{
			m_optimizedGrids.Add(grid);
		}

		public void Unregister(MyGridPhysics grid)
		{
			m_optimizedGrids.Remove(grid);
		}

		public void DebugDraw()
		{
			foreach (MyGridPhysics optimizedGrid in Static.m_optimizedGrids)
			{
				MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(optimizedGrid.Entity.LocalAABB, optimizedGrid.Entity.WorldMatrix), Color.Yellow, 1f, depthRead: false, smooth: false);
			}
		}
	}
}
