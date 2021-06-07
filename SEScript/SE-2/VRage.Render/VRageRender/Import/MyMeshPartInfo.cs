using System;
using System.Collections.Generic;
using System.IO;

namespace VRageRender.Import
{
	public class MyMeshPartInfo
	{
		public int m_MaterialHash;

		public MyMaterialDescriptor m_MaterialDesc;

		public List<int> m_indices = new List<int>();

		public MyMeshDrawTechnique Technique;

		public string GetMaterialName()
		{
			string result = "";
			if (m_MaterialDesc != null)
			{
				result = m_MaterialDesc.MaterialName;
			}
			return result;
		}

		public bool Export(BinaryWriter writer)
		{
			writer.Write(m_MaterialHash);
			writer.Write(m_indices.Count);
			foreach (int index in m_indices)
			{
				writer.Write(index);
			}
			bool result = true;
			if (m_MaterialDesc != null)
			{
				writer.Write(value: true);
				result = m_MaterialDesc.Write(writer);
			}
			else
			{
				writer.Write(value: false);
			}
			return result;
		}

		public bool Import(BinaryReader reader, int version)
		{
			m_MaterialHash = reader.ReadInt32();
			if (version < 1052001)
			{
				reader.ReadInt32();
			}
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				m_indices.Add(reader.ReadInt32());
			}
			bool num2 = reader.ReadBoolean();
			bool result = true;
			if (num2)
			{
				m_MaterialDesc = new MyMaterialDescriptor();
				result = m_MaterialDesc.Read(reader, version);
				result &= Enum.TryParse(m_MaterialDesc.Technique, out Technique);
			}
			else
			{
				m_MaterialDesc = null;
			}
			return result;
		}
	}
}
