using System.IO;

namespace VRageRender.Import
{
	public class MyMeshSectionMeshInfo
	{
		public string MaterialName
		{
			get;
			set;
		}

		/// <summary>Offset in index list</summary>
		public int StartIndex
		{
			get;
			set;
		}

		/// <summary>Offset in index list</summary>
		public int IndexCount
		{
			get;
			set;
		}

		public MyMeshSectionMeshInfo()
		{
			StartIndex = -1;
		}

		public bool Export(BinaryWriter writer)
		{
			writer.Write(MaterialName);
			writer.Write(StartIndex);
			writer.Write(IndexCount);
			return true;
		}

		public bool Import(BinaryReader reader, int version)
		{
			MaterialName = reader.ReadString();
			StartIndex = reader.ReadInt32();
			IndexCount = reader.ReadInt32();
			return true;
		}
	}
}
