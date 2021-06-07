using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Game.Components
{
	public sealed class MyAggregateComponentList
	{
		private List<MyComponentBase> m_components = new List<MyComponentBase>();

		public ListReader<MyComponentBase> Reader => new ListReader<MyComponentBase>(m_components);

		public void AddComponent(MyComponentBase component)
		{
			m_components.Add(component);
		}

		public void RemoveComponentAt(int index)
		{
			m_components.RemoveAtFast(index);
		}

		public int GetComponentIndex(MyComponentBase component)
		{
			return m_components.IndexOf(component);
		}

		public bool RemoveComponent(MyComponentBase component)
		{
			if (Contains(component))
			{
				component.OnBeforeRemovedFromContainer();
				if (m_components.Remove(component))
				{
					return true;
				}
				foreach (MyComponentBase component2 in m_components)
				{
					if (component2 is IMyComponentAggregate && (component2 as IMyComponentAggregate).ChildList.RemoveComponent(component))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		public bool Contains(MyComponentBase component)
		{
			if (m_components.Contains(component))
			{
				return true;
			}
			foreach (MyComponentBase component2 in m_components)
			{
				if (component2 is IMyComponentAggregate && (component2 as IMyComponentAggregate).ChildList.Contains(component))
				{
					return true;
				}
			}
			return false;
		}
	}
}
