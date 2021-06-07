namespace VRage
{
    using System;
    using System.Runtime.InteropServices;
    using System.Xml;
    using System.Xml.Serialization;

    public class MyAbstractXmlSerializer<TAbstractBase> : MyXmlSerializerBase<TAbstractBase>
    {
        public MyAbstractXmlSerializer()
        {
        }

        public MyAbstractXmlSerializer(TAbstractBase data)
        {
            base.m_data = data;
        }

        private XmlSerializer GetSerializer(XmlReader reader, out string customRootName)
        {
            string typeAttribute = this.GetTypeAttribute(reader);
            if ((typeAttribute == null) || !MyXmlSerializerManager.TryGetSerializer(typeAttribute, out XmlSerializer serializer))
            {
                typeAttribute = MyXmlSerializerManager.GetSerializedName((Type) typeof(TAbstractBase));
                serializer = MyXmlSerializerManager.GetSerializer(typeAttribute);
            }
            customRootName = typeAttribute;
            return serializer;
        }

        protected virtual string GetTypeAttribute(XmlReader reader) => 
            reader.GetAttribute("xsi:type");

        public static implicit operator MyAbstractXmlSerializer<TAbstractBase>(TAbstractBase builder)
        {
            if (builder != null)
            {
                return new MyAbstractXmlSerializer<TAbstractBase>(builder);
            }
            return null;
        }

        public override void ReadXml(XmlReader reader)
        {
            XmlSerializer serializer = this.GetSerializer(reader, out string str);
            base.m_data = (TAbstractBase) base.Deserialize(reader, serializer, str);
        }
    }
}

