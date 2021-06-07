using System.Collections.Generic;
using VRage.Collections;

namespace Sandbox.Game.GameSystems
{
	public static class MyDroneAIDataStatic
	{
		public static MyDroneAIData Default = new MyDroneAIData();

		private static Dictionary<string, MyDroneAIData> presets = new Dictionary<string, MyDroneAIData>();

		public static DictionaryReader<string, MyDroneAIData> Presets => presets;

		public static void Reset()
		{
			presets.Clear();
		}

		public static void SavePreset(string key, MyDroneAIData preset)
		{
			presets[key] = preset;
		}

		public static MyDroneAIData LoadPreset(string key)
		{
			return presets.GetValueOrDefault(key, Default);
		}
	}
}
