using Sandbox.Game.Entities.Cube;
using System.Collections.Generic;
using VRage.Game;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	internal static class MyRadioBroadcasters
	{
		private static MyDynamicAABBTreeD m_aabbTree = new MyDynamicAABBTreeD(MyConstants.GAME_PRUNING_STRUCTURE_AABB_EXTENSION);

		public static void AddBroadcaster(MyRadioBroadcaster broadcaster)
		{
			if (broadcaster.RadioProxyID == -1)
			{
				BoundingBoxD aabb = BoundingBoxD.CreateFromSphere(new BoundingSphereD(broadcaster.BroadcastPosition, broadcaster.BroadcastRadius));
				broadcaster.RadioProxyID = m_aabbTree.AddProxy(ref aabb, broadcaster, 0u);
			}
		}

		public static void RemoveBroadcaster(MyRadioBroadcaster broadcaster)
		{
			if (broadcaster.RadioProxyID != -1)
			{
				m_aabbTree.RemoveProxy(broadcaster.RadioProxyID);
				broadcaster.RadioProxyID = -1;
			}
		}

		public static void MoveBroadcaster(MyRadioBroadcaster broadcaster)
		{
			if (broadcaster.RadioProxyID != -1)
			{
				BoundingBoxD aabb = BoundingBoxD.CreateFromSphere(new BoundingSphereD(broadcaster.BroadcastPosition, broadcaster.BroadcastRadius));
				m_aabbTree.MoveProxy(broadcaster.RadioProxyID, ref aabb, Vector3.Zero);
			}
		}

		public static void Clear()
		{
			m_aabbTree.Clear();
		}

		public static void GetAllBroadcastersInSphere(BoundingSphereD sphere, List<MyDataBroadcaster> result)
		{
			m_aabbTree.OverlapAllBoundingSphere(ref sphere, result, clear: false);
			for (int num = result.Count - 1; num >= 0; num--)
			{
				MyRadioBroadcaster myRadioBroadcaster = result[num] as MyRadioBroadcaster;
				if (myRadioBroadcaster != null && myRadioBroadcaster.Entity != null)
				{
					double num2 = sphere.Radius + (double)myRadioBroadcaster.BroadcastRadius;
					num2 *= num2;
					if (Vector3D.DistanceSquared(sphere.Center, myRadioBroadcaster.BroadcastPosition) > num2)
					{
						result.RemoveAtFast(num);
					}
				}
			}
		}

		public static void DebugDraw()
		{
			List<MyRadioBroadcaster> list = new List<MyRadioBroadcaster>();
			List<BoundingBoxD> boxsList = new List<BoundingBoxD>();
			m_aabbTree.GetAll(list, clear: true, boxsList);
			for (int i = 0; i < list.Count; i++)
			{
				MyRenderProxy.DebugDrawSphere(list[i].BroadcastPosition, list[i].BroadcastRadius, Color.White, 1f, depthRead: false);
			}
		}
	}
}
