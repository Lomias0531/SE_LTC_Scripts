using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using System.Text;

namespace Sandbox.Graphics.GUI
{
	internal static class MyControlHelpers
	{
		public static void SetDetailedInfo<TBlock>(this MyGuiControlBlockProperty control, MyTerminalControl<TBlock>.WriterDelegate writer, TBlock block) where TBlock : MyTerminalBlock
		{
			StringBuilder textToDraw = control.ExtraInfoLabel.TextToDraw;
			textToDraw.Clear();
			if (writer != null && block != null)
			{
				writer(block, textToDraw);
			}
			control.ExtraInfoLabel.TextToDraw = textToDraw;
			control.ExtraInfoLabel.Visible = (textToDraw.Length > 0);
		}

		public static void SetDetailedInfo<TBlock>(this MyGuiControlBlockProperty control, MyTerminalControl<TBlock>.AdvancedWriterDelegate writer, TBlock block) where TBlock : MyTerminalBlock
		{
			StringBuilder textToDraw = control.ExtraInfoLabel.TextToDraw;
			textToDraw.Clear();
			if (writer != null && block != null)
			{
				writer(block, control, textToDraw);
			}
			control.ExtraInfoLabel.TextToDraw = textToDraw;
			control.ExtraInfoLabel.Visible = (textToDraw.Length > 0);
		}
	}
}
