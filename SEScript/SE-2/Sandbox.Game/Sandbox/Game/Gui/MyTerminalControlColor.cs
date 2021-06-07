using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using VRage;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyTerminalControlColor<TBlock> : MyTerminalValueControl<TBlock, Color>, IMyTerminalControlColor, IMyTerminalControl, IMyTerminalValueControl<Color>, ITerminalProperty, IMyTerminalControlTitleTooltip where TBlock : MyTerminalBlock
	{
		public MyStringId Title;

		public MyStringId Tooltip;

		private MyGuiControlColor m_color;

		private Action<MyGuiControlColor> m_changeColor;

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

		public MyTerminalControlColor(string id, MyStringId title)
			: base(id)
		{
			Title = title;
			Serializer = delegate(BitStream stream, ref Color value)
			{
				stream.Serialize(ref value.PackedValue);
			};
		}

		protected override MyGuiControlBase CreateGui()
		{
			m_color = new MyGuiControlColor(MyTexts.Get(Title).ToString(), 0.95f, Vector2.Zero, Color.White, Color.White, MyCommonTexts.DialogAmount_SetValueCaption, placeSlidersVertically: true);
			m_changeColor = OnChangeColor;
			m_color.OnChange += m_changeColor;
			m_color.Size = new Vector2(MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH, m_color.Size.Y);
			return new MyGuiControlBlockProperty(string.Empty, string.Empty, m_color);
		}

		private void OnChangeColor(MyGuiControlColor obj)
		{
			foreach (TBlock targetBlock in base.TargetBlocks)
			{
				SetValue(targetBlock, obj.GetColor());
			}
		}

		protected override void OnUpdateVisual()
		{
			base.OnUpdateVisual();
			TBlock firstBlock = base.FirstBlock;
			if (firstBlock != null)
			{
				m_color.OnChange -= m_changeColor;
				m_color.SetColor(GetValue(firstBlock));
				m_color.OnChange += m_changeColor;
			}
		}

		public override void SetValue(TBlock block, Color value)
		{
			base.SetValue(block, new Color(Vector4.Clamp(value.ToVector4(), Vector4.Zero, Vector4.One)));
		}

		public override Color GetDefaultValue(TBlock block)
		{
			return new Color(Vector4.One);
		}

		public override Color GetMinimum(TBlock block)
		{
			return new Color(Vector4.Zero);
		}

		public override Color GetMaximum(TBlock block)
		{
			return new Color(Vector4.One);
		}
	}
}
