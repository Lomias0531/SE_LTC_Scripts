using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Input
{
	public static class MyGuiGameControlsHelpers
	{
		private static readonly Dictionary<MyStringId, MyDescriptor> m_gameControlHelpers = new Dictionary<MyStringId, MyDescriptor>(MyStringId.Comparer);

		public static MyDescriptor GetGameControlHelper(MyStringId controlHelper)
		{
			if (m_gameControlHelpers.TryGetValue(controlHelper, out MyDescriptor value))
			{
				return value;
			}
			return null;
		}

		public static void Add(MyStringId control, MyDescriptor descriptor)
		{
			m_gameControlHelpers.Add(control, descriptor);
		}

		public static void Reset()
		{
			m_gameControlHelpers.Clear();
		}
	}
}
