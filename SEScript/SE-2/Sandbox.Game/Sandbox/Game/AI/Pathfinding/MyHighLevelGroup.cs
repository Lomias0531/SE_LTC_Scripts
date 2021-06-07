using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Algorithms;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyHighLevelGroup : MyPathFindingSystem<MyNavigationPrimitive>, IMyNavigationGroup
	{
		private IMyNavigationGroup m_lowLevel;

		private Dictionary<int, MyHighLevelPrimitive> m_primitives;

		private Dictionary<int, List<IMyHighLevelPrimitiveObserver>> m_primitiveObservers;

		private MyNavgroupLinks m_links;

		private int m_removingPrimitive = -1;

		private static List<int> m_tmpNeighbors = new List<int>();

		public IMyNavigationGroup LowLevelGroup => m_lowLevel;

		public MyHighLevelGroup HighLevelGroup => null;

		public MyHighLevelGroup(IMyNavigationGroup lowLevelPathfinding, MyNavgroupLinks links, Func<long> timestampFunction)
			: base(128, timestampFunction)
		{
			m_lowLevel = lowLevelPathfinding;
			m_primitives = new Dictionary<int, MyHighLevelPrimitive>();
			m_primitiveObservers = new Dictionary<int, List<IMyHighLevelPrimitiveObserver>>();
			m_links = links;
		}

		public override string ToString()
		{
			if (m_lowLevel == null)
			{
				return "Invalid HLPFG";
			}
			return "HLPFG of " + m_lowLevel.ToString();
		}

		public void AddPrimitive(int index, Vector3 localPosition)
		{
			m_primitives.Add(index, new MyHighLevelPrimitive(this, index, localPosition));
		}

		public MyHighLevelPrimitive TryGetPrimitive(int index)
		{
			MyHighLevelPrimitive value = null;
			m_primitives.TryGetValue(index, out value);
			return value;
		}

		public MyHighLevelPrimitive GetPrimitive(int index)
		{
			MyHighLevelPrimitive value = null;
			m_primitives.TryGetValue(index, out value);
			return value;
		}

		public void RemovePrimitive(int index)
		{
			m_removingPrimitive = index;
			MyHighLevelPrimitive value = null;
			if (!m_primitives.TryGetValue(index, out value))
			{
				m_removingPrimitive = -1;
				return;
			}
			List<IMyHighLevelPrimitiveObserver> value2 = null;
			if (m_primitiveObservers.TryGetValue(index, out value2))
			{
				foreach (IMyHighLevelPrimitiveObserver item in value2)
				{
					item.Invalidate();
				}
			}
			m_primitiveObservers.Remove(index);
			m_links.RemoveAllLinks(value);
			m_tmpNeighbors.Clear();
			value.GetNeighbours(m_tmpNeighbors);
			foreach (int tmpNeighbor in m_tmpNeighbors)
			{
				MyHighLevelPrimitive value3 = null;
				m_primitives.TryGetValue(tmpNeighbor, out value3);
				value3?.Disconnect(index);
			}
			m_primitives.Remove(index);
			m_removingPrimitive = -1;
		}

		public void ConnectPrimitives(int a, int b)
		{
			Connect(a, b);
		}

		public void DisconnectPrimitives(int a, int b)
		{
			Disconnect(a, b);
		}

		private void Connect(int a, int b)
		{
			MyHighLevelPrimitive primitive = GetPrimitive(a);
			MyHighLevelPrimitive primitive2 = GetPrimitive(b);
			if (primitive != null && primitive2 != null)
			{
				primitive.Connect(b);
				primitive2.Connect(a);
			}
		}

		private void Disconnect(int a, int b)
		{
			MyHighLevelPrimitive primitive = GetPrimitive(a);
			MyHighLevelPrimitive primitive2 = GetPrimitive(b);
			if (primitive != null && primitive2 != null)
			{
				primitive.Disconnect(b);
				primitive2.Disconnect(a);
			}
		}

		public MyNavigationPrimitive FindClosestPrimitive(Vector3D point, bool highLevel, ref double closestDistanceSq)
		{
			throw new NotImplementedException();
		}

		public int GetExternalNeighborCount(MyNavigationPrimitive primitive)
		{
			return m_links.GetLinkCount(primitive);
		}

		public MyNavigationPrimitive GetExternalNeighbor(MyNavigationPrimitive primitive, int index)
		{
			return m_links.GetLinkedNeighbor(primitive, index);
		}

		public IMyPathEdge<MyNavigationPrimitive> GetExternalEdge(MyNavigationPrimitive primitive, int index)
		{
			return m_links.GetEdge(primitive, index);
		}

		public void RefinePath(MyPath<MyNavigationPrimitive> path, List<Vector4D> output, ref Vector3 startPoint, ref Vector3 endPoint, int begin, int end)
		{
			throw new NotImplementedException();
		}

		public Vector3 GlobalToLocal(Vector3D globalPos)
		{
			return m_lowLevel.GlobalToLocal(globalPos);
		}

		public Vector3D LocalToGlobal(Vector3 localPos)
		{
			return m_lowLevel.LocalToGlobal(localPos);
		}

		public void ObservePrimitive(MyHighLevelPrimitive primitive, IMyHighLevelPrimitiveObserver observer)
		{
			if (primitive.Parent == this)
			{
				List<IMyHighLevelPrimitiveObserver> value = null;
				int index = primitive.Index;
				if (!m_primitiveObservers.TryGetValue(index, out value))
				{
					value = new List<IMyHighLevelPrimitiveObserver>(4);
					m_primitiveObservers.Add(index, value);
				}
				value.Add(observer);
			}
		}

		public void StopObservingPrimitive(MyHighLevelPrimitive primitive, IMyHighLevelPrimitiveObserver observer)
		{
			if (primitive.Parent != this)
			{
				return;
			}
			List<IMyHighLevelPrimitiveObserver> value = null;
			int index = primitive.Index;
			if (index != m_removingPrimitive && m_primitiveObservers.TryGetValue(index, out value))
			{
				value.Remove(observer);
				if (value.Count == 0)
				{
					m_primitiveObservers.Remove(index);
				}
			}
		}

		public void DebugDraw(bool lite)
		{
			long lastHighLevelTimestamp = MyCestmirPathfindingShorts.Pathfinding.LastHighLevelTimestamp;
			foreach (KeyValuePair<int, MyHighLevelPrimitive> primitive in m_primitives)
			{
				if (lite)
				{
					MyRenderProxy.DebugDrawPoint(primitive.Value.WorldPosition, Color.CadetBlue, depthRead: false);
				}
				else
				{
					MyHighLevelPrimitive value = primitive.Value;
					Vector3D value2 = MySector.MainCamera.WorldMatrix.Down * 0.30000001192092896;
					float num = (float)Vector3D.Distance(value.WorldPosition, MySector.MainCamera.Position);
					float num2 = 7f / num;
					if (num2 > 30f)
					{
						num2 = 30f;
					}
					if (num2 < 0.5f)
					{
						num2 = 0.5f;
					}
					if (num < 100f)
					{
						List<IMyHighLevelPrimitiveObserver> value3 = null;
						if (m_primitiveObservers.TryGetValue(primitive.Key, out value3))
						{
							MyRenderProxy.DebugDrawText3D(value.WorldPosition + value2, value3.Count.ToString(), Color.Red, num2 * 3f, depthRead: false);
						}
						MyRenderProxy.DebugDrawText3D(value.WorldPosition + value2, primitive.Key.ToString(), Color.CadetBlue, num2, depthRead: false);
					}
					for (int i = 0; i < value.GetOwnNeighborCount(); i++)
					{
						MyHighLevelPrimitive myHighLevelPrimitive = value.GetOwnNeighbor(i) as MyHighLevelPrimitive;
						MyRenderProxy.DebugDrawLine3D(value.WorldPosition, myHighLevelPrimitive.WorldPosition, Color.CadetBlue, Color.CadetBlue, depthRead: false);
					}
					if (value.PathfindingData.GetTimestamp() == lastHighLevelTimestamp)
					{
						MyRenderProxy.DebugDrawSphere(value.WorldPosition, 0.5f, Color.DarkRed, 1f, depthRead: false);
					}
				}
			}
		}

		public MyHighLevelPrimitive GetHighLevelPrimitive(MyNavigationPrimitive myNavigationTriangle)
		{
			return null;
		}

		public IMyHighLevelComponent GetComponent(MyHighLevelPrimitive highLevelPrimitive)
		{
			return null;
		}

		public void GetPrimitivesOnPath(ref List<MyHighLevelPrimitive> primitives)
		{
			foreach (KeyValuePair<int, List<IMyHighLevelPrimitiveObserver>> primitiveObserver in m_primitiveObservers)
			{
				MyHighLevelPrimitive item = TryGetPrimitive(primitiveObserver.Key);
				primitives.Add(item);
			}
		}
	}
}
