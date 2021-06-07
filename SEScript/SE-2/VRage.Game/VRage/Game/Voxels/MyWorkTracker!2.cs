namespace VRage.Game.Voxels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class MyWorkTracker<TWorkId, TWork> : IEnumerable<KeyValuePair<TWorkId, TWork>>, IEnumerable where TWork: MyPrecalcJob
    {
        private readonly Dictionary<TWorkId, TWork> m_worksById;

        public MyWorkTracker(IEqualityComparer<TWorkId> comparer = null)
        {
            this.m_worksById = new Dictionary<TWorkId, TWork>((IEqualityComparer<TWorkId>) (comparer ?? EqualityComparer<TWorkId>.get_Default()));
        }

        public void Add(TWorkId id, TWork work)
        {
            Dictionary<TWorkId, TWork> worksById = this.m_worksById;
            lock (worksById)
            {
                work.IsValid = true;
                this.m_worksById.Add(id, work);
            }
        }

        public TWork Cancel(TWorkId id)
        {
            Dictionary<TWorkId, TWork> worksById = this.m_worksById;
            lock (worksById)
            {
                if (this.m_worksById.TryGetValue(id, out TWork local) && this.m_worksById.Remove(id))
                {
                    local.Cancel();
                }
            }
            return local;
        }

        public void CancelAll()
        {
            Dictionary<TWorkId, TWork> worksById = this.m_worksById;
            lock (worksById)
            {
                using (Dictionary<TWorkId, TWork>.Enumerator enumerator = this.m_worksById.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.get_Current().Value.Cancel();
                    }
                }
                this.m_worksById.Clear();
            }
        }

        public TWork CancelIfStarted(TWorkId id)
        {
            Dictionary<TWorkId, TWork> worksById = this.m_worksById;
            lock (worksById)
            {
                if ((this.m_worksById.TryGetValue(id, out TWork local) && local.Started) && this.m_worksById.Remove(id))
                {
                    local.Cancel();
                }
            }
            return local;
        }

        public void Complete(TWorkId id)
        {
            Dictionary<TWorkId, TWork> worksById = this.m_worksById;
            lock (worksById)
            {
                this.m_worksById.Remove(id);
            }
        }

        public bool Exists(TWorkId id)
        {
            Dictionary<TWorkId, TWork> worksById = this.m_worksById;
            lock (worksById)
            {
                return this.m_worksById.ContainsKey(id);
            }
        }

        public Enumerator<TWorkId, TWork> GetEnumerator() => 
            new Enumerator<TWorkId, TWork>(this.m_worksById);

        public bool Invalidate(TWorkId id)
        {
            Dictionary<TWorkId, TWork> worksById = this.m_worksById;
            lock (worksById)
            {
                if (this.m_worksById.TryGetValue(id, out TWork local))
                {
                    local.IsValid = false;
                    return true;
                }
            }
            return false;
        }

        public void InvalidateAll()
        {
            Dictionary<TWorkId, TWork> worksById = this.m_worksById;
            lock (worksById)
            {
                using (Dictionary<TWorkId, TWork>.ValueCollection.Enumerator enumerator = this.m_worksById.get_Values().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.IsValid = false;
                    }
                }
            }
        }

        IEnumerator<KeyValuePair<TWorkId, TWork>> IEnumerable<KeyValuePair<TWorkId, TWork>>.GetEnumerator() => 
            this.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public bool TryGet(TWorkId id, out TWork work)
        {
            Dictionary<TWorkId, TWork> worksById = this.m_worksById;
            lock (worksById)
            {
                return this.m_worksById.TryGetValue(id, out work);
            }
        }

        public bool HasAny
        {
            get
            {
                Dictionary<TWorkId, TWork> worksById = this.m_worksById;
                lock (worksById)
                {
                    return (this.m_worksById.Count > 0);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Enumerator : IEnumerator<KeyValuePair<TWorkId, TWork>>, IEnumerator, IDisposable
        {
            private Dictionary<TWorkId, TWork>.Enumerator m_enumerator;
            private readonly Dictionary<TWorkId, TWork> m_syncRoot;
            public Enumerator(Dictionary<TWorkId, TWork> dictionary)
            {
                this.m_syncRoot = dictionary;
                Monitor.Enter(this.m_syncRoot);
                this.m_enumerator = dictionary.GetEnumerator();
            }

            public void Dispose()
            {
                try
                {
                    this.m_enumerator.Dispose();
                }
                finally
                {
                    Monitor.Exit(this.m_syncRoot);
                }
            }

            public bool MoveNext() => 
                this.m_enumerator.MoveNext();

            public void Reset()
            {
                this.m_enumerator = this.m_syncRoot.GetEnumerator();
            }

            public KeyValuePair<TWorkId, TWork> Current =>
                this.m_enumerator.get_Current();
            object IEnumerator.Current =>
                this.m_enumerator.get_Current();
        }
    }
}

