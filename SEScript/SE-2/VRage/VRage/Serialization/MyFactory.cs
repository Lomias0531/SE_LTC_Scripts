using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Collections;
using VRage.Network;
using VRage.ObjectBuilder;

namespace VRage.Serialization
{
	public static class MyFactory
	{
		private static ThreadSafeStore<Type, MySerializer> m_serializers;

		private static Dictionary<Type, Type> m_serializerTypes;

		static MyFactory()
		{
			m_serializers = new ThreadSafeStore<Type, MySerializer>(CreateSerializerInternal);
			m_serializerTypes = new Dictionary<Type, Type>();
			RegisterFromAssembly(Assembly.GetExecutingAssembly());
		}

		public static MySerializer<T> GetSerializer<T>()
		{
			return (MySerializer<T>)GetSerializer(typeof(T));
		}

		public static MySerializer GetSerializer(Type t)
		{
			return m_serializers.Get(t);
		}

		public static MySerializeInfo CreateInfo(MemberInfo member)
		{
			return MySerializeInfo.Create(member);
		}

		public static MyMemberSerializer<TOwner> CreateMemberSerializer<TOwner>(MemberInfo member)
		{
			return (MyMemberSerializer<TOwner>)CreateMemberSerializer(member, typeof(TOwner));
		}

		public static MyMemberSerializer CreateMemberSerializer(MemberInfo member, Type ownerType)
		{
			MyMemberSerializer obj = (MyMemberSerializer)Activator.CreateInstance(typeof(MyMemberSerializer<, >).MakeGenericType(ownerType, member.GetMemberType()));
			obj.Init(member, CreateInfo(member));
			return obj;
		}

		private static MySerializer CreateSerializerInternal(Type t)
		{
			Type value;
			lock (m_serializerTypes)
			{
				m_serializerTypes.TryGetValue(t, out value);
			}
			if (value != null)
			{
				return (MySerializer)Activator.CreateInstance(value);
			}
			if (t.IsEnum)
			{
				return (MySerializer)Activator.CreateInstance(typeof(MySerializerEnum<>).MakeGenericType(t));
			}
			if (t.IsArray)
			{
				return (MySerializer)Activator.CreateInstance(typeof(MySerializerArray<>).MakeGenericType(t.GetElementType()));
			}
			if (typeof(IMyNetObject).IsAssignableFrom(t))
			{
				return (MySerializer)Activator.CreateInstance(typeof(MySerializerNetObject<>).MakeGenericType(t));
			}
			if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return (MySerializer)Activator.CreateInstance(typeof(MySerializerNullable<>).MakeGenericType(t.GetGenericArguments()[0]));
			}
			if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(MySerializableList<>))
			{
				return (MySerializer)Activator.CreateInstance(typeof(MySerializerObList<>).MakeGenericType(t.GetGenericArguments()[0]));
			}
			if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
			{
				return (MySerializer)Activator.CreateInstance(typeof(MySerializerList<>).MakeGenericType(t.GetGenericArguments()[0]));
			}
			if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(HashSet<>))
			{
				return (MySerializer)Activator.CreateInstance(typeof(MySerializerHashSet<>).MakeGenericType(t.GetGenericArguments()[0]));
			}
			if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<, >))
			{
				Type[] genericArguments = t.GetGenericArguments();
				return (MySerializer)Activator.CreateInstance(typeof(MySerializerDictionary<, >).MakeGenericType(genericArguments[0], genericArguments[1]));
			}
			if (t.IsClass || t.IsStruct())
			{
				return (MySerializer)Activator.CreateInstance(typeof(MySerializerObject<>).MakeGenericType(t));
			}
			throw new InvalidOperationException("No serializer found for type: " + t.Name);
		}

		public static void Register(Type serializedType, Type serializer)
		{
			lock (m_serializerTypes)
			{
				m_serializerTypes.Add(serializedType, serializer);
			}
		}

		public static void RegisterFromAssembly(Assembly assembly)
		{
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (!type.IsGenericType && type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(MySerializer<>))
				{
					Register(type.BaseType.GetGenericArguments()[0], type);
				}
			}
		}
	}
}
