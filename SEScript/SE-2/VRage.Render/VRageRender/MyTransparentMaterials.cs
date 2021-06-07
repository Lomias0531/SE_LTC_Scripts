using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Utils;
using VRageMath;

namespace VRageRender
{
	public static class MyTransparentMaterials
	{
		private static readonly Dictionary<MyStringId, MyTransparentMaterial> m_materialsByName;

		public static readonly MyTransparentMaterial ErrorMaterial;

		private static Action m_onUpdate;

		public static DictionaryValuesReader<MyStringId, MyTransparentMaterial> Materials => new DictionaryValuesReader<MyStringId, MyTransparentMaterial>(m_materialsByName);

		public static int Count => m_materialsByName.Count;

		static MyTransparentMaterials()
		{
			m_materialsByName = new Dictionary<MyStringId, MyTransparentMaterial>(MyStringId.Comparer);
			ErrorMaterial = new MyTransparentMaterial(MyStringId.GetOrCompute("ErrorMaterial"), MyTransparentMaterialTextureType.FileTexture, "Textures\\FAKE.dds", "Textures\\FAKE.dds", 9999f, canBeAffectedByOtherLights: false, alphaMistingEnable: false, Color.Pink.ToVector4(), Color.Black, Color.Black, Vector4.One * 0.1f, isFlareOccluder: false, triangleFaceCulling: true);
			Clear();
		}

		public static bool TryGetMaterial(MyStringId materialId, out MyTransparentMaterial material)
		{
			return m_materialsByName.TryGetValue(materialId, out material);
		}

		public static MyTransparentMaterial GetMaterial(MyStringId materialId)
		{
			if (m_materialsByName.TryGetValue(materialId, out MyTransparentMaterial value))
			{
				return value;
			}
			return ErrorMaterial;
		}

		public static bool ContainsMaterial(MyStringId materialId)
		{
			return m_materialsByName.ContainsKey(materialId);
		}

		public static void AddMaterial(MyTransparentMaterial material)
		{
			m_materialsByName[material.Id] = material;
		}

		private static void Clear()
		{
			m_materialsByName.Clear();
			AddMaterial(ErrorMaterial);
		}

		public static void Update()
		{
			if (m_onUpdate != null)
			{
				m_onUpdate();
			}
		}
	}
}
