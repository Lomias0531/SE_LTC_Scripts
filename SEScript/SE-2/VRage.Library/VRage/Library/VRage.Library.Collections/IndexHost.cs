using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Library.Collections.__helper_namespace;

namespace VRage.Library.Collections
{
	public class IndexHost
	{
		private readonly ComponentIndex NullIndex = new ComponentIndex(new List<Type>());

		private readonly Dictionary<List<Type>, WeakReference> m_indexes;

		public IndexHost()
		{
			m_indexes = new Dictionary<List<Type>, WeakReference>(new TypeListComparer());
			m_indexes[NullIndex.Types] = new WeakReference(NullIndex);
		}

		private ComponentIndex GetForTypes(List<Type> types)
		{
			ComponentIndex result;
			if (!m_indexes.TryGetValue(types, out WeakReference value) || !value.IsAlive)
			{
				if (value == null)
				{
					value = new WeakReference(null);
				}
				result = new ComponentIndex(types);
				m_indexes[types] = value;
			}
			else
			{
				result = (ComponentIndex)value.Target;
			}
			return result;
		}

		public ComponentIndex GetAfterInsert(ComponentIndex current, Type newType, out int insertionPoint)
		{
			List<Type> list = current.Types.ToList();
			insertionPoint = ~list.BinarySearch(newType, TypeComparer.Instance);
			list.Insert(insertionPoint, newType);
			return GetForTypes(list);
		}

		public ComponentIndex GetAfterRemove(ComponentIndex current, Type oldType, out int removalPoint)
		{
			List<Type> list = current.Types.ToList();
			removalPoint = current.Index[oldType];
			list.RemoveAt(removalPoint);
			return GetForTypes(list);
		}

		public ComponentIndex GetEmptyComponentIndex()
		{
			return NullIndex;
		}
	}
}
