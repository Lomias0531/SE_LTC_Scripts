using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using VRage.Generics;

namespace VRage
{
	/// <summary>
	/// Xml serializer base class with custom root element reader/writer caching
	/// </summary>
	/// <typeparam name="TAbstractBase"></typeparam>
	public abstract class MyXmlSerializerBase<TAbstractBase> : IMyXmlSerializable, IXmlSerializable
	{
		[ThreadStatic]
		private static MyObjectsPool<CustomRootReader> m_readerPool;

		[ThreadStatic]
		private static MyObjectsPool<CustomRootWriter> m_writerPool;

		protected TAbstractBase m_data;

		protected static MyObjectsPool<CustomRootReader> ReaderPool
		{
			get
			{
				if (m_readerPool == null)
				{
					m_readerPool = new MyObjectsPool<CustomRootReader>(2);
				}
				return m_readerPool;
			}
		}

		protected static MyObjectsPool<CustomRootWriter> WriterPool
		{
			get
			{
				if (m_writerPool == null)
				{
					m_writerPool = new MyObjectsPool<CustomRootWriter>(2);
				}
				return m_writerPool;
			}
		}

		public TAbstractBase Data => m_data;

		object IMyXmlSerializable.Data => m_data;

		public static implicit operator TAbstractBase(MyXmlSerializerBase<TAbstractBase> o)
		{
			return o.Data;
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public abstract void ReadXml(XmlReader reader);

		protected object Deserialize(XmlReader reader, XmlSerializer serializer, string customRootName)
		{
			ReaderPool.AllocateOrCreate(out CustomRootReader item);
			item.Init(customRootName, reader);
			object result = serializer.Deserialize(item);
			item.Release();
			ReaderPool.Deallocate(item);
			return result;
		}

		public void WriteXml(XmlWriter writer)
		{
			Type type = m_data.GetType();
			XmlSerializer orCreateSerializer = MyXmlSerializerManager.GetOrCreateSerializer(type);
			string serializedName = MyXmlSerializerManager.GetSerializedName(type);
			WriterPool.AllocateOrCreate(out CustomRootWriter item);
			item.Init(serializedName, writer);
			orCreateSerializer.Serialize(item, m_data);
			item.Release();
			WriterPool.Deallocate(item);
		}
	}
}
