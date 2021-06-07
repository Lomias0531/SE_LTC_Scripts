namespace ParallelTasks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using VRage.Library.Threading;

    public class Hashtable<TKey, TData> : IEnumerable<KeyValuePair<TKey, TData>>, IEnumerable
    {
        private static readonly EqualityComparer<TKey> KeyComparer;
        public volatile HashtableNode<TKey, TData>[] array;
        private SpinLock writeLock;
        private static readonly HashtableNode<TKey, TData> DeletedNode;

        static Hashtable()
        {
            Hashtable<TKey, TData>.KeyComparer = EqualityComparer<TKey>.get_Default();
            Hashtable<TKey, TData>.DeletedNode = new HashtableNode<TKey, TData>(default(TKey), default(TData), HashtableToken.Deleted);
        }

        public Hashtable(int initialCapacity)
        {
            if (initialCapacity < 1)
            {
                throw new ArgumentOutOfRangeException("initialCapacity", "cannot be < 1");
            }
            this.array = new HashtableNode<TKey, TData>[initialCapacity];
            this.writeLock = new SpinLock();
        }

        public void Add(TKey key, TData data)
        {
            try
            {
                this.writeLock.Enter();
                if (!this.Insert(this.array, key, data))
                {
                    this.Resize();
                    this.Insert(this.array, key, data);
                }
            }
            finally
            {
                this.writeLock.Exit();
            }
        }

        private bool Find(TKey key, out HashtableNode<TKey, TData> node)
        {
            node = new HashtableNode<TKey, TData>();
            HashtableNode<TKey, TData>[] array = this.array;
            int num = Math.Abs(GetHashCode_HashTable<TKey>.GetHashCode(key)) % array.Length;
            int index = num;
            do
            {
                HashtableNode<TKey, TData> node2 = array[index];
                if (node2.Token == HashtableToken.Empty)
                {
                    return false;
                }
                if ((node2.Token == HashtableToken.Deleted) || !Hashtable<TKey, TData>.KeyComparer.Equals(key, node2.Key))
                {
                    index = (index + 1) % array.Length;
                }
                else
                {
                    node = node2;
                    return true;
                }
            }
            while (index != num);
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TData>> GetEnumerator() => 
            new HashTableEnumerator<TKey, TData>((Hashtable<TKey, TData>) this);

        private bool Insert(HashtableNode<TKey, TData>[] table, TKey key, TData data)
        {
            int num = Math.Abs(GetHashCode_HashTable<TKey>.GetHashCode(key)) % table.Length;
            int index = num;
            do
            {
                HashtableNode<TKey, TData> node = table[index];
                if (((node.Token == HashtableToken.Empty) || (node.Token == HashtableToken.Deleted)) || Hashtable<TKey, TData>.KeyComparer.Equals(key, node.Key))
                {
                    table[index] = new HashtableNode<TKey, TData> { 
                        Key = key,
                        Data = data,
                        Token = HashtableToken.Used
                    };
                    return true;
                }
                index = (index + 1) % table.Length;
            }
            while (index != num);
            return false;
        }

        public void Remove(TKey key)
        {
            try
            {
                HashtableNode<TKey, TData> node;
                this.writeLock.Enter();
                HashtableNode<TKey, TData>[] array = this.array;
                int num = Math.Abs(GetHashCode_HashTable<TKey>.GetHashCode(key)) % array.Length;
                int index = num;
            Label_0026:
                node = array[index];
                if (node.Token != HashtableToken.Empty)
                {
                    if ((node.Token == HashtableToken.Deleted) || !Hashtable<TKey, TData>.KeyComparer.Equals(key, node.Key))
                    {
                        index = (index + 1) % array.Length;
                    }
                    else
                    {
                        array[index] = Hashtable<TKey, TData>.DeletedNode;
                    }
                    if (index != num)
                    {
                        goto Label_0026;
                    }
                }
            }
            finally
            {
                this.writeLock.Exit();
            }
        }

        private void Resize()
        {
            HashtableNode<TKey, TData>[] table = new HashtableNode<TKey, TData>[this.array.Length * 2];
            for (int i = 0; i < this.array.Length; i++)
            {
                HashtableNode<TKey, TData> node = this.array[i];
                if (node.Token == HashtableToken.Used)
                {
                    this.Insert(table, node.Key, node.Data);
                }
            }
            this.array = table;
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            ((IEnumerator) this.GetEnumerator());

        public bool TryGet(TKey key, out TData data)
        {
            if (this.Find(key, out HashtableNode<TKey, TData> node))
            {
                data = node.Data;
                return true;
            }
            data = default(TData);
            return false;
        }

        public void UnsafeSet(TKey key, TData value)
        {
            HashtableNode<TKey, TData>[] array;
            bool flag = false;
            do
            {
                array = this.array;
                int num = Math.Abs(GetHashCode_HashTable<TKey>.GetHashCode(key)) % array.Length;
                int index = num;
                do
                {
                    HashtableNode<TKey, TData> node = array[index];
                    if (Hashtable<TKey, TData>.KeyComparer.Equals(key, node.Key))
                    {
                        array[index] = new HashtableNode<TKey, TData> { 
                            Key = key,
                            Data = value,
                            Token = HashtableToken.Used
                        };
                        flag = true;
                        break;
                    }
                    index = (index + 1) % array.Length;
                }
                while (index != num);
            }
            while (array != this.array);
            if (!flag)
            {
                this.Add(key, value);
            }
        }
    }
}

