using VRage.Game.ObjectBuilders.Gui;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlImage))]
	public class MyGuiControlImage : MyGuiControlBase
	{
		public class StyleDefinition
		{
			public MyGuiCompositeTexture BackgroundTexture;

			public MyGuiBorderThickness Padding;
		}

		public struct MyDrawTexture
		{
			public string Texture;

			public Vector4? ColorMask;

			public string MaskTexture;
		}

		protected MyGuiBorderThickness m_padding;

		private StyleDefinition m_styleDefinition;

		public MyDrawTexture[] Textures
		{
			get;
			private set;
		}

		public MyGuiBorderThickness Padding
		{
			get
			{
				return m_padding;
			}
			set
			{
				m_padding = value;
			}
		}

		public MyGuiControlImage()
			: this(null, null, null, null, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
		{
		}

		public MyGuiControlImage(Vector2? position = null, Vector2? size = null, Vector4? backgroundColor = null, string backgroundTexture = null, string[] textures = null, string toolTip = null, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
			: base(position, size, backgroundColor, toolTip, new MyGuiCompositeTexture
			{
				Center = new MyGuiSizedTexture
				{
					Texture = backgroundTexture
				}
			}, isActiveControl: false, canHaveFocus: false, MyGuiControlHighlightType.NEVER, originAlign)
		{
			base.Visible = true;
			SetTextures(textures);
		}

		public void SetPadding(MyGuiBorderThickness padding)
		{
			m_padding = padding;
		}

		public void SetTextures(string[] textures = null)
		{
			MyDrawTexture myDrawTexture;
			if (textures == null)
			{
				MyDrawTexture[] array = new MyDrawTexture[1];
				myDrawTexture = new MyDrawTexture
				{
					Texture = "",
					ColorMask = null
				};
				array[0] = myDrawTexture;
				Textures = array;
				return;
			}
			Textures = new MyDrawTexture[textures.Length];
			for (int i = 0; i < textures.Length; i++)
			{
				myDrawTexture = (Textures[i] = new MyDrawTexture
				{
					Texture = textures[i],
					ColorMask = null
				});
			}
		}

		public void SetTexture(string texture = null, string maskTexture = null)
		{
			MyDrawTexture myDrawTexture;
			if (texture == null)
			{
				MyDrawTexture[] array = new MyDrawTexture[1];
				myDrawTexture = new MyDrawTexture
				{
					Texture = "",
					ColorMask = null
				};
				array[0] = myDrawTexture;
				Textures = array;
			}
			else
			{
				Textures = new MyDrawTexture[1];
				MyDrawTexture[] textures = Textures;
				myDrawTexture = new MyDrawTexture
				{
					Texture = texture,
					ColorMask = null,
					MaskTexture = maskTexture
				};
				textures[0] = myDrawTexture;
			}
		}

		public void SetTextures(MyDrawTexture[] textures = null)
		{
			if (textures == null)
			{
				Textures = new MyDrawTexture[1]
				{
					new MyDrawTexture
					{
						Texture = "",
						ColorMask = null
					}
				};
			}
			else
			{
				Textures = textures;
			}
		}

		public bool IsAnyTextureValid()
		{
			for (int i = 0; i < Textures.Length; i++)
			{
				if (!string.IsNullOrEmpty(Textures[i].Texture))
				{
					return true;
				}
			}
			return false;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			DrawBackground(backgroundTransitionAlpha);
			if (Textures != null)
			{
				for (int i = 0; i < Textures.Length; i++)
				{
					MyGuiManager.DrawSpriteBatch(Textures[i].Texture, GetPositionAbsoluteTopLeft() + m_padding.TopLeftOffset / MyGuiConstants.GUI_OPTIMAL_SIZE, base.Size - m_padding.SizeChange / MyGuiConstants.GUI_OPTIMAL_SIZE, MyGuiControlBase.ApplyColorMaskModifiers(Textures[i].ColorMask.HasValue ? Textures[i].ColorMask.Value : base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, useFullClientArea: false, waitTillLoaded: false, Textures[i].MaskTexture);
				}
			}
			DrawElements(transitionAlpha, backgroundTransitionAlpha);
			DrawBorder(transitionAlpha);
		}

		private void RefreshInternals()
		{
			if (m_styleDefinition != null)
			{
				BackgroundTexture = m_styleDefinition.BackgroundTexture;
				m_padding = m_styleDefinition.Padding;
			}
		}

		public void ApplyStyle(StyleDefinition style)
		{
			m_styleDefinition = style;
			RefreshInternals();
		}
	}
}
