using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Utils
{
	/// <summary>
	/// Generates unique IDs for strings. When used as key for hash tables (Dictionary or HashSet)
	/// always pass in MyStringId.Comparer, otherwise lookups will allocate memory! Never serialize to network or disk!
	///
	/// IDs are created sequentially as they get requested so two IDs might be different between sessions or clients and
	/// server. You can safely use ToString() as it will not allocate.
	/// </summary>
	[ProtoContract]
	public struct MyStringId : IXmlSerializable
	{
		public class IdComparerType : IComparer<MyStringId>, IEqualityComparer<MyStringId>
		{
			public int Compare(MyStringId x, MyStringId y)
			{
				return x.m_id - y.m_id;
			}

			public bool Equals(MyStringId x, MyStringId y)
			{
				return x.m_id == y.m_id;
			}

			public int GetHashCode(MyStringId obj)
			{
				return obj.m_id;
			}
		}

		protected class VRage_Utils_MyStringId_003C_003Em_id_003C_003EAccessor : IMemberAccessor<MyStringId, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStringId owner, in int value)
			{
				owner.m_id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStringId owner, out int value)
			{
				value = owner.m_id;
			}
		}

		public static readonly MyStringId NullOrEmpty;

		[ProtoMember(1)]
		public int m_id;

		public static readonly IdComparerType Comparer;

		private static readonly FastResourceLock m_lock;

		private static Dictionary<string, MyStringId> m_stringToId;

		private static Dictionary<MyStringId, string> m_idToString;

		public int Id => m_id;

		public string String
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					return m_idToString[this];
				}
			}
		}

		private MyStringId(int id)
		{
			m_id = id;
		}

		public override string ToString()
		{
			return String;
		}

		public override int GetHashCode()
		{
			return m_id;
		}

		public override bool Equals(object obj)
		{
			if (obj is MyStringId)
			{
				return Equals((MyStringId)obj);
			}
			return false;
		}

		public bool Equals(MyStringId id)
		{
			return m_id == id.m_id;
		}

		public static bool operator ==(MyStringId lhs, MyStringId rhs)
		{
			return lhs.m_id == rhs.m_id;
		}

		public static bool operator !=(MyStringId lhs, MyStringId rhs)
		{
			return lhs.m_id != rhs.m_id;
		}

		public static explicit operator int(MyStringId id)
		{
			return id.m_id;
		}

		static MyStringId()
		{
			Comparer = new IdComparerType();
			m_lock = new FastResourceLock();
			m_stringToId = new Dictionary<string, MyStringId>(50);
			m_idToString = new Dictionary<MyStringId, string>(50, Comparer);
			NullOrEmpty = GetOrCompute("");
		}

		public static MyStringId GetOrCompute(string str)
		{
			if (str == null)
			{
				return NullOrEmpty;
			}
			MyStringId value;
			using (m_lock.AcquireSharedUsing())
			{
				if (m_stringToId.TryGetValue(str, out value))
				{
					return value;
				}
			}
			using (m_lock.AcquireExclusiveUsing())
			{
				if (!m_stringToId.TryGetValue(str, out value))
				{
					value = new MyStringId(m_stringToId.Count);
					m_idToString.Add(value, str);
					m_stringToId.Add(str, value);
				}
				return value;
			}
		}

		public static MyStringId Get(string str)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_stringToId[str];
			}
		}

		public static bool TryGet(string str, out MyStringId id)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_stringToId.TryGetValue(str, out id);
			}
		}

		public static MyStringId TryGet(string str)
		{
			using (m_lock.AcquireSharedUsing())
			{
				m_stringToId.TryGetValue(str, out MyStringId value);
				return value;
			}
		}

		public static bool IsKnown(MyStringId id)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_idToString.ContainsKey(id);
			}
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			m_id = GetOrCompute(reader.ReadInnerXml()).Id;
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteString(String);
		}
	}
}
