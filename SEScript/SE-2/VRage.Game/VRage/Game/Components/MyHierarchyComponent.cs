using System;
using System.Collections.Generic;
using VRage.Network;
using VRageMath;

namespace VRage.Game.Components
{
	public class MyHierarchyComponent<TYPE> : MyHierarchyComponentBase
	{
		private class VRage_Game_Components_MyHierarchyComponent_00601_003C_003EActor : IActivator, IActivator<MyHierarchyComponent<TYPE>>
		{
			private sealed override object CreateInstance()
			{
				return new MyHierarchyComponent<TYPE>();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyHierarchyComponent<TYPE> CreateInstance()
			{
				return new MyHierarchyComponent<TYPE>();
			}

			MyHierarchyComponent<TYPE> IActivator<MyHierarchyComponent<TYPE>>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Action<BoundingBoxD, List<TYPE>> QueryAABBImpl;

		public Action<BoundingSphereD, List<TYPE>> QuerySphereImpl;

		public Action<LineD, List<MyLineSegmentOverlapResult<TYPE>>> QueryLineImpl;

		public void QueryAABB(ref BoundingBoxD aabb, List<TYPE> result)
		{
			if (base.Entity != null && !base.Entity.MarkedForClose && QueryAABBImpl != null)
			{
				QueryAABBImpl(aabb, result);
			}
		}

		public void QuerySphere(ref BoundingSphereD sphere, List<TYPE> result)
		{
			if (!base.Entity.MarkedForClose && QuerySphereImpl != null)
			{
				QuerySphereImpl(sphere, result);
			}
		}

		public void QueryLine(ref LineD line, List<MyLineSegmentOverlapResult<TYPE>> result)
		{
			if (!base.Entity.MarkedForClose && QueryLineImpl != null)
			{
				QueryLineImpl(line, result);
			}
		}
	}
}
