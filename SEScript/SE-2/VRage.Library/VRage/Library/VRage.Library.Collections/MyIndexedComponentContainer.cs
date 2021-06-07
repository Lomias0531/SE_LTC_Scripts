using System;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Library.Collections
{
	public class MyIndexedComponentContainer<T> where T : class
	{
		private static readonly IndexHost Host = new IndexHost();

		private ComponentIndex m_componentIndex;

		private readonly List<T> m_components = new List<T>();

		public ListReader<T> Components => m_components;

		public T this[int index] => m_components[index];

		public T this[Type type] => m_components[m_componentIndex.Index[type]];

		public int Count => m_components.Count;

		public MyIndexedComponentContainer()
		{
			m_componentIndex = Host.GetEmptyComponentIndex();
		}

		/// Create a component from a template.
		public MyIndexedComponentContainer(MyComponentContainerTemplate<T> template)
		{
			m_components.Capacity = template.Components.Count;
			for (int i = 0; i < template.Components.Count; i++)
			{
				Func<Type, T> func = template.Components[i];
				Type arg = template.Components.m_componentIndex.Types[i];
				m_components.Add(func(arg));
			}
			m_componentIndex = template.Components.m_componentIndex;
		}

		public TComponent GetComponent<TComponent>() where TComponent : T
		{
			return (TComponent)this[typeof(TComponent)];
		}

		public TComponent TryGetComponent<TComponent>() where TComponent : class, T
		{
			return (TComponent)TryGetComponent(typeof(TComponent));
		}

		public T TryGetComponent(Type t)
		{
			if (m_componentIndex.Index.TryGetValue(t, out int value))
			{
				return m_components[value];
			}
			return null;
		}

		public void Add(Type slot, T component)
		{
			if (!m_componentIndex.Index.ContainsKey(slot))
			{
				m_componentIndex = Host.GetAfterInsert(m_componentIndex, slot, out int insertionPoint);
				m_components.Insert(insertionPoint, component);
			}
		}

		public void Remove(Type slot)
		{
			if (m_componentIndex.Index.ContainsKey(slot))
			{
				m_componentIndex = Host.GetAfterRemove(m_componentIndex, slot, out int removalPoint);
				m_components.RemoveAt(removalPoint);
			}
		}

		public void Clear()
		{
			m_components.Clear();
			m_componentIndex = Host.GetEmptyComponentIndex();
		}

		public bool Contains<TComponent>() where TComponent : T
		{
			return m_componentIndex.Index.ContainsKey(typeof(TComponent));
		}
	}
}
