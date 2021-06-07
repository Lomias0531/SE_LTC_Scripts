using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using VRage.Collections;

namespace VRage
{
	public static class TypeExtensions
	{
		/// <summary>
		/// CLR core types that you should account for.
		/// </summary>
		public static readonly HashSetReader<Type> CoreTypes = new HashSet<Type>(new Type[22]
		{
			typeof(object),
			typeof(string),
			typeof(int),
			typeof(short),
			typeof(long),
			typeof(uint),
			typeof(ushort),
			typeof(ulong),
			typeof(double),
			typeof(float),
			typeof(bool),
			typeof(char),
			typeof(byte),
			typeof(sbyte),
			typeof(decimal),
			typeof(Enum),
			typeof(ValueType),
			typeof(Delegate),
			typeof(MulticastDelegate),
			typeof(Type),
			typeof(Attribute),
			typeof(Exception)
		});

		public static bool IsStruct(this Type type)
		{
			if (type.IsValueType && !type.IsPrimitive && !type.IsEnum)
			{
				return type != typeof(decimal);
			}
			return false;
		}

		/// <summary>
		/// Check that the type is public to an unrelated scope, this will also check the declaring type hierarchy if needed..
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsAccessible(this Type type)
		{
			do
			{
				if (type.IsPublic)
				{
					return true;
				}
				if ((type.Attributes & TypeAttributes.NestedPublic) == 0)
				{
					return false;
				}
				type = type.DeclaringType;
			}
			while (type != null);
			return false;
		}

		public static IEnumerable<MemberInfo> GetDataMembers(this Type t, bool fields, bool properties, bool nonPublic, bool inherited, bool _static, bool instance, bool read, bool write)
		{
			BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public;
			if (nonPublic)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			if (_static)
			{
				bindingFlags |= BindingFlags.Static;
			}
			if (instance)
			{
				bindingFlags |= BindingFlags.Instance;
			}
			IEnumerable<MemberInfo> enumerable = t.GetMembers(bindingFlags);
			if (inherited && t.IsClass && t != typeof(object))
			{
				Type baseType = t.BaseType;
				while (baseType != typeof(object) && baseType != null)
				{
					enumerable = enumerable.Concat(baseType.GetMembers(bindingFlags));
					baseType = baseType.BaseType;
				}
			}
			SortedDictionary<string, MemberInfo> sortedDictionary = new SortedDictionary<string, MemberInfo>();
			foreach (MemberInfo item in enumerable)
			{
				if ((fields && item.MemberType == MemberTypes.Field) || (properties && CheckProperty(item, read, write)))
				{
					sortedDictionary.Add(item.DeclaringType.Name + item.Name, item);
				}
			}
			return sortedDictionary.Values;
		}

		private static bool CheckProperty(MemberInfo info, bool read, bool write)
		{
			PropertyInfo propertyInfo = info as PropertyInfo;
			if (propertyInfo != null && (!read || propertyInfo.CanRead))
			{
				if (write)
				{
					return propertyInfo.CanWrite;
				}
				return true;
			}
			return false;
		}

		public static Type FindGenericBaseTypeArgument(this Type type, Type genericTypeDefinition)
		{
			Type[] array = type.FindGenericBaseTypeArguments(genericTypeDefinition);
			if (array.Length == 0)
			{
				return null;
			}
			return array[0];
		}

		public static Type[] FindGenericBaseTypeArguments(this Type type, Type genericTypeDefinition)
		{
			if (type.IsValueType || type.IsInterface)
			{
				return Type.EmptyTypes;
			}
			while (type != typeof(object))
			{
				if (type.IsGenericType)
				{
					Type genericTypeDefinition2 = type.GetGenericTypeDefinition();
					if (genericTypeDefinition2 == genericTypeDefinition)
					{
						return type.GetGenericArguments();
					}
				}
				type = type.BaseType;
			}
			return Type.EmptyTypes;
		}

		public static bool IsInstanceOfGenericType(this Type subtype, Type genericType)
		{
			Type type = subtype;
			while (type != null)
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
				{
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}

		public static bool ImplementsGenericInterface(this Type subtype, Type genericInterface)
		{
			return subtype.GetInterfaces().Any((Type x) => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterface);
		}

		public static bool HasDefaultConstructor(this Type type)
		{
			if (!type.IsAbstract)
			{
				return type.GetConstructor(Type.EmptyTypes) != null;
			}
			return false;
		}

		/// <summary>
		/// Get full type name with full namespace names
		/// </summary>
		/// <param name="type">Type. May be generic or nullable</param>
		/// <returns>Full type name, fully qualified namespaces</returns>
		public static string PrettyName(this Type type)
		{
			Type underlyingType = Nullable.GetUnderlyingType(type);
			if (underlyingType != null)
			{
				return underlyingType.PrettyName() + "?";
			}
			if (type.IsByRef)
			{
				return $"{type.GetElementType().PrettyName()}&";
			}
			if (type.IsArray)
			{
				return $"{type.GetElementType().PrettyName()}[{new string(',', type.GetArrayRank() - 1)}]";
			}
			if (!type.IsGenericType)
			{
				switch (type.Name)
				{
				case "String":
					return "string";
				case "Byte":
					return "byte";
				case "SByte":
					return "sbyte";
				case "Int16":
					return "short";
				case "Int32":
					return "int";
				case "Int64":
					return "long";
				case "UInt16":
					return "ushort";
				case "UInt32":
					return "uint";
				case "UInt64":
					return "ulong";
				case "Decimal":
					return "decimal";
				case "Object":
					return "object";
				case "Void":
					return "void";
				default:
					if (!string.IsNullOrWhiteSpace(type.FullName))
					{
						return type.FullName;
					}
					return type.Name;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = type.Name.IndexOf('`');
			if (num >= 0)
			{
				stringBuilder.Append(type.Name.Substring(0, num));
			}
			else
			{
				stringBuilder.Append(type.Name);
			}
			stringBuilder.Append('<');
			bool flag = true;
			Type[] genericArguments = type.GetGenericArguments();
			foreach (Type type2 in genericArguments)
			{
				if (!flag)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(type2.PrettyName());
				flag = false;
			}
			stringBuilder.Append('>');
			return stringBuilder.ToString();
		}

		public static int SizeOf<T>()
		{
			return Unsafe.SizeOf<T>();
		}
	}
}
