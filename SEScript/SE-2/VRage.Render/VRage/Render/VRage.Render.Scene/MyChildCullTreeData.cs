using System;
using VRageMath;

namespace VRage.Render.Scene
{
	public class MyChildCullTreeData
	{
		public bool FarCull;

		public Action<MyCullResultsBase, bool> Add;

		public Action<MyCullResultsBase> Remove;

		public Func<Color> DebugColor;
	}
}
