namespace VRage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Xml;
    using System.Xml.Serialization;

    public class MyStructXmlSerializer<TStruct> : MyXmlSerializerBase<TStruct> where TStruct: struct
    {
        public static FieldInfo m_defaultValueField;
        private static Dictionary<string, Accessor<TStruct>> m_accessorMap;

        public MyStructXmlSerializer()
        {
        }

        public MyStructXmlSerializer(ref TStruct data)
        {
            base.m_data = data;
        }

        private static void BuildAccessorsInfo()
        {
            if (MyStructXmlSerializer<TStruct>.m_defaultValueField == null)
            {
                Type type = (Type) typeof(TStruct);
                lock (type)
                {
                    if (MyStructXmlSerializer<TStruct>.m_defaultValueField == null)
                    {
                        MyStructXmlSerializer<TStruct>.m_defaultValueField = MyStructDefault.GetDefaultFieldInfo((Type) typeof(TStruct));
                        if (MyStructXmlSerializer<TStruct>.m_defaultValueField == null)
                        {
                            throw new Exception("Missing default value for struct " + typeof(TStruct).FullName + ". Decorate one static read-only field with StructDefault attribute");
                        }
                        MyStructXmlSerializer<TStruct>.m_accessorMap = new Dictionary<string, Accessor<TStruct>>();
                        foreach (FieldInfo info in typeof(TStruct).GetFields(((BindingFlags) BindingFlags.Public) | ((BindingFlags) BindingFlags.Instance)))
                        {
                            if (CustomAttributeExtensions.GetCustomAttribute((MemberInfo) info, (Type) typeof(XmlIgnoreAttribute)) == null)
                            {
                                MyStructXmlSerializer<TStruct>.m_accessorMap.Add(info.Name, new FieldAccessor<TStruct>(info));
                            }
                        }
                        foreach (PropertyInfo info2 in typeof(TStruct).GetProperties(((BindingFlags) BindingFlags.Public) | ((BindingFlags) BindingFlags.Instance)))
                        {
                            if ((CustomAttributeExtensions.GetCustomAttribute((MemberInfo) info2, (Type) typeof(XmlIgnoreAttribute)) == null) && (info2.GetIndexParameters().Length == 0))
                            {
                                MyStructXmlSerializer<TStruct>.m_accessorMap.Add(info2.Name, new PropertyAccessor<TStruct>(info2));
                            }
                        }
                    }
                }
            }
        }

        public static implicit operator MyStructXmlSerializer<TStruct>(TStruct data) => 
            new MyStructXmlSerializer<TStruct>(ref data);

        public override void ReadXml(XmlReader reader)
        {
            MyStructXmlSerializer<TStruct>.BuildAccessorsInfo();
            object obj2 = (TStruct) MyStructXmlSerializer<TStruct>.m_defaultValueField.GetValue(null);
            reader.MoveToElement();
            if (reader.IsEmptyElement)
            {
                reader.Skip();
            }
            else
            {
                reader.ReadStartElement();
                reader.MoveToContent();
                while ((reader.get_NodeType() != ((XmlNodeType) ((int) XmlNodeType.EndElement))) && (reader.get_NodeType() != ((XmlNodeType) ((int) XmlNodeType.None))))
                {
                    if (reader.get_NodeType() == ((XmlNodeType) ((int) XmlNodeType.Element)))
                    {
                        if (MyStructXmlSerializer<TStruct>.m_accessorMap.TryGetValue(reader.LocalName, out Accessor<TStruct> accessor))
                        {
                            object data;
                            if (accessor.IsPrimitiveType)
                            {
                                string str = reader.ReadElementString();
                                data = TypeDescriptor.GetConverter(accessor.Type).ConvertFrom(null, CultureInfo.get_InvariantCulture(), str);
                            }
                            else if (accessor.SerializerType != null)
                            {
                                IMyXmlSerializable serializable1 = Activator.CreateInstance(accessor.SerializerType) as IMyXmlSerializable;
                                serializable1.ReadXml(reader.ReadSubtree());
                                data = serializable1.Data;
                                reader.ReadEndElement();
                            }
                            else
                            {
                                XmlSerializer orCreateSerializer = MyXmlSerializerManager.GetOrCreateSerializer(accessor.Type);
                                string serializedName = MyXmlSerializerManager.GetSerializedName(accessor.Type);
                                data = base.Deserialize(reader, orCreateSerializer, serializedName);
                            }
                            accessor.SetValue(obj2, data);
                        }
                        else
                        {
                            reader.Skip();
                        }
                    }
                    reader.MoveToContent();
                }
                reader.ReadEndElement();
                base.m_data = (TStruct) obj2;
            }
        }

        private abstract class Accessor
        {
            protected Accessor()
            {
            }

            protected void CheckXmlElement(MemberInfo info)
            {
                XmlElementAttribute attribute = CustomAttributeExtensions.GetCustomAttribute(info, (System.Type) typeof(XmlElementAttribute), false) as XmlElementAttribute;
                if (((attribute != null) && (attribute.get_Type() != null)) && typeof(IMyXmlSerializable).IsAssignableFrom(attribute.get_Type()))
                {
                    this.SerializerType = attribute.get_Type();
                }
            }

            public abstract object GetValue(object obj);
            public abstract void SetValue(object obj, object value);

            public abstract System.Type Type { get; }

            public System.Type SerializerType { get; private set; }

            public bool IsPrimitiveType
            {
                get
                {
                    System.Type type = this.Type;
                    if (!type.IsPrimitive)
                    {
                        return (type == typeof(string));
                    }
                    return true;
                }
            }
        }

        private class FieldAccessor : MyStructXmlSerializer<TStruct>.Accessor
        {
            public FieldAccessor(FieldInfo field)
            {
                this.Field = field;
                base.CheckXmlElement((MemberInfo) field);
            }

            public override object GetValue(object obj) => 
                this.Field.GetValue(obj);

            public override void SetValue(object obj, object value)
            {
                this.Field.SetValue(obj, value);
            }

            public FieldInfo Field { get; private set; }

            public override System.Type Type =>
                this.Field.get_FieldType();
        }

        private class PropertyAccessor : MyStructXmlSerializer<TStruct>.Accessor
        {
            public PropertyAccessor(PropertyInfo property)
            {
                this.Property = property;
                base.CheckXmlElement((MemberInfo) property);
            }

            public override object GetValue(object obj) => 
                this.Property.GetValue(obj);

            public override void SetValue(object obj, object value)
            {
                this.Property.SetValue(obj, value);
            }

            public PropertyInfo Property { get; private set; }

            public override System.Type Type =>
                this.Property.get_PropertyType();
        }
    }
}

