using System.Collections.Generic;

namespace VRage.Game.VisualScripting.Utils
{
	public static class MyVSCollectionExtensions
	{
		[VisualScriptingMember(false, false)]
		public static T At<T>(this List<T> list, int index)
		{
			if (index >= 0 && index < list.Count)
			{
				return list[index];
			}
			return default(T);
		}

		[VisualScriptingMember(false, false)]
		public static int Count<T>(this List<T> list)
		{
			return list.Count;
		}
	}
}
