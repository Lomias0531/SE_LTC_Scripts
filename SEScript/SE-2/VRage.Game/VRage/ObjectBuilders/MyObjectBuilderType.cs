using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Reflection;

namespace VRage.ObjectBuilders
{
	public struct MyObjectBuilderType
	{
		public class ComparerType : IEqualityComparer<MyObjectBuilderType>
		{
			public bool Equals(MyObjectBuilderType x, MyObjectBuilderType y)
			{
				return x == y;
			}

			public int GetHashCode(MyObjectBuilderType obj)
			{
				return obj.GetHashCode();
			}
		}

		private const string LEGACY_TYPE_PREFIX = "MyObjectBuilder_";

		public static readonly MyObjectBuilderType Invalid;

		private readonly Type m_type;

		public static readonly ComparerType Comparer;

		private static Dictionary<string, MyObjectBuilderType> m_typeByName;

		private static Dictionary<string, MyObjectBuilderType> m_typeByLegacyName;

		private static Dictionary<MyRuntimeObjectBuilderId, MyObjectBuilderType> m_typeById;

		private static Dictionary<MyObjectBuilderType, MyRuntimeObjectBuilderId> m_idByType;

		private static ushort m_idCounter;

		private const int EXPECTED_TYPE_COUNT = 500;

		public bool IsNull => m_type == null;

		public MyObjectBuilderType(Type type)
		{
			m_type = type;
		}

		public static implicit operator MyObjectBuilderType(Type t)
		{
			return new MyObjectBuilderType(t);
		}

		public static implicit operator Type(MyObjectBuilderType t)
		{
			return t.m_type;
		}

		public static explicit operator MyRuntimeObjectBuilderId(MyObjectBuilderType t)
		{
			if (!m_idByType.TryGetValue(t, out MyRuntimeObjectBuilderId value))
			{
				value = default(MyRuntimeObjectBuilderId);
			}
			return value;
		}

		public static explicit operator MyObjectBuilderType(MyRuntimeObjectBuilderId id)
		{
			return m_typeById[id];
		}

		public static bool operator ==(MyObjectBuilderType lhs, MyObjectBuilderType rhs)
		{
			return lhs.m_type == rhs.m_type;
		}

		public static bool operator !=(MyObjectBuilderType lhs, MyObjectBuilderType rhs)
		{
			return lhs.m_type != rhs.m_type;
		}

		public static bool operator ==(Type lhs, MyObjectBuilderType rhs)
		{
			return lhs == rhs.m_type;
		}

		public static bool operator !=(Type lhs, MyObjectBuilderType rhs)
		{
			return lhs != rhs.m_type;
		}

		public static bool operator ==(MyObjectBuilderType lhs, Type rhs)
		{
			return lhs.m_type == rhs;
		}

		public static bool operator !=(MyObjectBuilderType lhs, Type rhs)
		{
			return lhs.m_type != rhs;
		}

		public override bool Equals(object obj)
		{
			if (obj != null && obj is MyObjectBuilderType)
			{
				return Equals((MyObjectBuilderType)obj);
			}
			return false;
		}

		public bool Equals(MyObjectBuilderType type)
		{
			return type.m_type == m_type;
		}

		public override int GetHashCode()
		{
			if (!(m_type != null))
			{
				return 0;
			}
			return m_type.GetHashCode();
		}

		public override string ToString()
		{
			if (!(m_type != null))
			{
				return null;
			}
			return m_type.Name;
		}

		public static MyObjectBuilderType Parse(string value)
		{
			return m_typeByName[value];
		}

		/// <summary>
		/// Can handle old values as well.
		/// </summary>
		public static MyObjectBuilderType ParseBackwardsCompatible(string value)
		{
			if (m_typeByName.TryGetValue(value, out MyObjectBuilderType value2))
			{
				return value2;
			}
			if (m_typeByLegacyName.TryGetValue(value, out value2))
			{
				return value2;
			}
			return Invalid;
		}

		public static bool IsValidTypeName(string value)
		{
			if (value == null)
			{
				return false;
			}
			if (!m_typeByName.ContainsKey(value))
			{
				return m_typeByLegacyName.ContainsKey(value);
			}
			return true;
		}

		public static bool TryParse(string value, out MyObjectBuilderType result)
		{
			if (value == null)
			{
				result = Invalid;
				return false;
			}
			return m_typeByName.TryGetValue(value, out result);
		}

		static MyObjectBuilderType()
		{
			Invalid = new MyObjectBuilderType(null);
			Comparer = new ComparerType();
			m_typeByName = new Dictionary<string, MyObjectBuilderType>(500);
			m_typeByLegacyName = new Dictionary<string, MyObjectBuilderType>(500);
			m_typeById = new Dictionary<MyRuntimeObjectBuilderId, MyObjectBuilderType>(500, MyRuntimeObjectBuilderId.Comparer);
			m_idByType = new Dictionary<MyObjectBuilderType, MyRuntimeObjectBuilderId>(500, Comparer);
		}

		public static bool IsReady()
		{
			return m_typeByName.Count > 0;
		}

		public static void RegisterFromAssembly(Assembly assembly, bool registerLegacyNames = false)
		{
			if (assembly == null)
			{
				return;
			}
			Type typeFromHandle = typeof(MyObjectBuilder_Base);
			Type[] types = assembly.GetTypes();
			Array.Sort(types, FullyQualifiedNameComparer.Default);
			Type[] array = types;
			foreach (Type type in array)
			{
				if (!typeFromHandle.IsAssignableFrom(type) || m_typeByName.ContainsKey(type.Name))
				{
					continue;
				}
				MyObjectBuilderType myObjectBuilderType = new MyObjectBuilderType(type);
				MyRuntimeObjectBuilderId myRuntimeObjectBuilderId = new MyRuntimeObjectBuilderId(++m_idCounter);
				m_typeById.Add(myRuntimeObjectBuilderId, myObjectBuilderType);
				m_idByType.Add(myObjectBuilderType, myRuntimeObjectBuilderId);
				m_typeByName.Add(type.Name, myObjectBuilderType);
				if (registerLegacyNames && type.Name.StartsWith("MyObjectBuilder_"))
				{
					RegisterLegacyName(myObjectBuilderType, type.Name.Substring("MyObjectBuilder_".Length));
				}
				object[] customAttributes = type.GetCustomAttributes(typeof(MyObjectBuilderDefinitionAttribute), inherit: true);
				if (customAttributes.Length != 0)
				{
					MyObjectBuilderDefinitionAttribute myObjectBuilderDefinitionAttribute = (MyObjectBuilderDefinitionAttribute)customAttributes[0];
					if (!string.IsNullOrEmpty(myObjectBuilderDefinitionAttribute.LegacyName))
					{
						RegisterLegacyName(myObjectBuilderType, myObjectBuilderDefinitionAttribute.LegacyName);
					}
				}
			}
		}

		internal static void RegisterLegacyName(MyObjectBuilderType type, string legacyName)
		{
			m_typeByLegacyName.Add(legacyName, type);
		}

		/// <summary>
		/// Used for type remapping when overriding definition types
		/// </summary>
		internal static void RemapType(ref SerializableDefinitionId id, Dictionary<string, string> typeOverrideMap)
		{
			string value;
			bool flag = typeOverrideMap.TryGetValue(id.TypeIdString, out value);
			if (!flag && id.TypeIdString.StartsWith("MyObjectBuilder_"))
			{
				flag = typeOverrideMap.TryGetValue(id.TypeIdString.Substring("MyObjectBuilder_".Length), out value);
			}
			if (flag)
			{
				id.TypeIdString = value;
			}
		}

		public static void UnregisterAssemblies()
		{
			if (m_typeByLegacyName != null)
			{
				m_typeByLegacyName.Clear();
			}
			if (m_typeById != null)
			{
				m_typeById.Clear();
			}
			if (m_idByType != null)
			{
				m_idByType.Clear();
			}
			if (m_typeByName != null)
			{
				m_typeByName.Clear();
			}
			m_idCounter = 0;
		}
	}
}
