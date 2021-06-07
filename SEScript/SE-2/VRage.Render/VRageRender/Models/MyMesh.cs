using VRageRender.Import;

namespace VRageRender.Models
{
	public class MyMesh
	{
		public readonly string AssetName;

		public readonly MyMeshMaterial Material;

		public int IndexStart;

		public int TriStart;

		public int TriCount;

		/// <summary>
		/// c-tor - generic way for collecting resources
		/// </summary>
		/// <param name="meshInfo"></param>
		/// assetName - just for debug output
		public MyMesh(MyMeshPartInfo meshInfo, string assetName)
		{
			MyMaterialDescriptor materialDesc = meshInfo.m_MaterialDesc;
			if (materialDesc != null)
			{
				materialDesc.Textures.TryGetValue("DiffuseTexture", out string _);
				Material = new MyMeshMaterial
				{
					Name = meshInfo.m_MaterialDesc.MaterialName,
					Textures = materialDesc.Textures,
					DrawTechnique = meshInfo.Technique,
					GlassCW = meshInfo.m_MaterialDesc.GlassCW,
					GlassCCW = meshInfo.m_MaterialDesc.GlassCCW,
					GlassSmooth = meshInfo.m_MaterialDesc.GlassSmoothNormals
				};
			}
			else
			{
				Material = new MyMeshMaterial();
			}
			AssetName = assetName;
		}

		public MyMesh(MyMeshMaterial material, string assetName)
		{
			Material = material;
			AssetName = assetName;
		}
	}
}
