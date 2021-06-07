using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
	public class MyGuiProgressCompositeTextureAdvanced : MyGuiProgressCompositeTexture
	{
		private float[] phasesThresholds;

		public MyGuiProgressCompositeTextureAdvanced(MyGuiCompositeTexture texture)
		{
			base.LeftBottom = texture.LeftBottom;
			base.LeftCenter = texture.LeftCenter;
			base.LeftTop = texture.LeftTop;
			base.CenterBottom = texture.CenterBottom;
			base.Center = texture.Center;
			base.CenterTop = texture.CenterTop;
			base.RightBottom = texture.RightBottom;
			base.RightCenter = texture.RightCenter;
			base.RightTop = texture.RightTop;
		}

		public override void Draw(float progression, Color colorMask)
		{
			if (m_positionsAndSizesDirty)
			{
				RefreshPositionsAndSizes();
			}
			progression = MyMath.Clamp(progression, 0f, 1f);
			int num = 0;
			if (progression <= phasesThresholds[0])
			{
				num = 1;
			}
			if (progression <= phasesThresholds[1])
			{
				num = 2;
			}
			progression = (progression - phasesThresholds[num]) / (((num == 0) ? 1f : phasesThresholds[num - 1]) - phasesThresholds[num]);
			Vector2 progress = Vector2.One;
			bool flag = false;
			switch (base.Orientation)
			{
			case BarOrientation.HORIZONTAL:
				progress = new Vector2(progression, 1f);
				flag = false;
				break;
			case BarOrientation.VERTICAL:
				progress = new Vector2(1f, progression);
				flag = true;
				break;
			}
			RectangleF dest = default(RectangleF);
			Rectangle? source = default(Rectangle);
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					int num2 = flag ? i : j;
					if (base.IsInverted)
					{
						if ((num >= 1 && num2 == 0) || (num >= 2 && num2 == 1))
						{
							continue;
						}
						TextureData texData = m_textures[i, j];
						if (!string.IsNullOrEmpty(texData.Texture.Texture))
						{
							if (num == 0 && num2 == 0)
							{
								SetTarget(ref dest, ref source, texData, progress);
								MyRenderProxy.DrawSprite(texData.Texture.Texture, ref dest, source, colorMask, 0f);
							}
							else if (num == 1 && num2 == 1)
							{
								SetTarget(ref dest, ref source, texData, progress);
								MyRenderProxy.DrawSprite(texData.Texture.Texture, ref dest, source, colorMask, 0f);
							}
							else if (num == 2 && num2 == 2)
							{
								SetTarget(ref dest, ref source, texData, progress);
								MyRenderProxy.DrawSprite(texData.Texture.Texture, ref dest, source, colorMask, 0f);
							}
							else
							{
								SetTarget(ref dest, ref source, texData, Vector2.One);
								MyRenderProxy.DrawSprite(texData.Texture.Texture, ref dest, source, colorMask, 0f);
							}
						}
					}
					else
					{
						if ((num >= 1 && num2 == 2) || (num >= 2 && num2 == 1))
						{
							continue;
						}
						TextureData texData2 = m_textures[i, j];
						if (!string.IsNullOrEmpty(texData2.Texture.Texture))
						{
							if (num == 0 && num2 == 2)
							{
								SetTarget(ref dest, ref source, texData2, progress);
								MyRenderProxy.DrawSprite(texData2.Texture.Texture, ref dest, source, colorMask, 0f);
							}
							else if (num == 1 && num2 == 1)
							{
								SetTarget(ref dest, ref source, texData2, progress);
								MyRenderProxy.DrawSprite(texData2.Texture.Texture, ref dest, source, colorMask, 0f);
							}
							else if (num == 2 && num2 == 0)
							{
								SetTarget(ref dest, ref source, texData2, progress);
								MyRenderProxy.DrawSprite(texData2.Texture.Texture, ref dest, source, colorMask, 0f);
							}
							else
							{
								SetTarget(ref dest, ref source, texData2, Vector2.One);
								MyRenderProxy.DrawSprite(texData2.Texture.Texture, ref dest, source, colorMask, 0f);
							}
						}
					}
				}
			}
		}

		protected void SetTarget(ref RectangleF dest, ref Rectangle? source, TextureData texData, Vector2 progress)
		{
			if (base.IsInverted)
			{
				dest.X = texData.Position.X + (int)((float)texData.Size.X * (1f - progress.X) + 0.5f);
				dest.Y = texData.Position.Y + (int)((float)texData.Size.Y * (1f - progress.Y) + 0.5f);
				dest.Width = (int)((float)texData.Size.X * progress.X + 0.5f);
				dest.Height = (int)((float)texData.Size.Y * progress.Y + 0.5f);
				source = new Rectangle((int)(texData.Texture.SizePx.X * (1f - progress.X) + 0.5f), (int)(texData.Texture.SizePx.Y * (1f - progress.Y) + 0.5f), (int)(texData.Texture.SizePx.X * progress.X), (int)(texData.Texture.SizePx.Y * progress.Y));
			}
			else
			{
				dest.X = texData.Position.X;
				dest.Y = texData.Position.Y;
				dest.Width = (int)((float)texData.Size.X * progress.X);
				dest.Height = (int)((float)texData.Size.Y * progress.Y);
				source = new Rectangle(0, 0, (int)(texData.Texture.SizePx.X * progress.X), (int)(texData.Texture.SizePx.Y * progress.Y));
			}
		}

		protected override void RefreshPositionsAndSizes()
		{
			m_textures[0, 0].Position = m_position;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					m_textures[i, j].Size = (Vector2I)MyGuiManager.GetScreenSizeFromNormalizedSize(m_textures[i, j].Texture.SizeGui);
				}
			}
			Vector2I size = m_size - m_textures[0, 0].Size - m_textures[2, 2].Size;
			m_textures[1, 0].Size.Y = size.Y;
			m_textures[1, 2].Size.Y = size.Y;
			m_textures[0, 1].Size.X = size.X;
			m_textures[2, 1].Size.X = size.X;
			m_textures[1, 1].Size = size;
			for (int k = 0; k < 3; k++)
			{
				for (int l = 0; l < 3; l++)
				{
					if (k != 0 || l != 0)
					{
						int x = (l > 0) ? (m_textures[k, l - 1].Position.X + m_textures[k, l - 1].Size.X) : m_textures[0, 0].Position.X;
						int y = (k > 0) ? (m_textures[k - 1, l].Position.Y + m_textures[k - 1, l].Size.Y) : m_textures[0, 0].Position.Y;
						m_textures[k, l].Position = new Vector2I(x, y);
					}
				}
			}
			phasesThresholds = new float[3];
			if (base.IsInverted)
			{
				switch (base.Orientation)
				{
				case BarOrientation.HORIZONTAL:
					phasesThresholds[0] = (float)(m_textures[0, 2].Size.X + m_textures[0, 1].Size.X) / (float)m_size.X;
					phasesThresholds[1] = (float)m_textures[0, 2].Size.X / (float)m_size.X;
					phasesThresholds[2] = 0f;
					break;
				case BarOrientation.VERTICAL:
					phasesThresholds[0] = (float)(m_textures[2, 0].Size.Y + m_textures[1, 0].Size.Y) / (float)m_size.Y;
					phasesThresholds[1] = (float)m_textures[2, 0].Size.Y / (float)m_size.Y;
					phasesThresholds[2] = 0f;
					break;
				}
			}
			else
			{
				switch (base.Orientation)
				{
				case BarOrientation.HORIZONTAL:
					phasesThresholds[0] = (float)(m_textures[0, 0].Size.X + m_textures[0, 1].Size.X) / (float)m_size.X;
					phasesThresholds[1] = (float)m_textures[0, 0].Size.X / (float)m_size.X;
					phasesThresholds[2] = 0f;
					break;
				case BarOrientation.VERTICAL:
					phasesThresholds[0] = (float)(m_textures[0, 0].Size.Y + m_textures[1, 0].Size.Y) / (float)m_size.Y;
					phasesThresholds[1] = (float)m_textures[0, 0].Size.Y / (float)m_size.Y;
					phasesThresholds[2] = 0f;
					break;
				}
			}
			m_positionsAndSizesDirty = false;
		}
	}
}
