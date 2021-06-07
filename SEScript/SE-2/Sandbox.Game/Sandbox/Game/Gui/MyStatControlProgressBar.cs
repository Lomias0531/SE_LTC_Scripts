using Sandbox.Graphics.GUI;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace Sandbox.Game.GUI
{
	public class MyStatControlProgressBar : MyStatControlBase
	{
		private readonly MyGuiProgressCompositeTexture m_progressionCompositeTexture;

		private readonly MyGuiProgressSimpleTexture m_progressionSimpleTexture;

		public bool Inverted
		{
			get
			{
				if (m_progressionSimpleTexture != null)
				{
					return m_progressionSimpleTexture.Inverted;
				}
				if (m_progressionCompositeTexture != null)
				{
					return m_progressionCompositeTexture.IsInverted;
				}
				return false;
			}
			set
			{
				if (m_progressionSimpleTexture != null)
				{
					m_progressionSimpleTexture.Inverted = value;
				}
				if (m_progressionCompositeTexture != null)
				{
					m_progressionCompositeTexture.IsInverted = value;
				}
			}
		}

		public MyStatControlProgressBar(MyStatControls parent, MyObjectBuilder_CompositeTexture texture)
			: base(parent)
		{
			if (texture.IsValid())
			{
				m_progressionCompositeTexture = new MyGuiProgressCompositeTexture();
				MyObjectBuilder_GuiTexture texture2 = MyGuiTextures.Static.GetTexture(texture.LeftTop);
				MyGuiProgressCompositeTexture progressionCompositeTexture = m_progressionCompositeTexture;
				MyGuiSizedTexture myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture2.Path,
					SizePx = texture2.SizePx
				};
				progressionCompositeTexture.LeftTop = myGuiSizedTexture;
				texture2 = MyGuiTextures.Static.GetTexture(texture.LeftCenter);
				MyGuiProgressCompositeTexture progressionCompositeTexture2 = m_progressionCompositeTexture;
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture2.Path,
					SizePx = texture2.SizePx
				};
				progressionCompositeTexture2.LeftCenter = myGuiSizedTexture;
				texture2 = MyGuiTextures.Static.GetTexture(texture.LeftBottom);
				MyGuiProgressCompositeTexture progressionCompositeTexture3 = m_progressionCompositeTexture;
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture2.Path,
					SizePx = texture2.SizePx
				};
				progressionCompositeTexture3.LeftBottom = myGuiSizedTexture;
				texture2 = MyGuiTextures.Static.GetTexture(texture.CenterTop);
				MyGuiProgressCompositeTexture progressionCompositeTexture4 = m_progressionCompositeTexture;
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture2.Path,
					SizePx = texture2.SizePx
				};
				progressionCompositeTexture4.CenterTop = myGuiSizedTexture;
				texture2 = MyGuiTextures.Static.GetTexture(texture.Center);
				MyGuiProgressCompositeTexture progressionCompositeTexture5 = m_progressionCompositeTexture;
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture2.Path,
					SizePx = texture2.SizePx
				};
				progressionCompositeTexture5.Center = myGuiSizedTexture;
				texture2 = MyGuiTextures.Static.GetTexture(texture.CenterBottom);
				MyGuiProgressCompositeTexture progressionCompositeTexture6 = m_progressionCompositeTexture;
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture2.Path,
					SizePx = texture2.SizePx
				};
				progressionCompositeTexture6.CenterBottom = myGuiSizedTexture;
				texture2 = MyGuiTextures.Static.GetTexture(texture.RightTop);
				MyGuiProgressCompositeTexture progressionCompositeTexture7 = m_progressionCompositeTexture;
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture2.Path,
					SizePx = texture2.SizePx
				};
				progressionCompositeTexture7.RightTop = myGuiSizedTexture;
				texture2 = MyGuiTextures.Static.GetTexture(texture.RightCenter);
				MyGuiProgressCompositeTexture progressionCompositeTexture8 = m_progressionCompositeTexture;
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture2.Path,
					SizePx = texture2.SizePx
				};
				progressionCompositeTexture8.RightCenter = myGuiSizedTexture;
				texture2 = MyGuiTextures.Static.GetTexture(texture.RightBottom);
				MyGuiProgressCompositeTexture progressionCompositeTexture9 = m_progressionCompositeTexture;
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture2.Path,
					SizePx = texture2.SizePx
				};
				progressionCompositeTexture9.RightBottom = myGuiSizedTexture;
			}
		}

		public MyStatControlProgressBar(MyStatControls parent, MyObjectBuilder_GuiTexture background, MyObjectBuilder_GuiTexture progressBar, Vector2I progressBarOffset, Vector4? backgroundColorMask = null, Vector4? progressColorMask = null)
			: base(parent)
		{
			m_progressionSimpleTexture = new MyGuiProgressSimpleTexture
			{
				BackgroundTexture = background,
				ProgressBarTexture = progressBar,
				ProgressBarTextureOffset = progressBarOffset
			};
			m_progressionSimpleTexture.BackgroundColorMask = (backgroundColorMask ?? Vector4.One);
			m_progressionSimpleTexture.ProgressBarColorMask = (progressColorMask ?? Vector4.One);
		}

		protected override void OnPositionChanged(Vector2 oldPosition, Vector2 newPosition)
		{
			if (m_progressionCompositeTexture != null)
			{
				m_progressionCompositeTexture.Position = new Vector2I(newPosition);
			}
			if (m_progressionSimpleTexture != null)
			{
				m_progressionSimpleTexture.Position = new Vector2I(newPosition);
			}
		}

		protected override void OnSizeChanged(Vector2 oldSize, Vector2 newSize)
		{
			if (m_progressionCompositeTexture != null)
			{
				m_progressionCompositeTexture.Size = new Vector2I(newSize);
			}
			if (m_progressionSimpleTexture != null)
			{
				m_progressionSimpleTexture.Size = new Vector2I(newSize);
			}
		}

		public override void Draw(float transitionAlpha)
		{
			float progression = 0f;
			if (base.StatMaxValue != 0f)
			{
				progression = base.StatCurrent / base.StatMaxValue;
			}
			Vector4 sourceColorMask = (m_progressionSimpleTexture != null) ? m_progressionSimpleTexture.ProgressBarColorMask : base.ColorMask;
			base.BlinkBehavior.UpdateBlink();
			if (base.BlinkBehavior.Blink)
			{
				transitionAlpha = base.BlinkBehavior.CurrentBlinkAlpha;
				if (base.BlinkBehavior.ColorMask.HasValue)
				{
					sourceColorMask = base.BlinkBehavior.ColorMask.Value;
				}
			}
			if (m_progressionCompositeTexture != null)
			{
				m_progressionCompositeTexture.Draw(progression, MyGuiControlBase.ApplyColorMaskModifiers(sourceColorMask, enabled: true, transitionAlpha));
			}
			if (m_progressionSimpleTexture != null)
			{
				m_progressionSimpleTexture.Draw(progression, m_progressionSimpleTexture.BackgroundColorMask, MyGuiControlBase.ApplyColorMaskModifiers(sourceColorMask, enabled: true, transitionAlpha));
			}
		}
	}
}
