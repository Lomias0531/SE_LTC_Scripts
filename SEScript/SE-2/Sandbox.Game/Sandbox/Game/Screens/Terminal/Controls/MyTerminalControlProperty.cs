using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;

namespace Sandbox.Game.Screens.Terminal.Controls
{
	public class MyTerminalControlProperty<TBlock, TValue> : MyTerminalValueControl<TBlock, TValue>, IMyTerminalControlProperty<TValue>, IMyTerminalControl, IMyTerminalValueControl<TValue>, ITerminalProperty where TBlock : MyTerminalBlock
	{
		public MyTerminalControlProperty(string id)
			: base(id)
		{
			Visible = ((TBlock x) => false);
		}

		public override TValue GetDefaultValue(TBlock block)
		{
			return default(TValue);
		}

		public override TValue GetMaximum(TBlock block)
		{
			return GetDefaultValue(block);
		}

		public override TValue GetMinimum(TBlock block)
		{
			return GetDefaultValue(block);
		}

		protected override MyGuiControlBase CreateGui()
		{
			Visible = ((TBlock x) => false);
			return null;
		}
	}
}
