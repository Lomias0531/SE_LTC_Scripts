using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace VRage.Game.Voxels
{
	public class MyWorkTracker<TWorkId, TWork> : IEnumerable<KeyValuePair<TWorkId, TWork>>, IEnumerable where TWork : MyPrecalcJob
	{
		public struct Enumerator : IEnumerator<KeyValuePair<TWorkId, TWork>>, IEnumerator, IDisposable
		{
			private Dictionary<TWorkId, TWork>.Enumerator m_enumerator;

			private readonly Dictionary<TWorkId, TWork> m_syncRoot;

			/// <inheritdoc />
			public KeyValuePair<TWorkId, TWork> Current => m_enumerator.Current;

			/// <inheritdoc />
			object IEnumerator.Current => m_enumerator.Current;

			public Enumerator(Dictionary<TWorkId, TWork> dictionary)
			{
				m_syncRoot = dictionary;
				Monitor.Enter(m_syncRoot);
				m_enumerator = dictionary.GetEnumerator();
			}

			/// <inheritdoc />
			public void Dispose()
			{
				try
				{
					m_enumerator.Dispose();
				}
				finally
				{
					Monitor.Exit(m_syncRoot);
				}
			}

			/// <inheritdoc />
			public bool MoveNext()
			{
				return m_enumerator.MoveNext();
			}

			/// <inheritdoc />
			public void Reset()
			{
				m_enumerator = m_syncRoot.GetEnumerator();
			}
		}

		private readonly Dictionary<TWorkId, TWork> m_worksById;

		public bool HasAny
		{
			get
			{
				lock (m_worksById)
				{
					return m_worksById.Count > 0;
				}
			}
		}

		public MyWorkTracker(IEqualityComparer<TWorkId> comparer = null)
		{
			m_worksById = new Dictionary<TWorkId, TWork>(comparer ?? EqualityComparer<TWorkId>.Default);
		}

		public void Add(TWorkId id, TWork work)
		{
			lock (m_worksById)
			{
				work.IsValid = true;
				m_worksById.Add(id, work);
			}
		}

		public bool Invalidate(TWorkId id)
		{
			lock (m_worksById)
			{
				if (m_worksById.TryGetValue(id, out TWork value))
				{
					value.IsValid = false;
					return true;
				}
			}
			return false;
		}

		public void InvalidateAll()
		{
			lock (m_worksById)
			{
				foreach (TWork value in m_worksById.Values)
				{
					value.IsValid = false;
				}
			}
		}

		public void CancelAll()
		{
			lock (m_worksById)
			{
				foreach (KeyValuePair<TWorkId, TWork> item in m_worksById)
				{
					item.Value.Cancel();
				}
				m_worksById.Clear();
			}
		}

		public TWork Cancel(TWorkId id)
		{
			lock (m_worksById)
			{
				if (!m_worksById.TryGetValue(id, out TWork value))
				{
					return value;
				}
				if (!m_worksById.Remove(id))
				{
					return value;
				}
				value.Cancel();
				return value;
			}
		}

		public TWork CancelIfStarted(TWorkId id)
		{
			lock (m_worksById)
			{
				if (!m_worksById.TryGetValue(id, out TWork value))
				{
					return value;
				}
				if (!value.Started)
				{
					return value;
				}
				if (!m_worksById.Remove(id))
				{
					return value;
				}
				value.Cancel();
				return value;
			}
		}

		public bool Exists(TWorkId id)
		{
			lock (m_worksById)
			{
				return m_worksById.ContainsKey(id);
			}
		}

		public bool TryGet(TWorkId id, out TWork work)
		{
			lock (m_worksById)
			{
				return m_worksById.TryGetValue(id, out work);
			}
		}

		public void Complete(TWorkId id)
		{
			lock (m_worksById)
			{
				m_worksById.Remove(id);
			}
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(m_worksById);
		}

		IEnumerator<KeyValuePair<TWorkId, TWork>> IEnumerable<KeyValuePair<TWorkId, TWork>>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
