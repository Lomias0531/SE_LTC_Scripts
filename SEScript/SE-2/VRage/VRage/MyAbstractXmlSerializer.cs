using System.Xml;
using System.Xml.Serialization;

namespace VRage
{
	/// <summary>
	/// Custom xml serializer that allows object instantiation on elements with xsl:type attribute
	/// </summary>
	/// <typeparam name="TAbstractBase"></typeparam>
	public class MyAbstractXmlSerializer<TAbstractBase> : MyXmlSerializerBase<TAbstractBase>
	{
		public MyAbstractXmlSerializer()
		{
		}

		public MyAbstractXmlSerializer(TAbstractBase data)
		{
			m_data = data;
		}

		public override void ReadXml(XmlReader reader)
		{
			string customRootName;
			XmlSerializer serializer = GetSerializer(reader, out customRootName);
			m_data = (TAbstractBase)Deserialize(reader, serializer, customRootName);
		}

		private XmlSerializer GetSerializer(XmlReader reader, out string customRootName)
		{
			string text = GetTypeAttribute(reader);
			if (text == null || !MyXmlSerializerManager.TryGetSerializer(text, out XmlSerializer serializer))
			{
				text = MyXmlSerializerManager.GetSerializedName(typeof(TAbstractBase));
				serializer = MyXmlSerializerManager.GetSerializer(text);
			}
			customRootName = text;
			return serializer;
		}

		protected virtual string GetTypeAttribute(XmlReader reader)
		{
			return reader.GetAttribute("xsi:type");
		}

		public static implicit operator MyAbstractXmlSerializer<TAbstractBase>(TAbstractBase builder)
		{
			if (builder != null)
			{
				return new MyAbstractXmlSerializer<TAbstractBase>(builder);
			}
			return null;
		}
	}
}
