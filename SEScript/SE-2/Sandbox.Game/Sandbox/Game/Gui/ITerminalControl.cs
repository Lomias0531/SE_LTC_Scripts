using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Gui
{
	public interface ITerminalControl
	{
		string Id
		{
			get;
		}

		bool SupportsMultipleBlocks
		{
			get;
		}

		MyTerminalBlock[] TargetBlocks
		{
			get;
			set;
		}

		ITerminalAction[] Actions
		{
			get;
		}

		MyGuiControlBase GetGuiControl();

		void UpdateVisual();

		bool IsVisible(MyTerminalBlock block);
	}
}
