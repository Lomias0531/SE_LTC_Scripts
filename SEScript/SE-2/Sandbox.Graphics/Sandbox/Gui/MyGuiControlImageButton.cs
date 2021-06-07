using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Gui
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlImageButton))]
	public class MyGuiControlImageButton : MyGuiControlBase
	{
		public struct ButtonIcon
		{
			private string m_normal;

			private string m_active;

			private string m_highlight;

			private string m_activeHighlight;

			private string m_disabled;

			public string Normal
			{
				get
				{
					return m_normal;
				}
				set
				{
					m_normal = value;
				}
			}

			public string Active
			{
				get
				{
					if (!string.IsNullOrEmpty(m_active))
					{
						return m_active;
					}
					return Highlight;
				}
				set
				{
					m_active = value;
				}
			}

			public string Highlight
			{
				get
				{
					if (!string.IsNullOrEmpty(m_highlight))
					{
						return m_highlight;
					}
					return m_normal;
				}
				set
				{
					m_highlight = value;
				}
			}

			public string ActiveHighlight
			{
				get
				{
					if (!string.IsNullOrEmpty(m_activeHighlight))
					{
						return Active;
					}
					return Highlight;
				}
				set
				{
					m_activeHighlight = value;
				}
			}

			public string Disabled
			{
				get
				{
					if (!string.IsNullOrEmpty(m_disabled))
					{
						return m_disabled;
					}
					return Normal;
				}
				set
				{
					m_disabled = value;
				}
			}
		}

		public class StateDefinition
		{
			public MyGuiCompositeTexture Texture;

			public string Font;

			public string CornerTextFont;

			public float CornerTextSize;
		}

		public class StyleDefinition
		{
			public StateDefinition Normal;

			public StateDefinition Active;

			public StateDefinition Highlight;

			public StateDefinition ActiveHighlight;

			public StateDefinition Disabled;

			public MyGuiBorderThickness Padding;

			public Vector4 BackgroundColor = Vector4.One;
		}

		private StyleDefinition m_styleDefinition;

		private bool m_readyToClick;

		private bool m_readyToRightClick;

		private string m_text;

		private MyStringId m_textEnum;

		private float m_textScale;

		private float m_buttonScale = 1f;

		private bool m_activateOnMouseRelease;

		private StringBuilder m_drawText = new StringBuilder();

		private StringBuilder m_cornerText = new StringBuilder();

		private bool m_drawRedTextureWhenDisabled = true;

		private RectangleF m_internalArea;

		protected GuiSounds m_cueEnum;

		private bool m_checked;

		public bool Selected;

		private MyKeys m_boundKey;

		private bool m_allowBoundKey;

		private float m_textScaleWithLanguage;

		public MyGuiDrawAlignEnum TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;

		public string TextFont;

		public string CornerTextFont;

		public float CornerTextSize;

		public bool DrawCrossTextureWhenDisabled;

		public ButtonIcon Icon;

		public StyleDefinition CurrentStyle => m_styleDefinition;

		public bool Checked
		{
			get
			{
				return m_checked;
			}
			set
			{
				m_checked = value;
				RefreshInternals();
			}
		}

		public bool ActivateOnMouseRelease
		{
			get
			{
				return m_activateOnMouseRelease;
			}
			set
			{
				m_activateOnMouseRelease = value;
			}
		}

		public MyKeys BoundKey
		{
			get
			{
				return m_boundKey;
			}
			set
			{
				m_boundKey = value;
			}
		}

		public bool AllowBoundKey
		{
			get
			{
				return m_allowBoundKey;
			}
			set
			{
				m_allowBoundKey = value;
			}
		}

		public int Index
		{
			get;
			private set;
		}

		public string Text
		{
			get
			{
				return m_text;
			}
			set
			{
				m_text = value;
				UpdateText();
			}
		}

		public MyStringId TextEnum
		{
			get
			{
				return m_textEnum;
			}
			set
			{
				m_textEnum = value;
				UpdateText();
			}
		}

		public string CornerText
		{
			get
			{
				return m_cornerText.ToString();
			}
			set
			{
				m_cornerText.Clear();
				m_cornerText.Append(value);
			}
		}

		public GuiSounds CueEnum
		{
			get
			{
				return m_cueEnum;
			}
			set
			{
				m_cueEnum = value;
			}
		}

		public float TextScale
		{
			get
			{
				return m_textScale;
			}
			set
			{
				m_textScale = value;
				TextScaleWithLanguage = value * MyGuiManager.LanguageTextScale;
			}
		}

		protected float ButtonScale
		{
			get
			{
				return m_buttonScale;
			}
			set
			{
				m_buttonScale = value;
			}
		}

		public float TextScaleWithLanguage
		{
			get
			{
				return m_textScaleWithLanguage;
			}
			private set
			{
				m_textScaleWithLanguage = value;
			}
		}

		public event Action<MyGuiControlImageButton> ButtonClicked;

		public event Action<MyGuiControlImageButton> ButtonRightClicked;

		public MyGuiControlImageButton()
			: this("Button", null, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, null, null, GuiSounds.MouseClick, 1f, null, activateOnMouseRelease: false)
		{
		}

		public MyGuiControlImageButton(string name = "Button", Vector2? position = null, Vector2? size = null, Vector4? colorMask = null, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, string toolTip = null, StringBuilder text = null, float textScale = 0.8f, MyGuiDrawAlignEnum textAlignment = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType highlightType = MyGuiControlHighlightType.WHEN_ACTIVE, Action<MyGuiControlImageButton> onButtonClick = null, Action<MyGuiControlImageButton> onButtonRightClick = null, GuiSounds cueEnum = GuiSounds.MouseClick, float buttonScale = 1f, int? buttonIndex = null, bool activateOnMouseRelease = false)
			: base(position ?? Vector2.Zero, size, colorMask ?? MyGuiConstants.BUTTON_BACKGROUND_COLOR, toolTip, null, isActiveControl: true, canHaveFocus: true, highlightType, originAlign)
		{
			m_styleDefinition = new StyleDefinition
			{
				Active = new StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL
				},
				Disabled = new StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL
				},
				Normal = new StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL
				},
				Highlight = new StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_HIGHLIGHT
				},
				ActiveHighlight = new StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_HIGHLIGHT
				}
			};
			base.Name = (name ?? "Button");
			this.ButtonClicked = onButtonClick;
			this.ButtonRightClicked = onButtonRightClick;
			Index = (buttonIndex ?? 0);
			UpdateText();
			m_drawText.Clear().Append((object)text);
			TextScale = textScale;
			TextAlignment = textAlignment;
			m_cueEnum = cueEnum;
			m_activateOnMouseRelease = activateOnMouseRelease;
			ButtonScale = buttonScale;
			base.Size *= ButtonScale;
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase == null)
			{
				if (!m_activateOnMouseRelease)
				{
					if (base.IsMouseOver && MyInput.Static.IsNewPrimaryButtonPressed())
					{
						m_readyToClick = true;
					}
					if (!base.IsMouseOver && MyInput.Static.IsNewPrimaryButtonReleased())
					{
						m_readyToClick = false;
					}
					if (base.IsMouseOver && MyInput.Static.IsNewSecondaryButtonPressed())
					{
						m_readyToRightClick = true;
					}
					if (!base.IsMouseOver && MyInput.Static.IsNewSecondaryButtonReleased())
					{
						m_readyToRightClick = false;
					}
				}
				else
				{
					m_readyToClick = true;
					m_readyToRightClick = true;
				}
				if ((base.IsMouseOver && MyInput.Static.IsNewPrimaryButtonReleased() && m_readyToClick) || (base.HasFocus && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsNewKeyPressed(MyKeys.Space) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01))))
				{
					if (base.Enabled)
					{
						MyGuiSoundManager.PlaySound(m_cueEnum);
						if (this.ButtonClicked != null)
						{
							this.ButtonClicked(this);
						}
					}
					myGuiControlBase = this;
					m_readyToClick = false;
					return myGuiControlBase;
				}
				if (base.IsMouseOver && MyInput.Static.IsNewSecondaryButtonReleased() && m_readyToRightClick)
				{
					if (base.Enabled)
					{
						MyGuiSoundManager.PlaySound(m_cueEnum);
						if (this.ButtonRightClicked != null)
						{
							this.ButtonRightClicked(this);
						}
					}
					myGuiControlBase = this;
					m_readyToRightClick = false;
					return myGuiControlBase;
				}
				if (base.IsMouseOver && (MyInput.Static.IsPrimaryButtonPressed() || MyInput.Static.IsNewSecondaryButtonPressed()))
				{
					myGuiControlBase = this;
				}
				if (myGuiControlBase == null && base.Enabled && AllowBoundKey && BoundKey != 0 && MyInput.Static.IsNewKeyPressed(BoundKey))
				{
					MyGuiSoundManager.PlaySound(m_cueEnum);
					if (this.ButtonClicked != null)
					{
						this.ButtonClicked(this);
					}
					myGuiControlBase = this;
					m_readyToRightClick = false;
					m_readyToClick = false;
				}
			}
			return myGuiControlBase;
		}

		public override string GetMouseCursorTexture()
		{
			return "Textures\\GUI\\MouseCursor.dds";
		}

		protected void RaiseButtonClicked()
		{
			if (this.ButtonClicked != null)
			{
				this.ButtonClicked(this);
			}
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, transitionAlpha);
			if (!base.Enabled && DrawCrossTextureWhenDisabled)
			{
				MyGuiManager.DrawSpriteBatch("Textures\\GUI\\LockedButton.dds", GetPositionAbsolute(), base.Size * MyGuiConstants.LOCKBUTTON_SIZE_MODIFICATION, MyGuiConstants.DISABLED_BUTTON_COLOR, base.OriginAlign);
			}
			Vector2 topLeft = GetPositionAbsoluteTopLeft() + m_internalArea.Position;
			if (!string.IsNullOrEmpty(Icon.Normal))
			{
				MyGuiManager.DrawSpriteBatch((!base.Enabled) ? Icon.Disabled : ((base.HasHighlight && Checked) ? Icon.ActiveHighlight : (base.HasHighlight ? Icon.Highlight : (Checked ? Icon.Active : Icon.Normal))), GetPositionAbsoluteCenter(), base.Size - m_styleDefinition.Padding.SizeChange, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, useFullClientArea: false, waitTillLoaded: false);
			}
			Vector4 sourceColorMask = base.Enabled ? Vector4.One : MyGuiConstants.DISABLED_CONTROL_COLOR_MASK_MULTIPLIER;
			if (m_drawText.Length > 0 && TextScaleWithLanguage > 0f)
			{
				Vector2 coordAlignedFromTopLeft = MyUtils.GetCoordAlignedFromTopLeft(topLeft, m_internalArea.Size, TextAlignment);
				MyGuiManager.DrawString(TextFont, m_drawText, coordAlignedFromTopLeft, TextScaleWithLanguage, MyGuiControlBase.ApplyColorMaskModifiers(sourceColorMask, base.Enabled, transitionAlpha), TextAlignment);
			}
			if (m_cornerText.Length > 0 && CornerTextSize > 0f)
			{
				Vector2 coordAlignedFromTopLeft2 = MyUtils.GetCoordAlignedFromTopLeft(topLeft, m_internalArea.Size, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
				MyGuiManager.DrawString(CornerTextFont, m_cornerText, coordAlignedFromTopLeft2, CornerTextSize, MyGuiControlBase.ApplyColorMaskModifiers(sourceColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
			}
		}

		private void DebugDraw()
		{
			MyGuiManager.DrawBorders(GetPositionAbsoluteTopLeft() + m_internalArea.Position, m_internalArea.Size, Color.White, 1);
		}

		private void UpdateText()
		{
			if (!string.IsNullOrEmpty(m_text))
			{
				m_drawText.Clear();
				m_drawText.Append(m_text);
			}
			else
			{
				m_drawText.Clear();
				m_drawText.Append(MyTexts.GetString(m_textEnum));
			}
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlImageButton obj = (MyObjectBuilder_GuiControlImageButton)base.GetObjectBuilder();
			obj.Text = Text;
			obj.TextEnum = m_textEnum.ToString();
			obj.TextScale = TextScale;
			obj.TextAlignment = (int)TextAlignment;
			obj.DrawCrossTextureWhenDisabled = DrawCrossTextureWhenDisabled;
			return obj;
		}

		public override void Init(MyObjectBuilder_GuiControlBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_GuiControlImageButton myObjectBuilder_GuiControlImageButton = (MyObjectBuilder_GuiControlImageButton)objectBuilder;
			Text = myObjectBuilder_GuiControlImageButton.Text;
			m_textEnum = MyStringId.GetOrCompute(myObjectBuilder_GuiControlImageButton.TextEnum);
			TextScale = myObjectBuilder_GuiControlImageButton.TextScale;
			TextAlignment = (MyGuiDrawAlignEnum)myObjectBuilder_GuiControlImageButton.TextAlignment;
			DrawCrossTextureWhenDisabled = myObjectBuilder_GuiControlImageButton.DrawCrossTextureWhenDisabled;
			UpdateText();
		}

		protected override bool ShouldHaveHighlight()
		{
			if (HighlightType == MyGuiControlHighlightType.FORCED)
			{
				return Selected;
			}
			return base.ShouldHaveHighlight();
		}

		protected override void OnHasHighlightChanged()
		{
			RefreshInternals();
			base.OnHasHighlightChanged();
		}

		protected override void OnOriginAlignChanged()
		{
			base.OnOriginAlignChanged();
			RefreshInternals();
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshInternals();
		}

		private void RefreshInternals()
		{
			base.ColorMask = m_styleDefinition.BackgroundColor;
			if (!base.Enabled)
			{
				BackgroundTexture = m_styleDefinition.Disabled.Texture;
				TextFont = m_styleDefinition.Disabled.Font;
				CornerTextFont = m_styleDefinition.Disabled.CornerTextFont;
				CornerTextSize = m_styleDefinition.Disabled.CornerTextSize;
			}
			if (base.HasHighlight && Checked)
			{
				BackgroundTexture = m_styleDefinition.ActiveHighlight.Texture;
				TextFont = m_styleDefinition.ActiveHighlight.Font;
				CornerTextFont = m_styleDefinition.ActiveHighlight.CornerTextFont;
				CornerTextSize = m_styleDefinition.ActiveHighlight.CornerTextSize;
			}
			else if (base.HasHighlight)
			{
				BackgroundTexture = m_styleDefinition.Highlight.Texture;
				TextFont = m_styleDefinition.Highlight.Font;
				CornerTextFont = m_styleDefinition.Highlight.CornerTextFont;
				CornerTextSize = m_styleDefinition.Highlight.CornerTextSize;
			}
			else if (Checked)
			{
				BackgroundTexture = m_styleDefinition.Active.Texture;
				TextFont = m_styleDefinition.Active.Font;
				CornerTextFont = m_styleDefinition.Active.CornerTextFont;
				CornerTextSize = m_styleDefinition.Active.CornerTextSize;
			}
			else
			{
				BackgroundTexture = m_styleDefinition.Normal.Texture;
				TextFont = m_styleDefinition.Normal.Font;
				CornerTextFont = m_styleDefinition.Normal.CornerTextFont;
				CornerTextSize = m_styleDefinition.Normal.CornerTextSize;
			}
			Vector2 vector = base.Size;
			if (BackgroundTexture != null)
			{
				base.MinSize = BackgroundTexture.MinSizeGui;
				base.MaxSize = BackgroundTexture.MaxSizeGui;
			}
			else
			{
				base.MinSize = Vector2.Zero;
				base.MaxSize = Vector2.PositiveInfinity;
				vector = Vector2.Zero;
			}
			if (vector == Vector2.Zero && m_drawText != null && !string.IsNullOrEmpty(m_drawText.ToString()))
			{
				vector = MyGuiManager.MeasureString(TextFont, m_drawText, TextScaleWithLanguage);
			}
			MyGuiBorderThickness padding = m_styleDefinition.Padding;
			m_internalArea.Position = padding.TopLeftOffset;
			m_internalArea.Size = base.Size - padding.SizeChange;
			base.Size = vector;
		}

		public void ApplyStyle(StyleDefinition style)
		{
			m_styleDefinition = style;
			RefreshInternals();
		}
	}
}
