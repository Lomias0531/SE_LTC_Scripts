using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using VRage.Utils;

namespace VRage
{
	public class MyXmlSerializerManager
	{
		private static readonly HashSet<Type> m_serializableBaseTypes = new HashSet<Type>();

		private static readonly Dictionary<Type, XmlSerializer> m_serializersByType = new Dictionary<Type, XmlSerializer>();

		private static readonly Dictionary<string, XmlSerializer> m_serializersBySerializedName = new Dictionary<string, XmlSerializer>();

		private static readonly Dictionary<Type, string> m_serializedNameByType = new Dictionary<Type, string>();

		private static HashSet<Assembly> m_registeredAssemblies = new HashSet<Assembly>();

		public static void RegisterSerializer(Type type)
		{
			if (!m_serializersByType.ContainsKey(type))
			{
				RegisterType(type, forceRegister: true, checkAttributes: false);
			}
		}

		public static void RegisterSerializableBaseType(Type type)
		{
			m_serializableBaseTypes.Add(type);
		}

		public static void RegisterFromAssembly(Assembly assembly)
		{
			if (!(assembly == null) && !m_registeredAssemblies.Contains(assembly))
			{
				m_registeredAssemblies.Add(assembly);
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					try
					{
						if (!m_serializersByType.ContainsKey(type))
						{
							RegisterType(type);
						}
					}
					catch (Exception innerException)
					{
						throw new InvalidOperationException("Error creating XML serializer for type " + type.Name, innerException);
					}
				}
			}
		}

		public static XmlSerializer GetSerializer(Type type)
		{
			return m_serializersByType[type];
		}

		public static XmlSerializer GetOrCreateSerializer(Type type)
		{
			if (!m_serializersByType.TryGetValue(type, out XmlSerializer value))
			{
				return RegisterType(type, forceRegister: true);
			}
			return value;
		}

		public static string GetSerializedName(Type type)
		{
			return m_serializedNameByType[type];
		}

		public static bool TryGetSerializer(string serializedName, out XmlSerializer serializer)
		{
			return m_serializersBySerializedName.TryGetValue(serializedName, out serializer);
		}

		public static XmlSerializer GetSerializer(string serializedName)
		{
			return m_serializersBySerializedName[serializedName];
		}

		public static bool IsSerializerAvailable(string name)
		{
			return m_serializersBySerializedName.ContainsKey(name);
		}

		/// <param name="forceRegister">Force registration for types without XmlType
		/// attribute or not object builders</param>
		private static XmlSerializer RegisterType(Type type, bool forceRegister = false, bool checkAttributes = true)
		{
			string text = null;
			if (checkAttributes)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(XmlTypeAttribute), inherit: false);
				if (customAttributes.Length != 0)
				{
					XmlTypeAttribute xmlTypeAttribute = (XmlTypeAttribute)customAttributes[0];
					text = type.Name;
					if (!string.IsNullOrEmpty(xmlTypeAttribute.TypeName))
					{
						text = xmlTypeAttribute.TypeName;
					}
				}
				else
				{
					foreach (Type serializableBaseType in m_serializableBaseTypes)
					{
						if (serializableBaseType.IsAssignableFrom(type))
						{
							text = type.Name;
							break;
						}
					}
				}
			}
			if (text == null)
			{
				if (!forceRegister)
				{
					return null;
				}
				text = type.Name;
			}
			XmlSerializer xmlSerializer = null;
			foreach (Attribute item in EnumerateThisAndParentAttributes(type))
			{
				Type type2 = item.GetType();
				if (type2.Name == "XmlSerializerAssemblyAttribute")
				{
					xmlSerializer = TryLoadSerializerFrom((string)type2.GetProperty("AssemblyName").GetValue(item), type.Name);
					if (xmlSerializer != null)
					{
						break;
					}
				}
			}
			if (xmlSerializer == null)
			{
				string text2 = type.Assembly.GetName().Name + ".XmlSerializers";
				MyLog.Default.Error("Type {0} is missing missing XmlSerializerAssemblyAttribute. Falling back to default {1}", type.Name, text2);
				xmlSerializer = TryLoadSerializerFrom(text2, type.Name);
			}
			if (xmlSerializer == null)
			{
				xmlSerializer = new XmlSerializer(type);
			}
			m_serializersByType.Add(type, xmlSerializer);
			m_serializersBySerializedName.Add(text, xmlSerializer);
			m_serializedNameByType.Add(type, text);
			return xmlSerializer;
		}

		private static XmlSerializer TryLoadSerializerFrom(string assemblyName, string typeName)
		{
			Assembly assembly = null;
			try
			{
				assembly = Assembly.Load(new AssemblyName(assemblyName));
			}
			catch
			{
			}
			if (assembly == null)
			{
				try
				{
					assembly = Assembly.Load(assemblyName);
				}
				catch
				{
				}
			}
			if (assembly == null)
			{
				return null;
			}
			Type type = assembly.GetType("Microsoft.Xml.Serialization.GeneratedAssembly." + typeName + "Serializer");
			if (type != null)
			{
				return (XmlSerializer)Activator.CreateInstance(type);
			}
			return null;
		}

		private static IEnumerable<Attribute> EnumerateThisAndParentAttributes(Type type)
		{
			if (!(type == null))
			{
				foreach (Attribute customAttribute in type.GetCustomAttributes())
				{
					yield return customAttribute;
				}
				foreach (Attribute item in EnumerateThisAndParentAttributes(type.DeclaringType))
				{
					yield return item;
				}
			}
		}
	}
}
