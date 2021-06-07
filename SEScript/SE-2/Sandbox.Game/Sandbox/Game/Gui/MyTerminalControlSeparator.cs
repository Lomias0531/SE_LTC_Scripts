using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyTerminalControlSeparator<TBlock> : MyTerminalControl<TBlock>, IMyTerminalControlSeparator, IMyTerminalControl where TBlock : MyTerminalBlock
	{
		string IMyTerminalControl.Id => "";

		public MyTerminalControlSeparator()
			: base("Separator")
		{
		}

		protected override MyGuiControlBase CreateGui()
		{
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.Size = new Vector2(0.485f, 0.01f);
			myGuiControlSeparatorList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			myGuiControlSeparatorList.AddHorizontal(Vector2.Zero, 0.225f);
			return myGuiControlSeparatorList;
		}
	}
}
