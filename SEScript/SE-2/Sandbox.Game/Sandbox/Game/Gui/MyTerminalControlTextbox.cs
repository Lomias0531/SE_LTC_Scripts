using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
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
	public class MyTerminalControlTextbox<TBlock> : MyTerminalValueControl<TBlock, StringBuilder>, ITerminalControlSync, IMyTerminalControlTextbox, IMyTerminalControl, IMyTerminalValueControl<StringBuilder>, ITerminalProperty, IMyTerminalControlTitleTooltip where TBlock : MyTerminalBlock
	{
		public new delegate StringBuilder GetterDelegate(TBlock block);

		public new delegate void SetterDelegate(TBlock block, StringBuilder value);

		public new delegate void SerializerDelegate(BitStream stream, StringBuilder value);

		private char[] m_tmpArray = new char[64];

		private MyGuiControlTextbox m_textbox;

		public new SerializerDelegate Serializer;

		public MyStringId Title;

		public MyStringId Tooltip;

		private StringBuilder m_tmpText = new StringBuilder(15);

		private Action<MyGuiControlTextbox> m_textChanged;

		public new GetterDelegate Getter
		{
			private get;
			set;
		}

		public new SetterDelegate Setter
		{
			private get;
			set;
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

		Func<IMyTerminalBlock, StringBuilder> IMyTerminalValueControl<StringBuilder>.Getter
		{
			get
			{
				GetterDelegate oldGetter = Getter;
				return (IMyTerminalBlock x) => oldGetter((TBlock)x);
			}
			set
			{
				Getter = value.Invoke;
			}
		}

		Action<IMyTerminalBlock, StringBuilder> IMyTerminalValueControl<StringBuilder>.Setter
		{
			get
			{
				SetterDelegate oldSetter = Setter;
				return delegate(IMyTerminalBlock x, StringBuilder y)
				{
					oldSetter((TBlock)x, y);
				};
			}
			set
			{
				Setter = value.Invoke;
			}
		}

		public MyTerminalControlTextbox(string id, MyStringId title, MyStringId tooltip)
			: base(id)
		{
			Title = title;
			Tooltip = tooltip;
			Serializer = delegate(BitStream s, StringBuilder sb)
			{
				s.Serialize(sb, ref m_tmpArray, Encoding.UTF8);
			};
		}

		public new void Serialize(BitStream stream, MyTerminalBlock block)
		{
		}

		protected override MyGuiControlBase CreateGui()
		{
			m_textbox = new MyGuiControlTextbox();
			m_textbox.Size = new Vector2(MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH, m_textbox.Size.Y);
			m_textChanged = OnTextChanged;
			m_textbox.TextChanged += m_textChanged;
			MyGuiControlBlockProperty myGuiControlBlockProperty = new MyGuiControlBlockProperty(MyTexts.GetString(Title), MyTexts.GetString(Tooltip), m_textbox);
			myGuiControlBlockProperty.Size = new Vector2(MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH, myGuiControlBlockProperty.Size.Y);
			return myGuiControlBlockProperty;
		}

		private void OnTextChanged(MyGuiControlTextbox obj)
		{
			m_tmpText.Clear();
			obj.GetText(m_tmpText);
			foreach (TBlock targetBlock in base.TargetBlocks)
			{
				SetValue(targetBlock, m_tmpText);
			}
		}

		protected override void OnUpdateVisual()
		{
			base.OnUpdateVisual();
			if (m_textbox.IsImeActive)
			{
				return;
			}
			TBlock firstBlock = base.FirstBlock;
			if (firstBlock != null)
			{
				StringBuilder value = GetValue(firstBlock);
				if (!m_textbox.TextEquals(value))
				{
					m_textbox.TextChanged -= m_textChanged;
					m_textbox.SetText(value);
					m_textbox.TextChanged += m_textChanged;
				}
			}
		}

		public override StringBuilder GetValue(TBlock block)
		{
			return Getter(block);
		}

		public override void SetValue(TBlock block, StringBuilder value)
		{
			Setter(block, new StringBuilder(value.ToString()));
			block.NotifyTerminalValueChanged(this);
		}

		public override StringBuilder GetDefaultValue(TBlock block)
		{
			return new StringBuilder();
		}

		public override StringBuilder GetMinimum(TBlock block)
		{
			return new StringBuilder();
		}

		public override StringBuilder GetMaximum(TBlock block)
		{
			return new StringBuilder();
		}
	}
}
