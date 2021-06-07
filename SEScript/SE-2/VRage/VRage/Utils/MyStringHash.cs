using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Utils
{
	/// <summary>
	/// Generates string hashes deterministically and crashes on collisions. When used as key for hash tables (Dictionary or HashSet)
	/// always pass in MyStringHash.Comparer, otherwise lookups will allocate memory! Can be safely used in network but never serialize to disk!
	///
	/// IDs are computed as hash from string so there is a risk of collisions. Use only when MyStringId is
	/// not sufficient (eg. sending over network). Because the odds of collision get higher the more hashes are in use, do not use this for
	/// generated strings and make sure hashes are computed deterministically (eg. at startup) and don't require lengthy gameplay. This way
	/// we know about any collision early and not from rare and random crash reports.
	/// </summary>
	[ProtoContract]
	public struct MyStringHash : IEquatable<MyStringHash>, IXmlSerializable
	{
		public class HashComparerType : IComparer<MyStringHash>, IEqualityComparer<MyStringHash>
		{
			public int Compare(MyStringHash x, MyStringHash y)
			{
				return x.m_hash - y.m_hash;
			}

			public bool Equals(MyStringHash x, MyStringHash y)
			{
				return x.m_hash == y.m_hash;
			}

			public int GetHashCode(MyStringHash obj)
			{
				return obj.m_hash;
			}
		}

		protected class VRage_Utils_MyStringHash_003C_003Em_hash_003C_003EAccessor : IMemberAccessor<MyStringHash, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStringHash owner, in int value)
			{
				owner.m_hash = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStringHash owner, out int value)
			{
				value = owner.m_hash;
			}
		}

		public static readonly MyStringHash NullOrEmpty;

		[ProtoMember(1)]
		public int m_hash;

		public static readonly HashComparerType Comparer;

		private static readonly FastResourceLock m_lock;

		private static Dictionary<string, MyStringHash> m_stringToHash;

		private static Dictionary<MyStringHash, string> m_hashToString;

		public string String
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					return m_hashToString[this];
				}
			}
		}

		private MyStringHash(int hash)
		{
			m_hash = hash;
		}

		public override string ToString()
		{
			return String;
		}

		public override int GetHashCode()
		{
			return m_hash;
		}

		public override bool Equals(object obj)
		{
			if (obj is MyStringHash)
			{
				return Equals((MyStringHash)obj);
			}
			return false;
		}

		public bool Equals(MyStringHash id)
		{
			return m_hash == id.m_hash;
		}

		public static bool operator ==(MyStringHash lhs, MyStringHash rhs)
		{
			return lhs.m_hash == rhs.m_hash;
		}

		public static bool operator !=(MyStringHash lhs, MyStringHash rhs)
		{
			return lhs.m_hash != rhs.m_hash;
		}

		public static explicit operator int(MyStringHash id)
		{
			return id.m_hash;
		}

		static MyStringHash()
		{
			Comparer = new HashComparerType();
			m_lock = new FastResourceLock();
			m_stringToHash = new Dictionary<string, MyStringHash>(50);
			m_hashToString = new Dictionary<MyStringHash, string>(50, Comparer);
			NullOrEmpty = GetOrCompute("");
		}

		public static MyStringHash GetOrCompute(string str)
		{
			if (str == null)
			{
				return NullOrEmpty;
			}
			MyStringHash value;
			using (m_lock.AcquireSharedUsing())
			{
				if (m_stringToHash.TryGetValue(str, out value))
				{
					return value;
				}
			}
			using (m_lock.AcquireExclusiveUsing())
			{
				if (!m_stringToHash.TryGetValue(str, out value))
				{
					value = new MyStringHash(MyUtils.GetHash(str, 0));
					m_hashToString.Add(value, str);
					m_stringToHash.Add(str, value);
				}
				return value;
			}
		}

		public static MyStringHash Get(string str)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_stringToHash[str];
			}
		}

		public static bool TryGet(string str, out MyStringHash id)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_stringToHash.TryGetValue(str, out id);
			}
		}

		public static MyStringHash TryGet(string str)
		{
			using (m_lock.AcquireSharedUsing())
			{
				m_stringToHash.TryGetValue(str, out MyStringHash value);
				return value;
			}
		}

		/// <summary>
		/// Think HARD before using this. Usually you should be able to use MyStringHash as it is without conversion to int.
		/// </summary>
		public static MyStringHash TryGet(int id)
		{
			using (m_lock.AcquireSharedUsing())
			{
				MyStringHash myStringHash = new MyStringHash(id);
				if (m_hashToString.ContainsKey(myStringHash))
				{
					return myStringHash;
				}
				return NullOrEmpty;
			}
		}

		public static bool IsKnown(MyStringHash id)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_hashToString.ContainsKey(id);
			}
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			m_hash = GetOrCompute(reader.ReadInnerXml()).m_hash;
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteString(String);
		}
	}
}
