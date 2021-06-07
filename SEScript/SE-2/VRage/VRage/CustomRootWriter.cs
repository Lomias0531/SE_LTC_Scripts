using System.Linq;
using System.Xml;
using VRage.Network;

namespace VRage
{
	[GenerateActivator]
	public class CustomRootWriter : XmlWriter
	{
		private class VRage_CustomRootWriter_003C_003EActor : IActivator, IActivator<CustomRootWriter>
		{
			private sealed override object CreateInstance()
			{
				return new CustomRootWriter();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override CustomRootWriter CreateInstance()
			{
				return new CustomRootWriter();
			}

			CustomRootWriter IActivator<CustomRootWriter>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private XmlWriter m_target;

		private string m_customRootType;

		private int m_currentDepth;

		public override WriteState WriteState => m_target.WriteState;

		internal void Init(string customRootType, XmlWriter target)
		{
			m_target = target;
			m_customRootType = customRootType;
			m_target.WriteAttributeString("xsi:type", m_customRootType);
			m_currentDepth = 0;
		}

		internal void Release()
		{
			m_target = null;
			m_customRootType = null;
		}

		public override void Close()
		{
			m_target.Close();
		}

		public override void Flush()
		{
			m_target.Flush();
		}

		public override string LookupPrefix(string ns)
		{
			return m_target.LookupPrefix(ns);
		}

		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			m_target.WriteBase64(buffer, index, count);
		}

		public override void WriteCData(string text)
		{
			m_target.WriteCData(text);
		}

		public override void WriteCharEntity(char ch)
		{
			m_target.WriteCharEntity(ch);
		}

		public override void WriteChars(char[] buffer, int index, int count)
		{
			m_target.WriteChars(buffer, index, count);
		}

		public override void WriteComment(string text)
		{
			m_target.WriteComment(text);
		}

		public override void WriteEndAttribute()
		{
			m_target.WriteEndAttribute();
		}

		public override void WriteEndElement()
		{
			m_currentDepth--;
			if (m_currentDepth > 0)
			{
				m_target.WriteEndElement();
			}
		}

		public override void WriteEntityRef(string name)
		{
			m_target.WriteEntityRef(name);
		}

		public override void WriteFullEndElement()
		{
			m_target.WriteFullEndElement();
		}

		public override void WriteProcessingInstruction(string name, string text)
		{
			m_target.WriteProcessingInstruction(name, text);
		}

		public override void WriteRaw(string data)
		{
			m_target.WriteRaw(data);
		}

		public override void WriteRaw(char[] buffer, int index, int count)
		{
			m_target.WriteRaw(buffer, index, count);
		}

		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			m_target.WriteStartAttribute(prefix, localName, ns);
		}

		public override void WriteString(string text)
		{
			if (!IsValidXmlString(text))
			{
				text = RemoveInvalidXmlChars(text);
			}
			m_target.WriteString(text);
		}

		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			m_target.WriteSurrogateCharEntity(lowChar, highChar);
		}

		public override void WriteWhitespace(string ws)
		{
			m_target.WriteWhitespace(ws);
		}

		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
		}

		public override void WriteStartDocument(bool standalone)
		{
		}

		public override void WriteStartDocument()
		{
		}

		public override void WriteEndDocument()
		{
			while (m_currentDepth > 0)
			{
				WriteEndElement();
			}
		}

		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (m_currentDepth > 0)
			{
				m_target.WriteStartElement(prefix, localName, ns);
			}
			m_currentDepth++;
		}

		private static string RemoveInvalidXmlChars(string text)
		{
			return new string(text.Where((char ch) => XmlConvert.IsXmlChar(ch)).ToArray());
		}

		private static bool IsValidXmlString(string text)
		{
			return text.All(XmlConvert.IsXmlChar);
		}
	}
}
