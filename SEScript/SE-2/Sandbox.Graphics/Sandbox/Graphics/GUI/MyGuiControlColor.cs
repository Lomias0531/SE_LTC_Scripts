using System;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlColor : MyGuiControlParent
	{
		private const float SLIDER_WIDTH = 0.09f;

		private Color m_color;

		private MyGuiControlLabel m_textLabel;

		private MyGuiControlSlider m_RSlider;

		private MyGuiControlSlider m_GSlider;

		private MyGuiControlSlider m_BSlider;

		private MyGuiControlLabel m_RLabel;

		private MyGuiControlLabel m_GLabel;

		private MyGuiControlLabel m_BLabel;

		private Vector2 m_minSize;

		private MyStringId m_caption;

		private bool m_canChangeColor = true;

		private bool m_placeSlidersVertically;

		public Color Color => m_color;

		public event Action<MyGuiControlColor> OnChange;

		public MyGuiControlColor(string text, float textScale, Vector2 position, Color color, Color defaultColor, MyStringId dialogAmountCaption, bool placeSlidersVertically = false, string font = "Blue")
			: base(position)
		{
			m_color = color;
			m_placeSlidersVertically = placeSlidersVertically;
			m_textLabel = MakeLabel(textScale, font);
			m_textLabel.Text = text.ToString();
			m_caption = dialogAmountCaption;
			m_RSlider = MakeSlider(font, defaultColor.R);
			m_GSlider = MakeSlider(font, defaultColor.G);
			m_BSlider = MakeSlider(font, defaultColor.B);
			MyGuiControlSlider rSlider = m_RSlider;
			rSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(rSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider sender)
			{
				if (m_canChangeColor)
				{
					m_color.R = (byte)sender.Value;
					UpdateTexts();
					if (this.OnChange != null)
					{
						this.OnChange(this);
					}
				}
			});
			MyGuiControlSlider gSlider = m_GSlider;
			gSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(gSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider sender)
			{
				if (m_canChangeColor)
				{
					m_color.G = (byte)sender.Value;
					UpdateTexts();
					if (this.OnChange != null)
					{
						this.OnChange(this);
					}
				}
			});
			MyGuiControlSlider bSlider = m_BSlider;
			bSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(bSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider sender)
			{
				if (m_canChangeColor)
				{
					m_color.B = (byte)sender.Value;
					UpdateTexts();
					if (this.OnChange != null)
					{
						this.OnChange(this);
					}
				}
			});
			m_RLabel = MakeLabel(textScale, font);
			m_GLabel = MakeLabel(textScale, font);
			m_BLabel = MakeLabel(textScale, font);
			m_RSlider.Value = (int)m_color.R;
			m_GSlider.Value = (int)m_color.G;
			m_BSlider.Value = (int)m_color.B;
			Elements.Add(m_textLabel);
			Elements.Add(m_RSlider);
			Elements.Add(m_GSlider);
			Elements.Add(m_BSlider);
			Elements.Add(m_RLabel);
			Elements.Add(m_GLabel);
			Elements.Add(m_BLabel);
			UpdateTexts();
			RefreshInternals();
			base.Size = m_minSize;
		}

		private MyGuiControlSlider MakeSlider(string font, byte defaultVal)
		{
			Vector2? position = Vector2.Zero;
			float width = 121f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			Vector4? color = base.ColorMask;
			return new MyGuiControlSlider(position, 0f, 255f, width, (int)defaultVal, color, null, 1, 0.8f, 0f, font, null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				SliderClicked = OnSliderClicked
			};
		}

		private bool OnSliderClicked(MyGuiControlSlider who)
		{
			if (MyInput.Static.IsAnyCtrlKeyPressed())
			{
				MyGuiScreenDialogAmount obj = new MyGuiScreenDialogAmount(0f, 255f, defaultAmount: who.Value, caption: m_caption, minMaxDecimalDigits: 3, parseAsInteger: true);
				obj.OnConfirmed += delegate(float v)
				{
					who.Value = v;
				};
				MyScreenManager.AddScreen(obj);
				return true;
			}
			return false;
		}

		private MyGuiControlLabel MakeLabel(float scale, string font)
		{
			return new MyGuiControlLabel(null, null, string.Empty, base.ColorMask, 0.8f * scale, font, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			Vector2 positionAbsoluteTopRight = GetPositionAbsoluteTopRight();
			Vector2 vector = new Vector2(m_BSlider.Size.X, m_textLabel.Size.Y);
			MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", positionAbsoluteTopRight, vector, MyGuiControlBase.ApplyColorMaskModifiers(m_color.ToVector4(), enabled: true, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			positionAbsoluteTopRight.X -= vector.X;
			Color white = Color.White;
			white.A = (byte)((float)(int)white.A * transitionAlpha);
			MyGuiManager.DrawBorders(positionAbsoluteTopRight, vector, white, base.BorderSize);
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase == null)
			{
				myGuiControlBase = HandleInputElements();
			}
			return myGuiControlBase;
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshInternals();
		}

		private void RefreshInternals()
		{
			Vector2 vector = -0.5f * base.Size;
			Vector2 zero = Vector2.Zero;
			if (m_placeSlidersVertically)
			{
				zero.X = Math.Max(m_textLabel.Size.X, m_GSlider.MinSize.X + 0.06f);
				zero.Y = m_textLabel.Size.Y * 1.1f + 3f * Math.Max(m_GSlider.Size.Y, m_GLabel.Size.Y);
			}
			else
			{
				zero.X = MathHelper.Max(m_textLabel.Size.X, 3f * (m_GSlider.MinSize.X + 0.06f));
				zero.Y = m_textLabel.Size.Y * 1.1f + m_RSlider.Size.Y + m_RLabel.Size.Y;
			}
			if (base.Size.X < zero.X || base.Size.Y < zero.Y)
			{
				base.Size = Vector2.Max(base.Size, zero);
				return;
			}
			m_textLabel.Position = vector;
			vector.Y += m_textLabel.Size.Y * 1.1f;
			if (m_placeSlidersVertically)
			{
				Vector2 vector2 = new Vector2(base.Size.X - 0.06f, m_RSlider.MinSize.Y);
				float num = Math.Max(m_RLabel.Size.Y, m_RSlider.Size.Y);
				MyGuiControlSlider rSlider = m_RSlider;
				MyGuiControlSlider gSlider = m_GSlider;
				Vector2 vector4 = m_BSlider.Size = vector2;
				Vector2 vector7 = rSlider.Size = (gSlider.Size = vector4);
				m_RLabel.Position = vector + new Vector2(0f, 0.5f) * num;
				m_RSlider.Position = new Vector2(vector.X + base.Size.X, vector.Y + 0.5f * num);
				m_RLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_RSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
				vector.Y += num;
				m_GLabel.Position = vector + new Vector2(0f, 0.5f) * num;
				m_GSlider.Position = new Vector2(vector.X + base.Size.X, vector.Y + 0.5f * num);
				m_GLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_GSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
				vector.Y += num;
				m_BLabel.Position = vector + new Vector2(0f, 0.5f) * num;
				m_BSlider.Position = new Vector2(vector.X + base.Size.X, vector.Y + 0.5f * num);
				m_BLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_BSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
				vector.Y += num;
			}
			else
			{
				float num2 = MathHelper.Max(m_RLabel.Size.X, m_RSlider.MinSize.X, base.Size.X / 3f);
				Vector2 vector8 = new Vector2(num2, m_RSlider.Size.Y);
				MyGuiControlSlider rSlider2 = m_RSlider;
				MyGuiControlSlider gSlider2 = m_GSlider;
				Vector2 vector4 = m_BSlider.Size = vector8;
				Vector2 vector7 = rSlider2.Size = (gSlider2.Size = vector4);
				Vector2 position = vector;
				m_RSlider.Position = position;
				position.X += num2;
				m_GSlider.Position = position;
				position.X += num2;
				m_BSlider.Position = position;
				vector.Y += m_RSlider.Size.Y;
				m_RLabel.Position = vector;
				vector.X += num2;
				m_GLabel.Position = vector;
				vector.X += num2;
				m_BLabel.Position = vector;
				vector.X += num2;
				MyGuiControlLabel rLabel = m_RLabel;
				MyGuiControlLabel gLabel = m_GLabel;
				MyGuiDrawAlignEnum myGuiDrawAlignEnum2 = m_BLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				MyGuiDrawAlignEnum myGuiDrawAlignEnum5 = rLabel.OriginAlign = (gLabel.OriginAlign = myGuiDrawAlignEnum2);
				MyGuiControlSlider rSlider3 = m_RSlider;
				MyGuiControlSlider gSlider3 = m_GSlider;
				myGuiDrawAlignEnum2 = (m_BSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				myGuiDrawAlignEnum5 = (rSlider3.OriginAlign = (gSlider3.OriginAlign = myGuiDrawAlignEnum2));
			}
		}

		private void UpdateSliders()
		{
			m_canChangeColor = false;
			m_RSlider.Value = (int)m_color.R;
			m_GSlider.Value = (int)m_color.G;
			m_BSlider.Value = (int)m_color.B;
			UpdateTexts();
			m_canChangeColor = true;
		}

		private void UpdateTexts()
		{
			m_RLabel.Text = $"R: {m_color.R}";
			m_GLabel.Text = $"G: {m_color.G}";
			m_BLabel.Text = $"B: {m_color.B}";
		}

		public void SetColor(Vector3 color)
		{
			SetColor(new Color(color));
		}

		public void SetColor(Vector4 color)
		{
			SetColor(new Color(color));
		}

		public void SetColor(Color color)
		{
			bool num = m_color != color;
			m_color = color;
			UpdateSliders();
			if (num && this.OnChange != null)
			{
				this.OnChange(this);
			}
		}

		public Color GetColor()
		{
			return m_color;
		}
	}
}
