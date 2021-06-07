using Sandbox.Engine.Utils;
using System.Collections.Generic;
using VRage.Algorithms;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyNavgroupLinks
	{
		private class PathEdge : IMyPathEdge<MyNavigationPrimitive>
		{
			private static PathEdge Static = new PathEdge();

			private MyNavigationPrimitive m_primitive1;

			private MyNavigationPrimitive m_primitive2;

			public static PathEdge GetEdge(MyNavigationPrimitive primitive1, MyNavigationPrimitive primitive2)
			{
				Static.m_primitive1 = primitive1;
				Static.m_primitive2 = primitive2;
				return Static;
			}

			public float GetWeight()
			{
				if (m_primitive1.Group == m_primitive2.Group)
				{
					return Vector3.Distance(m_primitive1.Position, m_primitive2.Position);
				}
				return (float)Vector3D.Distance(m_primitive1.WorldPosition, m_primitive2.WorldPosition);
			}

			public MyNavigationPrimitive GetOtherVertex(MyNavigationPrimitive vertex1)
			{
				if (vertex1 == m_primitive1)
				{
					return m_primitive2;
				}
				return m_primitive1;
			}
		}

		private Dictionary<MyNavigationPrimitive, List<MyNavigationPrimitive>> m_links;

		public MyNavgroupLinks()
		{
			m_links = new Dictionary<MyNavigationPrimitive, List<MyNavigationPrimitive>>();
		}

		public void AddLink(MyNavigationPrimitive primitive1, MyNavigationPrimitive primitive2, bool onlyIfNotPresent = false)
		{
			AddLinkInternal(primitive1, primitive2, onlyIfNotPresent);
			AddLinkInternal(primitive2, primitive1, onlyIfNotPresent);
			primitive1.HasExternalNeighbors = true;
			primitive2.HasExternalNeighbors = true;
		}

		public void RemoveLink(MyNavigationPrimitive primitive1, MyNavigationPrimitive primitive2)
		{
			if (RemoveLinkInternal(primitive1, primitive2))
			{
				primitive1.HasExternalNeighbors = false;
			}
			if (RemoveLinkInternal(primitive2, primitive1))
			{
				primitive2.HasExternalNeighbors = false;
			}
		}

		public int GetLinkCount(MyNavigationPrimitive primitive)
		{
			List<MyNavigationPrimitive> value = null;
			m_links.TryGetValue(primitive, out value);
			return value?.Count ?? 0;
		}

		public MyNavigationPrimitive GetLinkedNeighbor(MyNavigationPrimitive primitive, int index)
		{
			List<MyNavigationPrimitive> value = null;
			m_links.TryGetValue(primitive, out value);
			return value?[index];
		}

		public IMyPathEdge<MyNavigationPrimitive> GetEdge(MyNavigationPrimitive primitive, int index)
		{
			List<MyNavigationPrimitive> value = null;
			m_links.TryGetValue(primitive, out value);
			if (value == null)
			{
				return null;
			}
			MyNavigationPrimitive primitive2 = value[index];
			return PathEdge.GetEdge(primitive, primitive2);
		}

		public List<MyNavigationPrimitive> GetLinks(MyNavigationPrimitive primitive)
		{
			List<MyNavigationPrimitive> value = null;
			m_links.TryGetValue(primitive, out value);
			return value;
		}

		public void RemoveAllLinks(MyNavigationPrimitive primitive)
		{
			List<MyNavigationPrimitive> value = null;
			m_links.TryGetValue(primitive, out value);
			if (value != null)
			{
				foreach (MyNavigationPrimitive item in value)
				{
					List<MyNavigationPrimitive> value2 = null;
					m_links.TryGetValue(item, out value2);
					if (value2 == null)
					{
						return;
					}
					value2.Remove(primitive);
					if (value2.Count == 0)
					{
						m_links.Remove(item);
					}
				}
				m_links.Remove(primitive);
			}
		}

		public void DebugDraw(Color lineColor)
		{
			if (MyFakes.DEBUG_DRAW_NAVMESH_LINKS)
			{
				foreach (KeyValuePair<MyNavigationPrimitive, List<MyNavigationPrimitive>> link in m_links)
				{
					MyNavigationPrimitive key = link.Key;
					List<MyNavigationPrimitive> value = link.Value;
					for (int i = 0; i < value.Count; i++)
					{
						MyNavigationPrimitive myNavigationPrimitive = value[i];
						Vector3D worldPosition = key.WorldPosition;
						Vector3D worldPosition2 = myNavigationPrimitive.WorldPosition;
						Vector3D value2 = (worldPosition + worldPosition2) * 0.5;
						Vector3D value3 = (value2 + worldPosition) * 0.5;
						Vector3D up = Vector3D.Up;
						MyRenderProxy.DebugDrawLine3D(worldPosition, value3 + up * 0.4, lineColor, lineColor, depthRead: false);
						MyRenderProxy.DebugDrawLine3D(value3 + up * 0.4, value2 + up * 0.5, lineColor, lineColor, depthRead: false);
					}
				}
			}
		}

		private void AddLinkInternal(MyNavigationPrimitive primitive1, MyNavigationPrimitive primitive2, bool onlyIfNotPresent = false)
		{
			List<MyNavigationPrimitive> value = null;
			m_links.TryGetValue(primitive1, out value);
			if (value == null)
			{
				value = new List<MyNavigationPrimitive>();
				m_links.Add(primitive1, value);
			}
			if (onlyIfNotPresent)
			{
				if (!value.Contains(primitive2))
				{
					value.Add(primitive2);
				}
			}
			else
			{
				value.Add(primitive2);
			}
		}

		private bool RemoveLinkInternal(MyNavigationPrimitive primitive1, MyNavigationPrimitive primitive2)
		{
			List<MyNavigationPrimitive> value = null;
			m_links.TryGetValue(primitive1, out value);
			if (value != null)
			{
				value.Remove(primitive2);
				if (value.Count == 0)
				{
					m_links.Remove(primitive1);
					return true;
				}
			}
			return false;
		}
	}
}
