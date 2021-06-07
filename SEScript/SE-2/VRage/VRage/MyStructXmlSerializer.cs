using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace VRage
{
	/// <summary>
	/// Deserializes structs using a specified default value (see StructDefaultAttribute).
	/// </summary>
	public class MyStructXmlSerializer<TStruct> : MyXmlSerializerBase<TStruct> where TStruct : struct
	{
		/// <summary>
		/// Abstract accessor for both fields and properties
		/// </summary>
		private abstract class Accessor
		{
			public abstract Type Type
			{
				get;
			}

			public Type SerializerType
			{
				get;
				private set;
			}

			public bool IsPrimitiveType
			{
				get
				{
					Type type = Type;
					if (!type.IsPrimitive)
					{
						return type == typeof(string);
					}
					return true;
				}
			}

			public abstract object GetValue(object obj);

			public abstract void SetValue(object obj, object value);

			protected void CheckXmlElement(MemberInfo info)
			{
				XmlElementAttribute xmlElementAttribute = info.GetCustomAttribute(typeof(XmlElementAttribute), inherit: false) as XmlElementAttribute;
				if (xmlElementAttribute != null && xmlElementAttribute.Type != null && typeof(IMyXmlSerializable).IsAssignableFrom(xmlElementAttribute.Type))
				{
					SerializerType = xmlElementAttribute.Type;
				}
			}
		}

		private class FieldAccessor : Accessor
		{
			public FieldInfo Field
			{
				get;
				private set;
			}

			public override Type Type => Field.FieldType;

			public FieldAccessor(FieldInfo field)
			{
				Field = field;
				CheckXmlElement(field);
			}

			public override object GetValue(object obj)
			{
				return Field.GetValue(obj);
			}

			public override void SetValue(object obj, object value)
			{
				Field.SetValue(obj, value);
			}
		}

		private class PropertyAccessor : Accessor
		{
			public PropertyInfo Property
			{
				get;
				private set;
			}

			public override Type Type => Property.PropertyType;

			public PropertyAccessor(PropertyInfo property)
			{
				Property = property;
				CheckXmlElement(property);
			}

			public override object GetValue(object obj)
			{
				return Property.GetValue(obj);
			}

			public override void SetValue(object obj, object value)
			{
				Property.SetValue(obj, value);
			}
		}

		public static FieldInfo m_defaultValueField;

		private static Dictionary<string, Accessor> m_accessorMap;

		public MyStructXmlSerializer()
		{
		}

		public MyStructXmlSerializer(ref TStruct data)
		{
			m_data = data;
		}

		public override void ReadXml(XmlReader reader)
		{
			BuildAccessorsInfo();
			object obj = (TStruct)m_defaultValueField.GetValue(null);
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				reader.Skip();
				return;
			}
			reader.ReadStartElement();
			reader.MoveToContent();
			while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != 0)
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (m_accessorMap.TryGetValue(reader.LocalName, out Accessor value))
					{
						object value3;
						if (value.IsPrimitiveType)
						{
							string value2 = reader.ReadElementString();
							value3 = TypeDescriptor.GetConverter(value.Type).ConvertFrom(null, CultureInfo.InvariantCulture, value2);
						}
						else if (value.SerializerType != null)
						{
							IMyXmlSerializable obj2 = Activator.CreateInstance(value.SerializerType) as IMyXmlSerializable;
							obj2.ReadXml(reader.ReadSubtree());
							value3 = obj2.Data;
							reader.ReadEndElement();
						}
						else
						{
							XmlSerializer orCreateSerializer = MyXmlSerializerManager.GetOrCreateSerializer(value.Type);
							string serializedName = MyXmlSerializerManager.GetSerializedName(value.Type);
							value3 = Deserialize(reader, orCreateSerializer, serializedName);
						}
						value.SetValue(obj, value3);
					}
					else
					{
						reader.Skip();
					}
				}
				reader.MoveToContent();
			}
			reader.ReadEndElement();
			m_data = (TStruct)obj;
		}

		private static void BuildAccessorsInfo()
		{
			if (!(m_defaultValueField != null))
			{
				lock (typeof(TStruct))
				{
					if (!(m_defaultValueField != null))
					{
						m_defaultValueField = MyStructDefault.GetDefaultFieldInfo(typeof(TStruct));
						if (m_defaultValueField == null)
						{
							throw new Exception("Missing default value for struct " + typeof(TStruct).FullName + ". Decorate one static read-only field with StructDefault attribute");
						}
						m_accessorMap = new Dictionary<string, Accessor>();
						FieldInfo[] fields = typeof(TStruct).GetFields(BindingFlags.Instance | BindingFlags.Public);
						foreach (FieldInfo fieldInfo in fields)
						{
							if (fieldInfo.GetCustomAttribute(typeof(XmlIgnoreAttribute)) == null)
							{
								m_accessorMap.Add(fieldInfo.Name, new FieldAccessor(fieldInfo));
							}
						}
						PropertyInfo[] properties = typeof(TStruct).GetProperties(BindingFlags.Instance | BindingFlags.Public);
						foreach (PropertyInfo propertyInfo in properties)
						{
							if (propertyInfo.GetCustomAttribute(typeof(XmlIgnoreAttribute)) == null && propertyInfo.GetIndexParameters().Length == 0)
							{
								m_accessorMap.Add(propertyInfo.Name, new PropertyAccessor(propertyInfo));
							}
						}
					}
				}
			}
		}

		public static implicit operator MyStructXmlSerializer<TStruct>(TStruct data)
		{
			return new MyStructXmlSerializer<TStruct>(ref data);
		}
	}
}
