using Sandbox.Engine.Physics;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Collections;
using VRage.Game.ModAPI;
using VRage.Scripting;
using VRage.Utils;
using VRageMath;

namespace Sandbox.ModAPI.Physics
{
	internal class MyPhysics : IMyPhysics
	{
		public static readonly MyPhysics Static;

		private MyConcurrentPool<List<Sandbox.Engine.Physics.MyPhysics.HitInfo>> m_collectorsPool = new MyConcurrentPool<List<Sandbox.Engine.Physics.MyPhysics.HitInfo>>(10, delegate(List<Sandbox.Engine.Physics.MyPhysics.HitInfo> x)
		{
			x.Clear();
		});

		int IMyPhysics.StepsLastSecond => Sandbox.Engine.Physics.MyPhysics.StepsLastSecond;

		float IMyPhysics.SimulationRatio => Sandbox.Engine.Physics.MyPhysics.SimulationRatio;

		float IMyPhysics.ServerSimulationRatio => Sync.ServerSimulationRatio;

		static MyPhysics()
		{
			Static = new MyPhysics();
		}

		bool IMyPhysics.CastLongRay(Vector3D from, Vector3D to, out IHitInfo hitInfo, bool any)
		{
			AssertMainThread();
			Sandbox.Engine.Physics.MyPhysics.HitInfo? hitInfo2 = Sandbox.Engine.Physics.MyPhysics.CastLongRay(from, to, any);
			if (hitInfo2.HasValue)
			{
				hitInfo = hitInfo2;
				return true;
			}
			hitInfo = null;
			return false;
		}

		bool IMyPhysics.CastRay(Vector3D from, Vector3D to, out IHitInfo hitInfo, int raycastFilterLayer)
		{
			AssertMainThread();
			Sandbox.Engine.Physics.MyPhysics.HitInfo? hitInfo2 = Sandbox.Engine.Physics.MyPhysics.CastRay(from, to, raycastFilterLayer);
			if (hitInfo2.HasValue)
			{
				hitInfo = hitInfo2;
				return true;
			}
			hitInfo = null;
			return false;
		}

		void IMyPhysics.CastRay(Vector3D from, Vector3D to, List<IHitInfo> toList, int raycastFilterLayer)
		{
			AssertMainThread();
			List<Sandbox.Engine.Physics.MyPhysics.HitInfo> list = m_collectorsPool.Get();
			toList.Clear();
			Sandbox.Engine.Physics.MyPhysics.CastRay(from, to, list, raycastFilterLayer);
			foreach (Sandbox.Engine.Physics.MyPhysics.HitInfo item in list)
			{
				toList.Add(item);
			}
			m_collectorsPool.Return(list);
		}

		bool IMyPhysics.CastRay(Vector3D from, Vector3D to, out IHitInfo hitInfo, uint raycastCollisionFilter, bool ignoreConvexShape)
		{
			AssertMainThread();
			Sandbox.Engine.Physics.MyPhysics.HitInfo hitInfo2;
			bool result = Sandbox.Engine.Physics.MyPhysics.CastRay(from, to, out hitInfo2, raycastCollisionFilter, ignoreConvexShape);
			hitInfo = hitInfo2;
			return result;
		}

		public void CastRayParallel(ref Vector3D from, ref Vector3D to, int raycastCollisionFilter, Action<IHitInfo> callback)
		{
			Sandbox.Engine.Physics.MyPhysics.CastRayParallel(ref from, ref to, raycastCollisionFilter, delegate(Sandbox.Engine.Physics.MyPhysics.HitInfo? x)
			{
				callback(x);
			});
		}

		public void CastRayParallel(ref Vector3D from, ref Vector3D to, List<IHitInfo> toList, int raycastCollisionFilter, Action<List<IHitInfo>> callback)
		{
			Sandbox.Engine.Physics.MyPhysics.CastRayParallel(ref from, ref to, m_collectorsPool.Get(), raycastCollisionFilter, delegate(List<Sandbox.Engine.Physics.MyPhysics.HitInfo> hits)
			{
				foreach (Sandbox.Engine.Physics.MyPhysics.HitInfo hit in hits)
				{
					toList.Add(hit);
				}
				m_collectorsPool.Return(hits);
				callback(toList);
			});
		}

		void IMyPhysics.EnsurePhysicsSpace(BoundingBoxD aabb)
		{
			AssertMainThread();
			Sandbox.Engine.Physics.MyPhysics.EnsurePhysicsSpace(aabb);
		}

		int IMyPhysics.GetCollisionLayer(string strLayer)
		{
			return Sandbox.Engine.Physics.MyPhysics.GetCollisionLayer(strLayer);
		}

		private void AssertMainThread()
		{
			if (MyUtils.MainThread != Thread.CurrentThread)
			{
				MyModWatchdog.ReportIncorrectBehaviour(MyCommonTexts.ModRuleViolation_PhysicsParallelAccess);
			}
		}
	}
}
