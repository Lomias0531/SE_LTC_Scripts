using VRage.Game.Entity.UseObject;

namespace Sandbox.Game.Gui
{
	internal struct MyHudSelectedObjectStatus
	{
		public IMyUseObject Instance;

		public string[] SectionNames;

		public int InstanceId;

		public uint[] SubpartIndices;

		public MyHudObjectHighlightStyle Style;

		public void Reset()
		{
			Instance = null;
			SectionNames = null;
			InstanceId = -1;
			SubpartIndices = null;
			Style = MyHudObjectHighlightStyle.None;
		}
	}
}
