﻿namespace ProtoBuf
{
    using ProtoBuf.Meta;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal sealed class NetObjectCache
    {
        internal const int Root = 0;
        private MutableList underlyingList;
        private object rootObject;
        private int trapStartIndex;
        private Dictionary<string, int> stringKeys;
        private Dictionary<object, int> objectKeys;

        internal int AddObjectKey(object value, out bool existing)
        {
            int num;
            bool flag;
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (value == this.rootObject)
            {
                existing = true;
                return 0;
            }
            string key = value as string;
            BasicList list = this.List;
            if (key == null)
            {
                if (this.objectKeys == null)
                {
                    this.objectKeys = new Dictionary<object, int>(ReferenceComparer.Default);
                    num = -1;
                }
                else if (!this.objectKeys.TryGetValue(value, out num))
                {
                    num = -1;
                }
            }
            else if (this.stringKeys == null)
            {
                this.stringKeys = new Dictionary<string, int>();
                num = -1;
            }
            else if (!this.stringKeys.TryGetValue(key, out num))
            {
                num = -1;
            }
            existing = flag = num >= 0;
            if (!flag)
            {
                num = list.Add(value);
                if (key == null)
                {
                    this.objectKeys.Add(value, num);
                }
                else
                {
                    this.stringKeys.Add(key, num);
                }
            }
            return (num + 1);
        }

        internal object GetKeyedObject(int key)
        {
            key--;
            if (key == 0)
            {
                if (this.rootObject == null)
                {
                    throw new ProtoException("No root object assigned");
                }
                return this.rootObject;
            }
            BasicList list = this.List;
            if ((key < 0) || (key >= list.Count))
            {
                throw new ProtoException("Internal error; a missing key occurred");
            }
            object obj2 = list[key];
            if (obj2 == null)
            {
                throw new ProtoException("A deferred key does not have a value yet");
            }
            return obj2;
        }

        internal void RegisterTrappedObject(object value)
        {
            if (this.rootObject == null)
            {
                this.rootObject = value;
            }
            else if (this.underlyingList != null)
            {
                for (int i = this.trapStartIndex; i < this.underlyingList.Count; i++)
                {
                    this.trapStartIndex = i + 1;
                    if (this.underlyingList[i] == null)
                    {
                        this.underlyingList[i] = value;
                        return;
                    }
                }
            }
        }

        internal void SetKeyedObject(int key, object value)
        {
            key--;
            if (key == 0)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if ((this.rootObject != null) && (this.rootObject != value))
                {
                    throw new ProtoException("The root object cannot be reassigned");
                }
                this.rootObject = value;
            }
            else
            {
                MutableList list = this.List;
                if (key >= list.Count)
                {
                    if (key != list.Add(value))
                    {
                        throw new ProtoException("Internal error; a key mismatch occurred");
                    }
                }
                else
                {
                    object obj2 = list[key];
                    if (obj2 == null)
                    {
                        list[key] = value;
                    }
                    else if (obj2 != value)
                    {
                        throw new ProtoException("Reference-tracked objects cannot change reference");
                    }
                }
            }
        }

        private MutableList List
        {
            get
            {
                if (this.underlyingList == null)
                {
                    this.underlyingList = new MutableList();
                }
                return this.underlyingList;
            }
        }

        private sealed class ReferenceComparer : IEqualityComparer<object>
        {
            public static readonly NetObjectCache.ReferenceComparer Default = new NetObjectCache.ReferenceComparer();

            private ReferenceComparer()
            {
            }

            bool IEqualityComparer<object>.Equals(object x, object y) => 
                (x == y);

            int IEqualityComparer<object>.GetHashCode(object obj) => 
                RuntimeHelpers.GetHashCode(obj);
        }
    }
}

