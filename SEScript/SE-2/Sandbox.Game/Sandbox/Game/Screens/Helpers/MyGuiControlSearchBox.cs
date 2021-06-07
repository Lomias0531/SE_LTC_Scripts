using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlSearchBox : MyGuiControlParent
	{
		public delegate void TextChangedDelegate(string newText);

		private MyGuiControlLabel m_label;

		private MyGuiControlTextbox m_textbox;

		private MyGuiControlButton m_clearButton;

		public MyGuiControlTextbox TextBox => m_textbox;

		public Vector4 SearchLabelColor
		{
			get
			{
				if (m_label != null)
				{
					return m_label.ColorMask;
				}
				return Vector4.One;
			}
			set
			{
				if (m_label != null)
				{
					m_label.ColorMask = value;
				}
			}
		}

		public string SearchText
		{
			get
			{
				return m_textbox.Text;
			}
			set
			{
				m_textbox.Text = value;
			}
		}

		public event TextChangedDelegate OnTextChanged;

		public MyGuiControlSearchBox(Vector2? position = null, Vector2? size = null, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
			: base(position, size)
		{
			base.OriginAlign = originAlign;
			float y = 0.004f;
			m_textbox = new MyGuiControlTextbox();
			m_textbox.VisualStyle = MyGuiControlTextboxStyleEnum.Default;
			m_textbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_textbox.Position = new Vector2((0f - base.Size.X) / 2f, 0f);
			m_textbox.Size = base.Size;
			m_textbox.TextChanged += m_textbox_TextChanged;
			base.Controls.Add(m_textbox);
			m_label = new MyGuiControlLabel();
			m_label.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_label.Position = new Vector2((0f - base.Size.X) / 2f + 0.0075f, y);
			m_label.Text = MyTexts.GetString(MyCommonTexts.ScreenMods_SearchLabel);
			m_label.Font = "DarkBlue";
			base.Controls.Add(m_label);
			m_clearButton = new MyGuiControlButton
			{
				Position = m_textbox.Position + new Vector2(m_textbox.Size.X - 0.005f, y),
				Size = new Vector2(0.0234f, 0.029466f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
				ActivateOnMouseRelease = true
			};
			m_clearButton.VisualStyle = MyGuiControlButtonStyleEnum.Close;
			m_clearButton.Size = new Vector2(0.0234f, 0.029466f);
			m_clearButton.ButtonClicked += m_clearButton_ButtonClicked;
			base.Controls.Add(m_clearButton);
		}

		public MyGuiControlTextbox GetTextbox()
		{
			return m_textbox;
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			m_label.Position = new Vector2((0f - base.Size.X) / 2f + 0.0075f, 0f);
			m_textbox.Position = new Vector2((0f - base.Size.X) / 2f, 0f);
			m_textbox.Size = base.Size;
			m_clearButton.Position = m_textbox.Position + new Vector2(m_textbox.Size.X - 0.005f, 0f);
		}

		private void m_textbox_TextChanged(MyGuiControlTextbox obj)
		{
			m_label.Visible = string.IsNullOrEmpty(obj.Text);
			if (this.OnTextChanged != null)
			{
				this.OnTextChanged(obj.Text);
			}
		}

		private void m_clearButton_ButtonClicked(MyGuiControlButton obj)
		{
			m_textbox.Text = "";
		}
	}
}
