using System.Collections.Generic;
using System.IO;

namespace VRageRender.Import
{
	public class MyMeshSectionInfo
	{
		public string Name
		{
			get;
			set;
		}

		public List<MyMeshSectionMeshInfo> Meshes
		{
			get;
			private set;
		}

		public MyMeshSectionInfo()
		{
			Meshes = new List<MyMeshSectionMeshInfo>();
		}

		public bool Export(BinaryWriter writer)
		{
			writer.Write(Name);
			writer.Write(Meshes.Count);
			bool flag = true;
			foreach (MyMeshSectionMeshInfo mesh in Meshes)
			{
				flag &= mesh.Export(writer);
			}
			return flag;
		}

		public bool Import(BinaryReader reader, int version)
		{
			Name = reader.ReadString();
			int num = reader.ReadInt32();
			bool flag = true;
			for (int i = 0; i < num; i++)
			{
				MyMeshSectionMeshInfo myMeshSectionMeshInfo = new MyMeshSectionMeshInfo();
				flag &= myMeshSectionMeshInfo.Import(reader, version);
				Meshes.Add(myMeshSectionMeshInfo);
			}
			return flag;
		}
	}
}
