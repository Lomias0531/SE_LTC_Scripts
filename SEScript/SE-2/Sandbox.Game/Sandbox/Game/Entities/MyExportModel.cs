using System;
using System.Collections.Generic;
using VRage.Game.Models;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender;
using VRageRender.Import;
using VRageRender.Models;

namespace Sandbox.Game.Entities
{
	public class MyExportModel
	{
		public struct Material
		{
			public int LastTri;

			public int FirstTri;

			public bool IsGlass;

			public Vector3 ColorMaskHSV;

			public string ExportedMaterialName;

			public string AddMapsTexture;

			public string AlphamaskTexture;

			public string ColorMetalTexture;

			public string NormalGlossTexture;

			public bool EqualsMaterialWise(Material x)
			{
				if (string.Equals(AddMapsTexture, x.AddMapsTexture, StringComparison.OrdinalIgnoreCase) && string.Equals(AlphamaskTexture, x.AlphamaskTexture, StringComparison.OrdinalIgnoreCase) && string.Equals(ColorMetalTexture, x.ColorMetalTexture, StringComparison.OrdinalIgnoreCase))
				{
					return string.Equals(NormalGlossTexture, x.NormalGlossTexture, StringComparison.OrdinalIgnoreCase);
				}
				return false;
			}
		}

		private readonly MyModel m_model;

		private List<Material> m_materials;

		public MyExportModel(MyModel model)
		{
			m_model = model;
			m_model.LoadData();
			ExtractMaterialsFromModel();
		}

		public HalfVector2[] GetTexCoords()
		{
			return m_model.TexCoords;
		}

		public List<Material> GetMaterials()
		{
			return m_materials;
		}

		public int GetVerticesCount()
		{
			return m_model.GetVerticesCount();
		}

		public int GetTrianglesCount()
		{
			return m_model.GetTrianglesCount();
		}

		public MyTriangleVertexIndices GetTriangle(int index)
		{
			return m_model.GetTriangle(index);
		}

		private void ExtractMaterialsFromModel()
		{
			m_materials = new List<Material>();
			List<MyMesh> meshList = m_model.GetMeshList();
			if (meshList != null)
			{
				foreach (MyMesh item in meshList)
				{
					if (item.Material != null)
					{
						Dictionary<string, string> textures = item.Material.Textures;
						if (textures != null)
						{
							m_materials.Add(new Material
							{
								AddMapsTexture = textures.Get("AddMapsTexture"),
								AlphamaskTexture = textures.Get("AlphamaskTexture"),
								ColorMetalTexture = textures.Get("ColorMetalTexture"),
								NormalGlossTexture = textures.Get("NormalGlossTexture"),
								FirstTri = item.IndexStart / 3,
								LastTri = item.IndexStart / 3 + item.TriCount - 1,
								IsGlass = (item.Material.DrawTechnique == MyMeshDrawTechnique.GLASS || item.Material.DrawTechnique == MyMeshDrawTechnique.HOLO || item.Material.DrawTechnique == MyMeshDrawTechnique.SHIELD || item.Material.DrawTechnique == MyMeshDrawTechnique.SHIELD_LIT)
							});
						}
					}
				}
			}
		}
	}
}
