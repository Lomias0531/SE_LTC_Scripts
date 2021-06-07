using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlContentButton : MyGuiControlRadioButton
	{
		private readonly MyGuiControlLabel m_titleLabel;

		private MyGuiControlImage m_previewImage;

		private string m_previewImagePath;

		private readonly MyGuiControlImage m_workshopIconNormal;

		private readonly MyGuiControlImage m_workshopIconHighlight;

		private readonly MyGuiControlImage m_localmodIconNormal;

		private readonly MyGuiControlImage m_localmodIconHighlight;

		private readonly List<MyGuiControlImage> m_dlcIcons;

		private bool m_isWorkshopMod;

		private bool m_isLocalMod;

		private readonly MyGuiCompositeTexture m_noThumbnailTexture = new MyGuiCompositeTexture("Textures\\GUI\\Icons\\Blueprints\\NoThumbnailFound.png");

		private readonly Color m_noThumbnailColor = new Color(94, 115, 127);

		public string Title => m_titleLabel.Text;

		public bool IsWorkshopMod
		{
			get
			{
				return m_isWorkshopMod;
			}
			set
			{
				if (m_workshopIconNormal == null)
				{
					return;
				}
				if (value)
				{
					Elements.Add(base.HasHighlight ? m_workshopIconHighlight : m_workshopIconNormal);
					if (IsLocalMod)
					{
						IsLocalMod = false;
					}
				}
				else
				{
					Elements.Remove(m_workshopIconNormal);
					Elements.Remove(m_workshopIconHighlight);
				}
				m_isWorkshopMod = value;
			}
		}

		public string PreviewImagePath => m_previewImagePath;

		public bool IsLocalMod
		{
			get
			{
				return m_isLocalMod;
			}
			set
			{
				if (m_localmodIconNormal == null)
				{
					return;
				}
				if (value)
				{
					Elements.Add(base.HasHighlight ? m_localmodIconHighlight : m_localmodIconNormal);
					if (IsWorkshopMod)
					{
						IsWorkshopMod = false;
					}
				}
				else
				{
					Elements.Remove(m_localmodIconNormal);
					Elements.Remove(m_localmodIconHighlight);
				}
				m_isLocalMod = value;
			}
		}

		public MyGuiControlContentButton(string title, string imagePath)
		{
			m_dlcIcons = new List<MyGuiControlImage>();
			IsWorkshopMod = false;
			IsLocalMod = false;
			base.VisualStyle = MyGuiControlRadioButtonStyleEnum.ScenarioButton;
			base.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_titleLabel = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = title
			};
			m_workshopIconNormal = new MyGuiControlImage(null, null, null, null, new string[1]
			{
				MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP.Normal
			})
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM,
				Size = MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP.SizeGui
			};
			m_workshopIconHighlight = new MyGuiControlImage(null, null, null, null, new string[1]
			{
				MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP.Highlight
			})
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM,
				Size = MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP.SizeGui
			};
			m_localmodIconNormal = new MyGuiControlImage(null, null, null, null, new string[1]
			{
				MyGuiConstants.TEXTURE_ICON_MODS_LOCAL.Normal
			})
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM,
				Size = MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP.SizeGui
			};
			m_localmodIconHighlight = new MyGuiControlImage(null, null, null, null, new string[1]
			{
				MyGuiConstants.TEXTURE_ICON_MODS_LOCAL.Highlight
			})
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM,
				Size = MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP.SizeGui
			};
			m_previewImagePath = imagePath;
			CreatePreview(imagePath);
			Elements.Add(m_titleLabel);
		}

		public void SetPreviewVisibility(bool visible)
		{
			m_previewImage.Visible = visible;
			Vector2 size = new Vector2(242f, 128f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_titleLabel.Size = new Vector2(size.X, m_titleLabel.Size.Y);
			m_titleLabel.AutoEllipsis = true;
			if (visible)
			{
				m_previewImage.Size = size;
				m_previewImage.BorderEnabled = true;
				m_previewImage.BorderColor = new Vector4(0.23f, 0.27f, 0.3f, 1f);
				base.Size = new Vector2(m_previewImage.Size.X, m_titleLabel.Size.Y + m_previewImage.Size.Y);
				int num = 0;
				Vector2 value = new Vector2(base.Size.X * 0.48f, (0f - base.Size.Y) * 0.48f + m_titleLabel.Size.Y);
				foreach (MyGuiControlImage dlcIcon in m_dlcIcons)
				{
					dlcIcon.Visible = true;
					dlcIcon.Position = value + new Vector2(0f, (float)num * (dlcIcon.Size.Y + 0.002f));
					num++;
				}
			}
			else
			{
				m_previewImage.Size = new Vector2(0f, 0f);
				m_previewImage.BorderEnabled = true;
				m_previewImage.BorderColor = new Vector4(0.23f, 0.27f, 0.3f, 1f);
				base.Size = new Vector2(size.X, m_titleLabel.Size.Y + 0.002f);
			}
			foreach (MyGuiControlImage dlcIcon2 in m_dlcIcons)
			{
				dlcIcon2.Visible = visible;
			}
		}

		public void CreatePreview(string path)
		{
			if (m_previewImage != null && Elements.Contains(m_previewImage))
			{
				Elements.Remove(m_previewImage);
			}
			m_previewImagePath = path;
			m_previewImage = new MyGuiControlImage(null, null, null, null, new string[1]
			{
				path
			})
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			if (!m_previewImage.IsAnyTextureValid())
			{
				m_previewImage.BackgroundTexture = m_noThumbnailTexture;
				m_previewImage.ColorMask = m_noThumbnailColor;
			}
			Elements.Add(m_previewImage);
			UpdatePositions();
			SetPreviewVisibility(visible: true);
		}

		public void AddDlcIcon(string path)
		{
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(null, null, null, null, new string[1]
			{
				path
			})
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Size = new Vector2(32f) / MyGuiConstants.GUI_OPTIMAL_SIZE
			};
			m_dlcIcons.Add(myGuiControlImage);
			Elements.Add(myGuiControlImage);
		}

		public void ClearDlcIcons()
		{
			if (m_dlcIcons != null && m_dlcIcons.Count != 0)
			{
				foreach (MyGuiControlImage dlcIcon in m_dlcIcons)
				{
					Elements.Remove(dlcIcon);
				}
				m_dlcIcons.Clear();
			}
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			UpdatePositions();
		}

		private void UpdatePositions()
		{
			if (m_previewImage.Visible)
			{
				Vector2 value = new Vector2(base.Size.X * -0.5f, base.Size.Y * -0.52f);
				m_titleLabel.Position = value + new Vector2(0.003f, 0.002f);
				m_previewImage.Position = value + new Vector2(0f, m_titleLabel.Size.Y * 1f);
				m_workshopIconNormal.Position = base.Size * 0.5f - new Vector2(0.001f, 0.002f);
				m_workshopIconHighlight.Position = base.Size * 0.5f - new Vector2(0.001f, 0.002f);
				m_localmodIconNormal.Position = base.Size * 0.5f - new Vector2(0.001f, 0.002f);
				m_localmodIconHighlight.Position = base.Size * 0.5f - new Vector2(0.001f, 0.002f);
				m_localmodIconHighlight.Size = new Vector2(m_localmodIconHighlight.Size.X, m_localmodIconHighlight.Size.Y + 0.002f);
				int num = 0;
				value = new Vector2(base.Size.X * 0.48f, (0f - base.Size.Y) * 0.5f + m_titleLabel.Size.Y);
				foreach (MyGuiControlImage dlcIcon in m_dlcIcons)
				{
					dlcIcon.Visible = true;
					dlcIcon.Position = value + new Vector2(0f, (float)num * (dlcIcon.Size.Y + 0.002f));
					num++;
				}
			}
			else
			{
				Vector2 value2 = new Vector2(base.Size.X * -0.5f, base.Size.Y * -0.61f);
				m_titleLabel.Position = value2 + new Vector2(0.003f, 0.002f);
				m_previewImage.Position = value2 + new Vector2(0f, m_titleLabel.Size.Y * 1f);
				m_workshopIconNormal.Position = base.Size * 0.5f - new Vector2(0.001f, 0.002f);
				m_workshopIconHighlight.Position = base.Size * 0.5f - new Vector2(0.001f, 0.002f);
				m_localmodIconNormal.Position = base.Size * 0.5f - new Vector2(0.001f, 0.002f);
				m_localmodIconHighlight.Position = base.Size * 0.5f - new Vector2(0.001f, 0.002f);
				m_localmodIconHighlight.Size = new Vector2(m_localmodIconHighlight.Size.X, m_localmodIconHighlight.Size.Y + 0.002f);
				foreach (MyGuiControlImage dlcIcon2 in m_dlcIcons)
				{
					dlcIcon2.Visible = false;
				}
			}
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			if (base.HasHighlight)
			{
				BorderEnabled = true;
				BorderColor = new Vector4(0.41f, 0.45f, 0.48f, 1f);
				base.BorderSize = 2;
				m_titleLabel.Font = "White";
				if (IsWorkshopMod)
				{
					Elements.Remove(m_workshopIconNormal);
					Elements.Add(m_workshopIconHighlight);
				}
				else if (IsLocalMod)
				{
					Elements.Remove(m_localmodIconNormal);
					Elements.Add(m_localmodIconHighlight);
				}
			}
			else
			{
				BorderEnabled = false;
				BorderColor = new Vector4(0.23f, 0.27f, 0.3f, 1f);
				base.BorderSize = 1;
				m_titleLabel.Font = "Blue";
				if (IsWorkshopMod)
				{
					Elements.Remove(m_workshopIconHighlight);
					Elements.Add(m_workshopIconNormal);
				}
				else if (IsLocalMod)
				{
					Elements.Remove(m_localmodIconHighlight);
					Elements.Add(m_localmodIconNormal);
				}
			}
		}
	}
}
