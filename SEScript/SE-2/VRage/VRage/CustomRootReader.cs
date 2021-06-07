using System.Xml;
using VRage.Network;

namespace VRage
{
	/// <summary>
	/// Custom XmlReader that allows to read xml fragments
	/// </summary>
	[GenerateActivator]
	public class CustomRootReader : XmlReader
	{
		private class VRage_CustomRootReader_003C_003EActor : IActivator, IActivator<CustomRootReader>
		{
			private sealed override object CreateInstance()
			{
				return new CustomRootReader();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override CustomRootReader CreateInstance()
			{
				return new CustomRootReader();
			}

			CustomRootReader IActivator<CustomRootReader>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private XmlReader m_source;

		private string m_customRootName;

		private int m_rootDepth;

		public override int AttributeCount => m_source.AttributeCount;

		public override string BaseURI => m_source.BaseURI;

		public override int Depth => m_source.Depth;

		public override bool EOF => m_source.EOF;

		public override bool IsEmptyElement => m_source.IsEmptyElement;

		public override XmlNameTable NameTable => m_source.NameTable;

		public override XmlNodeType NodeType => m_source.NodeType;

		public override string Prefix => m_source.Prefix;

		public override ReadState ReadState => m_source.ReadState;

		public override string Value => m_source.Value;

		public override string LocalName
		{
			get
			{
				if (m_source.Depth != m_rootDepth)
				{
					return m_source.LocalName;
				}
				return m_source.NameTable.Get(m_customRootName);
			}
		}

		public override string NamespaceURI
		{
			get
			{
				if (m_source.Depth != m_rootDepth)
				{
					return m_source.NamespaceURI;
				}
				return m_source.NameTable.Get("");
			}
		}

		internal void Init(string customRootName, XmlReader source)
		{
			m_source = source;
			m_customRootName = customRootName;
			m_rootDepth = source.Depth;
		}

		internal void Release()
		{
			m_source = null;
			m_customRootName = null;
			m_rootDepth = -1;
		}

		public override void Close()
		{
			m_source.Close();
		}

		public override string GetAttribute(int i)
		{
			return m_source.GetAttribute(i);
		}

		public override string GetAttribute(string name)
		{
			return m_source.GetAttribute(name);
		}

		public override string LookupNamespace(string prefix)
		{
			return m_source.LookupNamespace(prefix);
		}

		public override bool MoveToAttribute(string name, string ns)
		{
			return m_source.MoveToAttribute(name, ns);
		}

		public override bool MoveToAttribute(string name)
		{
			return m_source.MoveToAttribute(name);
		}

		public override bool MoveToElement()
		{
			return m_source.MoveToElement();
		}

		public override bool MoveToFirstAttribute()
		{
			return m_source.MoveToFirstAttribute();
		}

		public override bool MoveToNextAttribute()
		{
			return m_source.MoveToNextAttribute();
		}

		public override bool Read()
		{
			return m_source.Read();
		}

		public override bool ReadAttributeValue()
		{
			return m_source.ReadAttributeValue();
		}

		public override void ResolveEntity()
		{
			m_source.ResolveEntity();
		}

		public override string GetAttribute(string name, string namespaceURI)
		{
			if (m_source.Depth != m_rootDepth)
			{
				return m_source.GetAttribute(name, namespaceURI);
			}
			return null;
		}
	}
}
