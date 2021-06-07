using Sandbox.Game.Screens.Helpers;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Testing Tool")]
	internal class MyGuiScreenDebugTestingTool : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugTestingTool()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Test Tool Control", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddButton("Almighty Button", delegate
			{
				MyTestingToolHelper.Instance.Action_SpawnBlockSaveTestReload();
			});
			AddButton("Spawn monolith Button (less mighty)", delegate
			{
				MyTestingToolHelper.Instance.Action_Test();
			});
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugTestingTool";
		}
	}
}
