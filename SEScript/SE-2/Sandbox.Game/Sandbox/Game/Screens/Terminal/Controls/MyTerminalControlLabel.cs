using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.Screens.Terminal.Controls
{
	public class MyTerminalControlLabel<TBlock> : MyTerminalControl<TBlock>, IMyTerminalControlLabel, IMyTerminalControl where TBlock : MyTerminalBlock
	{
		public MyStringId Label;

		private MyGuiControlLabel m_label;

		MyStringId IMyTerminalControlLabel.Label
		{
			get
			{
				return Label;
			}
			set
			{
				Label = value;
			}
		}

		public MyTerminalControlLabel(MyStringId label)
			: base("Label")
		{
			Label = label;
		}

		protected override MyGuiControlBase CreateGui()
		{
			m_label = new MyGuiControlLabel();
			return new MyGuiControlBlockProperty(MyTexts.GetString(Label), null, m_label, MyGuiControlBlockPropertyLayoutEnum.Horizontal);
		}
	}
}
