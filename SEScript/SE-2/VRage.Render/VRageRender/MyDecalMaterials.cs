using System.Collections.Generic;
using VRage.Utils;

namespace VRageRender
{
	public static class MyDecalMaterials
	{
		private static Dictionary<string, List<MyDecalMaterial>> m_decalMaterials = new Dictionary<string, List<MyDecalMaterial>>();

		public static void AddDecalMaterial(MyDecalMaterial decalMaterial)
		{
			if (!m_decalMaterials.TryGetValue(decalMaterial.StringId, out List<MyDecalMaterial> value))
			{
				value = new List<MyDecalMaterial>();
				m_decalMaterials[decalMaterial.StringId] = value;
			}
			value.Add(decalMaterial);
		}

		public static void ClearMaterials()
		{
			m_decalMaterials.Clear();
		}

		public static bool TryGetDecalMaterial(string source, string target, out IReadOnlyList<MyDecalMaterial> decalMaterials)
		{
			List<MyDecalMaterial> decalMaterial;
			bool result = TryGetDecalMateriald(source, target, out decalMaterial);
			decalMaterials = decalMaterial;
			return result;
		}

		private static bool TryGetDecalMateriald(string source, string target, out List<MyDecalMaterial> decalMaterial)
		{
			string stringId = GetStringId(source, target);
			return m_decalMaterials.TryGetValue(stringId, out decalMaterial);
		}

		public static string GetStringId(string source, string target)
		{
			return (string.IsNullOrEmpty(source) ? "NULL" : source) + "_" + (string.IsNullOrEmpty(target) ? "NULL" : target);
		}

		public static string GetStringId(MyStringHash source, MyStringHash target)
		{
			return ((source == MyStringHash.NullOrEmpty) ? "NULL" : source.String) + "_" + ((target == MyStringHash.NullOrEmpty) ? "NULL" : target.String);
		}
	}
}
