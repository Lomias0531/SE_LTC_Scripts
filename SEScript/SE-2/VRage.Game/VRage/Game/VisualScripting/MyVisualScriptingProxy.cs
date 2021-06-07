using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using VRage.Game.Entity;
using VRageMath;

namespace VRage.Game.VisualScripting
{
	public static class MyVisualScriptingProxy
	{
		private static readonly Dictionary<string, MethodInfo> m_visualScriptingMethodsBySignature = new Dictionary<string, MethodInfo>();

		private static readonly Dictionary<Type, HashSet<MethodInfo>> m_whitelistedMethods = new Dictionary<Type, HashSet<MethodInfo>>();

		private static readonly Dictionary<MethodInfo, bool> m_whitelistedMethodsSequenceDependency = new Dictionary<MethodInfo, bool>();

		private static readonly Dictionary<string, FieldInfo> m_visualScriptingEventFields = new Dictionary<string, FieldInfo>();

		private static readonly Dictionary<string, Type> m_registeredTypes = new Dictionary<string, Type>();

		private static readonly List<Type> m_supportedTypes = new List<Type>();

		private static bool m_initialized = false;

		public static IEnumerable<FieldInfo> EventFields => m_visualScriptingEventFields.Values;

		public static List<Type> SupportedTypes => m_supportedTypes;

		/// <summary>
		/// Loads reflection data.
		/// </summary>
		public static void Init()
		{
			if (!m_initialized)
			{
				m_supportedTypes.Add(typeof(int));
				m_supportedTypes.Add(typeof(float));
				m_supportedTypes.Add(typeof(string));
				m_supportedTypes.Add(typeof(Vector3D));
				m_supportedTypes.Add(typeof(bool));
				m_supportedTypes.Add(typeof(long));
				m_supportedTypes.Add(typeof(List<bool>));
				m_supportedTypes.Add(typeof(List<int>));
				m_supportedTypes.Add(typeof(List<float>));
				m_supportedTypes.Add(typeof(List<string>));
				m_supportedTypes.Add(typeof(List<long>));
				m_supportedTypes.Add(typeof(List<Vector3D>));
				m_supportedTypes.Add(typeof(List<MyEntity>));
				m_supportedTypes.Add(typeof(MyEntity));
				MyVisualScriptLogicProvider.Init();
				m_initialized = true;
			}
		}

		private static void RegisterMethod(Type declaringType, MethodInfo method, VisualScriptingMember attribute, bool? overrideSequenceDependency = null)
		{
			if (declaringType.IsGenericType)
			{
				declaringType = declaringType.GetGenericTypeDefinition();
			}
			if (!m_whitelistedMethods.ContainsKey(declaringType))
			{
				m_whitelistedMethods[declaringType] = new HashSet<MethodInfo>();
			}
			m_whitelistedMethods[declaringType].Add(method);
			m_whitelistedMethodsSequenceDependency[method] = (overrideSequenceDependency ?? attribute.Sequential);
			foreach (KeyValuePair<Type, HashSet<MethodInfo>> whitelistedMethod in m_whitelistedMethods)
			{
				if (whitelistedMethod.Key.IsAssignableFrom(declaringType))
				{
					whitelistedMethod.Value.Add(method);
				}
				else if (declaringType.IsAssignableFrom(whitelistedMethod.Key))
				{
					HashSet<MethodInfo> hashSet = m_whitelistedMethods[declaringType];
					foreach (MethodInfo item in whitelistedMethod.Value)
					{
						hashSet.Add(item);
					}
				}
			}
		}

		public static void RegisterType(Type type)
		{
			string key = type.Signature();
			if (!m_registeredTypes.ContainsKey(key))
			{
				m_registeredTypes.Add(key, type);
			}
		}

		public static void WhitelistExtensions(Type type)
		{
			MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
			foreach (MethodInfo methodInfo in methods)
			{
				VisualScriptingMember customAttribute = methodInfo.GetCustomAttribute<VisualScriptingMember>();
				if (customAttribute != null && methodInfo.IsDefined(typeof(ExtensionAttribute), inherit: false))
				{
					RegisterMethod(methodInfo.GetParameters()[0].ParameterType, methodInfo, customAttribute);
				}
			}
			m_registeredTypes[type.Signature()] = type;
		}

		public static void WhitelistMethod(MethodInfo method, bool sequenceDependent)
		{
			Type declaringType = method.DeclaringType;
			if (!(declaringType == null))
			{
				RegisterMethod(declaringType, method, null, sequenceDependent);
			}
		}

		public static IEnumerable<MethodInfo> GetWhitelistedMethods(Type type)
		{
			if (type == null)
			{
				return null;
			}
			if (m_whitelistedMethods.TryGetValue(type, out HashSet<MethodInfo> value))
			{
				return value;
			}
			if (type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				Type[] genericArguments = type.GetGenericArguments();
				if (m_whitelistedMethods.TryGetValue(genericTypeDefinition, out value))
				{
					HashSet<MethodInfo> hashSet = new HashSet<MethodInfo>();
					m_whitelistedMethods[type] = hashSet;
					{
						foreach (MethodInfo item in value)
						{
							MethodInfo methodInfo = null;
							methodInfo = ((!item.IsDefined(typeof(ExtensionAttribute))) ? type.GetMethod(item.Name) : item.MakeGenericMethod(genericArguments));
							hashSet.Add(methodInfo);
							bool value2 = m_whitelistedMethodsSequenceDependency[item];
							m_whitelistedMethodsSequenceDependency[methodInfo] = value2;
							m_visualScriptingMethodsBySignature[methodInfo.Signature()] = methodInfo;
						}
						return hashSet;
					}
				}
			}
			return null;
		}

		public static void RegisterLogicProvider(Type type)
		{
			MethodInfo[] methods = type.GetMethods();
			foreach (MethodInfo methodInfo in methods)
			{
				if (methodInfo.GetCustomAttribute<VisualScriptingMember>() != null)
				{
					string key = methodInfo.Signature();
					if (!m_visualScriptingMethodsBySignature.ContainsKey(key))
					{
						m_visualScriptingMethodsBySignature.Add(key, methodInfo);
					}
				}
			}
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.FieldType.GetCustomAttribute<VisualScriptingEvent>() != null && fieldInfo.FieldType.IsSubclassOf(typeof(MulticastDelegate)) && !m_visualScriptingEventFields.ContainsKey(fieldInfo.Signature()))
				{
					m_visualScriptingEventFields.Add(fieldInfo.Signature(), fieldInfo);
				}
			}
		}

		/// <summary>
		/// Looks for given type using executing assembly.
		/// </summary>
		/// <param name="typeFullName"></param>
		/// <returns></returns>
		public static Type GetType(string typeFullName)
		{
			if (typeFullName == null || typeFullName.Length == 0)
			{
				Debugger.Break();
			}
			if (m_registeredTypes.TryGetValue(typeFullName, out Type value))
			{
				return value;
			}
			value = Type.GetType(typeFullName);
			if (value != null)
			{
				return value;
			}
			return typeof(Vector3D).Assembly.GetType(typeFullName);
		}

		/// <summary>
		/// Looks for methodInfo about method with given signature.
		/// </summary>
		/// <param name="signature">Full signature of a method.</param>
		/// <returns>null if not found.</returns>
		public static MethodInfo GetMethod(string signature)
		{
			m_visualScriptingMethodsBySignature.TryGetValue(signature, out MethodInfo value);
			return value;
		}

		public static MethodInfo GetMethod(Type type, string signature)
		{
			if (!m_whitelistedMethods.ContainsKey(type))
			{
				GetWhitelistedMethods(type);
			}
			return GetMethod(signature);
		}

		/// <summary>
		/// All attributed methods from VisualScriptingProxy.
		/// </summary>
		/// <returns></returns>
		public static List<MethodInfo> GetMethods()
		{
			List<MethodInfo> list = new List<MethodInfo>();
			foreach (KeyValuePair<string, MethodInfo> item in m_visualScriptingMethodsBySignature)
			{
				list.Add(item.Value);
			}
			return list;
		}

		/// <summary>
		/// Returns event field with specified signature.
		/// </summary>
		/// <param name="signature"></param>
		/// <returns></returns>
		public static FieldInfo GetField(string signature)
		{
			m_visualScriptingEventFields.TryGetValue(signature, out FieldInfo value);
			return value;
		}

		public static string Signature(this FieldInfo info)
		{
			return info.DeclaringType.Namespace + "." + info.DeclaringType.Name + "." + info.Name;
		}

		public static bool TryToRecoverMethodInfo(ref string oldSignature, Type declaringType, Type extensionType, out MethodInfo info)
		{
			info = null;
			int i;
			for (i = 0; i < oldSignature.Length && i < declaringType.FullName.Length && oldSignature[i] == declaringType.FullName[i]; i++)
			{
			}
			oldSignature = oldSignature.Remove(0, i + 1);
			oldSignature = oldSignature.Remove(oldSignature.IndexOf('('));
			if (extensionType != null && extensionType.IsGenericType)
			{
				Type[] genericArguments = extensionType.GetGenericArguments();
				MethodInfo method = declaringType.GetMethod(oldSignature);
				if (method != null)
				{
					info = method.MakeGenericMethod(genericArguments);
				}
			}
			else
			{
				info = declaringType.GetMethod(oldSignature);
			}
			if (info != null)
			{
				oldSignature = info.Signature();
			}
			return info != null;
		}

		public static string Signature(this MethodInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder(info.DeclaringType.Signature());
			ParameterInfo[] parameters = info.GetParameters();
			stringBuilder.Append('.').Append(info.Name).Append('(');
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].ParameterType.IsGenericType)
				{
					stringBuilder.Append(parameters[i].ParameterType.Signature());
				}
				else
				{
					stringBuilder.Append(parameters[i].ParameterType.Name);
				}
				stringBuilder.Append(' ').Append(parameters[i].Name);
				if (i < parameters.Length - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			stringBuilder.Append(')');
			return stringBuilder.ToString();
		}

		public static string MethodGroup(this MethodInfo info)
		{
			return info.GetCustomAttribute<VisualScriptingMiscData>()?.Group;
		}

		public static string Signature(this Type type)
		{
			if (type.IsEnum)
			{
				return type.FullName.Replace("+", ".");
			}
			return type.FullName;
		}

		public static bool IsSequenceDependent(this MethodInfo method)
		{
			VisualScriptingMember customAttribute = method.GetCustomAttribute<VisualScriptingMember>();
			if (customAttribute == null && !method.IsStatic)
			{
				bool value = true;
				if (m_whitelistedMethodsSequenceDependency.TryGetValue(method, out value))
				{
					return value;
				}
				return true;
			}
			return customAttribute?.Sequential ?? true;
		}

		public static string ReadableName(this Type type)
		{
			if (type == null)
			{
				Debugger.Break();
				return null;
			}
			if (type == typeof(bool))
			{
				return "Bool";
			}
			if (type == typeof(int))
			{
				return "Int";
			}
			if (type == typeof(string))
			{
				return "String";
			}
			if (type == typeof(float))
			{
				return "Float";
			}
			if (type == typeof(long))
			{
				return "Long";
			}
			if (type.IsGenericType)
			{
				StringBuilder stringBuilder = new StringBuilder(type.Name.Remove(type.Name.IndexOf('`')));
				Type[] genericArguments = type.GetGenericArguments();
				stringBuilder.Append(" - ");
				Type[] array = genericArguments;
				foreach (Type type2 in array)
				{
					stringBuilder.Append(type2.ReadableName());
					stringBuilder.Append(",");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				return stringBuilder.ToString();
			}
			return type.Name;
		}
	}
}
