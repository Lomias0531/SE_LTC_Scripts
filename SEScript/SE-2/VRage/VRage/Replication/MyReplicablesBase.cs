using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage.Collections;
using VRage.Network;
using VRageMath;

namespace VRage.Replication
{
	public abstract class MyReplicablesBase
	{
		private static readonly HashSet<IMyReplicable> m_empty = new HashSet<IMyReplicable>();

		private readonly Stack<HashSet<IMyReplicable>> m_hashSetPool = new Stack<HashSet<IMyReplicable>>();

		private readonly ConcurrentDictionary<IMyReplicable, HashSet<IMyReplicable>> m_parentToChildren = new ConcurrentDictionary<IMyReplicable, HashSet<IMyReplicable>>();

		private readonly ConcurrentDictionary<IMyReplicable, IMyReplicable> m_childToParent = new ConcurrentDictionary<IMyReplicable, IMyReplicable>();

		private readonly Thread m_mainThread;

		protected MyReplicablesBase(Thread mainThread)
		{
			m_mainThread = mainThread;
		}

		public void GetAllChildren(IMyReplicable replicable, List<IMyReplicable> resultList)
		{
			foreach (IMyReplicable child in GetChildren(replicable))
			{
				resultList.Add(child);
				GetAllChildren(child, resultList);
			}
		}

		/// <summary>
		/// Sets or resets replicable (updates child status and parent).
		/// Returns true if replicable is root, otherwise false.
		/// </summary>
		public void Add(IMyReplicable replicable, out IMyReplicable parent)
		{
			if (replicable.HasToBeChild && TryGetParent(replicable, out parent))
			{
				AddChild(replicable, parent);
			}
			else if (!replicable.HasToBeChild)
			{
				parent = null;
				AddRoot(replicable);
			}
			else
			{
				parent = null;
			}
		}

		/// <summary>
		/// Removes replicable with all children, children of children, etc.
		/// </summary>
		public void RemoveHierarchy(IMyReplicable replicable)
		{
			HashSet<IMyReplicable> valueOrDefault = m_parentToChildren.GetValueOrDefault(replicable, m_empty);
			while (valueOrDefault.Count > 0)
			{
				HashSet<IMyReplicable>.Enumerator enumerator = valueOrDefault.GetEnumerator();
				enumerator.MoveNext();
				RemoveHierarchy(enumerator.Current);
			}
			Remove(replicable);
		}

		private HashSet<IMyReplicable> Obtain()
		{
			if (m_hashSetPool.Count <= 0)
			{
				return new HashSet<IMyReplicable>();
			}
			return m_hashSetPool.Pop();
		}

		public HashSetReader<IMyReplicable> GetChildren(IMyReplicable replicable)
		{
			return m_parentToChildren.GetValueOrDefault(replicable, m_empty);
		}

		private static bool TryGetParent(IMyReplicable replicable, out IMyReplicable parent)
		{
			parent = replicable.GetParent();
			return parent != null;
		}

		/// <summary>
		/// Refreshes replicable, updates it's child status and parent.
		/// </summary>
		public void Refresh(IMyReplicable replicable)
		{
			IMyReplicable value2;
			if (replicable.HasToBeChild && TryGetParent(replicable, out IMyReplicable parent))
			{
				if (m_childToParent.TryGetValue(replicable, out IMyReplicable value))
				{
					if (value != parent)
					{
						RemoveChild(replicable, value);
						AddChild(replicable, parent);
					}
				}
				else
				{
					RemoveRoot(replicable);
					AddChild(replicable, parent);
				}
			}
			else if (m_childToParent.TryGetValue(replicable, out value2))
			{
				RemoveChild(replicable, value2);
				AddRoot(replicable);
			}
		}

		/// <summary>
		/// Removes replicable, children should be already removed
		/// </summary>
		private void Remove(IMyReplicable replicable)
		{
			if (m_childToParent.TryGetValue(replicable, out IMyReplicable value))
			{
				RemoveChild(replicable, value);
			}
			RemoveRoot(replicable);
		}

		protected virtual void AddChild(IMyReplicable replicable, IMyReplicable parent)
		{
			if (!m_parentToChildren.TryGetValue(parent, out HashSet<IMyReplicable> value))
			{
				value = Obtain();
				m_parentToChildren[parent] = value;
			}
			value.Add(replicable);
			m_childToParent[replicable] = parent;
		}

		protected virtual void RemoveChild(IMyReplicable replicable, IMyReplicable parent)
		{
			m_childToParent.Remove(replicable);
			HashSet<IMyReplicable> hashSet = m_parentToChildren[parent];
			hashSet.Remove(replicable);
			if (hashSet.Count == 0)
			{
				m_parentToChildren.Remove(parent);
				m_hashSetPool.Push(hashSet);
			}
		}

		public abstract void IterateRoots(Action<IMyReplicable> p);

		public abstract void GetReplicablesInBox(BoundingBoxD aabb, List<IMyReplicable> list);

		protected abstract void AddRoot(IMyReplicable replicable);

		protected abstract void RemoveRoot(IMyReplicable replicable);

		[Conditional("DEBUG")]
		protected void CheckThread()
		{
		}
	}
}
