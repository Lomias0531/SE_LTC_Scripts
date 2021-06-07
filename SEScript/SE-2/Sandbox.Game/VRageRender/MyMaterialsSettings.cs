using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using VRage;

namespace VRageRender
{
	[Obsolete]
	public class MyMaterialsSettings
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Struct
		{
			public static Struct Default;
		}

		public struct MyChangeableMaterial
		{
			public string MaterialName;
		}

		[XmlElement(Type = typeof(MyStructXmlSerializer<Struct>))]
		public Struct Data = Struct.Default;

		private MyChangeableMaterial[] m_changeableMaterials;

		[XmlArrayItem("ChangeableMaterial")]
		public MyChangeableMaterial[] ChangeableMaterials
		{
			get
			{
				return m_changeableMaterials;
			}
			set
			{
				if (m_changeableMaterials.Length != value.Length)
				{
					m_changeableMaterials = new MyChangeableMaterial[value.Length];
				}
				value.CopyTo(m_changeableMaterials, 0);
			}
		}

		public MyMaterialsSettings()
		{
			m_changeableMaterials = new MyChangeableMaterial[0];
		}

		public void CopyFrom(MyMaterialsSettings settings)
		{
			ChangeableMaterials = settings.ChangeableMaterials;
		}
	}
}
