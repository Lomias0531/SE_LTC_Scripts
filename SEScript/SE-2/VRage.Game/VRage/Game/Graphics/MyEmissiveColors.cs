using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Graphics
{
	public static class MyEmissiveColors
	{
		private static Dictionary<MyStringHash, Color> m_EmissiveColorDictionary = new Dictionary<MyStringHash, Color>();

		public static bool AddEmissiveColor(MyStringHash id, Color color, bool overWrite = false)
		{
			if (m_EmissiveColorDictionary.ContainsKey(id))
			{
				if (overWrite)
				{
					m_EmissiveColorDictionary[id] = color;
					return true;
				}
				return false;
			}
			m_EmissiveColorDictionary.Add(id, color);
			return true;
		}

		public static Color GetEmissiveColor(MyStringHash id)
		{
			if (m_EmissiveColorDictionary.ContainsKey(id))
			{
				return m_EmissiveColorDictionary[id];
			}
			return Color.Black;
		}

		public static void ClearColors()
		{
			m_EmissiveColorDictionary.Clear();
		}
	}
}
