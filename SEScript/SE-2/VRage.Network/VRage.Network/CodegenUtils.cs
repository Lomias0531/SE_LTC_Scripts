using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VRage.Network
{
	public static class CodegenUtils
	{
		public const string EventAttributeName = "VRage.Network.EventAttribute";

		public const string GenerateFieldAccessorsAttributeName = "VRage.Network.GenerateFieldAccessorsAttribute";

		public const string GenerateActivatorAttributeName = "VRage.Network.GenerateActivatorAttribute";

		public const string IMyEventOwnerInterface = "VRage.Network.IMyEventOwner";

		public const string ICallSiteInterface = "VRage.Network.ICallSite";

		public const string IMemberAccessorInterface = "VRage.Network.IMemberAccessor";

		public const string ISyncComposerInterface = "VRage.Network.ISyncComposer";

		public const string IActivatorInterface = "VRage.Network.IActivator";

		public const string SyncTypeInterface = "VRage.Network.ISyncType";

		public const string ISerializerInfoInterface = "VRage.Network.ISerializerInfo";

		public const string VRageNetworkAssemblyName = "VRage.Network";

		public const string VRageAssemblyName = "VRage";

		public static string GetSafeName(Type type)
		{
			if (type.IsGenericType && !type.IsGenericTypeDefinition)
			{
				IEnumerable<string> values = type.GenericTypeArguments.Select((Type x) => GetSafeName(x));
				return type.Namespace?.Replace('.', '_') + "_" + type.Name + "<" + string.Join("#", values) + ">";
			}
			if (type.IsArray)
			{
				return GetSafeName(type.GetElementType()) + "<" + new string('#', type.GetArrayRank()) + ">";
			}
			if (type.IsByRef)
			{
				return GetSafeName(type.GetElementType()) + "#<";
			}
			if (type.IsNested)
			{
				return GetSafeName(type.DeclaringType) + "<>" + type.Name;
			}
			return type.Namespace?.Replace('.', '_') + "_" + type.Name;
		}

		public static string MakeCallSiteName(MethodInfo method)
		{
			return MakeCallSiteName(method.Name, from x in method.GetParameters()
				select GetSafeName(x.ParameterType));
		}

		private static string MakeCallSiteName(string methodName, IEnumerable<string> safeParameterTypes)
		{
			return methodName + "<>" + string.Join("#", safeParameterTypes);
		}

		private static string MakeMemberAccessorName(string safeTypeName, string memberName)
		{
			return safeTypeName + "<>" + memberName + "<>Accessor";
		}

		public static string MakeMemberAccessorName(Type reflectedType, MemberInfo member)
		{
			if (reflectedType.IsGenericType && !reflectedType.IsGenericTypeDefinition)
			{
				reflectedType = reflectedType.GetGenericTypeDefinition();
			}
			return MakeMemberAccessorName(GetSafeName(reflectedType), member.Name);
		}

		private static string MakeSyncComposerName(string memberName)
		{
			return memberName + "<>SyncComposer";
		}

		public static string MakeSyncComposerName(MemberInfo member)
		{
			return MakeSyncComposerName(member.Name);
		}

		private static string MakeActivatorName(string typeName)
		{
			return typeName + "<>Actor";
		}

		public static string MakeActivatorName(Type declaringType)
		{
			return MakeActivatorName(GetSafeName(declaringType));
		}

		public static ICallSite GetCallSite(MethodInfo method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			string callSiteName = MakeCallSiteName(method);
			TypeInfo typeInfo = method.DeclaringType.GetTypeInfo().DeclaredNestedTypes.FirstOrDefault((TypeInfo x) => x.Name == callSiteName);
			if (typeInfo == null)
			{
				return null;
			}
			return (ICallSite)Activator.CreateInstance(typeInfo);
		}

		public static IMemberAccessor GetMemberAccessor(Type parentType, MemberInfo member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			string accessorName = MakeMemberAccessorName(parentType, member);
			TypeInfo typeInfo = parentType.GetTypeInfo().DeclaredNestedTypes.FirstOrDefault((TypeInfo x) => x.Name == accessorName);
			if (typeInfo == null)
			{
				return null;
			}
			if (typeInfo.IsGenericTypeDefinition)
			{
				typeInfo = typeInfo.MakeGenericType(member.DeclaringType.GetGenericArguments()).GetTypeInfo();
			}
			return (IMemberAccessor)Activator.CreateInstance(typeInfo);
		}

		public static IMemberAccessor GetFieldAccessor(Type parentType, FieldInfo field)
		{
			return GetMemberAccessor(parentType, field);
		}

		public static IMemberAccessor GetPropertyAccessor(Type parentType, PropertyInfo property)
		{
			return GetMemberAccessor(parentType, property);
		}

		public static IMemberAccessor GetFieldAccessor(FieldInfo field)
		{
			return GetFieldAccessor(field.DeclaringType, field);
		}

		public static ISyncComposer GetSyncComposer(FieldInfo field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			string accessorName = MakeSyncComposerName(field);
			TypeInfo typeInfo = field.DeclaringType.GetTypeInfo().DeclaredNestedTypes.FirstOrDefault((TypeInfo x) => x.Name == accessorName);
			if (typeInfo == null)
			{
				return null;
			}
			if (typeInfo.IsGenericTypeDefinition)
			{
				typeInfo = typeInfo.MakeGenericType(field.DeclaringType.GetGenericArguments()).GetTypeInfo();
			}
			return (ISyncComposer)Activator.CreateInstance(typeInfo);
		}

		public static IActivator GetActivator(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			string name = (!type.IsConstructedGenericType) ? MakeActivatorName(type) : MakeActivatorName(type.GetGenericTypeDefinition());
			Type type2 = type.GetNestedType(name, BindingFlags.NonPublic);
			if (type2 == null)
			{
				return null;
			}
			if (type2.IsGenericTypeDefinition)
			{
				type2 = type2.MakeGenericType(type.GetGenericArguments()).GetTypeInfo();
			}
			return (IActivator)Activator.CreateInstance(type2);
		}

		public static IActivator<T> GetActivator<T>()
		{
			return (IActivator<T>)GetActivator(typeof(T));
		}
	}
}
