using System.Xml;

namespace VRage.Game
{
	/// <summary>
	/// Custom XmlSerializer for definitions that allows to override the definition type
	/// </summary>
	public class MyDefinitionXmlSerializer : MyAbstractXmlSerializer<MyObjectBuilder_DefinitionBase>
	{
		public const string DEFINITION_ELEMENT_NAME = "Definition";

		public MyDefinitionXmlSerializer()
		{
		}

		public MyDefinitionXmlSerializer(MyObjectBuilder_DefinitionBase data)
		{
			m_data = data;
		}

		protected override string GetTypeAttribute(XmlReader reader)
		{
			string typeAttribute = base.GetTypeAttribute(reader);
			if (typeAttribute == null)
			{
				return null;
			}
			MyXmlTextReader myXmlTextReader = reader as MyXmlTextReader;
			if (myXmlTextReader != null && myXmlTextReader.DefinitionTypeOverrideMap != null && myXmlTextReader.DefinitionTypeOverrideMap.TryGetValue(typeAttribute, out string value))
			{
				return value;
			}
			return typeAttribute;
		}

		public static implicit operator MyDefinitionXmlSerializer(MyObjectBuilder_DefinitionBase builder)
		{
			if (builder != null)
			{
				return new MyDefinitionXmlSerializer(builder);
			}
			return null;
		}
	}
}
