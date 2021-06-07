using System;
using System.Text;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlRadioButton))]
	public class MyGuiControlRadioButton : MyGuiControlBase
	{
		public class StyleDefinition
		{
			public MyGuiCompositeTexture NormalTexture;

			public MyGuiCompositeTexture HighlightTexture;

			public string NormalFont;

			public string HighlightFont;

			public MyGuiBorderThickness Padding;

			public Vector2 BorderMargin;
		}

		private static StyleDefinition[] m_styles;

		private bool m_selected;

		private MyGuiControlRadioButtonStyleEnum m_visualStyle;

		private StyleDefinition m_styleDef;

		private StringBuilder m_text;

		private string m_font;

		private RectangleF m_internalArea;

		private int? m_doubleClickStarted;

		public MyGuiDrawAlignEnum TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;

		public MyGuiHighlightTexture? Icon;

		public MyGuiDrawAlignEnum IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;

		private bool m_lastMouseOver;

		public MyGuiControlRadioButtonStyleEnum VisualStyle
		{
			get
			{
				return m_visualStyle;
			}
			set
			{
				m_visualStyle = value;
				RefreshVisualStyle();
			}
		}

		public StringBuilder Text
		{
			get
			{
				return m_text;
			}
			set
			{
				if (value != null)
				{
					if (m_text == null)
					{
						m_text = new StringBuilder();
					}
					m_text.Clear().AppendStringBuilder(value);
				}
				else if (m_text != null)
				{
					m_text = null;
				}
			}
		}

		public int Key
		{
			get;
			set;
		}

		public bool Selected
		{
			get
			{
				return m_selected;
			}
			set
			{
				if (m_selected != value)
				{
					BorderEnabled = value;
					m_selected = value;
					if (value && this.SelectedChanged != null)
					{
						this.SelectedChanged(this);
					}
				}
			}
		}

		public event Action<MyGuiControlRadioButton> SelectedChanged;

		public event Action<MyGuiControlRadioButton> MouseDoubleClick;

		public event Action<MyGuiControlRadioButton, bool> MouseOverChanged;

		static MyGuiControlRadioButton()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlRadioButtonStyleEnum>()];
			m_styles[0] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_CHARACTER,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_CHARACTER_HIGHLIGHT,
				BorderMargin = new Vector2(0.005f)
			};
			m_styles[1] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_GRID,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_GRID_HIGHLIGHT,
				BorderMargin = new Vector2(0.005f)
			};
			m_styles[2] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_ALL,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_ALL_HIGHLIGHT,
				BorderMargin = new Vector2(0.005f)
			};
			m_styles[3] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_ENERGY,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_ENERGY_HIGHLIGHT,
				BorderMargin = new Vector2(0.005f)
			};
			m_styles[5] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_STORAGE,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_STORAGE_HIGHLIGHT,
				BorderMargin = new Vector2(0.005f)
			};
			m_styles[6] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_SYSTEM,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_SYSTEM_HIGHLIGHT,
				BorderMargin = new Vector2(0.005f)
			};
			m_styles[7] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_NULL,
				HighlightTexture = MyGuiConstants.TEXTURE_NULL
			};
			m_styles[8] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK,
				HighlightTexture = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL,
				NormalFont = "Blue",
				HighlightFont = "White",
				Padding = new MyGuiBorderThickness(6f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 6f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y)
			};
			m_styles[9] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER,
				HighlightTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED_BORDER,
				NormalFont = "Blue",
				HighlightFont = "White",
				Padding = new MyGuiBorderThickness(20f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 6f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y)
			};
			m_styles[4] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_SHIP,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_FILTER_SHIP_HIGHLIGHT,
				BorderMargin = new Vector2(0.005f)
			};
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlRadioButtonStyleEnum style)
		{
			return m_styles[(int)style];
		}

		public MyGuiControlRadioButton()
			: this(null, null, 0, null)
		{
		}

		public MyGuiControlRadioButton(Vector2? position = null, Vector2? size = null, int key = 0, Vector4? colorMask = null)
			: base(position, size, colorMask, null, null, isActiveControl: true, canHaveFocus: true)
		{
			base.Name = "RadioButton";
			m_selected = false;
			Key = key;
			VisualStyle = MyGuiControlRadioButtonStyleEnum.Rectangular;
		}

		public override void Init(MyObjectBuilder_GuiControlBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GuiControlRadioButton myObjectBuilder_GuiControlRadioButton = (MyObjectBuilder_GuiControlRadioButton)builder;
			Key = myObjectBuilder_GuiControlRadioButton.Key;
			if (myObjectBuilder_GuiControlRadioButton.VisualStyle == MyGuiControlRadioButtonStyleEnum.Custom)
			{
				if (myObjectBuilder_GuiControlRadioButton.CustomVisualStyle.HasValue)
				{
					MyGuiCustomVisualStyle value = myObjectBuilder_GuiControlRadioButton.CustomVisualStyle.Value;
					m_styleDef = new StyleDefinition();
					m_styleDef.HighlightFont = value.HighlightFont;
					m_styleDef.NormalFont = value.NormalFont;
					StyleDefinition styleDef = m_styleDef;
					MyGuiCompositeTexture myGuiCompositeTexture = new MyGuiCompositeTexture();
					MyGuiSizedTexture leftTop = new MyGuiSizedTexture
					{
						SizePx = value.Size,
						Texture = value.HighlightTexture
					};
					myGuiCompositeTexture.LeftTop = leftTop;
					styleDef.HighlightTexture = myGuiCompositeTexture;
					StyleDefinition styleDef2 = m_styleDef;
					MyGuiCompositeTexture myGuiCompositeTexture2 = new MyGuiCompositeTexture();
					leftTop = new MyGuiSizedTexture
					{
						SizePx = value.Size,
						Texture = value.NormalTexture
					};
					myGuiCompositeTexture2.LeftTop = leftTop;
					styleDef2.NormalTexture = myGuiCompositeTexture2;
					m_styleDef.Padding = new MyGuiBorderThickness(value.HorizontalPadding, value.VerticalPadding);
					VisualStyle = myObjectBuilder_GuiControlRadioButton.VisualStyle;
				}
				else
				{
					VisualStyle = MyGuiControlRadioButtonStyleEnum.Rectangular;
				}
			}
			else
			{
				VisualStyle = myObjectBuilder_GuiControlRadioButton.VisualStyle;
			}
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlRadioButton obj = (MyObjectBuilder_GuiControlRadioButton)base.GetObjectBuilder();
			obj.Key = Key;
			obj.VisualStyle = VisualStyle;
			if (VisualStyle == MyGuiControlRadioButtonStyleEnum.Custom)
			{
				MyGuiCustomVisualStyle myGuiCustomVisualStyle = new MyGuiCustomVisualStyle
				{
					HighlightTexture = m_styleDef.HighlightTexture.LeftTop.Texture,
					NormalTexture = m_styleDef.NormalTexture.LeftTop.Texture,
					Size = m_styleDef.HighlightTexture.LeftTop.SizePx,
					HighlightFont = m_styleDef.HighlightFont,
					NormalFont = m_styleDef.NormalFont,
					VerticalPadding = m_styleDef.Padding.VerticalSum,
					HorizontalPadding = m_styleDef.Padding.HorizontalSum
				};
			}
			return obj;
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (m_lastMouseOver != base.IsMouseOver)
			{
				m_lastMouseOver = base.IsMouseOver;
				if (this.MouseOverChanged != null)
				{
					this.MouseOverChanged(this, base.IsMouseOver);
				}
			}
			if (myGuiControlBase == null && base.Enabled)
			{
				if (((base.IsMouseOver && MyInput.Static.IsNewPrimaryButtonReleased()) || (base.HasFocus && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsNewKeyPressed(MyKeys.Space) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01)))) && !Selected)
				{
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
					Selected = true;
					myGuiControlBase = this;
				}
				if (base.IsMouseOver && MyInput.Static.IsNewPrimaryButtonPressed())
				{
					if (!m_doubleClickStarted.HasValue)
					{
						m_doubleClickStarted = MyGuiManager.TotalTimeInMilliseconds;
					}
					else if ((float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) <= 500f)
					{
						this.MouseDoubleClick.InvokeIfNotNull(this);
						m_doubleClickStarted = null;
					}
				}
			}
			if (m_doubleClickStarted.HasValue && (float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) >= 500f)
			{
				m_doubleClickStarted = null;
			}
			return myGuiControlBase;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, transitionAlpha);
			Vector2 value = Vector2.Zero;
			if (Icon.HasValue || (Text != null && Text.Length > 0))
			{
				value = GetPositionAbsoluteTopLeft();
			}
			Vector2 topLeft = value + m_internalArea.Position;
			Vector2 size = m_internalArea.Size;
			if (Icon.HasValue)
			{
				Vector2 coordAlignedFromTopLeft = MyUtils.GetCoordAlignedFromTopLeft(topLeft, size, IconOriginAlign);
				MyGuiHighlightTexture value2 = Icon.Value;
				Vector2 vector = Vector2.Min(value2.SizeGui, size) / value2.SizeGui;
				float scaleFactor = Math.Min(vector.X, vector.Y);
				MyGuiManager.DrawSpriteBatch(base.HasHighlight ? value2.Highlight : value2.Normal, coordAlignedFromTopLeft, value2.SizeGui * scaleFactor, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), IconOriginAlign);
			}
			if (Text != null && Text.Length > 0)
			{
				Vector2 coordAlignedFromTopLeft2 = MyUtils.GetCoordAlignedFromTopLeft(topLeft, m_internalArea.Size, TextAlignment);
				MyGuiManager.DrawString(m_font, scale: 0.8f * MyGuiManager.LanguageTextScale, text: Text, normalizedCoord: coordAlignedFromTopLeft2, colorMask: MyGuiControlBase.ApplyColorMaskModifiers(Vector4.One, base.Enabled, transitionAlpha), drawAlign: TextAlignment);
			}
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			RefreshInternals();
		}

		protected override void OnSizeChanged()
		{
			RefreshInternalArea();
			base.OnSizeChanged();
		}

		private void RefreshVisualStyle()
		{
			if (m_visualStyle != MyGuiControlRadioButtonStyleEnum.Custom)
			{
				m_styleDef = GetVisualStyle(VisualStyle);
			}
			RefreshInternals();
		}

		private void RefreshInternals()
		{
			if (base.HasHighlight)
			{
				m_font = m_styleDef.HighlightFont;
				BackgroundTexture = m_styleDef.HighlightTexture;
			}
			else
			{
				m_font = m_styleDef.NormalFont;
				BackgroundTexture = m_styleDef.NormalTexture;
			}
			BorderMargin = m_styleDef.BorderMargin;
			base.MinSize = BackgroundTexture.MinSizeGui;
			base.MaxSize = BackgroundTexture.MaxSizeGui;
			RefreshInternalArea();
		}

		private void RefreshInternalArea()
		{
			m_internalArea.Position = m_styleDef.Padding.TopLeftOffset;
			m_internalArea.Size = base.Size - m_styleDef.Padding.SizeChange;
		}
	}
}
