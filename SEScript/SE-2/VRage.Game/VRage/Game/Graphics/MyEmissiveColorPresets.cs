using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Game.Graphics
{
	public static class MyEmissiveColorPresets
	{
		private static Dictionary<MyStringHash, Dictionary<MyStringHash, MyEmissiveColorState>> m_presets = new Dictionary<MyStringHash, Dictionary<MyStringHash, MyEmissiveColorState>>();

		public static bool AddPreset(MyStringHash id, Dictionary<MyStringHash, MyEmissiveColorState> preset = null, bool overWrite = false)
		{
			if (m_presets.ContainsKey(id))
			{
				if (overWrite)
				{
					m_presets[id] = preset;
					return true;
				}
				return false;
			}
			m_presets.Add(id, preset);
			return true;
		}

		public static bool ContainsPreset(MyStringHash id)
		{
			return m_presets.ContainsKey(id);
		}

		public static bool LoadPresetState(MyStringHash presetId, MyStringHash stateId, out MyEmissiveColorStateResult result)
		{
			result = default(MyEmissiveColorStateResult);
			if (presetId == MyStringHash.NullOrEmpty)
			{
				presetId = MyStringHash.GetOrCompute("Default");
			}
			if (m_presets.ContainsKey(presetId) && m_presets[presetId].ContainsKey(stateId))
			{
				result.EmissiveColor = MyEmissiveColors.GetEmissiveColor(m_presets[presetId][stateId].EmissiveColor);
				result.DisplayColor = MyEmissiveColors.GetEmissiveColor(m_presets[presetId][stateId].DisplayColor);
				result.Emissivity = m_presets[presetId][stateId].Emissivity;
				return true;
			}
			return false;
		}

		public static Dictionary<MyStringHash, MyEmissiveColorState> GetPreset(MyStringHash id)
		{
			if (m_presets.ContainsKey(id))
			{
				return m_presets[id];
			}
			return null;
		}

		public static void ClearPresets()
		{
			m_presets.Clear();
		}

		public static void ClearPresetStates(MyStringHash id)
		{
			if (m_presets.ContainsKey(id) && m_presets[id] != null)
			{
				m_presets[id].Clear();
			}
		}

		public static bool AddPresetState(MyStringHash presetId, MyStringHash stateId, MyEmissiveColorState state, bool overWrite = false)
		{
			if (m_presets.ContainsKey(presetId))
			{
				if (m_presets[presetId] == null)
				{
					m_presets[presetId] = new Dictionary<MyStringHash, MyEmissiveColorState>();
				}
				if (m_presets[presetId].ContainsKey(stateId))
				{
					if (overWrite)
					{
						ClearPresetStates(presetId);
						m_presets[presetId][stateId] = state;
						return true;
					}
					return false;
				}
				m_presets[presetId].Add(stateId, state);
				return true;
			}
			return false;
		}
	}
}
