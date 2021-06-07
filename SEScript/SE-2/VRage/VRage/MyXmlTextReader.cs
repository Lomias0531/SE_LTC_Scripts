using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace VRage
{
	/// <summary>
	/// Custom XML Reader with user data attached
	/// </summary>
	public class MyXmlTextReader : XmlReader
	{
		private XmlReader m_reader;

		/// <summary>
		/// Map to override definitions types
		/// </summary>
		public Dictionary<string, string> DefinitionTypeOverrideMap
		{
			get;
			set;
		}

		public override int AttributeCount => m_reader.AttributeCount;

		public override string BaseURI => m_reader.BaseURI;

		public override int Depth => m_reader.Depth;

		public override bool EOF => m_reader.EOF;

		public override bool IsEmptyElement => m_reader.IsEmptyElement;

		public override string LocalName => m_reader.LocalName;

		public override XmlNameTable NameTable => m_reader.NameTable;

		public override string NamespaceURI => m_reader.NamespaceURI;

		public override XmlNodeType NodeType => m_reader.NodeType;

		public override string Prefix => m_reader.Prefix;

		public override ReadState ReadState => m_reader.ReadState;

		public override string Value => m_reader.Value;

		public MyXmlTextReader(Stream input, XmlReaderSettings settings)
		{
			m_reader = XmlReader.Create(input, settings);
		}

		public override string GetAttribute(int i)
		{
			return m_reader.GetAttribute(i);
		}

		public override string GetAttribute(string name, string namespaceURI)
		{
			return m_reader.GetAttribute(name, namespaceURI);
		}

		public override string GetAttribute(string name)
		{
			return m_reader.GetAttribute(name);
		}

		public override string LookupNamespace(string prefix)
		{
			return m_reader.LookupNamespace(prefix);
		}

		public override bool MoveToAttribute(string name, string ns)
		{
			return m_reader.MoveToAttribute(name, ns);
		}

		public override bool MoveToAttribute(string name)
		{
			return m_reader.MoveToAttribute(name);
		}

		public override bool MoveToElement()
		{
			return m_reader.MoveToElement();
		}

		public override bool MoveToFirstAttribute()
		{
			return m_reader.MoveToFirstAttribute();
		}

		public override bool MoveToNextAttribute()
		{
			return m_reader.MoveToNextAttribute();
		}

		public override bool Read()
		{
			return m_reader.Read();
		}

		public override bool ReadAttributeValue()
		{
			return m_reader.ReadAttributeValue();
		}

		public override void ResolveEntity()
		{
			m_reader.ResolveEntity();
		}
	}
}
