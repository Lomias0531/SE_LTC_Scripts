using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.GUI
{
	public class MyGuiProgressCompositeTexture
	{
		public enum BarOrientation
		{
			HORIZONTAL,
			VERTICAL
		}

		protected struct TextureData
		{
			public Vector2I Position;

			public Vector2I Size;

			public MyGuiSizedTexture Texture;

			public override string ToString()
			{
				return string.Concat("Position: ", Position, " Size: ", Size);
			}
		}

		protected readonly TextureData[,] m_textures = new TextureData[3, 3];

		protected bool m_positionsAndSizesDirty = true;

		protected Vector2I m_position = Vector2I.Zero;

		protected Vector2I m_size = Vector2I.Zero;

		public bool IsInverted
		{
			get;
			set;
		}

		public BarOrientation Orientation
		{
			get;
			set;
		}

		public MyGuiSizedTexture LeftTop
		{
			get
			{
				return m_textures[0, 0].Texture;
			}
			set
			{
				m_textures[0, 0].Texture = value;
				m_textures[0, 0].Size = ToVector2I(value.SizePx);
				m_positionsAndSizesDirty = true;
			}
		}

		public MyGuiSizedTexture LeftCenter
		{
			get
			{
				return m_textures[1, 0].Texture;
			}
			set
			{
				m_textures[1, 0].Texture = value;
				m_textures[1, 0].Size = ToVector2I(value.SizePx);
				m_positionsAndSizesDirty = true;
			}
		}

		public MyGuiSizedTexture LeftBottom
		{
			get
			{
				return m_textures[2, 0].Texture;
			}
			set
			{
				m_textures[2, 0].Texture = value;
				m_textures[2, 0].Size = ToVector2I(value.SizePx);
				m_positionsAndSizesDirty = true;
			}
		}

		public MyGuiSizedTexture CenterTop
		{
			get
			{
				return m_textures[0, 1].Texture;
			}
			set
			{
				m_textures[0, 1].Texture = value;
				m_textures[0, 1].Size = ToVector2I(value.SizePx);
				m_positionsAndSizesDirty = true;
			}
		}

		public MyGuiSizedTexture Center
		{
			get
			{
				return m_textures[1, 1].Texture;
			}
			set
			{
				m_textures[1, 1].Texture = value;
				m_textures[1, 1].Size = ToVector2I(value.SizePx);
				m_positionsAndSizesDirty = true;
			}
		}

		public MyGuiSizedTexture CenterBottom
		{
			get
			{
				return m_textures[2, 1].Texture;
			}
			set
			{
				m_textures[2, 1].Texture = value;
				m_textures[2, 1].Size = ToVector2I(value.SizePx);
				m_positionsAndSizesDirty = true;
			}
		}

		public MyGuiSizedTexture RightTop
		{
			get
			{
				return m_textures[0, 2].Texture;
			}
			set
			{
				m_textures[0, 2].Texture = value;
				m_textures[0, 2].Size = ToVector2I(value.SizePx);
				m_positionsAndSizesDirty = true;
			}
		}

		public MyGuiSizedTexture RightCenter
		{
			get
			{
				return m_textures[1, 2].Texture;
			}
			set
			{
				m_textures[1, 2].Texture = value;
				m_textures[1, 2].Size = ToVector2I(value.SizePx);
				m_positionsAndSizesDirty = true;
			}
		}

		public MyGuiSizedTexture RightBottom
		{
			get
			{
				return m_textures[2, 2].Texture;
			}
			set
			{
				m_textures[2, 2].Texture = value;
				m_textures[2, 2].Size = ToVector2I(value.SizePx);
				m_positionsAndSizesDirty = true;
			}
		}

		public Vector2I Position
		{
			get
			{
				return m_position;
			}
			set
			{
				m_position = value;
				m_positionsAndSizesDirty = true;
			}
		}

		public Vector2I Size
		{
			get
			{
				return m_size;
			}
			set
			{
				m_size = value;
				m_positionsAndSizesDirty = true;
			}
		}

		public virtual void Draw(float progression, Color colorMask)
		{
			if (m_positionsAndSizesDirty)
			{
				RefreshPositionsAndSizes();
			}
			progression = MyMath.Clamp(progression, 0f, 1f);
			Rectangle rect = default(Rectangle);
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					TextureData texData = m_textures[i, j];
					if (string.IsNullOrEmpty(texData.Texture.Texture))
					{
						continue;
					}
					if (i == 1 && j == 1)
					{
						int num;
						int k;
						int num2;
						int num3;
						if (Orientation == BarOrientation.HORIZONTAL)
						{
							num = m_textures[1, 1].Size.X;
							k = num;
							num2 = m_textures[0, 1].Size.X;
							num3 = (int)((float)num2 * progression) + 1;
						}
						else
						{
							num = m_textures[1, 1].Size.Y;
							k = num;
							num2 = m_textures[1, 0].Size.Y;
							num3 = (int)((float)num2 * progression) + 1;
						}
						SetTarget(ref rect, texData);
						if (IsInverted)
						{
							if (Orientation == BarOrientation.HORIZONTAL)
							{
								rect.X += num2 - num;
							}
							else
							{
								rect.Y += num2 - num;
							}
						}
						for (; k < num3; k += num)
						{
							MyGuiManager.DrawSprite(texData.Texture.Texture, rect, colorMask);
							if (Orientation == BarOrientation.HORIZONTAL)
							{
								if (IsInverted)
								{
									rect.X = texData.Position.X + num2 - k;
								}
								else
								{
									rect.X = texData.Position.X + k;
								}
							}
							else if (IsInverted)
							{
								rect.Y = texData.Position.Y + num2 - k;
							}
							else
							{
								rect.Y = texData.Position.Y + k;
							}
						}
						int num4 = k - num3;
						int num5 = num - num4;
						if (num4 <= 1)
						{
							continue;
						}
						if (Orientation == BarOrientation.HORIZONTAL)
						{
							rect.Width = num5;
							if (IsInverted)
							{
								rect.X += num;
							}
						}
						else
						{
							rect.Height = num5;
							if (IsInverted)
							{
								rect.Y += num;
							}
						}
						MyGuiManager.DrawSprite(texData.Texture.Texture, rect, colorMask);
					}
					else
					{
						SetTarget(ref rect, texData);
						MyGuiManager.DrawSprite(texData.Texture.Texture, rect, colorMask);
					}
				}
			}
		}

		protected void SetTarget(ref Rectangle rect, TextureData texData)
		{
			rect.X = texData.Position.X;
			rect.Y = texData.Position.Y;
			rect.Width = texData.Size.X;
			rect.Height = texData.Size.Y;
		}

		protected virtual void RefreshPositionsAndSizes()
		{
			m_textures[0, 0].Position = m_position;
			Vector2I size = m_size - m_textures[0, 0].Size - m_textures[2, 2].Size;
			m_textures[1, 0].Size.Y = size.Y;
			m_textures[1, 2].Size.Y = size.Y;
			m_textures[0, 1].Size.X = size.X;
			m_textures[2, 1].Size.X = size.X;
			m_textures[1, 1].Size.Y = size.Y;
			Vector2I size2 = m_textures[1, 1].Size;
			m_textures[1, 1].Size = size;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (i != 0 || j != 0)
					{
						int x = (j > 0) ? (m_textures[i, j - 1].Position.X + m_textures[i, j - 1].Size.X) : m_textures[0, 0].Position.X;
						int y = (i > 0) ? (m_textures[i - 1, j].Position.Y + m_textures[i - 1, j].Size.Y) : m_textures[0, 0].Position.Y;
						m_textures[i, j].Position = new Vector2I(x, y);
					}
				}
			}
			m_textures[1, 1].Size = size2;
			m_positionsAndSizesDirty = false;
		}

		private Vector2I ToVector2I(Vector2 source)
		{
			return new Vector2I((int)source.X, (int)source.Y);
		}
	}
}
