using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Text;
using VRage;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyTerminalControlCheckbox<TBlock> : MyTerminalValueControl<TBlock, bool>, IMyTerminalControlCheckbox, IMyTerminalControl, IMyTerminalValueControl<bool>, ITerminalProperty, IMyTerminalControlTitleTooltip where TBlock : MyTerminalBlock
	{
		private Action<TBlock> m_action;

		private MyGuiControlCheckbox m_checkbox;

		private MyGuiControlBlockProperty m_checkboxAligner;

		private Action<MyGuiControlCheckbox> m_checkboxClicked;

		public MyStringId Title;

		public MyStringId OnText;

		public MyStringId OffText;

		public MyStringId Tooltip;

		private bool m_justify;

		MyStringId IMyTerminalControlCheckbox.OnText
		{
			get
			{
				return OnText;
			}
			set
			{
				OnText = value;
			}
		}

		MyStringId IMyTerminalControlCheckbox.OffText
		{
			get
			{
				return OffText;
			}
			set
			{
				OffText = value;
			}
		}

		MyStringId IMyTerminalControlTitleTooltip.Title
		{
			get
			{
				return Title;
			}
			set
			{
				Title = value;
			}
		}

		MyStringId IMyTerminalControlTitleTooltip.Tooltip
		{
			get
			{
				return Tooltip;
			}
			set
			{
				Tooltip = value;
			}
		}

		public MyTerminalControlCheckbox(string id, MyStringId title, MyStringId tooltip, MyStringId? on = null, MyStringId? off = null, bool justify = false)
			: base(id)
		{
			Title = title;
			OnText = (on ?? MySpaceTexts.SwitchText_On);
			OffText = (off ?? MySpaceTexts.SwitchText_Off);
			Tooltip = tooltip;
			Serializer = delegate(BitStream stream, ref bool value)
			{
				stream.Serialize(ref value);
			};
			m_justify = justify;
		}

		protected override MyGuiControlBase CreateGui()
		{
			m_checkbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(Tooltip));
			m_checkboxClicked = OnCheckboxClicked;
			m_checkbox.IsCheckedChanged = m_checkboxClicked;
			m_checkboxAligner = new MyGuiControlBlockProperty(MyTexts.GetString(Title), MyTexts.GetString(Tooltip), m_checkbox, MyGuiControlBlockPropertyLayoutEnum.Horizontal);
			return m_checkboxAligner;
		}

		private void OnCheckboxClicked(MyGuiControlCheckbox obj)
		{
			foreach (TBlock targetBlock in base.TargetBlocks)
			{
				SetValue(targetBlock, obj.IsChecked);
			}
		}

		protected override void OnUpdateVisual()
		{
			base.OnUpdateVisual();
			TBlock firstBlock = base.FirstBlock;
			if (firstBlock != null)
			{
				m_checkbox.IsCheckedChanged = null;
			}
			m_checkbox.IsChecked = GetValue(firstBlock);
			m_checkbox.IsCheckedChanged = m_checkboxClicked;
			if (m_justify && m_checkboxAligner.Owner is MyGuiControlList)
			{
				m_checkboxAligner.Size = new Vector2(0.235f, m_checkboxAligner.Size.Y);
			}
		}

		private void SwitchAction(TBlock block)
		{
			SetValue(block, !GetValue(block));
		}

		private void CheckAction(TBlock block)
		{
			SetValue(block, value: true);
		}

		private void UncheckAction(TBlock block)
		{
			SetValue(block, value: false);
		}

		private void Writer(TBlock block, StringBuilder result, StringBuilder onText, StringBuilder offText)
		{
			result.Append((object)(GetValue(block) ? onText : offText));
		}

		public MyTerminalAction<TBlock> EnableAction(string icon, StringBuilder name, StringBuilder onText, StringBuilder offText, Func<TBlock, bool> enabled = null, Func<TBlock, bool> callable = null)
		{
			MyTerminalAction<TBlock> myTerminalAction = new MyTerminalAction<TBlock>(Id, name, SwitchAction, delegate(TBlock x, StringBuilder r)
			{
				Writer(x, r, onText, offText);
			}, icon, enabled, callable);
			base.Actions = new MyTerminalAction<TBlock>[1]
			{
				myTerminalAction
			};
			return myTerminalAction;
		}

		public override bool GetDefaultValue(TBlock block)
		{
			return false;
		}

		public override bool GetMinimum(TBlock block)
		{
			return false;
		}

		public override bool GetMaximum(TBlock block)
		{
			return true;
		}

		public override void SetValue(TBlock block, bool value)
		{
			base.SetValue(block, value);
		}

		public override bool GetValue(TBlock block)
		{
			return base.GetValue(block);
		}
	}
}
